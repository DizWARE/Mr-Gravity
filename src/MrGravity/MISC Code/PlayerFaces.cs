using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MrGravity.MISC_Code
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
        private static Dictionary<String, Texture2D> _mFaces;

        /// <summary>
        /// Loads all the faces from the content
        /// </summary>
        /// <param name="content">Content to load from</param>
        public static void Load(ContentManager content)
        {
            _mFaces = new Dictionary<string, Texture2D>();

            var directory = new DirectoryInfo("Content/Images/Player");
            foreach (var file in directory.GetFiles())
            {
                var name = file.Name.Substring(0,file.Name.IndexOf('.'));
                _mFaces.Add(name, content.Load<Texture2D>("Images/Player/" + name));
            }
        }
        /// <summary>
        /// Given a name of one of the faces, returns the face texture
        /// </summary>
        /// <param name="faceName">Name of the face</param>
        /// <returns>Texture of that face</returns>
        public static Texture2D FromString(string faceName)
        {
            return _mFaces[faceName];
        }
    }
}
