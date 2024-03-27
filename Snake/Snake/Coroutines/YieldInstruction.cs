namespace Snake.Coroutines
{
    /// <summary>
    /// An abstract class that defines a yield instruction for a coroutine<br><br></br></br>
    /// Inherit from this class to create new instructions
    /// </summary>
    public abstract class YieldInstruction
    {
        public abstract bool ShouldYield { get; }
    }
}
