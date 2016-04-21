using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MrGravity.Menu_Code;
using MrGravity.MISC_Code;

namespace MrGravity
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class GravityShiftMain : Game
    {
        public static GraphicsDeviceManager MGraphics;
        private SpriteBatch _mSpriteBatch;

        // Scale - Used to make sure the HUD is drawn based on the screen size
        public Matrix Scale;

        private Title _mTitle;

        // Instance of the AfterScore class
        private AfterScore _mAfterScore;

        private ResetConfirm _mResetConfirm;

        private StartLevelSplash _mStartLevelSplash;

        private PreScore _mPreScore;

        private MainMenu _mMainMenu;
        private Level _mMainMenuLevel;

        //Instance of the scoring class
        private Scoring _mScoring;

        //Instance of the level selection class
        //LevelSelect mLevelSelect;

        private WorldSelect _mWorldSelect;

        //Instance of the pause class
        private Pause _mPause;

        private Options _mOptions;
        private Credits _mCredits;

        //Instance of the Controller class
        private Controller _mController;

        private SoundOptions _mSoundOptions;

        private PurchaseScreenSplash _mPurchaseScreenSplash;
        private WorldPurchaseSplash _mWorldPurchaseScreenSplash;
		
        private GameStates _mCurrentState = GameStates.Title;

        //Max duration of a sequence
        private static readonly int VictoryDuration = 120;
        private static readonly int DeathDuration = 50;

        //Duration of a sequence
        private int _mSequence;

        //Boolean toggle variable
        private bool _mToggledSequence;

        private readonly IControlScheme _mControls;

        //Current level
        private Level _mCurrentLevel;

        //Fonts for this game
        private SpriteFont _mDefaultFont;
        private SpriteFont _mQuartz;

        //Transparant HUD
        private Texture2D _mHudTrans;

        //Lives
        private Texture2D[] _mLives;

        //TO BE CHANGED- Actually, this may be ok since we use this to play test.
        public string LevelLocation { get { return _mLevelLocation; } set { _mLevelLocation = "..\\..\\..\\Content\\Levels\\" + value; } }        
        private string _mLevelLocation = "..\\..\\..\\Content\\Levels\\DefaultLevel.xml";

        private bool _mCheckedForSave;

        private bool _mShowedSignIn;

        public GravityShiftMain()
        {
            Components.Add(new GamerServicesComponent(this));
            MGraphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            _mShowedSignIn = false;

#if XBOX360
            mControls = new ControllerControl();
#else
            if (GamePad.GetState(PlayerIndex.One).IsConnected || GamePad.GetState(PlayerIndex.Two).IsConnected ||
                GamePad.GetState(PlayerIndex.Three).IsConnected || GamePad.GetState(PlayerIndex.Four).IsConnected)
                _mControls = new ControllerControl();
            else
                _mControls = new KeyboardControl();

            MGraphics.GraphicsProfile = GraphicsProfile.HiDef;

            _mCheckedForSave = false;

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

            MGraphics.PreferredBackBufferWidth = 1280;
            MGraphics.PreferredBackBufferHeight = 720;
            //mGraphics.ToggleFullScreen();// REMEMBER TO RESET AFTER DEBUGGING!!!!!!!!!
            MGraphics.ApplyChanges();


            _mTitle = new Title(_mControls, MGraphics);
            _mMainMenu = new MainMenu(_mControls, MGraphics);
           
            //mMenu = new Menu(mControls, mGraphics);
            _mScoring = new Scoring(_mControls);

            _mWorldSelect = new WorldSelect(_mControls, MGraphics);

            _mPause = new Pause(_mControls);
            _mCredits = new Credits(_mControls, MGraphics);
            _mOptions = new Options(_mControls, MGraphics);
            _mAfterScore = new AfterScore(_mControls);
            _mResetConfirm = new ResetConfirm(_mControls);
            _mStartLevelSplash = new StartLevelSplash(_mControls);

            _mPreScore = new PreScore(_mControls);

            _mController = new Controller(_mControls, MGraphics);
            _mSoundOptions = new SoundOptions(_mControls, MGraphics);
            _mPurchaseScreenSplash = new PurchaseScreenSplash(_mControls, MGraphics);
            _mWorldPurchaseScreenSplash = new WorldPurchaseSplash(_mControls, MGraphics);

            _mSpriteBatch = new SpriteBatch(MGraphics.GraphicsDevice);
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
            var screenscaleX =
                MGraphics.GraphicsDevice.Viewport.Width / 1280.0f;
            var screenscaleY = MGraphics.GraphicsDevice.Viewport.Height / 720.0f;
            // Create the scale transform for Draw. 
            // Do not scale the sprite depth (Z=1).
            Scale = Matrix.CreateScale(screenscaleX, screenscaleY, 1);

            //mGraphics.PreferredBackBufferWidth = 1280;
            //mGraphics.PreferredBackBufferHeight = 720;
            //mGraphics.ApplyChanges();

            _mTitle.Load(Content, MGraphics.GraphicsDevice);

            _mMainMenu.Load(Content);
            _mMainMenuLevel = Level.MainMenuLevel("Content\\Levels\\MainMenu.xml", _mControls, MGraphics.GraphicsDevice.Viewport, _mMainMenu.GetInnerRegion());

            _mMainMenuLevel.Load(Content);
            _mCredits.Load(Content);
            _mOptions.Load(Content);
            
            _mPause.Load(Content);
            GameSound.Load(Content);
            _mCurrentLevel = new Level(_mLevelLocation, _mControls, GraphicsDevice.Viewport);
            _mCurrentLevel.Load(Content);

            _mWorldSelect.Load(Content);
            _mScoring.Load(Content, MGraphics.GraphicsDevice, _mWorldSelect);
            _mAfterScore.Load(Content, GraphicsDevice);
            _mPreScore.Load(Content, GraphicsDevice, _mWorldSelect);
            _mResetConfirm.Load(Content);
            _mController.Load(Content);
            _mSoundOptions.Load(Content);
            _mStartLevelSplash.Load(Content, GraphicsDevice);
            _mPurchaseScreenSplash.Load(Content, GraphicsDevice);
            _mWorldPurchaseScreenSplash.Load(Content, GraphicsDevice);
            // Create a new SpriteBatch, which can be used to draw textures.
            _mSpriteBatch = new SpriteBatch(GraphicsDevice);

            _mDefaultFont = Content.Load<SpriteFont>("Fonts/Kootenay");
            _mQuartz = Content.Load<SpriteFont>("Fonts/QuartzLarge");
            _mHudTrans = Content.Load<Texture2D>("Images/HUD/HUDTrans");

            _mLives = new Texture2D[6];
            _mLives[5] = Content.Load<Texture2D>("Images/Player/Laugh2");
            _mLives[4] = Content.Load<Texture2D>("Images/Player/Laugh");
            _mLives[3] = Content.Load<Texture2D>("Images/Player/Smile");
            _mLives[2] = Content.Load<Texture2D>("Images/Player/Surprise");
            _mLives[1] = Content.Load<Texture2D>("Images/Player/Worry");
            _mLives[0] = Content.Load<Texture2D>("Images/Player/Dead2");
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
            _mWorldSelect.Save();
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

            if (_mCurrentState == GameStates.Credits)
                _mCredits.Update(gameTime, ref _mCurrentState);

            if (_mCurrentState == GameStates.Options)
            {
                _mOptions.Update(gameTime, ref _mCurrentState, _mMainMenuLevel);
                _mMainMenuLevel.Update(gameTime, ref _mCurrentState);
            }

            if (_mCurrentState == GameStates.Title)
            {
                //Check for mute
                GameSound.MenuMusicTitle.Volume = GameSound.Volume;

                //If the correct music isn't already playing
                if (GameSound.MenuMusicTitle.State != SoundState.Playing)
                    GameSound.StopOthersAndPlay(GameSound.MenuMusicTitle);

                _mTitle.Update(gameTime, ref _mCurrentState);
            }

            if (_mCurrentState == GameStates.InGame)
                _mCurrentLevel.Update(gameTime, ref _mCurrentState);
            else if (_mCurrentState == GameStates.MainMenu)
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
                GameSound.MenuMusicTitle.Volume = GameSound.Volume;

                //If the correct music isn't already playing
                if (GameSound.MenuMusicTitle.State != SoundState.Playing)
                    GameSound.StopOthersAndPlay(GameSound.MenuMusicTitle);

                _mMainMenu.Update(gameTime,ref _mCurrentState, _mMainMenuLevel);
                _mMainMenuLevel.Update(gameTime, ref _mCurrentState);
                
            }
            else if (_mCurrentState == GameStates.Score)
            {
                //Check for mute
                GameSound.MenuMusicTitle.Volume = GameSound.Volume;
                GameSound.LevelStageVictory.Volume = GameSound.Volume * .75f;

                //First play win, then menu
                if (GameSound.LevelStageVictory.State != SoundState.Playing)
                    if (GameSound.MenuMusicTitle.State != SoundState.Playing)
                        GameSound.StopOthersAndPlay(GameSound.MenuMusicTitle);

                _mWorldSelect.UpdateStarCount();
                
                _mScoring.Update(gameTime, ref _mCurrentState, ref _mCurrentLevel, _mWorldSelect);
            }
            else if (_mCurrentState == GameStates.LevelSelection)
            {
                //Check for mute
                GameSound.MenuMusicTitle.Volume = GameSound.Volume;

                //If the correct music isn't already playing
                if (GameSound.MenuMusicTitle.State != SoundState.Playing)
                    GameSound.StopOthersAndPlay(GameSound.MenuMusicTitle);

                //mLevelSelect.Update(gameTime, ref mCurrentState, ref mCurrentLevel);
                _mWorldSelect.Update(gameTime, ref _mCurrentState, ref _mCurrentLevel);
            }
            else if (_mCurrentState == GameStates.NewLevelSelection)
            {
                //Check for mute
                GameSound.MenuMusicTitle.Volume = GameSound.Volume;

                //If the correct music isn't already playing
                if (GameSound.MenuMusicTitle.State != SoundState.Playing)
                    GameSound.StopOthersAndPlay(GameSound.MenuMusicTitle);

                _mWorldSelect.Reset();

                _mCurrentState = GameStates.Options;
            }
            else if (_mCurrentState == GameStates.Pause)
                _mPause.Update(gameTime, ref _mCurrentState, ref _mCurrentLevel);
            else if (_mCurrentState == GameStates.Controls)
                _mController.Update(gameTime, ref _mCurrentState);
            else if (_mCurrentState == GameStates.SoundOptions)
                _mSoundOptions.Update(gameTime, ref _mCurrentState);
            else if (_mCurrentState == GameStates.Unlock)
            {
                //Update the stars in level
                //Update star count
                _mCurrentLevel.UpdateStars();
//                mWorldSelect.UpdateStarCount();

                _mSequence = VictoryDuration;
                _mCurrentState = GameStates.Victory;
            }
            else if (_mCurrentState == GameStates.NextLevel)
            {
              /*  Level tempLevel = mWorldSelect.NextLevel();
                if (tempLevel != null && !tempLevel.Name.Equals(mCurrentLevel.Name))
                {
                    mCurrentLevel = tempLevel;
                    mCurrentLevel.Load(Content);
                    mCurrentState = GameStates.In_Game;
                }
                else*/
                    _mCurrentState = GameStates.LevelSelection;
            }
            else if (_mCurrentState == GameStates.Victory)
            {
                _mSequence--;
                
                Debug.WriteLine(_mWorldSelect.GetLevelDeath());

                if (_mSequence <= 0) 
                {
                    if ((_mCurrentLevel.CollectionStar == 3 && (_mWorldSelect.GetLevelCollect()) != 3) ||
                       (_mCurrentLevel.DeathStar == 3 && (_mWorldSelect.GetLevelDeath() != 3)) ||
                       (_mCurrentLevel.TimerStar == 3 && (_mWorldSelect.GetLevelTime() != 3)))
                        _mCurrentState = GameStates.PreScore;
                    else
                        _mCurrentState = GameStates.Score;
                }
            }
            else if (_mCurrentState == GameStates.Death)
            {
                if (!_mToggledSequence)
                {
                    _mSequence = DeathDuration;
                    _mToggledSequence = true;
                }
                _mSequence--;

                if (_mSequence <= 0)
                {
                    _mCurrentState = GameStates.InGame;
                    _mToggledSequence = false;
                }
            }
            else if (_mCurrentState == GameStates.Exit)
            {
#if XBOX360
                if (Guide.IsTrialMode)
                    mCurrentState = GameStates.PurchaseScreen;
                else
#endif
                    _mCurrentState = GameStates.WaitingToExit;
            }
            else if (_mCurrentState == GameStates.PurchaseScreen)
            {
                _mPurchaseScreenSplash.Update(gameTime, ref _mCurrentState);
            }
            else if (_mCurrentState == GameStates.WorldPurchaseScreen)
            {
                _mWorldPurchaseScreenSplash.Update(gameTime, ref _mCurrentState);
            }
            else if (_mCurrentState == GameStates.TrialExit)
            {
                if (Gamer.SignedInGamers[((ControllerControl)_mControls).ControllerIndex] == null)
                {
                    Guide.ShowSignIn(1, true);
                    _mCurrentState = GameStates.WaitingForSignIn;
                }
                else
                    _mCurrentState = GameStates.ShowMarketplace;
            }
            else if (_mCurrentState == GameStates.WorldPurchase)
            {
                if (Gamer.SignedInGamers[((ControllerControl)_mControls).ControllerIndex] == null)
                {
                    Guide.ShowSignIn(1, true);
                    _mCurrentState = GameStates.WorldWaitingForSignIn;
                }
                else
                    _mCurrentState = GameStates.WorldShowMarketplace;
            }
            else if (_mCurrentState == GameStates.WaitingForSignIn)
            {
                if (!Guide.IsVisible)
                {
                    if (Gamer.SignedInGamers[((ControllerControl)_mControls).ControllerIndex] == null)
                        _mCurrentState = GameStates.WaitingToExit;
                    else
                        _mCurrentState = GameStates.ShowMarketplace;
                }
            }
            else if (_mCurrentState == GameStates.WorldWaitingForSignIn)
            {
                if (!Guide.IsVisible)
                {
                    if (Gamer.SignedInGamers[((ControllerControl)_mControls).ControllerIndex] == null)
                        _mCurrentState = GameStates.LevelSelection;
                    else
                        _mCurrentState = GameStates.WorldShowMarketplace;
                }
            }
            else if (_mCurrentState == GameStates.WaitingForSaveSignIn)
            {
                if (!Guide.IsVisible)
                {
                    _mShowedSignIn = true;
                    _mCurrentState = GameStates.MainMenu;
                }
            }
            else if (_mCurrentState == GameStates.ShowMarketplace)
            {
                if (Gamer.SignedInGamers[((ControllerControl)_mControls).ControllerIndex].IsSignedInToLive)
                {
                    if (Gamer.SignedInGamers[((ControllerControl)_mControls).ControllerIndex].Privileges.AllowPurchaseContent)
                    {
                        Guide.ShowMarketplace(((ControllerControl)_mControls).ControllerIndex);
                        _mCurrentState = GameStates.WaitForMarketplace;
                    }
                    else
                        _mCurrentState = GameStates.WaitingToExit;
                }
                else
                    _mCurrentState = GameStates.WaitingToExit;
            }
            else if (_mCurrentState == GameStates.WorldShowMarketplace)
            {
                if (Gamer.SignedInGamers[((ControllerControl)_mControls).ControllerIndex].IsSignedInToLive)
                {
                    if (Gamer.SignedInGamers[((ControllerControl)_mControls).ControllerIndex].Privileges.AllowPurchaseContent)
                    {
                        Guide.ShowMarketplace(((ControllerControl)_mControls).ControllerIndex);
                        _mCurrentState = GameStates.WorldWaitForMarketplace;
                    }
                    else
                        _mCurrentState = GameStates.LevelSelection;
                }
                else
                    _mCurrentState = GameStates.WaitingToExit;
            }
            else if (_mCurrentState == GameStates.WaitForMarketplace)
            {
                if (!Guide.IsVisible)
                    if (!Guide.IsTrialMode)
                    {
                        _mCurrentState = GameStates.MainMenu;
                    }
                    else
                    {
                        _mCurrentState = GameStates.WaitingToExit;
                    }
            }
            else if (_mCurrentState == GameStates.WorldWaitForMarketplace)
            {
                if (!Guide.IsVisible)
                    _mCurrentState = GameStates.LevelSelection;
            }
            else if (_mCurrentState == GameStates.WaitingToExit)
            {
                if (!Guide.IsVisible)
                    Exit();
            }
            else if (_mCurrentState == GameStates.AfterScore)
            {
                _mAfterScore.Update(gameTime, ref _mCurrentState, ref _mCurrentLevel);
            }
            else if (_mCurrentState == GameStates.PreScore)
            {
                //Check for mute
                GameSound.MenuMusicTitle.Volume = GameSound.Volume;
                GameSound.LevelStageVictory.Volume = GameSound.Volume * .75f;

                //First play win, then menu
                if (GameSound.LevelStageVictory.State != SoundState.Playing)
                    if (GameSound.MenuMusicTitle.State != SoundState.Playing)
                        GameSound.StopOthersAndPlay(GameSound.MenuMusicTitle);

                _mPreScore.Update(gameTime, ref _mCurrentState, ref _mCurrentLevel);
            }
            else if (_mCurrentState == GameStates.ResetConfirm)
            {
                _mResetConfirm.Update(gameTime, ref _mCurrentState, ref _mCurrentLevel);
            }
            else if (_mCurrentState == GameStates.StartLevelSplash)
            {
                _mStartLevelSplash.Update(gameTime, ref _mCurrentState);
            }
        }

        /// <summary>
        /// Disables the menu from showing initially
        /// </summary>
        public void DisableMenu()
        {
            _mCurrentState = GameStates.InGame;
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            if (_mCurrentState == GameStates.InGame)
            {
                _mCurrentLevel.Draw(_mSpriteBatch, gameTime, Scale);
                DrawHud();
            }
            else if (_mCurrentState == GameStates.MainMenu)
            {
                _mMainMenu.Draw(gameTime, _mSpriteBatch, Scale);
                _mMainMenuLevel.Draw(_mSpriteBatch, gameTime, Scale);
            }
            else if (_mCurrentState == GameStates.Credits)
            {
                _mCredits.Draw(gameTime, _mSpriteBatch, Scale);
                _mCredits.DrawLevel(_mSpriteBatch, gameTime, Scale);
            }
            else if (_mCurrentState == GameStates.Options)
            {
                _mOptions.Draw(gameTime, _mSpriteBatch, Scale);
                _mMainMenuLevel.Draw(_mSpriteBatch, gameTime, Scale);
            }
            else if (_mCurrentState == GameStates.Controls)
                _mController.Draw(gameTime, _mSpriteBatch, Scale);
            else if (_mCurrentState == GameStates.SoundOptions)
                _mSoundOptions.Draw(gameTime, _mSpriteBatch, Scale);
            else if (_mCurrentState == GameStates.Score)
                _mScoring.Draw(_mSpriteBatch, MGraphics, _mCurrentLevel, Scale);
            else if (_mCurrentState == GameStates.LevelSelection)
                _mWorldSelect.Draw(_mSpriteBatch, Scale);
            else if (_mCurrentState == GameStates.Pause)
            {
                _mCurrentLevel.Draw(_mSpriteBatch, gameTime, Scale);
                _mPause.Draw(_mSpriteBatch, MGraphics, Scale);
            }
            else if (_mCurrentState == GameStates.Victory)
            {
                //TODO - Change this to a victory animation
                _mCurrentLevel.Draw(_mSpriteBatch, gameTime, Scale);
                DrawHud();
            }
            else if (_mCurrentState == GameStates.Death)
            {
                //TODO - Change this to a death animation
                _mCurrentLevel.Draw(_mSpriteBatch, gameTime, Scale);
                DrawHud();
            }
            else if (_mCurrentState == GameStates.Title)
            {
                _mTitle.Draw(_mSpriteBatch, gameTime, Scale);
            }
            else if (_mCurrentState == GameStates.AfterScore)
            {
                _mScoring.Draw(_mSpriteBatch, MGraphics, _mCurrentLevel, Scale);
                _mAfterScore.Draw(_mSpriteBatch, MGraphics, Scale);
            }
            else if (_mCurrentState == GameStates.PreScore)
            {
                _mScoring.Draw(_mSpriteBatch, MGraphics, _mCurrentLevel, Scale);
                _mPreScore.Draw(_mSpriteBatch, MGraphics, Scale, _mCurrentLevel);
            }
            else if (_mCurrentState == GameStates.ResetConfirm)
            {
                _mResetConfirm.Draw(_mSpriteBatch, MGraphics, Scale);
            }
            else if (_mCurrentState == GameStates.StartLevelSplash)
            {
                _mCurrentLevel.Draw(_mSpriteBatch, gameTime, Scale);
                _mStartLevelSplash.Draw(_mSpriteBatch, MGraphics, _mCurrentLevel, Scale);
            }
            else if (_mCurrentState == GameStates.PurchaseScreen)
            {
                _mPurchaseScreenSplash.Draw(_mSpriteBatch, gameTime, Scale);
            }
            else if (_mCurrentState == GameStates.WorldPurchaseScreen)
            {
                _mWorldPurchaseScreenSplash.Draw(_mSpriteBatch, gameTime, Scale);
            }
                
            base.Draw(gameTime);
        }

        public void DrawHud()
        {
            Rectangle mScreenRect = MGraphics.GraphicsDevice.Viewport.TitleSafeArea;
            _mSpriteBatch.Begin();
           
            var goalString = "Goal: ";
            Vector2 goalLength = _mQuartz.MeasureString(goalString);
            
            /* TIMER VARIABLES*/
            var timerString = "Timer: ";
            var timeString = _mCurrentLevel.Timer.ToString();
            var timeGoal = _mCurrentLevel.IdealTime.ToString();
            Vector2 timerLength = _mQuartz.MeasureString(timerString);
            Vector2 numberLength = _mQuartz.MeasureString("99   ");

            /* COLLECTED VARIABLES*/
            var collectedString = "Collected: ";
            var collectString = _mCurrentLevel.NumCollected.ToString();// + "/" + mCurrentLevel.NumCollectable;
            var collectGoal = _mCurrentLevel.NumCollectable.ToString();
            Vector2 collectedLength = _mQuartz.MeasureString(collectedString);

            /* DEATHS VARIABLE*/
            var livesString = "Lives:";
            //int lives = mCurrentLevel.NumLives;
            Vector2 livesLength = _mQuartz.MeasureString(livesString);

            //Draw Timer

            //HUDTrans block
            _mSpriteBatch.Draw(_mHudTrans, new Rectangle(mScreenRect.Left, mScreenRect.Top, 
                (int)timerLength.X + (int)numberLength.X, (int)timerLength.Y + (int)numberLength.Y), Color.White);

            var placement = new Vector2(mScreenRect.Left + 10, mScreenRect.Top);
            _mSpriteBatch.DrawString(_mQuartz, timerString, new Vector2(placement.X - 1, placement.Y - 1), Color.White);
            _mSpriteBatch.DrawString(_mQuartz, timerString, placement, Color.SteelBlue);
            _mSpriteBatch.DrawString(_mQuartz, timeString, new Vector2(placement.X + timerLength.X, placement.Y), Color.White);
            placement.Y += timerLength.Y;
            _mSpriteBatch.DrawString(_mQuartz, goalString, new Vector2(placement.X - 1, placement.Y - 1), Color.White);
            _mSpriteBatch.DrawString(_mQuartz, goalString, placement, Color.SteelBlue);
            _mSpriteBatch.DrawString(_mQuartz, timeGoal, new Vector2(placement.X + timerLength.X, placement.Y), Color.White);

            //Draw collected

            //HUDTrans block
            _mSpriteBatch.Draw(_mHudTrans, new Rectangle(mScreenRect.Right - (int)collectedLength.X - (int)numberLength.X, mScreenRect.Top,
               (int)collectedLength.X + (int)numberLength.X, (int)collectedLength.Y + (int)numberLength.Y), Color.White);
            
            placement = new Vector2(mScreenRect.Right - collectedLength.X - numberLength.X, mScreenRect.Top);
            _mSpriteBatch.DrawString(_mQuartz, collectedString, new Vector2(placement.X - 1, placement.Y - 1), Color.White);
            _mSpriteBatch.DrawString(_mQuartz, collectedString, placement, Color.SteelBlue);
            _mSpriteBatch.DrawString(_mQuartz, collectString, new Vector2(placement.X + collectedLength.X, placement.Y), Color.White);
            placement.Y += collectedLength.Y;
            _mSpriteBatch.DrawString(_mQuartz, goalString, new Vector2(placement.X - 1, placement.Y - 1), Color.White);
            _mSpriteBatch.DrawString(_mQuartz, goalString, placement, Color.SteelBlue);
            _mSpriteBatch.DrawString(_mQuartz, collectGoal, new Vector2(placement.X + collectedLength.X, placement.Y), Color.White);
            placement.Y += goalLength.Y;

            //Draw deaths

            //HUDTrans block
            _mSpriteBatch.Draw(_mHudTrans, new Rectangle(mScreenRect.Left, mScreenRect.Bottom - _mLives[0].Height - 10,
                (int)livesLength.X + (int)(_mLives[0].Width * 0.75f) * (_mCurrentLevel.NumLives+1), _mLives[0].Height + 5), Color.White);
            
            //Number lives
            placement = new Vector2(mScreenRect.Left + 10, mScreenRect.Bottom - _mLives[0].Height - 10);

            _mSpriteBatch.DrawString(_mQuartz, livesString, new Vector2(placement.X - 1, placement.Y - 1), Color.White);
            _mSpriteBatch.DrawString(_mQuartz, livesString, placement, Color.SteelBlue);

            placement = new Vector2(mScreenRect.Left + livesLength.X, mScreenRect.Bottom - _mLives[0].Height - 10);
            if (_mCurrentLevel.NumLives == 5)
            {
                _mSpriteBatch.Draw(_mLives[5], new Rectangle((int)(placement.X + (_mLives[0].Width * 0.75f)), (int) placement.Y, (int)(_mLives[0].Width * 0.6f), (int)(_mLives[0].Height * 0.6f)), Color.White);
                _mSpriteBatch.Draw(_mLives[5], new Rectangle((int)(placement.X + 2 * (_mLives[0].Width * 0.75f)), (int)placement.Y, (int)(_mLives[0].Width * 0.6f), (int)(_mLives[0].Height * 0.6f)), Color.White);
                _mSpriteBatch.Draw(_mLives[5], new Rectangle((int)(placement.X + 3 * (_mLives[0].Width * 0.75f)), (int)placement.Y, (int)(_mLives[0].Width * 0.6f), (int)(_mLives[0].Height * 0.6f)), Color.White);
                _mSpriteBatch.Draw(_mLives[5], new Rectangle((int)(placement.X + 4 * (_mLives[0].Width * 0.75f)), (int)placement.Y, (int)(_mLives[0].Width * 0.6f), (int)(_mLives[0].Height * 0.6f)), Color.White);
                _mSpriteBatch.Draw(_mLives[5], new Rectangle((int)(placement.X + 5 * (_mLives[0].Width * 0.75f)), (int)placement.Y, (int)(_mLives[0].Width * 0.6f), (int)(_mLives[0].Height * 0.6f)), Color.White);
            }
            else if (_mCurrentLevel.NumLives == 4)
            {
                _mSpriteBatch.Draw(_mLives[4], new Rectangle((int)(placement.X + (_mLives[0].Width * 0.75f)), (int)placement.Y, (int)(_mLives[0].Width * 0.6f), (int)(_mLives[0].Height * 0.6f)), Color.White);
                _mSpriteBatch.Draw(_mLives[4], new Rectangle((int)(placement.X + 2 * (_mLives[0].Width * 0.75f)), (int)placement.Y, (int)(_mLives[0].Width * 0.6f), (int)(_mLives[0].Height * 0.6f)), Color.White);
                _mSpriteBatch.Draw(_mLives[4], new Rectangle((int)(placement.X + 3 * (_mLives[0].Width * 0.75f)), (int)placement.Y, (int)(_mLives[0].Width * 0.6f), (int)(_mLives[0].Height * 0.6f)), Color.White);
                _mSpriteBatch.Draw(_mLives[4], new Rectangle((int)(placement.X + 4 * (_mLives[0].Width * 0.75f)), (int)placement.Y, (int)(_mLives[0].Width * 0.6f), (int)(_mLives[0].Height * 0.6f)), Color.White);
            }
            else if (_mCurrentLevel.NumLives == 3)
            {
                _mSpriteBatch.Draw(_mLives[3], new Rectangle((int)(placement.X + (_mLives[0].Width * 0.75f)), (int)placement.Y, (int)(_mLives[0].Width * 0.6f), (int)(_mLives[0].Height * 0.6f)), Color.White);
                _mSpriteBatch.Draw(_mLives[3], new Rectangle((int)(placement.X + 2 * (_mLives[0].Width * 0.75f)), (int)placement.Y, (int)(_mLives[0].Width * 0.6f), (int)(_mLives[0].Height * 0.6f)), Color.White);
                _mSpriteBatch.Draw(_mLives[3], new Rectangle((int)(placement.X + 3 * (_mLives[0].Width * 0.75f)), (int)placement.Y, (int)(_mLives[0].Width * 0.6f), (int)(_mLives[0].Height * 0.6f)), Color.White);
            }
            else if (_mCurrentLevel.NumLives == 2)
            {
                _mSpriteBatch.Draw(_mLives[2], new Rectangle((int)(placement.X + (_mLives[0].Width * 0.75f)), (int)placement.Y, (int)(_mLives[0].Width * 0.6f), (int)(_mLives[0].Height * 0.6f)), Color.White);
                _mSpriteBatch.Draw(_mLives[2], new Rectangle((int)(placement.X + 2 * (_mLives[0].Width * 0.75f)), (int)placement.Y, (int)(_mLives[0].Width * 0.6f), (int)(_mLives[0].Height * 0.6f)), Color.White);
            }
            else if (_mCurrentLevel.NumLives == 1)
            {
                _mSpriteBatch.Draw(_mLives[1], new Rectangle((int)(placement.X + (_mLives[0].Width * 0.75f)), (int)placement.Y, (int)(_mLives[0].Width * 0.6f), (int)(_mLives[0].Height * 0.6f)), Color.White);
            }
            else if (_mCurrentLevel.NumLives == 0)
            {
                _mSpriteBatch.Draw(_mLives[0], new Rectangle((int)(placement.X + (_mLives[0].Width * 0.75f)), (int)placement.Y, (int)(_mLives[0].Width * 0.6f), (int)(_mLives[0].Height * 0.6f)), Color.White);
            }


            if (_mCurrentLevel.NumLives <= 0)
            {
                var request = "Out of Lives       Press A to Restart";

                Vector2 stringSize = _mQuartz.MeasureString(request);
                _mSpriteBatch.DrawString(_mQuartz, request, new Vector2(mScreenRect.Center.X - stringSize.X/2, mScreenRect.Center.Y), Color.DarkTurquoise);
                _mSpriteBatch.DrawString(_mQuartz, request, new Vector2(mScreenRect.Center.X - stringSize.X / 2 + 2, mScreenRect.Center.Y + 2), Color.White);

            }
            _mSpriteBatch.End();
        }
    }
}
