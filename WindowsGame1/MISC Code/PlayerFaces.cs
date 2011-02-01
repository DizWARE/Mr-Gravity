using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using GravityShift.Import_Code;

namespace GravityShift.MISC_Code
{
    public class PlayerFaces
    {
        public static Texture2D SMILE;
        public static Texture2D LAUGH;
        public static Texture2D DAZED;
        public static Texture2D DEAD;
        public static Texture2D DEAD2;
        public static Texture2D MEH;
        public static Texture2D SAD;
        public static Texture2D SAD2;
        public static Texture2D SKEPTIC;
        public static Texture2D SURPRISE;
        public static Texture2D WORRY;
        public static Texture2D BLANK;
        public static Texture2D GRID;

        /// <summary>
        /// Loads all the faces from the content
        /// </summary>
        /// <param name="content">Content to load from</param>
        public static void Load(ContentManager content)
        {
            SMILE = content.Load<Texture2D>("Images/Player/NeonCharSmile");
            LAUGH = content.Load<Texture2D>("Images/Player/NeonCharLaugh");
            DAZED = content.Load<Texture2D>("Images/Player/NeonCharDazed");
            DEAD = content.Load<Texture2D>("Images/Player/NeonCharDead");
            DEAD2 = content.Load<Texture2D>("Images/Player/NeonCharDead2");
            MEH = content.Load<Texture2D>("Images/Player/NeonCharMeh");
            SAD = content.Load<Texture2D>("Images/Player/NeonCharSad");
            SAD2 = content.Load<Texture2D>("Images/Player/NeonCharSad2");
            SKEPTIC = content.Load<Texture2D>("Images/Player/NeonCharSkeptic");
            SURPRISE = content.Load<Texture2D>("Images/Player/NeonCharSurprise");
            WORRY = content.Load<Texture2D>("Images/Player/NeonCharWorry");
            BLANK = content.Load<Texture2D>("Images/Player/NeonCharBlank");
            GRID = content.Load<Texture2D>("Images/Player/NeonCharGrid");
        }

        public static Texture2D FromString(string faceName)
        {
            if(faceName.Equals(XmlKeys.Faces.Smile.ToString()))
                return SMILE;
            if (faceName.Equals(XmlKeys.Faces.Laugh.ToString()))
                return LAUGH;
            if (faceName.Equals(XmlKeys.Faces.Dazed.ToString()))
                return DAZED;
            if (faceName.Equals(XmlKeys.Faces.Dead.ToString()))
                return DEAD;
            if (faceName.Equals(XmlKeys.Faces.Dead2.ToString()))
                return DEAD2;
            if (faceName.Equals(XmlKeys.Faces.Meh.ToString()))
                return MEH;
            if (faceName.Equals(XmlKeys.Faces.Sad.ToString()))
                return SAD;
            if (faceName.Equals(XmlKeys.Faces.Sad2.ToString()))
                return SAD2;
            if (faceName.Equals(XmlKeys.Faces.Skeptic.ToString()))
                return SKEPTIC;
            if (faceName.Equals(XmlKeys.Faces.Worry.ToString()))
                return WORRY;
            if (faceName.Equals(XmlKeys.Faces.Blank.ToString()))
                return BLANK;
            if (faceName.Equals(XmlKeys.Faces.Grid.ToString()))
                return GRID;

            return SMILE;
        }

    }
}
