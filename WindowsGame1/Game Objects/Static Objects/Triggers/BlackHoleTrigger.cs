using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using GravityShift.Import_Code;
using Microsoft.Xna.Framework.Content;
using GravityShift.MISC_Code;

namespace GravityShift.Game_Objects.Static_Objects.Triggers
{
    class BlackHoleTrigger : Trigger
    {
        List<PhysicsObject> affectedObjects = new List<PhysicsObject>();
        AnimatedSprite blackHole = new AnimatedSprite();
        float mForce = 2;

        /// <summary>
        /// Constructs a trigger that is capable of acting like a black hole
        /// </summary>
        /// <param name="content">Content Manager to use</param>
        /// <param name="entity">Info on this entity</param>
        public BlackHoleTrigger(ContentManager content, EntityInfo entity)
            : base(content, entity)
        {
            if (entity.mProperties.ContainsKey(XmlKeys.FORCE))
                mForce = float.Parse(entity.mProperties[XmlKeys.FORCE]);
            blackHole.Load(content, "BlackHole",3, 6);
        }

        /// <summary>
        /// Draws this trigger on screen. Big Black hole :D
        /// </summary>
        /// <param name="canvas">Where we are drawing to</param>
        /// <param name="gametime">current game time</param>
        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch canvas, GameTime gametime)
        {
            blackHole.Update((float)gametime.ElapsedGameTime.Milliseconds/25);
            blackHole.Draw(canvas,Vector2.Subtract(mPosition, GridSpace.GetPixelCoord(new Vector2(1.5f,1.5f))));
        }

        /// <summary>
        /// Runs the black hole trigger
        /// </summary>
        /// <param name="objects">List of objects in the game</param>
        /// <param name="player">Player in the game</param>
        public override void RunTrigger(List<GameObject> objects, Player player)
        {
            foreach(GameObject gObj in objects)
            {
                if (!mBoundingBox.Intersects(gObj.BoundingBox)) continue;

                if (gObj is Player && player.mCurrentTexture == PlayerFaces.SMILE) 
                    player.mCurrentTexture = PlayerFaces.WORRY;

                if (gObj is PhysicsObject)
                {
                    PhysicsObject pObj = (PhysicsObject)gObj;

                    Vector2 posDiff = Vector2.Subtract(mPosition, pObj.mPosition);

                    //Gets the angle that the player is at
                    double degrees = 0;
                    if(posDiff.X > 0) degrees = Math.Atan(posDiff.Y/posDiff.X);
                    if(posDiff.X < 0 && posDiff.Y >= 0) degrees = Math.Atan(posDiff.Y/posDiff.X) + Math.PI;
                    if(posDiff.X < 0 && posDiff.Y < 0) degrees = Math.Atan(posDiff.Y/posDiff.X) - Math.PI;
                    if(posDiff.X == 0 && posDiff.Y > 0) degrees = Math.PI/2;
                    if(posDiff.X == 0 && posDiff.Y <0) degrees = - Math.PI/2;

                    //Distance of this trigger and pObj, squared
                    float distance = Vector2.DistanceSquared(GridSpace.GetGridCoord(pObj.mPosition), 
                                                                    GridSpace.GetGridCoord(mPosition))+1;

                    //Force on the object( G * M / r^2)
                    Vector2 newForce = new Vector2(mForce * (1 / pObj.Mass) / distance * (float)Math.Cos(degrees), 
                        mForce * (1 / pObj.Mass) / distance * (float)Math.Sin(degrees));
                   
                    //Imediately add this to the objects velocity so that we don't have lingering force additions left over
                    pObj.mVelocity = new Vector2(Math.Min(newForce.X + pObj.mVelocity.X, pObj.Environment.TerminalSpeed), 
                            Math.Min(newForce.Y + pObj.mVelocity.Y, pObj.Environment.TerminalSpeed));
                }
            }
        }
    }
}
