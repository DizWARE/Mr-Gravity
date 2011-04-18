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
        public static string FORCE = "Force";
        public static string RAIL = "Rail";
        public static string RAIL_X = "X";
        public static string RAIL_Y = "Y";
        public static string LENGTH = "Length";
        public static string REVERSE = "Reverse";
        public static string MUSIC_FILE = "MusicFile";
        public static string SOUND_FILE = "SoundFile";
        public static string LOOP = "Loop";
        public static string PLAYER_FACE = "PlayerFace";
        public static string IDEAL_TIME = "IdealTime";
        public static string POPUP_TYPE = "PopupType";
        public static string IMAGE_FILE = "ImageFile";
        public static string TEXT = "Text";
        
        //Names
        public static string WIDTH = "Width";
        public static string HEIGHT = "Height";
        public static string SHAPE = "Shape";

        //Specific Types
        public static string STATIC_OBJECT = "Static Objects";
        public static string PHYSICS_OBJECT = "Physics Objects";
        public static string PLAYER_LOCATION = "Level Positions";
        public static string TRIGGER = "Triggers";
        public static string WALLS = "Walls";

        //Values
        public static string TRUE = "True";
        public static string FALSE = "False";
        public static string CIRCLE = "Circle";
        public static string SQUARE = "Square";
        public static string COLLECTABLE = "Collectable";
        public static string HAZARDOUS = "Hazardous";
        public static string ERROR_TEXTURE = "Images/Error";
        public static string POPUP_IMAGE = "Image";
        public static string POPUP_TEXT = "Text";

        //Level Select Keys
        public static string LEVELS = "Levels";
        public static string LEVEL_DATA = "LevelData";
        public static string LEVEL_NAME = "Name";
        public static string UNLOCKED = "Unlocked";
        public static string TIMERSTAR = "Timerstar";
        public static string COLLECTIONSTAR = "Collectionstar";
        public static string DEATHSTAR = "Deathstar";
        public static string GOALTIME = "GoalTime";
        public static string GOALCOLLECTABLE = "GoalCollectable";


        //Smile Strings
        public enum Faces
        { Smile, Laugh, Dizzy, Dead, Dead2, Meh, Sad, Sad2, Skeptic, Surprise, Worry }
    }
}
