using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MrGravity.MISC_Code;

namespace MrGravity.Menu_Code
{
    internal class StartLevelSplash
    {
        #region Member Variables

        private Rectangle _mScreenRect;

        private ContentManager _mContent;

        private readonly IControlScheme _mControls;

        private SpriteFont _mQuartzSmall;
        private SpriteFont _mQuartzLarge;

        #endregion

        #region Art

        private Texture2D _mTitle;
        private Texture2D _mTrans;
        private Texture2D _mStar;

        #endregion

        public StartLevelSplash(IControlScheme controls)
        {
            _mControls = controls;
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
            _mContent = content;

            _mScreenRect = graphics.Viewport.TitleSafeArea;

            _mTitle = content.Load<Texture2D>("Images/Menu/Mr_Gravity");
            _mTrans = content.Load<Texture2D>("Images/Menu/Pause/PausedTrans");
            _mStar = content.Load<Texture2D>("Images/NonHazards/YellowStar");

            _mQuartzSmall = content.Load<SpriteFont>("Fonts/QuartzSmall");
            _mQuartzLarge = content.Load<SpriteFont>("Fonts/QuartzLarge");
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
            if (_mControls.IsAPressed(false) || _mControls.IsStartPressed(false))
            {
                GameSound.MenuSoundSelect.Play(GameSound.Volume, 0.0f, 0.0f);

                gameState = GameStates.InGame;
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

            var mSize = new float[2] { _mScreenRect.Width / (float)graphics.GraphicsDevice.Viewport.Width, _mScreenRect.Height / (float)graphics.GraphicsDevice.Viewport.Height };

            float mHeight = _mScreenRect.Top;

            spriteBatch.Draw(_mTrans, graphics.GraphicsDevice.Viewport.Bounds, Color.White);
            //spriteBatch.Draw(mTitle, new Rectangle(mScreenRect.Center.X - (int)(mTitle.Width * mSize[0]) / 2, mScreenRect.Top, (int)(mTitle.Width * mSize[0]), (int)(mTitle.Height * mSize[1])), Color.White);

            var request = "Goals for " + level.Name + ":";

            Vector2 stringSize = _mQuartzLarge.MeasureString(request);
            spriteBatch.DrawString(_mQuartzLarge, request, new Vector2(_mScreenRect.Center.X - stringSize.X / 2, mHeight), Color.DarkTurquoise);
            spriteBatch.DrawString(_mQuartzLarge, request, new Vector2(_mScreenRect.Center.X - stringSize.X / 2 + 2, mHeight + 2), Color.White);

            mHeight += (_mQuartzLarge.LineSpacing + _mQuartzSmall.LineSpacing);

            request = "Gems:";

            stringSize = _mQuartzSmall.MeasureString(request);
            spriteBatch.DrawString(_mQuartzSmall, request, new Vector2(_mScreenRect.Center.X - stringSize.X / 2, mHeight), Color.DarkTurquoise);

            mHeight += _mQuartzSmall.LineSpacing;

            /* Print the collectable goals for the level */
            if ((int)(level.NumCollectable * .8) - 1 != 0)
                request = " 0 - " + ((int)(level.NumCollectable * .8) - 1) + " gems";
            else
                request = " 0 gems";

            stringSize = _mQuartzSmall.MeasureString(request);
            spriteBatch.DrawString(_mQuartzSmall, request, new Vector2(_mScreenRect.Center.X - _mScreenRect.Width / 3 - stringSize.X / 2, mHeight), Color.White);

            if ((level.NumCollectable - 1) != (int)(level.NumCollectable * .8))
                request = (int)(level.NumCollectable * .8) + " - " + (level.NumCollectable - 1) + " gems";
            else
                request = (int)(level.NumCollectable * .8) + " gems";
            

            stringSize = _mQuartzSmall.MeasureString(request);
            spriteBatch.DrawString(_mQuartzSmall, request, new Vector2(_mScreenRect.Center.X - stringSize.X / 2, mHeight), Color.White);

            request = level.NumCollectable + " gems";

            stringSize = _mQuartzSmall.MeasureString(request);
            spriteBatch.DrawString(_mQuartzSmall, request, new Vector2(_mScreenRect.Center.X + _mScreenRect.Width / 3 - stringSize.X / 2, mHeight), Color.White);

            mHeight += _mQuartzSmall.LineSpacing;

            /* 1 Star */
            spriteBatch.Draw(_mStar, new Vector2(_mScreenRect.Center.X - _mScreenRect.Width / 3 - _mStar.Width / 2, mHeight), Color.White);

            /* 2 Star */
            spriteBatch.Draw(_mStar, new Vector2(_mScreenRect.Center.X - _mStar.Width, mHeight), Color.White);
            spriteBatch.Draw(_mStar, new Vector2(_mScreenRect.Center.X, mHeight), Color.White);

            /* 3 Star */
            spriteBatch.Draw(_mStar, new Vector2(_mScreenRect.Center.X + _mScreenRect.Width / 3 - 3*_mStar.Width/2, mHeight), Color.White);
            spriteBatch.Draw(_mStar, new Vector2(_mScreenRect.Center.X + _mScreenRect.Width / 3 - _mStar.Width / 2, mHeight), Color.White);
            spriteBatch.Draw(_mStar, new Vector2(_mScreenRect.Center.X + _mScreenRect.Width / 3 + _mStar.Width / 2, mHeight), Color.White);

            mHeight += (_mStar.Height + _mQuartzSmall.LineSpacing);

            request = "Time in seconds:";

            stringSize = _mQuartzSmall.MeasureString(request);
            spriteBatch.DrawString(_mQuartzSmall, request, new Vector2(_mScreenRect.Center.X - stringSize.X / 2, mHeight), Color.DarkTurquoise);

            mHeight += _mQuartzSmall.LineSpacing;

            /* Print the timer goals for the level */
            request = "Over " + (int)(level.IdealTime * 1.2) + " secs";

            stringSize = _mQuartzSmall.MeasureString(request);
            spriteBatch.DrawString(_mQuartzSmall, request, new Vector2(_mScreenRect.Center.X - _mScreenRect.Width / 3 - stringSize.X / 2, mHeight), Color.White);

            if(level.IdealTime + 1 != (int)(level.IdealTime * 1.2))
                request = level.IdealTime + 1 + " - " + (int)(level.IdealTime * 1.2) + " secs";
            else
                request = (int)(level.IdealTime * 1.2) + " secs";

            stringSize = _mQuartzSmall.MeasureString(request);
            spriteBatch.DrawString(_mQuartzSmall, request, new Vector2(_mScreenRect.Center.X - stringSize.X / 2, mHeight), Color.White);

            request = level.IdealTime +" secs";

            stringSize = _mQuartzSmall.MeasureString(request);
            spriteBatch.DrawString(_mQuartzSmall, request, new Vector2(_mScreenRect.Center.X + _mScreenRect.Width / 3 - stringSize.X / 2, mHeight), Color.White);

            mHeight += _mQuartzSmall.LineSpacing;

            /* 1 Star */
            spriteBatch.Draw(_mStar, new Vector2(_mScreenRect.Center.X - _mScreenRect.Width / 3 - _mStar.Width / 2, mHeight), Color.White);

            /* 2 Star */
            spriteBatch.Draw(_mStar, new Vector2(_mScreenRect.Center.X - _mStar.Width, mHeight), Color.White);
            spriteBatch.Draw(_mStar, new Vector2(_mScreenRect.Center.X, mHeight), Color.White);

            /* 3 Star */
            spriteBatch.Draw(_mStar, new Vector2(_mScreenRect.Center.X + _mScreenRect.Width / 3 - 3 * _mStar.Width / 2, mHeight), Color.White);
            spriteBatch.Draw(_mStar, new Vector2(_mScreenRect.Center.X + _mScreenRect.Width / 3 - _mStar.Width / 2, mHeight), Color.White);
            spriteBatch.Draw(_mStar, new Vector2(_mScreenRect.Center.X + _mScreenRect.Width / 3 + _mStar.Width / 2, mHeight), Color.White);

            mHeight += (_mStar.Height + _mQuartzSmall.LineSpacing);

            request = "Deaths:";

            stringSize = _mQuartzSmall.MeasureString(request);
            spriteBatch.DrawString(_mQuartzSmall, request, new Vector2(_mScreenRect.Center.X - stringSize.X / 2, mHeight), Color.DarkTurquoise);

            mHeight += _mQuartzSmall.LineSpacing;

            /* Print the death goals for the level */
            request = " 3 - 5 Deaths";

            stringSize = _mQuartzSmall.MeasureString(request);
            spriteBatch.DrawString(_mQuartzSmall, request, new Vector2(_mScreenRect.Center.X - _mScreenRect.Width / 3 - stringSize.X / 2, mHeight), Color.White);

            request = " 1 - 2 Deaths";

            stringSize = _mQuartzSmall.MeasureString(request);
            spriteBatch.DrawString(_mQuartzSmall, request, new Vector2(_mScreenRect.Center.X - stringSize.X / 2, mHeight), Color.White);

            request = " 0 Deaths";

            stringSize = _mQuartzSmall.MeasureString(request);
            spriteBatch.DrawString(_mQuartzSmall, request, new Vector2(_mScreenRect.Center.X + _mScreenRect.Width / 3 - stringSize.X / 2, mHeight), Color.White);

            mHeight += _mQuartzSmall.LineSpacing;
            /* 1 Star */
            spriteBatch.Draw(_mStar, new Vector2(_mScreenRect.Center.X - _mScreenRect.Width / 3 - _mStar.Width / 2, mHeight), Color.White);

            /* 2 Star */
            spriteBatch.Draw(_mStar, new Vector2(_mScreenRect.Center.X - _mStar.Width, mHeight), Color.White);
            spriteBatch.Draw(_mStar, new Vector2(_mScreenRect.Center.X, mHeight), Color.White);

            /* 3 Star */
            spriteBatch.Draw(_mStar, new Vector2(_mScreenRect.Center.X + _mScreenRect.Width / 3 - 3 * _mStar.Width / 2, mHeight), Color.White);
            spriteBatch.Draw(_mStar, new Vector2(_mScreenRect.Center.X + _mScreenRect.Width / 3 - _mStar.Width / 2, mHeight), Color.White);
            spriteBatch.Draw(_mStar, new Vector2(_mScreenRect.Center.X + _mScreenRect.Width / 3 + _mStar.Width / 2, mHeight), Color.White);

            spriteBatch.End();
        }
    }
}
