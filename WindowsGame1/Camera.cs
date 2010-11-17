using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using System.Text;

namespace GravityShift
{
    public class Camera
    {
        #region Member Variables

        private Vector3 mPosition;

        private float mHeight;
        private float mWidth;
                
        private float mZoom;

        #endregion

        /// <summary>
        /// Construcs a camera object.
        /// </summary>
        /// <param name="viewport">Current viewport for the game</param>
        public Camera(Viewport viewport)
        {
            mPosition = new Vector3(0.0f, 0.0f, 0.0f);
            mZoom = 1.0f;
            mHeight = viewport.Height;
            mWidth = viewport.Width;
        }

        /// <summary>
        /// Gets the current transformation using the position and zoom.
        /// </summary>
        /// <returns>A matrix with the current transformations</returns>
        public Matrix get_transformation()
        {
              return Matrix.CreateTranslation(new Vector3(-mPosition.X, -mPosition.Y, 0.0f)) *
                        Matrix.CreateScale(new Vector3(Zoom, Zoom, 0.0f)) *
                        Matrix.CreateTranslation(new Vector3(mWidth * 0.5f, mHeight * 0.5f, 0.0f));
        }

        /// <summary>
        /// Gets and sets the mZoom variable
        /// </summary>
        /// <returns>A float value representing the zoom (1.0 is default, <1 zoom out, >1 zoom in)</returns>
        public float Zoom
        {
            get { return mZoom; }
            set { mZoom = value; }
        }

        /// <summary>
        /// Gets and sets the mPosition variable
        /// </summary>
        /// <returns>A Vector3 value representing the position of the camera</returns>
        public Vector3 Postion
        {
            get { return mPosition; }
            set { mPosition = value; }
        }

        /// <summary>
        /// Moves the camera by the desired amount
        /// </summary>
        /// <param name="amount">Vector3 representing the amount to move in each direction</param>
        public void Move(Vector3 amount)
        {
            mPosition += amount;
        }
    }
}