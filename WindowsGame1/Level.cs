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
using GravityShift.Game_Objects.Static_Objects;

namespace GravityShift
{
    /// <summary>
    /// This class represents a level in the game
    /// </summary>
    public class Level
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
        /// Gets the filepath of the level
        /// </summary>
        public string FileName { get { return mFileName; } }
        private string mFileName;

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
        public static Camera mCam;
        public static Camera mCam1;

        /* Tracks the previous zoom of the camera */
        private float mPrevZoom = 1.0f;

        /* Timer variable */
        private static double TIMER;

        private List<GameObject>[][] mCollisionMatrix;

        List<GameObject> mObjects;
        List<GameObject> mCollected;
        List<GameObject> mRemoveCollected;
        List<Trigger> mTrigger;

        bool mDiedThisUpdate;

        Player mPlayer;

        PhysicsEnvironment mPhysicsEnvironment;

        IControlScheme mControls;

        /* SpriteFond */
        SpriteFont mKootenay;

        #region HUD

        private Texture2D[] mDirections;
        private Texture2D[] mLives;

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
            mFileName = name;
            mControls = controls;

            mCam = new Camera(viewport);
            mCam1 = new Camera(viewport);

            mObjects = new List<GameObject>();
            mCollected = new List<GameObject>();
            mRemoveCollected = new List<GameObject>();
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

            mKootenay = content.Load<SpriteFont>("fonts/Kootenay");

            mDirections = new Texture2D[4];
            mDirections[3] = content.Load<Texture2D>("HUD/arrow_left");
            mDirections[2] = content.Load<Texture2D>("HUD/arrow_down");
            mDirections[1] = content.Load<Texture2D>("HUD/arrow_right");
            mDirections[0] = content.Load<Texture2D>("HUD/arrow_up");

