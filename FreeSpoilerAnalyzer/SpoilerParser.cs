using FreeSpoilerAnalyzer.Constants;
using FreeSpoilerAnalyzer.Enums;
using FreeSpoilerAnalyzer.Extensions;
using System.Text.RegularExpressions;

namespace FreeSpoilerAnalyzer
{
    public partial class SpoilerParser
    {
        public static async Task<Dictionary<KeyItem, KeyItemLocation>> ParseKeyItemPlacementAsync(FileInfo fileInfo)
        {
            //Default the tracking dictionary to have everything in the overworld
            var keyItems = Enum.GetValues<KeyItem>().Where(x => (int)x > -1).ToDictionary(x => x, y => KeyItemLocation.Starting);
            var keyItemLocationsMap = Enum.GetValues<KeyItemLocation>().ToDictionary(key => key.GetDescription(), value => value);
            using var streamReader = new StreamReader(fileInfo.FullName);

            var currentLine = await streamReader.ReadLineAsync();

            //Read to the header section for KI Locations
            while (currentLine is not null && !currentLine.StartsWith(SpoilerConstants.KeyItemLocations))
            {
                currentLine = await streamReader.ReadLineAsync();
            }

            //Read to the section divider, getting KI & World Pairings along the way
            while (currentLine is not null && !currentLine.StartsWith(SpoilerConstants.SectionDivider))
            {
                currentLine = await streamReader.ReadLineAsync();
                if (!string.IsNullOrWhiteSpace(currentLine))
                {
                    var matches = SpoilerKeyValueRegex().Match(currentLine);
                    if (matches.Success)
                    {
                        var success = Enum.TryParse<KeyItem>((matches.Groups[1].Captures.FirstOrDefault()?.Value ?? "").Replace(" ", string.Empty), out var keyItem);
                        if (!success) { continue; }

                        success = keyItemLocationsMap.TryGetValue(matches.Groups[2].Captures.FirstOrDefault()?.Value ?? "", out var keyItemLocation);
                        if (!success) { continue; }

                        keyItems[keyItem] = keyItemLocation;
                    }
                }
            }

            return keyItems;
        }

        [GeneratedRegex(@"(^[\w\s\.?/']+)\s\.{2,}\s([\w\s-()/\.']+)", RegexOptions.IgnoreCase, "en-US")]
        private static partial Regex SpoilerKeyValueRegex();
    }
}
