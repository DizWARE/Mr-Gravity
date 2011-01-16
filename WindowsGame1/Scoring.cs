﻿using System;
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

        ContentManager mContent;

        IControlScheme mControls;

        /* Equivalent of stars */
        private static Texture2D[] mNumApples;
        private const int POSSIBLE_APPLES = 3;

        /* Keep track of the level */
        private static int[,] mLevel;
        private static int[,] mScore;

        private Texture2D[] mSelItems;
        private Texture2D[] mUnselItems;
        private Texture2D[] mItems;

        private int mCurrent;

        private const int NUM_OPTIONS = 2;

        #endregion

        #region Art

        private Texture2D mApple;
        private Texture2D mApple_gray;

        private Texture2D mBackUnsel;
        private Texture2D mBackSel;
        private Texture2D mRestartSel;
        private Texture2D mRestartUnsel;

        private Texture2D mTitle;

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
        public void Load(ContentManager content)
        {
            mContent = content;
            mKootenay = content.Load<SpriteFont>("fonts/Kootenay");

            mApple = content.Load<Texture2D>("scoring/apple");
            mApple_gray = content.Load<Texture2D>("scoring/apple_gray");

            /* Set up with the number of worlds and levels */
            /* For now we have 1 world with 3 levels */
            mLevel = new int[1, 3];
            mScore = new int[1, 3];
            mNumApples = new Texture2D[3];

            mNumApples[0] = mApple;
            mNumApples[1] = mApple;
            mNumApples[2] = mApple;

            mCurrent = 0;

            mSelItems = new Texture2D[NUM_OPTIONS];
            mUnselItems = new Texture2D[NUM_OPTIONS];
            mItems = new Texture2D[NUM_OPTIONS];

            mBackUnsel = content.Load<Texture2D>("menu/BackUnselected");
            mBackSel = content.Load<Texture2D>("menu/BackSelected");
        
            mRestartUnsel = content.Load<Texture2D>("menu/RestartUnselected");
            mRestartSel = content.Load<Texture2D>("menu/RestartSelected");

            mTitle = content.Load<Texture2D>("menu/title");

            mSelItems[0] = mRestartSel;
            mSelItems[1] = mBackSel;

            mUnselItems[0] = mRestartUnsel;
            mUnselItems[1] = mBackUnsel;

            mItems[0] = mRestartSel;
            mItems[1] = mBackUnsel;
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

            /* If the user hits up */
            if (mControls.isUpPressed(false))
            {
                /* If we are not on the first element already */
                if (mCurrent > 0)
                {
                    GameSound.menuSound_rollover.Play(GameSound.volume, 0.0f, 0.0f);
                    /* Decrement current and change the images */
                    mCurrent--;
                    for (int i = 0; i < NUM_OPTIONS; i++)
                        mItems[i] = mUnselItems[i];
                    mItems[mCurrent] = mSelItems[mCurrent];
                }
            }
            /* If the user hits the down button */
            if (mControls.isDownPressed(false))
            {
                /* If we are on the last element in the menu */
                if (mCurrent < NUM_OPTIONS - 1)
                {
                    GameSound.menuSound_rollover.Play(GameSound.volume, 0.0f, 0.0f);
                    /* Increment current and update graphics */
                    mCurrent++;
                    for (int i = 0; i < NUM_OPTIONS; i++)
                        mItems[i] = mUnselItems[i];
                    mItems[mCurrent] = mSelItems[mCurrent];
                }
            }

            /* If the user selects one of the menu items */
            if (mControls.isAPressed(false) || mControls.isStartPressed(false))
            {
                GameSound.menuSound_select.Play(GameSound.volume, 0.0f, 0.0f);
                /* Restart Game */
                if (mCurrent == 0)
                {
                    /* Start the game*/
                    gameState = GameStates.In_Game;
                    level.Reset();
                    level.Load(mContent);
                    mCurrent = 0;
                }
                /* Back Game */
                else if (mCurrent == 1)
                {
                    /*Back To Main Menu*/
                    gameState = GameStates.Main_Menu;

                    mCurrent = 0;
                }
            }
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
        public void Draw(SpriteBatch spriteBatch, GraphicsDeviceManager graphics)
        {
            spriteBatch.Begin();

            spriteBatch.Draw(mTitle, new Vector2(150.0f, 50.0f), Color.White);

            //spriteBatch.DrawString(kootenay, "Time: " + (int)GravityShiftMain.Timer + " Seconds", new Vector2(350.0f, 350.0f), Color.White);
            spriteBatch.Draw(mNumApples[0], new Vector2(350.0f, 400.0f), Color.White);
            spriteBatch.Draw(mNumApples[1], new Vector2(425.0f, 400.0f), Color.White);
            spriteBatch.Draw(mNumApples[2], new Vector2(500.0f, 400.0f), Color.White);

            spriteBatch.Draw(mItems[0], new Vector2(900.0f, 600.0f), Color.White);
            spriteBatch.Draw(mItems[1], new Vector2(900.0f, 675.0f), Color.White);

            spriteBatch.End();
        }
    }
}