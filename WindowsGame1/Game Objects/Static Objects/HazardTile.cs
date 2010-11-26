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
    class HazardTile : StaticObject
    {
        public HazardTile(ContentManager content, String name,  Vector2 initialPosition, float friction, bool isSquare)
            : base(content, name, initialPosition, friction, isSquare, true)
        {

        }
    }
}
