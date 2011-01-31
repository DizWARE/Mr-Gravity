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
        /// Gets or sets the name of this level
        /// </summary>
        public string Name 
        { 
            get { return mName; } 
            set { mName = value; }
        }
        private string mName;

        /// <summary>
        /// Gets or sets the filepath of this level
        /// </summary>
        public string Filepath 
        { 
            get { return mFilepath; } 
            set{ 
                mFilepath = value;
                int start = mFilepath.LastIndexOf('\\');
                int end = mFilepath.LastIndexOf('.');
                mName = mFilepath.Substring(start+1, end - start - 1);
            } }
        private string mFilepath;

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

        //Enumerator for different states of death (playing game, in need of respawn, or panning back to start point)
        private enum DeathStates
        {
            Playing,
            Respawning,
            Panning
        }

        private DeathStates mDeathState = DeathStates.Playing;

        private Vector3 mDeathPanLength;
        private float mDeathPanUpdates;
        private static float SCALING_FACTOR = 50;

        // Camera
        public static Camera mCam;
        public static Camera mCam1;

        /* Tracks the previous zoom of the camera */
        private float mPrevZoom = 1.0f;

        /* Timer variable */
        public static double TIMER;

        private List<GameObject>[][] mCollisionMatrix;

        List<GameObject> mObjects;
        List<GameObject> mCollected;
        List<GameObject> mRemoveCollected;
        List<Trigger> mTrigger;
		
        List<EntityInfo> mRails;
        Texture2D mRailLeft;
        Texture2D mRailRight;
        Texture2D mRailHor;
        Texture2D mRailTop;
        Texture2D mRailBottom;
        Texture2D mRailVert;

        Player mPlayer;

        PhysicsEnvironment mPhysicsEnvironment;

        IControlScheme mControls;

        bool mHasRespawned = true;

        /* SpriteFont */
        SpriteFont mKootenay;
        SpriteFont mQuartz;

        // Particle Engine
        ParticleEngine particleEngine;

        #region HUD

        private Texture2D mHUDTrans;
