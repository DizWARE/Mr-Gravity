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
    public class Menu
    {
        #region Member Variables

        private SpriteFont mKootenay;

        private Texture2D[] mSelMenuItems;
        private Texture2D[] mUnselMenuItems;
        private Texture2D[] mMenuItems;

        private bool mMuted;

        private float mScreenHeight;
        private float mScreenWidth;

        enum states
        {
            TITLE,
            OPTIONS,
            CONTROLLER,
            SOUNDS,
            CREDITS,
        };

        states mState;

        private const int NUM_TITLE = 3;
        private const int NUM_LOAD = 4;
        private const int NUM_OPTIONS = 4;
        private const int NUM_SOUND = 2;

        IControlScheme mControls;

        int mCurrent;

        #endregion

        #region Menu Art

        private Texture2D mTitle;

        /* Title */
        private Texture2D mNewGameUnsel;
        private Texture2D mNewGameSel;
        private Texture2D mOptionsSel;
        private Texture2D mOptionsUnsel;
        private Texture2D mCreditsSel;
        private Texture2D mCreditsUnsel;

        /* Options Screen */
        private Texture2D mControlUnsel;
        private Texture2D mControlSel;
        private Texture2D mSoundUnsel;
        private Texture2D mSoundSel;
        private Texture2D mResetUnsel;
        private Texture2D mResetSel;

        /* Back Button */
        private Texture2D mBackUnsel;
        private Texture2D mBackSel;

        /* Controller Settings */
        private Texture2D mXboxControl;

        /* Sounds Screen */
        private Texture2D mMute;
        private Texture2D mUnMute;
        private Texture2D mMuteSel;
        private Texture2D mMuteUnsel;

        private GraphicsDeviceManager mGraphics;

        #endregion

        /*
         * Menu Contructor
         *
         * Currently does not do anything
         */
        public Menu(IControlScheme controls, GraphicsDeviceManager graphics) 
        {
            mGraphics = graphics;
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
        public void Load(ContentManager content)
        {
            mState = states.TITLE;
            mCurrent = 0;

            mMuted = false;

            mSelMenuItems = new Texture2D[NUM_TITLE];
            mUnselMenuItems = new Texture2D[NUM_TITLE];
            mMenuItems = new Texture2D[NUM_TITLE];

            mTitle = content.Load<Texture2D>("Images/Menu/Title");

            /* Title Screen */
            mNewGameSel = content.Load<Texture2D>("Images/Menu/Main/NewGameSelected");
            mNewGameUnsel = content.Load<Texture2D>("Images/Menu/Main/NewGameUnselected");
            mOptionsSel = content.Load<Texture2D>("Images/Menu/Main/OptionsSelected");
            mOptionsUnsel = content.Load<Texture2D>("Images/Menu/Main/OptionsUnselected");
            mCreditsSel = content.Load<Texture2D>("Images/Menu/Main/CreditsSelected");
            mCreditsUnsel = content.Load<Texture2D>("Images/Menu/Main/CreditsUnselected");

            /* Options Screen */
            mControlUnsel = content.Load<Texture2D>("Images/Menu/Main/ControllerUnselected");
            mControlSel = content.Load<Texture2D>("Images/Menu/Main/ControllerSelected");
            mSoundUnsel = content.Load<Texture2D>("Images/Menu/Main/SoundUnselected");
            mSoundSel = content.Load<Texture2D>("Images/Menu/Main/SoundSelected");
            mBackUnsel = content.Load<Texture2D>("Images/Menu/Main/BackUnselected");
            mBackSel = content.Load<Texture2D>("Images/Menu/Main/BackSelected");
            mResetUnsel = content.Load<Texture2D>("Images/Menu/Main/ResetUnselected");
            mResetSel = content.Load<Texture2D>("Images/Menu/Main/ResetSelected");

            /* Controller */
            mXboxControl = content.Load<Texture2D>("Images/Menu/Main/XboxController");

            /* Sound Screen */
            mMute = content.Load<Texture2D>("Images/Menu/Main/Mute");
            mUnMute = content.Load<Texture2D>("Images/Menu/Main/UnMute");
            mMuteSel = content.Load<Texture2D>("Images/Menu/Main/MuteSelected");
            mMuteUnsel = content.Load<Texture2D>("Images/Menu/Main/MuteUnselected");

            /* Initialize the menu item arrays */
            mSelMenuItems[0] = mNewGameSel;
            mSelMenuItems[1] = mOptionsSel;
            mSelMenuItems[2] = mCreditsSel;

            mUnselMenuItems[0] = mNewGameUnsel;
            mUnselMenuItems[1] = mOptionsUnsel;
            mUnselMenuItems[2] = mCreditsUnsel;

            mMenuItems[0] = mNewGameSel;
            mMenuItems[1] = mOptionsUnsel;
            mMenuItems[2] = mCreditsUnsel;

            /* Load the fonts */
            mKootenay = content.Load<SpriteFont>("Fonts/Kootenay");

            mScreenHeight = mGraphics.GraphicsDevice.Viewport.Height;
            mScreenWidth = mGraphics.GraphicsDevice.Viewport.Width;
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
            switch (mState)
            {                
                case states.TITLE:
                    /* If the user hits up */
                    if (mControls.isUpPressed(false))
                    {
                        /* If we are not on the first element already */
                        if (mCurrent > 0)
                        {
                            GameSound.menuSound_rollover.Play(GameSound.volume, 0.0f, 0.0f);
                            /* Decrement current and change the images */
                            mCurrent--;
                            for (int i = 0; i < NUM_TITLE; i++)
                                mMenuItems[i] = mUnselMenuItems[i];
                            mMenuItems[mCurrent] = mSelMenuItems[mCurrent];
                        }
                    }
                    /* If the user hits the down button */
                    if (mControls.isDownPressed(false))
                    {
                        /* If we are on the last element in the menu */
                        if (mCurrent < NUM_TITLE - 1)
                        {
                            GameSound.menuSound_rollover.Play(GameSound.volume, 0.0f, 0.0f);
                            /* Increment current and update graphics */
                            mCurrent++;
                            for (int i = 0; i < NUM_TITLE; i++)
                                mMenuItems[i] = mUnselMenuItems[i];
                            mMenuItems[mCurrent] = mSelMenuItems[mCurrent];
                        }
                    }

                    /* If the user selects one of the menu items */
                    if (mControls.isAPressed(false)||mControls.isStartPressed(false))
                    {
                        GameSound.menuSound_select.Play(GameSound.volume, 0.0f, 0.0f);
                        /* New Game */
                        if (mCurrent == 0)
                        {
                            /* Start the game */
                            gameState = GameStates.Level_Selection;

                            /* Initialize variables to the load menu items */
                            mSelMenuItems = new Texture2D[NUM_LOAD];
                            mUnselMenuItems = new Texture2D[NUM_LOAD];
                            mMenuItems = new Texture2D[NUM_LOAD];

                            mSelMenuItems[0] = mNewGameSel;
                            mSelMenuItems[1] = mOptionsSel;
                            mSelMenuItems[2] = mCreditsSel;

                            mUnselMenuItems[0] = mNewGameUnsel;
                            mUnselMenuItems[1] = mOptionsUnsel;
                            mUnselMenuItems[2] = mCreditsUnsel;

                            mMenuItems[0] = mNewGameSel;
                            mMenuItems[1] = mOptionsUnsel;
                            mMenuItems[2] = mCreditsUnsel;

                            mCurrent = 0;
                        }
                        /* Options */
                        else if (mCurrent == 1)
                        {
                            /* Change to the options menu */
                            mState = states.OPTIONS;

                            mSelMenuItems = new Texture2D[NUM_OPTIONS];
                            mUnselMenuItems = new Texture2D[NUM_OPTIONS];
                            mMenuItems = new Texture2D[NUM_OPTIONS];

                            mSelMenuItems[0] = mControlSel;
                            mSelMenuItems[1] = mSoundSel;
                            mSelMenuItems[2] = mResetSel;
                            mSelMenuItems[3] = mBackSel;

                            mUnselMenuItems[0] = mControlUnsel;
                            mUnselMenuItems[1] = mSoundUnsel;
                            mUnselMenuItems[2] = mResetUnsel;
                            mUnselMenuItems[3] = mBackUnsel;

                            mMenuItems[0] = mControlSel;
                            mMenuItems[1] = mSoundUnsel;
                            mMenuItems[2] = mResetUnsel;
                            mMenuItems[3] = mBackUnsel;

                            mCurrent = 0;
                        }
                        /* Credits */
                        else if (mCurrent == 2)
                        {
                            /* Change to the credits */
                            mState = states.CREDITS;

                            mCurrent = 0;
                        }
                    }
                    break;

                /* Options Menu*/
                case states.OPTIONS:
                    if (mControls.isUpPressed(false))
                    {
                        if (mCurrent > 0)
                        {
                            GameSound.menuSound_rollover.Play(GameSound.volume, 0.0f, 0.0f);
                            mCurrent--;
                            for (int i = 0; i < NUM_OPTIONS; i++)
                                mMenuItems[i] = mUnselMenuItems[i];
                            mMenuItems[mCurrent] = mSelMenuItems[mCurrent];
                        }
                    }
                    if (mControls.isDownPressed(false))
                    {
                        if (mCurrent < NUM_OPTIONS - 1)
                        {
                            GameSound.menuSound_rollover.Play(GameSound.volume, 0.0f, 0.0f);
                            mCurrent++;
                            for (int i = 0; i < NUM_OPTIONS; i++)
                                mMenuItems[i] = mUnselMenuItems[i];
                            mMenuItems[mCurrent] = mSelMenuItems[mCurrent];
                        }
                    }

                    if (mControls.isAPressed(false) || mControls.isStartPressed(false))
                    {
                        GameSound.menuSound_select.Play(GameSound.volume, 0.0f, 0.0f);
                        /* Controller Settings */
                        if (mCurrent == 0)
                        {
                            mState = states.CONTROLLER;
                        }
                        /* Sound Settings */
                        else if (mCurrent == 1)
                        {
                            mState = states.SOUNDS;

                            mSelMenuItems = new Texture2D[NUM_SOUND];
                            mUnselMenuItems = new Texture2D[NUM_SOUND];
                            mMenuItems = new Texture2D[NUM_SOUND];

                            mSelMenuItems[0] = mMuteSel;
                            mSelMenuItems[1] = mBackSel;

                            mUnselMenuItems[0] = mMuteUnsel;
                            mUnselMenuItems[1] = mBackUnsel;

                            mMenuItems[0] = mMuteSel;
                            mMenuItems[1] = mBackUnsel;
                   
                            mCurrent = 0;
                        }
                        /* Reset Data */
                        else if (mCurrent == 2)
                        {
                            gameState = GameStates.New_Level_Selection;
                            gameState = GameStates.Main_Menu;
                            mState = states.TITLE;

                            mSelMenuItems = new Texture2D[NUM_TITLE];
                            mUnselMenuItems = new Texture2D[NUM_TITLE];
                            mMenuItems = new Texture2D[NUM_TITLE];

                            mSelMenuItems[0] = mNewGameSel;
                            mSelMenuItems[1] = mOptionsSel;
                            mSelMenuItems[2] = mCreditsSel;

                            mUnselMenuItems[0] = mNewGameUnsel;
                            mUnselMenuItems[1] = mOptionsUnsel;
                            mUnselMenuItems[2] = mCreditsUnsel;

                            mMenuItems[0] = mNewGameSel;
                            mMenuItems[1] = mOptionsUnsel;
                            mMenuItems[2] = mCreditsUnsel;

                            mCurrent = 0;
                        }
                        /* Back */
                        else if (mCurrent == 3)
                        {
                            mState = states.TITLE;

                            mSelMenuItems = new Texture2D[NUM_TITLE];
                            mUnselMenuItems = new Texture2D[NUM_TITLE];
                            mMenuItems = new Texture2D[NUM_TITLE];

                            mSelMenuItems[0] = mNewGameSel;
                            mSelMenuItems[1] = mOptionsSel;
                            mSelMenuItems[2] = mCreditsSel;

                            mUnselMenuItems[0] = mNewGameUnsel;
                            mUnselMenuItems[1] = mOptionsUnsel;
                            mUnselMenuItems[2] = mCreditsUnsel;

                            mMenuItems[0] = mNewGameSel;
                            mMenuItems[1] = mOptionsUnsel;
                            mMenuItems[2] = mCreditsUnsel;

                            mCurrent = 0;
                        }
                    }
                    break;

                /* Credits Menu*/
                case states.CREDITS:
                    if (mControls.isAPressed(false) || mControls.isStartPressed(false))
                    {
                        GameSound.menuSound_select.Play(GameSound.volume, 0.0f, 0.0f);
                        /* Back */
                        mState = states.TITLE;

                        mSelMenuItems = new Texture2D[NUM_TITLE];
                        mUnselMenuItems = new Texture2D[NUM_TITLE];
                        mMenuItems = new Texture2D[NUM_TITLE];
                        
                        mSelMenuItems[0] = mNewGameSel;
                        mSelMenuItems[1] = mOptionsSel;
                        mSelMenuItems[2] = mCreditsSel;

                        mUnselMenuItems[0] = mNewGameUnsel;
                        mUnselMenuItems[1] = mOptionsUnsel;
                        mUnselMenuItems[2] = mCreditsUnsel;

                        mMenuItems[0] = mNewGameSel;
                        mMenuItems[1] = mOptionsUnsel;
                        mMenuItems[2] = mCreditsUnsel;

                        mCurrent = 0;
                    }
                    break;

                /* Controller Settings */
                case states.CONTROLLER:
                    if (mControls.isAPressed(false) || mControls.isStartPressed(false))
                    {
                        GameSound.menuSound_select.Play(GameSound.volume, 0.0f, 0.0f);

                        /* Change to the options menu */
                        mState = states.OPTIONS;

                        mSelMenuItems = new Texture2D[NUM_OPTIONS];
                        mUnselMenuItems = new Texture2D[NUM_OPTIONS];
                        mMenuItems = new Texture2D[NUM_OPTIONS];

                        mSelMenuItems[0] = mControlSel;
                        mSelMenuItems[1] = mSoundSel;
                        mSelMenuItems[2] = mBackSel;

                        mUnselMenuItems[0] = mControlUnsel;
                        mUnselMenuItems[1] = mSoundUnsel;
                        mUnselMenuItems[2] = mBackUnsel;

                        mMenuItems[0] = mControlSel;
                        mMenuItems[1] = mSoundUnsel;
                        mMenuItems[2] = mBackUnsel;

                        mCurrent = 0;
                    }

                    break;

                /* Sound Settings */
                case states.SOUNDS:
                    if (mControls.isUpPressed(false))
                    {
                        if (mCurrent > 0)
                        {
                            GameSound.menuSound_rollover.Play(GameSound.volume, 0.0f, 0.0f);
                            mCurrent--;
                            for (int i = 0; i < NUM_SOUND; i++)
                                mMenuItems[i] = mUnselMenuItems[i];
                            mMenuItems[mCurrent] = mSelMenuItems[mCurrent];
                        }
                    }
                    if (mControls.isDownPressed(false))
                    {
                        if (mCurrent < NUM_SOUND - 1)
                        {
                            GameSound.menuSound_rollover.Play(GameSound.volume, 0.0f, 0.0f);
                            mCurrent++;
                            for (int i = 0; i < NUM_SOUND; i++)
                                mMenuItems[i] = mUnselMenuItems[i];
                            mMenuItems[mCurrent] = mSelMenuItems[mCurrent];
                        }
                    }

                    if (mControls.isAPressed(false) || mControls.isStartPressed(false))
                    {
                        GameSound.menuSound_select.Play(GameSound.volume, 0.0f, 0.0f);
                        /* Mute */
                        if (mCurrent == 0)
                        {
                            if (mMuted == false)
                            {
                                mMuted = true;
                                GameSound.volume = 0.0f;
                            }
                            else
                            {
                                mMuted = false;
                                GameSound.volume = 1.0f;
                            }
                        }
                        /* Back */
                        else if (mCurrent == 1)
                        {
                            /* Return back to the title screen */
                            mState = states.TITLE;

                            mSelMenuItems = new Texture2D[NUM_TITLE];
                            mUnselMenuItems = new Texture2D[NUM_TITLE];
                            mMenuItems = new Texture2D[NUM_TITLE];

                            mSelMenuItems[0] = mNewGameSel;
                            mSelMenuItems[1] = mOptionsSel;
                            mSelMenuItems[2] = mCreditsSel;

                            mUnselMenuItems[0] = mNewGameUnsel;
                            mUnselMenuItems[1] = mOptionsUnsel;
                            mUnselMenuItems[2] = mCreditsUnsel;

                            mMenuItems[0] = mNewGameSel;
                            mMenuItems[1] = mOptionsUnsel;
                            mMenuItems[2] = mCreditsUnsel;

                            mCurrent = 0;
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
        public void Draw(SpriteBatch spriteBatch, GraphicsDeviceManager graphics)
        {
            spriteBatch.Begin();

            /* Draw the title of the game  and main background */
            spriteBatch.Draw(mTitle, new Vector2(mScreenWidth / 10, mScreenHeight / 20), Color.White);

            /* If on the title screen */
            switch (mState)
            {
                case states.TITLE:
                    /* Draw the title items */
                    spriteBatch.Draw(mMenuItems[0], new Vector2(mScreenWidth / 4, mScreenHeight / 2 + 100), Color.White);
                    spriteBatch.Draw(mMenuItems[1], new Vector2(mScreenWidth / 4, mScreenHeight / 2 + 175), Color.White);
                    spriteBatch.Draw(mMenuItems[2], new Vector2(mScreenWidth / 4, mScreenHeight / 2 + 250), Color.White);
                    break;

                /* If on the options menu */
                case states.OPTIONS:
                    spriteBatch.Draw(mMenuItems[0], new Vector2(mScreenWidth / 4, mScreenHeight / 2 -100), Color.White);
                    spriteBatch.Draw(mMenuItems[1], new Vector2(mScreenWidth / 4, mScreenHeight / 2 -25), Color.White);
                    spriteBatch.Draw(mMenuItems[2], new Vector2(mScreenWidth / 4, mScreenHeight / 2 + 50), Color.White);
                    spriteBatch.Draw(mMenuItems[3], new Vector2(mScreenWidth - 300, mScreenHeight / 2 + 250), Color.White);
                    break;

                case states.CREDITS:
                    spriteBatch.DrawString(mKootenay, "Developed By:", new Vector2(400.0f, 375.0f), Color.White);
                    spriteBatch.DrawString(mKootenay, "Lukas Black", new Vector2(400.0f, 425.0f), Color.White);
                    spriteBatch.DrawString(mKootenay, "Nate Bradford", new Vector2(400.0f, 450.0f), Color.White);
                    spriteBatch.DrawString(mKootenay, "Curtis Taylor", new Vector2(400.0f, 475.0f), Color.White);
                    spriteBatch.DrawString(mKootenay, "Steven Doxey", new Vector2(400.0f, 500.0f), Color.White);
                    spriteBatch.DrawString(mKootenay, "Kamron Egan", new Vector2(400.0f, 525.0f), Color.White);
                    spriteBatch.DrawString(mKootenay, "Jeremy Heintz", new Vector2(400.0f, 550.0f), Color.White);
                    spriteBatch.DrawString(mKootenay, "Morgan Reynolds", new Vector2(400.0f, 575.0f), Color.White);
                    spriteBatch.DrawString(mKootenay, "Tyler Robinson", new Vector2(400.0f, 600.0f), Color.White);
                    spriteBatch.DrawString(mKootenay, "Casey Spencer", new Vector2(400.0f, 625.0f), Color.White);

                    spriteBatch.Draw(mBackSel, new Vector2(mScreenWidth - 300, mScreenHeight / 2 + 250), Color.White);

                    break;

                /* If on the controller settings screen */
                case states.CONTROLLER:
                    spriteBatch.Draw(mXboxControl, new Vector2(mScreenWidth / 5, mScreenHeight / 3), Color.White);
                    spriteBatch.Draw(mBackSel, new Vector2(mScreenWidth - 300, mScreenHeight / 2 + 250), Color.White);
                    /* TODO */
                    break;

                /* If on the sound settings screen */
                case states.SOUNDS:

                    spriteBatch.Draw(mMenuItems[0], new Vector2(mScreenWidth / 20, mScreenHeight / 2), Color.White);
                    if (mMuted)
                        spriteBatch.Draw(mMute, new Vector2(mScreenWidth / 3, mScreenHeight / 2 - 5), Color.White);
                    else
                        spriteBatch.Draw(mUnMute, new Vector2(mScreenWidth / 3, mScreenHeight / 2 - 5), Color.White);
                    spriteBatch.Draw(mMenuItems[1], new Vector2(mScreenWidth - 300, mScreenHeight / 2 + 250), Color.White);
                    /* TODO */
                    break;
            }
            spriteBatch.End();
        }
    }
}