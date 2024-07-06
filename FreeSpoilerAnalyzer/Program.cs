using FreeSpoilerAnalyzer;
using FreeSpoilerAnalyzer.Enums;

List<string> folders = [];

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

foreach (var folder in folders)
{
    var directoryInfo = new DirectoryInfo(folder);
    var spoilerLogs = directoryInfo.GetFiles(searchPattern: "*.spoiler.private");

    foreach (var spoilerLog in spoilerLogs)
    {
        var keyItemPlacement = await SpoilerParser.ParseKeyItemPlacementAsync(spoilerLog);
        if (SpoilerAnalyzer.IsViaUnderground(keyItemPlacement, KeyItem.DarknessCrystal)) { undergroundCount++; };
        fileCount++;
    }
}

Console.WriteLine($"Total Files: {fileCount}\r\nDarkness via Underground: {undergroundCount}\r\nPercentage: {100.0 * (double)undergroundCount/(double)fileCount}");
Console.WriteLine("\r\nPress any key to close");
Console.ReadKey();
