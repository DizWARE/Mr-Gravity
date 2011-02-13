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
            int []scores = GetRank((int)GravityShift.Level.TIMER, level.IdealTime, (int)GravityShift.Level.mNumCollected, level.CollectableCount, GravityShift.Level.mDeaths);
            level.TimerStar = scores[0];
            level.CollectionStar = scores[1];
            level.DeathStar = scores[2];

            if (mControls.isStartPressed(false) || mControls.isAPressed(false))
                gameState = GameStates.AfterScore;
            
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
         */
        public int[] GetRank(int time, int timeGoal, int collect, int collectGoal, int deathTotal)
        {
            int[] result = new int[3];

            /* TIME -- 100%+, <120%, <140%, >140% */
            if (time < timeGoal) { result[0] = 3; }
            else if (((double) time / (double) timeGoal) > 1.2) { result[0] = 2; }
            else if (((double) time / (double) timeGoal) > 1.4) { result[0] = 1; }
            else { result[0] = 0; }

            /* COLLECTABLES -- 100%, >80%, >60%, <60% */
            if (collect == collectGoal) { result[1] = 3; }
            else if (((double) collect / (double) collectGoal) > 0.8) { result[1] = 2; }
            else if (((double) collect / (double) collectGoal) > 0.6) { result[1] = 1; }
            else { result[1] = 0; }

            /* DEATHS -- 0, 1, 2-3, >3 */
            if (deathTotal == 0) { result[2] = 3; }
            else if (deathTotal == 1) { result[2] = 2; }
            else if (deathTotal <= 3) { result[2] = 1; }
            else { result[2] = 0; }

            return result;
        }

        /*
         * Draw
         *
         * This function will draw the current menu
         *
         * SpriteBatch spriteBatch: The current sprite batch used to draw
         * 
         * GraphicsDeviceManager graphics: The current graphics manager
         */
        public void Draw(SpriteBatch spriteBatch, GraphicsDeviceManager graphics, Matrix scale)
        {
            spriteBatch.Begin(SpriteSortMode.Immediate,
                BlendState.AlphaBlend,
                SamplerState.LinearClamp,
                DepthStencilState.None,
                RasterizerState.CullCounterClockwise,
                null,
                scale);

            //TODO: CHANGE TO DYNAMIC PLACING

            spriteBatch.Draw(mBackground, new Rectangle(0, 0, graphics.GraphicsDevice.Viewport.Width, graphics.GraphicsDevice.Viewport.Height), Color.White);
            spriteBatch.Draw(mTitle, new Vector2(mScreenRect.Left + (mScreenRect.Width - mTitle.Width) / 2, mScreenRect.Top), Color.White);

            spriteBatch.DrawString(mQuartz, "Time:", new Vector2(mScreenRect.Left + (mScreenRect.Width / 5) , mScreenRect.Top + mScreenRect.Height / 4), Color.White);
            spriteBatch.DrawString(mQuartz, "Time:", new Vector2(mScreenRect.Left + (mScreenRect.Width / 5) + 1, mScreenRect.Top + mScreenRect.Height / 4 + 1), Color.SteelBlue);
            spriteBatch.DrawString(mQuartz, (int)GravityShift.Level.TIMER + " Seconds", new Vector2(mScreenRect.Left + (mScreenRect.Width / 3 + 100), mScreenRect.Top + mScreenRect.Height / 4), Color.White);

            spriteBatch.DrawString(mQuartz, "Collected:", new Vector2(mScreenRect.Left + (mScreenRect.Width / 5), mScreenRect.Top + mScreenRect.Height / 4 + 50), Color.White);
            spriteBatch.DrawString(mQuartz, "Collected:", new Vector2(mScreenRect.Left + (mScreenRect.Width / 5) + 1, mScreenRect.Top + mScreenRect.Height / 4 + 51), Color.SteelBlue);
            spriteBatch.DrawString(mQuartz, (int)GravityShift.Level.mNumCollected + " / " + GravityShift.Level.mNumCollectable, new Vector2(mScreenRect.Left + (mScreenRect.Width / 3 + 100), mScreenRect.Top + mScreenRect.Height / 4 + 50), Color.White);

            spriteBatch.DrawString(mQuartz, "Deaths:", new Vector2(mScreenRect.Left + (mScreenRect.Width / 5), mScreenRect.Top + mScreenRect.Height / 4 + 100), Color.White);
            spriteBatch.DrawString(mQuartz, "Deaths:", new Vector2(mScreenRect.Left + (mScreenRect.Width / 5) + 1, mScreenRect.Top + mScreenRect.Height / 4 + 101), Color.SteelBlue);
            spriteBatch.DrawString(mQuartz, "" + (int)GravityShift.Level.mDeaths, new Vector2(mScreenRect.Left + (mScreenRect.Width / 3 + 100), mScreenRect.Top + mScreenRect.Height / 4 + 100), Color.White);

            string request = "Press Start Or A To Continue";

            Vector2 stringSize = mQuartz.MeasureString(request);

            spriteBatch.DrawString(mQuartz, request, new Vector2(mScreenRect.Center.X - (stringSize.X / 2), mScreenRect.Bottom - (stringSize.Y) - 50), Color.SteelBlue);
            spriteBatch.DrawString(mQuartz, request, new Vector2(mScreenRect.Center.X - (stringSize.X / 2), mScreenRect.Bottom - (stringSize.Y) - 48), Color.White);

            spriteBatch.End();
        }
    }
}
