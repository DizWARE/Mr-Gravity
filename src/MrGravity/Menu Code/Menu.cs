using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MrGravity.MISC_Code;

namespace MrGravity.Menu_Code
{
    public class Menu
    {
        #region Member Variables

        private SpriteFont _mKootenay;

        private Texture2D[] _mSelMenuItems;
        private Texture2D[] _mUnselMenuItems;
        private Texture2D[] _mMenuItems;

        private bool _mMuted;

        private float _mScreenHeight;
        private float _mScreenWidth;

        private enum States
        {
            Title,
            Options,
            Controller,
            Sounds,
            Credits
        }

        private States _mState;

        private const int NumTitle = 3;
        private const int NumLoad = 4;
        private const int NumOptions = 4;
        private const int NumSound = 2;

        private readonly IControlScheme _mControls;

        private int _mCurrent;

        #endregion

        #region Menu Art

        private Texture2D _mTitle;

        /* Title */
        private Texture2D _mNewGameUnsel;
        private Texture2D _mNewGameSel;
        private Texture2D _mOptionsSel;
        private Texture2D _mOptionsUnsel;
        private Texture2D _mCreditsSel;
        private Texture2D _mCreditsUnsel;

        /* Options Screen */
        private Texture2D _mControlUnsel;
        private Texture2D _mControlSel;
        private Texture2D _mSoundUnsel;
        private Texture2D _mSoundSel;
        private Texture2D _mResetUnsel;
        private Texture2D _mResetSel;

        /* Back Button */
        private Texture2D _mBackUnsel;
        private Texture2D _mBackSel;

        /* Controller Settings */
        private Texture2D _mXboxControl;

        /* Sounds Screen */
        private Texture2D _mMute;
        private Texture2D _mUnMute;
        private Texture2D _mMuteSel;
        private Texture2D _mMuteUnsel;

        private readonly GraphicsDeviceManager _mGraphics;

        /* Title Safe Area */
        private Rectangle _mScreenRect;

        #endregion

