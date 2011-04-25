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
    class StartLevelSplash
    {
        #region Member Variables

        private Rectangle mScreenRect;

        ContentManager mContent;

        IControlScheme mControls;

        private SpriteFont mQuartzSmall;
        private SpriteFont mQuartzLarge;

        #endregion

        #region Art

        private Texture2D mTitle;
        private Texture2D mTrans;
        private Texture2D mStar;

        #endregion

        public StartLevelSplash(IControlScheme controls)
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

            mTitle = content.Load<Texture2D>("Images/Menu/Mr_Gravity");
            mTrans = content.Load<Texture2D>("Images/Menu/Pause/PausedTrans");
            mStar = content.Load<Texture2D>("Images/NonHazards/YellowStar");

            mQuartzSmall = content.Load<SpriteFont>("Fonts/QuartzSmall");
            mQuartzLarge = content.Load<SpriteFont>("Fonts/QuartzLarge");
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
        public void Update(GameTime gameTime, ref GameStates gameState)
        {
            /* If the user selects one of the menu items */
            if (mControls.isAPressed(false) || mControls.isStartPressed(false))
            {
                GameSound.menuSound_select.Play(GameSound.volume, 0.0f, 0.0f);

                gameState = GameStates.In_Game;
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
        public void Draw(SpriteBatch spriteBatch, GraphicsDeviceManager graphics, Level level, Matrix scale)
        {
            spriteBatch.Begin(SpriteSortMode.Immediate,
                BlendState.AlphaBlend,
                SamplerState.LinearClamp,
                DepthStencilState.None,
                RasterizerState.CullCounterClockwise,
                null,
                scale);

            float[] mSize = new float[2] { (float)mScreenRect.Width / (float)graphics.GraphicsDevice.Viewport.Width, (float)mScreenRect.Height / (float)graphics.GraphicsDevice.Viewport.Height };

            float mHeight = mScreenRect.Top;

            spriteBatch.Draw(mTrans, graphics.GraphicsDevice.Viewport.Bounds, Color.White);
            //spriteBatch.Draw(mTitle, new Rectangle(mScreenRect.Center.X - (int)(mTitle.Width * mSize[0]) / 2, mScreenRect.Top, (int)(mTitle.Width * mSize[0]), (int)(mTitle.Height * mSize[1])), Color.White);

            string request = "Goals for " + level.Name + ":";

            Vector2 stringSize = mQuartzLarge.MeasureString(request);
            spriteBatch.DrawString(mQuartzLarge, request, new Vector2(mScreenRect.Center.X - stringSize.X / 2, mHeight), Color.DarkTurquoise);
            spriteBatch.DrawString(mQuartzLarge, request, new Vector2(mScreenRect.Center.X - stringSize.X / 2 + 2, mHeight + 2), Color.White);

            mHeight += (mQuartzLarge.LineSpacing + mQuartzSmall.LineSpacing);

            request = "Gems:";

            stringSize = mQuartzSmall.MeasureString(request);
            spriteBatch.DrawString(mQuartzSmall, request, new Vector2(mScreenRect.Center.X - stringSize.X / 2, mHeight), Color.DarkTurquoise);

            mHeight += mQuartzSmall.LineSpacing;

            /* Print the collectable goals for the level */
            request = " 0 - " + ((int)(level.NumCollectable * .8) - 1) + " gems";

            stringSize = mQuartzSmall.MeasureString(request);
            spriteBatch.DrawString(mQuartzSmall, request, new Vector2(mScreenRect.Center.X - mScreenRect.Width / 3 - stringSize.X / 2, mHeight), Color.White);
            request = (int)(level.NumCollectable * .8) + " - " + (level.NumCollectable - 1).ToString() + " gems";
            

            stringSize = mQuartzSmall.MeasureString(request);
            spriteBatch.DrawString(mQuartzSmall, request, new Vector2(mScreenRect.Center.X - stringSize.X / 2, mHeight), Color.White);

            request = level.NumCollectable.ToString() + " gems";

            stringSize = mQuartzSmall.MeasureString(request);
            spriteBatch.DrawString(mQuartzSmall, request, new Vector2(mScreenRect.Center.X + mScreenRect.Width / 3 - stringSize.X / 2, mHeight), Color.White);

            mHeight += mQuartzSmall.LineSpacing;

            /* 1 Star */
            spriteBatch.Draw(mStar, new Vector2(mScreenRect.Center.X - mScreenRect.Width / 3 - mStar.Width / 2, mHeight), Color.White);

            /* 2 Star */
            spriteBatch.Draw(mStar, new Vector2(mScreenRect.Center.X - mStar.Width, mHeight), Color.White);
            spriteBatch.Draw(mStar, new Vector2(mScreenRect.Center.X, mHeight), Color.White);

            /* 3 Star */
            spriteBatch.Draw(mStar, new Vector2(mScreenRect.Center.X + mScreenRect.Width / 3 - 3*mStar.Width/2, mHeight), Color.White);
            spriteBatch.Draw(mStar, new Vector2(mScreenRect.Center.X + mScreenRect.Width / 3 - mStar.Width / 2, mHeight), Color.White);
            spriteBatch.Draw(mStar, new Vector2(mScreenRect.Center.X + mScreenRect.Width / 3 + mStar.Width / 2, mHeight), Color.White);

            mHeight += (mStar.Height + mQuartzSmall.LineSpacing);

            request = "Time in seconds:";

            stringSize = mQuartzSmall.MeasureString(request);
            spriteBatch.DrawString(mQuartzSmall, request, new Vector2(mScreenRect.Center.X - stringSize.X / 2, mHeight), Color.DarkTurquoise);

            mHeight += mQuartzSmall.LineSpacing;

            /* Print the timer goals for the level */
            request = "Over " + (int)(level.IdealTime * 1.2) + " secs";

            stringSize = mQuartzSmall.MeasureString(request);
            spriteBatch.DrawString(mQuartzSmall, request, new Vector2(mScreenRect.Center.X - mScreenRect.Width / 3 - stringSize.X / 2, mHeight), Color.White);

            if((int)(level.IdealTime + 1) != (int)(level.IdealTime * 1.2))
                request = (int)(level.IdealTime + 1) + " - " + (int)(level.IdealTime * 1.2) + " secs";
            else
                request = (int)(level.IdealTime * 1.2) + " secs";

            stringSize = mQuartzSmall.MeasureString(request);
            spriteBatch.DrawString(mQuartzSmall, request, new Vector2(mScreenRect.Center.X - stringSize.X / 2, mHeight), Color.White);

            request = (int)(level.IdealTime) +" secs";

            stringSize = mQuartzSmall.MeasureString(request);
            spriteBatch.DrawString(mQuartzSmall, request, new Vector2(mScreenRect.Center.X + mScreenRect.Width / 3 - stringSize.X / 2, mHeight), Color.White);

            mHeight += mQuartzSmall.LineSpacing;

            /* 1 Star */
            spriteBatch.Draw(mStar, new Vector2(mScreenRect.Center.X - mScreenRect.Width / 3 - mStar.Width / 2, mHeight), Color.White);

            /* 2 Star */
            spriteBatch.Draw(mStar, new Vector2(mScreenRect.Center.X - mStar.Width, mHeight), Color.White);
            spriteBatch.Draw(mStar, new Vector2(mScreenRect.Center.X, mHeight), Color.White);

            /* 3 Star */
            spriteBatch.Draw(mStar, new Vector2(mScreenRect.Center.X + mScreenRect.Width / 3 - 3 * mStar.Width / 2, mHeight), Color.White);
            spriteBatch.Draw(mStar, new Vector2(mScreenRect.Center.X + mScreenRect.Width / 3 - mStar.Width / 2, mHeight), Color.White);
            spriteBatch.Draw(mStar, new Vector2(mScreenRect.Center.X + mScreenRect.Width / 3 + mStar.Width / 2, mHeight), Color.White);

            mHeight += (mStar.Height + mQuartzSmall.LineSpacing);

            request = "Deaths:";

            stringSize = mQuartzSmall.MeasureString(request);
            spriteBatch.DrawString(mQuartzSmall, request, new Vector2(mScreenRect.Center.X - stringSize.X / 2, mHeight), Color.DarkTurquoise);

            mHeight += mQuartzSmall.LineSpacing;

            /* Print the death goals for the level */
            request = " 3 - 5 Deaths";

            stringSize = mQuartzSmall.MeasureString(request);
            spriteBatch.DrawString(mQuartzSmall, request, new Vector2(mScreenRect.Center.X - mScreenRect.Width / 3 - stringSize.X / 2, mHeight), Color.White);

            request = " 1 - 2 Deaths";

            stringSize = mQuartzSmall.MeasureString(request);
            spriteBatch.DrawString(mQuartzSmall, request, new Vector2(mScreenRect.Center.X - stringSize.X / 2, mHeight), Color.White);

            request = " 0 Deaths";

            stringSize = mQuartzSmall.MeasureString(request);
            spriteBatch.DrawString(mQuartzSmall, request, new Vector2(mScreenRect.Center.X + mScreenRect.Width / 3 - stringSize.X / 2, mHeight), Color.White);

            mHeight += mQuartzSmall.LineSpacing;
            /* 1 Star */
            spriteBatch.Draw(mStar, new Vector2(mScreenRect.Center.X - mScreenRect.Width / 3 - mStar.Width / 2, mHeight), Color.White);

            /* 2 Star */
            spriteBatch.Draw(mStar, new Vector2(mScreenRect.Center.X - mStar.Width, mHeight), Color.White);
            spriteBatch.Draw(mStar, new Vector2(mScreenRect.Center.X, mHeight), Color.White);

            /* 3 Star */
            spriteBatch.Draw(mStar, new Vector2(mScreenRect.Center.X + mScreenRect.Width / 3 - 3 * mStar.Width / 2, mHeight), Color.White);
            spriteBatch.Draw(mStar, new Vector2(mScreenRect.Center.X + mScreenRect.Width / 3 - mStar.Width / 2, mHeight), Color.White);
            spriteBatch.Draw(mStar, new Vector2(mScreenRect.Center.X + mScreenRect.Width / 3 + mStar.Width / 2, mHeight), Color.White);

            spriteBatch.End();
        }
    }
}
