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
using GravityShift.MISC_Code;
using GravityShift.Import_Code;

namespace GravityShift.Game_Objects.Static_Objects.Triggers
{
    abstract class Trigger : StaticObject
    {
        /// <summary>
        /// Construcsts a new trigger
        /// 
        /// See Static object for parameter defs
        /// </summary>
        /// <param name="width">Width of the trigger activation field</param>
        /// <param name="height">Height of the trigger activation field</param>
        public Trigger(ContentManager content, EntityInfo entity)
            : base(content, .0f, entity)
        {
            this.mSize = new Vector2(3, 3);
            if(entity.mProperties.ContainsKey(XmlKeys.WIDTH)) this.mSize.X = int.Parse(entity.mProperties[XmlKeys.WIDTH]);
            if (entity.mProperties.ContainsKey(XmlKeys.HEIGHT)) this.mSize.Y = int.Parse(entity.mProperties[XmlKeys.HEIGHT]);

            mSize = GridSpace.GetDrawingCoord(mSize);
            this.mBoundingBox.X -= (int)mSize.X / 2;
            this.mBoundingBox.Y -= (int)mSize.Y / 2;
            this.mBoundingBox.Width = (int)mSize.X;
            this.mBoundingBox.Height = (int)mSize.Y;
        }

        /// <summary>
        /// Ignores drawing triggers. They should be invisible anyway
        /// </summary>
        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch canvas, Microsoft.Xna.Framework.GameTime gametime)
        {
            return; //Don't draw :D
        }

        /// <summary>
        /// Runs whatever the trigger should do.
        /// </summary>
        /// <param name="objects">List of all the objects in the level</param>
        /// <param name="player">The player in the game</param>
        public abstract void RunTrigger(List<GameObject> objects, Player player);
    }
}
