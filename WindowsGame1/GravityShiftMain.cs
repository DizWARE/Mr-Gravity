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
        private static bool inGame;
        private static bool inMenu;

        //List of objects that comform to the game physics
        List<GameObject> mObjects;

        Player player;

        //Current level
        Level mCurrentLevel;
        PhysicsEnvironment mPhysicsEnvironment;

        //Font for this game
        SpriteFont mDefaultFont;

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
            mGraphics.PreferredBackBufferWidth = mGraphics.GraphicsDevice.DisplayMode.Width;
            mGraphics.PreferredBackBufferHeight = mGraphics.GraphicsDevice.DisplayMode.Height;
            //mGraphics.ToggleFullScreen();// REMEMBER TO RESET AFTER DEBUGGING!!!!!!!!!
            mGraphics.ApplyChanges();

            mObjects = new List<GameObject>();
            mCurrentLevel = new Level();
            mPhysicsEnvironment = new PhysicsEnvironment();

            menu = new Menu();
            inMenu = true;
            inGame = false;

            base.Initialize();
        }

        public static bool InMenu
        {
            get { return inMenu; }
            set { inMenu = value; }
        }

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
            // Create a new SpriteBatch, which can be used to draw textures.
            mSpriteBatch = new SpriteBatch(GraphicsDevice);
            mCurrentLevel.Load(Content,1);

            mDefaultFont = Content.Load<SpriteFont>("fonts/Kootenay");

            player = new Player(Content, "Player",
                new Vector2(1, 1), new Vector2(300,100), 
                ref mPhysicsEnvironment, new KeyboardControl(),.8f,false);
            mObjects.Add(player);

            //MovingTile movTile = new MovingTile(Content, "Tile", Vector2.One, new Vector2(100, 100), ref mPhysicsEnvironment, .8f,true);
            //mObjects.Add(movTile);

            Tile tile1 = new Tile(Content, "Tile", Vector2.One, new Vector2(300, 300), .8f, true);
            mObjects.Add(tile1);
            // create floor
            for (int i = 0; i < 20; i++)
            {
                Tile tile = new Tile(Content, "Tile", Vector2.One, new Vector2(i*64, 704), .8f,true);
                mObjects.Add(tile);
            }
            // create left wall
            for (int i = 0; i < 11; i++)
            {
                Tile tile = new Tile(Content, "Tile", Vector2.One, new Vector2(0, i*64), .8f,true);
                mObjects.Add(tile);
            }
            // create right wall
            for (int i = 0; i < 10; i++)
            {
                Tile tile = new Tile(Content, "Tile", Vector2.One, new Vector2(1216,64+ i * 64), .8f,true);
                mObjects.Add(tile);
            }
            // create top
            for (int i = 0; i < 19; i++)
            {
                Tile tile = new Tile(Content, "Tile", Vector2.One, new Vector2(64+ i * 64, 0), .8f,true);
                mObjects.Add(tile);
            }
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

            if (inGame)
            {
                Random rand = new Random();
                PhysicsEnvironment environment = new PhysicsEnvironment();
                environment.TerminalSpeed = 20;
                environment.GravityMagnitude = 1;
                if (keyboard.IsKeyDown(Keys.N))
                {
                    if (rand.Next() % 3 == 0)
                        environment = mPhysicsEnvironment;
                    mObjects.Add(new GenericObject(Content, "Player",
                            new Vector2(1, 1), new Vector2(rand.Next(), rand.Next()), ref environment, .8f, false));
                }

                foreach (GameObject gObject in mObjects)
                {
                    if (gObject is PhysicsObject)
                    {
                        PhysicsObject pObject = (PhysicsObject)gObject;
                        pObject.Update(gameTime);
                        // handle collision right after you move
                        HandleCollisions(pObject);
                        if (pObject is Player) ChangeValues((Player)pObject, keyboard);
                        pObject.FixForBounds(mGraphics.PreferredBackBufferWidth, mGraphics.PreferredBackBufferHeight);
                    }
                }
                base.Update(gameTime);
            }
            else if (inMenu)
                menu.Update(gameTime);
        }
        /// <summary>
        /// TODO - REMOVE WHEN NO LONGER A DEMONSTRACTION (Demonstraction? HAHA!)
        /// </summary>
        /// <param name="player"></param>
        /// <param name="keyboardState"></param>
        private void ChangeValues(Player player, KeyboardState keyboardState)
        {
            if (keyboardState.IsKeyDown(Keys.G)) mPhysicsEnvironment.GravityMagnitude += 1;
            if (keyboardState.IsKeyDown(Keys.H)) mPhysicsEnvironment.GravityMagnitude -= 1;
            if (keyboardState.IsKeyDown(Keys.T)) mPhysicsEnvironment.TerminalSpeed += 1;
            if (keyboardState.IsKeyDown(Keys.Y)) mPhysicsEnvironment.TerminalSpeed -= 1;
            if (keyboardState.IsKeyDown(Keys.W)) mPhysicsEnvironment.ErosionFactor += .01f;
            if (keyboardState.IsKeyDown(Keys.E)) mPhysicsEnvironment.ErosionFactor -= .01f;
            if (keyboardState.IsKeyDown(Keys.U)) 
                mPhysicsEnvironment.IncrementDirectionalMagnifier(GravityDirections.Up);
            if (keyboardState.IsKeyDown(Keys.I))
                mPhysicsEnvironment.DecrementDirectionalMagnifier(GravityDirections.Up);
            if (keyboardState.IsKeyDown(Keys.K)) 
                mPhysicsEnvironment.DecrementDirectionalMagnifier(GravityDirections.Left);
            if (keyboardState.IsKeyDown(Keys.L)) 
                mPhysicsEnvironment.IncrementDirectionalMagnifier(GravityDirections.Left);
            if (keyboardState.IsKeyDown(Keys.O)) 
                mPhysicsEnvironment.IncrementDirectionalMagnifier(GravityDirections.Right);
            if (keyboardState.IsKeyDown(Keys.P)) 
                mPhysicsEnvironment.DecrementDirectionalMagnifier(GravityDirections.Right);
            if (keyboardState.IsKeyDown(Keys.D))
                mPhysicsEnvironment.IncrementDirectionalMagnifier(GravityDirections.Down);
            if (keyboardState.IsKeyDown(Keys.F))
                mPhysicsEnvironment.DecrementDirectionalMagnifier(GravityDirections.Down);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            if (inGame)
            {
                mSpriteBatch.Begin();

                //mCurrentLevel.Draw(mSpriteBatch);
                foreach (GameObject gObject in mObjects)
                {
                    gObject.Draw(mSpriteBatch, gameTime);
                }

                DrawHUD(gameTime);

                mSpriteBatch.End();

                base.Draw(gameTime);
            }
            else if (inMenu)
                menu.Draw(mSpriteBatch, mGraphics);
        }

        /// <summary>
        /// TODO - ADD REAL HUD CODE
        /// </summary>
        /// <param name="gameTime"></param>
        private void DrawHUD(GameTime gameTime)
        {
            Rectangle titleSafeArea = GraphicsDevice.Viewport.TitleSafeArea;

            Player player = null;
            foreach (GameObject obj in mObjects)
            {
                if (obj is Player)
                {
                    player = (Player)obj;
                }
            }
            
            Vector2 location = new Vector2(titleSafeArea.X+10,titleSafeArea.Y+5);            

            mSpriteBatch.DrawString(mDefaultFont,"Gravity Force(G/H): " + mPhysicsEnvironment.GravityMagnitude, location,Color.Black);
            location = Vector2.Add(location, new Vector2(0, 20));
            mSpriteBatch.DrawString(mDefaultFont, "Terminal Speed(T/Y): " + mPhysicsEnvironment.TerminalSpeed, location, Color.Black);
            location = Vector2.Add(location, new Vector2(0, 20));
            mSpriteBatch.DrawString(mDefaultFont, "Erosion Factor(W/E): " + mPhysicsEnvironment.ErosionFactor, location, Color.Black);

            location = new Vector2(titleSafeArea.Right - 490, titleSafeArea.Y+5);
            mSpriteBatch.DrawString(mDefaultFont, "Up Force Magnifier(U/I): " + 
                mPhysicsEnvironment.GetGravityMagnifier(GravityDirections.Up), location, Color.Black);
            location = Vector2.Add(location, new Vector2(-250, 20));
            mSpriteBatch.DrawString(mDefaultFont, "Left Force Magnifier(K/L): " + 
                mPhysicsEnvironment.GetGravityMagnifier(GravityDirections.Left), location, Color.Black);
            location = Vector2.Add(location, new Vector2(+450, 0));
            mSpriteBatch.DrawString(mDefaultFont, "Right Force Magnifier(O/P): " + 
                mPhysicsEnvironment.GetGravityMagnifier(GravityDirections.Right), location, Color.Black);
            location = Vector2.Add(location, new Vector2(-200, 20));
            mSpriteBatch.DrawString(mDefaultFont, "Down Force Magnifier(D/F): " + 
                mPhysicsEnvironment.GetGravityMagnifier(GravityDirections.Down), location, Color.Black);

            location = new Vector2(titleSafeArea.X + 10, titleSafeArea.Height - 70); 
            mSpriteBatch.DrawString(mDefaultFont, "Velocity X : " + (int)player.ObjectVelocity.X + 
                " Velocity Y: " + (int)player.ObjectVelocity.Y, location, Color.Black);
            location = Vector2.Add(location, new Vector2(0, 20));
            mSpriteBatch.DrawString(mDefaultFont, "Total Force X: " + player.TotalForce.X + 
                " Total Force Y: " + player.TotalForce.Y, location, Color.Black);
            location = Vector2.Add(location, new Vector2(0, 20));
            mSpriteBatch.DrawString(mDefaultFont, "Direction of Gravity: " + mPhysicsEnvironment.GravityDirection.ToString(), location, Color.Black);
        }
        /// <summary>
        /// Checks to see if given object is colliding with any other object and handles the collision
        /// </summary>
        /// <param name="physObj">object to see if anything is colliding with it</param>
        private void HandleCollisions(PhysicsObject physObj)
        {
            if (physObj.mIsSquare)
            {
                // handle collisions for each physics object
                foreach (GameObject obj in mObjects)
                {
                    if (obj.mIsSquare)// square/square collision
                    {
                        physObj.HandleCollideBoxAndBox(obj);
                    }
                    else//square/ circle collision
                    {
                        if (obj is PhysicsObject)
                        {
                            PhysicsObject physTemp = (PhysicsObject)obj;
                            physTemp.HandleCollideCircleAndBox(physObj);
                        }
                    }
                }
            }
            else// circle
            {
                // handle collisions for each physics object
                foreach (GameObject obj in mObjects)
                {
                    if (obj.mIsSquare)// circle/square collision
                    {
                        physObj.HandleCollideCircleAndBox(obj);
                    }
                    else// circle/circle collision
                    {
                        physObj.HandleCollideCircleAndCircle(obj);
                    }
                }

            }
        }
    }
}
