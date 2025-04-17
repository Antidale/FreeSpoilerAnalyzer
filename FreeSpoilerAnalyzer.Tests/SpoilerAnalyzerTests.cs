using FreeSpoilerAnalyzer.Enums;

namespace FreeSpoilerAnalyzer.Tests
{
    public class SpoilerAnalyzerTests
    {
        SpoilerAnalyzer _analyzer;

        public SpoilerAnalyzerTests()
        {
            _analyzer = new SpoilerAnalyzer();
        }


        [Fact]
        public void ViaUnderground_CorrectlyAnalyzes_RatTail_UndergroundGate_OnlyOneUnderground()
        {
            var keyItemInfo = new Dictionary<KeyItem, KeyItemLocation>
            {
                [KeyItem.RatTail] = KeyItemLocation.MtOrdeals,
                [KeyItem.Hook] = KeyItemLocation.DwarfCastle,
                [KeyItem.DarknessCrystal] = KeyItemLocation.RatTailTrade
            };

            var result = _analyzer.IsViaUnderground(keyItemInfo, KeyItem.DarknessCrystal);

            result.Should().BeTrue("Darkness Crystal should be underground if either part of the rat tail turn in is");
        }

        [Fact]
        public void ViaUnderground_CorrectlyAnalyzes_RatTail_UndergroundGate_NeitherUnderground()
        {
            var keyItemInfo = new Dictionary<KeyItem, KeyItemLocation>
            {
                [KeyItem.RatTail] = KeyItemLocation.MtOrdeals,
                [KeyItem.Hook] = KeyItemLocation.BaronInn,
                [KeyItem.DarknessCrystal] = KeyItemLocation.RatTailTrade
            };

            var result = _analyzer.IsViaUnderground(keyItemInfo, KeyItem.DarknessCrystal);

            result.Should().BeFalse("Darkness Crystal should be not found via underground if neither part of the rat tail turn in is");
        }

        [Fact]
        public void ViaUnderground_CorrectlyAnalyzes_RatTail_UndergroundGate_OneUnderground_ComplexSetup()
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

            var result = _analyzer.IsViaUnderground(keyItemInfo, KeyItem.DarknessCrystal);

            result.Should().BeTrue("Darkness Crystal should be considered found via underground via the Pan bonk");
        }

        [Fact]
        public void ViaOverworldOnly_CorrectlyAnalyzes_RatTail_UndergroundGate_NeitherUnderground()
        {
            var keyItemInfo = new Dictionary<KeyItem, KeyItemLocation>
            {
                [KeyItem.RatTail] = KeyItemLocation.MtOrdeals,
                [KeyItem.Hook] = KeyItemLocation.BaronInn,
                [KeyItem.DarknessCrystal] = KeyItemLocation.RatTailTrade
            };

            var result = _analyzer.IsViaOverworldOnly(keyItemInfo, KeyItem.DarknessCrystal);

            result.Should().BeTrue("Darkness Crystal should be marked as Overworld Only, because nothing that was underground blocked it");
        }

        [Fact]
        public void ViaOverworldOnly_CorrectlyAnalyzes_DwarfCastle_UndergroundGate()
        {
            var keyItemInfo = new Dictionary<KeyItem, KeyItemLocation>
            {
                [KeyItem.MagmaKey] = KeyItemLocation.Starting,
                [KeyItem.Hook] = KeyItemLocation.DwarfCastle,
                [KeyItem.DarknessCrystal] = KeyItemLocation.RatTailTrade
            };

            var result = _analyzer.IsViaOverworldOnly(keyItemInfo, KeyItem.Hook);

            result.Should().BeFalse("Hook is at Dwarf Castle, which is underground, so it cannot be understood as Overworld only");
        }


        [Fact]
        public void ViaOverworldOnly_CorrectlyAnalyzes_RatTail_UndergroundGate_OneUnderground_ComplexSetup()
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

            var result = _analyzer.IsViaOverworldOnly(keyItemInfo, KeyItem.DarknessCrystal);

            result.Should().BeFalse("Darkness Crystal should not be considered Overworld Only, because Baron Key requires Bonk, and Hook is on Baron Throwe");
        }

        [Fact]
        public void CheckCount_Adds_Zero_ForStartingItem()
        {
            var keyItemInfo = new Dictionary<KeyItem, KeyItemLocation>
            {
                [KeyItem.DarknessCrystal] = KeyItemLocation.Starting
            };

            var result = _analyzer.CheckCount(keyItemInfo, KeyItem.DarknessCrystal);

            result.Should().Be(0, "The starting item doesn't count as a check since you can't avoid getting it");
        }

        [Fact]
        public void CheckCount_Adds_One_ForUngatedLocations()
        {
            var keyItemInfo = new Dictionary<KeyItem, KeyItemLocation>
            {
                [KeyItem.DarknessCrystal] = KeyItemLocation.Edward
            };

            var result = _analyzer.CheckCount(keyItemInfo, KeyItem.DarknessCrystal);

            result.Should().Be(1, "An ungated, but not starting, location should count as one check");
        }

        [Fact]
        public void CheckCount_CorrectlyAdds_StartingAndSingleGated()
        {
            var keyItemInfo = new Dictionary<KeyItem, KeyItemLocation>
            {
                [KeyItem.DarknessCrystal] = KeyItemLocation.TowerOfZot,
                [KeyItem.EarthCrystal] = KeyItemLocation.Starting
            };

            var result = _analyzer.CheckCount(keyItemInfo, KeyItem.DarknessCrystal);

            result.Should().Be(1, "Starting item doesn't count, Earth Crystal is one check, leading to Darkness");
        }

        [Fact]
        public void CheckCount_CorrectlyHandles_RatTailCounting()
        {
            var keyItemInfo = new Dictionary<KeyItem, KeyItemLocation>
            {
                [KeyItem.Hook] = KeyItemLocation.TowerOfZot,
                [KeyItem.RatTail] = KeyItemLocation.MtOrdeals,
                [KeyItem.EarthCrystal] = KeyItemLocation.Starting,
                [KeyItem.DarknessCrystal] = KeyItemLocation.RatTailTrade,
            };

            var result = _analyzer.CheckCount(keyItemInfo, KeyItem.DarknessCrystal);

            result.Should().Be(3, "Starting Earth doesn't count, Both Hook and Rat are behind a single check, then add one for turning in the Rat Tail");
        }

        [Fact]
        public void CheckCount_CorrectlyHandles_Or_GateType()
        {
            var keyItemInfo = new Dictionary<KeyItem, KeyItemLocation>
            {
                [KeyItem.Hook] = KeyItemLocation.TowerOfZot,
                [KeyItem.MagmaKey] = KeyItemLocation.MtOrdeals,
                [KeyItem.TwinHarp] = KeyItemLocation.Starting,
                [KeyItem.EarthCrystal] = KeyItemLocation.TwinHarp,
                [KeyItem.DarknessCrystal] = KeyItemLocation.FeymarchFreebie,
            };

            var result = _analyzer.CheckCount(keyItemInfo, KeyItem.DarknessCrystal);

            result.Should().Be(2, "Should use the path for Ordeals => Feymarch, and not Harp => Zot => Hook");
        }
    }
}