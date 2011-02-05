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
using GravityShift.Import_Code;
using GravityShift.MISC_Code;

namespace GravityShift
{
    /// <summary>
    /// This represents an object that exists in the game
    /// </summary>
    abstract class GameObject
    {
        private static int ID_CREATER = 0;

        //Creates a unique identifier for every object
        public int ID = GameObject.ID_CREATER++;

        public Vector2 mPrevPos;
        public Vector2 mPosition;
        protected Vector2 mSize;

        protected string mCollisionType;
        public string CollisionType { get { return mCollisionType; } }

        private Vector2 mInitialPosition;

        protected EntityInfo mOriginalInfo;
        public EntityInfo OriginalInfo { get { return mOriginalInfo; } set { mOriginalInfo = value; } }

        protected Texture2D mTexture;
        public Rectangle mBoundingBox;
        public Vector2 mVelocity;

        private bool mBeingAnimated;
        private AnimatedSprite mAnimationTexture;

        protected bool mIsSquare;
        public bool IsSquare { get { return mIsSquare; } }

        public String mName;

        /// <summary>
        /// float that acts as a multiplier per frame 
        /// 0.0f = 100% friction
        /// 1.0f = 0% friction
        /// </summary>
        public float mFriction;
        // pixel perfect stuff
        public Color[] mSpriteImageData;

        /// <summary>
        /// Constructs a GameObject
        /// </summary>
        /// <param name="content">The games content manager</param>
        /// <param name="name">Name of the Object("Images/{Type}/{Name}"</param>
        /// <param name="initialPosition">Starting position</param>
        /// <param name="friction">Friction that reacts to physics objects</param>
        /// <param name="isSquare">True if the object should behave like a square</param>
        /// <param name="isHazardous">True if the object should kill the player when touched</param>
        public GameObject(ContentManager content, float friction, EntityInfo entity)
        {
            mName = "Images\\" + entity.mTextureFile;
            mFriction = friction;
            mIsSquare = !entity.mProperties.ContainsKey("Shape") || entity.mProperties["Shape"] == "Square";
            mCollisionType = entity.mCollisionType;
            mOriginalInfo = entity;

            Load(content, mName);

            ID = entity.mId;

            mPosition = GridSpace.GetDrawingCoord(entity.mLocation);
            mInitialPosition = mPosition;
            mSize = new Vector2(mTexture.Width, mTexture.Height);

            mBoundingBox = new Rectangle((int)mPosition.X, (int)mPosition.Y, (int)mSize.X, (int)mSize.Y);
        }


        /// <summary>
        /// Initializes an empty GameObject.
        /// </summary>
        public GameObject()
        {
        }

        /// <summary>
        /// Resets this object to its initial position
        /// </summary>
        public virtual void Respawn()
        {
            mPosition = mInitialPosition;
        }

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

            try
            {   mTexture = content.Load<Texture2D>(name);   }
            catch (Exception ex)
            {   mTexture = content.Load<Texture2D>("Images\\error");    }

            // pixel perfect stuff (may need to remove)
            mSpriteImageData = new Color[mTexture.Width * mTexture.Height];
            mTexture.GetData(mSpriteImageData);
            //////////////////////////////////////
        }

        /// <summary>
        /// Draws the object to the screen
        /// </summary>
        /// <param name="canvas">Canvas that the game is being drawn on</param>
        /// <param name="gametime">The current gametime</param>
        public virtual void Draw(SpriteBatch canvas, GameTime gametime)
        {
            canvas.Draw(mTexture, mBoundingBox, new Rectangle(0, 0, (int)mSize.X, (int)mSize.Y), Color.White);
        }
        
    }
}
