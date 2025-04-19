using FreeSpoilerAnalyzer.Enums;
using FreeSpoilerAnalyzer.Extensions;

namespace FreeSpoilerAnalyzer;

public static class Reporter
{
    public static void ReportKeyItemLocations(Dictionary<(KeyItem, KeyItemLocation), int> locationData, int fileCount)
    {
        foreach (var pair in locationData)
        {
            Console.WriteLine($"{pair.Key.Item1} was at {pair.Key.Item2.GetDescription()} {pair.Value}");
        }
    }

    public static void ReportDarknessUndergroundPercentage(int totalFileCount, int bothOverworldCount, int magmaOverworldCount, int hookOverworldCount)
    {
        Console.WriteLine(@$"
Total Files: {totalFileCount}
Magma & Hook Both Overworld: {bothOverworldCount}
Percentage: {100.0 * (double)bothOverworldCount / (double)totalFileCount}

Magma Overworld: {magmaOverworldCount}
Percentage: {100.0 * (double)magmaOverworldCount / (double)totalFileCount}

Hook Overworld: {hookOverworldCount}
Percentage: {100.0 * (double)hookOverworldCount / (double)totalFileCount}
    ");
    }

    public static void ReportLockedPinkTailObjectiveCount(int count)
    {
        Console.WriteLine($"Pink Tail Trade was locked behind Pink tail {count} times");
    }

    public static void ReportEblanKiCount(Dictionary<int, int> eblanKiCounts)
    {

        Console.WriteLine($"Eblan Castle KI counts:");
        foreach (var pair in eblanKiCounts)
        {
            Console.WriteLine($"\t{pair.Key} KI {pair.Value} times");
        }
    }
}
