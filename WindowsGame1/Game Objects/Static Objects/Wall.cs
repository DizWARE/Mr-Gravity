using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using GravityShift.Import_Code;
using Microsoft.Xna.Framework;
using GravityShift.MISC_Code;

namespace GravityShift.Game_Objects.Static_Objects
{
    /// <summary>
    /// 
    /// </summary>
    class Wall : StaticObject
    {
        List<StaticObject> mWalls;
        public List<StaticObject> Walls
        {   get { return mWalls; }  }

        /// <summary>
        /// Initializes a new instance of the <see cref="Wall"/> class.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <param name="friction">The friction.</param>
        /// <param name="entity">The entity information from the importer</param>
        /// <param name="wallPieces">The wall pieces.</param>
        public Wall(float friction, List<StaticObject> walls):base()
        {
            mWalls = new List<StaticObject>(walls);
            ID = walls[0].ID;

            mFriction = friction;
            mIsSquare = true;

            mCollisionType = walls[0].CollisionType;

            mPosition = walls[0].mPosition;
            mSize = Vector2.Subtract(Vector2.Add(walls[walls.Count - 1].mPosition,GridSpace.SIZE), mPosition);

            mBoundingBox = new Rectangle((int)mPosition.X, (int)mPosition.Y, (int)mSize.X, (int)mSize.Y);
        }

        /// <summary>
        /// This will not do anything. overrides to avoid trying to load this class
        /// </summary>
        /// <param name="content">Content manager</param>
        /// <param name="name">Name of the object</param>
        public override void Load(ContentManager content, string name) { }

        /// <summary>
        /// Draws the object to the screen
        /// </summary>
        /// <param name="canvas">Canvas that the game is being drawn on</param>
        /// <param name="gametime">The current gametime</param>
        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch canvas, Microsoft.Xna.Framework.GameTime gametime)
        {
            foreach (StaticObject obj in mWalls)
                obj.Draw(canvas, gametime);
        }

        public KeyValuePair<Vector2, string> NearestWallPosition(Vector2 position)
        {
            if (mWalls.Count == 1)
                return new KeyValuePair<Vector2, string>(mWalls[0].mPosition, mWalls[0].mName);

            float currDist = float.MaxValue;
            KeyValuePair<Vector2, string> animation = new KeyValuePair<Vector2,string>();

            foreach (StaticObject obj in mWalls)
                if (Vector2.Distance(position, obj.mPosition) < currDist)
                {
                    currDist = Vector2.Distance(position, obj.mPosition);
                    animation = new KeyValuePair<Vector2, string>(obj.mPosition, obj.mName);
                }
            return animation;
        }
    }
}
