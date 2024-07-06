using FreeSpoilerAnalyzer.Attributes;
using FreeSpoilerAnalyzer.Enums;
using FreeSpoilerAnalyzer.Extensions;

namespace FreeSpoilerAnalyzer
{
    public partial class SpoilerAnalyzer
    {
        public static bool IsViaUnderground(Dictionary<KeyItem, KeyItemLocation> keyItemInfo, KeyItem keyItem)
        {
            if (!keyItemInfo.TryGetValue(keyItem, out var keyItemLocation)) return false;

            if (keyItemLocation.HasAttribute<UngatedAttribute>()) return false;

            if (keyItemLocation.GetAttribute<WorldAttribute>().Area == World.Underworld) return true;

            var gatingItems = keyItemLocation.GetAttributes<GatedByAttribute>();
            var gateType = keyItemLocation.GetAttribute<GateTypeAttribute>();

            return gateType.Type switch
            {
                GateType.And => gatingItems.ToArray().Any(x => IsViaUnderground(keyItemInfo, x.GatingItem)),
                GateType.Or => gatingItems.ToArray().Any(x => !IsViaUnderground(keyItemInfo, x.GatingItem)),
                _ => false
            };
        }
    }
}
