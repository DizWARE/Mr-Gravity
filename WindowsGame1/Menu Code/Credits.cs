using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using GravityShift.MISC_Code;

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

        Texture2D mSmile;
        Texture2D mGirlSmile;
        Texture2D mSad;
        Texture2D mGirlSad;

        Level mBackgroundLevel;

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

            mBackgroundLevel = Level.MainMenuLevel("Content\\Levels\\MainMenu.xml", mControls, mGraphics.GraphicsDevice.Viewport, mGraphics.GraphicsDevice.Viewport.Bounds);
            mBackgroundLevel.StartingPoint = new Vector2(mGraphics.GraphicsDevice.Viewport.Bounds.Center.X + mGraphics.GraphicsDevice.Viewport.Bounds.Center.X * .2f,
                                                        mGraphics.GraphicsDevice.Viewport.Bounds.Center.Y + mGraphics.GraphicsDevice.Viewport.Bounds.Center.Y * .1f);

            //Easily add names and titles here
            mTitles = new Dictionary<string, string[]>();
            mTitles.Add("Angry Newton Production", new string[] { "", "Angry" });
            mTitles.Add("Developed At", new string[] { "University Of Utah; Senior EAE Capstone Class" });
            mTitles.Add("Executive Producer", new string[]{"Roger Altizer", "Dr. Bob Kessler"});
            mTitles.Add("Original Concept & Design", new string[] { "Tyler Robinson" });
            mTitles.Add("Team Lead", new string[] { "Curtis Taylor" });
            mTitles.Add("-Programming-", new string[]{ "", "Surprise"});
            mTitles.Add("Lead Programmer", new string[] { "Tyler Robinson" });
            mTitles.Add("Programmers", new string[] { "Curtis Taylor", "Nate Bradford", "Jeremy Heintz", "Casey Spencer", "Kamron Egan", "Morgan Reynolds" });
            mTitles.Add("Xbox Tech", new string[] { "Kamron Egan" });
            mTitles.Add("-Art-", new string[] { "", "Laugh" });
            mTitles.Add("Lead Artist", new string[] { "Lukas Black" });
            mTitles.Add("Artists", new string[] { "Nate Bradford", "Jeremy Heintz", "Casey Spencer" });
            mTitles.Add("Animations", new string[] { "Lukas Black", "Nate Bradford", "Jeremy Heintz", "Kamron Egan" });
            mTitles.Add("-Design & Development-", new string[] { "", "Bored" } );
            mTitles.Add("Character Design", new string[] { "Lukas Black" });
            mTitles.Add("Level Design", new string[]{"Nate Bradford", "Curtis Taylor", "Jeremy Heintz", "Morgan Reynolds", "Steven Doxey", "Casey Spencer"});
            mTitles.Add("Game Mechanics", new string[] { "Tyler Robinson", "Curtis Taylor", "Morgan Reynolds" });
            mTitles.Add("-Music & Sound-", new string[] { "", "Worry" });
            mTitles.Add("Music Lead", new string[] { "Steven Doxey" });
            mTitles.Add("Music", new string[] { "Michelle MacArt", "Cuyler Stuwe" });
            mTitles.Add("Sound FX Lead", new string[] { "Steven Doxey" });
            mTitles.Add("Sound FX", new string[] { "Michelle MacArt", "Tyler Robinson", "Morgan Reynolds" });
            mTitles.Add("Find Us", new string[] { "", "Laugh2" });
            mTitles.Add("Our Official Website", new string[] { "TBD" });
            mTitles.Add("EAE Website", new string[] { "http://eae.utah.edu" });
            mTitles.Add("Like us on Facebook", new string[] { "http://www.facebook.com/", "Search Mr. Gravity" });
            mTitles.Add("Follow us on Twitter", new string[] { "http://www.twitter.com/AngryNewton", "Or @AngryNewton"});
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
            mSmile = content.Load<Texture2D>("Images\\Player\\Smile");
            mGirlSmile = content.Load<Texture2D>("Images\\Player\\GirlSmile");
            mSad = content.Load<Texture2D>("Images\\Player\\Sad");
            mGirlSad = content.Load<Texture2D>("Images\\Player\\GirlSad");

            mBackgroundLevel.Load(content);
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
            mTopY -= 1.25f;

            mBackgroundLevel.Update(gametime, ref states);
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

            spriteBatch.Draw(mSmile, new Rectangle(mGraphics.GraphicsDevice.Viewport.TitleSafeArea.Center.X - mTitle.Width / 4 - mSmile.Width, (int)mTopY - mSmile.Height / 2, mSmile.Width, mSmile.Height), Color.White);
            spriteBatch.Draw(mGirlSmile, new Rectangle(mGraphics.GraphicsDevice.Viewport.TitleSafeArea.Center.X + mTitle.Width / 4, (int)mTopY - mGirlSmile.Height / 2,mGirlSmile.Width, mGirlSmile.Height), Color.White);
            //Draw the game title before the words. Scrolls too
            spriteBatch.Draw(mTitle, new Rectangle(mGraphics.GraphicsDevice.Viewport.TitleSafeArea.Center.X - mTitle.Width / 4, (int)mTopY - mTitle.Height / 4,mTitle.Width/2,mTitle.Height/2),
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

                //If the first item under this key is empty, then this is an image block. 
                if (mTitles[key][0] == "")
                {
                    spriteBatch.Draw(PlayerFaces.FromString(mTitles[key][1]), new Rectangle(GetTextXLocation(key, true) - 3*mSad.Width/2, top, mSad.Width, mSad.Height), Color.White);
                    spriteBatch.Draw(PlayerFaces.FromString("Girl" + mTitles[key][1]), new Rectangle((int)(GetTextXLocation(key, true) + mFontBig.MeasureString(key).X) + mSad.Width/2, top, mSad.Width, mSad.Height), Color.White);
                    top += 100;
                    continue;
                }

                //Goes through all the titles under that header and draws it
                foreach (string name in mTitles[key])
                {
                    top += 40;
                    spriteBatch.DrawString(mFontSmall, name, new Vector2(GetTextXLocation(name, false), top), Color.White);
                }
                //Clear spacing between headers
                top += 100;
                
            }

            spriteBatch.Draw(mSad, new Rectangle(mGraphics.GraphicsDevice.Viewport.TitleSafeArea.Center.X - mSad.Width, top, mSad.Width, mSad.Height), Color.White);
            spriteBatch.Draw(mGirlSad, new Rectangle(mGraphics.GraphicsDevice.Viewport.TitleSafeArea.Center.X, top, mGirlSad.Width, mGirlSad.Height), Color.White);


            //If bottom has been reached, reset to the top
            if (top+mSad.Height <= mGraphics.GraphicsDevice.Viewport.TitleSafeArea.Top)
                mTopY = mGraphics.GraphicsDevice.Viewport.TitleSafeArea.Bottom;
            spriteBatch.End();
        }

        public void DrawLevel(SpriteBatch spriteBatch, GameTime gameTime, Matrix scale)
        {
            mBackgroundLevel.Draw(spriteBatch, gameTime, scale);
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
