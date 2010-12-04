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
        Menu menu;

        // Boolean to track whether we are in the game or the menu
        private static bool inGame = false;
        private static bool inMenu = true;

        // Camera
        public static Camera cam;

        // see if we are paused
        private static bool isPaused;

        // check for press of pause button
        bool wasDown=false;

        //List of objects that comform to the game physics
        List<GameObject> mObjects;
        List<GameObject> mCollected;

        List<Trigger> mTrigger;

        Player player;

        //Current level
        Level mCurrentLevel;
        PhysicsEnvironment mPhysicsEnvironment;

        //Font for this game
        SpriteFont mDefaultFont;

        public string LevelLocation { get { return mLevelLocation; } set { mLevelLocation = "..\\..\\..\\Content\\Levels\\" + value; } }
        
        private string mLevelLocation = "..\\..\\..\\Content\\Levels\\DefaultLevel.xml";

        public GravityShiftMain()
        {
            mGraphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            cam = new Camera(GraphicsDevice.Viewport);

            mGraphics.PreferredBackBufferWidth = mGraphics.GraphicsDevice.DisplayMode.Width;
            mGraphics.PreferredBackBufferHeight = mGraphics.GraphicsDevice.DisplayMode.Height;
            //mGraphics.ToggleFullScreen();// REMEMBER TO RESET AFTER DEBUGGING!!!!!!!!!
            mGraphics.ApplyChanges();

            mObjects = new List<GameObject>();
            mCollected = new List<GameObject>();
            mTrigger = new List<Trigger>();
            mPhysicsEnvironment = new PhysicsEnvironment();

            menu = new Menu();

            isPaused = false;

            base.Initialize();
        }

        /* Getter/Setter to change the inMenu variables */
        public static bool InMenu
        {
            get { return inMenu; }
            set { inMenu = value; }
        }

        /* Getter/Setter to change the inGame variables */
        public static bool InGame
        {
            get { return inGame; }
            set { inGame = value; }
        } 

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            menu.Load(Content);
            GameSound.Load(Content);
            // Create a new SpriteBatch, which can be used to draw textures.
            mSpriteBatch = new SpriteBatch(GraphicsDevice);

            //Should eventually not require 
            Importer importer = new Importer(Content);
            mCurrentLevel = importer.ImportLevel(LevelLocation);

            mDefaultFont = Content.Load<SpriteFont>("fonts/Kootenay");

            player = new Player(Content, ref mPhysicsEnvironment, 
                new KeyboardControl(), .8f, EntityInfo.CreatePlayerInfo(GridSpace.GetGridCoord(mCurrentLevel.StartingPoint)));

            mObjects.Add(player);

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
            KeyboardState keyboard = Keyboard.GetState();
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed
                || keyboard.IsKeyDown(Keys.Escape))
                this.Exit();
            if (GamePad.GetState(PlayerIndex.One).Buttons.Start == ButtonState.Pressed || keyboard.IsKeyDown(Keys.Space))
            {
                if (!wasDown)
                {
                    isPaused = !isPaused;// toggle pause
                }
            }

            wasDown =(GamePad.GetState(PlayerIndex.One).Buttons.Start == ButtonState.Pressed || keyboard.IsKeyDown(Keys.Space));

            if (inGame)
            {
                Random rand = new Random();
                PhysicsEnvironment environment = new PhysicsEnvironment();
                environment.TerminalSpeed = 20;
                environment.GravityMagnitude = 1;
                if ((player.mIsAlive)&&!isPaused)// only update while player is alive
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
                                foreach(Trigger trigger in mTrigger) 
                                    trigger.RunTrigger(mObjects,(Player)pObject);                 

                            // Update the camera to keep the player at the center of the screen
                            // Also only update if the velocity if greater than 0.5f in either direction
                            if (Math.Abs(player.ObjectVelocity.X) > 0.5f || Math.Abs(player.ObjectVelocity.Y) > 0.5f)
                                cam.Postion = new Vector3(player.Position.X - 275, player.Position.Y - 100, 0);

                            // Update zoom based on players velocity
                            if (Math.Abs(player.ObjectVelocity.X) > 8 || Math.Abs(player.ObjectVelocity.Y) > 8)
                            {
                            //    if (cam.Zoom > 0.5f)
                            //        cam.Zoom -= 0.00005f;
                            }
                            if (Math.Abs(player.ObjectVelocity.X) > 15 || Math.Abs(player.ObjectVelocity.Y) > 15)
                            {
                                if (cam.Zoom > 0.25f)
                                    cam.Zoom -= 0.000015f;
                            }
                            else if (cam.Zoom < 1.0f)
                                cam.Zoom += 0.00025f;
                            
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

                    base.Update(gameTime);
                }
            }
            else if (inMenu)
                menu.Update(gameTime);

            if (!player.mIsAlive)
            {
                if (keyboard.IsKeyDown(Keys.A))// resets game after game over
                {// TODO: add gamepad functionality
                    player.mNumLives = 5;
                    player.mIsAlive = true;
                    inGame = false;
                    inMenu = true;
                }

            }
        }

        /// <summary>
        /// Disables the menu from showing initially
        /// </summary>
        public void DisableMenu()
        {
            inGame = true;
            inMenu = false;
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            if (inGame)
            {
                // Begin spritebatch with the desired camera transformations
                mSpriteBatch.Begin(SpriteSortMode.Immediate, 
                                    BlendState.AlphaBlend,
                                    SamplerState.LinearClamp,
                                    DepthStencilState.None, 
                                    RasterizerState.CullCounterClockwise, 
                                    null, 
                                    cam.get_transformation());
                    
                mCurrentLevel.Draw(mSpriteBatch);

                foreach (GameObject gObject in mObjects)
                {
                    gObject.Draw(mSpriteBatch, gameTime);
                }

                mSpriteBatch.End();

                base.Draw(gameTime);
            }
            else if (inMenu)
                menu.Draw(mSpriteBatch, mGraphics);
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
                gameObject.Respawn();
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
                    inGame = false; 
                    inMenu = true; 
                    GameSound.level_stageVictory.Play(); 
                }

                //If player collided with a collectable object
                if (collided && ((physObj is Player) && obj.CollisionType == XmlKeys.COLLECTABLE || (obj is Player) && physObj.CollisionType == XmlKeys.COLLECTABLE))
                {
                    //Play sound
                    //GameSound.playerCol_hazard.Play();
                    player.mScore += 100;
                    if (physObj.CollisionType == XmlKeys.COLLECTABLE) mCollected.Add(physObj);
                    else if (obj.CollisionType == XmlKeys.COLLECTABLE) mCollected.Add(obj);
                }

                else if (collided && ((physObj is Player) && obj.CollisionType == XmlKeys.HAZARDOUS || (obj is Player) && physObj.CollisionType == XmlKeys.HAZARDOUS))
                {
                    //Play sound
                    GameSound.playerCol_hazard.Play();
                    Respawn();
                    if (physObj is Player) physObj.Kill();
                    else ((Player)obj).Kill();
                }
            }
        }
    }
}
