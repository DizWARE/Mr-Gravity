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
    class Tile : StaticObject
    {
        public Tile(ContentManager content, String name, Vector2 scalingFactors, float friction, bool isSquare, string collisionType)
            : base(content, name, scalingFactors, friction, isSquare, collisionType)
        {

        }
    }
}
