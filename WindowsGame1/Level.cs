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

        private int mIdealTime;
        public int IdealTime
        { get { return mIdealTime; } set { mIdealTime = value; } }

        private int mCollectableCount;
        public int CollectableCount
        { get { return mCollectableCount; } set { mCollectableCount = value; } }

        public int Timer
        {
            get { return (int)mTimer; }
        }

        public int NumCollected
        {
            get { return mNumCollected; }
        }

        public int NumCollectable
        {
            get { return mNumCollectable; }
        }

        public int NumLives
        {
            get { return mPlayer.NumLives; }
        }

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
        private static float SCALING_FACTOR = 85;


        private bool isCameraFixed = false;
        private bool shouldAnimate = true;

        public bool IsMainMenu
        {
            get { return isCameraFixed; }
        }
        // Camera
        public static Camera mCam;

        /* Tracks the previous zoom of the camera */
        //private float mPrevZoom = 0.75f;

        /* Timer variable */
        public double mTimer;

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

        ContentManager mContent;

        Dictionary<Vector2, AnimatedSprite> mActiveAnimations;
        bool mUseVert = true;
        List<Vector2> mCollectableLocations;
        AnimatedSprite mCollectableAnimation = null;

        Player mPlayer;
        PlayerEnd mPlayerEnd;

        private PhysicsEnvironment mPhysicsEnvironment;
        public PhysicsEnvironment Environment
        { get { return mPhysicsEnvironment; } }

        IControlScheme mControls;

        bool mHasRespawned = true;

        /* SpriteFont */
        SpriteFont mKootenay;
        SpriteFont mQuartz;

        // Particle Engine
        ParticleEngine collectibleEngine;
        ParticleEngine wallEngine;
        GameObject[] lastCollided;

        /* Title Safe Area */
        Rectangle mScreenRect;

        /* Scoring Values */
        int mTimerStar;
        int mCollectionStar;
        int mDeathStar;

        public int TimerStar 
        { 
            get { return mTimerStar; }
            set
            {
                if (value > mTimerStar)
                {
                    mTimerStar = value;
                }
            } 
        }
        public int DeathStar
        {
            get { return mDeathStar; }
            set
            {
                if (value > mDeathStar)
                {
                    mDeathStar = value;
                }
            }
        }
        public int CollectionStar
        {
            get { return mCollectionStar; }
            set
            {
                if (value > mCollectionStar)
                {
                    mCollectionStar = value;
                }
            }
        }

        /// <summary>
        /// Resets the Scores for this level to 0.
        /// </summary>
        public void ResetScores()
        {
            mTimerStar = mDeathStar = mCollectionStar = 0;
        }

        #region HUD

        //private Texture2D mHUDTrans;
