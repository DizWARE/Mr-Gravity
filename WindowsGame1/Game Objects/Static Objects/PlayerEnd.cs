using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace GravityShift
{
    class PlayerEnd : StaticObject
    {
        public PlayerEnd(ContentManager content, string name, Vector2 initialPosition)
            : base(content, name, initialPosition, .8f,true, "Normal")
        {
        }
    }
}
