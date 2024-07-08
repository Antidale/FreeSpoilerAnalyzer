using FreeSpoilerAnalyzer.Enums;

namespace FreeSpoilerAnalyzer.Attributes
{
    /// <summary>
    /// Attribute used for listing a single or pair of key items that gate a location. If there is an Or condition (e.g. Hook and Magma Key), use multiple attributes
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    public class GatedByAttribute : Attribute
    {
        /// <summary>
        /// The primary gate
        /// </summary>
        public KeyItem GatingItem { get; set; } = KeyItem.None;
        /// <summary>
        /// If a key item is locked by a singular second. As in Hook and Rat Tail, or Magma and Tower Key.
        /// </summary>
        public KeyItem SecondaryGatingItem { get; set; } = KeyItem.None;

        public IEnumerable<KeyItem> GetGatingItems() => gatingItems.Where(x => !x.Equals(KeyItem.None));

        private List<KeyItem> gatingItems = [];


        public GateType GateType { get; set; } = GateType.And;

        public GatedByAttribute() { }
        public GatedByAttribute(KeyItem gatingItem, KeyItem secondaryGatingItem = KeyItem.None)
        {
            GatingItem = gatingItem;
            SecondaryGatingItem = secondaryGatingItem;
            gatingItems = [gatingItem, secondaryGatingItem];
        }
    }
}
