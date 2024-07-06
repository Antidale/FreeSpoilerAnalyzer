using FreeSpoilerAnalyzer.Enums;

namespace FreeSpoilerAnalyzer.Attributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    public class GatedByAttribute : Attribute
    {
        public KeyItem GatingItem { get; set; } = KeyItem.None;

        public GatedByAttribute() { }
        public GatedByAttribute(KeyItem gatingItem) 
        {
            GatingItem = gatingItem;
        }
    }
}
