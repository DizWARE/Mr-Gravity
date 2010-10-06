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
    /// Represents an object that has rules based on physics
    /// </summary>
    abstract class PhysicsObject
    {
        public static int ID_CREATER = 0;

        //Creates a unique identifier for every Physics object
        public int ID = PhysicsObject.ID_CREATER++;

        protected PhysicsEnvironment mEnvironment;
        
        //All forces applied to this physicsObject
        private Vector2 mGravityForce = new Vector2(0,0);
        private Vector2 mResistiveForce = new Vector2(1,1);
        private Vector2 mAdditionalForces = new Vector2(0, 0);

        private Vector2 mVelocity = new Vector2(0, 0);
        private Vector2 mPosition;
        private Vector2 mSize;

        private Texture2D mTexture;
        private Rectangle mBoundingBox;

        private String mName;

        /// <summary>
        /// Gets the unique identifier for this object
        /// </summary>
        public int ObjectID
        {
            get { return ID; }
        }

        /// <summary>
        /// Gets the bounding box of this object
        /// </summary>
        public Rectangle BoundingBox
        {
            get { return mBoundingBox;  }
            set { mBoundingBox = value; }
        }

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
        public PhysicsObject(ContentManager content, String name, Vector2 scalingFactors, Vector2 initialPosition, ref PhysicsEnvironment environment)
        {
            mName = name;
            Load(content, name);
            mEnvironment = environment;

            mPosition = initialPosition;
            mSize = new Vector2(mTexture.Width * scalingFactors.X, mTexture.Height * scalingFactors.Y);

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
        public void fixForBounds(int width, int height)
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
            UpdateBoundingBoxes();
            mPosition = Vector2.Add(mPosition, mVelocity);
        }

        /// <summary>
        /// Returns true if the physics objects are colliding with each other
        /// 
        /// TODO - Add pixel perfect collision
        /// </summary>
        /// <param name="otherObject">The other object to test against</param>
        /// <returns>True if they are colliding with each other; False otherwise</returns>
        public virtual bool IsColliding(PhysicsObject otherObject)
        {
            return !Equals(otherObject) && mBoundingBox.Intersects(otherObject.mBoundingBox);
        }

        /// <summary>
        /// Checks to see if the other object is equal to this object
        /// </summary>
        /// <param name="obj">The other object</param>
        /// <returns>True if they are equal; false otherwise</returns>
        public override bool Equals(object obj)
        {
            if(obj is PhysicsObject)
                return ObjectID == ((PhysicsObject)obj).ObjectID;
            return false;
        }

        /// <summary>
        /// Returns the Unique Object ID for this object. This should map the object in a "perfect" hashed, hash table
        /// </summary>
        /// <returns>The unique Object ID</returns>
        public override int GetHashCode()
        {
            return ObjectID;
        }

        /// <summary>
        /// Loads the visual representation of this character 
        /// </summary>
        /// <param name="content"></param>
        /// <param name="name"></param>
        public virtual void Load(ContentManager content, String name)
        {   mTexture = content.Load<Texture2D>(name);   }

        /// <summary>
        /// Draws the physics object to the screen
        /// </summary>
        /// <param name="canvas">Canvas that the game is being drawn on</param>
        /// <param name="gametime">The current gametime</param>
        public virtual void Draw(SpriteBatch canvas, GameTime gametime)
        {
            canvas.Draw(mTexture, mBoundingBox, new Rectangle(0, 0, (int)mSize.X, (int)mSize.Y), Color.White);
        }

        public abstract void Kill();
        public abstract override string ToString();
    }
}