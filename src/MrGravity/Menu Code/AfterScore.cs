using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MrGravity.MISC_Code;

namespace MrGravity.Menu_Code
{
    internal class AfterScore
    {
        #region Member Variables

        private Rectangle _mScreenRect;

        private ContentManager _mContent;

        private readonly IControlScheme _mControls;

        private Texture2D[] _mSelItems;
        private Texture2D[] _mUnselItems;
        private Texture2D[] _mItems;

        private int _mCurrent;

        private const int NumOptions = 3;

        #endregion

        #region Art

        private Texture2D _mNextLevelSel;
        private Texture2D _mNextLevelUnsel;
        private Texture2D _mSelectLevelSel;
        private Texture2D _mSelectLevelUnsel;
        private Texture2D _mMainMenuSel;
        private Texture2D _mMainMenuUnsel;
        private Texture2D _mRestartSel;
        private Texture2D _mRestartUnsel;

        private Texture2D _mTitle;
        private Texture2D _mTrans;

        #endregion

        public AfterScore(IControlScheme controls)
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

            _mCurrent = 0;

            _mScreenRect = graphics.Viewport.TitleSafeArea;

            _mSelItems = new Texture2D[NumOptions];
            _mUnselItems = new Texture2D[NumOptions];
            _mItems = new Texture2D[NumOptions];

            _mNextLevelSel = content.Load<Texture2D>("Images/Menu/Score/NextLevelSelected");
            _mNextLevelUnsel = content.Load<Texture2D>("Images/Menu/Score/NextLevelUnselected");

            _mSelectLevelSel = content.Load<Texture2D>("Images/Menu/SelectLevelSelected");
            _mSelectLevelUnsel = content.Load<Texture2D>("Images/Menu/SelectLevelUnselected");

            _mMainMenuSel = content.Load<Texture2D>("Images/Menu/MainMenuSelected");
            _mMainMenuUnsel = content.Load<Texture2D>("Images/Menu/MainMenuUnselected");

            _mRestartUnsel = content.Load<Texture2D>("Images/Menu/Score/RestartUnselected");
            _mRestartSel = content.Load<Texture2D>("Images/Menu/Score/RestartSelected");

            _mTitle = content.Load<Texture2D>("Images/Menu/Mr_Gravity");

            _mTrans = content.Load<Texture2D>("Images/Menu/Pause/PausedTrans");

            _mSelItems[0] = _mSelectLevelSel;
            _mSelItems[1] = _mRestartSel;
            _mSelItems[2] = _mMainMenuSel;

            _mUnselItems[0] = _mSelectLevelUnsel;
            _mUnselItems[1] = _mRestartUnsel;
            _mUnselItems[2] = _mMainMenuUnsel;

            _mItems[0] = _mSelectLevelSel;
            _mItems[1] = _mRestartUnsel;
            _mItems[2] = _mMainMenuUnsel;
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

            /* If the user selects one of the menu items */
            if (_mControls.IsAPressed(false) || _mControls.IsStartPressed(false))
            {
                level.MTimer = 0;
                GameSound.MenuSoundSelect.Play(GameSound.Volume, 0.0f, 0.0f);

                /* Level Select */
                if (_mCurrent == 0)
                {
                    /*Back To Level Selection*/
                    gameState = GameStates.LevelSelection;

                    _mCurrent = 0;

                    _mItems[0] = _mSelectLevelSel;
                    _mItems[1] = _mRestartUnsel;
                    _mItems[2] = _mMainMenuUnsel;
                }

                /* Restart Level */
                else if (_mCurrent == 1)
                {

                    /* Start the game*/
                    gameState = GameStates.StartLevelSplash;
                    level.ResetAll();
                    _mCurrent = 0;

                    _mItems[0] = _mSelectLevelSel;
                    _mItems[1] = _mRestartUnsel;
                    _mItems[2] = _mMainMenuUnsel;
                }

                /* Main Menu */
                else if (_mCurrent == 2)
                {
                    gameState = GameStates.MainMenu;

                    _mCurrent = 0;

                    _mItems[0] = _mSelectLevelSel;
                    _mItems[1] = _mRestartUnsel;
                    _mItems[2] = _mMainMenuUnsel;
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

            var mSize = new float[2] { _mScreenRect.Width / (float)graphics.GraphicsDevice.Viewport.Width, _mScreenRect.Height / (float)graphics.GraphicsDevice.Viewport.Height };

            spriteBatch.Draw(_mTrans, graphics.GraphicsDevice.Viewport.Bounds, Color.White);

            spriteBatch.Draw(_mTitle, new Rectangle(_mScreenRect.Center.X - (int)(_mTitle.Width * mSize[0]) / 2, _mScreenRect.Top, (int)(_mTitle.Width * mSize[0]), (int)(_mTitle.Height * mSize[1])), Color.White);

            var currentLocation = new Vector2(_mScreenRect.Left, _mScreenRect.Top + (int)(_mTitle.Height * mSize[1]));
            var height = _mScreenRect.Height - (int)(_mTitle.Height * mSize[1]);
            height -= ((int)(_mItems[0].Height * mSize[1]) + (int)(_mItems[1].Height * mSize[1]) + (int)(_mItems[2].Height * mSize[1]));
            height /= 2;
            currentLocation.Y += height;
            for (var i = 0; i < NumOptions; i++)
            {
                spriteBatch.Draw(_mItems[i], new Rectangle(_mScreenRect.Center.X - ((int)(_mItems[i].Width * mSize[0]) / 2), (int)currentLocation.Y, (int)(_mItems[i].Width * mSize[0]), (int)(_mItems[i].Height * mSize[1])), Color.White);
                currentLocation.Y += (int)(_mItems[i].Height * mSize[1]);
            }

            spriteBatch.End();
        }
    }
}
