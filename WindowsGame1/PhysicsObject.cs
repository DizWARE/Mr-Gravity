﻿using System;
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
    /// Represents an object that has rules based on physics
    /// </summary>
    abstract class PhysicsObject : GameObject
    {
        protected PhysicsEnvironment mEnvironment;
        
        //All forces applied to this physicsObject
        private Vector2 mGravityForce = new Vector2(0,0);
        private Vector2 mResistiveForce = new Vector2(1,1);
        private Vector2 mAdditionalForces = new Vector2(0, 0);

        private Vector2 mVelocity = new Vector2(0, 0);

        /// <summary>
        /// Directional force on this object
        /// </summary>
        public Vector2 TotalForce
        {
            get {  return (Vector2.Add(mGravityForce,mAdditionalForces));  }
        }

        /// <summary>
        /// Speed and direction of this object
        /// </summary>
        public Vector2 ObjectVelocity
        {
            get { return mVelocity;  }
            set { mVelocity = value; }
        }

        /// <summary>
        /// Constructs a PhysicsObject; Loads the required info from the content pipeline, and defines its size and location
        /// </summary>
        /// <param name="content">Content pipeline</param>
        /// <param name="spriteBatch">The drawing canvas of the game. Used for collision detection with the level</param>
        /// <param name="name">Name of the physics object so that it can be loaded</param>
        /// <param name="scalingFactors">Factor for the image resource(i.e. half the size would be (.5,.5)</param>
        /// <param name="initialPosition">Position of where this object starts in the level</param>
        public PhysicsObject(ContentManager content, String name, Vector2 scalingFactors, Vector2 initialPosition, ref PhysicsEnvironment environment, float friction)
            :base(content,name,scalingFactors,initialPosition, friction)
        {
            mEnvironment = environment;
            UpdateBoundingBoxes();
        }

        /// <summary>
        /// Adds an additional force to the physics object
        /// </summary>
        /// <param name="force">Force to be added</param>
        public void AddForce(Vector2 force)
        {
            mAdditionalForces = Vector2.Add(mAdditionalForces, force);
        }

        /// <summary>
        /// TEMP METHOD - WILL GIVE THE PLAYER THE ABILITY TO FALL FROM ONE END OF THE SCREEN TO THE OTHER
        /// </summary>
        public void FixForBounds(int width, int height)
        {
            if (mPosition.X < 0) mPosition.X += width;
            if (mPosition.Y < 0) mPosition.Y += height;

            mPosition.X %= width;
            mPosition.Y %= height;
        }

        /// <summary>
        /// Reorient gravity in the given direction
        /// </summary>
        /// <param name="direction">Direction to enforce gravity on</param>
        public void ChangeGravityForceDirection(GravityDirections direction)
        {
            mResistiveForce = new Vector2(1, 1);

            if (direction == GravityDirections.Up || 
                direction == GravityDirections.Down) 
                mResistiveForce.X = mEnvironment.ErosionFactor;
            else 
                mResistiveForce.Y = mEnvironment.ErosionFactor;

            mGravityForce = mEnvironment.GravityForce;
        }

        /// <summary>
        /// Enforces a maximum speed that a force can 
        /// </summary>
        private void EnforceTerminalVelocity()
        {
            if (mVelocity.X > mEnvironment.TerminalSpeed)
                mVelocity.X = mEnvironment.TerminalSpeed;
            if (mVelocity.X < -mEnvironment.TerminalSpeed)
                mVelocity.X = -mEnvironment.TerminalSpeed;
            if (mVelocity.Y > mEnvironment.TerminalSpeed)
                mVelocity.Y = mEnvironment.TerminalSpeed;
            if (mVelocity.Y < -mEnvironment.TerminalSpeed)
                mVelocity.Y = -mEnvironment.TerminalSpeed;
        }

        /// <summary>
        /// Updates the bounding box around this object
        /// </summary>
        private void UpdateBoundingBoxes()
        {
            mBoundingBox = new Rectangle((int)mPosition.X, (int)mPosition.Y,
                (int)mSize.X, (int)mSize.Y);
        }

        /// <summary>
        /// Updates the velocity based on the force
        /// </summary>
        private void UpdateVelocities()
        {
            mVelocity = Vector2.Add(mVelocity, mEnvironment.GravityForce);
            mVelocity = Vector2.Add(mVelocity, mAdditionalForces);

            //Force erosion on the resistive forces(friction/wind resistance)
            ChangeGravityForceDirection(mEnvironment.GravityDirection);
            mVelocity = Vector2.Multiply(mVelocity,mResistiveForce);

            EnforceTerminalVelocity();
        }

        /// <summary>
        /// Update the physics object based on the given gametime
        /// </summary>
        /// <param name="gametime">Current gametime</param>
        public virtual void Update(GameTime gametime)
        {
            UpdateVelocities();
            mPosition = Vector2.Add(mPosition, mVelocity);
            UpdateBoundingBoxes();
        }

        /// <summary>
        /// Returns true if the physics objects are colliding with each other
        /// (only good for 2 boxes) (no circles yet)
        /// TODO - Add pixel perfect collision
        /// </summary>
        /// <param name="otherObject">The other object to test against</param>
        /// <returns>True if they are colliding with each other; False otherwise</returns>
        public virtual bool IsColliding(GameObject otherObject)
        {
            return !Equals(otherObject) && mBoundingBox.Intersects(otherObject.mBoundingBox);
        }


        /// <summary>
        /// Handles collision for two boxes (this, and other)
        /// </summary>
        /// <returns>nothing</returns>
        public virtual void HandleCollideBox(GameObject otherObject)
        {
            Vector2 colDepth = GetCollitionDepth(otherObject);

            if (Math.Abs(colDepth.X) > Math.Abs(colDepth.Y))
            {
                //Reset Y Velocity to 0
                mVelocity.Y = 0;
                // reduce x velocity for friction
                mVelocity.X *= otherObject.mFriction;
                // place the Y pos just so it is not colliding. 
                mPosition.Y += colDepth.Y;
            }
            else
            {
                //Reset X Velocity to 0
                mVelocity.X = 0;
                // reduce Y velocity for friction
                mVelocity.Y *= otherObject.mFriction;
                // place the X pos just so it is not colliding.
                mPosition.X += colDepth.X;
            }
            UpdateBoundingBoxes();
        }
        /// <summary>
        /// finds how deep they are intersecting (That is what she said!)
        /// </summary>
        /// <returns>vector decribing depth</returns>
        public Vector2 GetCollitionDepth(GameObject otherObject)
        {
            //Find Center
            int halfHeight1 = this.BoundingBox.Height / 2;
            int halfWidth1 = this.BoundingBox.Width / 2;

            //Calculate Center Position
            Vector2 center1 = new Vector2(this.BoundingBox.Left + halfWidth1, this.BoundingBox.Top + halfHeight1);
            
            //Find Center of otherObject
            int halfHeight2 = otherObject.BoundingBox.Height / 2;
            int halfWidth2 = otherObject.BoundingBox.Width / 2;

            //Calculate Center Position
            Vector2 center2 = new Vector2(otherObject.BoundingBox.Left + halfWidth2, otherObject.BoundingBox.Top + halfHeight2);
            
            //Center distances between both objects
            float distX = center1.X - center2.X;
            float distY = center1.Y - center2.Y;

            //Minimum distance 
            float minDistX = halfWidth1 + halfWidth2;
            float minDistY = halfHeight1 + halfHeight2;

            if (!IsColliding(otherObject))
            {
                return Vector2.Zero;
            }

            float depthX, depthY;
            if (distX > 0)
            {
                depthX = minDistX - distX;
            }
            else
            {
                depthX = -minDistX - distX;
            }
            if (distY > 0)
            {
                depthY = minDistY - distY;
            }
            else
            {
                depthY = -minDistY - distY;
            }

            return new Vector2(depthX, depthY);
        }

        public abstract void Kill();
        public abstract override string ToString();
    }
}