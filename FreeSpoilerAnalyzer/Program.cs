using System.Collections.Concurrent;
using FreeSpoilerAnalyzer;
using FreeSpoilerAnalyzer.Enums;
using FreeSpoilerAnalyzer.Extensions;
using KeyItemLocation = FreeSpoilerAnalyzer.Enums.KeyItemLocation;
using Models = FreeSpoilerAnalyzer.Models;

List<string> folders = [];
List<string> spoilerLogs = [];
int MaxConcurrencyLevel = Environment.ProcessorCount;
ConcurrentDictionary<(KeyItem, KeyItemLocation), int> darknessLocationCount = new ConcurrentDictionary<(KeyItem, KeyItemLocation), int>(MaxConcurrencyLevel, capacity: 18 * 28);


var undergroundCount = 0;
var fileCount = 0;

foreach (var arg in args)
{
    var attributes = File.GetAttributes(arg);
    if ((attributes & FileAttributes.Directory) == FileAttributes.Directory)
    {
        folders.Add(arg);
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

await Parallel.ForEachAsync(spoilerLogs, async (log, token) =>
{
    using (var streamReader = new StreamReader(log))
    {
        var parser = new SpoilerParser();
        var analyzer = new SpoilerAnalyzer();
        var metadata = await parser.GetSpoilerMetadata(streamReader);
        // await dataaseHelper.DbConnection.InsertAsync(metadata);
        var keyItemPlacement = await parser.ParseKeyItemPlacementAsync(streamReader);
        var models = keyItemPlacement.Select(x => new Models.KeyItemLocation
        {
            Seed = metadata.Seed,
            KeyItem = x.Key.ToString(),
            Location = x.Value.ToString()
        });

        // if (analyzer.IsViaUnderground(keyItemPlacement, KeyItem.DarknessCrystal)) { Interlocked.Increment(ref undergroundCount); };
        Interlocked.Increment(ref fileCount);

        if (fileCount % 2000 == 0)
        {
            Console.WriteLine($"analyzed {fileCount} logs so far");
        }
        foreach (var itemLocationPair in keyItemPlacement)
        {
            darknessLocationCount.AddOrUpdate((itemLocationPair.Key, itemLocationPair.Value), 1, (k, v) => v + 1);
        }

    }
});


ReportDarknessLocations(darknessLocationCount.ToDictionary(), fileCount);
Console.WriteLine("\r\nPress any key to close");

Console.ReadKey();


static void ReportDarknessLocations(Dictionary<(KeyItem, KeyItemLocation), int> locationData, int fileCount)
{
    foreach (var pair in locationData)
    {
        if (pair.Value == 0)
        {
            continue;
        }

        Console.WriteLine($"{pair.Key.Item1} was at {pair.Key.Item2.GetDescription()} {pair.Value} times at {100.0 * (double)pair.Value / (double)fileCount}% of the time");
    }
}

static void ReportDarknessUndergroundPercentage(int totalFileCount, int totalUndergroundCount)
{
    Console.WriteLine($"\r\nTotal Files: {totalFileCount}\r\nDarkness via Underground: {totalUndergroundCount}\r\nPercentage: {100.0 * (double)totalUndergroundCount / (double)totalFileCount}");
}

