using FluentAssertions.Execution;

namespace FreeSpoilerAnalyzer.Tests
{
    public class SpoilerRegexTests
    {
        const string Delimiter = " ........... ";

        [Theory]
        [InlineData("Package", "Rat Tail trade item")]
        [InlineData("Luca Key", "Lower Bab-il item (Tower Key slot)")]
        [InlineData("Rat Tail", "Dwarf Castle/Luca item")]
        [InlineData("Pan", "Mt. Ordeals item")]
        [InlineData("Pan", "D.Mist/Rydia's Mom item")]
        public void SpoilerKeyValueRegex_Captures_KeyItemLocations(string key, string value)
        {

            var inputString = string.Join(Delimiter, key, value);
            var matches = SpoilerParser.SpoilerKeyValueRegex().Match(inputString);

            using (new AssertionScope())
            {
                matches.Should().NotBeNull();
                matches.Success.Should().BeTrue("Matches should have been found");
                matches.Groups.Count.Should().Be(3, "epecting three groups");
                matches.Groups[0].Value.Should().Be(inputString);
                matches.Groups[1].Value.Should().Be(key);
                matches.Groups[2].Value.Should().Be(value);
            }
        }

        [Theory]
        [InlineData("Found Yang item (Pan slot)", "Baron Key")]
        [InlineData("Lower Bab-il item (Tower Key slot)", "Luca Key")]
        [InlineData("D.Mist/Rydia's Mom item", "Pan")]
        [InlineData("Lunar Subterrane altar 4 (Masamune slot)", "Excalibur")]
        public void SpoilerKeyValueRegex_Captures_QuestRewards(string key, string value)
        {

            var inputString = string.Join(Delimiter, key, value);
            var matches = SpoilerParser.SpoilerKeyValueRegex().Match(inputString);

            using (new AssertionScope())
            {
                matches.Should().NotBeNull();
                matches.Success.Should().BeTrue("Matches should have been found");
                matches.Groups.Count.Should().Be(3, "epecting three groups");
                matches.Groups[0].Value.Should().Be(inputString);
                matches.Groups[1].Value.Should().Be(key);
                matches.Groups[2].Value.Should().Be(value);
            }
        }
    }
}
