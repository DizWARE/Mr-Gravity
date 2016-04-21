using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MrGravity.MISC_Code;

namespace MrGravity.Menu_Code
{
    internal class Pause
    {
        #region Member Variables

        /* Spritefont */
        private SpriteFont _mKootenay;

        private Texture2D[] _mSelItems;
        private Texture2D[] _mUnselItems;
        private Texture2D[] _mItems;

        private readonly IControlScheme _mControls;

        private int _mCurrent;
        
        private Texture2D _mLoading;

        private ContentManager _mContent;

        private const int NumOptions = 4;

        #endregion

        #region Art

        private Texture2D _mPauseTitle;
        private Texture2D _mPausedTrans;

        private Texture2D _mResumeSel;
        private Texture2D _mResumeUnsel;
        private Texture2D _mSelectLevelSel;
        private Texture2D _mSelectLevelUnsel;
        private Texture2D _mMainMenuSel;
        private Texture2D _mMainMenuUnsel;
        private Texture2D _mRestartSel;
        private Texture2D _mRestartUnsel;

        #endregion

        public Pause(IControlScheme controlScheme)
        {
            _mControls = controlScheme;
        }

        public void Load(ContentManager content)
        {
            _mContent = content;

            _mKootenay = content.Load<SpriteFont>("Fonts/Kootenay");

            _mPauseTitle = content.Load<Texture2D>("Images/Menu/Pause/Paused");
            _mPausedTrans = content.Load<Texture2D>("Images/Menu/Pause/PausedTrans");

            _mCurrent = 0;

            _mSelItems = new Texture2D[NumOptions];
            _mUnselItems = new Texture2D[NumOptions];
            _mItems = new Texture2D[NumOptions];

            _mResumeSel = content.Load<Texture2D>("Images/Menu/Pause/ResumeSelected");
            _mResumeUnsel = content.Load<Texture2D>("Images/Menu/Pause/ResumeUnselected");
            _mSelectLevelSel = content.Load<Texture2D>("Images/Menu/SelectLevelSelected");
            _mSelectLevelUnsel = content.Load<Texture2D>("Images/Menu/SelectLevelUnselected");
            _mMainMenuSel = content.Load<Texture2D>("Images/Menu/MainMenuSelected");
            _mMainMenuUnsel = content.Load<Texture2D>("Images/Menu/MainMenuUnselected");
            _mRestartSel = content.Load<Texture2D>("Images/Menu/Score/RestartSelected");
            _mRestartUnsel = content.Load<Texture2D>("Images/Menu/Score/RestartUnselected");

            _mLoading = content.Load<Texture2D>("Images/Menu/LevelSelect/LoadingMenu");

            _mSelItems[0] = _mResumeSel;
            _mSelItems[1] = _mRestartSel;
            _mSelItems[2] = _mSelectLevelSel;
            _mSelItems[3] = _mMainMenuSel;

            _mUnselItems[0] = _mResumeUnsel;
            _mUnselItems[1] = _mRestartUnsel;
            _mUnselItems[2] = _mSelectLevelUnsel;
            _mUnselItems[3] = _mMainMenuUnsel;

            _mItems[0] = _mResumeSel;
            _mItems[1] = _mRestartUnsel;
            _mItems[2] = _mSelectLevelUnsel;
            _mItems[3] = _mMainMenuUnsel;
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
            if (_mControls.IsBPressed(false) || _mControls.IsBackPressed(false))
            {
                _mCurrent = 0;
                gameState = GameStates.InGame;

                _mItems[0] = _mResumeSel;
                _mItems[1] = _mRestartUnsel;
                _mItems[2] = _mSelectLevelUnsel;
                _mItems[3] = _mMainMenuUnsel;
            }
            /* If the user selects a menu item */
            if (_mControls.IsAPressed(false) || _mControls.IsStartPressed(false))
            {
                 GameSound.MenuSoundSelect.Play(GameSound.Volume, 0.0f, 0.0f);
                /* Resume Game */
                 if (_mCurrent == 0)
                     gameState = GameStates.InGame;
                 /* Restart */
                 else if (_mCurrent == 1)
                 {
                     level.ResetAll();
                     gameState = GameStates.StartLevelSplash;
                     _mCurrent = 0;
                 }
                 /* Select Level */
                 else if (_mCurrent == 2)
                 {
                     gameState = GameStates.LevelSelection;
                     level.Reset();
                     _mCurrent = 0;
                 }
                 /* Main Menu */
                 else if (_mCurrent == 3)
                 {
                     gameState = GameStates.MainMenu;
                     level.Reset();
                     _mCurrent = 0;
                 }
                 
                 _mItems[0] = _mResumeSel;
                 _mItems[1] = _mRestartUnsel;
                 _mItems[2] = _mSelectLevelUnsel;
                 _mItems[3] = _mMainMenuUnsel;
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

            var mSize = new float[2] { mScreenRect.Width / (float)graphics.GraphicsDevice.Viewport.Width, mScreenRect.Height / (float)graphics.GraphicsDevice.Viewport.Height };

            /* Draw the transparent background */
            spriteBatch.Draw(_mPausedTrans, new Rectangle(0, 0, graphics.GraphicsDevice.Viewport.Width, graphics.GraphicsDevice.Viewport.Height), Color.White); 

            /* Draw the pause title */
            spriteBatch.Draw(_mPauseTitle, new Rectangle(center.X - (int)(_mPauseTitle.Width * mSize[0]) / 2, mScreenRect.Top, (int)(_mPauseTitle.Width * mSize[0]), (int)(_mPauseTitle.Height * mSize[1])), Color.White);

            var currentLocation = new Vector2(mScreenRect.Left, mScreenRect.Top + (int)(_mPauseTitle.Height  * mSize[1]));
            var height = mScreenRect.Height - (int)(_mPauseTitle.Height  * mSize[1]);
            height -= ((int)(_mItems[0].Height * mSize[1]) + (int)(_mItems[1].Height * mSize[1]) + (int)(_mItems[2].Height * mSize[1]));
            height /= 2;
            currentLocation.Y += height;


            /* Draw the pause options */
            for (var i = 0; i < NumOptions; i++)
            {
                spriteBatch.Draw(_mItems[i], new Rectangle(mScreenRect.Center.X - ((int)(_mItems[i].Width * mSize[0]) / 2), (int)currentLocation.Y, (int)(_mItems[i].Width * mSize[0]), (int)(_mItems[i].Height * mSize[1])), Color.White);
                currentLocation.Y += (int)(_mItems[i].Height * mSize[1]);
            }
            
            spriteBatch.End();
        }
    }
}
