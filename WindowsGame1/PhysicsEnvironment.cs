using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace GravityShift
{
    /// <summary>
    /// Environment where Physics objects exist
    /// </summary>
    class PhysicsEnvironment
    {
        //Vectorized directions
        public Vector2 DIRECTION_UP = new Vector2(0, -1f);
        public Vector2 DIRECTION_DOWN = new Vector2(0, 1f);
        public Vector2 DIRECTION_LEFT = new Vector2(-1f, 0);
        public Vector2 DIRECTION_RIGHT = new Vector2(1f, 0);

        //Default values used for physics environments
        public const int DEFAULT_TERMINAL_SPEED = 20;//pixels per update MAX speed;
        public const int DEFAULT_GRAVITY_FORCE = 1;//(pixels per update) per update;
        public const float DEFAULT_EROSION_FACTOR = .99f;
        public const float DEFAULT_DIRECTIONAL_FORCE = .3f;

        //Gravity directions magnifiers to allow for different forces in each direction
        private float mGravityUpMagnifier = DEFAULT_DIRECTIONAL_FORCE;            
        private float mGravityDownMagnifier = DEFAULT_DIRECTIONAL_FORCE;
        private float mGravityLeftMagnifier = DEFAULT_DIRECTIONAL_FORCE;
        private float mGravityRightMagnifier = DEFAULT_DIRECTIONAL_FORCE;

        /// <summary>
        /// Gets the gravity magnifier for the given direction
        /// </summary>
        /// <param name="direction">Magnifier that we are getting</param>
        /// <returns>The magnitude of the given direction</returns>
        public float GetGravityMagnifier(GravityDirections direction)
        {
            if (direction == GravityDirections.Up) return mGravityUpMagnifier;
            else if (direction == GravityDirections.Down) return mGravityDownMagnifier;
            else if (direction == GravityDirections.Left) return mGravityLeftMagnifier;
            else return mGravityRightMagnifier;
        }

        /// <summary>
        /// Sets the magnitude of force in the given direction
        /// </summary>
        /// <param name="direction">The direction of gravity to change</param>
        /// <param name="magnitude">Magnitude for the given direction</param>
        public void SetDirectionalMagnifier(GravityDirections direction, float magnitude)
        {
            if (direction == GravityDirections.Up) mGravityUpMagnifier = magnitude;
            if (direction == GravityDirections.Down) mGravityDownMagnifier = magnitude;
            if (direction == GravityDirections.Left) mGravityLeftMagnifier = magnitude;
            if (direction == GravityDirections.Right) mGravityRightMagnifier = magnitude;
        }

        /// <summary>
        /// Increments the force magnitude of the given direction by .01 
        /// </summary>
        /// <param name="direction">Force Direction to increment</param>
        public void IncrementDirectionalMagnifier(GravityDirections direction)
        {
            if (direction == GravityDirections.Up) mGravityUpMagnifier += .01f;
            if (direction == GravityDirections.Down) mGravityDownMagnifier += .01f;
            if (direction == GravityDirections.Left) mGravityLeftMagnifier += .01f;
            if (direction == GravityDirections.Right) mGravityRightMagnifier += .01f;
        }

        /// <summary>
        /// Decrements the force magnitude of the given direction by .01
        /// </summary>
        /// <param name="direction"></param>
        public void DecrementDirectionalMagnifier(GravityDirections direction)
        {
            if (direction == GravityDirections.Up) mGravityUpMagnifier -= .01f;
            if (direction == GravityDirections.Down) mGravityDownMagnifier -= .01f;
            if (direction == GravityDirections.Left) mGravityLeftMagnifier -= .01f;
            if (direction == GravityDirections.Right) mGravityRightMagnifier -= .01f;
        }

        private int mTerminalSpeed = DEFAULT_TERMINAL_SPEED;
        public int TerminalSpeed
        {
            get { return mTerminalSpeed; }
            set { mTerminalSpeed = value; }
        }

        private int mGravityMagnitude = DEFAULT_GRAVITY_FORCE;
        public int GravityMagnitude
        {
            get { return mGravityMagnitude; }
            set { mGravityMagnitude = value; }
        }

        private float mErosionFactor = DEFAULT_EROSION_FACTOR;
        public float ErosionFactor
        {
            get { return mErosionFactor; }
            set { mErosionFactor = value; }
        }

        private GravityDirections mGravityDirection = GravityDirections.Down;
        public GravityDirections GravityDirection
        {
            get { return mGravityDirection; }
            set { mGravityDirection = value; }
        }

        /// <summary>
        /// Gets the total gravity force in this environment
        /// </summary>
        public Vector2 GravityForce
        {
            get
            {
                Vector2 gravityDirection = DIRECTION_DOWN;
                float gravityMagnifier = mGravityDownMagnifier;
                if (mGravityDirection == GravityDirections.Up)
                { gravityDirection = DIRECTION_UP; gravityMagnifier = mGravityUpMagnifier;}
                if (mGravityDirection == GravityDirections.Left)
                { gravityDirection = DIRECTION_LEFT; gravityMagnifier = mGravityLeftMagnifier;}
                if (mGravityDirection == GravityDirections.Right)
                { gravityDirection = DIRECTION_RIGHT; gravityMagnifier = mGravityRightMagnifier;}

                return Vector2.Multiply(gravityDirection, gravityMagnifier * mGravityMagnitude);
            }
        }
    }
}
