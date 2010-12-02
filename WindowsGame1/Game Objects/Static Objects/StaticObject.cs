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

namespace GravityShift
{
    /// <summary>
    /// This class represents objects that don't move
    /// </summary>
    abstract class StaticObject : GameObject
    {
        /// <summary>
        /// Constructs a new static object
        /// </summary>
        /// <param name="content">The games content manager</param>
        /// <param name="name">Name of the Object("Images/{Type}/{Name}"</param>
        /// <param name="initialPosition">Starting position</param>
        /// <param name="friction">Friction that reacts to physics objects</param>
        /// <param name="isSquare">True if the object should behave like a square</param>
        /// <param name="isHazardous">True if the object should kill the player when touched</param>
        public StaticObject(ContentManager content, String name, Vector2 initialPosition, float friction, bool isSquare, bool isHazardous)
            : base(content, name, initialPosition, friction,isSquare,isHazardous)
        {
            this.mVelocity = Vector2.Zero;
        }
    }
}
