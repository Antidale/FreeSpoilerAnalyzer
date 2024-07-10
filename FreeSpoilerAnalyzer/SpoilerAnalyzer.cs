using FreeSpoilerAnalyzer.Attributes;
using FreeSpoilerAnalyzer.Enums;
using FreeSpoilerAnalyzer.Extensions;
using System.Collections.Frozen;

namespace FreeSpoilerAnalyzer
{

    public class SpoilerAnalyzer
    {
        static readonly FrozenDictionary<KeyItemLocation, GatedByAttribute[]> KeyItemLocationGating = Enum.GetValues<KeyItemLocation>().ToFrozenDictionary(key => key, value => value.GetAttributes<GatedByAttribute>().ToArray());
        static readonly FrozenDictionary<KeyItemLocation, World> KeyItemWorlds = Enum.GetValues<KeyItemLocation>().ToFrozenDictionary(key => key, value => value.GetAttribute<WorldAttribute>().World);

        /// <summary>
        /// Determines if the desired Key item requires any path through the Underground to access
        /// </summary>
        /// <param name="keyItemInfo"></param>
        /// <param name="keyItem"></param>
        /// <returns></returns>
        public bool IsViaUnderground(Dictionary<KeyItem, KeyItemLocation> keyItemInfo, KeyItem keyItem)
        {
            return IsViaWorld(keyItemInfo, keyItem, World.Underworld);
        }

        /// <summary>
        /// Determines if the desired Key item requires any path through the Underground to access
        /// </summary>
        /// <param name="keyItemInfo"></param>
        /// <param name="keyItem"></param>
        /// <returns></returns>
        public bool IsViaWorld(Dictionary<KeyItem, KeyItemLocation> keyItemInfo, KeyItem keyItem, World world)
        {
            if (!keyItemInfo.TryGetValue(keyItem, out var keyItemLocation)) return false;

            if (world == World.Overworld) return true;

            if (KeyItemWorlds[keyItemLocation] == world) return true;

            var gateType = keyItemLocation.GetAttribute<GateTypeAttribute>();

            return gateType.Type switch
            {
                GateType.And => KeyItemLocationGating[keyItemLocation].SelectMany(x => x.GetGatingItems()).Any(x => IsViaWorld(keyItemInfo, x, world)),
                GateType.Or => KeyItemLocationGating[keyItemLocation].Any(x => !IsViaWorld(keyItemInfo, x.GatingItem, world)),
                _ => false
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

            var stuff = Enumerable.Empty<KeyItemLocation>();

            return (keyItemLocation, gatingItems.Length, gateType.Type) switch
            {
                (KeyItemLocation.Starting, _, _) => 0,
                (_, < 1, _) => 1,
                (_, _, GateType.And) => gatingItems.ToArray().SelectMany(x => x.GetGatingItems()).Sum(x => CheckCount(keyItemInfo, x)) + 1,
                (_, _, GateType.Or) => gatingItems.ToArray().Min(x => x.GetGatingItems().Sum(x => CheckCount(keyItemInfo, x)) + 1),
                (_, _, _) => 0
            };
        }

        // public List<string> GetGatingItems(Dictionary<KeyItem, KeyItemLocation> keyItemInfo, KeyItem keyItem, List<string>? collectedItems = null)
        // {
        //     collectedItems ??= [];


        // }
    }
}
