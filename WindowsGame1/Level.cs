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
using GravityShift.Game_Objects.Static_Objects.Triggers;
using GravityShift.Import_Code;
using GravityShift.MISC_Code;

namespace GravityShift
{
    /// <summary>
    /// This class represents a level in the game
    /// </summary>
    class Level
    {
        /// <summary>
        /// Gets the background texture of this level
        /// </summary>
        public Texture2D Texture { get { return mTexture; } }
        private Texture2D mTexture;

        /// <summary>
        /// Gets or sets the name of this level
        /// </summary>
        public string Name { get { return mName; } set { mName = value; } }
        private string mName;

        /// <summary>
        /// Gets or sets the size of the level(in pixels)
        /// </summary>
        public Vector2 Size { get { return mSize; } set { mSize = value; } }
        private Vector2 mSize;
        
        /// <summary>
        /// Gets or sets the player starting point of this level(in pixels)
        /// </summary>
        public Vector2 StartingPoint { get { return mStartingPoint; } set { mStartingPoint = value; } }
        private Vector2 mStartingPoint;

        // Camera
        public static Camera cam;
        public static Camera cam1;

        /* Tracks the previous zoom of the camera */
        private float prev_zoom = 1.0f;

        /* Timer variable */
        private static double timer;

        private List<GameObject>[][] mCollisionMatrix;

        List<GameObject> mObjects;
        List<GameObject> mCollected;
        List<Trigger> mTrigger;

        Player player;

        PhysicsEnvironment mPhysicsEnvironment;

        IControlScheme mControls;

        /* SpriteFond */
        SpriteFont kootenay;

        #region HUD

        private Texture2D[] directions;
        private Texture2D[] lives;

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="Level"/> class.
        /// </summary>
        /// <param name="name">The name of the level</param>
        /// <param name="controls">The controls scheme</param>
        /// <param name="viewport">The viewport for the cameras</param>
        public Level(String name, IControlScheme controls, Viewport viewport)
        {
            mName = name;
            mControls = controls;

            cam = new Camera(viewport);
            cam1 = new Camera(viewport);

            mObjects = new List<GameObject>();
            mCollected = new List<GameObject>();
            mTrigger = new List<Trigger>();
            mPhysicsEnvironment = new PhysicsEnvironment();
        }

        /// <summary>
        /// Loads the level from the content manager
        /// </summary>
        /// <param name="content">Content Manager to load from</param>
        public void Load(ContentManager content, string assetName)
        {
            try
            { mTexture = content.Load<Texture2D>("Images\\" + assetName); }
            catch (Exception ex)
            { mTexture = content.Load<Texture2D>("Images\\errorBG"); }

            kootenay = content.Load<SpriteFont>("fonts/Kootenay");

            directions = new Texture2D[4];
            directions[3] = content.Load<Texture2D>("HUD/arrow_left");
            directions[2] = content.Load<Texture2D>("HUD/arrow_down");
            directions[1] = content.Load<Texture2D>("HUD/arrow_right");
            directions[0] = content.Load<Texture2D>("HUD/arrow_up");

            lives = new Texture2D[10];
            for (int i = 0; i < lives.Length; i++)
                lives[i] = content.Load<Texture2D>("HUD/NeonLifeCount" + i);
        }

        /// <summary>
        /// Loads the level from the content manager
        /// </summary>
        /// <param name="content">Content Manager to load from</param>
        public void Load(ContentManager content)
        {
            Importer importer = new Importer(content);
            importer.ImportLevel(this);

            player = new Player(content, ref mPhysicsEnvironment,
                mControls, .8f, EntityInfo.CreatePlayerInfo(GridSpace.GetGridCoord(StartingPoint)));

            mObjects.Add(player);
            mObjects.AddRange(importer.GetObjects(ref mPhysicsEnvironment));
            mObjects.Add(importer.GetPlayerEnd());
            mTrigger.AddRange(importer.GetTriggers());

            PrepareCollisionMatrix();
        }

        /// <summary>
        /// Prepares the collision matrix in the game
        /// </summary>
        private void PrepareCollisionMatrix()
        {
            Vector2 gridSize = GridSpace.GetGridCoord(Size);
            mCollisionMatrix = new List<GameObject>[(int)gridSize.Y][];
            for (int i = 0; i < gridSize.Y; i++)
            {
                mCollisionMatrix[i] = new List<GameObject>[(int)gridSize.X];
                for (int j = 0; j < gridSize.X; j++)
                    mCollisionMatrix[i][j] = new List<GameObject>();
            }

            foreach (GameObject obj in mObjects)
            {
                Vector2 gridLoc = GridSpace.GetGridCoord(obj.mPosition);
                mCollisionMatrix[(int)gridLoc.Y][(int)gridLoc.X].Add(obj);
            }
        }
        
