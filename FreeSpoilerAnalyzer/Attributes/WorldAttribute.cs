using FreeSpoilerAnalyzer.Enums;

namespace FreeSpoilerAnalyzer.Attributes
{
    [AttributeUsage(AttributeTargets.Field)]
    public class WorldAttribute : Attribute
    {
        public World World { get; set; }

        public WorldAttribute() { }

        public WorldAttribute(World world)
        {
            World = world;
        }
    }
}
