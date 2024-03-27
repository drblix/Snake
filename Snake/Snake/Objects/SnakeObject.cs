using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Snake.Coroutines;
using System;
using System.Collections.Generic;

namespace Snake.Objects
{
    public class SnakeObject : Interfaces.IUpdateable, Interfaces.IDrawable
    {
        public event Action OnDeath;

        /// <summary>
        /// Represents directions in which the snake can move
        /// </summary>
        public enum Direction
        {
            Up,
            Down,
            Left,
            Right
        }

        public int Score => _score;
        public int Length => _body.Count;

        private readonly LinkedList<SnakeNode> _body = new();

        private readonly int _cellSize;
        private readonly float _moveDelay;
        private readonly float _fillOfCell;
        private readonly Viewport _viewport;
        private readonly SoundEffect _eatSound;

        private Apple _apple;
        private Color _colour;
        private Color _headColour;

        private Direction _direction;
        private Direction _desiredDirection;

        private int _score = 0;
        private float _moveTimer = 0f;
        private bool _alive = true;


        /// <summary>
        /// Creates a new snake object
        /// </summary>
        /// <param name="viewport">The current viewport being used by the game</param>
        /// <param name="start">The position where the snake starts</param>
        /// <param name="moveDelay">The delay (in seconds) between the snake moving</param>
        /// <param name="snakeColour">The snakeColour at which the snake is to be rendered</param>
        /// <param name="cellSize">The size (in pixels) of each snake segment</param>
        /// <param name="startingSize">The number of nodes the snake should start with (not including the head)</param>
        /// <param name="fillOfCell">The percent [0.05, 1] of the cell that should be filled when rendering each snake node</param>
        /// <param name="headColour">The colour at which the snake head should be rendered relative to the rest of the body</param>
        public SnakeObject(Viewport viewport, Point start, float moveDelay, Color snakeColour, int cellSize, int startingSize = 0, float fillOfCell = .85f, Color? headColour = null)
        {
            _viewport = viewport;
            _moveDelay = moveDelay;
            _colour = snakeColour;
            _cellSize = cellSize;
            _fillOfCell = MathHelper.Clamp(fillOfCell, 0.05f, 1f);

            if (headColour == null)
                _headColour = snakeColour;
            else
                _headColour = headColour.Value;

            // creating head
            _body.AddFirst(new SnakeNode(start, _cellSize));

            if (startingSize > 0)
                for (int i = 0; i < startingSize; i++)
                    _body.AddLast(new SnakeNode(_body.Last.Value.Position, _cellSize));

            _apple = new(this, viewport, Color.Red, _cellSize);
            _eatSound = Globals.Load<SoundEffect>("eat");
        }

        /// <summary>
        /// Updates the snake object every frame<br><br></br></br>
        /// Changes direction depending on keystrokes during this update cycle
        /// </summary>
        public void Update()
        {
            if (!_alive) return;

            if (_direction != Direction.Down && (Keyboard.GetState().IsKeyDown(Keys.Up) || Keyboard.GetState().IsKeyDown(Keys.W)))
                _desiredDirection = Direction.Up;
            else if (_direction != Direction.Up && (Keyboard.GetState().IsKeyDown(Keys.Down) || Keyboard.GetState().IsKeyDown(Keys.S)))
                _desiredDirection = Direction.Down;
            else if (_direction != Direction.Right && (Keyboard.GetState().IsKeyDown(Keys.Left) || Keyboard.GetState().IsKeyDown(Keys.A)))
                _desiredDirection = Direction.Left;
            else if (_direction != Direction.Left && (Keyboard.GetState().IsKeyDown(Keys.Right) || Keyboard.GetState().IsKeyDown(Keys.D)))
                _desiredDirection = Direction.Right;


            _moveTimer += Time.DeltaTime;
            if (_moveTimer > _moveDelay)
            {
                _direction = _desiredDirection;
                _moveTimer = 0f;
                Move();

                if (IsGameOver(_body.First.Value.Position))
                {
                    _alive = false;
                    _apple = null;
                    CoroutineManager.CreateCoroutine(GameOverRoutine());
                    return;
                }

                if (_apple?.Position == _body.First.Value.Position)
                {
                    Grow(_apple.Substance);
                    _apple = new(this, _viewport, Color.Red, _cellSize);
                    _score++;
                    _eatSound.Play();
                }
            }   
        }

        public void Draw()
        {
            // iterates through each node in the linked list, rendering each node
            LinkedListNode<SnakeNode> node = _body.First;

            do
            {
                Globals.SpriteBatch.Draw(
                    Globals.PixelTexture,
                    node.Value.Position.ToVector2(),
                    null,
                    node == _body.First ? _headColour : _colour,
                    0f,
                    Vector2.Zero,
                    _cellSize * _fillOfCell,
                    SpriteEffects.None,
                    0f
                );

                node = node.Next;
            } while (node != null);
            
            _apple?.Draw();
        }

        /// <summary>
        /// Grows the snake
        /// </summary>
        /// <param name="amount">Amount of nodes to add onto the snake</param>
        private void Grow(int amount = 1)
        {
            // minimum value must be >= 1
            amount = MathHelper.Max(amount, 1);

            for (int i = 0; i < amount; i++)
                _body.AddLast(new SnakeNode(_body.Last.Value.Position, _cellSize));
        }

        /// <summary>
        /// Moves the snake in the current direction
        /// </summary>
        private void Move()
        {
            if (IsGameOver(_body.First.Value.FuturePosition(_direction)))
            {
                _alive = false;
                _apple = null;
                CoroutineManager.CreateCoroutine(GameOverRoutine());
                return;
            }

            // just shift head if no other nodes in body
            if (_body.Count <= 1)
            {
                _body.First.Value.Shift(_direction);
                return;
            }

            // shift every other node to previous node's position
            LinkedListNode<SnakeNode> node = _body.Last.Previous;

            while (node != null)
            {
                node.Next.Value.Position = node.Value.Position;
                node = node.Previous;
            }

            // shifts head to new position relative to direction
            _body.First.Value.Shift(_direction);
        }


        /// <summary>
        /// Checks if the provided point has collided either with itself or the boundary of the screen
        /// </summary>
        private bool IsGameOver(Point point) => IsOccupiedBySnake(point) || IsOutOfBounds(point);

        /// <summary>
        /// Checks if the specified point falls outside of the boundaries of the screen
        /// </summary>
        /// <param name="point">Point to check</param>
        private bool IsOutOfBounds(Point point) => point.X < 0 || point.Y < 0 || point.X > _viewport.Width - _cellSize || point.Y > _viewport.Height - _cellSize;

        /// <summary>
        /// Checks if the specified point is occupied by the body of the snake (does NOT include the head)
        /// </summary>
        /// <param name="point">Point to check</param>
        public bool IsOccupiedBySnake(Point point)
        {
            // cannot collide with head
            if (_body.Count <= 1) return false;

            // node after head
            LinkedListNode<SnakeNode> node = _body.First.Next;

            // check each node's position to see if it occupies the provided point
            do
            {
                if (node.Value.Position == point) return true;
                node = node.Next;
            } while (node != null);

            return false;
        }

        private IEnumerator<YieldInstruction> GameOverRoutine()
        {
            yield return new WaitForSeconds(.85f);
            for (int i = 0; i < 4; i++)
            {
                _colour = Color.Red;
                _headColour = Color.Red;
                yield return new WaitForSeconds(.2f);
                _colour = Color.Lime;
                _headColour = Color.Lime;
                yield return new WaitForSeconds(.2f);
            }

            OnDeath?.Invoke();
        }
    }
}