        /// <summary>
        /// Updates the collision matrix with the new position
        /// </summary>
        /// <param name="obj">Object we want to update</param>
        /// <param name="oldPosition">Position where object was before</param>
        private void UpdateCollisionMatrix(GameObject obj, Vector2 oldPosition)
        {
            Vector2 newPosition = GridSpace.GetGridCoord(obj.mPosition);
            if (oldPosition.Equals(newPosition)) return;

            mCollisionMatrix[(int)oldPosition.Y][(int)oldPosition.X].Remove(obj);
            if(!mCollisionMatrix[(int)newPosition.Y][(int)newPosition.X].Contains(obj))
                mCollisionMatrix[(int)newPosition.Y][(int)newPosition.X].Add(obj);
        }

        /// <summary>
        /// Removes the object from the matrix(for collectables
        /// </summary>
        /// <param name="obj">Object we want to remove</param>
        private void RemoveFromMatrix(GameObject obj)
        {
            Vector2 position = GridSpace.GetGridCoord(obj.mPosition);

            mCollisionMatrix[(int)position.Y][(int)position.X].Remove(obj);
        }

        /// <summary>
        /// Updates the level's progress
        /// </summary>
        /// <param name="gameTime">The game time.</param>
        /// <param name="gameState">State of the game.</param>
        public void Update(GameTime gameTime, ref GameStates gameState)
        {
            timer += (gameTime.ElapsedGameTime.TotalSeconds);
            if ((player.mIsAlive))// only update while player is alive
            {
                foreach (GameObject gObject in mObjects)
                {
                    if (gObject is PhysicsObject)
                    {
                        PhysicsObject pObject = (PhysicsObject)gObject;

                        pObject.FixForBounds((int)Size.X, (int)Size.Y);
                        Vector2 oldPos = GridSpace.GetGridCoord(pObject.mPosition);
                        pObject.Update(gameTime);
                        // Update zoom based on players velocity                 
                        pObject.FixForBounds((int)Size.X, (int)Size.Y);
                        UpdateCollisionMatrix(pObject, oldPos);

                        // handle collision right after you move
                        HandleCollisions(pObject, ref gameState);
                        if (pObject is Player)
                            foreach (Trigger trigger in mTrigger)
                                trigger.RunTrigger(mObjects, (Player)pObject);

                    }
                }

                //Check to see if we collected anything
                if (mCollected.Count > 0)
                {
                    //Safely remove the collected objects
                    foreach (GameObject g in mCollected)
                    {
                        RemoveFromMatrix(g);
                        mObjects.Remove(g);
                    }

                    //Then clear the list
                    mCollected.Clear();
                }

                // Update the camera to keep the player at the center of the screen
                // Also only update if the velocity if greater than 0.5f in either direction
                if (Math.Abs(player.ObjectVelocity.X) > 0.5f || Math.Abs(player.ObjectVelocity.Y) > 0.5f)
                {
                    cam.Position = new Vector3(player.Position.X - 275, player.Position.Y - 100, 0);
                    cam1.Position = new Vector3(player.Position.X - 275, player.Position.Y - 100, 0);
                }

                /* Gradual Zoom Out */
                if (mControls.isLeftShoulderPressed(true)) //&&
                {
                    if (cam.Zoom > 0.4f)
                        cam.Zoom -= 0.003f;
                    prev_zoom = cam.Zoom;
                }

                /* Gradual Zoom In */
                else if (mControls.isRightShoulderPressed(true)) //&&
                {
                    if (cam.Zoom < 1.0f)
                        cam.Zoom += 0.003f;
                    prev_zoom = cam.Zoom;
                }

                /* Snap Zoom Out */
                else if (mControls.isYPressed(true))
                    cam.Zoom = 0.4f;

                /* Snap Zoom In */
                else if (cam.Zoom == .4f && !mControls.isYPressed(false))
                    cam.Zoom = prev_zoom;
            }

            if (!player.mIsAlive)
            {
                if (mControls.isAPressed(false))// resets game after game over
                {
                    player.mNumLives = 5;
                    player.mIsAlive = true;
                }
            }
        }

        /// <summary>
        /// Draws the level background on to the screen
        /// </summary>
        /// <param name="spriteBatch">Sprite batch that we use to draw textures</param>
        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            /* Cam is used to draw everything except the HUD - SEE BELOW FOR DRAWING HUD */
            spriteBatch.Begin(SpriteSortMode.Immediate,
                BlendState.AlphaBlend,
                SamplerState.LinearClamp,
                DepthStencilState.None,
                RasterizerState.CullCounterClockwise,
                null,
                cam.get_transformation());
            //spriteBatch.Begin();

            spriteBatch.Draw(mTexture, new Rectangle(0, 0, (int)mSize.X, (int)mSize.Y), Color.White);

            foreach (GameObject gObject in mObjects)
                gObject.Draw(spriteBatch, gameTime);

