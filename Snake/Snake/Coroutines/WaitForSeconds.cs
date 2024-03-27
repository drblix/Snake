using System;

namespace Snake.Coroutines
{
    /// <summary>
    /// Commands this coroutine to wait for a specified amount of seconds
    /// </summary>
    public class WaitForSeconds : YieldInstruction
    {
        public override bool ShouldYield
        {
            get
            {
                _internalTimer += Time.DeltaTime;
                return _internalTimer < _secondsToWait;
            }
        }

        private readonly float _secondsToWait;
        private float _internalTimer = 0f;

        public WaitForSeconds(float seconds)
        {
            if (seconds < 0f)
                throw new ArgumentException("seconds must be greater than or equal to 0", nameof(seconds));

            _secondsToWait = seconds;
        }
    }
}
