using System;
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
    ///     This is the main type for your game
    /// </summary>
    public class MrGravityMain : Game
    {
        #region Fields

        public static GraphicsDeviceManager Graphics;
        private SpriteBatch _mSpriteBatch;

        // _mScale - Used to make sure the HUD is drawn based on the screen size
        private Matrix _mScale;

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
        private const int VictoryDuration = 120;
        private const int DeathDuration = 50;

        //Duration of a sequence
        private int _mSequence;

        //Boolean toggle variable
        private bool _mToggledSequence;

        private readonly IControlScheme _mControls;

        //Current level
        private Level _mCurrentLevel;

        //Fonts for this game
        private SpriteFont _mQuartz;

        //Transparant HUD
        private Texture2D _mHudTrans;

        //Lives
        private Texture2D[] _mLives;
     
        private string _mLevelLocation;

        #endregion

        #region Properties

        //TO BE CHANGED- Actually, this may be ok since we use this to play test.
        public string LevelLocation
        {
            get { return _mLevelLocation; }
            set { _mLevelLocation = @"..\..\..\Content\Levels\" + value; }
        }

        public GameStates CurrentState
        {
            get { return _mCurrentState; }
            set { _mCurrentState = value; }
        }
        #endregion

        #region Constructors

        /// <summary>
        ///     Constructs a new game.
        /// </summary>
        public MrGravityMain()
        {
            Components.Add(new GamerServicesComponent(this));
            Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            if (GamePad.GetState(PlayerIndex.One).IsConnected || GamePad.GetState(PlayerIndex.Two).IsConnected ||
                GamePad.GetState(PlayerIndex.Three).IsConnected || GamePad.GetState(PlayerIndex.Four).IsConnected)
                _mControls = new ControllerControl();
            else
                _mControls = new KeyboardControl();

            Graphics.GraphicsProfile = GraphicsProfile.HiDef;
            LevelLocation = "DefaultLevel.xml";
        }

        #endregion

        #region Initializaation Methods

        /// <summary>
        ///     Allows the game to perform any initialization it needs to before starting to run.
        ///     This is where it c5an query for any required services and load any non-graphic
        ///     related content.  Calling base.Initialize will enumerate through any components
        ///     and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            //COMMENT OUT AFTER TESTING TRIAL MODE
            //Guide.SimulateTrialMode = true;

            Graphics.PreferredBackBufferWidth = 1280;
            Graphics.PreferredBackBufferHeight = 720;
            //Graphics.ToggleFullScreen();// REMEMBER TO RESET AFTER DEBUGGING!!!!!!!!!
            Graphics.ApplyChanges();

            _mTitle = new Title(_mControls, Graphics);
            _mMainMenu = new MainMenu(_mControls, Graphics);
            _mWorldSelect = new WorldSelect(_mControls, Graphics);

            _mPause = new Pause(_mControls);
            _mCredits = new Credits(_mControls, Graphics);
            _mOptions = new Options(_mControls, Graphics);
            _mAfterScore = new AfterScore(_mControls);
            _mResetConfirm = new ResetConfirm(_mControls);
            _mStartLevelSplash = new StartLevelSplash(_mControls);

            _mPreScore = new PreScore(_mControls);
            _mScoring = new Scoring(_mControls);

            _mController = new Controller(_mControls, Graphics);
            _mSoundOptions = new SoundOptions(_mControls, Graphics);
            _mPurchaseScreenSplash = new PurchaseScreenSplash(_mControls, Graphics);
            _mWorldPurchaseScreenSplash = new WorldPurchaseSplash(_mControls, Graphics);

            _mSpriteBatch = new SpriteBatch(Graphics.GraphicsDevice);
            base.Initialize();

            Components.Add(new GamerServicesComponent(this));
        }

        /// <summary>
        ///     LoadContent will be called once per game and is the place to load
        ///     all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // current viewport
            var screenscaleX = Graphics.GraphicsDevice.Viewport.Width / 1280.0f;
            var screenscaleY = Graphics.GraphicsDevice.Viewport.Height / 720.0f;

            // Create the scale transform for Draw. 
            // Do not scale the sprite depth (Z=1).
            _mScale = Matrix.CreateScale(screenscaleX, screenscaleY, 1);

            _mTitle.Load(Content, Graphics.GraphicsDevice);

            _mMainMenu.Load(Content);
            _mMainMenuLevel = Level.MainMenuLevel("Content\\Levels\\MainMenu.xml", _mControls, Graphics.GraphicsDevice.Viewport, _mMainMenu.GetInnerRegion());

            _mMainMenuLevel.Load(Content);
            _mCredits.Load(Content);
            _mOptions.Load(Content);
            
            _mPause.Load(Content);
            GameSound.Load(Content);
            _mCurrentLevel = new Level(_mLevelLocation, _mControls, GraphicsDevice.Viewport);
            _mCurrentLevel.Load(Content);

            _mWorldSelect.Load(Content);
            _mScoring.Load(Content, Graphics.GraphicsDevice, _mWorldSelect);
            _mAfterScore.Load(Content, GraphicsDevice);
            _mPreScore.Load(Content, GraphicsDevice, _mWorldSelect);
            _mResetConfirm.Load(Content);
            _mController.Load(Content);
            _mSoundOptions.Load(Content);
            _mStartLevelSplash.Load(Content, GraphicsDevice);
            _mPurchaseScreenSplash.Load(Content, GraphicsDevice);
            _mWorldPurchaseScreenSplash.Load(Content, GraphicsDevice);
            
            _mSpriteBatch = new SpriteBatch(GraphicsDevice);

            Content.Load<SpriteFont>("Fonts/Kootenay");

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

        #endregion

        #region OnExit Methods

        /// <summary>
        ///     UnloadContent will be called once per game and is the place to unload
        ///     all content.
        /// </summary>
        protected override void UnloadContent() {}

        /// <summary>
        ///     Handle when the game exits
        /// </summary>
        /// <param name="sender">Typical event stuff</param>
        /// <param name="args">Typical event stuff</param>
        protected override void OnExiting(object sender, EventArgs args)
        {
            _mWorldSelect.Save();
        }

        #endregion

        #region Update Methods

        /// <summary>
        ///     Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            switch (_mCurrentState)
            {
                case GameStates.Credits:
                    _mCredits.Update(gameTime, ref _mCurrentState);
                    break;
                case GameStates.Options:
                    _mOptions.Update(gameTime, ref _mCurrentState, _mMainMenuLevel);
                    _mMainMenuLevel.Update(gameTime, ref _mCurrentState);
                    break;
                case GameStates.Title:
                    //Check for mute
                    GameSound.MenuMusicTitle.Volume = GameSound.Volume;

                    //If the correct music isn't already playing
                    if (GameSound.MenuMusicTitle.State != SoundState.Playing)
                    {
                        GameSound.StopOthersAndPlay(GameSound.MenuMusicTitle);
                    }

                    _mTitle.Update(gameTime, ref _mCurrentState);
                    break;
                case GameStates.InGame:
                    _mCurrentLevel.Update(gameTime, ref _mCurrentState);
                    break;
                case GameStates.MainMenu:
                    //Check for mute
                    GameSound.MenuMusicTitle.Volume = GameSound.Volume;

                    //If the correct music isn't already playing
                    if (GameSound.MenuMusicTitle.State != SoundState.Playing)
                        GameSound.StopOthersAndPlay(GameSound.MenuMusicTitle);

                    _mMainMenu.Update(gameTime, ref _mCurrentState, _mMainMenuLevel);
                    _mMainMenuLevel.Update(gameTime, ref _mCurrentState);
                    break;
                case GameStates.Score:
                    //Check for mute
                    GameSound.MenuMusicTitle.Volume = GameSound.Volume;
                    GameSound.LevelStageVictory.Volume = GameSound.Volume*.75f;

                    //First play win, then menu
                    if (GameSound.LevelStageVictory.State != SoundState.Playing)
                    {
                        if (GameSound.MenuMusicTitle.State != SoundState.Playing)
                        {
                            GameSound.StopOthersAndPlay(GameSound.MenuMusicTitle);
                        }
                    }

                    _mWorldSelect.UpdateStarCount();

                    _mScoring.Update(gameTime, ref _mCurrentState, ref _mCurrentLevel, _mWorldSelect);
                    break;
                case GameStates.LevelSelection:
                    //Check for mute
                    GameSound.MenuMusicTitle.Volume = GameSound.Volume;

                    //If the correct music isn't already playing
                    if (GameSound.MenuMusicTitle.State != SoundState.Playing)
                    {
                        GameSound.StopOthersAndPlay(GameSound.MenuMusicTitle);
                    }

                    _mWorldSelect.Update(gameTime, ref _mCurrentState, ref _mCurrentLevel);
                    break;
                case GameStates.NewLevelSelection:
                    //Check for mute
                    GameSound.MenuMusicTitle.Volume = GameSound.Volume;

                    //If the correct music isn't already playing
                    if (GameSound.MenuMusicTitle.State != SoundState.Playing)
                    {
                        GameSound.StopOthersAndPlay(GameSound.MenuMusicTitle);
                    }

                    _mWorldSelect.Reset();

                    _mCurrentState = GameStates.Options;
                    break;
                case GameStates.Pause:
                    _mPause.Update(gameTime, ref _mCurrentState, ref _mCurrentLevel);
                    break;
                case GameStates.Controls:
                    _mController.Update(gameTime, ref _mCurrentState);
                    break;
                case GameStates.SoundOptions:
                    _mSoundOptions.Update(gameTime, ref _mCurrentState);
                    break;
                case GameStates.Unlock:
                    //Update the stars in level
                    //Update star count
                    _mCurrentLevel.UpdateStars();

                    _mSequence = VictoryDuration;
                    _mCurrentState = GameStates.Victory;
                    break;
                case GameStates.NextLevel:
                    _mCurrentState = GameStates.LevelSelection;
                    break;
                case GameStates.Victory:
                    _mSequence--;

                    if (_mSequence <= 0)
                    {
                        if ((_mCurrentLevel.CollectionStar == 3 && (_mWorldSelect.GetLevelCollect()) != 3) ||
                            (_mCurrentLevel.DeathStar == 3 && (_mWorldSelect.GetLevelDeath() != 3)) ||
                            (_mCurrentLevel.TimerStar == 3 && (_mWorldSelect.GetLevelTime() != 3)))
                        {
                            _mCurrentState = GameStates.PreScore;
                        }
                        else
                        {
                            _mCurrentState = GameStates.Score;
                        }
                    }
                    break;
                case GameStates.Death:
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
                    break;
                case GameStates.Exit:
                    _mCurrentState = GameStates.WaitingToExit;
                    break;
                case GameStates.PurchaseScreen:
                    _mPurchaseScreenSplash.Update(gameTime, ref _mCurrentState);
                    break;
                case GameStates.WorldPurchaseScreen:
                    _mWorldPurchaseScreenSplash.Update(gameTime, ref _mCurrentState);
                    break;
                case GameStates.TrialExit:
                    if (Gamer.SignedInGamers[((ControllerControl) _mControls).ControllerIndex] == null)
                    {
                        Guide.ShowSignIn(1, true);
                        _mCurrentState = GameStates.WaitingForSignIn;
                    }
                    else
                    {
                        _mCurrentState = GameStates.ShowMarketplace;
                    }
                    break;
                case GameStates.WorldPurchase:
                    if (Gamer.SignedInGamers[((ControllerControl) _mControls).ControllerIndex] == null)
                    {
                        Guide.ShowSignIn(1, true);
                        _mCurrentState = GameStates.WorldWaitingForSignIn;
                    }
                    else
                    {
                        _mCurrentState = GameStates.WorldShowMarketplace;
                    }
                    break;
                case GameStates.WaitingForSignIn:
                    if (!Guide.IsVisible)
                    {
                        _mCurrentState = Gamer.SignedInGamers[((ControllerControl) _mControls).ControllerIndex] == null 
                            ? GameStates.WaitingToExit 
                            : GameStates.ShowMarketplace;
                    }
                    break;
                case GameStates.WorldWaitingForSignIn:
                    if (!Guide.IsVisible)
                    {
                        _mCurrentState = Gamer.SignedInGamers[((ControllerControl) _mControls).ControllerIndex] == null 
                            ? GameStates.LevelSelection 
                            : GameStates.WorldShowMarketplace;
                    }
                    break;
                case GameStates.WaitingForSaveSignIn:
                    if (!Guide.IsVisible)
                    {
                        _mCurrentState = GameStates.MainMenu;
                    }
                    break;
                case GameStates.ShowMarketplace:
                    if (Gamer.SignedInGamers[((ControllerControl) _mControls).ControllerIndex].IsSignedInToLive)
                    {
                        if (
                            Gamer.SignedInGamers[((ControllerControl) _mControls).ControllerIndex].Privileges
                                .AllowPurchaseContent)
                        {
                            Guide.ShowMarketplace(((ControllerControl) _mControls).ControllerIndex);
                            _mCurrentState = GameStates.WaitForMarketplace;
                        }
                        else
                        {
                            _mCurrentState = GameStates.WaitingToExit;
                        }
                    }
                    else
                    {
                        _mCurrentState = GameStates.WaitingToExit;
                    }
                    break;
                case GameStates.WorldShowMarketplace:
                    if (Gamer.SignedInGamers[((ControllerControl) _mControls).ControllerIndex].IsSignedInToLive)
                    {
                        if (
                            Gamer.SignedInGamers[((ControllerControl) _mControls).ControllerIndex].Privileges
                                .AllowPurchaseContent)
                        {
                            Guide.ShowMarketplace(((ControllerControl) _mControls).ControllerIndex);
                            _mCurrentState = GameStates.WorldWaitForMarketplace;
                        }
                        else
                        {
                            _mCurrentState = GameStates.LevelSelection;
                        }
                    }
                    else
                    {
                        _mCurrentState = GameStates.WaitingToExit;
                    }
                    break;
                case GameStates.WaitForMarketplace:
                    if (!Guide.IsVisible)
                    {
                        _mCurrentState = !Guide.IsTrialMode
                            ? GameStates.MainMenu
                            : GameStates.WaitingToExit;
                    }
                    break;
                case GameStates.WorldWaitForMarketplace:
                    if (!Guide.IsVisible)
                    {
                        _mCurrentState = GameStates.LevelSelection;
                    }
                    break;
                case GameStates.WaitingToExit:
                    if (!Guide.IsVisible)
                    {
                        Exit();
                    }
                    break;
                case GameStates.AfterScore:
                    _mAfterScore.Update(gameTime, ref _mCurrentState, ref _mCurrentLevel);
                    break;
                case GameStates.PreScore:
                    //Check for mute
                    GameSound.MenuMusicTitle.Volume = GameSound.Volume;
                    GameSound.LevelStageVictory.Volume = GameSound.Volume*.75f;

                    //First play win, then menu
                    if (GameSound.LevelStageVictory.State != SoundState.Playing)
                    {
                        if (GameSound.MenuMusicTitle.State != SoundState.Playing)
                        {
                            GameSound.StopOthersAndPlay(GameSound.MenuMusicTitle);
                        }
                    }

                    _mPreScore.Update(gameTime, ref _mCurrentState, ref _mCurrentLevel);
                    break;
                case GameStates.ResetConfirm:
                    _mResetConfirm.Update(gameTime, ref _mCurrentState, ref _mCurrentLevel);
                    break;
                case GameStates.StartLevelSplash:
                    _mStartLevelSplash.Update(gameTime, ref _mCurrentState);
                    break;
            }
        }


        #endregion

        #region Drawing Methods

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            switch (_mCurrentState)
            {
                case GameStates.InGame:
                    _mCurrentLevel.Draw(_mSpriteBatch, gameTime, _mScale);
                    DrawHud();
                    break;
                case GameStates.MainMenu:
                    _mMainMenu.Draw(gameTime, _mSpriteBatch, _mScale);
                    _mMainMenuLevel.Draw(_mSpriteBatch, gameTime, _mScale);
                    break;
                case GameStates.Credits:
                    _mCredits.Draw(gameTime, _mSpriteBatch, _mScale);
                    _mCredits.DrawLevel(_mSpriteBatch, gameTime, _mScale);
                    break;
                case GameStates.Options:
                    _mOptions.Draw(gameTime, _mSpriteBatch, _mScale);
                    _mMainMenuLevel.Draw(_mSpriteBatch, gameTime, _mScale);
                    break;
                case GameStates.Controls:
                    _mController.Draw(gameTime, _mSpriteBatch, _mScale);
                    break;
                case GameStates.SoundOptions:
                    _mSoundOptions.Draw(gameTime, _mSpriteBatch, _mScale);
                    break;
                case GameStates.Score:
                    _mScoring.Draw(_mSpriteBatch, Graphics, _mCurrentLevel, _mScale);
                    break;
                case GameStates.LevelSelection:
                    _mWorldSelect.Draw(_mSpriteBatch, _mScale);
                    break;
                case GameStates.Pause:
                    _mCurrentLevel.Draw(_mSpriteBatch, gameTime, _mScale);
                    _mPause.Draw(_mSpriteBatch, Graphics, _mScale);
                    break;
                case GameStates.Victory:
                    //TODO - Change this to a victory animation
                    _mCurrentLevel.Draw(_mSpriteBatch, gameTime, _mScale);
                    DrawHud();
                    break;
                case GameStates.Death:
                    //TODO - Change this to a death animation
                    _mCurrentLevel.Draw(_mSpriteBatch, gameTime, _mScale);
                    DrawHud();
                    break;
                case GameStates.Title:
                    _mTitle.Draw(_mSpriteBatch, gameTime, _mScale);
                    break;
                case GameStates.AfterScore:
                    _mScoring.Draw(_mSpriteBatch, Graphics, _mCurrentLevel, _mScale);
                    _mAfterScore.Draw(_mSpriteBatch, Graphics, _mScale);
                    break;
                case GameStates.PreScore:
                    _mScoring.Draw(_mSpriteBatch, Graphics, _mCurrentLevel, _mScale);
                    _mPreScore.Draw(_mSpriteBatch, Graphics, _mScale, _mCurrentLevel);
                    break;
                case GameStates.ResetConfirm:
                    _mResetConfirm.Draw(_mSpriteBatch, Graphics, _mScale);
                    break;
                case GameStates.StartLevelSplash:
                    _mCurrentLevel.Draw(_mSpriteBatch, gameTime, _mScale);
                    _mStartLevelSplash.Draw(_mSpriteBatch, Graphics, _mCurrentLevel, _mScale);
                    break;
                case GameStates.PurchaseScreen:
                    _mPurchaseScreenSplash.Draw(_mSpriteBatch, gameTime, _mScale);
                    break;
                case GameStates.WorldPurchaseScreen:
                    _mWorldPurchaseScreenSplash.Draw(_mSpriteBatch, gameTime, _mScale);
                    break;
            }
                
            base.Draw(gameTime);
        }

        public void DrawHud()
        {
            Rectangle mScreenRect = Graphics.GraphicsDevice.Viewport.TitleSafeArea;
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
            var collectString = _mCurrentLevel.NumCollected.ToString(); // + "/" + mCurrentLevel.NumCollectable;
            var collectGoal = _mCurrentLevel.NumCollectable.ToString();
            Vector2 collectedLength = _mQuartz.MeasureString(collectedString);

            /* DEATHS VARIABLE*/
            var livesString = "Lives:";
            //int lives = mCurrentLevel.NumLives;
            Vector2 livesLength = _mQuartz.MeasureString(livesString);

            //Draw Timer

            //HUDTrans block
            _mSpriteBatch.Draw(_mHudTrans, new Rectangle(mScreenRect.Left, mScreenRect.Top,
                (int) timerLength.X + (int) numberLength.X, (int) timerLength.Y + (int) numberLength.Y), Color.White);

            var placement = new Vector2(mScreenRect.Left + 10, mScreenRect.Top);
            _mSpriteBatch.DrawString(_mQuartz, timerString, new Vector2(placement.X - 1, placement.Y - 1), Color.White);
            _mSpriteBatch.DrawString(_mQuartz, timerString, placement, Color.SteelBlue);
            _mSpriteBatch.DrawString(_mQuartz, timeString, new Vector2(placement.X + timerLength.X, placement.Y),
                Color.White);
            placement.Y += timerLength.Y;
            _mSpriteBatch.DrawString(_mQuartz, goalString, new Vector2(placement.X - 1, placement.Y - 1), Color.White);
            _mSpriteBatch.DrawString(_mQuartz, goalString, placement, Color.SteelBlue);
            _mSpriteBatch.DrawString(_mQuartz, timeGoal, new Vector2(placement.X + timerLength.X, placement.Y),
                Color.White);

            //Draw collected

            //HUDTrans block
            _mSpriteBatch.Draw(_mHudTrans,
                new Rectangle(mScreenRect.Right - (int) collectedLength.X - (int) numberLength.X, mScreenRect.Top,
                    (int) collectedLength.X + (int) numberLength.X, (int) collectedLength.Y + (int) numberLength.Y),
                Color.White);

            placement = new Vector2(mScreenRect.Right - collectedLength.X - numberLength.X, mScreenRect.Top);
            _mSpriteBatch.DrawString(_mQuartz, collectedString, new Vector2(placement.X - 1, placement.Y - 1),
                Color.White);
            _mSpriteBatch.DrawString(_mQuartz, collectedString, placement, Color.SteelBlue);
            _mSpriteBatch.DrawString(_mQuartz, collectString, new Vector2(placement.X + collectedLength.X, placement.Y),
                Color.White);
            placement.Y += collectedLength.Y;
            _mSpriteBatch.DrawString(_mQuartz, goalString, new Vector2(placement.X - 1, placement.Y - 1), Color.White);
            _mSpriteBatch.DrawString(_mQuartz, goalString, placement, Color.SteelBlue);
            _mSpriteBatch.DrawString(_mQuartz, collectGoal, new Vector2(placement.X + collectedLength.X, placement.Y),
                Color.White);
            placement.Y += goalLength.Y;

            //Draw deaths

            //HUDTrans block
            _mSpriteBatch.Draw(_mHudTrans, new Rectangle(mScreenRect.Left, mScreenRect.Bottom - _mLives[0].Height - 10,
                (int) livesLength.X + (int) (_mLives[0].Width*0.75f)*(_mCurrentLevel.NumLives + 1),
                _mLives[0].Height + 5), Color.White);

            //Number lives
            placement = new Vector2(mScreenRect.Left + 10, mScreenRect.Bottom - _mLives[0].Height - 10);

            _mSpriteBatch.DrawString(_mQuartz, livesString, new Vector2(placement.X - 1, placement.Y - 1), Color.White);
            _mSpriteBatch.DrawString(_mQuartz, livesString, placement, Color.SteelBlue);

            placement = new Vector2(mScreenRect.Left + livesLength.X, mScreenRect.Bottom - _mLives[0].Height - 10);
            if (_mCurrentLevel.NumLives == 5)
            {
                _mSpriteBatch.Draw(_mLives[5],
                    new Rectangle((int) (placement.X + (_mLives[0].Width*0.75f)), (int) placement.Y,
                        (int) (_mLives[0].Width*0.6f), (int) (_mLives[0].Height*0.6f)), Color.White);
                _mSpriteBatch.Draw(_mLives[5],
                    new Rectangle((int) (placement.X + 2*(_mLives[0].Width*0.75f)), (int) placement.Y,
                        (int) (_mLives[0].Width*0.6f), (int) (_mLives[0].Height*0.6f)), Color.White);
                _mSpriteBatch.Draw(_mLives[5],
                    new Rectangle((int) (placement.X + 3*(_mLives[0].Width*0.75f)), (int) placement.Y,
                        (int) (_mLives[0].Width*0.6f), (int) (_mLives[0].Height*0.6f)), Color.White);
                _mSpriteBatch.Draw(_mLives[5],
                    new Rectangle((int) (placement.X + 4*(_mLives[0].Width*0.75f)), (int) placement.Y,
                        (int) (_mLives[0].Width*0.6f), (int) (_mLives[0].Height*0.6f)), Color.White);
                _mSpriteBatch.Draw(_mLives[5],
                    new Rectangle((int) (placement.X + 5*(_mLives[0].Width*0.75f)), (int) placement.Y,
                        (int) (_mLives[0].Width*0.6f), (int) (_mLives[0].Height*0.6f)), Color.White);
            }
            else if (_mCurrentLevel.NumLives == 4)
            {
                _mSpriteBatch.Draw(_mLives[4],
                    new Rectangle((int) (placement.X + (_mLives[0].Width*0.75f)), (int) placement.Y,
                        (int) (_mLives[0].Width*0.6f), (int) (_mLives[0].Height*0.6f)), Color.White);
                _mSpriteBatch.Draw(_mLives[4],
                    new Rectangle((int) (placement.X + 2*(_mLives[0].Width*0.75f)), (int) placement.Y,
                        (int) (_mLives[0].Width*0.6f), (int) (_mLives[0].Height*0.6f)), Color.White);
                _mSpriteBatch.Draw(_mLives[4],
                    new Rectangle((int) (placement.X + 3*(_mLives[0].Width*0.75f)), (int) placement.Y,
                        (int) (_mLives[0].Width*0.6f), (int) (_mLives[0].Height*0.6f)), Color.White);
                _mSpriteBatch.Draw(_mLives[4],
                    new Rectangle((int) (placement.X + 4*(_mLives[0].Width*0.75f)), (int) placement.Y,
                        (int) (_mLives[0].Width*0.6f), (int) (_mLives[0].Height*0.6f)), Color.White);
            }
            else if (_mCurrentLevel.NumLives == 3)
            {
                _mSpriteBatch.Draw(_mLives[3],
                    new Rectangle((int) (placement.X + (_mLives[0].Width*0.75f)), (int) placement.Y,
                        (int) (_mLives[0].Width*0.6f), (int) (_mLives[0].Height*0.6f)), Color.White);
                _mSpriteBatch.Draw(_mLives[3],
                    new Rectangle((int) (placement.X + 2*(_mLives[0].Width*0.75f)), (int) placement.Y,
                        (int) (_mLives[0].Width*0.6f), (int) (_mLives[0].Height*0.6f)), Color.White);
                _mSpriteBatch.Draw(_mLives[3],
                    new Rectangle((int) (placement.X + 3*(_mLives[0].Width*0.75f)), (int) placement.Y,
                        (int) (_mLives[0].Width*0.6f), (int) (_mLives[0].Height*0.6f)), Color.White);
            }
            else if (_mCurrentLevel.NumLives == 2)
            {
                _mSpriteBatch.Draw(_mLives[2],
                    new Rectangle((int) (placement.X + (_mLives[0].Width*0.75f)), (int) placement.Y,
                        (int) (_mLives[0].Width*0.6f), (int) (_mLives[0].Height*0.6f)), Color.White);
                _mSpriteBatch.Draw(_mLives[2],
                    new Rectangle((int) (placement.X + 2*(_mLives[0].Width*0.75f)), (int) placement.Y,
                        (int) (_mLives[0].Width*0.6f), (int) (_mLives[0].Height*0.6f)), Color.White);
            }
            else if (_mCurrentLevel.NumLives == 1)
            {
                _mSpriteBatch.Draw(_mLives[1],
                    new Rectangle((int) (placement.X + (_mLives[0].Width*0.75f)), (int) placement.Y,
                        (int) (_mLives[0].Width*0.6f), (int) (_mLives[0].Height*0.6f)), Color.White);
            }
            else if (_mCurrentLevel.NumLives == 0)
            {
                _mSpriteBatch.Draw(_mLives[0],
                    new Rectangle((int) (placement.X + (_mLives[0].Width*0.75f)), (int) placement.Y,
                        (int) (_mLives[0].Width*0.6f), (int) (_mLives[0].Height*0.6f)), Color.White);
            }


            if (_mCurrentLevel.NumLives <= 0)
            {
                var request = "Out of Lives       Press A to Restart";

                Vector2 stringSize = _mQuartz.MeasureString(request);
                _mSpriteBatch.DrawString(_mQuartz, request,
                    new Vector2(mScreenRect.Center.X - stringSize.X/2, mScreenRect.Center.Y), Color.DarkTurquoise);
                _mSpriteBatch.DrawString(_mQuartz, request,
                    new Vector2(mScreenRect.Center.X - stringSize.X/2 + 2, mScreenRect.Center.Y + 2), Color.White);

            }
            _mSpriteBatch.End();
        }

        #endregion
    }
}
