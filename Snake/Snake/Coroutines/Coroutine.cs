
using System.Collections.Generic;
using System.Diagnostics;

namespace Snake.Coroutines
{
    public class Coroutine
    {
        public bool Finished => _finished;

        private readonly IEnumerator<YieldInstruction> _enumerator;
        private bool _finished;

        public Coroutine(IEnumerator<YieldInstruction> enumerator)
        {
            _enumerator = enumerator;
        }

        public void Update(float deltaTime)
        {
            if (_enumerator.Current == null)
                _enumerator.MoveNext();

            YieldInstruction instruction = _enumerator.Current;
            if (!instruction.ShouldYield)
                _finished = !_enumerator.MoveNext();
        }
    }
}
