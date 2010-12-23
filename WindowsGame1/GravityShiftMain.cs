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

        //Instance of the scoring class
        Scoring mScoring;
		
        private GameStates mCurrentState = GameStates.Main_Menu;

        private IControlScheme mControls;

        //Current level
        Level mCurrentLevel;

        //Fonts for this game
        SpriteFont mDefaultFont;
        SpriteFont kootenay;

        //TO BE CHANGED
        public string LevelLocation { get { return mLevelLocation; } set { mLevelLocation = "..\\..\\..\\Content\\Levels\\" + value; } }        
        private string mLevelLocation = "..\\..\\..\\Content\\Levels\\DefaultLevel.xml";

        public GravityShiftMain()
        {
            mGraphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            if (GamePad.GetState(PlayerIndex.One).IsConnected || GamePad.GetState(PlayerIndex.Two).IsConnected ||
                GamePad.GetState(PlayerIndex.Three).IsConnected || GamePad.GetState(PlayerIndex.Four).IsConnected)
                mControls = new ControllerControl();
            else
                mControls = new KeyboardControl();
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it c5an query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            

            mGraphics.PreferredBackBufferWidth = mGraphics.GraphicsDevice.DisplayMode.Width;
            mGraphics.PreferredBackBufferHeight = mGraphics.GraphicsDevice.DisplayMode.Height;
            //mGraphics.ToggleFullScreen();// REMEMBER TO RESET AFTER DEBUGGING!!!!!!!!!
            mGraphics.ApplyChanges();

            mMenu = new Menu(mControls);
            mScoring = new Scoring(mControls);

            mSpriteBatch = new SpriteBatch(mGraphics.GraphicsDevice);
            base.Initialize();
		}

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            mMenu.Load(Content);
            mScoring.Load(Content);
            GameSound.Load(Content);
            mCurrentLevel = new Level(mLevelLocation, mControls, GraphicsDevice.Viewport);
            mCurrentLevel.Load(Content);

            // Create a new SpriteBatch, which can be used to draw textures.
            mSpriteBatch = new SpriteBatch(GraphicsDevice);

            mDefaultFont = Content.Load<SpriteFont>("fonts/Kootenay");
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
            // Allows the game to exit
            if (mControls.isBackPressed(false))
                this.Exit();

            if (mCurrentState == GameStates.In_Game)
                mCurrentLevel.Update(gameTime, ref mCurrentState);
            else if (mCurrentState == GameStates.Main_Menu)
                mMenu.Update(gameTime, ref mCurrentState);
            else if (mCurrentState == GameStates.Score)
                mScoring.Update(gameTime, ref mCurrentState);
            else if (mCurrentState == GameStates.Level_Selection) ; //TODO
            else if (mCurrentState == GameStates.Pause) ;
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
                mCurrentLevel.Draw(mSpriteBatch, gameTime);
                mCurrentLevel.DrawHud(mSpriteBatch, gameTime);                
            }
            else if (mCurrentState == GameStates.Main_Menu)
                mMenu.Draw(mSpriteBatch, mGraphics);

            else if (mCurrentState == GameStates.Score)
                mScoring.Draw(mSpriteBatch, mGraphics);
            else if (mCurrentState == GameStates.Level_Selection) ; //TODO
            else if (mCurrentState == GameStates.Pause) ;
            base.Draw(gameTime);
        }
    }
}
