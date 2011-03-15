using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace GravityShift
{
    class Credits
    {
        Dictionary<string, string[]> mTitles; 
        Texture2D mTitle;
        SpriteFont mFontBig;
        SpriteFont mFontSmall;
        Texture2D mBack;
        Texture2D mBackground;

        IControlScheme mControls;
        GraphicsDeviceManager mGraphics;

        private float mTopY;

        /// <summary>
        /// Constructor for the credits screen. Sets up the list of names and categories to scrolll up
        /// </summary>
        /// <param name="controlScheme">Player's method of control</param>
        /// <param name="graphics">Graphics Manager for the game</param>
        public Credits(IControlScheme controlScheme, GraphicsDeviceManager graphics)
        {
            mControls = controlScheme;
            mGraphics = graphics;
            mTopY = mGraphics.GraphicsDevice.Viewport.TitleSafeArea.Bottom;

            //Easily add names and titles here
            mTitles = new Dictionary<string, string[]>();
            mTitles.Add("Developed At", new string[] { "University Of Utah; Senior EAE Capstone Class" });
            mTitles.Add("Executive Producer", new string[]{"Roger Altizer", "Dr. Bob Kessler"});
            mTitles.Add("Scrum Master", new string[] { "Curtis Taylor" });
            mTitles.Add("Content Director", new string[] { "Steven Doxey" });
            mTitles.Add("Technical Director", new string[] { "Tyler Robinson" });
            mTitles.Add("Character & Environmental Artist", new string[] { "Lukas Black" });
            mTitles.Add("Music & Sound", new string[] { "Steven Doxey", "Cuyler Stuwe", "Michelle MacArt" });
            mTitles.Add("Graphics Team", new string[] { "Lukas Black", "Nate Bradford", "Jeremy Heintz", "Casey Spencer" });
            mTitles.Add("Technical Team", new string[] { "Morgan Reynolds", "Tyler Robinson", "Casey Spencer", "Curtis Taylor", "Kamron Egan", "Jeremy Heintz", "Nate Bradford" });
        }

        /// <summary>
        /// Loads the images and fonts needed for this screen
        /// </summary>
        /// <param name="content">Content manager this game is using</param>
        public void Load(ContentManager content)
        {
            mTitle = content.Load<Texture2D>("Images\\Menu\\Mr_Gravity");
            mBack = content.Load<Texture2D>("Images\\Menu\\Main\\BackSelected");
            mFontBig = content.Load<SpriteFont>("Fonts\\QuartzLarge");
            mFontSmall = content.Load<SpriteFont>("Fonts\\QuartzSmall");
            mBackground = content.Load<Texture2D>("Images\\Menu\\backgroundSquares1");
        }

        /// <summary>
        /// Update process of this screen. Moves the credits up one pixel
        /// </summary>
        /// <param name="gametime">Curent gametime(unnecessary)</param>
        /// <param name="states">State of the current game</param>
        public void Update(GameTime gametime, ref GameStates states)
        {
            if (mControls.isAPressed(false) || mControls.isStartPressed(false) || mControls.isBackPressed(false) || mControls.isBPressed(false))
            { mTopY = mGraphics.GraphicsDevice.Viewport.TitleSafeArea.Bottom; states = GameStates.Main_Menu; }

            mTopY -= 1.5f;
        }

        /// <summary>
        /// Draws the credits on screen
        /// </summary>
        /// <param name="gametime">Curent gametime(unnecessary)</param>
        /// <param name="spriteBatch">Canvas the screen is drawing on</param>
        /// <param name="scale">Not sure yet :)</param>
        public void Draw(GameTime gametime, SpriteBatch spriteBatch, Matrix scale)
        {
            spriteBatch.Begin(SpriteSortMode.Immediate,
                BlendState.AlphaBlend,
                SamplerState.LinearClamp,
                DepthStencilState.None,
                RasterizerState.CullCounterClockwise,
                null,
                scale);

            spriteBatch.Draw(mBackground, new Rectangle(0, 0, mGraphics.GraphicsDevice.Viewport.Width, mGraphics.GraphicsDevice.Viewport.Height), Color.White);

            float[] mSize = new float[2] { (float)mGraphics.GraphicsDevice.Viewport.TitleSafeArea.Width / (float)mGraphics.GraphicsDevice.Viewport.Width, (float)mGraphics.GraphicsDevice.Viewport.TitleSafeArea.Height / (float)mGraphics.GraphicsDevice.Viewport.Height };
            //Draws the back button. TODO - Better back button and probably better placement
            spriteBatch.Draw(mBack, new Rectangle(mGraphics.GraphicsDevice.Viewport.TitleSafeArea.Right - (int)(mBack.Width * mSize[0]) / 2,
                                                mGraphics.GraphicsDevice.Viewport.TitleSafeArea.Bottom - (int)(mBack.Height * mSize[1]) / 2, (int)(mBack.Width * mSize[0]) / 2, (int)(mBack.Height * mSize[1]) / 2), Color.White);

            //Draw the game title before the words. Scrolls too
            spriteBatch.Draw(mTitle, new Rectangle(mGraphics.GraphicsDevice.Viewport.TitleSafeArea.Center.X - mTitle.Width / 4, (int)mTopY,mTitle.Width/2,mTitle.Height/2),
                Color.White);

            //Make room for the game title
            int top = (int)mTopY+200;

            //Goes through all the headers
            foreach (string key in mTitles.Keys)
            {
                //Base and highlight for the header of the names under it
                spriteBatch.DrawString(mFontBig, key, new Vector2(GetTextXLocation(key, true), top), Color.White);
                spriteBatch.DrawString(mFontBig, key, 
                    Vector2.Add(new Vector2(GetTextXLocation(key, true), top), new Vector2(2,2)), Color.SteelBlue);
             
                //Goes through all the titles under that header and draws it
                foreach (string name in mTitles[key])
                {
                    top += 40;
                    spriteBatch.DrawString(mFontSmall, name, new Vector2(GetTextXLocation(name, false), top), Color.White);
                }

                //Clear spacing between headers
                top += 100;
            }

            //If bottom has been reached, reset to the top
            if (top == mGraphics.GraphicsDevice.Viewport.TitleSafeArea.Top)
                mTopY = mGraphics.GraphicsDevice.Viewport.TitleSafeArea.Bottom;
            spriteBatch.End();
        }

        /// <summary>
        /// Gets the x location of the text so that is exactly center for the string that is given
        /// </summary>
        /// <param name="text">The text to center</param>
        /// <param name="header">If it is a header(bigger text) or not</param>
        /// <returns></returns>
        private int GetTextXLocation(string text, bool header)
        {
            Vector2 size = mFontSmall.MeasureString(text);
            if(header)
                size = mFontBig.MeasureString(text);

            return mGraphics.GraphicsDevice.Viewport.TitleSafeArea.Center.X - (int)size.X / 2;
        }

    }
}
