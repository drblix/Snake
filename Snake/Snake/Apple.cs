using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Snake.Interfaces;
using Snake.Objects;
using System.Collections.Generic;

namespace Snake
{
    public class Apple : Interfaces.IDrawable
    {
        public Point Position => _position;
        private Point _position;

        private readonly Color _colour;
        private readonly int _size;

        public Apple(Color colour, int size)
        {
            _colour = colour;
            _size = size;
        }

        public void Draw()
        {
            Globals.SpriteBatch.Draw(
                Globals.PixelTexture,
                _position.ToVector2(),
                null,
                _colour,
                0f,
                Vector2.Zero,
                _size * .85f,
                SpriteEffects.None,
                0f
            );
        }

        /// <summary>
        /// Creates and sets the apple to a new position that is within the bounds of the screen and is not occupied by the snake body
        /// </summary>
        /// <param name="snake">The active snake object</param>
        /// <param name="viewport">The viewport being used by the game</param>
        public void GotoNewPosition(SnakeObject snake, Viewport viewport)
        {
            Point newPoint = CreateRandomPoint();
            while (snake.IsOccupiedBySnake(newPoint))
                newPoint = CreateRandomPoint();
            _position = newPoint;

            // creating new point that is snapped to size increments of apple
            Point CreateRandomPoint() => new((Globals.Random.Next(0, viewport.Width) / _size) * _size, (Globals.Random.Next(0, viewport.Height) / _size) * _size);
        }
    }
}
