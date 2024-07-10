namespace FreeSpoilerAnalyzer.Enums
{
    /// <summary>
    /// Areas refer to the broadest possible concept of where a location is. This similar to, but different from, what flag enables a location to potentially have a Key Item.
    /// </summary>
    public enum World
    {
        /// <summary>
        /// Any location that is neither gated by Moon nor Underground access. Can still be gated by other KI
        /// </summary>
        Overworld = 0,
        /// <summary>
        /// Any location gated by either Hook or Magma
        /// </summary>
        Underworld,
        /// <summary>
        /// Any location gated by the Darkness Crystal
        /// </summary>
        Moon
    }
}
