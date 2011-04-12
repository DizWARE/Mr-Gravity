using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using System.Xml.Linq;
using GravityShift.Import_Code;
using System.IO;

namespace GravityShift
{
    class Pause
    {
        #region Member Variables

        /* Spritefont */
        private SpriteFont mKootenay;

        private Texture2D[] mSelItems;
        private Texture2D[] mUnselItems;
        private Texture2D[] mItems;

        IControlScheme mControls;

        private int mCurrent;
        
        private Texture2D mLoading;

        ContentManager mContent;

        private const int NUM_OPTIONS = 4;

        #endregion

        #region Art

        private Texture2D mPauseTitle;
        private Texture2D mPausedTrans;

        private Texture2D mResumeSel;
        private Texture2D mResumeUnsel;
        private Texture2D mSelectLevelSel;
        private Texture2D mSelectLevelUnsel;
        private Texture2D mMainMenuSel;
        private Texture2D mMainMenuUnsel;
        private Texture2D mRestartSel;
        private Texture2D mRestartUnsel;

        #endregion

        public Pause(IControlScheme controlScheme)
        {
            mControls = controlScheme;
        }

        public void Load(ContentManager content)
        {
            mContent = content;

            mKootenay = content.Load<SpriteFont>("Fonts/Kootenay");

            mPauseTitle = content.Load<Texture2D>("Images/Menu/Pause/Paused");
            mPausedTrans = content.Load<Texture2D>("Images/Menu/Pause/PausedTrans");

            mCurrent = 0;

            mSelItems = new Texture2D[NUM_OPTIONS];
            mUnselItems = new Texture2D[NUM_OPTIONS];
            mItems = new Texture2D[NUM_OPTIONS];

            mResumeSel = content.Load<Texture2D>("Images/Menu/Pause/ResumeSelected");
            mResumeUnsel = content.Load<Texture2D>("Images/Menu/Pause/ResumeUnselected");
            mSelectLevelSel = content.Load<Texture2D>("Images/Menu/SelectLevelSelected");
            mSelectLevelUnsel = content.Load<Texture2D>("Images/Menu/SelectLevelUnselected");
            mMainMenuSel = content.Load<Texture2D>("Images/Menu/MainMenuSelected");
            mMainMenuUnsel = content.Load<Texture2D>("Images/Menu/MainMenuUnselected");
            mRestartSel = content.Load<Texture2D>("Images/Menu/Score/RestartSelected");
            mRestartUnsel = content.Load<Texture2D>("Images/Menu/Score/RestartUnselected");

            mLoading = content.Load<Texture2D>("Images/Menu/LevelSelect/LoadingMenu");

            mSelItems[0] = mResumeSel;
            mSelItems[1] = mRestartSel;
            mSelItems[2] = mSelectLevelSel;
            mSelItems[3] = mMainMenuSel;

            mUnselItems[0] = mResumeUnsel;
            mUnselItems[1] = mRestartUnsel;
            mUnselItems[2] = mSelectLevelUnsel;
            mUnselItems[3] = mMainMenuUnsel;

            mItems[0] = mResumeSel;
            mItems[1] = mRestartUnsel;
            mItems[2] = mSelectLevelUnsel;
            mItems[3] = mMainMenuUnsel;
        }

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
            if (mControls.isBPressed(false) || mControls.isBackPressed(false))
            {
                mCurrent = 0;
                gameState = GameStates.In_Game;

                mItems[0] = mResumeSel;
                mItems[1] = mRestartUnsel;
                mItems[2] = mSelectLevelUnsel;
                mItems[3] = mMainMenuUnsel;
            }
            /* If the user selects a menu item */
            if (mControls.isAPressed(false) || mControls.isStartPressed(false))
            {
                 GameSound.menuSound_select.Play(GameSound.volume, 0.0f, 0.0f);
                /* Resume Game */
                 if (mCurrent == 0)
                     gameState = GameStates.In_Game;
                 /* Restart */
                 else if (mCurrent == 1)
                 {
                     level.ResetAll();
                     gameState = GameStates.StartLevelSplash;
                     mCurrent = 0;
                 }
                 /* Select Level */
                 else if (mCurrent == 2)
                 {
                     gameState = GameStates.Level_Selection;
                     level.Reset();
                     mCurrent = 0;
                 }
                 /* Main Menu */
                 else if (mCurrent == 3)
                 {
                     gameState = GameStates.Main_Menu;
                     level.Reset();
                     mCurrent = 0;
                 }
                 
                 mItems[0] = mResumeSel;
                 mItems[1] = mRestartUnsel;
                 mItems[2] = mSelectLevelUnsel;
                 mItems[3] = mMainMenuUnsel;
            }
        }

        public void Draw(SpriteBatch spriteBatch, GraphicsDeviceManager graphics, Matrix scale)
        {
            spriteBatch.Begin(SpriteSortMode.Immediate,
                BlendState.AlphaBlend,
                SamplerState.LinearClamp,
                DepthStencilState.None,
                RasterizerState.CullCounterClockwise,
                null,
                scale);

            Point center = graphics.GraphicsDevice.Viewport.TitleSafeArea.Center;
            Rectangle mScreenRect = graphics.GraphicsDevice.Viewport.TitleSafeArea;

            float[] mSize = new float[2] { (float)mScreenRect.Width / (float)graphics.GraphicsDevice.Viewport.Width, (float)mScreenRect.Height / (float)graphics.GraphicsDevice.Viewport.Height };

            /* Draw the transparent background */
            spriteBatch.Draw(mPausedTrans, new Rectangle(0, 0, graphics.GraphicsDevice.Viewport.Width, graphics.GraphicsDevice.Viewport.Height), Color.White); 

            /* Draw the pause title */
            spriteBatch.Draw(mPauseTitle, new Rectangle(center.X - (int)(mPauseTitle.Width * mSize[0]) / 2, mScreenRect.Top, (int)(mPauseTitle.Width * mSize[0]), (int)(mPauseTitle.Height * mSize[1])), Color.White);

            Vector2 currentLocation = new Vector2(mScreenRect.Left, mScreenRect.Top + (int)(mPauseTitle.Height  * mSize[1]));
            int height = mScreenRect.Height - (int)(mPauseTitle.Height  * mSize[1]);
            height -= ((int)(mItems[0].Height * mSize[1]) + (int)(mItems[1].Height * mSize[1]) + (int)(mItems[2].Height * mSize[1]));
            height /= 2;
            currentLocation.Y += height;


            /* Draw the pause options */
            for (int i = 0; i < NUM_OPTIONS; i++)
            {
                spriteBatch.Draw(mItems[i], new Rectangle(mScreenRect.Center.X - ((int)(mItems[i].Width * mSize[0]) / 2), (int)currentLocation.Y, (int)(mItems[i].Width * mSize[0]), (int)(mItems[i].Height * mSize[1])), Color.White);
                currentLocation.Y += (int)(mItems[i].Height * mSize[1]);
            }
            
            spriteBatch.End();
        }
    }
}
