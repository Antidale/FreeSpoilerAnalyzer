﻿using FreeSpoilerAnalyzer.Attributes;
using FreeSpoilerAnalyzer.Enums;
using FreeSpoilerAnalyzer.Extensions;

namespace FreeSpoilerAnalyzer
{
    public partial class SpoilerAnalyzer
    {
        public static bool IsViaUnderground(Dictionary<KeyItem, KeyItemLocation> keyItemInfo, KeyItem keyItem)
        {
            if (!keyItemInfo.TryGetValue(keyItem, out var keyItemLocation)) return false;

            var gatingItems = keyItemLocation.GetAttributes<GatedByAttribute>();
            var gateType = keyItemLocation.GetAttribute<GateTypeAttribute>();

            return (keyItemLocation, keyItemLocation.GetAttribute<WorldAttribute>().Area, gateType.Type) switch
            {
                (KeyItemLocation.Starting, _, _) => false,
                (_, World.Underworld, _) => true,
                (_, _, GateType.And) => gatingItems.ToArray().Any(x => IsViaUnderground(keyItemInfo, x.GatingItem)),
                (_, _, GateType.Or) => gatingItems.ToArray().Any(x => !IsViaUnderground(keyItemInfo, x.GatingItem)),
                (_, _, _) => false
            };
        }

        public static int CheckCount(Dictionary<KeyItem, KeyItemLocation> keyItemInfo, KeyItem keyItem)
        {
            if (!keyItemInfo.TryGetValue(keyItem, out var keyItemLocation)) return 0;

            var gatingItems = keyItemLocation.GetAttributes<GatedByAttribute>();
            var gateType = keyItemLocation.GetAttribute<GateTypeAttribute>();

            return (keyItemLocation, gatingItems.ToArray().Length, gateType.Type) switch
            {
                (KeyItemLocation.Starting, _, _) => 0,
                (_, < 1, _) => 1,
                (_, _, GateType.And) => gatingItems.ToArray().Sum(x => CheckCount(keyItemInfo, x.GatingItem)) + 1,
                (_, _, GateType.Or) => gatingItems.ToArray().Min(x => CheckCount(keyItemInfo, x.GatingItem)) + 1,
                (_, _, _) => 0
            };
        }
    }
}
