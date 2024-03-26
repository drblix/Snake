using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Snake
{
    /// <summary>
    /// Contains commonly used items used throughout different objects
    /// </summary>
    public static class Globals
    {
        public static SpriteBatch SpriteBatch => _spriteBatch;

        private static SpriteBatch _spriteBatch;

        public static Texture2D PixelTexture => _pixelTexture ??= _contentManager.Load<Texture2D>("pixel");

        private static Texture2D _pixelTexture;

        private static ContentManager _contentManager;

        public static void Init(SpriteBatch spriteBatch, ContentManager contentManager)
        {
            _spriteBatch = spriteBatch;
            _contentManager = contentManager;
        }
    }
}
