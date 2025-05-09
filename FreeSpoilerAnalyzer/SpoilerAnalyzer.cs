﻿using System.Collections.Frozen;
using FreeSpoilerAnalyzer.Attributes;
using FreeSpoilerAnalyzer.Enums;
using FreeSpoilerAnalyzer.Extensions;

namespace FreeSpoilerAnalyzer
{


    public class SpoilerAnalyzer
    {
        static readonly FrozenDictionary<KeyItemLocation, GatedByAttribute[]> KeyItemLocationGating = Enum.GetValues<KeyItemLocation>().ToFrozenDictionary(key => key, value => value.GetAttributes<GatedByAttribute>().ToArray());
        static readonly FrozenDictionary<KeyItemLocation, World> KeyItemWorlds = Enum.GetValues<KeyItemLocation>().ToFrozenDictionary(key => key, value => value.GetAttribute<WorldAttribute>().Area);

        /// <summary>
        /// Determines if the desired Key item requires any path through the Underground to access
        /// </summary>
        /// <param name="keyItemInfo"></param>
        /// <param name="keyItem"></param>
        /// <returns></returns>
        public bool IsViaUnderground(Dictionary<KeyItem, KeyItemLocation> keyItemInfo, KeyItem keyItem)
        {
            var stuff = Enum.GetValues<KeyItemLocation>().ToFrozenDictionary(key => key, value => value.GetAttributes<GatedByAttribute>().ToArray());

            if (!keyItemInfo.TryGetValue(keyItem, out var keyItemLocation)) return false;

            if (KeyItemLocationGating[keyItemLocation].All(x => x.GatingItem == KeyItem.None)) return false;

            if (KeyItemWorlds[keyItemLocation] == World.Underworld) return true;


            var gateType = keyItemLocation.GetAttribute<GateTypeAttribute>();

            return gateType.Type switch
            {
                GateType.And => KeyItemLocationGating[keyItemLocation].Any(x => IsViaUnderground(keyItemInfo, x.GatingItem)),
                GateType.Or => KeyItemLocationGating[keyItemLocation].Any(x => !IsViaUnderground(keyItemInfo, x.GatingItem)),
                _ => false
            };
        }

        /// <summary>
        /// Determines if the desired Key item requires any path through the Underground to access
        /// </summary>
        /// <param name="keyItemInfo"></param>
        /// <param name="keyItem"></param>
        /// <returns></returns>
        public bool IsViaOverworldOnly(Dictionary<KeyItem, KeyItemLocation> keyItemInfo, KeyItem keyItem, int iterationCount = 0)
        {
            if (iterationCount > 70)
            { throw new ArgumentException("something's wrong with this seed"); }
            var stuff = Enum.GetValues<KeyItemLocation>().ToFrozenDictionary(key => key, value => value.GetAttributes<GatedByAttribute>().ToArray());

            if (!keyItemInfo.TryGetValue(keyItem, out var keyItemLocation)) return false;

            if (KeyItemLocationGating[keyItemLocation].All(x => x.GatingItem == KeyItem.None)) return true;

            if (KeyItemWorlds[keyItemLocation] != World.Overworld) return false;


            var gateType = keyItemLocation.GetAttribute<GateTypeAttribute>();
            iterationCount++;
            return gateType.Type switch
            {
                GateType.And => KeyItemLocationGating[keyItemLocation].All(x => IsViaOverworldOnly(keyItemInfo, x.GatingItem, iterationCount)),
                GateType.Or => KeyItemLocationGating[keyItemLocation].Any(x => IsViaOverworldOnly(keyItemInfo, x.GatingItem, iterationCount)),
                _ => true
            };
        }

        /// <summary>
        /// Determines how many player initiated checks it takes to find the Key Item
        /// </summary>
        /// <param name="keyItemInfo"></param>
        /// <param name="keyItem"></param>
        /// <returns>A count of all the checks required to find the item. Starting item is not counted</returns>
        public int CheckCount(Dictionary<KeyItem, KeyItemLocation> keyItemInfo, KeyItem keyItem)
        {
            if (!keyItemInfo.TryGetValue(keyItem, out var keyItemLocation)) return 0;

            var gatingItems = keyItemLocation.GetAttributes<GatedByAttribute>();
            var gateType = keyItemLocation.GetAttribute<GateTypeAttribute>();

            return (keyItemLocation, gatingItems.Length, gateType.Type) switch
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
