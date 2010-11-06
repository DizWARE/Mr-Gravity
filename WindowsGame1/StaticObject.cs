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
    abstract class StaticObject: GameObject
    {
        public StaticObject(ContentManager content, String name, Vector2 scalingFactors, Vector2 initialPosition, float friction)
            : base(content, name, scalingFactors, initialPosition, friction)
        {
        }
    }
}
