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
    /// Just a proof of concept class. Used to show that many objects can be added and handled in the game
    /// </summary>
    class GenericObject : PhysicsObject
    {
        public GenericObject(ContentManager content, String name, Vector2 scalingFactors, Vector2 initialPosition, ref PhysicsEnvironment environment) : 
            base(content, name, scalingFactors, initialPosition, ref environment)
        {

        }

        public override void Kill()
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            throw new NotImplementedException();
        }
    }
}
