using System.Collections.Concurrent;
using FreeSpoilerAnalyzer;
using FreeSpoilerAnalyzer.Enums;
using KeyItemLocation = FreeSpoilerAnalyzer.Enums.KeyItemLocation;
using Models = FreeSpoilerAnalyzer.Models;

//TODO: Add something like System.Command line to allow flags to control operations
//TODO: alternatively, add in a config file for a similar setup.
List<string> folders = [];
List<string> spoilerLogs = [];
int MaxConcurrencyLevel = Environment.ProcessorCount;
ConcurrentDictionary<(KeyItem, KeyItemLocation), int> keyItemLocationCount = new(MaxConcurrencyLevel, capacity: 18 * 28);

//TODO: Add a class/record/struct for holding this information
var undergroundCount = 0;
var magmaOverworldCount = 0;
var hookOverworldCount = 0;
var bothOverworldCount = 0;
var fileCount = 0;
var lockedPinkTailObjectiveCount = 0;
ConcurrentDictionary<int, int> elbanMiabLocationCount = new(MaxConcurrencyLevel, capacity: 4);

foreach (var arg in args)
{
    if (Directory.Exists(arg))
    {
        folders.Add(arg);
    }
    else
    {
        Console.WriteLine($"{arg} not found or is not a directory");
    }
}

if (folders.Count < 1)
{
    Console.WriteLine("No folders found");
    return;
}

spoilerLogs = folders.Select(folder => new DirectoryInfo(folder))
                     .Aggregate(spoilerLogs, (current, directoryInfo) => [.. current, .. directoryInfo.GetFiles(searchPattern: "*.spoiler.private").Select(x => x.FullName)]);

if (spoilerLogs.Count < 100)
{
    Console.WriteLine("Analyzer is for large amounts of files, please ensure you have at least 100");
    return;
}

Console.WriteLine($"Starting to analyze {spoilerLogs.Count} logs from {string.Join(", ", folders)}");

var stuff = new List<Models.KeyItemLocation>();

await Parallel.ForEachAsync(spoilerLogs, async (log, token) =>
{
    using var streamReader = new StreamReader(log);
    var parser = new SpoilerParser();
    var analyzer = new SpoilerAnalyzer();

    var keyItemPlacement = await parser.ParseKeyItemPlacementAsync(streamReader);

    var seedEblanKiCount = keyItemPlacement.Values.Count(x => x == KeyItemLocation.EblanCastleMiab);
    elbanMiabLocationCount.AddOrUpdate(seedEblanKiCount, 1, (k, v) => v + 1);

    var isMagmaUnderground = analyzer.IsViaOverworldOnly(keyItemPlacement, KeyItem.MagmaKey);
    var isHookUnderground = analyzer.IsViaOverworldOnly(keyItemPlacement, KeyItem.Hook);

    if (isMagmaUnderground) { Interlocked.Increment(ref magmaOverworldCount); }
    if (isHookUnderground) { Interlocked.Increment(ref hookOverworldCount); }
    if (isHookUnderground && isMagmaUnderground) { Interlocked.Increment(ref bothOverworldCount); }

    Interlocked.Increment(ref fileCount);

    if (fileCount % 2000 == 0)
    {
        Console.WriteLine($"analyzed {fileCount} logs so far");
    }

    foreach (var itemLocationPair in keyItemPlacement)
    {
        keyItemLocationCount.AddOrUpdate((itemLocationPair.Key, itemLocationPair.Value), 1, (k, v) => v + 1);

        /*
            uncomment below to write out seeds that have a self locking pink tail, or an uncompletable trade the pink tail objective.
        */
        // if (itemLocationPair.Key == KeyItem.PinkTail && itemLocationPair.Value == KeyItemLocation.PinkTailTrade)
        // {
        //     Console.WriteLine($"self locking pink tail: {log}");
        //     var isLockedObjective = await parser.HasPinkTailObjective(streamReader);
        //     if (isLockedObjective)
        //     {
        //         Interlocked.Increment(ref lockedPinkTailObjectiveCount);
        //         Console.WriteLine($"{log} has locked objective");
        //     }
        // }
    }
});

//If the flagset has knofree:package on, make sure to edit the KeyItemLocation entry for the package
//Also, this does not correctly handle bstandard/knofree, since I don't include boss info in the parsing or analysis currently.
Reporter.ReportDarknessUndergroundPercentage(fileCount, bothOverworldCount, magmaOverworldCount, hookOverworldCount);

Reporter.ReportKeyItemLocations(keyItemLocationCount.ToDictionary(), fileCount);
Reporter.ReportEblanKiCount(elbanMiabLocationCount.ToDictionary());
// /* Uncomment to write out how many seeds have an uncompletable trade pink tail objective */
// Reporter.ReportLockedPinkTailObjectiveCount(lockedPinkTailObjectiveCount);
