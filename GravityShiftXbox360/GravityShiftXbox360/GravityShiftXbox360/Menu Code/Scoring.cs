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
    class Scoring
    {
        #region Member Variables

        /* Spritefont */
        private SpriteFont mKootenay;
        private SpriteFont mQuartz;

        private Rectangle mScreenRect;

        ContentManager mContent;

        IControlScheme mControls;

        /* Keep track of the level */
        private static int[,] mLevel;
        private static int[,] mScore;

        #endregion

        #region Art

        private Texture2D mTitle;
        private Texture2D mBackground;
        private Texture2D mStar;

        #endregion

        #region Getters and Setters

        /* Getter/Setter for the apples variable */
//        public static int[] Apples
//        {
//            get { return num_apples; }
//            set { num_apples = value; }
//        }

        /* Getter/Setter for the level variable */
        public static int[,] Level
        {
            get { return mLevel; }
            set { mLevel = value; }
        }

        /* Getter/Setter for the score variable */
        public static int[,] Score
        {
            get { return mScore; }
            set { mScore = value; }
        }

        #endregion

        public Scoring(IControlScheme controls) 
        {
            mControls = controls;
        }

        /*
         * Load
         *
         * Similar to a loadContent function. This function loads and 
         * initializes the variable and art used in the class.
         *
         * ContentManager content: the Content file used in the game.
         */
        public void Load(ContentManager content, GraphicsDevice graphics)
        {
            mContent = content;
            mKootenay = content.Load<SpriteFont>("Fonts/Kootenay");
            mQuartz = content.Load<SpriteFont>("Fonts/QuartzLarge");

            mScreenRect = graphics.Viewport.TitleSafeArea;

            mTitle = content.Load<Texture2D>("Images/Menu/Mr_Gravity");
            mBackground = content.Load<Texture2D>("Images/Menu/backgroundSquares1");
            mStar = content.Load<Texture2D>("Images/AnimatedSprites/YellowStar");
        }

        /*
         * Update
         *
         * Updates the menu depending on what the user has selected.
         * It will handle the title, options, load and all other menu 
         * screens
         *
         * GameTime gameTime: The current game time variable
         */
        public void Update(GameTime gameTime, ref GameStates gameState, ref Level level)
        {

            if (mControls.isStartPressed(false))
            {
                gameState = GameStates.AfterScore;
//#if XBOX360
//                mSelect.Save(((ControllerControl)mControls).ControllerIndex);
//#else
//                mSelect.Save(PlayerIndex.One);
//#endif
            }
            else if (mControls.isAPressed(false))
            {
                level.mTimer = 0;

                gameState = GameStates.Next_Level;
//#if XBOX360
//                mSelect.Save(((ControllerControl)mControls).ControllerIndex);
//#else
//                mSelect.Save(PlayerIndex.One);
//#endif
            }
            
        }
        /* GetRank
         * 
         * int time: time to complete level
         * 
         * int timeGoal: goal time
         * 
         * int collect: collectables received
         * 
         * int collectGoal: Total collectables in level
         * 
         * int deathTotal: number of deaths
         * 
         * return int[] (number of stars-- 0=Bad, 1=Okay, 2=Good, 3=Perfect): [Time, Collectables, Death]
         
        public int[] GetRank(int time, int timeGoal, int collect, int collectGoal, int deathTotal)
        {
            int[] result = new int[3];

            // TIME -- 100%+, <120%, <140%, >140% 
            if (time < timeGoal) 
            { result[0] = 3; }
            else if (((double) time / (double) timeGoal) < 1.2) { result[0] = 2; }
            else { result[0] = 1; }

            // COLLECTABLES -- 100%, >80%, >60%, <60% 
            if (collect == collectGoal) { result[1] = 3; }
            else if (((double) collect / (double) collectGoal) > 0.8) { result[1] = 2; }
            else { result[1] = 1; }

            // DEATHS -- 0, 1, 2-3, >3 //
            if (deathTotal == 0) { result[2] = 3; }
            else if (deathTotal >= 2) { result[2] = 2; }
            else { result[2] = 1; }

            return result;
        }*/

        /*
         * Draw
         *
         * This function will draw the current menu
         *
         * SpriteBatch spriteBatch: The current sprite batch used to draw
         * 
         * GraphicsDeviceManager graphics: The current graphics manager
         */
        public void Draw(SpriteBatch spriteBatch, GraphicsDeviceManager graphics, Level level, Matrix scale)
        {
            spriteBatch.Begin(SpriteSortMode.Immediate,
                BlendState.AlphaBlend,
                SamplerState.LinearClamp,
                DepthStencilState.None,
                RasterizerState.CullCounterClockwise,
                null,
                scale);

            // TODO - CHANGE TO DYNAMIC PLACEMENT

            float textLength = mQuartz.MeasureString("Collected:").Length();
            float bufferLength = mQuartz.MeasureString("999/999  ").Length();

            int topPadding = mScreenRect.Top + mScreenRect.Height / 4;
            int[] attempt = {level.TimerStar, level.CollectionStar, level.DeathStar};
            //attempt = GetRank((int)level.mTimer, (int)level.IdealTime, (int)GravityShift.Level.mNumCollected, (int)GravityShift.Level.mNumCollectable, (int)GravityShift.Level.mDeaths);
            spriteBatch.Draw(mBackground, new Rectangle(0, 0, graphics.GraphicsDevice.Viewport.Width, graphics.GraphicsDevice.Viewport.Height), Color.White);
            spriteBatch.Draw(mTitle, new Vector2(mScreenRect.Left + (mScreenRect.Width - mTitle.Width) / 2, mScreenRect.Top), Color.White);

            spriteBatch.DrawString(mQuartz, "Time:", new Vector2(mScreenRect.Left + (mScreenRect.Width / 6), topPadding), Color.White);
            spriteBatch.DrawString(mQuartz, "Time:", new Vector2(mScreenRect.Left + (mScreenRect.Width / 6) + 1, topPadding + 1), Color.SteelBlue);
            spriteBatch.DrawString(mQuartz, (int)level.mTimer + " Sec", new Vector2(mScreenRect.Left + (mScreenRect.Width / 6) + textLength + 20, topPadding), Color.White);

            //Draw Stars
            if (attempt[0] >= 1)
                spriteBatch.Draw(mStar, new Vector2(mScreenRect.Left + (mScreenRect.Width / 6) + textLength + bufferLength, topPadding), null, Color.White, 0.0f, Vector2.Zero, 0.75f, SpriteEffects.None, 0.0f);
            if (attempt[0] >= 2)
                spriteBatch.Draw(mStar, new Vector2(mScreenRect.Left + (mScreenRect.Width / 6) + textLength + bufferLength + (0.5f * (mStar.Width)), topPadding), null, Color.White, 0.0f, Vector2.Zero, 0.75f, SpriteEffects.None, 0.0f);
            if (attempt[0] == 3)
                spriteBatch.Draw(mStar, new Vector2(mScreenRect.Left + (mScreenRect.Width / 6) + textLength + bufferLength + (0.5f * (2 * mStar.Width)), topPadding), null, Color.White, 0.0f, Vector2.Zero, 0.75f, SpriteEffects.None, 0.0f);

            level.TimerStar = attempt[0];

            if (attempt[0] == 3)
            {
                spriteBatch.DrawString(mQuartz, "Perfect!", new Vector2(mScreenRect.Left + (4 * mScreenRect.Width / 6), topPadding), Color.White);
                spriteBatch.DrawString(mQuartz, "Perfect!", new Vector2(mScreenRect.Left + (4 * mScreenRect.Width / 6) + 1, topPadding + 1), Color.SteelBlue);
            }
            
            topPadding += 65;

            spriteBatch.DrawString(mQuartz, "Collected:", new Vector2(mScreenRect.Left + (mScreenRect.Width / 6), topPadding), Color.White);
            spriteBatch.DrawString(mQuartz, "Collected:", new Vector2(mScreenRect.Left + (mScreenRect.Width / 6) + 1, topPadding + 1), Color.SteelBlue);
            spriteBatch.DrawString(mQuartz, (int)GravityShift.Level.mNumCollected + " / " + GravityShift.Level.mNumCollectable, new Vector2(mScreenRect.Left + (mScreenRect.Width / 6) + textLength + 20, topPadding), Color.White);

            if (attempt[1] >= 1)
                spriteBatch.Draw(mStar, new Vector2(mScreenRect.Left + (mScreenRect.Width / 6) + textLength + bufferLength, topPadding), null, Color.White, 0.0f, Vector2.Zero, 0.75f, SpriteEffects.None, 0.0f);
            if (attempt[1] >= 2)
                spriteBatch.Draw(mStar, new Vector2(mScreenRect.Left + (mScreenRect.Width / 6) + textLength + bufferLength + (0.5f * (mStar.Width)), topPadding), null, Color.White, 0.0f, Vector2.Zero, 0.75f, SpriteEffects.None, 0.0f);
            if (attempt[1] == 3)
                spriteBatch.Draw(mStar, new Vector2(mScreenRect.Left + (mScreenRect.Width / 6) + textLength + bufferLength + (0.5f * (2 * mStar.Width)), topPadding), null, Color.White, 0.0f, Vector2.Zero, 0.75f, SpriteEffects.None, 0.0f);

            level.CollectionStar = attempt[1];
            if (attempt[1] == 3)
            {
                spriteBatch.DrawString(mQuartz, "Perfect!", new Vector2(mScreenRect.Left + (4 * mScreenRect.Width / 6), topPadding), Color.White);
                spriteBatch.DrawString(mQuartz, "Perfect!", new Vector2(mScreenRect.Left + (4 * mScreenRect.Width / 6) + 1, topPadding + 1), Color.SteelBlue);
            }       
            
            topPadding += 65;

            spriteBatch.DrawString(mQuartz, "Deaths:", new Vector2(mScreenRect.Left + (mScreenRect.Width / 6), topPadding), Color.White);
            spriteBatch.DrawString(mQuartz, "Deaths:", new Vector2(mScreenRect.Left + (mScreenRect.Width / 6) + 1, topPadding + 1), Color.SteelBlue);
            spriteBatch.DrawString(mQuartz, "" + (int)GravityShift.Level.mDeaths, new Vector2(mScreenRect.Left + (mScreenRect.Width / 6) + textLength + 20, topPadding), Color.White);
           
            //Draw Stars
            if (attempt[2] >= 1)
                spriteBatch.Draw(mStar, new Vector2(mScreenRect.Left + (mScreenRect.Width / 6) + textLength + bufferLength, topPadding), null, Color.White, 0.0f, Vector2.Zero, 0.75f, SpriteEffects.None, 0.0f);
            if (attempt[2] >= 2)
                spriteBatch.Draw(mStar, new Vector2(mScreenRect.Left + (mScreenRect.Width / 6) + textLength + bufferLength + (0.5f * (mStar.Width)), topPadding), null, Color.White, 0.0f, Vector2.Zero, 0.75f, SpriteEffects.None, 0.0f);
            if (attempt[2] == 3)
                spriteBatch.Draw(mStar, new Vector2(mScreenRect.Left + (mScreenRect.Width / 6) + textLength + bufferLength + (0.5f * (2 * mStar.Width)), topPadding), null, Color.White, 0.0f, Vector2.Zero, 0.75f, SpriteEffects.None, 0.0f);

            level.DeathStar = attempt[2];
            if (attempt[2] == 3)
            {
                spriteBatch.DrawString(mQuartz, "Perfect!", new Vector2(mScreenRect.Left + (4 * mScreenRect.Width / 6), topPadding), Color.White);
                spriteBatch.DrawString(mQuartz, "Perfect!", new Vector2(mScreenRect.Left + (4 * mScreenRect.Width / 6) + 1, topPadding + 1), Color.SteelBlue);
            }
               

            string request = "Press A To Continue To Next Level";

            Vector2 stringSize = mQuartz.MeasureString(request);

            spriteBatch.DrawString(mQuartz, request, new Vector2(mScreenRect.Center.X - (stringSize.X / 2), mScreenRect.Bottom - (stringSize.Y) * 3), Color.SteelBlue);
            spriteBatch.DrawString(mQuartz, request, new Vector2(mScreenRect.Center.X - (stringSize.X / 2)  + 2, mScreenRect.Bottom - (stringSize.Y) * 3  + 2), Color.White);

            request = "Press Start To Access The Menu";

            stringSize = mQuartz.MeasureString(request);

            spriteBatch.DrawString(mQuartz, request, new Vector2(mScreenRect.Center.X - (stringSize.X / 2), mScreenRect.Bottom - (stringSize.Y) * 2), Color.SteelBlue);
            spriteBatch.DrawString(mQuartz, request, new Vector2(mScreenRect.Center.X - (stringSize.X / 2) + 2, mScreenRect.Bottom - (stringSize.Y) * 2 + 2), Color.White);

            spriteBatch.End();
        }
    }
}
