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
        public static Texture2D DIZZY;
        public static Texture2D DEAD;
        public static Texture2D DEAD2;
        public static Texture2D MEH;
        public static Texture2D SAD;
        public static Texture2D SAD2;
        public static Texture2D SKEPTIC;
        public static Texture2D SURPRISE;
        public static Texture2D WORRY;

        /// <summary>
        /// Loads all the faces from the content
        /// </summary>
        /// <param name="content">Content to load from</param>
        public static void Load(ContentManager content)
        {
            SMILE = content.Load<Texture2D>("Images/Player/Smile");
            LAUGH = content.Load<Texture2D>("Images/Player/Laugh");
            DIZZY = content.Load<Texture2D>("Images/Player/Dizzy");
            DEAD = content.Load<Texture2D>("Images/Player/Dead");
            DEAD2 = content.Load<Texture2D>("Images/Player/Dead2");
            MEH = content.Load<Texture2D>("Images/Player/NeonCharMeh");
            SAD = content.Load<Texture2D>("Images/Player/Sad");
            SAD2 = content.Load<Texture2D>("Images/Player/Sad2");
            SKEPTIC = content.Load<Texture2D>("Images/Player/NeonCharSkeptic");
            SURPRISE = content.Load<Texture2D>("Images/Player/Surprise");
            WORRY = content.Load<Texture2D>("Images/Player/Worry");
        }
        /// <summary>
        /// Given a name of one of the faces, returns the face texture
        /// </summary>
        /// <param name="faceName">Name of the face</param>
        /// <returns>Texture of that face</returns>
        public static Texture2D FromString(string faceName)
        {
            if(faceName.Equals(XmlKeys.Faces.Smile.ToString()))
                return SMILE;
            if(faceName.Equals(XmlKeys.Faces.Surprise.ToString()))
                return SURPRISE;
            if (faceName.Equals(XmlKeys.Faces.Laugh.ToString()))
                return LAUGH;
            if (faceName.Equals(XmlKeys.Faces.Dizzy.ToString()))
                return DIZZY;
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

            return SMILE;
        }

    }
}