        /*
         * Menu Contructor
         *
         * Currently does not do anything
         */
        public Menu(IControlScheme controls, GraphicsDeviceManager graphics) 
        {
            _mGraphics = graphics;
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
            _mState = States.Title;
            _mCurrent = 0;

            _mScreenRect = graphics.Viewport.TitleSafeArea;

            _mMuted = false;

            _mSelMenuItems = new Texture2D[NumTitle];
            _mUnselMenuItems = new Texture2D[NumTitle];
            _mMenuItems = new Texture2D[NumTitle];

            _mTitle = content.Load<Texture2D>("Images/Menu/Mr_Gravity");

            /* Title Screen */
            _mNewGameSel = content.Load<Texture2D>("Images/Menu/Main/NewGameSelected");
            _mNewGameUnsel = content.Load<Texture2D>("Images/Menu/Main/NewGameUnselected");
            _mOptionsSel = content.Load<Texture2D>("Images/Menu/Main/OptionsSelected");
            _mOptionsUnsel = content.Load<Texture2D>("Images/Menu/Main/OptionsUnselected");
            _mCreditsSel = content.Load<Texture2D>("Images/Menu/Main/CreditsSelected");
            _mCreditsUnsel = content.Load<Texture2D>("Images/Menu/Main/CreditsUnselected");

            /* Options Screen */
            _mControlUnsel = content.Load<Texture2D>("Images/Menu/Main/ControllerUnselected");
            _mControlSel = content.Load<Texture2D>("Images/Menu/Main/ControllerSelected");
            _mSoundUnsel = content.Load<Texture2D>("Images/Menu/Main/SoundUnselected");
            _mSoundSel = content.Load<Texture2D>("Images/Menu/Main/SoundSelected");
            _mBackUnsel = content.Load<Texture2D>("Images/Menu/Main/BackUnselected");
            _mBackSel = content.Load<Texture2D>("Images/Menu/Main/BackSelected");
            _mResetUnsel = content.Load<Texture2D>("Images/Menu/Main/ResetUnselected");
            _mResetSel = content.Load<Texture2D>("Images/Menu/Main/ResetSelected");

            /* Controller */
            _mXboxControl = content.Load<Texture2D>("Images/Menu/Main/XboxController");

            /* Sound Screen */
            _mMute = content.Load<Texture2D>("Images/Menu/Main/Mute");
            _mUnMute = content.Load<Texture2D>("Images/Menu/Main/UnMute");
            _mMuteSel = content.Load<Texture2D>("Images/Menu/Main/MuteSelected");
            _mMuteUnsel = content.Load<Texture2D>("Images/Menu/Main/MuteUnselected");

            /* Initialize the menu item arrays */
            _mSelMenuItems[0] = _mNewGameSel;
            _mSelMenuItems[1] = _mOptionsSel;
            _mSelMenuItems[2] = _mCreditsSel;

            _mUnselMenuItems[0] = _mNewGameUnsel;
            _mUnselMenuItems[1] = _mOptionsUnsel;
            _mUnselMenuItems[2] = _mCreditsUnsel;

            _mMenuItems[0] = _mNewGameSel;
            _mMenuItems[1] = _mOptionsUnsel;
            _mMenuItems[2] = _mCreditsUnsel;

            /* Load the fonts */
            _mKootenay = content.Load<SpriteFont>("Fonts/Kootenay");

            _mScreenHeight = _mGraphics.GraphicsDevice.Viewport.Height;
            _mScreenWidth = _mGraphics.GraphicsDevice.Viewport.Width;
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
            /* If we are on the title screen */
            switch (_mState)
            {                
                case States.Title:
                    /* If the user hits up */
                    if (_mControls.IsUpPressed(false))
                    {
                        /* If we are not on the first element already */
                        if (_mCurrent > 0)
                        {
                            GameSound.MenuSoundRollover.Play(GameSound.Volume, 0.0f, 0.0f);
                            /* Decrement current and change the images */
                            _mCurrent--;
                            for (var i = 0; i < NumTitle; i++)
                                _mMenuItems[i] = _mUnselMenuItems[i];
                            _mMenuItems[_mCurrent] = _mSelMenuItems[_mCurrent];
                        }
                    }
                    /* If the user hits the down button */
                    if (_mControls.IsDownPressed(false))
                    {
                        /* If we are on the last element in the menu */
                        if (_mCurrent < NumTitle - 1)
                        {
                            GameSound.MenuSoundRollover.Play(GameSound.Volume, 0.0f, 0.0f);
                            /* Increment current and update graphics */
                            _mCurrent++;
                            for (var i = 0; i < NumTitle; i++)
                                _mMenuItems[i] = _mUnselMenuItems[i];
                            _mMenuItems[_mCurrent] = _mSelMenuItems[_mCurrent];
                        }
                    }

                    /* If the user selects one of the menu items */
                    if (_mControls.IsAPressed(false)||_mControls.IsStartPressed(false))
                    {
                        GameSound.MenuSoundSelect.Play(GameSound.Volume, 0.0f, 0.0f);
                        /* New Game */
                        if (_mCurrent == 0)
                        {
                            /* Start the game */
                            gameState = GameStates.LevelSelection;

                            /* Initialize variables to the title items */
                            _mSelMenuItems = new Texture2D[NumTitle];
                            _mUnselMenuItems = new Texture2D[NumTitle];
                            _mMenuItems = new Texture2D[NumTitle];

                            _mSelMenuItems[0] = _mNewGameSel;
                            _mSelMenuItems[1] = _mOptionsSel;
                            _mSelMenuItems[2] = _mCreditsSel;

                            _mUnselMenuItems[0] = _mNewGameUnsel;
                            _mUnselMenuItems[1] = _mOptionsUnsel;
                            _mUnselMenuItems[2] = _mCreditsUnsel;

                            _mMenuItems[0] = _mNewGameSel;
                            _mMenuItems[1] = _mOptionsUnsel;
                            _mMenuItems[2] = _mCreditsUnsel;

                            _mCurrent = 0;
                        }

                        /* Options */
                        else if (_mCurrent == 1)
                        {
                            /* Change to the options menu */
                            _mState = States.Options;

                            _mSelMenuItems = new Texture2D[NumOptions];
                            _mUnselMenuItems = new Texture2D[NumOptions];
                            _mMenuItems = new Texture2D[NumOptions];

                            _mSelMenuItems[0] = _mControlSel;
                            _mSelMenuItems[1] = _mSoundSel;
                            _mSelMenuItems[2] = _mResetSel;
                            _mSelMenuItems[3] = _mBackSel;

                            _mUnselMenuItems[0] = _mControlUnsel;
                            _mUnselMenuItems[1] = _mSoundUnsel;
                            _mUnselMenuItems[2] = _mResetUnsel;
                            _mUnselMenuItems[3] = _mBackUnsel;

                            _mMenuItems[0] = _mControlSel;
                            _mMenuItems[1] = _mSoundUnsel;
                            _mMenuItems[2] = _mResetUnsel;
                            _mMenuItems[3] = _mBackUnsel;

                            _mCurrent = 0;
                        }
                        /* Credits */
                        else if (_mCurrent == 2)
                        {
                            /* Change to the credits */
                            _mState = States.Credits;

                            _mCurrent = 0;
                        }
                    }
                    break;

                /* Options Menu*/
                case States.Options:
                    if (_mControls.IsUpPressed(false))
                    {
                        if (_mCurrent > 0)
                        {
                            GameSound.MenuSoundRollover.Play(GameSound.Volume, 0.0f, 0.0f);
                            _mCurrent--;
                            for (var i = 0; i < NumOptions; i++)
                                _mMenuItems[i] = _mUnselMenuItems[i];
                            _mMenuItems[_mCurrent] = _mSelMenuItems[_mCurrent];
                        }
                    }
                    if (_mControls.IsDownPressed(false))
                    {
                        if (_mCurrent < NumOptions - 1)
                        {
                            GameSound.MenuSoundRollover.Play(GameSound.Volume, 0.0f, 0.0f);
                            _mCurrent++;
                            for (var i = 0; i < NumOptions; i++)
                                _mMenuItems[i] = _mUnselMenuItems[i];
                            _mMenuItems[_mCurrent] = _mSelMenuItems[_mCurrent];
                        }
                    }

                    if (_mControls.IsAPressed(false) || _mControls.IsStartPressed(false))
                    {
                        GameSound.MenuSoundSelect.Play(GameSound.Volume, 0.0f, 0.0f);
                        /* Controller Settings */
                        if (_mCurrent == 0)
                        {
                            _mState = States.Controller;
                        }
                        /* Sound Settings */
                        else if (_mCurrent == 1)
                        {
                            _mState = States.Sounds;

                            _mSelMenuItems = new Texture2D[NumSound];
                            _mUnselMenuItems = new Texture2D[NumSound];
                            _mMenuItems = new Texture2D[NumSound];

                            _mSelMenuItems[0] = _mMuteSel;
                            _mSelMenuItems[1] = _mBackSel;

                            _mUnselMenuItems[0] = _mMuteUnsel;
                            _mUnselMenuItems[1] = _mBackUnsel;

                            _mMenuItems[0] = _mMuteSel;
                            _mMenuItems[1] = _mBackUnsel;
                   
                            _mCurrent = 0;
                        }
                        /* Reset Data */
                        else if (_mCurrent == 2)
                        {
                            /* Change to the load screen */
                            gameState = GameStates.NewLevelSelection;

                            /* Initialize variables to the title items */
                            _mSelMenuItems = new Texture2D[NumTitle];
                            _mUnselMenuItems = new Texture2D[NumTitle];
                            _mMenuItems = new Texture2D[NumTitle];

                            _mSelMenuItems[0] = _mNewGameSel;
                            _mSelMenuItems[1] = _mOptionsSel;
                            _mSelMenuItems[2] = _mCreditsSel;

                            _mUnselMenuItems[0] = _mNewGameUnsel;
                            _mUnselMenuItems[1] = _mOptionsUnsel;
                            _mUnselMenuItems[2] = _mCreditsUnsel;

                            _mMenuItems[0] = _mNewGameSel;
                            _mMenuItems[1] = _mOptionsUnsel;
                            _mMenuItems[2] = _mCreditsUnsel;

                            _mCurrent = 0;

                            _mState = States.Title;
                        }
                        /* Back */
                        else if (_mCurrent == 3)
                        {
                            _mState = States.Title;

                            _mSelMenuItems = new Texture2D[NumTitle];
                            _mUnselMenuItems = new Texture2D[NumTitle];
                            _mMenuItems = new Texture2D[NumTitle];

                            _mSelMenuItems[0] = _mNewGameSel;
                            _mSelMenuItems[1] = _mOptionsSel;
                            _mSelMenuItems[2] = _mCreditsSel;

                            _mUnselMenuItems[0] = _mNewGameUnsel;
                            _mUnselMenuItems[1] = _mOptionsUnsel;
                            _mUnselMenuItems[2] = _mCreditsUnsel;

                            _mMenuItems[0] = _mNewGameSel;
                            _mMenuItems[1] = _mOptionsUnsel;
                            _mMenuItems[2] = _mCreditsUnsel;

                            _mCurrent = 0;
                        }
                    }
                    break;

                /* Credits Menu*/
                case States.Credits:
                    if (_mControls.IsAPressed(false) || _mControls.IsStartPressed(false))
                    {
                        GameSound.MenuSoundSelect.Play(GameSound.Volume, 0.0f, 0.0f);
                        /* Back */
                        _mState = States.Title;

                        _mSelMenuItems = new Texture2D[NumTitle];
                        _mUnselMenuItems = new Texture2D[NumTitle];
                        _mMenuItems = new Texture2D[NumTitle];
                        
                        _mSelMenuItems[0] = _mNewGameSel;
                        _mSelMenuItems[1] = _mOptionsSel;
                        _mSelMenuItems[2] = _mCreditsSel;

                        _mUnselMenuItems[0] = _mNewGameUnsel;
                        _mUnselMenuItems[1] = _mOptionsUnsel;
                        _mUnselMenuItems[2] = _mCreditsUnsel;

                        _mMenuItems[0] = _mNewGameSel;
                        _mMenuItems[1] = _mOptionsUnsel;
                        _mMenuItems[2] = _mCreditsUnsel;

                        _mCurrent = 0;
                    }
                    break;

                /* Controller Settings */
                case States.Controller:
                    if (_mControls.IsAPressed(false) || _mControls.IsStartPressed(false))
                    {
                        GameSound.MenuSoundSelect.Play(GameSound.Volume, 0.0f, 0.0f);

                        /* Change to the options menu */
                        _mState = States.Options;

                        _mSelMenuItems = new Texture2D[NumOptions];
                        _mUnselMenuItems = new Texture2D[NumOptions];
                        _mMenuItems = new Texture2D[NumOptions];

                        _mSelMenuItems[0] = _mControlSel;
                        _mSelMenuItems[1] = _mSoundSel;
                        _mSelMenuItems[2] = _mResetSel;
                        _mSelMenuItems[3] = _mBackSel;

                        _mUnselMenuItems[0] = _mControlUnsel;
                        _mUnselMenuItems[1] = _mSoundUnsel;
                        _mUnselMenuItems[2] = _mResetUnsel;
                        _mUnselMenuItems[3] = _mBackUnsel;

                        _mMenuItems[0] = _mControlSel;
                        _mMenuItems[1] = _mSoundUnsel;
                        _mMenuItems[2] = _mResetUnsel;
                        _mMenuItems[3] = _mBackUnsel;

                        _mCurrent = 0;
                    }

                    break;

                /* Sound Settings */
                case States.Sounds:
                    if (_mControls.IsUpPressed(false))
                    {
                        if (_mCurrent > 0)
                        {
                            GameSound.MenuSoundRollover.Play(GameSound.Volume, 0.0f, 0.0f);
                            _mCurrent--;
                            for (var i = 0; i < NumSound; i++)
                                _mMenuItems[i] = _mUnselMenuItems[i];
                            _mMenuItems[_mCurrent] = _mSelMenuItems[_mCurrent];
                        }
                    }
                    if (_mControls.IsDownPressed(false))
                    {
                        if (_mCurrent < NumSound - 1)
                        {
                            GameSound.MenuSoundRollover.Play(GameSound.Volume, 0.0f, 0.0f);
                            _mCurrent++;
                            for (var i = 0; i < NumSound; i++)
                                _mMenuItems[i] = _mUnselMenuItems[i];
                            _mMenuItems[_mCurrent] = _mSelMenuItems[_mCurrent];
                        }
                    }

                    if (_mControls.IsAPressed(false) || _mControls.IsStartPressed(false))
                    {
                        GameSound.MenuSoundSelect.Play(GameSound.Volume, 0.0f, 0.0f);
                        /* Mute */
                        if (_mCurrent == 0)
                        {
                            if (_mMuted == false)
                            {
                                _mMuted = true;
                                GameSound.Volume = 0.0f;
                            }
                            else
                            {
                                _mMuted = false;
                                GameSound.Volume = 1.0f;
                            }
                        }
                        /* Back */
                        else if (_mCurrent == 1)
                        {
                            /* Change to the options menu */
                            _mState = States.Options;

                            _mSelMenuItems = new Texture2D[NumOptions];
                            _mUnselMenuItems = new Texture2D[NumOptions];
                            _mMenuItems = new Texture2D[NumOptions];

                            _mSelMenuItems[0] = _mControlSel;
                            _mSelMenuItems[1] = _mSoundSel;
                            _mSelMenuItems[2] = _mResetSel;
                            _mSelMenuItems[3] = _mBackSel;

                            _mUnselMenuItems[0] = _mControlUnsel;
                            _mUnselMenuItems[1] = _mSoundUnsel;
                            _mUnselMenuItems[2] = _mResetUnsel;
                            _mUnselMenuItems[3] = _mBackUnsel;

                            _mMenuItems[0] = _mControlSel;
                            _mMenuItems[1] = _mSoundUnsel;
                            _mMenuItems[2] = _mResetUnsel;
                            _mMenuItems[3] = _mBackUnsel;

                            _mCurrent = 0;
                        }
                    }
                    break;
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

            /* Draw the title of the game  and main background */
            //spriteBatch.Draw(mTitle, new Vector2(180.0f, 50.0f), Color.White);
            spriteBatch.Draw(_mTitle, new Vector2(_mScreenRect.Left + (_mScreenRect.Width - _mTitle.Width) / 2, _mScreenRect.Top), Color.White);

            /* If on the title screen */
            switch (_mState)
            {
                case States.Title:
                    /* Draw the title items */
                    float height = _mMenuItems[2].Height;
                    for (var i = _mMenuItems.Length - 1; i >= 0; i--)
                    {
                        height += _mMenuItems[i].Height;
                        spriteBatch.Draw(_mMenuItems[i], new Vector2(_mScreenRect.Center.X - _mMenuItems[i].Width / 2, _mScreenRect.Bottom - height), Color.White);
                    }
                    //spriteBatch.Draw(mMenuItems[3], new Vector2(mScreenRect.Left + (mScreenRect.Width - 
                    break;

                /* If on the options menu */
                case States.Options:
                    height = _mScreenRect.Center.Y - _mMenuItems[2].Height*3;
                    for (var i = _mMenuItems.Length - 1; i >= 0; i--)
                    {
                        height += _mMenuItems[i].Height;
                        spriteBatch.Draw(_mMenuItems[i], new Vector2(_mScreenRect.Center.X - _mMenuItems[i].Width / 2, _mScreenRect.Bottom - height), Color.White);
                    }
                    break;

                case States.Credits:
                    var text = "Casey Spencer";
                    Vector2 size = _mKootenay.MeasureString(text);
                    height = _mScreenRect.Bottom - _mBackSel.Height * 2.5f;
                    spriteBatch.DrawString(_mKootenay, text, new Vector2(_mScreenRect.Center.X - (size.X / 2), height), Color.White);
                    text = "Tyler Robinson";
                    size = _mKootenay.MeasureString(text);
                    height -= size.Y;
                    spriteBatch.DrawString(_mKootenay, text, new Vector2(_mScreenRect.Center.X - (size.X / 2), height), Color.White);
                    text = "Morgan Reynolds";
                    size = _mKootenay.MeasureString(text);
                    height -= size.Y;
                    spriteBatch.DrawString(_mKootenay, text, new Vector2(_mScreenRect.Center.X - (size.X / 2), height), Color.White);
                    text = "Jeremy Heintz";
                    size = _mKootenay.MeasureString(text);
                    height -= size.Y;
                    spriteBatch.DrawString(_mKootenay, text, new Vector2(_mScreenRect.Center.X - (size.X / 2), height), Color.White);
                    text = "Kamron Egan";
                    size = _mKootenay.MeasureString(text);
                    height -= size.Y;
                    spriteBatch.DrawString(_mKootenay, text, new Vector2(_mScreenRect.Center.X - (size.X / 2), height), Color.White);
                    text = "Steven Doxley";
                    size = _mKootenay.MeasureString(text);
                    height -= size.Y;
                    spriteBatch.DrawString(_mKootenay, text, new Vector2(_mScreenRect.Center.X - (size.X / 2), height), Color.White);
                    text = "Curtis Taylor";
                    size = _mKootenay.MeasureString(text);
                    height -= size.Y;
                    spriteBatch.DrawString(_mKootenay, text, new Vector2(_mScreenRect.Center.X - (size.X / 2), height), Color.White);
                    text = "Nate Bradford";
                    size = _mKootenay.MeasureString(text);
                    height -= size.Y;
                    spriteBatch.DrawString(_mKootenay, text, new Vector2(_mScreenRect.Center.X - (size.X / 2), height), Color.White);
                    text = "Lukas Black";
                    size = _mKootenay.MeasureString(text);
                    height -= size.Y;
                    spriteBatch.DrawString(_mKootenay, text, new Vector2(_mScreenRect.Center.X - (size.X / 2), height), Color.White);
                    text = "Developed By:";
                    size = _mKootenay.MeasureString(text);
                    height -= size.Y * 2;
                    spriteBatch.DrawString(_mKootenay, text, new Vector2(_mScreenRect.Center.X - (size.X / 2), height), Color.White);

                    spriteBatch.Draw(_mBackSel, new Vector2(_mScreenRect.Center.X - _mBackSel.Width / 2, _mScreenRect.Bottom - _mBackSel.Height * 2), Color.White);
                    break;

                /* If on the controller settings screen */
                case States.Controller:
                    spriteBatch.Draw(_mXboxControl, new Vector2(_mScreenRect.Center.X - _mXboxControl.Width / 2, _mScreenRect.Center.Y - _mXboxControl.Height / 3), Color.White);
                    spriteBatch.Draw(_mBackSel, new Vector2(_mScreenRect.Center.X - _mBackSel.Width / 2, _mScreenRect.Bottom - _mBackSel.Height), Color.White);
                    /* TODO */
                    break;

                /* If on the sound settings screen */
                case States.Sounds:

                    height = _mScreenRect.Center.Y - _mMenuItems[0].Height*2;
                    var offset = 0;
                    for (var i = _mMenuItems.Length - 1; i >= 0; i--)
                    {
                        height += _mMenuItems[i].Height;
                        spriteBatch.Draw(_mMenuItems[i], new Vector2(_mScreenRect.Center.X - (_mMenuItems[i].Width) / 2  - offset, _mScreenRect.Bottom - height), Color.White);
                        offset = _mMute.Width / 2;
                    }
                    //spriteBatch.Draw(mMenuItems[0], new Vector2(300.0f, 300.0f), Color.White);

                    height = _mScreenRect.Center.Y;
                    if (_mMuted)
                        spriteBatch.Draw(_mMute, new Vector2(_mScreenRect.Center.X + (_mMenuItems[0].Width) / 2, _mScreenRect.Bottom - height), Color.White);
                    else
                        spriteBatch.Draw(_mUnMute, new Vector2(_mScreenRect.Center.X + (_mMenuItems[0].Width) / 2, _mScreenRect.Bottom - height), Color.White);
                    //spriteBatch.Draw(mMenuItems[1], new Vector2(900.0f, 700.0f), Color.White);
                    /* TODO */
                    break;
            }
            spriteBatch.End();
        }
    }
}