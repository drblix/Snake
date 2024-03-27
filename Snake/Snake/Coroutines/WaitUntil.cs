namespace Snake.Coroutines
{
    public class WaitUntil : YieldInstruction
    {
        public override bool ShouldYield => !(bool)(_waitUntil?.Invoke());

        public delegate bool WaitUntilDelegate();
        private readonly WaitUntilDelegate _waitUntil;

        public WaitUntil(WaitUntilDelegate @delegate)
        {
            _waitUntil = @delegate;
        }
    }
}
