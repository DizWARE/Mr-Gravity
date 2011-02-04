using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using System.Text;

namespace GravityShift
{
    //TODO:This will be used for our intial splash screen before the main menu
    class Title
    {

        private Texture2D mTitle;
        private SpriteFont mKootenay;

        public Title() 
        {
        }

        public void Load(ContentManager content)
        {
            mTitle = content.Load<Texture2D>("Images/Menu/Title");

            mKootenay = content.Load<SpriteFont>("Fonts/Kootenay");
        }




    }
}
