
namespace Snake
{
    public static class Time
    {
        /// <summary>
        /// The time that has passed (in seconds) since the last frame
        /// </summary>
        public static float DeltaTime => _deltaTime;
        private static float _deltaTime;

        static Time()
        {
            SnakeGame.OnUpdate += SnakeGame_OnUpdate;
        }

        private static void SnakeGame_OnUpdate(float deltaTime)
        {
            _deltaTime = deltaTime;
        }
    }
}
