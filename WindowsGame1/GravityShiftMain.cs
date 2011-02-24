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
using GravityShift.Import_Code;
using GravityShift.Game_Objects.Static_Objects.Triggers; 
using GravityShift.MISC_Code;

namespace GravityShift
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class GravityShiftMain : Microsoft.Xna.Framework.Game
    {
        public static GraphicsDeviceManager mGraphics;
        SpriteBatch mSpriteBatch;

        // Scale - Used to make sure the HUD is drawn based on the screen size
        public Matrix scale;

        Title mTitle;

        //Instance of the Menu class
        Menu mMenu;

        // Instance of the AfterScore class
        AfterScore mAfterScore;

        ResetConfirm mResetConfirm;

        StartLevelSplash mStartLevelSplash;

        MainMenu mMainMenu;
        Level mMainMenuLevel;

        //Instance of the scoring class
        Scoring mScoring;

        //Instance of the level selection class
        //LevelSelect mLevelSelect;

        WorldSelect mWorldSelect;

        //Instance of the pause class
        Pause mPause;

        Options mOptions;
        Credits mCredits;

        //Instance of the Controller class
        Controller mController;

        SoundOptions mSoundOptions;
		
        private GameStates mCurrentState = GameStates.Title;

        //Max duration of a sequence
        private static int VICTORY_DURATION = 120;
        private static int DEATH_DURATION = 50;

        //Duration of a sequence
        private int mSequence = 0;

        //Boolean toggle variable
        private bool mToggledSequence = false;

        private IControlScheme mControls;

        //Current level
        Level mCurrentLevel;

        //Fonts for this game
        SpriteFont mDefaultFont;
        SpriteFont mQuartz;

        //Transparant HUD
        Texture2D mHUDTrans;

        //Lives
        private Texture2D[] mLives;

        //TO BE CHANGED- Actually, this may be ok since we use this to play test.
        public string LevelLocation { get { return mLevelLocation; } set { mLevelLocation = "..\\..\\..\\Content\\Levels\\" + value; } }        
        private string mLevelLocation = "..\\..\\..\\Content\\Levels\\DefaultLevel.xml";

        private bool mCheckedForSave = false;

        public GravityShiftMain()
        {
            Components.Add(new GamerServicesComponent(this));
            mGraphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

#if XBOX360
            mControls = new ControllerControl();
#else
            if (GamePad.GetState(PlayerIndex.One).IsConnected || GamePad.GetState(PlayerIndex.Two).IsConnected ||
                GamePad.GetState(PlayerIndex.Three).IsConnected || GamePad.GetState(PlayerIndex.Four).IsConnected)
                mControls = new ControllerControl();
            else
                mControls = new KeyboardControl();

            mGraphics.GraphicsProfile = GraphicsProfile.Reach;

#endif
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it c5an query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            //COMMENT OUT AFTER TESTING TRIAL MODE
            //Guide.SimulateTrialMode = true;

            mGraphics.PreferredBackBufferWidth = 1280;
            mGraphics.PreferredBackBufferHeight = 720;
            //mGraphics.ToggleFullScreen();// REMEMBER TO RESET AFTER DEBUGGING!!!!!!!!!
            mGraphics.ApplyChanges();


            mTitle = new Title(mControls, mGraphics);
            mMainMenu = new MainMenu(mControls, mGraphics);
            mMainMenuLevel = Level.MainMenuLevel("Content\\Levels\\MainMenu.xml", mControls, mGraphics.GraphicsDevice.Viewport);

            mMenu = new Menu(mControls, mGraphics);
            mScoring = new Scoring(mControls);

            mWorldSelect = new WorldSelect(mControls, mGraphics);

            mPause = new Pause(mControls);
            mCredits = new Credits(mControls, mGraphics);
            mOptions = new Options(mControls, mGraphics);
            mAfterScore = new AfterScore(mControls);
            mResetConfirm = new ResetConfirm(mControls);
            mStartLevelSplash = new StartLevelSplash(mControls);

            mController = new Controller(mControls, mGraphics);
            mSoundOptions = new SoundOptions(mControls, mGraphics);

            mSpriteBatch = new SpriteBatch(mGraphics.GraphicsDevice);
            base.Initialize();

            Components.Add(new GamerServicesComponent(this));


		}

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // current viewport
            float screenscaleX =
                (float)mGraphics.GraphicsDevice.Viewport.Width / 1280.0f;
            float screenscaleY = (float)mGraphics.GraphicsDevice.Viewport.Height / 720.0f;
            // Create the scale transform for Draw. 
            // Do not scale the sprite depth (Z=1).
            scale = Matrix.CreateScale(screenscaleX, screenscaleY, 1);

            //mGraphics.PreferredBackBufferWidth = 1280;
            //mGraphics.PreferredBackBufferHeight = 720;
            //mGraphics.ApplyChanges();

            mTitle.Load(Content, mGraphics.GraphicsDevice);

            mMainMenuLevel.Load(Content);
            mMainMenu.Load(Content);
            mCredits.Load(Content);
            mOptions.Load(Content);

            mMenu.Load(Content, mGraphics.GraphicsDevice);
            mScoring.Load(Content, mGraphics.GraphicsDevice);
            mPause.Load(Content);
            GameSound.Load(Content);
            //mLevelSelect.Load(Content, mGraphics.GraphicsDevice);
            mCurrentLevel = new Level(mLevelLocation, mControls, GraphicsDevice.Viewport);
            mCurrentLevel.Load(Content);

            mWorldSelect.Load(Content);
            mAfterScore.Load(Content, GraphicsDevice);
            mResetConfirm.Load(Content);
            mController.Load(Content);
            mSoundOptions.Load(Content);
            mStartLevelSplash.Load(Content, GraphicsDevice, ref mCurrentLevel);
            // Create a new SpriteBatch, which can be used to draw textures.
            mSpriteBatch = new SpriteBatch(GraphicsDevice);

            mDefaultFont = Content.Load<SpriteFont>("Fonts/Kootenay");
            mQuartz = Content.Load<SpriteFont>("Fonts/QuartzLarge");
            mHUDTrans = Content.Load<Texture2D>("Images/HUD/HUDTrans");

            mLives = new Texture2D[10];
            for (int i = 0; i < mLives.Length; i++)
                mLives[i] = Content.Load<Texture2D>("Images/HUD/NeonLifeCount" + i);
    }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent() {}

        /// <summary>
        /// Handle when the game exits
        /// </summary>
        /// <param name="sender">Typical event stuff</param>
        /// <param name="args">Typical event stuff</param>
        protected override void OnExiting(object sender, EventArgs args)
        {
#if XBOX360

            //mLevelSelect.Save(((ControllerControl)mControls).ControllerIndex);

#else
           // mLevelSelect.Save(PlayerIndex.One);
#endif
            //if (mControls.controlScheme() == ControlSchemes.Gamepad)
            //    if (Guide.IsTrialMode && !Guide.IsVisible)
            //        if (Gamer.SignedInGamers[(int)((ControllerControl)mControls).ControllerIndex] != null)
            //            if (Gamer.SignedInGamers[((ControllerControl)mControls).ControllerIndex].IsSignedInToLive)
            //                if (Gamer.SignedInGamers[((ControllerControl)mControls).ControllerIndex].Privileges.AllowPurchaseContent)
            //                    Guide.ShowMarketplace(((ControllerControl)mControls).ControllerIndex);

        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            //mLevelSelect.Save(((ControllerControl)mControls).ControllerIndex);

            if (mCurrentState == GameStates.Credits)
                mCredits.Update(gameTime, ref mCurrentState);

            if (mCurrentState == GameStates.Options)
            {
                mOptions.Update(gameTime, ref mCurrentState, mMainMenuLevel.Environment);
                mMainMenuLevel.Update(gameTime, ref mCurrentState);
            }

            if (mCurrentState == GameStates.Title)
            {
                //Check for mute
                GameSound.menuMusic_title.Volume = GameSound.volume;

                //If the correct music isn't already playing
                if (GameSound.menuMusic_title.State != SoundState.Playing)
                    GameSound.StopOthersAndPlay(GameSound.menuMusic_title);

                mTitle.Update(gameTime, ref mCurrentState);
            }

            if (mCurrentState == GameStates.In_Game)
                mCurrentLevel.Update(gameTime, ref mCurrentState);
            else if (mCurrentState == GameStates.Main_Menu)
            {
#if XBOX360
                if (!mCheckedForSave)
                {
                    mLevelSelect.CheckForSave(((ControllerControl)mControls).ControllerIndex);
                    mCheckedForSave = true;
                }
#endif
                //mLevelSelect.Save(((ControllerControl)mControls).ControllerIndex);
                //Check for mute
                GameSound.menuMusic_title.Volume = GameSound.volume;

                //If the correct music isn't already playing
                if (GameSound.menuMusic_title.State != SoundState.Playing)
                    GameSound.StopOthersAndPlay(GameSound.menuMusic_title);

                mMainMenu.Update(gameTime,ref mCurrentState, mMainMenuLevel.Environment);
                mMainMenuLevel.Update(gameTime, ref mCurrentState);
                
            }
            else if (mCurrentState == GameStates.Score)
            {
                //Check for mute
                GameSound.menuMusic_title.Volume = GameSound.volume;
                GameSound.level_stageVictory.Volume = GameSound.volume * .75f;

                //First play win, then menu
                if (GameSound.level_stageVictory.State != SoundState.Playing)
                    if (GameSound.menuMusic_title.State != SoundState.Playing)
                        GameSound.StopOthersAndPlay(GameSound.menuMusic_title);

                mWorldSelect.UpdateStarCount();
                
                mScoring.Update(gameTime, ref mCurrentState, ref mCurrentLevel);
            }
            else if (mCurrentState == GameStates.Level_Selection)
            {
                //Check for mute
                GameSound.menuMusic_title.Volume = GameSound.volume;

                //If the correct music isn't already playing
                if (GameSound.menuMusic_title.State != SoundState.Playing)
                    GameSound.StopOthersAndPlay(GameSound.menuMusic_title);

                //mLevelSelect.Update(gameTime, ref mCurrentState, ref mCurrentLevel);
                mWorldSelect.Update(gameTime, ref mCurrentState, ref mCurrentLevel);
            }
            else if (mCurrentState == GameStates.New_Level_Selection)
            {
                //Check for mute
                GameSound.menuMusic_title.Volume = GameSound.volume;

                //If the correct music isn't already playing
                if (GameSound.menuMusic_title.State != SoundState.Playing)
                    GameSound.StopOthersAndPlay(GameSound.menuMusic_title);

                mWorldSelect.Reset();

                mCurrentState = GameStates.Level_Selection;

                mWorldSelect.Update(gameTime, ref mCurrentState, ref mCurrentLevel);

            }
            else if (mCurrentState == GameStates.Pause)
            {
                mPause.Update(gameTime, ref mCurrentState, ref mCurrentLevel);
            }
            else if (mCurrentState == GameStates.Controls)
            {
                mController.Update(gameTime, ref mCurrentState);
            }
            else if (mCurrentState == GameStates.SoundOptions)
            {
                mSoundOptions.Update(gameTime, ref mCurrentState);
            }
            else if (mCurrentState == GameStates.Unlock)
            {
                mSequence = VICTORY_DURATION;
                mCurrentState = GameStates.Victory;
            }
            else if (mCurrentState == GameStates.Next_Level)
            {
                Level tempLevel = mWorldSelect.NextLevel();
                if (tempLevel != null && !tempLevel.Name.Equals(mCurrentLevel.Name))
                {
                    mCurrentLevel = tempLevel;
                    mCurrentLevel.Load(Content);
                    mCurrentState = GameStates.In_Game;
                }
                else
                    mCurrentState = GameStates.Level_Selection;
            }
            else if (mCurrentState == GameStates.Victory)
            {
                mSequence--;

                if (mSequence <= 0)
                    mCurrentState = GameStates.Score;
            }
            else if (mCurrentState == GameStates.Death)
            {
                if (!mToggledSequence)
                {
                    mSequence = DEATH_DURATION;
                    mToggledSequence = true;
                }
                mSequence--;

                if (mSequence <= 0)
                {
                    mCurrentState = GameStates.In_Game;
                    mToggledSequence = false;
                }
            }
            else if (mCurrentState == GameStates.Exit)
            {
                if (Guide.IsTrialMode)
                {
                    mCurrentState = GameStates.TrialExit;
                }
                else
                    mCurrentState = GameStates.WaitingToExit;
            }
            else if (mCurrentState == GameStates.TrialExit)
            {
                if (Gamer.SignedInGamers[((ControllerControl)mControls).ControllerIndex] == null)
                {
                    Guide.ShowSignIn(1, true);
                    mCurrentState = GameStates.WaitingForSignIn;
                }
                else
                    mCurrentState = GameStates.ShowMarketplace;
            }
            else if (mCurrentState == GameStates.WaitingForSignIn)
            {
                if (!Guide.IsVisible)
                {
                    if (Gamer.SignedInGamers[((ControllerControl)mControls).ControllerIndex] == null)
                    {
                        mCurrentState = GameStates.WaitingToExit;
                    }
                    else
                        mCurrentState = GameStates.ShowMarketplace;
                }
            }
            else if (mCurrentState == GameStates.ShowMarketplace)
            {
                if (Gamer.SignedInGamers[((ControllerControl)mControls).ControllerIndex].IsSignedInToLive)
                {
                    if (Gamer.SignedInGamers[((ControllerControl)mControls).ControllerIndex].Privileges.AllowPurchaseContent)
                    {
                        Guide.ShowMarketplace(((ControllerControl)mControls).ControllerIndex);
                        mCurrentState = GameStates.WaitForMarketplace;
                    }
                    else
                        mCurrentState = GameStates.WaitingToExit;
                }
                else
                    mCurrentState = GameStates.WaitingToExit;
            }
            else if (mCurrentState == GameStates.WaitForMarketplace)
            {
                if (!Guide.IsVisible)
                    mCurrentState = GameStates.WaitingToExit;
            }
            else if (mCurrentState == GameStates.WaitingToExit)
            {
                if (!Guide.IsVisible)
                    this.Exit();
            }
            else if (mCurrentState == GameStates.AfterScore)
            {
                mAfterScore.Update(gameTime, ref mCurrentState, ref mCurrentLevel);
            }
            else if (mCurrentState == GameStates.ResetConfirm)
            {
                mResetConfirm.Update(gameTime, ref mCurrentState, ref mCurrentLevel);
            }
            else if (mCurrentState == GameStates.StartLevelSplash)
            {
                mStartLevelSplash.Update(gameTime, ref mCurrentState);
            }
        }

        /// <summary>
        /// Disables the menu from showing initially
        /// </summary>
        public void DisableMenu()
        {
            mCurrentState = GameStates.In_Game;
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            if (mCurrentState == GameStates.In_Game)
            {
                mCurrentLevel.Draw(mSpriteBatch, gameTime, scale);
                DrawHUD();
            }
            else if (mCurrentState == GameStates.Main_Menu)
            {
                mMainMenu.Draw(gameTime, mSpriteBatch, scale);
                mMainMenuLevel.Draw(mSpriteBatch, gameTime, scale);
            }
            else if (mCurrentState == GameStates.Credits)
                mCredits.Draw(gameTime, mSpriteBatch, scale);
            else if (mCurrentState == GameStates.Options)
            {
                mOptions.Draw(gameTime, mSpriteBatch, scale);
                mMainMenuLevel.Draw(mSpriteBatch, gameTime, scale);
            }
            else if (mCurrentState == GameStates.Controls)
                mController.Draw(gameTime, mSpriteBatch, scale);
            else if (mCurrentState == GameStates.SoundOptions)
                mSoundOptions.Draw(gameTime, mSpriteBatch, scale);
            else if (mCurrentState == GameStates.Score)
                mScoring.Draw(mSpriteBatch, mGraphics, mCurrentLevel, scale);
            else if (mCurrentState == GameStates.Level_Selection)
                mWorldSelect.Draw(mSpriteBatch, scale);
            else if (mCurrentState == GameStates.Pause)
            {
                mCurrentLevel.Draw(mSpriteBatch, gameTime, scale);
                mPause.Draw(mSpriteBatch, mGraphics, scale);
            }
            else if (mCurrentState == GameStates.Victory)
            {
                //TODO - Change this to a victory animation
                mCurrentLevel.Draw(mSpriteBatch, gameTime, scale);
                DrawHUD();
            }
            else if (mCurrentState == GameStates.Death)
            {
                //TODO - Change this to a death animation
                mCurrentLevel.Draw(mSpriteBatch, gameTime, scale);
                DrawHUD();
            }
            else if (mCurrentState == GameStates.Title)
            {
                mTitle.Draw(mSpriteBatch, gameTime, scale);
            }
            else if (mCurrentState == GameStates.AfterScore)
            {
                mScoring.Draw(mSpriteBatch, mGraphics, mCurrentLevel, scale);
                mAfterScore.Draw(mSpriteBatch, mGraphics, scale);
            }
            else if (mCurrentState == GameStates.ResetConfirm)
            {
                mResetConfirm.Draw(mSpriteBatch, mGraphics, scale);
            }
            else if (mCurrentState == GameStates.StartLevelSplash)
            {
                mCurrentLevel.Draw(mSpriteBatch, gameTime, scale);
                mStartLevelSplash.Draw(mSpriteBatch, mGraphics, scale);
            }
                
            base.Draw(gameTime);
        }

        public void DrawHUD()
        {
            Rectangle mScreenRect = mGraphics.GraphicsDevice.Viewport.TitleSafeArea;
            mSpriteBatch.Begin();
            mSpriteBatch.Draw(mHUDTrans, new Rectangle(mScreenRect.Left, mScreenRect.Top, mScreenRect.Right, mScreenRect.Height / 10), Color.White);
            mSpriteBatch.DrawString(mQuartz, "Timer: " + mCurrentLevel.Timer, new Vector2(mScreenRect.Left + mScreenRect.Width / 10, mScreenRect.Top), Color.DarkTurquoise);
            mSpriteBatch.DrawString(mQuartz, "Collected: " + mCurrentLevel.NumCollected + "/" + mCurrentLevel.NumCollectable, new Vector2(mScreenRect.Left + mScreenRect.Width / 3, mScreenRect.Top), Color.DarkTurquoise);

            mSpriteBatch.Draw(mLives[mCurrentLevel.NumLives], new Vector2(mScreenRect.Right - mLives[0].Width * 2, mScreenRect.Top), Color.White);

            if (mCurrentLevel.NumLives <= 0)
            {
                string request = "Out of Lives       Press A to Restart";

                Vector2 stringSize = mQuartz.MeasureString(request);
                mSpriteBatch.DrawString(mQuartz, request, new Vector2(mScreenRect.Center.X - stringSize.X/2, mScreenRect.Center.Y), Color.DarkTurquoise);
                mSpriteBatch.DrawString(mQuartz, request, new Vector2(mScreenRect.Center.X - stringSize.X / 2 + 2, mScreenRect.Center.Y + 2), Color.White);

            }
            mSpriteBatch.End();
        }
    }
}
