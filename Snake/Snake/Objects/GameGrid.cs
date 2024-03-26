using Microsoft.Xna.Framework;

namespace Snake.Objects
{
    public class GameGrid : Interfaces.IUpdateable, Interfaces.IDrawable
    {
        private readonly SnakeObject _snake;

        private readonly int _cellSize;
        private readonly int _width;
        private readonly int _height;

        public GameGrid(int width, int height, int cellSize)
        {
            _width = width;
            _height = height;
            _cellSize = cellSize;
            //_snake = new SnakeObject( .1f, Color.Lime, cellSize);
        }

        public void Update()
        {
            _snake.Update();
        }

        public void Draw()
        {
            _snake.Draw();
        }

    }
}
