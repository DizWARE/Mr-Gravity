using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MrGravity.Import_Code;
using MrGravity.MISC_Code;

namespace MrGravity.Game_Objects
{
    /// <summary>
    /// This represents an object that exists in the game
    /// </summary>
    internal abstract class GameObject
    {
        private static int _idCreater;

        //Creates a unique identifier for every object
        public int Id = _idCreater++;

        public Vector2 MPrevPos;
        public Vector2 MPosition;
        protected Vector2 MSize;

        protected string MCollisionType;
        public string CollisionType => MCollisionType;

        private readonly Vector2 _mInitialPosition;

        protected EntityInfo MOriginalInfo;
        public EntityInfo OriginalInfo { get { return MOriginalInfo; } set { MOriginalInfo = value; } }

        protected Texture2D MTexture;
        public Rectangle MBoundingBox;
        public Vector2 MVelocity;

        //private bool mBeingAnimated;
        //private AnimatedSprite mAnimationTexture;

        protected bool MIsSquare;
        public bool IsSquare => MIsSquare;

        public String MName;

        /// <summary>
        /// float that acts as a multiplier per frame 
        /// 0.0f = 100% friction
        /// 1.0f = 0% friction
        /// </summary>
        public float MFriction;
        // pixel perfect stuff
        public Color[] MSpriteImageData;

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
            MName = "Images\\" + entity.MTextureFile;
            MFriction = friction;
            MIsSquare = !entity.MProperties.ContainsKey("Shape") || entity.MProperties["Shape"] == "Square";
            MCollisionType = entity.MCollisionType;
            MOriginalInfo = entity;

            Load(content, MName);

            Id = entity.MId;

            MPosition = GridSpace.GetDrawingCoord(entity.MLocation);
            _mInitialPosition = MPosition;
            MSize = new Vector2(MTexture.Width, MTexture.Height);

            MBoundingBox = new Rectangle((int)MPosition.X, (int)MPosition.Y, (int)MSize.X, (int)MSize.Y);
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
            MPosition = _mInitialPosition;
        }

        /// <summary>
        /// Gets the unique identifier for this object
        /// </summary>
        public int ObjectId => Id;

        /// <summary>
        /// Gets the bounding box of this object
        /// </summary>
        public Rectangle BoundingBox
        {
            get { return MBoundingBox; }
            set { MBoundingBox = value; }
        }

        /// <summary>
        /// Checks to see if the other object is equal to this object
        /// </summary>
        /// <param name="obj">The other object</param>
        /// <returns>True if they are equal; false otherwise</returns>
        public override bool Equals(object obj)
        {
            if (obj is GameObject)
                return ObjectId == ((GameObject)obj).ObjectId;
            return false;
        }

        /// <summary>
        /// Returns the Unique Object ID for this object. This should map the object in a "perfect" hashed, hash table
        /// </summary>
        /// <returns>The unique Object ID</returns>
        public override int GetHashCode()
        {
            return ObjectId;
        }
        /// <summary>
        /// Loads the visual representation of this character 
        /// </summary>
        /// <param name="content"></param>
        /// <param name="name"></param>
        public virtual void Load(ContentManager content, String name)
        {

            try
            {   MTexture = content.Load<Texture2D>(name);   }
            catch (Exception ex)
            { var err = ex.ToString(); MTexture = content.Load<Texture2D>("Images\\error"); }

            // pixel perfect stuff (may need to remove)
            MSpriteImageData = new Color[MTexture.Width * MTexture.Height];
            MTexture.GetData(MSpriteImageData);
            //////////////////////////////////////
        }

        /// <summary>
        /// Draws the object to the screen
        /// </summary>
        /// <param name="canvas">Canvas that the game is being drawn on</param>
        /// <param name="gametime">The current gametime</param>
        public virtual void Draw(SpriteBatch canvas, GameTime gametime)
        {
            canvas.Draw(MTexture, MBoundingBox, new Rectangle(0, 0, (int)MSize.X, (int)MSize.Y), Color.White);
        }
        
    }
}
