using Enums = FreeSpoilerAnalyzer.Enums;

namespace FreeSpoilerAnalyzer.Attributes
{
    [AttributeUsage(AttributeTargets.Field)]
    public class GateTypeAttribute : Attribute
    {
        public Enums.GateType Type { get; set; } = Enums.GateType.And;
        public GateTypeAttribute() { }

        public GateTypeAttribute(Enums.GateType type)
        {
            Type = type;
        }
    }
}
