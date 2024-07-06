using FreeSpoilerAnalyzer;
using FreeSpoilerAnalyzer.Enums;

List<string> folders = [];
FileInfo[] spoilerLogs = [];

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

if(spoilerLogs.Count() < 100)
{
    Console.WriteLine("Analyzer is for large amounts of files, please ensure you have at least 100");
    return;
}

foreach (var spoilerLog in spoilerLogs)
{
    var keyItemPlacement = await SpoilerParser.ParseKeyItemPlacementAsync(spoilerLog);
    if (SpoilerAnalyzer.IsViaUnderground(keyItemPlacement, KeyItem.DarknessCrystal)) { undergroundCount++; };
    fileCount++;
}


Console.WriteLine($"Total Files: {fileCount}\r\nDarkness via Underground: {undergroundCount}\r\nPercentage: {100.0 * (double)undergroundCount / (double)fileCount}");
Console.WriteLine("\r\nPress any key to close");
Console.ReadKey();