            mLives = new Texture2D[10];
            for (int i = 0; i < mLives.Length; i++)
                mLives[i] = content.Load<Texture2D>("HUD/NeonLifeCount" + i);
        }

        /// <summary>
        /// Loads the level from the content manager
        /// </summary>
        /// <param name="content">Content Manager to load from</param>
        public void Load(ContentManager content)
        {
            Importer importer = new Importer(content);
            importer.ImportLevel(this);

            mPlayer = new Player(content, ref mPhysicsEnvironment,
                mControls, .8f, EntityInfo.CreatePlayerInfo(GridSpace.GetGridCoord(StartingPoint)));

            mObjects.Add(mPlayer);
            mObjects.AddRange(importer.GetObjects(ref mPhysicsEnvironment));
            mObjects.Add(importer.GetPlayerEnd());

            mObjects.AddRange(importer.GetWalls(this));

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

            //Adds each object into the collision matrix. Special case is for walls, in which it must extend over for it to be continuously 
                //considered as collidable
            foreach (GameObject obj in mObjects)
            {
                Vector2 gridLoc = GridSpace.GetGridCoord(obj.mPosition);
                if (!(obj is Wall))
                    mCollisionMatrix[(int)gridLoc.Y][(int)gridLoc.X].Add(obj);
                else
                {
                    Wall wall = (Wall)obj;
                    for (int i = 0; i < wall.Walls.Count; i+=2)
                    {
                        gridLoc = GridSpace.GetGridCoord(wall.Walls[i].mPosition);
                        mCollisionMatrix[(int)gridLoc.Y][(int)gridLoc.X].Add(obj);
                    }
                }
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
            TIMER += (gameTime.ElapsedGameTime.TotalSeconds);
            if ((mPlayer.mIsAlive))// only update while player is alive
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
                if (mRemoveCollected.Count > 0)
                {
                    //Safely remove the collected objects
                    foreach (GameObject g in mRemoveCollected)
                    {
                        RemoveFromMatrix(g);
                        mObjects.Remove(g);
                    }

                    //Then clear the list
                    mRemoveCollected.Clear();
                }

                //Check to see if we died
                if (mDiedThisUpdate)
                {
                    mDiedThisUpdate = false;

                    //Add the collected objects back to the object list
                    foreach (GameObject collected in mCollected)
                        mObjects.Add(collected);

                    //Reset the collision matrix
                    PrepareCollisionMatrix();
                    
                    //Clear the collection lists
                    mCollected.Clear();
                    mRemoveCollected.Clear();
                }

                // Update the camera to keep the player at the center of the screen
                // Also only update if the velocity if greater than 0.5f in either direction
                if (Math.Abs(mPlayer.ObjectVelocity.X) > 0.5f || Math.Abs(mPlayer.ObjectVelocity.Y) > 0.5f)
                {
                    mCam.Position = new Vector3(mPlayer.Position.X - 275, mPlayer.Position.Y - 100, 0);
                    mCam1.Position = new Vector3(mPlayer.Position.X - 275, mPlayer.Position.Y - 100, 0);
                }

                /* Gradual Zoom Out */
                if (mControls.isLeftShoulderPressed(true)) //&&
                {
                    if (mCam.Zoom > 0.4f)
                        mCam.Zoom -= 0.003f;
                    mPrevZoom = mCam.Zoom;
                }

                /* Gradual Zoom In */
                else if (mControls.isRightShoulderPressed(true)) //&&
                {
                    if (mCam.Zoom < 1.0f)
                        mCam.Zoom += 0.003f;
                    mPrevZoom = mCam.Zoom;
                }

                /* Snap Zoom Out */
                else if (mControls.isYPressed(true))
                    mCam.Zoom = 0.4f;

                /* Snap Zoom In */
                else if (mCam.Zoom == .4f && !mControls.isYPressed(false))
                    mCam.Zoom = mPrevZoom;
            }

            if (!mPlayer.mIsAlive)
            {
                if (mControls.isAPressed(false))// resets game after game over
                {
                    mPlayer.mNumLives = 5;
                    mPlayer.mIsAlive = true;
                }
            }

            if (mControls.isStartPressed(false))
                gameState = GameStates.Pause;
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
                mCam.get_transformation());
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
                                mCam1.get_transformation());

            spriteBatch.DrawString(mKootenay, "Timer: " + (int)TIMER, new Vector2(mCam1.Position.X - 275, mCam1.Position.Y - 200), Color.White);

            if (mPhysicsEnvironment.GravityDirection == GravityDirections.Up)
                spriteBatch.Draw(mDirections[0], new Vector2(mCam1.Position.X + 500, mCam1.Position.Y - 200), Color.White);
            else if (mPhysicsEnvironment.GravityDirection == GravityDirections.Right)
                spriteBatch.Draw(mDirections[1], new Vector2(mCam1.Position.X + 500, mCam1.Position.Y - 200), Color.White);
            else if (mPhysicsEnvironment.GravityDirection == GravityDirections.Down)
                spriteBatch.Draw(mDirections[2], new Vector2(mCam1.Position.X + 500, mCam1.Position.Y - 200), Color.White);
            else if (mPhysicsEnvironment.GravityDirection == GravityDirections.Left)
                spriteBatch.Draw(mDirections[3], new Vector2(mCam1.Position.X + 500, mCam1.Position.Y - 200), Color.White);

            spriteBatch.Draw(mLives[mPlayer.mNumLives], new Vector2(mCam1.Position.X + 600, mCam1.Position.Y - 200), Color.White);

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

            mPlayer.Respawn();
            mPhysicsEnvironment.GravityDirection = GravityDirections.Down;

            mDiedThisUpdate = true;

            foreach (GameObject gameObject in mObjects)
                if(gameObject != mPlayer)
                    gameObject.Respawn();
        }

        /// <summary>
        /// Preps the level to reload content
        /// </summary>
        public void Reset()
        {
            mPhysicsEnvironment.GravityDirection = GravityDirections.Down;
            mObjects.Clear();
            mCollected.Clear();
            mRemoveCollected.Clear();
            mTrigger.Clear();
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
                            TIMER = 0;
                        }

                        //If player collided with a collectable object
                        if (collided && ((physObj is Player) && obj.CollisionType == XmlKeys.COLLECTABLE || (obj is Player) && physObj.CollisionType == XmlKeys.COLLECTABLE))
                        {
                            mPlayer.mScore += 100;
                            if (physObj.CollisionType == XmlKeys.COLLECTABLE)
                            {
                                mCollected.Add(physObj);
                                mRemoveCollected.Add(physObj);
                            }
                            else if (obj.CollisionType == XmlKeys.COLLECTABLE)
                            {
                                mCollected.Add(obj);
                                mRemoveCollected.Add(obj);
                            }
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
