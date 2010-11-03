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
    abstract class GameObject
    {
        public static int ID_CREATER = 0;

        //Creates a unique identifier for every Physics object
        public int ID = PhysicsObject.ID_CREATER++;

        protected Vector2 mPosition;
        protected Vector2 mSize;

        protected Texture2D mTexture;
        protected Rectangle mBoundingBox;

        protected String mName;

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
            get { return mBoundingBox; }
            set { mBoundingBox = value; }
        }

        /// <summary>
        /// Checks to see if the other object is equal to this object
        /// </summary>
        /// <param name="obj">The other object</param>
        /// <returns>True if they are equal; false otherwise</returns>
        public override bool Equals(object obj)
        {
            if (obj is GameObject)
                return ObjectID == ((GameObject)obj).ObjectID;
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
        { 
            mTexture = content.Load<Texture2D>(name); 
        }

        /// <summary>
        /// Draws the physics object to the screen
        /// </summary>
        /// <param name="canvas">Canvas that the game is being drawn on</param>
        /// <param name="gametime">The current gametime</param>
        public virtual void Draw(SpriteBatch canvas, GameTime gametime)
        {
            canvas.Draw(mTexture, mBoundingBox, new Rectangle(0, 0, (int)mSize.X, (int)mSize.Y), Color.White);
        }
    }
}
