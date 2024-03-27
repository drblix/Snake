using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Snake.Coroutines;
using Snake.Objects;
using System.Runtime.CompilerServices;

namespace Snake
{
    public class SnakeGame : Game
    {
        public delegate void UpdateDelegate(float deltaTime);
        public static event UpdateDelegate OnUpdate;

        private readonly GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private SpriteFont _scoreText;
        
        private SnakeObject _snake;

        public SnakeGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            Globals.Init(null, Content);
            CoroutineManager.Init();
        }

        protected override void Initialize()
        {
            // grid settings
            const int GRID_WIDTH = 48;
            const int GRID_HEIGHT = 36;
            const int CELL_SIZE = 16;

            // creates a new viewport
            Viewport newViewport = new(0, 0, GRID_WIDTH * CELL_SIZE, GRID_HEIGHT * CELL_SIZE);
            GraphicsDevice.Viewport = newViewport;

            // adjusts size of window
            _graphics.PreferredBackBufferWidth = GRID_WIDTH * CELL_SIZE;
            _graphics.PreferredBackBufferHeight = GRID_HEIGHT * CELL_SIZE;
            _graphics.ApplyChanges();

            // creates new snake and links event
            CreateSnake(newViewport, .15f, Color.Lime, CELL_SIZE);
            _snake.OnDeath += Snake_OnDeath;
            
            base.Initialize();
        }

        private void Snake_OnDeath()
        {
            _snake.OnDeath -= Snake_OnDeath;
            CreateSnake(GraphicsDevice.Viewport, .15f, Color.Lime, 16);
            _snake.OnDeath += Snake_OnDeath;
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _scoreText = Content.Load<SpriteFont>("Text");

            Globals.Init(_spriteBatch, Content);
        }

        protected override void Update(GameTime gameTime)
        {
            // on update event is linked only to static utility classes; important this is updated before other objects are updated
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            OnUpdate?.Invoke(deltaTime);

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            _snake.Update();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // draw snake every frame
            _spriteBatch.Begin();
            _snake.Draw();
            _spriteBatch.DrawString(_scoreText, _snake.Score.ToString(), new Vector2(_graphics.PreferredBackBufferWidth / 2, _graphics.PreferredBackBufferHeight * .015f), Color.White);
            _spriteBatch.End();

            base.Draw(gameTime);
        }

        /// <summary>
        /// Creates a new snake object
        /// </summary>
        /// <param name="viewport">The current viewport being used by the game</param>
        /// <param name="moveDelay">The delay (in seconds) between the snake moving</param>
        /// <param name="snakeColour">The colour at which the snake is to be rendered</param>
        /// <param name="cellSize">The size (in pixels) of each snake segment</param>
        /// <param name="startingSize">The number of nodes the snake should start with (not including the head)</param>
        /// <param name="fillOfCell">The percent [0, 1] of the cell that should be filled when rendering each snake node</param>
        /// <param name="headColour">The colour at which the snake head should be rendered relative to the rest of the body</param>
        private void CreateSnake(Viewport viewport, float moveDelay, Color snakeColour, int cellSize, int startingSize = 0, float fillOfCell = .85f, Color? headColour = null)
        {
            _snake = new SnakeObject(
                viewport,
                // starts snake in middle of screen
                new Point(_graphics.PreferredBackBufferWidth / 2, _graphics.PreferredBackBufferHeight / 2),
                moveDelay,
                snakeColour,
                cellSize,
                startingSize,
                fillOfCell,
                headColour
            );
        }
    }
}
