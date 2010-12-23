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
        GraphicsDeviceManager mGraphics;
        SpriteBatch mSpriteBatch;

        //Instance of the Menu class
        Menu mMenu;

        /* Animated Sprite */
        private AnimatedSprite mBlackHole;

        /* Tracks the previous zoom of the camera */
        private float mPrevZoom;

        //Instance of the scoring class
        Scoring mScoring;

        /* Volume variable - used to mute and unmute the sound effects */
        private static float VOLUME;

        /* Timer variable */
        private static double TIMER;

        /* Textures for HUD */
        private Texture2D[] mDirections;
        private Texture2D[] mLives;

        //Keep track of game state
        public enum GameStates { IN_GAME, IN_MENU, IN_SCORE };
        private static GameStates mGameState = GameStates.IN_MENU;
        public static GameStates GameState { get { return mGameState; } set { mGameState = value; } }

        // Camera
        public static Camera mCam;
        public static Camera mCam1;

        // see if we are paused
        private static bool mPaused;

        // check for press of pause button
        private bool mWasDown = false;

        //List of objects that comform to the game physics
        List<GameObject> mObjects;
        List<GameObject> mCollected;

        List<Trigger> mTrigger;

        Player mPlayer;

        //Current level
        Level mCurrentLevel;
        PhysicsEnvironment mPhysicsEnvironment;

        //Fonts for this game
        SpriteFont mDefaultFont;
        SpriteFont kootenay;

        private string mLevelLocation = "..\\..\\..\\Content\\Levels\\Level A.xml";
        public string LevelLocation { get { return mLevelLocation; } set { mLevelLocation = "..\\..\\..\\Content\\Levels\\" + value; } }

        GamePadState mPrevGamepad;
        KeyboardState mPrevKeyboard;

        public GravityShiftMain()
        {
            mGraphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it c5an query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            VOLUME = 1.0f;

            TIMER = 0;
            mBlackHole = new AnimatedSprite();

            mPrevGamepad = GamePad.GetState(PlayerIndex.One);
            mPrevKeyboard = Keyboard.GetState();

            kootenay = Content.Load<SpriteFont>("fonts/Kootenay");

            mCam = new Camera(GraphicsDevice.Viewport);
            mCam1 = new Camera(GraphicsDevice.Viewport);

            mGraphics.PreferredBackBufferWidth = mGraphics.GraphicsDevice.DisplayMode.Width;
            mGraphics.PreferredBackBufferHeight = mGraphics.GraphicsDevice.DisplayMode.Height;
            //mGraphics.ToggleFullScreen();// REMEMBER TO RESET AFTER DEBUGGING!!!!!!!!!
            mGraphics.ApplyChanges();

            mObjects = new List<GameObject>();
            mCollected = new List<GameObject>();
            mTrigger = new List<Trigger>();
            mPhysicsEnvironment = new PhysicsEnvironment();

            mMenu = new Menu();
            mScoring = new Scoring();

            mPaused = false;

            mDirections = new Texture2D[4];
            mLives = new Texture2D[10];

            base.Initialize();
        }

        public static double Timer
        {
            get { return TIMER; }
            set { TIMER = value; }
        }

        public static float Volume
        {
            get { return VOLUME; }
            set { VOLUME = value; }
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            mBlackHole.Load(Content, "Blackhole", 3, 0.1f);

            //Load direction textures
            mDirections[0] = Content.Load<Texture2D>("HUD/arrow_up");
            mDirections[1] = Content.Load<Texture2D>("HUD/arrow_right");
            mDirections[2] = Content.Load<Texture2D>("HUD/arrow_down");
            mDirections[3] = Content.Load<Texture2D>("HUD/arrow_left");

            //Load life textures
            mLives[0] = Content.Load<Texture2D>("HUD/NeonLifeCount0");
            mLives[1] = Content.Load<Texture2D>("HUD/NeonLifeCount1");
            mLives[2] = Content.Load<Texture2D>("HUD/NeonLifeCount2");
            mLives[3] = Content.Load<Texture2D>("HUD/NeonLifeCount3");
            mLives[4] = Content.Load<Texture2D>("HUD/NeonLifeCount4");
            mLives[5] = Content.Load<Texture2D>("HUD/NeonLifeCount5");
            mLives[6] = Content.Load<Texture2D>("HUD/NeonLifeCount6");
            mLives[7] = Content.Load<Texture2D>("HUD/NeonLifeCount7");
            mLives[8] = Content.Load<Texture2D>("HUD/NeonLifeCount8");
            mLives[9] = Content.Load<Texture2D>("HUD/NeonLifeCount9");

            mMenu.Load(Content);
            mScoring.Load(Content);
            GameSound.Load(Content);
            // Create a new SpriteBatch, which can be used to draw textures.
            mSpriteBatch = new SpriteBatch(GraphicsDevice);

            //Should eventually not require 
            Importer importer = new Importer(Content);
            mCurrentLevel = importer.ImportLevel(LevelLocation);

            mDefaultFont = Content.Load<SpriteFont>("fonts/Kootenay");

            mPlayer = new Player(Content, ref mPhysicsEnvironment, 
                new KeyboardControl(), .8f, EntityInfo.CreatePlayerInfo(GridSpace.GetGridCoord(mCurrentLevel.StartingPoint)));

            mObjects.Add(mPlayer);

            mObjects.AddRange(importer.GetObjects(ref mPhysicsEnvironment));

            mObjects.Add(importer.GetPlayerEnd());

            mTrigger.AddRange(importer.GetTriggers());
    }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent() {}

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            /* Keyboard and GamePad states */
            KeyboardState keyboard = Keyboard.GetState();
            GamePadState gamepad = GamePad.GetState(PlayerIndex.One);

            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed
                || keyboard.IsKeyDown(Keys.Escape))
                this.Exit();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Start == ButtonState.Pressed || keyboard.IsKeyDown(Keys.Space))
            {
                if (!mWasDown)
                {
                    mPaused = !mPaused;// toggle pause
                }
            }

            mWasDown =(GamePad.GetState(PlayerIndex.One).Buttons.Start == ButtonState.Pressed || keyboard.IsKeyDown(Keys.Space));

            if (mGameState == GameStates.IN_GAME)
            {
                mBlackHole.Update((float)gameTime.ElapsedGameTime.TotalSeconds);

                TIMER += (gameTime.ElapsedGameTime.TotalSeconds);
                Random rand = new Random();
                PhysicsEnvironment environment = new PhysicsEnvironment();
                environment.TerminalSpeed = 20;
                environment.GravityMagnitude = 1;
                if ((mPlayer.mIsAlive) && !mPaused)// only update while player is alive
                {
                    foreach (GameObject gObject in mObjects)
                    {
                        if (gObject is PhysicsObject)
                        {
                            PhysicsObject pObject = (PhysicsObject)gObject;
                            pObject.Update(gameTime);
                            // handle collision right after you move
                            HandleCollisions(pObject);
                            if (pObject is Player)
                                foreach (Trigger trigger in mTrigger)
                                    trigger.RunTrigger(mObjects, (Player)pObject);

                            // Update zoom based on players velocity                 
                            pObject.FixForBounds((int)mCurrentLevel.Size.X, (int)mCurrentLevel.Size.Y);
                        }
                    }

                    //Check to see if we collected anything
                    if (mCollected.Count > 0)
                    {
                        //Safely remove the collected objects
                        foreach (GameObject g in mCollected)
                            mObjects.Remove(g);

                        //Then clear the list
                        mCollected.Clear();
                    }

                    // Update the camera to keep the player at the center of the screen
                    // Also only update if the velocity if greater than 0.5f in either direction
                    if (Math.Abs(mPlayer.ObjectVelocity.X) > 0.5f || Math.Abs(mPlayer.ObjectVelocity.Y) > 0.5f)
                    {
                        mCam.Position = new Vector3(mPlayer.Position.X - 275, mPlayer.Position.Y - 100, 0);
                        mCam1.Position = new Vector3(mPlayer.Position.X - 275, mPlayer.Position.Y - 100, 0);
                    }
  
                    /* Gradual Zoom Out */
                    if (gamepad.IsButtonDown(Buttons.LeftShoulder) ||
                        keyboard.IsKeyDown(Keys.OemMinus)) //&&
                    {
                        if (mCam.Zoom > 0.4f)
                            mCam.Zoom -= 0.003f;
                        mPrevZoom = mCam.Zoom;
                    }

                    /* Gradual Zoom In */
                    else if (gamepad.IsButtonDown(Buttons.RightShoulder) ||
                             keyboard.IsKeyDown(Keys.OemPlus)) //&&
                    {
                        if (mCam.Zoom < 1.0f)
                            mCam.Zoom += 0.003f;
                        mPrevZoom = mCam.Zoom;
                    }

                    /* Snap Zoom Out */
                    else if (gamepad.IsButtonDown(Buttons.Y) ||
                             keyboard.IsKeyDown(Keys.Y))
                        mCam.Zoom = 0.4f;

                    /* Snap Zoom In */
                    else if (mPrevGamepad.IsButtonDown(Buttons.Y) &&
                             gamepad.IsButtonUp(Buttons.Y) ||
                             mPrevKeyboard.IsKeyDown(Keys.Y) &&
                             keyboard.IsKeyUp(Keys.Y))
                        mCam.Zoom = mPrevZoom;

                    base.Update(gameTime);
                }
            }
            else if (mGameState == GameStates.IN_MENU)
                mMenu.Update(gameTime);
            else if (mGameState == GameStates.IN_SCORE)
                mScoring.Update(gameTime);

            if (!mPlayer.mIsAlive)
            {
                if (keyboard.IsKeyDown(Keys.A) ||
                    gamepad.IsButtonDown(Buttons.A))// resets game after game over
                {
                    mPlayer.mNumLives = 5;
                    mPlayer.mIsAlive = true;
                    mGameState = GameStates.IN_MENU;
                }

            }

            /* Set the previous states to the current states */
            mPrevGamepad = gamepad;
            mPrevKeyboard = keyboard;
        }

        /// <summary>
        /// Disables the menu from showing initially
        /// </summary>
        public void DisableMenu()
        {
            mGameState = GameStates.IN_GAME;
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            if (mGameState == GameStates.IN_GAME)
            {
                /* Cam is used to draw everything except the HUD - SEE BELOW FOR DRAWING HUD */
                mSpriteBatch.Begin(SpriteSortMode.Immediate,
                    BlendState.AlphaBlend,
                    SamplerState.LinearClamp,
                    DepthStencilState.None,
                    RasterizerState.CullCounterClockwise,
                    null,
                    mCam.get_transformation());

                mCurrentLevel.Draw(mSpriteBatch);

                foreach (GameObject gObject in mObjects)
                {
                    gObject.Draw(mSpriteBatch, gameTime);
                }

                //mBlackHole.Draw(mSpriteBatch, new Vector2(100.0f, 100.0f));

                mSpriteBatch.End();


                /* Cam 1 is for drawing the HUD - PLACE ALL YOUR HUD STUFF IN THIS SECTION */
                // Begin spritebatch with the desired camera transformations
                mSpriteBatch.Begin(SpriteSortMode.Immediate, 
                                    BlendState.AlphaBlend,
                                    SamplerState.LinearClamp,
                                    DepthStencilState.None, 
                                    RasterizerState.CullCounterClockwise, 
                                    null, 
                                    mCam1.get_transformation());

                mSpriteBatch.DrawString(kootenay, "Timer: " + (int)TIMER, new Vector2(mCam1.Position.X-275, mCam1.Position.Y-200), Color.White);

                if (mPhysicsEnvironment.GravityDirection == GravityDirections.Up)
                    mSpriteBatch.Draw(mDirections[0], new Vector2(mCam1.Position.X + 500, mCam1.Position.Y - 200), Color.White);
                else if (mPhysicsEnvironment.GravityDirection == GravityDirections.Right)
                    mSpriteBatch.Draw(mDirections[1], new Vector2(mCam1.Position.X + 500, mCam1.Position.Y - 200), Color.White);
                else if (mPhysicsEnvironment.GravityDirection == GravityDirections.Down)
                    mSpriteBatch.Draw(mDirections[2], new Vector2(mCam1.Position.X + 500, mCam1.Position.Y - 200), Color.White);
                else if (mPhysicsEnvironment.GravityDirection == GravityDirections.Left)
                    mSpriteBatch.Draw(mDirections[3], new Vector2(mCam1.Position.X + 500, mCam1.Position.Y - 200), Color.White);

                mSpriteBatch.Draw(mLives[mPlayer.mNumLives], new Vector2(mCam1.Position.X + 600, mCam1.Position.Y - 200), Color.White);

                mSpriteBatch.End();

                base.Draw(gameTime);
            }
            else if (mGameState == GameStates.IN_MENU)
                mMenu.Draw(mSpriteBatch, mGraphics);

            else if (mGameState == GameStates.IN_SCORE)
            {
                mScoring.Draw(mSpriteBatch, mGraphics);
            }
        }

        /// <summary>
        /// Respawn the player. Reset gravity direction and clear player velocity
        /// 
        /// TODO - Reset all other objects as well
        /// </summary>
        /// <param name="player">Player object</param>
        private void Respawn()
        {
            mPhysicsEnvironment.GravityDirection = GravityDirections.Down;
            foreach (GameObject gameObject in mObjects)
            {
                gameObject.Respawn();
                if (gameObject is PhysicsObject)
                {
                    ((PhysicsObject)gameObject).UpdateBoundingBoxes();
                    ((PhysicsObject)gameObject).mVelocity = Vector2.Zero;
                }
            }
        }

        /// <summary>
        /// Checks to see if given object is colliding with any other object and handles the collision
        /// </summary>
        /// <param name="physObj">object to see if anything is colliding with it</param>
        private void HandleCollisions(PhysicsObject physObj)
        {
            foreach (GameObject obj in mObjects)
            {
                if (obj is PlayerEnd && !(physObj is Player))
                    continue;

                bool collided = physObj.HandleCollisions(obj);

                if (collided && obj is PlayerEnd && physObj is Player)
                { 
                    Respawn();
                    mGameState = GameStates.IN_SCORE;
                    mScoring.Load(Content);

                    this.ResetElapsedTime();

                    GameSound.level_stageVictory.Play(VOLUME, 0.0f, 0.0f); 
                }

                //If player collided with a collectable object
                if (collided && ((physObj is Player) && obj.CollisionType == XmlKeys.COLLECTABLE || (obj is Player) && physObj.CollisionType == XmlKeys.COLLECTABLE))
                {
                    //Play sound
                    //GameSound.playerCol_hazard.Play(volume, 0.0f, 0.0f);
                    mPlayer.mScore += 100;
                    if (physObj.CollisionType == XmlKeys.COLLECTABLE) mCollected.Add(physObj);
                    else if (obj.CollisionType == XmlKeys.COLLECTABLE) mCollected.Add(obj);
                }

                else if (collided && ((physObj is Player) && obj.CollisionType == XmlKeys.HAZARDOUS || (obj is Player) && physObj.CollisionType == XmlKeys.HAZARDOUS))
                {
                    //Play sound
                    GameSound.playerCol_hazard.Play(VOLUME, 0.0f, 0.0f);
                    Respawn();
                    if (physObj is Player) physObj.Kill();
                    else ((Player)obj).Kill();
                }
            }
        }
    }
}
