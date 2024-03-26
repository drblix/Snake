using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Snake.Objects;
using System.Runtime.CompilerServices;

namespace Snake
{
    public class SnakeGame : Game
    {
        public delegate void UpdateDelegate(float deltaTime);
        public static event UpdateDelegate OnUpdate;

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private GameGrid _grid;
        
        private SnakeObject _snake;

        public SnakeGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            Globals.Init(null, Content);
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            // create game grid
            const int GRID_WIDTH = 46;
            const int GRID_HEIGHT = 30;
            const int CELL_SIZE = 12;

            GraphicsDevice.Viewport = new Viewport(0, 0, GRID_WIDTH * CELL_SIZE, GRID_HEIGHT * CELL_SIZE);

            _snake = new SnakeObject(
                new Point(_graphics.PreferredBackBufferWidth / 2, _graphics.PreferredBackBufferHeight / 2), 
                .1f, 
                Color.Lime, 
                CELL_SIZE
            );

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            Globals.Init(_spriteBatch, Content);

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            OnUpdate?.Invoke(deltaTime);

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            _snake.Update();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            _snake.Draw();
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
