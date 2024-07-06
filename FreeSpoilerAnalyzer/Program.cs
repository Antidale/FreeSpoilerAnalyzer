using FreeSpoilerAnalyzer;
using FreeSpoilerAnalyzer.Enums;
using FreeSpoilerAnalyzer.Extensions;

List<string> folders = [];
FileInfo[] spoilerLogs = [];
Dictionary<KeyItemLocation, int> darknessLocationCount= Enum.GetValues<KeyItemLocation>().ToDictionary(k => k, v => 0);

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
    spoilerLogs = [.. spoilerLogs, .. directoryInfo.GetFiles(searchPattern: "*.spoiler.private")];
}

if(spoilerLogs.Length < 100)
{
    Console.WriteLine("Analyzer is for large amounts of files, please ensure you have at least 100");
    return;
}

Console.WriteLine($"Starting to analyze {spoilerLogs.Length} logs from {string.Join(", ", folders)}");

foreach (var spoilerLog in spoilerLogs)
{
    var keyItemPlacement = await SpoilerParser.ParseKeyItemPlacementAsync(spoilerLog);
    if (SpoilerAnalyzer.IsViaUnderground(keyItemPlacement, KeyItem.DarknessCrystal)) { undergroundCount++; };
    fileCount++;

    if(fileCount % 500 == 0)
    {
        Console.WriteLine($"analyzed {fileCount} logs so far");
    }

    darknessLocationCount[keyItemPlacement[KeyItem.DarknessCrystal]]++;
}

ReportDarknessUndergroundPercentage(fileCount, undergroundCount);
ReportDarknessLocations(darknessLocationCount, fileCount);
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