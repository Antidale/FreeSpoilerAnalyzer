using FreeSpoilerAnalyzer.Attributes;
using System.ComponentModel;

namespace FreeSpoilerAnalyzer.Enums
{
    public enum KeyItemLocation
    {
        [Ungated]
        [Description("Starting item")]
        Starting,

        [Ungated]
        [Description("Antlion Nest item")] 
        Antlion,

        [Ungated]
        [Description("Defend Fabul reward item")]
        FabulDefense,
        
        [Ungated]
        [Description("Mt. Ordeals item")]
        MtOrdeals,

        [Ungated]
        [Description("Baron Inn item")]
        BaronInn,

        [GatedBy(KeyItem.BaronKey)]
        [Description("Baron Castle item")]
        BaronThrone,

        [GatedBy(KeyItem.BaronKey)]
        [Description("Baron Basement item (Odin slot)")]
        OdinThrone,

        [GatedBy(KeyItem.TwinHarp)]
        [Description("Cave Magnes item")]
        TwinHarp,

        [GatedBy(KeyItem.EarthCrystal)]
        [Description("Zot item")]
        TowerOfZot,
        
        [Ungated]
        [Description("Edward/Toroia item")]
        Edward,

        [Description("D.Mist/Rydia's Mom item")]
        DMist,

        [GatedBy(KeyItem.Hook)]
        [GatedBy(KeyItem.RatTail)]
        [GateType(GateType.And)]
        [Description("Rat Tail trade item")]
        RatTailTrade,

        [GatedBy(KeyItem.Hook)]
        [GatedBy(KeyItem.MagmaKey)]
        [GateType(GateType.Or)]
        [Description("Dwarf Castle/Luca item")]
        [World(World.Underworld)]
        DwarfCastle,

        [GatedBy(KeyItem.Hook)]
        [GatedBy(KeyItem.MagmaKey)]
        [GateType(GateType.Or)]
        [Description("Super Cannon destruction item")]
        [World(World.Underworld)]
        SuperCannon,

        [GatedBy(KeyItem.Hook)]
        [GatedBy(KeyItem.MagmaKey)]
        [GateType(GateType.Or)]
        [Description("Lower Bab-il item (Tower Key slot)")]
        [World(World.Underworld)]
        LowerBabil,

        [GatedBy(KeyItem.Hook)]
        [GatedBy(KeyItem.MagmaKey)]
        [GateType(GateType.Or)]
        [Description("Sealed Cave item")]
        [World(World.Underworld)]
        SealedCave,

        [GatedBy(KeyItem.Hook)]
        [GatedBy(KeyItem.MagmaKey)]
        [GateType(GateType.Or)]
        [Description("Found Yang item (Pan slot)")]
        [World(World.Underworld)]
        Sheila1,

        [GatedBy(KeyItem.Hook)]
        [GatedBy(KeyItem.MagmaKey)]
        [GateType(GateType.Or)]
        [Description("Pan trade item (Spoon slot)")]
        [World(World.Underworld)]
        Sheila2,

        [GatedBy(KeyItem.Hook)]
        [GatedBy(KeyItem.MagmaKey)]
        [GatedBy(KeyItem.Pan)]
        [GateType(GateType.And)]
        [Description("Wake Yang item (Sylph slot)")]
        [World(World.Underworld)]
        PanBonk,

        [GatedBy(KeyItem.Hook)]
        [GatedBy(KeyItem.MagmaKey)]
        [GateType(GateType.Or)]
        [Description("Town of Monsters chest item")]
        [World(World.Underworld)]
        FeymarchFreebie,

        [GatedBy(KeyItem.Hook)]
        [GatedBy(KeyItem.MagmaKey)]
        [GateType(GateType.Or)]
        [Description("Town of Monsters queen item (Asura slot)")]
        [World(World.Underworld)]
        FeymarchQueen,

        [GatedBy(KeyItem.Hook)]
        [GatedBy(KeyItem.MagmaKey)]
        [GateType(GateType.Or)]
        [Description("Town of Monsters king item (Levia slot)")]
        [World(World.Underworld)]
        FeymarchKing,

        [GatedBy(KeyItem.DarknessCrystal)]
        [Description("Cave Bahamut item")]
        [World(World.Moon)]
        CaveBahamut,

        [GatedBy(KeyItem.DarknessCrystal)]
        [Description("Lunar Subterrane altar 1 (Murasame slot)")]
        [World(World.Moon)]
        MurasameAltar,

        [GatedBy(KeyItem.DarknessCrystal)]
        [Description("Lunar Subterrane altar 2 (Crystal Sword slot)")]
        [World(World.Moon)]
        CrystalSwordAltar,

        [GatedBy(KeyItem.DarknessCrystal)]
        [Description("Lunar Subterrane altar 3 (White Spear slot)")]
        [World(World.Moon)]
        WhiteSpearAltar,

        [GatedBy(KeyItem.DarknessCrystal)]
        [Description("Lunar Subterrane pillar chest 1 (Ribbon slot)")]
        [World(World.Moon)]
        RibbonRoomLeft,

        [GatedBy(KeyItem.DarknessCrystal)]
        [Description("Lunar Subterrane pillar chest 2 (Ribbon slot)")]
        [World(World.Moon)]
        RibbonRoomRight,

        [GatedBy(KeyItem.DarknessCrystal)]
        [Description("Lunar Subterrane altar 4 (Masamune slot)")]
        [World(World.Moon)]
        MasamuneAltar,
        
    }
}
