using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MrGravity.MISC_Code;

namespace MrGravity
{
    internal class ResetConfirm
    {
        #region Member Variables

        /* Spritefont */
        private SpriteFont _mQuartz;

        private Texture2D[] _mSelItems;
        private Texture2D[] _mUnselItems;
        private Texture2D[] _mItems;

        private readonly IControlScheme _mControls;

        private int _mCurrent;

        public ContentManager Content { get; private set; }

        private const int NumOptions = 2;

        #endregion

        #region Art

        private Texture2D _mTitle;
        private Texture2D _mBackground;

        private Texture2D _mContinueSel;
        private Texture2D _mContinueUnsel;

        private Texture2D _mBackSel;
        private Texture2D _mBackUnsel;

        #endregion

        public ResetConfirm(IControlScheme controlScheme)
        {
            _mControls = controlScheme;
        }

        public void Load(ContentManager content)
        {
            Content = content;

            _mQuartz = content.Load<SpriteFont>("Fonts/QuartzLarge");

            _mTitle = content.Load<Texture2D>("Images/Menu/Mr_Gravity");
            _mBackground = content.Load<Texture2D>("Images/Menu/backgroundSquares1");

            _mCurrent = 1;

            _mSelItems = new Texture2D[NumOptions];
            _mUnselItems = new Texture2D[NumOptions];
            _mItems = new Texture2D[NumOptions];

            _mContinueSel = content.Load<Texture2D>("Images/Menu/Main/ContinueSelected");
            _mContinueUnsel = content.Load<Texture2D>("Images/Menu/Main/ContinueUnselected");
            _mBackSel = content.Load<Texture2D>("Images/Menu/Main/BackSelected");
            _mBackUnsel = content.Load<Texture2D>("Images/Menu/Main/BackUnselected");

            _mSelItems[0] = _mContinueSel;
            _mSelItems[1] = _mBackSel;

            _mUnselItems[0] = _mContinueUnsel;
            _mUnselItems[1] = _mBackUnsel;

            _mItems[0] = _mContinueUnsel;
            _mItems[1] = _mBackSel;
        }

        public void Update(GameTime gameTime, ref GameStates gameState, ref Level level)
        {
            /* If the user hits up */
            if (_mControls.IsUpPressed(false))
            {
                /* If we are not on the first element already */
                if (_mCurrent > 0)
                {
                    GameSound.MenuSoundRollover.Play(GameSound.Volume, 0.0f, 0.0f);
                    /* Decrement current and change the images */
                    _mCurrent--;
                    for (var i = 0; i < NumOptions; i++)
                        _mItems[i] = _mUnselItems[i];
                    _mItems[_mCurrent] = _mSelItems[_mCurrent];
                }
            }
            /* If the user hits the down button */
            if (_mControls.IsDownPressed(false))
            {
                /* If we are on the last element in the menu */
                if (_mCurrent < NumOptions - 1)
                {
                    GameSound.MenuSoundRollover.Play(GameSound.Volume, 0.0f, 0.0f);
                    /* Increment current and update graphics */
                    _mCurrent++;
                    for (var i = 0; i < NumOptions; i++)
                        _mItems[i] = _mUnselItems[i];
                    _mItems[_mCurrent] = _mSelItems[_mCurrent];
                }
            }
            /* If the user selects a menu item */
            if (_mControls.IsAPressed(false) || _mControls.IsStartPressed(false))
            {
                GameSound.MenuSoundSelect.Play(GameSound.Volume, 0.0f, 0.0f);
                /* Continue */
                if (_mCurrent == 0)
                {
                    gameState = GameStates.NewLevelSelection;
                    level.Reset();
                    _mCurrent = 1;

                    for (var i = 0; i < NumOptions; i++)
                        _mItems[i] = _mUnselItems[i];
                    _mItems[_mCurrent] = _mSelItems[_mCurrent];
                }
                /* Back */
                else if (_mCurrent == 1)
                {
                    gameState = GameStates.Options;

                }
            }
            if (_mControls.IsBPressed(false) || _mControls.IsBackPressed(false))
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
            spriteBatch.Draw(_mBackground, graphics.GraphicsDevice.Viewport.Bounds, Color.White);

            /* Draw the pause title */
            spriteBatch.Draw(_mTitle, new Vector2(center.X - _mTitle.Width / 2, mScreenRect.Top), Color.White);

            var currentLocation = new Vector2(mScreenRect.Left, mScreenRect.Top + _mTitle.Height);
            var height = mScreenRect.Height - _mTitle.Height;
            height -= (_mItems[0].Height + _mItems[1].Height);
            height /= 2;
            currentLocation.Y += height * 2;

            var request = "Are you sure you wish to reset your save file? \n" + "                   All save data will be lost!";

            Vector2 stringSize = _mQuartz.MeasureString(request);
            spriteBatch.DrawString(_mQuartz, request, new Vector2(mScreenRect.Center.X - stringSize.X / 2, mScreenRect.Center.Y), Color.White);
            spriteBatch.DrawString(_mQuartz, request, new Vector2(mScreenRect.Center.X - stringSize.X / 2 + 2, mScreenRect.Center.Y + 2), Color.CornflowerBlue);


            /* Draw the pause options */
            for (var i = 0; i < 2; i++)
            {
                spriteBatch.Draw(_mItems[i], new Rectangle(mScreenRect.Center.X - (_mItems[i].Width / 2), (int)currentLocation.Y, _mItems[i].Width, _mItems[i].Height), Color.White);
                currentLocation.Y += _mItems[i].Height;
            }

            spriteBatch.End();
        }
    }
}

