using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Snake.Objects
{
    public class SnakeObject : Interfaces.IUpdateable, Interfaces.IDrawable
    {
        public enum Direction
        {
            Up,
            Down,
            Left,
            Right
        }

        private readonly LinkedList<SnakeNode> _body = new();

        private readonly int _size;
        private readonly float _moveDelay;
        private readonly Color _color;

        private Direction _direction;
        private float _moveTimer = 0f;


        public SnakeObject(Point start, float moveDelay, Color color, int size)
        {
            _moveDelay = moveDelay;
            _color = color;
            _size = size;

            // creating head
            _body.AddFirst(new SnakeNode(start, size));
        }

        public void Update()
        {
            if (_direction != Direction.Down && (Keyboard.GetState().IsKeyDown(Keys.Up) || Keyboard.GetState().IsKeyDown(Keys.W)))
                _direction = Direction.Up;
            else if (_direction != Direction.Up && (Keyboard.GetState().IsKeyDown(Keys.Down) || Keyboard.GetState().IsKeyDown(Keys.S)))
                _direction = Direction.Down;
            else if (_direction != Direction.Right && (Keyboard.GetState().IsKeyDown(Keys.Left) || Keyboard.GetState().IsKeyDown(Keys.A)))
                _direction = Direction.Left;
            else if (_direction != Direction.Left && (Keyboard.GetState().IsKeyDown(Keys.Right) || Keyboard.GetState().IsKeyDown(Keys.D)))
                _direction = Direction.Right;

            if (Keyboard.GetState().IsKeyDown(Keys.Space))
                Grow();

            _moveTimer += Time.DeltaTime;
            if (_moveTimer > _moveDelay)
            {
                _moveTimer = 0f;
                Move();
            }
        }

        public void Draw()
        {
            LinkedListNode<SnakeNode> node = _body.First;

            do
            {
                Globals.SpriteBatch.Draw(
                    Globals.PixelTexture,
                    node.Value.Position.ToVector2(),
                    null,
                    _color,
                    0f,
                    Vector2.Zero,
                    _size * .85f,
                    SpriteEffects.None,
                    0f
                );

                node = node.Next;
            } while (node != null);
        }

        private void Grow()
        {
            /*
            Point lastPosition = _body.Last.Value.Position;

            // can we grow to the right?
            Point newPosition = lastPosition + new Point(_size, 0);
            if (!IsOccupied(newPosition))
            // check if position is occupied by another node

            // can we grow to the left?
            newPosition = lastPosition + new Point(-_size, 0);
            // check if position is occupied by another node

            // can we grow above?
            newPosition = lastPosition + new Point(0, -_size);
            // check if position is occupied by another node

            // can we grow below?
            newPosition = lastPosition + new Point(0, _size);
            // check if position is occupied by another node

            _body.AddLast(new SnakeNode(newPosition, _size));
            */

            _body.AddLast(new SnakeNode(_body.Last.Value.Position + new Point(_size, 0), _size));
        }

        private void Move()
        {
            if (_body.Count <= 1)
            {
                _body.First.Value.Shift(_direction);
                return;
            }


            // shift every other node to previous node's position
            Point lastPos = _body.Last.Value.Position;
            LinkedListNode<SnakeNode> node = _body.Last.Previous;

            while (node != null)
            {
                node.Next.Value.Position = node.Value.Position;
                node = node.Previous;
            }

            _body.First.Value.Position = lastPos;
            _body.First.Value.Shift(_direction);

            /*
            LinkedListNode<SnakeNode> node = _body.First;
            
            node = node.Next;
            while (node != null)
            {
                node.Value.Position = node.Previous.Value.Position;

                node = node.Next;
            }

            _body.First.Value.Shift(_direction);
            */
        }

        private bool IsOccupied(Point point)
        {
            LinkedListNode<SnakeNode> node = _body.First;

            do
            {
                if (node.Value.Position == point) return true;
                node = node.Next;
            } while (node != null);

            return false;
        }
    }
}
