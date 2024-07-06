using FreeSpoilerAnalyzer.Enums;
using FluentAssertions;

namespace FreeSpoilerAnalyzer.Tests
{
    public class SpoilerAnalyzerTests
    {
        [Fact]
        public void CorrectlyAnalyzes_RatTail_UndergroundGate_OnlyOneUnderground()
        {
            var keyItemInfo = new Dictionary<KeyItem, KeyItemLocation>
            {
                [KeyItem.RatTail] = KeyItemLocation.MtOrdeals,
                [KeyItem.Hook] = KeyItemLocation.DwarfCastle,
                [KeyItem.DarknessCrystal] = KeyItemLocation.RatTailTrade
            };

            var result = SpoilerAnalyzer.IsViaUnderground(keyItemInfo, KeyItem.DarknessCrystal);

            result.Should().BeTrue("Darkness Crystal should be underground if either part of the rat tail turn in is");
        }

        [Fact]
        public void CorrectlyAnalyzes_RatTail_UndergroundGate_NeithereUnderground()
        {
            var keyItemInfo = new Dictionary<KeyItem, KeyItemLocation>
            {
                [KeyItem.RatTail] = KeyItemLocation.MtOrdeals,
                [KeyItem.Hook] = KeyItemLocation.BaronInn,
                [KeyItem.DarknessCrystal] = KeyItemLocation.RatTailTrade
            };

            var result = SpoilerAnalyzer.IsViaUnderground(keyItemInfo, KeyItem.DarknessCrystal);

            result.Should().BeFalse("Darkness Crystal should be not found via underground if neither part of the rat tail turn in is");
        }

        [Fact]
        public void CorrectlyAnalyzes_RatTail_UndergroundGate_OneUnderground_ComplexSetup()
        {
            var keyItemInfo = new Dictionary<KeyItem, KeyItemLocation>
            {
                [KeyItem.RatTail] = KeyItemLocation.MtOrdeals,
                [KeyItem.Hook] = KeyItemLocation.BaronThrone,
                [KeyItem.Pan] = KeyItemLocation.Antlion,
                [KeyItem.MagmaKey] = KeyItemLocation.FabulDefense,
                
                //Underground Requirement
                [KeyItem.BaronKey] = KeyItemLocation.PanBonk,
                
                //Darkness Placement
                [KeyItem.DarknessCrystal] = KeyItemLocation.RatTailTrade
            };

            var result = SpoilerAnalyzer.IsViaUnderground(keyItemInfo, KeyItem.DarknessCrystal);

            result.Should().BeTrue("Darkness Crystal should be considered found via underground via the Pan bonk");
        }
    }
}