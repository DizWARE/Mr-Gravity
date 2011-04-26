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
    class ResetConfirm
    {
        #region Member Variables

        /* Spritefont */
        private SpriteFont mQuartz;

        private Texture2D[] mSelItems;
        private Texture2D[] mUnselItems;
        private Texture2D[] mItems;

        IControlScheme mControls;

        private int mCurrent;

        ContentManager mContent;

        private const int NUM_OPTIONS = 2;

        #endregion

        #region Art

        private Texture2D mTitle;
        private Texture2D mBackground;

        private Texture2D mContinueSel;
        private Texture2D mContinueUnsel;

        private Texture2D mBackSel;
        private Texture2D mBackUnsel;

        #endregion

        public ResetConfirm(IControlScheme controlScheme)
        {
            mControls = controlScheme;
        }

        public void Load(ContentManager content)
        {
            mContent = content;

            mQuartz = content.Load<SpriteFont>("Fonts/QuartzLarge");

            mTitle = content.Load<Texture2D>("Images/Menu/Mr_Gravity");
            mBackground = content.Load<Texture2D>("Images/Menu/backgroundSquares1");

            mCurrent = 1;

            mSelItems = new Texture2D[NUM_OPTIONS];
            mUnselItems = new Texture2D[NUM_OPTIONS];
            mItems = new Texture2D[NUM_OPTIONS];

            mContinueSel = content.Load<Texture2D>("Images/Menu/Main/ContinueSelected");
            mContinueUnsel = content.Load<Texture2D>("Images/Menu/Main/ContinueUnselected");
            mBackSel = content.Load<Texture2D>("Images/Menu/Main/BackSelected");
            mBackUnsel = content.Load<Texture2D>("Images/Menu/Main/BackUnselected");

            mSelItems[0] = mContinueSel;
            mSelItems[1] = mBackSel;

            mUnselItems[0] = mContinueUnsel;
            mUnselItems[1] = mBackUnsel;

            mItems[0] = mContinueUnsel;
            mItems[1] = mBackSel;
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
            /* If the user selects a menu item */
            if (mControls.isAPressed(false) || mControls.isStartPressed(false))
            {
                GameSound.menuSound_select.Play(GameSound.volume, 0.0f, 0.0f);
                /* Continue */
                if (mCurrent == 0)
                {
                    gameState = GameStates.New_Level_Selection;
                    level.Reset();
                    mCurrent = 1;

                    for (int i = 0; i < NUM_OPTIONS; i++)
                        mItems[i] = mUnselItems[i];
                    mItems[mCurrent] = mSelItems[mCurrent];
                }
                /* Back */
                else if (mCurrent == 1)
                {
                    gameState = GameStates.Options;

                }
            }
            if (mControls.isBPressed(false) || mControls.isBackPressed(false))
                gameState = GameStates.Options;
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

            /* Draw the transparent background */
            spriteBatch.Draw(mBackground, graphics.GraphicsDevice.Viewport.Bounds, Color.White);

            /* Draw the pause title */
            spriteBatch.Draw(mTitle, new Vector2(center.X - mTitle.Width / 2, mScreenRect.Top), Color.White);

            Vector2 currentLocation = new Vector2(mScreenRect.Left, mScreenRect.Top + mTitle.Height);
            int height = mScreenRect.Height - mTitle.Height;
            height -= (mItems[0].Height + mItems[1].Height);
            height /= 2;
            currentLocation.Y += height * 2;

            string request = "Are you sure you wish to reset your save file? \n" + "                   All save data will be lost!";

            Vector2 stringSize = mQuartz.MeasureString(request);
            spriteBatch.DrawString(mQuartz, request, new Vector2(mScreenRect.Center.X - stringSize.X / 2, mScreenRect.Center.Y), Color.White);
            spriteBatch.DrawString(mQuartz, request, new Vector2(mScreenRect.Center.X - stringSize.X / 2 + 2, mScreenRect.Center.Y + 2), Color.CornflowerBlue);


            /* Draw the pause options */
            for (int i = 0; i < 2; i++)
            {
                spriteBatch.Draw(mItems[i], new Rectangle(mScreenRect.Center.X - (mItems[i].Width / 2), (int)currentLocation.Y, mItems[i].Width, mItems[i].Height), Color.White);
                currentLocation.Y += mItems[i].Height;
            }

            /* Draw the pause options */
            //spriteBatch.Draw(mItems[0], new Rectangle(graphics.GraphicsDevice.Viewport.TitleSafeArea.Center.X - (mItems[0].Width / 2),
            //graphics.GraphicsDevice.Viewport.TitleSafeArea.Bottom - mItems[0].Height - 300,
            //mItems[0].Width, mItems[0].Height), Color.White);
            //spriteBatch.Draw(mItems[1], new Rectangle(graphics.GraphicsDevice.Viewport.TitleSafeArea.Center.X - (mItems[1].Width / 2),
            //graphics.GraphicsDevice.Viewport.TitleSafeArea.Bottom - mItems[1].Height - 200,
            //mItems[1].Width, mItems[1].Height), Color.White);
            //spriteBatch.Draw(mItems[2], new Rectangle(graphics.GraphicsDevice.Viewport.TitleSafeArea.Center.X - (mItems[2].Width / 2), 
            //graphics.GraphicsDevice.Viewport.TitleSafeArea.Bottom - mItems[2].Height - 100, 
            //mItems[2].Width, mItems[2].Height), Color.White);

            spriteBatch.End();
        }
    }
}

