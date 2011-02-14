using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using GravityShift.Import_Code;
using System.IO;

namespace GravityShift.MISC_Code
{
    public class PlayerFaces
    {
        /// <summary>
        /// List of current faces
        /// 
        /// ANGRY
        /// ANGRY2
        /// BORED
        /// DEAD 
        /// DEAD2 
        /// DIZZY 
        /// DIZZY2
        /// LAUGH 
        /// LAUGH2
        /// SAD 
        /// SAD2 
        /// SMILE 
        /// SURPRISE 
        /// WORRY 
        /// </summary>     
        private static Dictionary<String, Texture2D> mFaces;

        /// <summary>
        /// Loads all the faces from the content
        /// </summary>
        /// <param name="content">Content to load from</param>
        public static void Load(ContentManager content)
        {
            mFaces = new Dictionary<string, Texture2D>();

            DirectoryInfo directory = new DirectoryInfo("Content/Images/Player");
            foreach (FileInfo file in directory.GetFiles())
            {
                string name = file.Name.Substring(0,file.Name.IndexOf('.'));
                mFaces.Add(name, content.Load<Texture2D>("Images/Player/" + name));
            }
        }
        /// <summary>
        /// Given a name of one of the faces, returns the face texture
        /// </summary>
        /// <param name="faceName">Name of the face</param>
        /// <returns>Texture of that face</returns>
        public static Texture2D FromString(string faceName)
        {
            return mFaces[faceName];
        }

    }
}
