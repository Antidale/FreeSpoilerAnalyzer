namespace FreeSpoilerAnalyzer.Enums
{
    /// <summary>
    /// Areas refer to the broadest possible concept of where a location is. This similar to, but different from, what flag enables a location to potentially have a Key Item.
    /// </summary>
    public enum World
    {
        //Explicitly set Overworld to 0/default value
        Overworld = 0,
        Underworld,
        Moon
    }
}
