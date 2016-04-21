using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MrGravity
{
    public class Camera
    {
        #region Member Variables

        private readonly float _mHeight;
        private readonly float _mWidth;

        #endregion

        /// <summary>
        /// Constructs a camera object.
        /// </summary>
        /// <param name="viewport">Current viewport for the game</param>
        public Camera(Viewport viewport)
        {
            Position = new Vector3(0.0f, 0.0f, 0.0f);
            Zoom = 0.75f;
            _mHeight = viewport.Height;
            _mWidth = viewport.Width;
        }

        /// <summary>
        /// Gets the current transformation using the position and zoom.
        /// </summary>
        /// <returns>A matrix with the current transformations</returns>
        public Matrix get_transformation()
        {
              return Matrix.CreateTranslation(new Vector3(-Position.X, -Position.Y, 0.0f)) *
                           Matrix.CreateScale(new Vector3(Zoom, Zoom, 0.0f)) *
                           Matrix.CreateTranslation(new Vector3(_mWidth * 0.3f, _mHeight * 0.3f, 0.0f));
        }

        /// <summary>
        /// Gets and sets the mZoom variable
        /// </summary>
        /// <returns>A float value representing the zoom (1.0 is default, <1 zoom out, >1 zoom in)</returns>
        public float Zoom { get; set; }

        /// <summary>
        /// Gets and sets the mPosition variable
        /// </summary>
        /// <returns>A Vector3 value representing the position of the camera</returns>
        public Vector3 Position { get; set; }

        /// <summary>
        /// Moves the camera by the desired amount
        /// </summary>
        /// <param name="amount">Vector3 representing the amount to move in each direction</param>
        public void Move(Vector3 amount)
        {
            Position += amount;
        }
    }
}