//        private Texture2D[] mDirections;
        private Texture2D[] mLives;
        public static int mNumCollected;
        public static int mNumCollectable;

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="Level"/> class.
        /// </summary>
        /// <param name="name">The name of the level</param>
        /// <param name="controls">The controls scheme</param>
        /// <param name="viewport">The viewport for the cameras</param>
        public Level(String filepath, IControlScheme controls, Viewport viewport)
        {
            Filepath = filepath;
            mControls = controls;

            mCam = new Camera(viewport);
            mCam1 = new Camera(viewport);

            mRails = new List<EntityInfo>();

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
            mKootenay = content.Load<SpriteFont>("fonts/Kootenay");
            mQuartz = content.Load<SpriteFont>("fonts/QuartzLarge");

//            mDirections = new Texture2D[4];
//            mDirections[3] = content.Load<Texture2D>("HUD/arrow_left");
//            mDirections[2] = content.Load<Texture2D>("HUD/arrow_down");
//            mDirections[1] = content.Load<Texture2D>("HUD/arrow_right");
//            mDirections[0] = content.Load<Texture2D>("HUD/arrow_up");

            mRailLeft = content.Load<Texture2D>("Images/NonHazards/Rails/RailLeft");
            mRailHor = content.Load<Texture2D>("Images/NonHazards/Rails/RailHorizontal");
            mRailRight = content.Load<Texture2D>("Images/NonHazards/Rails/RailRight");
            mRailTop = content.Load<Texture2D>("Images/NonHazards/Rails/RailTop");
            mRailBottom = content.Load<Texture2D>("Images/NonHazards/Rails/RailBottom");
            mRailVert = content.Load<Texture2D>("Images/NonHazards/Rails/RailVertical");

            mLives = new Texture2D[10];
            for (int i = 0; i < mLives.Length; i++)
                mLives[i] = content.Load<Texture2D>("Images/HUD/NeonLifeCount" + i);

            mHUDTrans = content.Load<Texture2D>("Images/HUD/HUDTrans");

            mNumCollected = 0;
            mNumCollectable = 0;

            // Particle Engine
            List<Texture2D> textures = new List<Texture2D>();
            textures.Add(content.Load<Texture2D>("Images/Particles/diamond"));
            textures.Add(content.Load<Texture2D>("Images/Particles/star"));

            Random random = new Random();
            particleEngine = new ParticleEngine(textures, new Vector2(400, 240), random.Next(6));
        }

        /// <summary>
        /// Loads the level from the content manager
        /// </summary>
        /// <param name="content">Content Manager to load from</param>
        public void Load(ContentManager content)
        {
            mObjects.Clear();

            Importer importer = new Importer(content);
            importer.ImportLevel(this);

            mPlayer = new Player(content, ref mPhysicsEnvironment,
                mControls, .8f, EntityInfo.CreatePlayerInfo(GridSpace.GetGridCoord(StartingPoint)));

            mObjects.Add(mPlayer);
            mObjects.AddRange(importer.GetObjects(ref mPhysicsEnvironment));
            mObjects.Add(importer.GetPlayerEnd());

            mObjects.AddRange(importer.GetWalls(this).Cast<GameObject>());

            mRails = importer.GetRails();
            
            mTrigger.AddRange(importer.GetTriggers());

            PrepareCollisionMatrix();

            foreach (GameObject gObject in mObjects)
            {
                if (gObject.CollisionType == XmlKeys.COLLECTABLE)
                    mNumCollectable++;
            }
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
            if (mPlayer.mIsAlive)// only update while player is alive
            {

                if (mDeathState == DeathStates.Playing)
                {
                    TIMER += (gameTime.ElapsedGameTime.TotalSeconds);

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
                        if (!mHasRespawned) break;
                    }

                    //Check to see if we collected anything
                    if (mRemoveCollected.Count > 0)
                    {
                        mNumCollected = mNumCollectable - (mNumCollectable - mCollected.Count());

                        //Safely remove the collected objects
                        foreach (GameObject g in mRemoveCollected)
                        {
                            RemoveFromMatrix(g);
                            mObjects.Remove(g);
                        }

                        //Then clear the list
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

                    //Pause
                    if (mControls.isStartPressed(false))
                        gameState = GameStates.Pause;
                }

                else if (mDeathState == DeathStates.Respawning)
                {
                    mDeathState = DeathStates.Panning;
                    Respawn();
                }

                else//Pan back to player after death
                {
                    mCam.Position += mDeathPanLength;
                    mCam1.Position += mDeathPanLength;
                    mDeathPanUpdates++;

                    if (mDeathPanUpdates == SCALING_FACTOR)
                    {
                        mDeathState = DeathStates.Playing;
                    }
                }
            }

            if (!mPlayer.mIsAlive)
            {
                mPlayer.StopRumble();
                if (mControls.isAPressed(false))// resets game after game over
                {
                    mPlayer.mNumLives = 5;
                    mPlayer.mIsAlive = true;
                    TIMER = 0;


                    //Add the collected objects back to the object list
                    foreach (GameObject collected in mCollected)
                        mObjects.Add(collected);

                    //Reset the collision matrix
                    PrepareCollisionMatrix();

                    //Clear the collection lists
                    mCollected.Clear();
                    mRemoveCollected.Clear();
                }
            }

            // Please don't touch, let me know and I will change the engine, Thanks! -Jeremy
            //#region ParticleEngine

            //// Update particles. Emission based on velocity (fewer particles if smaller velocity)
            //double velocityVector = Math.Sqrt(Math.Pow(mPlayer.mVelocity.X, 2) + Math.Pow(mPlayer.mVelocity.Y, 2));
            //particleEngine.Update(Convert.ToInt32(velocityVector / 2) * 2);

            //// Change origin of emitter.
            //double displacement;
            //int multiplier = 2;
            //float x;
            //float y;

            ////Calculate the x-origin offset
            //displacement = multiplier * mPlayer.mVelocity.X;

            //if (displacement < -28)
            //    displacement = -28;
            //else if (displacement > 28)
            //    displacement = 28;

            //x = (mPlayer.mPosition.X + 32) - (float)displacement;

            ////Calculate the y-orgin offset
            //displacement = multiplier * mPlayer.mVelocity.Y;

            //if (displacement < -28)
            //    displacement = -28;
            //else if (displacement > 28)
            //    displacement = 28;

            //y = (mPlayer.mPosition.Y + 32) - (float)displacement;

            //particleEngine.EmitterLocation = new Vector2(x, y);

            //#endregion
            particleEngine.Update(0);
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
            foreach (Trigger trigger in mTrigger)
                trigger.Draw(spriteBatch, gameTime);

            // Loops through all rail objects and draws the appropriate rail image.
            #region DrawRails
            foreach (EntityInfo rail in mRails)
            {
                Vector2 position = new Vector2(rail.mLocation.X * 64, rail.mLocation.Y * 64);
                int length = Convert.ToInt32(rail.mProperties["Length"]);
                string type = rail.mProperties["Rail"];
                int width = mRailTop.Width;
                int height = mRailTop.Height;

                for (int i = 0; i <= length; i++)
                {
                    if (type == "X")
                    {
                        if (i == 0)
                            spriteBatch.Draw(mRailLeft, new Rectangle(Convert.ToInt32(position.X) + (i*64), Convert.ToInt32(position.Y), width, height), Color.White);
                        else if (i == length)
                            spriteBatch.Draw(mRailRight, new Rectangle(Convert.ToInt32(position.X) + (i*64), Convert.ToInt32(position.Y), width, height), Color.White);
                        else
                            spriteBatch.Draw(mRailHor, new Rectangle(Convert.ToInt32(position.X) + (i*64), Convert.ToInt32(position.Y), width, height), Color.White);
                    }
                    else
                    {
                        if (i == 0)
                            spriteBatch.Draw(mRailTop, new Rectangle(Convert.ToInt32(position.X), Convert.ToInt32(position.Y) + (i*64), width, height), Color.White);
                        else if (i == length)
                            spriteBatch.Draw(mRailBottom, new Rectangle(Convert.ToInt32(position.X), Convert.ToInt32(position.Y) + (i * 64), width, height), Color.White);
                        else
                            spriteBatch.Draw(mRailVert, new Rectangle(Convert.ToInt32(position.X), Convert.ToInt32(position.Y) + (i * 64), width, height), Color.White); ;
                    }
                }
            }
            #endregion

            if(mDeathState == DeathStates.Playing) 
                particleEngine.Draw(spriteBatch);
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

            // Draw the black background behind HUD
            spriteBatch.Draw(mHUDTrans, new Vector2(mCam1.Position.X - 300, mCam1.Position.Y - 500), null, Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);

            if (mPlayer.mIsAlive)
            {
                spriteBatch.DrawString(mQuartz, "Timer: " + (int)TIMER, new Vector2(mCam1.Position.X - 275, mCam1.Position.Y - 300), Color.DarkTurquoise);
                spriteBatch.DrawString(mQuartz, "Collected: " + mNumCollected, new Vector2(mCam1.Position.X, mCam1.Position.Y - 300), Color.DarkTurquoise);
            }

            spriteBatch.Draw(mLives[mPlayer.mNumLives], new Vector2(mCam1.Position.X + 600, mCam1.Position.Y - 300), Color.White);

            if (!mPlayer.mIsAlive)
                spriteBatch.DrawString(mQuartz, "Out of Lives       Press A to Restart", new Vector2(mCam1.Position.X - 275, mCam1.Position.Y - 300), Color.DarkTurquoise);

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

            mPlayer.StopRumble();
            mHasRespawned = true;

            mPhysicsEnvironment.GravityDirection = GravityDirections.Down;

            //Only play respawn noise when player is still alive
            if (mPlayer.mNumLives > 0)
                GameSound.playerSound_respawn.Play(GameSound.volume * 0.8f, 0.0f, 0.0f);

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
            TIMER = 0;
        }

        /// <summary>
        /// Checks to see if given object is colliding with any other object and handles the collision
        /// </summary>
        /// <param name="physObj">object to see if anything is colliding with it</param>
        private void HandleCollisions(PhysicsObject physObj, ref GameStates gameState)
        {
            // keep track of all object colliding with physObj
            List<GameObject> collidingList = new List<GameObject>();

            Vector2 gridPos = GridSpace.GetGridCoord(physObj.mPosition);

            //Goes through the 9 possible positions for collision to see if this physics object is colliding with anything
            for (int i = -1; i < 2; i++)
            {
                if (gridPos.Y + i < 0 || gridPos.Y + i >= mCollisionMatrix.Length) continue;//Bounds check
                for (int j = -1; j < 2; j++)
                {
                    if (gridPos.X + j < 0 || gridPos.X + j >= mCollisionMatrix[(int)gridPos.Y + i].Length) continue;//Bounds check

                    
                    foreach (GameObject obj in mCollisionMatrix[(int)gridPos.Y+i][(int)gridPos.X+j])
                    {
                        bool collided = false;

                        if (!physObj.IsSquare && obj.IsSquare) // phys obj is circle
                        {
                            collided = physObj.IsCollidingBoxAndBox(obj);
                        }
                        else if (physObj.IsSquare && !obj.IsSquare) //obj is circle 
                        {
                            collided = physObj.IsCollidingBoxAndBox(obj);
                        }
                        else // both circles
                        {
                            collided = physObj.IsCollidingCircleandCircle(obj);
                        }

                        if (obj.Equals(physObj) || obj is PlayerEnd && !(physObj is Player))
                            continue;

                        if (collided && !(obj is PlayerEnd) )
                        {
                            collidingList.Add(obj);
                        }
                        
                        //bool collided = physObj.HandleCollisions(obj);

                        //If player reaches the end, set the timer to 0
                        if (collided && obj is PlayerEnd && physObj is Player)
                        {
                            mPlayer.mCurrentTexture = PlayerFaces.LAUGH;

                            GameSound.StopOthersAndPlay(GameSound.level_stageVictory);
                            mPhysicsEnvironment.GravityDirection = GravityDirections.Down;
                            gameState = GameStates.Unlock;
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
                            particleEngine.EmitterLocation = new Vector2(obj.mPosition.X + 32, obj.mPosition.Y + 32);
                            particleEngine.Update(10);
                        }
                        //If player hits a hazard
                        else if (collided && ((physObj is Player) && obj.CollisionType == XmlKeys.HAZARDOUS || (obj is Player) && physObj.CollisionType == XmlKeys.HAZARDOUS))
                        {
                            if (physObj is Player) physObj.Kill();
                            else ((Player)obj).Kill();
                            

                            //Get difference of two positions
                            mDeathPanLength = Vector3.Subtract(new Vector3(mPlayer.SpawnPoint.X - 275, mPlayer.SpawnPoint.Y - 100, 0), mCam.Position);
                            //Divide by scaling factor to get camera pan at each update.
                            mDeathPanLength = Vector3.Divide(mDeathPanLength, SCALING_FACTOR);
                            //Set the update counter to zero
                            mDeathPanUpdates = 0;

                            gameState = GameStates.Death;
                            mDeathState = DeathStates.Respawning;

                            mHasRespawned = false;

                            return;
                        }
                        
                        // Particle Engine
                        //
                        // Midpoint Formula: (x1 + x2 / 2, y1 + y2 / 2)
                        if (collided && physObj is Player)
                        {
                            Console.WriteLine("Player: " + mPlayer.Position);
                            Console.WriteLine("Obj: " + obj.mPosition);

                            float x1 = mPlayer.Position.X;
                            float y1 = mPlayer.Position.Y;
                            float x2 = obj.mPosition.X;
                            float y2 = obj.mPosition.Y;

                            Vector2 midpoint = new Vector2((x1 + x2) / 2, (y1 + y2) / 2);
                            //particleEngine.EmitterLocation = midpoint;
                            //particleEngine.Update(10);
                        }
                        
                    }
                    physObj.HandleCollisionList(collidingList);
                }
            }
        }
    }
}
