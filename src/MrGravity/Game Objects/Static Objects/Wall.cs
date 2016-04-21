using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MrGravity.MISC_Code;

namespace MrGravity.Game_Objects.Static_Objects
{
    /// <summary>
    /// 
    /// </summary>
    internal class Wall : StaticObject
    {
        public List<StaticObject> Walls { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Wall"/> class.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <param name="friction">The friction.</param>
        /// <param name="entity">The entity information from the importer</param>
        /// <param name="wallPieces">The wall pieces.</param>
        public Wall(float friction, List<StaticObject> walls)
        {
            Walls = new List<StaticObject>(walls);
            Id = walls[0].Id;

            MFriction = friction;
            MIsSquare = true;

            MCollisionType = walls[0].CollisionType;

            MPosition = walls[0].MPosition;
            MSize = Vector2.Subtract(Vector2.Add(walls[walls.Count - 1].MPosition,GridSpace.Size), MPosition);

            MBoundingBox = new Rectangle((int)MPosition.X, (int)MPosition.Y, (int)MSize.X, (int)MSize.Y);
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
        public override void Draw(SpriteBatch canvas, GameTime gametime)
        {
            foreach (var obj in Walls)
                obj.Draw(canvas, gametime);
        }

        public KeyValuePair<Vector2, string> NearestWallPosition(Vector2 position)
        {
            if (Walls.Count == 1)
                return new KeyValuePair<Vector2, string>(Walls[0].MPosition, Walls[0].MName);

            var currDist = float.MaxValue;
            var animation = new KeyValuePair<Vector2,string>();

            foreach (var obj in Walls)
                if (Vector2.Distance(position, obj.MPosition) < currDist)
                {
                    currDist = Vector2.Distance(position, obj.MPosition);
                    animation = new KeyValuePair<Vector2, string>(obj.MPosition, obj.MName);
                }
            return animation;
        }
    }
}
