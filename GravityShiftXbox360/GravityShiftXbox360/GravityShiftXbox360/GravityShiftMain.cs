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
using System.Diagnostics;

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

        // Instance of the AfterScore class
        AfterScore mAfterScore;

        ResetConfirm mResetConfirm;

        StartLevelSplash mStartLevelSplash;

        PreScore mPreScore;

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

        private bool mCheckedForSave;

        private bool mShowedSignIn;

        public GravityShiftMain()
        {
            Components.Add(new GamerServicesComponent(this));
            mGraphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            mShowedSignIn = false;

#if XBOX360
            mControls = new ControllerControl();
#else
            if (GamePad.GetState(PlayerIndex.One).IsConnected || GamePad.GetState(PlayerIndex.Two).IsConnected ||
                GamePad.GetState(PlayerIndex.Three).IsConnected || GamePad.GetState(PlayerIndex.Four).IsConnected)
                mControls = new ControllerControl();
            else
                mControls = new KeyboardControl();

            mGraphics.GraphicsProfile = GraphicsProfile.Reach;

            mCheckedForSave = false;

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
           
            //mMenu = new Menu(mControls, mGraphics);
            mScoring = new Scoring(mControls);

            mWorldSelect = new WorldSelect(mControls, mGraphics);

            mPause = new Pause(mControls);
            mCredits = new Credits(mControls, mGraphics);
            mOptions = new Options(mControls, mGraphics);
            mAfterScore = new AfterScore(mControls);
            mResetConfirm = new ResetConfirm(mControls);
            mStartLevelSplash = new StartLevelSplash(mControls);

            mPreScore = new PreScore(mControls);

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

            mMainMenu.Load(Content);
            mMainMenuLevel = Level.MainMenuLevel("Content\\Levels\\MainMenu.xml", mControls, mGraphics.GraphicsDevice.Viewport, mMainMenu.GetInnerRegion());

            mMainMenuLevel.Load(Content);
            mCredits.Load(Content);
            mOptions.Load(Content);
            
            mPause.Load(Content);
            GameSound.Load(Content);
            mCurrentLevel = new Level(mLevelLocation, mControls, GraphicsDevice.Viewport);
            mCurrentLevel.Load(Content);

            mWorldSelect.Load(Content);
            mScoring.Load(Content, mGraphics.GraphicsDevice, mWorldSelect);
            mAfterScore.Load(Content, GraphicsDevice);
            mPreScore.Load(Content, GraphicsDevice, mWorldSelect);
            mResetConfirm.Load(Content);
            mController.Load(Content);
            mSoundOptions.Load(Content);
            mStartLevelSplash.Load(Content, GraphicsDevice);
            // Create a new SpriteBatch, which can be used to draw textures.
            mSpriteBatch = new SpriteBatch(GraphicsDevice);

            mDefaultFont = Content.Load<SpriteFont>("Fonts/Kootenay");
            mQuartz = Content.Load<SpriteFont>("Fonts/QuartzLarge");
            mHUDTrans = Content.Load<Texture2D>("Images/HUD/HUDTrans");

            mLives = new Texture2D[6];
            mLives[5] = Content.Load<Texture2D>("Images/Player/Laugh2");
            mLives[4] = Content.Load<Texture2D>("Images/Player/Laugh");
            mLives[3] = Content.Load<Texture2D>("Images/Player/Smile");
            mLives[2] = Content.Load<Texture2D>("Images/Player/Surprise");
            mLives[1] = Content.Load<Texture2D>("Images/Player/Worry");
            mLives[0] = Content.Load<Texture2D>("Images/Player/Dead2");
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
            SignedInGamer player = Gamer.SignedInGamers[((ControllerControl)mControls).ControllerIndex];
            if (player != null)
            {
                if (!player.IsGuest)
                {
                    mWorldSelect.Save();
                }
            }
            
#else
            mWorldSelect.Save();
#endif
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

#if XBOX360
            SignedInGamer player = Gamer.SignedInGamers[((ControllerControl)mControls).ControllerIndex];
            if (player != null)
            {
                if (Guide.IsTrialMode)
                {
                    player.Presence.PresenceMode = GamerPresenceMode.TutorialMode;
                }
                else if (mCurrentState == GameStates.In_Game)
                {
                    player.Presence.PresenceMode = GamerPresenceMode.PuzzleMode;
                }
                else if (mCurrentState == GameStates.Pause)
                {
                    player.Presence.PresenceMode = GamerPresenceMode.Paused;
                }
                else
                {
                    player.Presence.PresenceMode = GamerPresenceMode.AtMenu;
                }

            }
#endif

            if (mCurrentState == GameStates.Credits)
                mCredits.Update(gameTime, ref mCurrentState);

            if (mCurrentState == GameStates.Options)
            {
                mOptions.Update(gameTime, ref mCurrentState, mMainMenuLevel);
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
                player = Gamer.SignedInGamers[((ControllerControl)mControls).ControllerIndex];
                if (!mShowedSignIn && (player == null))
                {
                    Guide.ShowSignIn(1, false);
                    mCurrentState = GameStates.WaitingForSaveSignIn;
                }
                else
                {
                    mShowedSignIn = true;
                }
                if (!mCheckedForSave && mShowedSignIn)
                {

                    if ((player != null)  && !Guide.IsVisible)
                    {
                        if (!player.IsGuest)
                        {
                            mCheckedForSave = mWorldSelect.CheckForSave();
                        }
                        
                    }

                }
#endif


                //Check for mute
                GameSound.menuMusic_title.Volume = GameSound.volume;

                //If the correct music isn't already playing
                if (GameSound.menuMusic_title.State != SoundState.Playing)
                    GameSound.StopOthersAndPlay(GameSound.menuMusic_title);

                mMainMenu.Update(gameTime,ref mCurrentState, mMainMenuLevel);
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
                
                mScoring.Update(gameTime, ref mCurrentState, ref mCurrentLevel, mWorldSelect);
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

                mCurrentState = GameStates.Options;
            }
            else if (mCurrentState == GameStates.Pause)
                mPause.Update(gameTime, ref mCurrentState, ref mCurrentLevel);
            else if (mCurrentState == GameStates.Controls)
                mController.Update(gameTime, ref mCurrentState);
            else if (mCurrentState == GameStates.SoundOptions)
                mSoundOptions.Update(gameTime, ref mCurrentState);
            else if (mCurrentState == GameStates.Unlock)
            {
                //Update the stars in level
                //Update star count
                mCurrentLevel.UpdateStars();
//                mWorldSelect.UpdateStarCount();

                mSequence = VICTORY_DURATION;
                mCurrentState = GameStates.Victory;
            }
            else if (mCurrentState == GameStates.Next_Level)
            {
              /*  Level tempLevel = mWorldSelect.NextLevel();
                if (tempLevel != null && !tempLevel.Name.Equals(mCurrentLevel.Name))
                {
                    mCurrentLevel = tempLevel;
                    mCurrentLevel.Load(Content);
                    mCurrentState = GameStates.In_Game;
                }
                else*/
                    mCurrentState = GameStates.Level_Selection;
            }
            else if (mCurrentState == GameStates.Victory)
            {
                mSequence--;
                
                Debug.WriteLine(mWorldSelect.getLevelDeath());

                if (mSequence <= 0) 
                {
                    if ((mCurrentLevel.CollectionStar == 3 && (mWorldSelect.getLevelCollect()) != 3) ||
                       (mCurrentLevel.DeathStar == 3 && (mWorldSelect.getLevelDeath() != 3)) ||
                       (mCurrentLevel.TimerStar == 3 && (mWorldSelect.getLevelTime() != 3)))
                        mCurrentState = GameStates.PreScore;
                    else
                        mCurrentState = GameStates.Score;
                }
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
#if XBOX360
                if (Guide.IsTrialMode)
                    mCurrentState = GameStates.TrialExit;
                else
#endif
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
                        mCurrentState = GameStates.WaitingToExit;
                    else
                        mCurrentState = GameStates.ShowMarketplace;
                }
            }
            else if (mCurrentState == GameStates.WaitingForSaveSignIn)
            {
                if (!Guide.IsVisible)
                {
                    mShowedSignIn = true;
                    mCurrentState = GameStates.Main_Menu;
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
                    if (!Guide.IsTrialMode)
                    {
                        mCurrentState = GameStates.Main_Menu;
                    }
                    else
                    {
                        mCurrentState = GameStates.WaitingToExit;
                    }
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
            else if (mCurrentState == GameStates.PreScore)
            {
                //Check for mute
                GameSound.menuMusic_title.Volume = GameSound.volume;
                GameSound.level_stageVictory.Volume = GameSound.volume * .75f;

                //First play win, then menu
                if (GameSound.level_stageVictory.State != SoundState.Playing)
                    if (GameSound.menuMusic_title.State != SoundState.Playing)
                        GameSound.StopOthersAndPlay(GameSound.menuMusic_title);

                mPreScore.Update(gameTime, ref mCurrentState, ref mCurrentLevel);
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
            {
                mCredits.Draw(gameTime, mSpriteBatch, scale);
                mCredits.DrawLevel(mSpriteBatch, gameTime, scale);
            }
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
            else if (mCurrentState == GameStates.PreScore)
            {
                mScoring.Draw(mSpriteBatch, mGraphics, mCurrentLevel, scale);
                mPreScore.Draw(mSpriteBatch, mGraphics, scale, mCurrentLevel);
            }
            else if (mCurrentState == GameStates.ResetConfirm)
            {
                mResetConfirm.Draw(mSpriteBatch, mGraphics, scale);
            }
            else if (mCurrentState == GameStates.StartLevelSplash)
            {
                mCurrentLevel.Draw(mSpriteBatch, gameTime, scale);
                mStartLevelSplash.Draw(mSpriteBatch, mGraphics, mCurrentLevel, scale);
            }
                
            base.Draw(gameTime);
        }

        public void DrawHUD()
        {
            Rectangle mScreenRect = mGraphics.GraphicsDevice.Viewport.TitleSafeArea;
            mSpriteBatch.Begin();
           
            string goalString = "Goal: ";
            Vector2 goalLength = mQuartz.MeasureString(goalString);
            
            /* TIMER VARIABLES*/
            string timerString = "Timer: ";
            string timeString = mCurrentLevel.Timer.ToString();
            string timeGoal = mCurrentLevel.IdealTime.ToString();
            Vector2 timerLength = mQuartz.MeasureString(timerString);
            Vector2 numberLength = mQuartz.MeasureString("99   ");

            /* COLLECTED VARIABLES*/
            string collectedString = "Collected: ";
            string collectString = mCurrentLevel.NumCollected.ToString();// + "/" + mCurrentLevel.NumCollectable;
            string collectGoal = mCurrentLevel.NumCollectable.ToString();
            Vector2 collectedLength = mQuartz.MeasureString(collectedString);

            /* DEATHS VARIABLE*/
            string livesString = "Lives:";
            //int lives = mCurrentLevel.NumLives;
            Vector2 livesLength = mQuartz.MeasureString(livesString);

            //Draw Timer

            //HUDTrans block
            mSpriteBatch.Draw(mHUDTrans, new Rectangle(mScreenRect.Left, mScreenRect.Top, 
                (int)timerLength.X + (int)numberLength.X, (int)timerLength.Y + (int)numberLength.Y), Color.White);

            Vector2 placement = new Vector2(mScreenRect.Left + 10, mScreenRect.Top);
            mSpriteBatch.DrawString(mQuartz, timerString, new Vector2(placement.X - 1, placement.Y - 1), Color.White);
            mSpriteBatch.DrawString(mQuartz, timerString, placement, Color.SteelBlue);
            mSpriteBatch.DrawString(mQuartz, timeString, new Vector2(placement.X + timerLength.X, placement.Y), Color.White);
            placement.Y += timerLength.Y;
            mSpriteBatch.DrawString(mQuartz, goalString, new Vector2(placement.X - 1, placement.Y - 1), Color.White);
            mSpriteBatch.DrawString(mQuartz, goalString, placement, Color.SteelBlue);
            mSpriteBatch.DrawString(mQuartz, timeGoal, new Vector2(placement.X + timerLength.X, placement.Y), Color.White);

            //Draw collected

            //HUDTrans block
            mSpriteBatch.Draw(mHUDTrans, new Rectangle(mScreenRect.Right - (int)collectedLength.X - (int)numberLength.X, mScreenRect.Top,
               (int)collectedLength.X + (int)numberLength.X, (int)collectedLength.Y + (int)numberLength.Y), Color.White);
            
            placement = new Vector2(mScreenRect.Right - collectedLength.X - numberLength.X, mScreenRect.Top);
            mSpriteBatch.DrawString(mQuartz, collectedString, new Vector2(placement.X - 1, placement.Y - 1), Color.White);
            mSpriteBatch.DrawString(mQuartz, collectedString, placement, Color.SteelBlue);
            mSpriteBatch.DrawString(mQuartz, collectString, new Vector2(placement.X + collectedLength.X, placement.Y), Color.White);
            placement.Y += collectedLength.Y;
            mSpriteBatch.DrawString(mQuartz, goalString, new Vector2(placement.X - 1, placement.Y - 1), Color.White);
            mSpriteBatch.DrawString(mQuartz, goalString, placement, Color.SteelBlue);
            mSpriteBatch.DrawString(mQuartz, collectGoal, new Vector2(placement.X + collectedLength.X, placement.Y), Color.White);
            placement.Y += goalLength.Y;

            //Draw deaths

            //HUDTrans block
            mSpriteBatch.Draw(mHUDTrans, new Rectangle(mScreenRect.Left, mScreenRect.Bottom - (int)mLives[0].Height - 10,
                (int)livesLength.X + (int)(mLives[0].Width * 0.75f) * (mCurrentLevel.NumLives+1), (int)mLives[0].Height + 5), Color.White);
            
            //Number lives
            placement = new Vector2(mScreenRect.Left + 10, mScreenRect.Bottom - (int) mLives[0].Height - 10);

            mSpriteBatch.DrawString(mQuartz, livesString, new Vector2(placement.X - 1, placement.Y - 1), Color.White);
            mSpriteBatch.DrawString(mQuartz, livesString, placement, Color.SteelBlue);

            placement = new Vector2(mScreenRect.Left + livesLength.X, mScreenRect.Bottom - (int)mLives[0].Height - 10);
            if (mCurrentLevel.NumLives == 5)
            {
                mSpriteBatch.Draw(mLives[5], new Rectangle((int)(placement.X + (mLives[0].Width * 0.75f)), (int) placement.Y, (int)(mLives[0].Width * 0.6f), (int)(mLives[0].Height * 0.6f)), Color.White);
                mSpriteBatch.Draw(mLives[5], new Rectangle((int)(placement.X + 2 * (mLives[0].Width * 0.75f)), (int)placement.Y, (int)(mLives[0].Width * 0.6f), (int)(mLives[0].Height * 0.6f)), Color.White);
                mSpriteBatch.Draw(mLives[5], new Rectangle((int)(placement.X + 3 * (mLives[0].Width * 0.75f)), (int)placement.Y, (int)(mLives[0].Width * 0.6f), (int)(mLives[0].Height * 0.6f)), Color.White);
                mSpriteBatch.Draw(mLives[5], new Rectangle((int)(placement.X + 4 * (mLives[0].Width * 0.75f)), (int)placement.Y, (int)(mLives[0].Width * 0.6f), (int)(mLives[0].Height * 0.6f)), Color.White);
                mSpriteBatch.Draw(mLives[5], new Rectangle((int)(placement.X + 5 * (mLives[0].Width * 0.75f)), (int)placement.Y, (int)(mLives[0].Width * 0.6f), (int)(mLives[0].Height * 0.6f)), Color.White);
            }
            else if (mCurrentLevel.NumLives == 4)
            {
                mSpriteBatch.Draw(mLives[4], new Rectangle((int)(placement.X + (mLives[0].Width * 0.75f)), (int)placement.Y, (int)(mLives[0].Width * 0.6f), (int)(mLives[0].Height * 0.6f)), Color.White);
                mSpriteBatch.Draw(mLives[4], new Rectangle((int)(placement.X + 2 * (mLives[0].Width * 0.75f)), (int)placement.Y, (int)(mLives[0].Width * 0.6f), (int)(mLives[0].Height * 0.6f)), Color.White);
                mSpriteBatch.Draw(mLives[4], new Rectangle((int)(placement.X + 3 * (mLives[0].Width * 0.75f)), (int)placement.Y, (int)(mLives[0].Width * 0.6f), (int)(mLives[0].Height * 0.6f)), Color.White);
                mSpriteBatch.Draw(mLives[4], new Rectangle((int)(placement.X + 4 * (mLives[0].Width * 0.75f)), (int)placement.Y, (int)(mLives[0].Width * 0.6f), (int)(mLives[0].Height * 0.6f)), Color.White);
            }
            else if (mCurrentLevel.NumLives == 3)
            {
                mSpriteBatch.Draw(mLives[3], new Rectangle((int)(placement.X + (mLives[0].Width * 0.75f)), (int)placement.Y, (int)(mLives[0].Width * 0.6f), (int)(mLives[0].Height * 0.6f)), Color.White);
                mSpriteBatch.Draw(mLives[3], new Rectangle((int)(placement.X + 2 * (mLives[0].Width * 0.75f)), (int)placement.Y, (int)(mLives[0].Width * 0.6f), (int)(mLives[0].Height * 0.6f)), Color.White);
                mSpriteBatch.Draw(mLives[3], new Rectangle((int)(placement.X + 3 * (mLives[0].Width * 0.75f)), (int)placement.Y, (int)(mLives[0].Width * 0.6f), (int)(mLives[0].Height * 0.6f)), Color.White);
            }
            else if (mCurrentLevel.NumLives == 2)
            {
                mSpriteBatch.Draw(mLives[2], new Rectangle((int)(placement.X + (mLives[0].Width * 0.75f)), (int)placement.Y, (int)(mLives[0].Width * 0.6f), (int)(mLives[0].Height * 0.6f)), Color.White);
                mSpriteBatch.Draw(mLives[2], new Rectangle((int)(placement.X + 2 * (mLives[0].Width * 0.75f)), (int)placement.Y, (int)(mLives[0].Width * 0.6f), (int)(mLives[0].Height * 0.6f)), Color.White);
            }
            else if (mCurrentLevel.NumLives == 1)
            {
                mSpriteBatch.Draw(mLives[1], new Rectangle((int)(placement.X + (mLives[0].Width * 0.75f)), (int)placement.Y, (int)(mLives[0].Width * 0.6f), (int)(mLives[0].Height * 0.6f)), Color.White);
            }
            else if (mCurrentLevel.NumLives == 0)
            {
                mSpriteBatch.Draw(mLives[0], new Rectangle((int)(placement.X + (mLives[0].Width * 0.75f)), (int)placement.Y, (int)(mLives[0].Width * 0.6f), (int)(mLives[0].Height * 0.6f)), Color.White);
            }


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
