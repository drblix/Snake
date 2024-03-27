
namespace Snake.Interfaces
{
    /// <summary>
    /// Contract that allows an object to be drawn
    /// </summary>
    public interface IDrawable
    {
        /// <summary>
        /// Allows an object to draw to the screen<br><br></br></br>
        /// See <see cref="Globals.SpriteBatch"/> to access drawing features
        /// </summary>
        public void Draw();
    }
}
