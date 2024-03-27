
namespace Snake.Interfaces
{
    /// <summary>
    /// Contract that allows an object to be updated every frame
    /// </summary>
    public interface IUpdateable
    {
        /// <summary>
        /// Updates every frame<br><br></br></br>
        /// See <see cref="Time.DeltaTime"/> for access to the time that has passed since the last frame
        /// </summary>
        public void Update();
    }
}
