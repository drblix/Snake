
using Microsoft.Xna.Framework;

namespace Snake.Objects
{
    public class SnakeNode
    {
        public Point Position
        {
            get => _position;
            set => _position = value;
        }

        private Point _position;

        private readonly int _cellSize;

        public SnakeNode(Point position, int cellSize)
        {
            _position = position;
            _cellSize = cellSize;
        }

        /// <summary>
        /// Shifts the specified node in the provided direction
        /// </summary>
        public void Shift(SnakeObject.Direction direction) => _position = FuturePosition(direction);

        /// <summary>
        /// Returns the future position of this node if the provided direction were to be applied
        /// </summary>
        public Point FuturePosition(SnakeObject.Direction direction)
        {
            return direction switch
            {
                SnakeObject.Direction.Up => _position + new Point(0, -_cellSize),
                SnakeObject.Direction.Down => _position + new Point(0, _cellSize),
                SnakeObject.Direction.Left => _position + new Point(-_cellSize, 0),
                SnakeObject.Direction.Right => _position + new Point(_cellSize, 0),
                _ => throw new System.Exception("this should never happen, and if it does, my life is a lie"),
            };
        }
    }
}
