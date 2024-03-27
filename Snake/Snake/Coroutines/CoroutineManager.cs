using System.Collections.Generic;

namespace Snake.Coroutines
{
    public static class CoroutineManager
    {
        private readonly static List<Coroutine> coroutines = new();
        private readonly static List<YieldInstruction> yieldInstructions = new();

        public static void Init()
        {
            coroutines.Clear();
            yieldInstructions.Clear();

            SnakeGame.OnUpdate += Update;
        }

        public static void CreateCoroutine(IEnumerator<YieldInstruction> enumerator)
        {
            Coroutine coroutine = new(enumerator);
            coroutines.Add(coroutine);
        }

        private static void Update(float deltaTime)
        {
            if (coroutines.Count == 0) return;

            for (int i = 0; i < coroutines.Count; i++)
            {
                Coroutine coroutine = coroutines[i];

                coroutine.Update(deltaTime);
                if (coroutine.Finished)
                    coroutines.RemoveAt(i);
            }
        }
    }
}
