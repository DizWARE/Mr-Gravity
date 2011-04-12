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

using System.Diagnostics;

namespace GravityShift
{
    class PreScore
    {
        #region Member Variables

        private Rectangle mScreenRect;

        ContentManager mContent;

        IControlScheme mControls;

        #endregion

        #region Art

        private Texture2D mTrans;
        private SpriteFont mQuartz;

        #endregion

        private float xCoord, centerYCoord;
        private float topYCoord2, bottomYCoord2;
        private float topYCoord3, bottomYCoord3;

        private float mScale;

        private bool upToScale;
        private bool pulse;

        public static  List<string> starList;

        private string gemString, deathString, timeString;

        private bool mDoOnce;

        public PreScore(IControlScheme controls)
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

            mScreenRect = graphics.Viewport.TitleSafeArea;

            mTrans = content.Load<Texture2D>("Images/Menu/Pause/PausedTrans");
            mQuartz = content.Load<SpriteFont>("Fonts/QuartzEvenLarger");

            xCoord = mScreenRect.Center.X - mQuartz.MeasureString("GEM CHALLENGE").X / 2;
            centerYCoord = mScreenRect.Center.Y - mQuartz.MeasureString("TIME CHALLENGE").Y / 2;
            topYCoord2 = mScreenRect.Center.Y - mQuartz.MeasureString("TIME CHALLENGE").Y;
            bottomYCoord2 = mScreenRect.Center.Y + mQuartz.MeasureString("TIME CHALLENGE").Y;
            topYCoord3 = mScreenRect.Center.Y - mQuartz.MeasureString("GEM CHALLENGE").Y * 2;
            bottomYCoord3 = mScreenRect.Center.Y + mQuartz.MeasureString("DEATH CHALLENGE").Y;

            mScale = 0.0f;

            pulse = false;
            upToScale = false;

            gemString = "GEM CHALLENGE!";
            timeString = "TIME CHALLENGE!";
            deathString = "DEATH CHALLENGE!";

            mDoOnce = false;

            starList = new List<string>();
            starList.Clear();
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
            if (mScale <= 1 && !upToScale)
            {
                mScale += 0.05f;
            }
            else if (mScale >= 1.0f && mScale <= 1.02f && !pulse)
            {
                upToScale = false;
                mScale += 0.001f;
                if (mScale > 1.02f)
                    pulse = true;
            }
            else if (pulse)
            {
                mScale -= 0.001f;
                if (mScale <= 1.002f)
                    pulse = false;
            }
            /* If the user selects one of the menu items */
            if (mControls.isAPressed(false) || mControls.isStartPressed(false))
            {
                GameSound.menuSound_select.Play(GameSound.volume, 0.0f, 0.0f);

                /*Back To Level Selection*/
                gameState = GameStates.Score;
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
        public void Draw(SpriteBatch spriteBatch, GraphicsDeviceManager graphics, Matrix scale, Level currentLevel)
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

            if (!mDoOnce)
            {
                if (currentLevel.CollectionStar == 3)
                {
                    starList.Add(gemString);
                }
                if (currentLevel.TimerStar == 3)
                {
                    starList.Add(timeString);
                }
                if (currentLevel.DeathStar == 3)
                {
                    starList.Add(deathString);
                }
                mDoOnce = true;
            }

            if (starList.Count == 1)
            {
                spriteBatch.DrawString(mQuartz, starList[0], new Vector2(xCoord, centerYCoord), Color.White, 0.0f, Vector2.Zero, mScale, SpriteEffects.None, 0.0f);
                spriteBatch.DrawString(mQuartz, starList[0], new Vector2(xCoord + 4, centerYCoord + 4), Color.SteelBlue, 0.0f, Vector2.Zero, mScale, SpriteEffects.None, 0.0f);
            }
            else if (starList.Count == 2)
            {
                spriteBatch.DrawString(mQuartz, starList[0], new Vector2(xCoord, topYCoord2), Color.White, 0.0f, Vector2.Zero, mScale, SpriteEffects.None, 0.0f);
                spriteBatch.DrawString(mQuartz, starList[0], new Vector2(xCoord + 4, topYCoord2 + 4), Color.SteelBlue, 0.0f, Vector2.Zero, mScale, SpriteEffects.None, 0.0f);

                spriteBatch.DrawString(mQuartz, starList[1], new Vector2(xCoord, bottomYCoord2), Color.White, 0.0f, Vector2.Zero, mScale, SpriteEffects.None, 0.0f);
                spriteBatch.DrawString(mQuartz, starList[1], new Vector2(xCoord + 4, bottomYCoord2 + 4), Color.SteelBlue, 0.0f, Vector2.Zero, mScale, SpriteEffects.None, 0.0f);
            }
            else if (starList.Count == 3)
            {
                spriteBatch.DrawString(mQuartz, starList[0], new Vector2(xCoord, topYCoord3), Color.White, 0.0f, Vector2.Zero, mScale, SpriteEffects.None, 0.0f);
                spriteBatch.DrawString(mQuartz, starList[0], new Vector2(xCoord + 4, topYCoord3 + 4), Color.SteelBlue, 0.0f, Vector2.Zero, mScale, SpriteEffects.None, 0.0f);

                spriteBatch.DrawString(mQuartz, starList[1], new Vector2(xCoord, centerYCoord), Color.White, 0.0f, Vector2.Zero, mScale, SpriteEffects.None, 0.0f);
                spriteBatch.DrawString(mQuartz, starList[1], new Vector2(xCoord + 4, centerYCoord + 4), Color.SteelBlue, 0.0f, Vector2.Zero, mScale, SpriteEffects.None, 0.0f);

                spriteBatch.DrawString(mQuartz, starList[2], new Vector2(xCoord, bottomYCoord3), Color.White, 0.0f, Vector2.Zero, mScale, SpriteEffects.None, 0.0f);
                spriteBatch.DrawString(mQuartz, starList[2], new Vector2(xCoord + 4, bottomYCoord3 + 4), Color.SteelBlue, 0.0f, Vector2.Zero, mScale, SpriteEffects.None, 0.0f);
            }

            spriteBatch.End();
        }
    }
}
