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
            mGraphics.ToggleFullScreen();
            mGraphics.ApplyChanges();

            mObjects = new List<GameObject>();
            mCurrentLevel = new Level();
            mPhysicsEnvironment = new PhysicsEnvironment();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            mSpriteBatch = new SpriteBatch(GraphicsDevice);
            mCurrentLevel.Load(Content,1);

            mDefaultFont = Content.Load<SpriteFont>("defaultFont");

            player = new Player(Content, "Player",
                new Vector2(1, 1), mCurrentLevel.GetStartingPoint(), ref mPhysicsEnvironment, new ControllerControl(PlayerIndex.One));
            mObjects.Add(player);

            Tile tile = new Tile(Content,"Tile",Vector2.One,new Vector2(200,200));
            mObjects.Add(tile);

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

            Random rand = new Random();
            PhysicsEnvironment environment = new PhysicsEnvironment();
            environment.TerminalSpeed = rand.Next(100)+1;
            environment.GravityMagnitude = rand.Next(3)+1;
            if (keyboard.IsKeyDown(Keys.N))
            {
                if (rand.Next() % 3 == 0)
                    environment = mPhysicsEnvironment;
                mObjects.Add(new GenericObject(Content, "Player",
                        new Vector2(1, 1), new Vector2(rand.Next(), rand.Next()), ref environment));
            }
            // handle collisions
            foreach (GameObject gObject in mObjects)
            {
                if (gObject is PhysicsObject)
                {
                    PhysicsObject physObj = (PhysicsObject)gObject;
                    foreach (GameObject obj in mObjects)
                    {
                        if (!obj.Equals(gObject))
                        {
                            if (physObj.IsColliding(obj))
                            {
                                physObj.HandleCollideBox(obj);
                            }
                        }
                    }
                }
            }
            foreach (GameObject gObject in mObjects)
            {
                if (gObject is PhysicsObject)
                {
                    PhysicsObject pObject = (PhysicsObject)gObject;
                    pObject.Update(gameTime);
                    if (pObject is Player) ChangeValues((Player)pObject, keyboard);
                    pObject.FixForBounds(mGraphics.PreferredBackBufferWidth, mGraphics.PreferredBackBufferHeight);
                }
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// TODO - REMOVE WHEN NO LONGER A DEMONSTRACTION
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
    }
}
