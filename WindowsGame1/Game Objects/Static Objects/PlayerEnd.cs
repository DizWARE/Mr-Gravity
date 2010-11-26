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
        public PlayerEnd(ContentManager content, Vector2 initialPosition)
            : base(content, "Images\\PlayerEnd", new Vector2(1,1), initialPosition, .8f,true)
        {
        }
    }
}
