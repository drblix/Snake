using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

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

        public static Random Random => _random;

        private static ContentManager _contentManager;
        private static Random _random;

        public static void Init(SpriteBatch spriteBatch, ContentManager contentManager)
        {
            _spriteBatch = spriteBatch;
            _contentManager = contentManager;

            _random = new();
        }
    }
}
