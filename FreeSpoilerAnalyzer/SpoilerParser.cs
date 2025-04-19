using System.Text.RegularExpressions;
using FreeSpoilerAnalyzer.Constants;
using FreeSpoilerAnalyzer.Enums;
using FreeSpoilerAnalyzer.Extensions;
using FreeSpoilerAnalyzer.Models;
using KeyItemLocation = FreeSpoilerAnalyzer.Enums.KeyItemLocation;

namespace FreeSpoilerAnalyzer
{
    public partial class SpoilerParser
    {
        public async Task<SpoilerMetadata> GetSpoilerMetadata(StreamReader streamReader)
        {
            var seed = "unknown";
            var flagset = "unknown";

            var currentLine = await AdvanceToSectionAsync(streamReader, SpoilerConstants.BinaryFlags);

            var matches = SpoilerMetadataRegex().Match(currentLine);
            if (matches.Success)
            {
                flagset = matches.Groups[2].Captures.FirstOrDefault()?.Value ?? flagset;
            }

            currentLine = await AdvanceToSectionAsync(streamReader, SpoilerConstants.Seed);
            matches = SpoilerMetadataRegex().Match(currentLine);
            if (matches.Success)
            {
                seed = matches.Groups[2].Captures.FirstOrDefault()?.Value ?? seed;
            }

            return new SpoilerMetadata
            {
                Flagset = flagset,
                Seed = seed,
            };
        }

        public async Task<Dictionary<KeyItem, KeyItemLocation>> ParseKeyItemPlacementAsync(StreamReader streamReader)
        {
            //Default the tracking dictionary to have everything in the overworld
            var keyItems = Enum.GetValues<KeyItem>().Where(x => (int)x > -1).ToDictionary(x => x, y => KeyItemLocation.Starting);
            var keyItemLocationsMap = Enum.GetValues<KeyItemLocation>().ToDictionary(key => key.GetDescription(), value => value);

            var currentLine = await AdvanceToSectionAsync(streamReader, SpoilerConstants.KeyItemLocations);

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

        public async Task<bool> HasPinkTailObjective(StreamReader reader)
        {
            var currentLine = await AdvanceToSectionAsync(reader, SpoilerConstants.Objectives);
            while (currentLine is not null && !currentLine.Contains("Trade away the Pink Tail"))
            {
                currentLine = await reader.ReadLineAsync();
            }
            return currentLine?.Contains("Pink Tail", StringComparison.InvariantCultureIgnoreCase) ?? false;
        }

        private async Task<string> AdvanceToSectionAsync(StreamReader streamReader, string sectionStart, string? sectionEnd = null)
        {
            var currentLine = await streamReader.ReadLineAsync();
            while (currentLine is not null && !currentLine.StartsWith(sectionStart) && currentLine != sectionEnd)
            {
                currentLine = await streamReader.ReadLineAsync();
            }

            return currentLine ?? string.Empty;
        }

        [GeneratedRegex(@"(^[\w\s\-\(\)\.?/']+)\s\.{2,}\s([\w\s-()/\.']+)", RegexOptions.IgnoreCase, "en-US")]
        public static partial Regex SpoilerKeyValueRegex();

        [GeneratedRegex(@"(^[\w\:\']+)\s{2,}\s([\w]+)", RegexOptions.IgnoreCase, "en-US")]
        public static partial Regex SpoilerMetadataRegex();
    }
}
