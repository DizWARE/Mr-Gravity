using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace GravityShift
{
    /// <summary>
    /// Color codes
    /// </summary>
    class ColorCodes
    {
        public static Color UNWALKABLE = new Color(0,0,0);
        public static Color WALKABLE = new Color(255, 255, 255);
        public static Color DEADLY = new Color(255, 0, 0);
        public static Color STARTING_POINT = new Color(0, 0, 255);
        public static Color END_POINT = new Color(0, 255, 0);
    }
}
