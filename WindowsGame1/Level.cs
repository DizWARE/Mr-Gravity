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

namespace GravityShift
{
    /// <summary>
    /// MAJOR TODO - This class needs to be completely retooled. There is a major problem with a lot of the decisions I made here.
    /// It was only kept for the use of the demonstration
    /// </summary>
    class Level
    {
        /// <summary>
        /// Gets the background texture of this level
        /// </summary>
        public Texture2D Texture { get { return mTexture; } }
        private Texture2D mTexture;

        /// <summary>
        /// Gets or sets the name of this level
        /// </summary>
        public string Name { get { return mName; } set { mName = value; } }
        private string mName;

        /// <summary>
        /// Gets or sets the size of the level(in pixels)
        /// </summary>
        public Vector2 Size { get { return mSize; } set { mSize = value; } }
        private Vector2 mSize;
        
        /// <summary>
        /// Gets or sets the player starting point of this level(in pixels)
        /// </summary>
        public Vector2 StartingPoint { get { return mStartingPoint; } set { mStartingPoint = value; } }
        private Vector2 mStartingPoint;
        
        /// <summary>
        /// Gets or sets the player ending point of this level(in pixels)
        /// </summary>
        public Vector2 EndingPoint { get { return mEndingPoint; } set { mEndingPoint = value; } }
        private Vector2 mEndingPoint;


        /// <summary>
        /// Constructs a new level for the game.
        /// 
        /// TODO - Figure out if this will be used
        /// </summary>
        public Level()
        {
        }

        /// <summary>
        /// Loads the level from the content manager
        /// </summary>
        /// <param name="content">Content Manager to load from</param>
        public void Load(ContentManager content, string assetName)
        {
            mTexture = content.Load<Texture2D>("Images\\" + assetName);//assetName);
        }

        /// <summary>
        /// Draws the level background on to the screen
        /// </summary>
        /// <param name="spriteBatch">Sprite batch that we use to draw textures</param>
        public void Draw(SpriteBatch spriteBatch)
        {           
            spriteBatch.Draw(mTexture, new Rectangle(0,0,(int)mSize.X,(int)mSize.Y), Color.White);
        }
    }
}
