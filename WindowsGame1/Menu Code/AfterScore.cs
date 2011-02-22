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
    class AfterScore
    {
        #region Member Variables

        private Rectangle mScreenRect;

        ContentManager mContent;

        IControlScheme mControls;

        private Texture2D[] mSelItems;
        private Texture2D[] mUnselItems;
        private Texture2D[] mItems;

        private int mCurrent;

        private const int NUM_OPTIONS = 4;

        #endregion

        #region Art

        private Texture2D mNextLevelSel;
        private Texture2D mNextLevelUnsel;
        private Texture2D mSelectLevelSel;
        private Texture2D mSelectLevelUnsel;
        private Texture2D mMainMenuSel;
        private Texture2D mMainMenuUnsel;
        private Texture2D mRestartSel;
        private Texture2D mRestartUnsel;

        private Texture2D mTitle;
        private Texture2D mTrans;

        #endregion

        public AfterScore(IControlScheme controls)
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

            mCurrent = 0;

            mScreenRect = graphics.Viewport.TitleSafeArea;

            mSelItems = new Texture2D[NUM_OPTIONS];
            mUnselItems = new Texture2D[NUM_OPTIONS];
            mItems = new Texture2D[NUM_OPTIONS];

            mNextLevelSel = content.Load<Texture2D>("Images/Menu/Score/NextLevelSelected");
            mNextLevelUnsel = content.Load<Texture2D>("Images/Menu/Score/NextLevelUnselected");

            mSelectLevelSel = content.Load<Texture2D>("Images/Menu/SelectLevelSelected");
            mSelectLevelUnsel = content.Load<Texture2D>("Images/Menu/SelectLevelUnselected");

            mMainMenuSel = content.Load<Texture2D>("Images/Menu/MainMenuSelected");
            mMainMenuUnsel = content.Load<Texture2D>("Images/Menu/MainMenuUnselected");

            mRestartUnsel = content.Load<Texture2D>("Images/Menu/Score/RestartUnselected");
            mRestartSel = content.Load<Texture2D>("Images/Menu/Score/RestartSelected");

            mTitle = content.Load<Texture2D>("Images/Menu/Mr_Gravity");

            mTrans = content.Load<Texture2D>("Images/Menu/Pause/PausedTrans");

            mSelItems[0] = mNextLevelSel;
            mSelItems[1] = mRestartSel;
            mSelItems[2] = mSelectLevelSel;
            mSelItems[3] = mMainMenuSel;

            mUnselItems[0] = mNextLevelUnsel;
            mUnselItems[1] = mRestartUnsel;
            mUnselItems[2] = mSelectLevelUnsel;
            mUnselItems[3] = mMainMenuUnsel;

            mItems[0] = mNextLevelSel;
            mItems[1] = mRestartUnsel;
            mItems[2] = mSelectLevelUnsel;
            mItems[3] = mMainMenuUnsel;
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
                level.mTimer = 0;
                GameSound.menuSound_select.Play(GameSound.volume, 0.0f, 0.0f);

                /* Next Level */
                if (mCurrent == 0)
                {
                    /*Back To Level Selection*/
                    gameState = GameStates.Next_Level;

                    mCurrent = 0;

                    mItems[0] = mNextLevelSel;
                    mItems[1] = mRestartUnsel;
                    mItems[2] = mSelectLevelUnsel;
                    mItems[3] = mMainMenuUnsel;

                }
                /* Restart Level */
                else if (mCurrent == 1)
                {

                    /* Start the game*/
                    gameState = GameStates.In_Game;
                    level.Reset();
                    level.Load(mContent);
                    mCurrent = 0;

                    mItems[0] = mNextLevelSel;
                    mItems[1] = mRestartUnsel;
                    mItems[2] = mSelectLevelUnsel;
                    mItems[3] = mMainMenuUnsel;
                }

                /* Level Select */
                else if (mCurrent == 2)
                {
                    /*Back To Level Selection*/
                    gameState = GameStates.Level_Selection;

                    mCurrent = 0;

                    mItems[0] = mNextLevelSel;
                    mItems[1] = mRestartUnsel;
                    mItems[2] = mSelectLevelUnsel;
                    mItems[3] = mMainMenuUnsel;
                }

                /* Main Menu */
                else if (mCurrent == 3)
                {
                    gameState = GameStates.Main_Menu;

                    mCurrent = 0;

                    mItems[0] = mNextLevelSel;
                    mItems[1] = mRestartUnsel;
                    mItems[2] = mSelectLevelUnsel;
                    mItems[3] = mMainMenuUnsel;
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
        public void Draw(SpriteBatch spriteBatch, GraphicsDeviceManager graphics, Matrix scale)
        {
            spriteBatch.Begin(SpriteSortMode.Immediate,
                BlendState.AlphaBlend,
                SamplerState.LinearClamp,
                DepthStencilState.None,
                RasterizerState.CullCounterClockwise,
                null,
                scale);

            float[] mSize = new float[2] { (float)mScreenRect.Width / (float)graphics.GraphicsDevice.Viewport.Width, (float)mScreenRect.Height / (float)graphics.GraphicsDevice.Viewport.Height };

            spriteBatch.Draw(mTrans, new Rectangle(mScreenRect.Left, mScreenRect.Top, mScreenRect.Width, mScreenRect.Height), Color.White);

            spriteBatch.Draw(mTitle, new Rectangle(mScreenRect.Center.X - (int)(mTitle.Width * mSize[0]) / 2, mScreenRect.Top, (int)(mTitle.Width * mSize[0]), (int)(mTitle.Height * mSize[1])), Color.White);

            Vector2 currentLocation = new Vector2(mScreenRect.Left, mScreenRect.Top + (int)(mTitle.Height * mSize[1]));
            int height = mScreenRect.Height - (int)(mTitle.Height * mSize[1]);
            height -= ((int)(mItems[0].Height * mSize[1]) + (int)(mItems[1].Height * mSize[1]) + (int)(mItems[2].Height * mSize[1]) + (int)(mItems[3].Height * mSize[1]));
            height /= 2;
            currentLocation.Y += height;
            for (int i = 0; i < 4; i++)
            {
                spriteBatch.Draw(mItems[i], new Rectangle(mScreenRect.Center.X - ((int)(mItems[i].Width * mSize[0]) / 2), (int)currentLocation.Y, (int)(mItems[i].Width * mSize[0]), (int)(mItems[i].Height * mSize[1])), Color.White);
                currentLocation.Y += (int)(mItems[i].Height * mSize[1]);
            }

            //spriteBatch.Draw(mItems[0], new Rectangle(graphics.GraphicsDevice.Viewport.TitleSafeArea.Center.X - (mItems[0].Width / 2),
                //graphics.GraphicsDevice.Viewport.TitleSafeArea.Center.Y - mItems[0].Height,
                //mItems[0].Width, mItems[0].Height), Color.White);
            //spriteBatch.Draw(mItems[1], new Rectangle(graphics.GraphicsDevice.Viewport.TitleSafeArea.Center.X - (mItems[1].Width / 2),
                //graphics.GraphicsDevice.Viewport.TitleSafeArea.Center.Y,
                //mItems[1].Width, mItems[1].Height), Color.White);
            //spriteBatch.Draw(mItems[2], new Rectangle(graphics.GraphicsDevice.Viewport.TitleSafeArea.Center.X - (mItems[2].Width / 2),
                //graphics.GraphicsDevice.Viewport.TitleSafeArea.Bottom - mItems[2].Height - 200,
                //mItems[2].Width, mItems[2].Height), Color.White);
            //spriteBatch.Draw(mItems[3], new Rectangle(graphics.GraphicsDevice.Viewport.TitleSafeArea.Center.X - (mItems[3].Width / 2),
                //graphics.GraphicsDevice.Viewport.TitleSafeArea.Bottom - mItems[3].Height - 125,
                //mItems[3].Width, mItems[3].Height), Color.White);

            spriteBatch.End();
        }
    }
}
