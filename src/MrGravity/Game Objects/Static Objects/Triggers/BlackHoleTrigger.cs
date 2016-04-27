using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MrGravity.Game_Objects.Physics_Objects;
using MrGravity.Import_Code;
using MrGravity.MISC_Code;

namespace MrGravity.Game_Objects.Static_Objects.Triggers
{
    internal class BlackHoleTrigger : Trigger
    {
        private List<PhysicsObject> _affectedObjects = new List<PhysicsObject>();
        private readonly AnimatedSprite _blackHole = new AnimatedSprite();
        private readonly float _mForce = 2;

        /// <summary>
        /// Constructs a trigger that is capable of acting like a black hole
        /// </summary>
        /// <param name="content">Content Manager to use</param>
        /// <param name="entity">Info on this entity</param>
        public BlackHoleTrigger(ContentManager content, EntityInfo entity)
            : base(content, entity)
        {
            if (entity.MProperties.ContainsKey(XmlKeys.Force))
                _mForce = float.Parse(entity.MProperties[XmlKeys.Force]);
            _blackHole.Load(content, "BlackHole",3, 6);
        }

        /// <summary>
        /// Draws this trigger on screen. Big Black hole :D
        /// </summary>
        /// <param name="canvas">Where we are drawing to</param>
        /// <param name="gametime">current game time</param>
        public override void Draw(SpriteBatch canvas, GameTime gametime)
        {
            _blackHole.Update((float)gametime.ElapsedGameTime.Milliseconds/25);
            _blackHole.Draw(canvas,Vector2.Subtract(MPosition, GridSpace.GetPixelCoord(new Vector2(1.5f,1.5f))));
        }

        /// <summary>
        /// Runs the black hole trigger
        /// </summary>
        /// <param name="objects">List of objects in the game</param>
        /// <param name="player">Player in the game</param>
        public override void RunTrigger(List<GameObject> objects, Player player)
        {
            foreach(var gObj in objects)
            {
                if (!BoundingBox.Intersects(gObj.BoundingBox)) continue;

                if (gObj is Player && player.MCurrentTexture != PlayerFaces.FromString("Dead2")) 
                    player.MCurrentTexture = PlayerFaces.FromString("Worry");

                if (gObj is PhysicsObject)
                {
                    var pObj = (PhysicsObject)gObj;

                    Vector2 posDiff = Vector2.Subtract(MPosition, pObj.MPosition);

                    //Gets the angle that the player is at
                    double degrees = 0;
                    if(posDiff.X > 0) degrees = Math.Atan(posDiff.Y/posDiff.X);
                    if(posDiff.X < 0 && posDiff.Y >= 0) degrees = Math.Atan(posDiff.Y/posDiff.X) + Math.PI;
                    if(posDiff.X < 0 && posDiff.Y < 0) degrees = Math.Atan(posDiff.Y/posDiff.X) - Math.PI;
                    if(posDiff.X == 0 && posDiff.Y > 0) degrees = Math.PI/2;
                    if(posDiff.X == 0 && posDiff.Y <0) degrees = - Math.PI/2;

                    //Distance of this trigger and pObj, squared
                    var distance = Vector2.DistanceSquared(GridSpace.GetGridCoord(pObj.MPosition), 
                                                                    GridSpace.GetGridCoord(MPosition))+1;

                    //Force on the object( G * M / r^2)
                    var newForce = new Vector2(_mForce * (1 / pObj.Mass) / distance * (float)Math.Cos(degrees), 
                        _mForce * (1 / pObj.Mass) / distance * (float)Math.Sin(degrees));
                   
                    //Imediately add this to the objects velocity so that we don't have lingering force additions left over
                    pObj.Velocity = new Vector2(Math.Min(newForce.X + pObj.Velocity.X, pObj.Environment.TerminalSpeed), 
                            Math.Min(newForce.Y + pObj.Velocity.Y, pObj.Environment.TerminalSpeed));
                }
            }
        }
    }
}
