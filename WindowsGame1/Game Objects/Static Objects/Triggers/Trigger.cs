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
        public Trigger(ContentManager content, String name, Vector2 initialPosition, bool isSquare, int width, int height)
            : base(content, name, initialPosition, .0f, isSquare, "Normal")
        {
            this.mSize.X = width;
            this.mSize.Y = height;

            this.mBoundingBox.Width = width;
            this.mBoundingBox.Height = height;
        }

        /// <summary>
        /// Ignores drawing triggers. They should be invisible anyway
        /// </summary>
        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch canvas, Microsoft.Xna.Framework.GameTime gametime)
        {
            return; //Don't draw :D
        }

        public abstract void RunTrigger(List<GameObject> objects, Player player);
    }
}
