using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using GravityShift.Import_Code;

namespace GravityShift
{
    class PlayerEnd : StaticObject
    {
        public PlayerEnd(ContentManager content,EntityInfo entity)
            : base(content, .8f, entity)
        {
        }
    }
}
