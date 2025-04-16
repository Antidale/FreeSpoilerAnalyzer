using SQLite;

namespace FreeSpoilerAnalyzer.Models;

public class KeyItemLocation
{
    public string Seed { get; init; } = string.Empty;

    public string KeyItem { get; init; } = string.Empty;

    public string Location { get; init; } = string.Empty;
}