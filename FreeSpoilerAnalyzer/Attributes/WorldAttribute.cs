using FreeSpoilerAnalyzer.Enums;

namespace FreeSpoilerAnalyzer.Attributes
{
    [AttributeUsage(AttributeTargets.Field)]
    public class WorldAttribute : Attribute 
    {
        public World Area { get; set; }

        public WorldAttribute()
        {
            
        }

        public WorldAttribute(World area)
        {
            Area = area;
        }
    }
}