            spriteBatch.End();
        }

        /// <summary>
        /// Draws the hud.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch.</param>
        /// <param name="gameTime">The game time.</param>
        public void DrawHud(SpriteBatch spriteBatch, GameTime gameTime)
        {
            /* Cam 1 is for drawing the HUD - PLACE ALL YOUR HUD STUFF IN THIS SECTION */
            // Begin spritebatch with the desired camera transformations
            spriteBatch.Begin(SpriteSortMode.Immediate,
                                BlendState.AlphaBlend,
                                SamplerState.LinearClamp,
                                DepthStencilState.None,
                                RasterizerState.CullCounterClockwise,
                                null,
                                cam1.get_transformation());

            spriteBatch.DrawString(kootenay, "Timer: " + (int)timer, new Vector2(cam1.Position.X - 275, cam1.Position.Y - 200), Color.White);

            if (mPhysicsEnvironment.GravityDirection == GravityDirections.Up)
                spriteBatch.Draw(directions[0], new Vector2(cam1.Position.X + 500, cam1.Position.Y - 200), Color.White);
            else if (mPhysicsEnvironment.GravityDirection == GravityDirections.Right)
                spriteBatch.Draw(directions[1], new Vector2(cam1.Position.X + 500, cam1.Position.Y - 200), Color.White);
            else if (mPhysicsEnvironment.GravityDirection == GravityDirections.Down)
                spriteBatch.Draw(directions[2], new Vector2(cam1.Position.X + 500, cam1.Position.Y - 200), Color.White);
            else if (mPhysicsEnvironment.GravityDirection == GravityDirections.Left)
                spriteBatch.Draw(directions[3], new Vector2(cam1.Position.X + 500, cam1.Position.Y - 200), Color.White);

            spriteBatch.Draw(lives[player.mNumLives], new Vector2(cam1.Position.X + 600, cam1.Position.Y - 200), Color.White);

            spriteBatch.End();
        }

        /// <summary>
        /// Respawn the player. Reset gravity direction and clear player velocity
        /// 
        /// TODO - Reset all other objects as well
        /// </summary>
        /// <param name="player">Player object</param>
        private void Respawn()
        {
            player.Respawn();
            mPhysicsEnvironment.GravityDirection = GravityDirections.Down;
            foreach (GameObject gameObject in mObjects)
                if(gameObject != player)
                    gameObject.Respawn();
        }

        /// <summary>
        /// Checks to see if given object is colliding with any other object and handles the collision
        /// </summary>
        /// <param name="physObj">object to see if anything is colliding with it</param>
        private void HandleCollisions(PhysicsObject physObj, ref GameStates gameState)
        {
            Vector2 gridPos = GridSpace.GetGridCoord(physObj.mPosition);

            //Goes through the 9 possible positions for collision to see if this physics object is colliding with anything
            for (int i = -1; i < 2; i++)
            {
                if (gridPos.Y + i < 0 || gridPos.Y + i >= mCollisionMatrix.Length) continue;//Bounds check
                for (int j = -1; j < 2; j++)
                {
                    if (gridPos.X + j < 0 || gridPos.X + j >= mCollisionMatrix[(int)gridPos.Y + i].Length) continue;//Bounds check

                    //For each object registered at this spot, check for collisions
                    foreach (GameObject obj in mCollisionMatrix[(int)gridPos.Y+i][(int)gridPos.X+j])
                    {
                        if (obj.Equals(physObj) || obj is PlayerEnd && !(physObj is Player))
                            continue;

                        bool collided = physObj.HandleCollisions(obj);

                        //If player reaches the end, respawn him and set the timer to 0
                        if (collided && obj is PlayerEnd && physObj is Player)
                        {
                            Respawn();
                            GameSound.level_stageVictory.Play(GameSound.volume, 0.0f, 0.0f);
                            gameState = GameStates.Score;
                            timer = 0;
                        }

                        //If player collided with a collectable object
                        if (collided && ((physObj is Player) && obj.CollisionType == XmlKeys.COLLECTABLE || (obj is Player) && physObj.CollisionType == XmlKeys.COLLECTABLE))
                        {
                            player.mScore += 100;
                            if (physObj.CollisionType == XmlKeys.COLLECTABLE) mCollected.Add(physObj);
                            else if (obj.CollisionType == XmlKeys.COLLECTABLE) mCollected.Add(obj);
                        }
                        //If player hits a hazard
                        else if (collided && ((physObj is Player) && obj.CollisionType == XmlKeys.HAZARDOUS || (obj is Player) && physObj.CollisionType == XmlKeys.HAZARDOUS))
                        {
                            Respawn();
                            if (physObj is Player) physObj.Kill();
                            else ((Player)obj).Kill();
                        }
                    }
                }
            }
        }
    }
}