//        private Texture2D[] mDirections;
        public static int mNumCollected;
        public static int mNumCollectable;
        public static int mDeaths;

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

            mScreenRect = viewport.TitleSafeArea;

            mRails = new List<EntityInfo>();

            mObjects = new List<GameObject>();
            mCollected = new List<GameObject>();
            mRemoveCollected = new List<GameObject>();
            mActiveAnimations = new Dictionary<Vector2, AnimatedSprite>();
            mTrigger = new List<Trigger>();
            mPhysicsEnvironment = new PhysicsEnvironment();

            mTimerStar = mDeathStar = mTimerStar = 0;
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

            mContent = content;

            mNumCollected = 0;
            mNumCollectable = 0;

            // Particle Engine
            List<Texture2D> textures = new List<Texture2D>();
            textures.Add(content.Load<Texture2D>("Images/Particles/diamond"));
            textures.Add(content.Load<Texture2D>("Images/Particles/star"));
            collectibleEngine = new ParticleEngine(textures, new Vector2(400, 240), 20);
            collectibleEngine.colorScheme = "Yellow";

            textures = new List<Texture2D>();
            textures.Add(content.Load<Texture2D>("Images/Particles/line"));
            textures.Add(content.Load<Texture2D>("Images/Particles/square"));
            wallEngine = new ParticleEngine(textures, new Vector2(400, 240), 20);
            wallEngine.colorScheme = "Blue";


            lastCollided = new GameObject[2];
            lastCollided[0] = lastCollided[1] = null;

 //           lastCollided = null;

            mCollectableLocations = new List<Vector2>();
        }

        /// <summary>
        /// Reloads the content in this level
        /// </summary>
        public void Reload()
        {
            Load(mContent);
        }


        /// <summary>
        /// Loads the level from the content manager
        /// </summary>
        /// <param name="content">Content Manager to load from</param>
        public void Load(ContentManager content)
        {
            mObjects.Clear();
            Reset();

            Importer importer = new Importer(content);
            importer.ImportLevel(this);

            mPlayer = new Player(content, ref mPhysicsEnvironment,
                mControls, .8f, EntityInfo.CreatePlayerInfo(GridSpace.GetGridCoord(StartingPoint)));

            mObjects.Add(mPlayer);
            mObjects.AddRange(importer.GetObjects(ref mPhysicsEnvironment));

            mPlayerEnd = importer.GetPlayerEnd();
            if (mPlayerEnd != null)
                mObjects.Add(mPlayerEnd);

            mObjects.AddRange(importer.GetWalls(this).Cast<GameObject>());

            mRails = importer.GetRails();

            mTrigger.AddRange(importer.GetTriggers());

            PrepareCollisionMatrix();

            mNumCollected = 0;
            mNumCollectable = 0;

            //Clear the collection lists
            mCollected.Clear();
            mRemoveCollected.Clear();

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

        public void UpdateStars()
        {
            /* TIME -- 100%+, <120%, <140%, >140% */
            if ((int)mTimer <= mIdealTime)
            { mTimerStar = 3; }
            else if (((int)mTimer / mIdealTime) <= 1.2) { mTimerStar = 2; }
            else { mTimerStar = 1; }

            /* COLLECTABLES -- 100%, >80%, >60%, <60% */
            if (NumCollected == NumCollectable) { mCollectionStar = 3; }
            else if ((NumCollected / NumCollectable) >= 0.8) { mCollectionStar = 2; }
            else { mCollectionStar = 1; }

            /* DEATHS -- 0, 1, 2-3, >3 */
            if (mDeaths == 0) { mDeathStar = 3; }
            else if (mDeaths <= 2) { mDeathStar = 2; }
            else { mDeathStar = 1; }
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
                    mTimer += (gameTime.ElapsedGameTime.TotalSeconds);
                    
                    if (mPlayerEnd != null)
                        mPlayerEnd.UpdateFace(mTimer);

                    foreach (GameObject gObject in mObjects)
                    {
                        if (gObject.CollisionType == XmlKeys.COLLECTABLE)
                        {
                            if (mCollectableAnimation == null)
                            {
                                mCollectableAnimation = GetAnimation(gObject.mName);
                            }
                            if (!mCollectableLocations.Contains(gObject.mPosition))
                            {
                                mCollectableLocations.Add(gObject.mPosition);
                            }
                        }
                        if (gObject is PhysicsObject)
                        {
                            PhysicsObject pObject = (PhysicsObject)gObject;

                            pObject.FixForBounds((int)Size.X, (int)Size.Y, isCameraFixed);
                            Vector2 oldPos = GridSpace.GetGridCoord(pObject.mPosition);

                            if (pObject is Player)
                            {
                                ((Player)pObject).CurrentTime = (int)mTimer;
                            } 
                            pObject.Update(gameTime);

                            // Update zoom based on players velocity                 
                            pObject.FixForBounds((int)Size.X, (int)Size.Y, isCameraFixed);
                            UpdateCollisionMatrix(pObject, oldPos);

                            // handle collision right after you move
                            HandleCollisions(pObject, ref gameState);

                            if (pObject is Player)
                                foreach (Trigger trigger in mTrigger)
                                    trigger.RunTrigger(mObjects, (Player)pObject);
                        }
                        if (!mHasRespawned) break;
                    }

                    //Update wall animations
                    for(int i = 0; i < mActiveAnimations.Count; i++)
                    {
                        KeyValuePair<Vector2, AnimatedSprite> current = mActiveAnimations.ElementAt(i);
                        current.Value.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
                        if (current.Value.Frame == 0 && current.Value.PreviousFrame == current.Value.LastFrame - 1)
                            mActiveAnimations.Remove(current.Key); 
                    }

                    //Update collectable animations
                    if (mCollectableAnimation != null)
                        mCollectableAnimation.Update((float)gameTime.ElapsedGameTime.TotalSeconds);

                    //Check to see if we collected anything
                    if (mRemoveCollected.Count > 0)
                    {
                        mNumCollected = mNumCollectable - (mNumCollectable - mCollected.Count());

                        //Safely remove the collected objects
                        foreach (GameObject g in mRemoveCollected)
                        {
                            RemoveFromMatrix(g);
                            mObjects.Remove(g);
                            mCollectableLocations.Remove(g.mPosition);
                        }

                        //Then clear the list
                        mRemoveCollected.Clear();
                    }
                    
                    //Update number of deaths occured
                    mDeaths = 5 - mPlayer.mNumLives;

                    // Update the camera to keep the player at the center of the screen
                    // Also only update if the velocity if greater than 0.5f in either direction
                    if (!isCameraFixed && (Math.Abs(mPlayer.ObjectVelocity.X) > 0.5f || Math.Abs(mPlayer.ObjectVelocity.Y) > 0.5f))
                    {
                       mCam.Position = new Vector3(mPlayer.Position.X - 275, mPlayer.Position.Y - 175, 0);
                    }
                    else if(isCameraFixed)
                    {
                        mCam.Position = new Vector3(mPlayer.SpawnPoint.X - 275, mPlayer.SpawnPoint.Y - 100, 0);
                    }

                    //Pause
                    if (mControls.isStartPressed(false) || Guide.IsVisible)
                        if (gameState == GameStates.In_Game)
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
                    mDeathPanUpdates++;

                    if (mDeathPanUpdates == SCALING_FACTOR)
                        mDeathState = DeathStates.Playing;
                }
            }

            if (!mPlayer.mIsAlive)
            {
                mPlayer.StopRumble();
                if (mControls.isAPressed(false))// resets game after game over
                {
                    mPlayer.mNumLives = 5;
                    mPlayer.mIsAlive = true;
                    mNumCollected = 0;
                    mTimer = 0;

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

            collectibleEngine.Update(0);
            wallEngine.Update(0);
        }

        /// <summary>
        /// Draws the level background on to the screen
        /// </summary>
        /// <param name="spriteBatch">Sprite batch that we use to draw textures</param>
        public void Draw(SpriteBatch spriteBatch, GameTime gameTime, Matrix scale)
        {
            /* Cam is used to draw everything except the HUD - SEE BELOW FOR DRAWING HUD */
            spriteBatch.Begin(SpriteSortMode.Immediate,
                BlendState.AlphaBlend,
                SamplerState.LinearClamp,
                DepthStencilState.None,
                RasterizerState.CullCounterClockwise,
                null,
                mCam.get_transformation() * scale);

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

            if (mDeathState == DeathStates.Playing)
            {
                collectibleEngine.Draw(spriteBatch);
                wallEngine.Draw(spriteBatch);
            }

            //Draw all of our game objects
            foreach (GameObject gObject in mObjects)
                if (gObject.CollisionType != XmlKeys.COLLECTABLE)
                    gObject.Draw(spriteBatch, gameTime);

            //Draw all of our active animations
            if (shouldAnimate)
            {
                for (int i = 0; i < mActiveAnimations.Count; i++)
                    mActiveAnimations.ElementAt(i).Value.Draw(spriteBatch, mActiveAnimations.ElementAt(i).Key);

                if (mCollectableAnimation != null)
                {
                    for (int i = 0; i < mCollectableLocations.Count; i++)
                    {
                        mCollectableAnimation.Draw(spriteBatch, mCollectableLocations.ElementAt(i));
                    }
                }

            }

            spriteBatch.End();
        }

        /// <summary>
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

            mActiveAnimations.Clear();

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
            mTimer = 0;
            if (mPlayer != null)
            {
                mPlayer.ResetIdle((int)mTimer, mPhysicsEnvironment.GravityDirection);
            }

            if (mPlayerEnd != null)
            {
                mPlayerEnd.UpdateFace(mTimer);
            }
            

            ResetScores();
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

                        if (physObj.IsSquare && obj.IsSquare)// both squares
                        {
                            collided = physObj.IsCollidingBoxAndBox(obj);
                        }
                        else if (!physObj.IsSquare && obj.IsSquare) // phys obj is circle
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

                        if (collided && !(obj is PlayerEnd))
                        {
                            collidingList.Add(obj);
                        }

                        //If player reaches the end, set the timer to 0
                        if (collided && obj is PlayerEnd && physObj is Player)
                        {
                            mPlayer.mCurrentTexture = PlayerFaces.FromString("Laugh");
                            mPlayerEnd.mCurrentTexture = PlayerFaces.FromString("GirlLaugh3");

                            GameSound.StopOthersAndPlay(GameSound.level_stageVictory);
                            mPhysicsEnvironment.GravityDirection = GravityDirections.Down;
                            gameState = GameStates.Unlock;
                        }

                        //If player collided with a collectable object
                        if (collided && ((physObj is Player) && obj.CollisionType == XmlKeys.COLLECTABLE || (obj is Player) && physObj.CollisionType == XmlKeys.COLLECTABLE))
                        {
                            if (physObj.CollisionType == XmlKeys.COLLECTABLE)
                            {
                                mCollected.Add(physObj);
                                mRemoveCollected.Add(physObj);
                                mCollectableLocations.Remove(physObj.mPosition);
                            }
                            else if (obj.CollisionType == XmlKeys.COLLECTABLE)
                            {
                                mCollected.Add(obj);
                                mRemoveCollected.Add(obj);
                                mCollectableLocations.Remove(obj.mPosition);
                            }

                            GameSound.playerCol_collectable.Play(GameSound.volume * 0.8f, 0f, 0f);
                            collectibleEngine.EmitterLocation = new Vector2(obj.mPosition.X + 32, obj.mPosition.Y + 32);
                            collectibleEngine.Update(10);
                        }
                        //If player hits a hazard
                        else if (collided && ((physObj is Player) && obj.CollisionType == XmlKeys.HAZARDOUS || (obj is Player) && physObj.CollisionType == XmlKeys.HAZARDOUS))
                        {
                            // Particle Effects (don't work).
                            //Vector2 one = new Vector2(obj.mPosition.X + 32, obj.mPosition.Y + 32);
                            //Vector2 two = new Vector2(physObj.mPosition.X + 32, physObj.mPosition.Y + 32);
                            //Vector2 midpoint = new Vector2((one.X + two.X) / 2, (one.Y + two.Y) / 2);
                            //wallEngine.EmitterLocation = midpoint;
                            //wallEngine.Update(10);
                            GameSound.playerSound_death.Play(GameSound.volume * 0.8f, 0.0f, 0.0f);


                            if (physObj is Player)
                            {
                                physObj.Kill();
                                mPlayerEnd.mCurrentTexture = PlayerFaces.FromString("GirlSad");
                            }
                            else
                            {
                                ((Player)obj).Kill();
                                mPlayerEnd.mCurrentTexture = PlayerFaces.FromString("GirlSad");
                            }

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
                        
                    }

                    //Start any animations on walls we are touching
                    if (physObj is Player)
                        foreach (GameObject cObject in collidingList)
                        {
                            if (cObject is Wall)
                            {
                                KeyValuePair<Vector2, string> animation = ((Wall)cObject).NearestWallPosition(physObj.mPosition);
                                if (!mActiveAnimations.ContainsKey(animation.Key))
                                    mActiveAnimations.Add(animation.Key, GetAnimation(animation.Value));

                                // Particle Effects.
                                if (cObject != lastCollided[0] && cObject != lastCollided[1])
                                {
                                    Vector2 one = new Vector2(mPlayer.Position.X + 32, mPlayer.Position.Y + 32);
                                    Vector2 two = new Vector2(animation.Key.X + 32, animation.Key.Y + 32);
                                    Vector2 midpoint = new Vector2((one.X + two.X) / 2, (one.Y + two.Y) / 2);
                                    wallEngine.EmitterLocation = midpoint;
                                    wallEngine.Update(10);

                                    // play wall collision sound
                                    GameSound.playerCol_wall.Play(GameSound.volume * 0.8f, 0f, 0f);

                                    lastCollided[1] = lastCollided[0];
                                    lastCollided[0] = cObject;

                                }
                            }

                            else if (cObject is MovingTile && !((MovingTile)cObject).BeingAnimated && cObject.CollisionType != XmlKeys.HAZARDOUS)
                                ((MovingTile)cObject).StartAnimation(GetAnimation(cObject.mName));
                            else if (cObject is ReverseTile && !((ReverseTile)cObject).BeingAnimated && cObject.CollisionType != XmlKeys.HAZARDOUS)
                                ((ReverseTile)cObject).StartAnimation(GetAnimation(cObject.mName));
                            else if (cObject is StaticObject && cObject.CollisionType != XmlKeys.COLLECTABLE)
                            {
                                if (!mActiveAnimations.ContainsKey(cObject.mPosition))
                                    mActiveAnimations.Add(cObject.mPosition, GetAnimation(cObject.mName));

                                // Particle Effects.
                                if (cObject != lastCollided[0] && cObject != lastCollided[1])
                                {
                                    Vector2 one = new Vector2(mPlayer.Position.X + 32, mPlayer.Position.Y + 32);
                                    Vector2 two = new Vector2(cObject.mPosition.X + 32, cObject.mPosition.Y + 32);
                                    Vector2 midpoint = new Vector2((one.X + two.X) / 2, (one.Y + two.Y) / 2);
                                    wallEngine.EmitterLocation = midpoint;
                                    wallEngine.Update(10);

                                    // play wall collision sound
                                    GameSound.playerCol_wall.Play(GameSound.volume * 0.8f, 0f, 0f);

                                    lastCollided[1] = lastCollided[0];
                                    lastCollided[0] = cObject;

                                }
                            }
                        }
                   
                    physObj.HandleCollisionList(collidingList);
                }
            }
        }

        /// <summary>
        /// Returns the proper animation for the given tile
        /// </summary>
        /// <param name="name">Name of the tile that needs to be animated</param>
        /// <returns></returns>
        private AnimatedSprite GetAnimation(string name)
        {
            string concatName = name.Substring(name.LastIndexOf('\\') + 1);
            AnimatedSprite newAnimation = new AnimatedSprite();

            wallEngine.colorScheme = concatName;

            switch (concatName)
            {
                case "Green":
                    if(mUseVert)
                        newAnimation.Load(mContent, "GreenVert", 4, 0.15f);
                    else newAnimation.Load(mContent, "GreenHor", 4, 0.15f);
                    mUseVert = !mUseVert; 
                    break;
                case "Pink":
                    if(mUseVert)
                        newAnimation.Load(mContent, "PinkVert", 4, 0.15f);
                    else newAnimation.Load(mContent, "PinkHor", 4, 0.15f);
                    mUseVert = !mUseVert;
                    break;
                case "Blue":
                    if(mUseVert)
                        newAnimation.Load(mContent, "BlueVert", 4, 0.15f);
                    else newAnimation.Load(mContent, "BlueHor", 4, 0.15f);
                    mUseVert = !mUseVert;
                    break;
                case "Yellow":
                    if(mUseVert)
                        newAnimation.Load(mContent, "YellowVert", 4, 0.15f);
                    else newAnimation.Load(mContent, "YellowHor", 4, 0.15f);
                    mUseVert = !mUseVert;
                    break;
                case "Purple":
                    if(mUseVert)
                        newAnimation.Load(mContent, "PurpleVert", 4, 0.15f);
                    else newAnimation.Load(mContent, "PurpleHor", 4, 0.15f);
                    mUseVert = !mUseVert;
                    break;
                case "Orange":
                    if(mUseVert)
                        newAnimation.Load(mContent, "OrangeVert", 4, 0.15f);
                    else newAnimation.Load(mContent, "OrangeHor", 4, 0.15f);
                    mUseVert = !mUseVert;
                    break;
                case "GreenDiamond":
                    newAnimation.Load(mContent, "GreenDiamond", 3, 0.225f);
                    break;
                case "BlueDiamond":
                    newAnimation.Load(mContent, "BlueDiamond", 3, 0.225f);
                    break;
                case "OrangeDiamond":
                    newAnimation.Load(mContent, "OrangeDiamond", 3, 0.225f);
                    break;
                case "PinkDiamond":
                    newAnimation.Load(mContent, "PinkDiamond", 3, 0.225f);
                    break;
                case "PurpleDiamond":
                    newAnimation.Load(mContent, "PurpleDiamond", 3, 0.225f);
                    break;
                case "YellowDiamond":
                    newAnimation.Load(mContent, "YellowDiamond", 3, 0.225f);
                    break;
                case "BlueGem":
                    newAnimation.Load(mContent, "BlueGem", 6, 0.15f);
                    break;
                case "OrangeGem":
                    newAnimation.Load(mContent, "OrangeGem", 6, 0.15f);
                    break;
                case "PinkGem":
                    newAnimation.Load(mContent, "PinkGem", 6, 0.15f);
                    break;
                case "PurpleGem":
                    newAnimation.Load(mContent, "PurpleGem", 6, 0.15f);
                    break;
                case "GreenGem":
                    newAnimation.Load(mContent, "GreenGem", 6, 0.15f);
                    break;
                case "YellowGem":
                    newAnimation.Load(mContent, "YellowGem", 6, 0.15f);
                    break;
                default:
                    newAnimation.Load(mContent, "NoAnimation", 1, 0.5f);
                    break;
            }
            return newAnimation;
        }

        public static Level MainMenuLevel(string filepath, IControlScheme controls, Viewport viewport, Rectangle region)
        {
            Level main = new Level(filepath,controls,viewport);
            main.isCameraFixed = true;
            main.shouldAnimate = false;
            main.Size = new Vector2(region.Width*33/32, region.Height*7/8);
            main.mStartingPoint = new Vector2(region.Width/2, region.Height/8);
            return main;
        }
    }
}
