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
 //       private Vector3 lookAt;

        private float mHeight;
        private float mWidth;
        
        private Vector2 mCenter;
        private float aspectRatio;
        
 //       private Matrix projMatrix;
 //       private Matrix lookMatrix;

        private float mZoom;

        #endregion

        /*
         * 
         */
        public Camera(Viewport viewport)
        {
            mPosition = new Vector3(0.0f, 0.0f, 0.0f);
            mZoom = 1.0f;
            mHeight = viewport.Height;
            mWidth = viewport.Width;
            mCenter = new Vector2(mWidth / 2, mHeight / 2);
            aspectRatio = mWidth / mHeight;
//            projMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45.0f), aspectRatio, 1.0f, 10000.0f);
        }

        public Matrix get_transformation()
        {
              return Matrix.CreateTranslation(new Vector3(-mPosition.X, -mPosition.Y, 0.0f)) *
                        Matrix.CreateScale(new Vector3(Zoom, Zoom, 0.0f)) *
                        Matrix.CreateTranslation(new Vector3(mWidth * 0.5f, mHeight * 0.5f, 0.0f));
        }

        public float Zoom
        {
            get { return mZoom; }
            set { mZoom = value; }
        }

        public Vector3 Postion
        {
            get { return mPosition; }
            set { mPosition = value; }
        }

        public void Move(Vector3 amount)
        {
            mPosition += amount;
        }

        public Vector2 ScreenCenter
        {
            get { return mCenter; }
            set { mCenter = value; }
        }

//        public Vector3 LookAt
//        {
//            get { return this.lookAt; }
//            set { this.lookAt = value; }
//        }

//        public Matrix LookMatrix
//        {
//            get { return this.lookMatrix; }
//        }

//        public Matrix ProjMatrix
//        {
//            get { return this.projMatrix; }
//        }

//        public void Update()
//        {
//            lookMatrix = Matrix.CreateLookAt(this.mPosition, this.lookAt, Vector3.Up);    
//        }
    }
}