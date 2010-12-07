using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GravityShift.Import_Code
{
    class XmlKeys
    {
        //Specific entity Names
        public static string PLAYER_START = "PlayerStart";
        public static string PLAYER_END = "PlayerEnd";

        //Property info
        public static string TYPE = "Type";
        public static string NAME = "Name";
        public static string SIZE = "Size";
        public static string COLLISION_TYPE = "CollisionType";
        public static string BACKGROUND = "Background";
        public static string LOCATION = "Location";
        public static string VISIBLE = "Visible";
        public static string PROPERTIES = "Properties";
        public static string TEXTURE = "Texture";
        public static string ID = "ID";
        public static string MASS = "Mass";
        public static string XFORCE = "XForce";
        public static string YFORCE = "YForce";
        
        //Names
        public static string WIDTH = "Width";
        public static string HEIGHT = "Height";
        public static string SHAPE = "Shape";

        //Specific Types
        public static string STATIC_OBJECT = "Static Objects";
        public static string PHYSICS_OBJECT = "Physics Objects";
        public static string PLAYER_LOCATION = "Level Positions";
        public static string TRIGGER = "Triggers";

        //Values
        public static string TRUE = "True";
        public static string FALSE = "False";
        public static string CIRCLE = "Circle";
        public static string SQUARE = "Square";
        public static string COLLECTABLE = "Collectable";
        public static string HAZARDOUS = "Hazardous";
        public static string ERROR_TEXTURE = "Images/Error";
    }
}
