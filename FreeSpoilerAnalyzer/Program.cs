using FreeSpoilerAnalyzer;
using FreeSpoilerAnalyzer.Enums;
using FreeSpoilerAnalyzer.Extensions;
using System.Collections.Concurrent;
using System.Diagnostics;

List<string> folders = [];
List<string> spoilerLogs = [];
int MaxConcurrencyLevel = Environment.ProcessorCount;
ConcurrentDictionary<KeyItemLocation, int> darknessLocationCount = new ConcurrentDictionary<KeyItemLocation, int>(MaxConcurrencyLevel, capacity: 28);

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

foreach (var folder in folders)
{
    var directoryInfo = new DirectoryInfo(folder);
    spoilerLogs = [.. spoilerLogs, .. directoryInfo.GetFiles(searchPattern: "*.spoiler.private").Select(x => x.FullName)];
}

if(spoilerLogs.Count < 100)
{
    Console.WriteLine("Analyzer is for large amounts of files, please ensure you have at least 100");
    return;
}

Console.WriteLine($"Starting to analyze {spoilerLogs.Count} logs from {string.Join(", ", folders)}");

var stopwatch = new Stopwatch();
stopwatch.Start();

await Parallel.ForEachAsync(spoilerLogs, async (log, token) => 
{
    using (var streamReader = new StreamReader(log))
    {
        var parser = new SpoilerParser();
        var analyzer = new SpoilerAnalyzer();
        var keyItemPlacement = await parser.ParseKeyItemPlacementAsync(streamReader);
        if (analyzer.IsViaUnderground(keyItemPlacement, KeyItem.DarknessCrystal)) { Interlocked.Increment(ref undergroundCount); };
        Interlocked.Increment(ref fileCount);

        if (fileCount % 2000 == 0)
        {
            Console.WriteLine($"analyzed {fileCount} logs so far");
        }

        darknessLocationCount.AddOrUpdate(keyItemPlacement[KeyItem.DarknessCrystal], 1, (k,v) => v + 1);
    }
});


stopwatch.Stop();

Console.WriteLine($"total analysis time {stopwatch.ElapsedMilliseconds}ms");

ReportDarknessUndergroundPercentage(fileCount, undergroundCount);
ReportDarknessLocations(darknessLocationCount.ToDictionary(), fileCount);
Console.WriteLine("\r\nPress any key to close");

Console.ReadKey();


static void ReportDarknessLocations(Dictionary<KeyItemLocation, int> locationData, int fileCount)
{
    foreach (var pair in locationData)
    {
        if (pair.Value == 0)
        {
            continue;
        }

        Console.WriteLine($"Darkness was at {pair.Key.GetDescription()} {pair.Value} times at {100.0 * (double)pair.Value / (double)fileCount}% of the time");
    }
}

static void ReportDarknessUndergroundPercentage(int totalFileCount, int totalUndergroundCount)
{
    Console.WriteLine($"\r\nTotal Files: {totalFileCount}\r\nDarkness via Underground: {totalUndergroundCount}\r\nPercentage: {100.0 * (double)totalUndergroundCount / (double)totalFileCount}");
}

