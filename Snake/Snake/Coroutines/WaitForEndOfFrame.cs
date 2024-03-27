namespace Snake.Coroutines
{
    /// <summary>
    /// Commands this coroutine to wait for exactly one frame
    /// </summary>
    public class WaitForEndOfFrame : YieldInstruction
    {
        public override bool ShouldYield => false;
    }
}
