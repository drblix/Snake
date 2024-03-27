using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Snake.Objects
{
    /// <summary>
    /// Item the snake can eat to grow in size
    /// </summary>
    public class Apple : Interfaces.IDrawable
    {
        /// <summary>
        /// How much this apple will grow the snake upon consumption
        /// </summary>
        public int Substance => _substance;

        /// <summary>
        /// The position of this apple
        /// </summary>
        public Point Position => _position;
        private Point _position;

        private readonly Color _colour;
        private readonly int _size;
        private readonly int _substance;

        /// <summary>
        /// Creates a new apple
        /// </summary>
        /// <param name="snake">The current snake object</param>
        /// <param name="viewport">The current viewport in use</param>
        /// <param name="colour">The colour of the apple</param>
        /// <param name="size">The size of the apple (in pixels)</param>
        /// <param name="chanceOfSpecial">The chance this apple has of being deemed "special"<br></br>Specifically, 1 / number</param>
        /// <param name="minSpecial">The minimum special value that can be applied</param>
        /// <param name="maxSpecial">The maximum special value that can be applied</param>
        public Apple(SnakeObject snake, Viewport viewport, Color colour, int size, int chanceOfSpecial = 8, int minSpecial = 5, int maxSpecial = 8)
        {
            _colour = colour;
            _size = size;

            GotoNewPosition(snake, viewport);

            if (Globals.Random.Next(chanceOfSpecial) == 0)
            {
                _substance = Globals.Random.Next(minSpecial, maxSpecial + 1);
                _colour = Color.Yellow;
            }
            else
                _substance = 1;
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
        private void GotoNewPosition(SnakeObject snake, Viewport viewport)
        {
            Point newPoint = CreateRandomPoint();
            while (snake.IsOccupiedBySnake(newPoint))
                newPoint = CreateRandomPoint();
            _position = newPoint;

            // creating new point that is snapped to size increments of apple
            Point CreateRandomPoint() => new(Globals.Random.Next(0, viewport.Width) / _size * _size, Globals.Random.Next(0, viewport.Height) / _size * _size);
        }
    }
}
