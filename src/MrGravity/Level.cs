using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using MrGravity.Game_Objects;
using MrGravity.Game_Objects.Physics_Objects;
using MrGravity.Game_Objects.Static_Objects;
using MrGravity.Game_Objects.Static_Objects.Triggers;
using MrGravity.Import_Code;
using MrGravity.MISC_Code;
using MrGravity.ParticleEngine;

namespace MrGravity
{
    /// <summary>
    /// This class represents a level in the game
    /// </summary>
    public class Level
    {
        /// <summary>
        /// Gets or sets the name of this level
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the filepath of this level
        /// </summary>
        public string Filepath 
        { 
            get { return _mFilepath; } 
            set{ 
                _mFilepath = value;
                var start = _mFilepath.LastIndexOf('\\');
                var end = _mFilepath.LastIndexOf('.');
                Name = _mFilepath.Substring(start+1, end - start - 1);
            } }
        private string _mFilepath;

        /// <summary>
        /// Gets or sets the size of the level(in pixels)
        /// </summary>
        public Vector2 Size { get; set; }

        /// <summary>
        /// Gets or sets the player starting point of this level(in pixels)
        /// </summary>
        public Vector2 StartingPoint { get; set; }

        public int IdealTime { get; set; }

        public int CollectableCount { get; set; }

        public int Timer => (int)MTimer;

        public int NumCollected => MNumCollected;

        public int NumCollectable => MNumCollectable;

        public int NumLives => _mPlayer.NumLives;

        //Enumerator for different states of death (playing game, in need of respawn, or panning back to start point)
        private enum DeathStates
        {
            Playing,
            Respawning,
            Panning
        }

        private DeathStates _mDeathState = DeathStates.Playing;

        private Vector3 _mDeathPanLength;
        private float _mDeathPanUpdates;
        private static readonly float ScalingFactor = 85;


        private bool _shouldAnimate = true;

        public bool IsMainMenu { get; private set; }

        // Camera
        public static Camera MCam;

        /* Tracks the previous zoom of the camera */
        //private float mPrevZoom = 0.75f;

        /* Timer variable */
        public double MTimer;

        private List<GameObject>[][] _mCollisionMatrix;

        private readonly List<GameObject> _mObjects;
        private readonly List<GameObject> _mCollected;
        private readonly List<GameObject> _mRemoveCollected;
        private readonly List<Trigger> _mTrigger;

        private List<EntityInfo> _mRails;
        private Texture2D _mRailLeft;
        private Texture2D _mRailRight;
        private Texture2D _mRailHor;
        private Texture2D _mRailTop;
        private Texture2D _mRailBottom;
        private Texture2D _mRailVert;

        private ContentManager _mContent;

        private readonly Dictionary<Vector2, AnimatedSprite> _mActiveAnimations;
        private bool _mUseVert = true;
        private List<Vector2> _mCollectableLocations;
        private AnimatedSprite _mCollectableAnimation;

        private AnimatedSprite _mHazardAnimation;
        private AnimatedSprite _mReverseHazardAnimation;

        private Player _mPlayer;
        private PlayerEnd _mPlayerEnd;

        private PhysicsEnvironment _mPhysicsEnvironment;
        public PhysicsEnvironment Environment => _mPhysicsEnvironment;

        private readonly IControlScheme _mControls;

        private bool _mHasRespawned = true;

        /* SpriteFont */
        private SpriteFont _mKootenay;
        private SpriteFont _mQuartz;

        // Particle Engine
        private ParticleEngine.ParticleEngine _collectibleEngine;
        private ParticleEngine.ParticleEngine _wallEngine;
        private GameObject _lastCollided;

        private Particle[] _backgroundParticles;
        private int _backGroundParticleCount;

        /* Title Safe Area */
        private Rectangle _mScreenRect;
        private Rectangle _mBounds;

        /* Scoring Values */
        private int _mTimerStar;
        private int _mCollectionStar;
        private int _mDeathStar;

        public int TimerStar 
        { 
            get { return _mTimerStar; }
            set
            {
                if (value > _mTimerStar)
                {
                    _mTimerStar = value;
                }
            } 
        }
        public int DeathStar
        {
            get { return _mDeathStar; }
            set
            {
                if (value > _mDeathStar)
                {
                    _mDeathStar = value;
                }
            }
        }
        public int CollectionStar
        {
            get { return _mCollectionStar; }
            set
            {
                if (value > _mCollectionStar)
                {
                    _mCollectionStar = value;
                }
            }
        }

        private readonly BackgroundWorker _bw;

        /// <summary>
        /// Resets the Scores for this level to 0.
        /// </summary>
        public void ResetScores()
        {
            _mTimerStar = _mDeathStar = _mCollectionStar = 0;
        }

        #region HUD

        //private Texture2D mHUDTrans;
//        private Texture2D[] mDirections;
        public static int MNumCollected;
        public static int MNumCollectable;
        public static int MDeaths;

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
            _mControls = controls;

            MCam = new Camera(viewport);

            _mBounds = viewport.Bounds;
            _mScreenRect = viewport.TitleSafeArea;

            _mRails = new List<EntityInfo>();

            _mObjects = new List<GameObject>();
            _mCollected = new List<GameObject>();
            _mRemoveCollected = new List<GameObject>();
            _mActiveAnimations = new Dictionary<Vector2, AnimatedSprite>();
            _mTrigger = new List<Trigger>();
            _mPhysicsEnvironment = new PhysicsEnvironment();

            _mTimerStar = _mDeathStar = _mTimerStar = 0;

            _bw = new BackgroundWorker { WorkerReportsProgress = false, WorkerSupportsCancellation = false };
            _bw.DoWork += UpdateParticles;
        }

        public void Dispose()
        {
            _mActiveAnimations.Clear();
            _backgroundParticles = null;
            var temp = _mObjects.Capacity;
            _mObjects.Clear();
            _mObjects.TrimExcess();
            _mCollisionMatrix = null;
            _mTrigger.Clear();
            _mTrigger.TrimExcess();
            _collectibleEngine = null;
            _wallEngine = null;
            _mContent = null;
            if (_mCollectableLocations != null)
            {
                _mCollectableLocations.Clear();
                _mCollectableLocations.TrimExcess();
            }
            GC.Collect();

        }

        /// <summary>
        /// Loads the level from the content manager
        /// </summary>
        /// <param name="content">Content Manager to load from</param>
        public void Load(ContentManager content, string assetName)
        {
            _mKootenay = content.Load<SpriteFont>("fonts/Kootenay");
            _mQuartz = content.Load<SpriteFont>("fonts/QuartzLarge");

//            mDirections = new Texture2D[4];
//            mDirections[3] = content.Load<Texture2D>("HUD/arrow_left");
//            mDirections[2] = content.Load<Texture2D>("HUD/arrow_down");
//            mDirections[1] = content.Load<Texture2D>("HUD/arrow_right");
//            mDirections[0] = content.Load<Texture2D>("HUD/arrow_up");

            _mRailLeft = content.Load<Texture2D>("Images/NonHazards/Rails/RailLeft");
            _mRailHor = content.Load<Texture2D>("Images/NonHazards/Rails/RailHorizontal");
            _mRailRight = content.Load<Texture2D>("Images/NonHazards/Rails/RailRight");
            _mRailTop = content.Load<Texture2D>("Images/NonHazards/Rails/RailTop");
            _mRailBottom = content.Load<Texture2D>("Images/NonHazards/Rails/RailBottom");
            _mRailVert = content.Load<Texture2D>("Images/NonHazards/Rails/RailVertical");

            _mContent = content;

            MNumCollected = 0;
            MNumCollectable = 0;

            // Particle Engine
            var textures = new List<Texture2D>();
            textures.Add(content.Load<Texture2D>("Images/Particles/diamond"));
            textures.Add(content.Load<Texture2D>("Images/Particles/star"));
            _collectibleEngine = new ParticleEngine.ParticleEngine(textures, new Vector2(400, 240), 20);
            _collectibleEngine.ColorScheme = "Yellow";

            textures = new List<Texture2D>();
            textures.Add(content.Load<Texture2D>("Images/Particles/line"));
            textures.Add(content.Load<Texture2D>("Images/Particles/square"));
            _wallEngine = new ParticleEngine.ParticleEngine(textures, new Vector2(400, 240), 20);
            _wallEngine.ColorScheme = "Blue";

            _backGroundParticleCount = 500;
            _backgroundParticles = new Particle[_backGroundParticleCount];
            var random = new Random();
            var particle = content.Load<Texture2D>("Images/Particles/diamond");
            for (var i = 0; i < _backGroundParticleCount; i++)
            {
                var pos = new Vector2(random.Next(-(int)(_mBounds.Width + Size.X) / 2, 3 * (int)(_mBounds.Width + Size.X) / 2),
                    random.Next(-(int)(_mBounds.Height + Size.Y) / 2, 3 * (int)(_mBounds.Height + Size.Y) / 2));
                _backgroundParticles[i] = new Particle(particle, pos, random);
            }

            //lastCollided = new GameObject[2];
            //lastCollided[0] = lastCollided[1] = null;
            _lastCollided = null;

            _mCollectableLocations = new List<Vector2>();
        }

        /// <summary>
        /// Reloads the content in this level
        /// </summary>
        public void Reload()
        {
            Load(_mContent);
        }

        public void ResetAll()
        {
            _mPlayer.Respawn();

            _mActiveAnimations.Clear();

            _mPhysicsEnvironment.GravityDirection = GravityDirections.Down;

            foreach (var gameObject in _mObjects)
                if (gameObject != _mPlayer)
                    gameObject.Respawn();

            _mPlayer.MNumLives = 5;
            _mPlayer.MIsAlive = true;
            MNumCollected = 0;
            MTimer = 0;

            //Add the collected objects back to the object list
            foreach (var collected in _mCollected)
                _mObjects.Add(collected);

            //Reset the collision matrix
            PrepareCollisionMatrix();

            //Clear the collection lists
            _mCollected.Clear();
            _mRemoveCollected.Clear();
        }

        /// <summary>
        /// Loads the level from the content manager
        /// </summary>
        /// <param name="content">Content Manager to load from</param>
        public void Load(ContentManager content)
        {
            _mObjects.Clear();
            Reset();

            var importer = new Importer(content);
            importer.ImportLevel(this);

            _mPlayer = new Player(content, ref _mPhysicsEnvironment,
                _mControls, .8f, EntityInfo.CreatePlayerInfo(GridSpace.GetGridCoord(StartingPoint)));

            _mObjects.Add(_mPlayer);
            _mObjects.AddRange(importer.GetObjects(ref _mPhysicsEnvironment));

            _mPlayerEnd = importer.GetPlayerEnd();
            if (_mPlayerEnd != null)
                _mObjects.Add(_mPlayerEnd);

            _mObjects.AddRange(importer.GetWalls(this).Cast<GameObject>());

            _mRails = importer.GetRails();

            _mTrigger.AddRange(importer.GetTriggers());

            PrepareCollisionMatrix();

            MNumCollected = 0;
            MNumCollectable = 0;

            //Clear the collection lists
            _mCollected.Clear();
            _mRemoveCollected.Clear();

            foreach (var gObject in _mObjects)
            {
                if (gObject.CollisionType == XmlKeys.Collectable)
                    MNumCollectable++;
            }
        }

        /// <summary>
        /// Prepares the collision matrix in the game
        /// </summary>
        private void PrepareCollisionMatrix()
        {
            var gridSize = GridSpace.GetGridCoord(Size);
            _mCollisionMatrix = new List<GameObject>[(int)gridSize.Y][];
            for (var i = 0; i < gridSize.Y; i++)
            {
                _mCollisionMatrix[i] = new List<GameObject>[(int)gridSize.X];
                for (var j = 0; j < gridSize.X; j++)
                    _mCollisionMatrix[i][j] = new List<GameObject>();
            }

            //Adds each object into the collision matrix. Special case is for walls, in which it must extend over for it to be continuously 
                //considered as collidable
            foreach (var obj in _mObjects)
            {
                var gridLoc = GridSpace.GetGridCoord(obj.MPosition);
                if (!(obj is Wall))
                    _mCollisionMatrix[(int)gridLoc.Y][(int)gridLoc.X].Add(obj);
                else
                {
                    var wall = (Wall)obj;
                    for (var i = 0; i < wall.Walls.Count; i+=2)
                    {
                        gridLoc = GridSpace.GetGridCoord(wall.Walls[i].MPosition);
                        _mCollisionMatrix[(int)gridLoc.Y][(int)gridLoc.X].Add(obj);
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
            var newPosition = GridSpace.GetGridCoord(obj.MPosition);
            if (oldPosition.Equals(newPosition)) return;

            _mCollisionMatrix[(int)oldPosition.Y][(int)oldPosition.X].Remove(obj);
            if(!_mCollisionMatrix[(int)newPosition.Y][(int)newPosition.X].Contains(obj))
                _mCollisionMatrix[(int)newPosition.Y][(int)newPosition.X].Add(obj);
        }

        /// <summary>
        /// Removes the object from the matrix(for collectables
        /// </summary>
        /// <param name="obj">Object we want to remove</param>
        private void RemoveFromMatrix(GameObject obj)
        {
            var position = GridSpace.GetGridCoord(obj.MPosition);

            _mCollisionMatrix[(int)position.Y][(int)position.X].Remove(obj);
        }
        
        public void UpdateStars()
        {
            /* TIME -- 100%+, <120%, <140%, >140% */
            if ((int)MTimer <= IdealTime)
            { _mTimerStar = 3; }
            else if (((int)MTimer / IdealTime) <= 1.2) { _mTimerStar = 2; }
            else { _mTimerStar = 1; }

            /* COLLECTABLES -- 100%, >80%, >60%, <60% */
            if (NumCollected == NumCollectable) { _mCollectionStar = 3; }
            else if (((double)NumCollected / NumCollectable) >= 0.8) { _mCollectionStar = 2; }
            else { _mCollectionStar = 1; }

            /* DEATHS -- 0, 1, 2-3, >3 */
            if (MDeaths == 0) { _mDeathStar = 3; }
            else if (MDeaths <= 2) { _mDeathStar = 2; }
            else { _mDeathStar = 1; }
        }

        public void UpdateParticles(object sender, DoWorkEventArgs e)
        {
            for (var i = 0; i < _backGroundParticleCount; i++)// && !IsMainMenu; i++)
            {
                var random = new Random();
                var randomness = new Vector2((float)(random.NextDouble() * 2 - 1), (float)(random.NextDouble() * 2 - 1));
                _backgroundParticles[i].Velocity = Vector2.Multiply(_mPhysicsEnvironment.GravityForce, 15) +
                                           Vector2.Multiply(_mPlayer.Velocity, .25f) + _backgroundParticles[i].Randomness;
                _backgroundParticles[i].Update();

                Vector2 posDiff;
                Vector2 shiftValue;
                if (!IsMainMenu)
                    posDiff = (shiftValue = _mPlayer.Position) - _backgroundParticles[i].Position;
                else
                    posDiff = (shiftValue = new Vector2(_mBounds.Center.X,_mBounds.Center.Y)) - _backgroundParticles[i].Position;

                if (posDiff.X < -_mBounds.Width || posDiff.Y < -_mBounds.Height ||
                    posDiff.X > _mBounds.Width || posDiff.Y > _mBounds.Height)
                        _backgroundParticles[i].Position = posDiff + shiftValue;
            }

        }

        /// <summary>
        /// Updates the level's progress
        /// </summary>
        /// <param name="gameTime">The game time.</param>
        /// <param name="gameState">State of the game.</param>
        public void Update(GameTime gameTime, ref GameStates gameState)
        {
            if (_mPlayer.MIsAlive)// only update while player is alive
            {
                if (!_bw.IsBusy)
                    _bw.RunWorkerAsync();
                
                if (_mDeathState == DeathStates.Playing)
                {
                    MTimer += (gameTime.ElapsedGameTime.TotalSeconds);
                    
                    if (_mPlayerEnd != null)
                        _mPlayerEnd.UpdateFace(MTimer);

                    foreach (var gObject in _mObjects)
                    {
                        if (gObject.CollisionType == XmlKeys.Collectable)
                        {
                            if (_mCollectableAnimation == null)
                            {
                                _mCollectableAnimation = GetAnimation(gObject.MName);
                            }
                            if (!_mCollectableLocations.Contains(gObject.MPosition))
                            {
                                _mCollectableLocations.Add(gObject.MPosition);
                            }
                        }
                        if (gObject.CollisionType == XmlKeys.Hazardous)
                        {
                            if (gObject is ReverseTile)
                            {
                                if (_mReverseHazardAnimation == null)
                                {
                                    _mReverseHazardAnimation = GetAnimation("ReverseMovingHazard");
                                }
                            }
                            else if (gObject is MovingTile)
                            {
                                if (_mHazardAnimation == null)
                                    _mHazardAnimation = GetAnimation("MovingHazard");
                            }
                        }
                        if (gObject is PhysicsObject)
                        {
                            var pObject = (PhysicsObject)gObject;

                            pObject.FixForBounds((int)Size.X, (int)Size.Y, IsMainMenu);
                            var oldPos = GridSpace.GetGridCoord(pObject.MPosition);

                            if (pObject is Player)
                            {
                                ((Player)pObject).CurrentTime = (int)MTimer;
                            } 
                            pObject.Update(gameTime);

                            // Update zoom based on players velocity                 
                            pObject.FixForBounds((int)Size.X, (int)Size.Y, IsMainMenu);
                            UpdateCollisionMatrix(pObject, oldPos);

                            // handle collision right after you move
                            HandleCollisions(pObject, ref gameState);

                            if (pObject is Player)
                                foreach (var trigger in _mTrigger)
                                    trigger.RunTrigger(_mObjects, (Player)pObject);
                        }
                        if (!_mHasRespawned) break;
                    }

                    //Update wall animations
                    for(var i = 0; i < _mActiveAnimations.Count; i++)
                    {
                        var current = _mActiveAnimations.ElementAt(i);
                        current.Value.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
                        if (current.Value.Frame == 0 && current.Value.PreviousFrame == current.Value.LastFrame - 1)
                            _mActiveAnimations.Remove(current.Key); 
                    }

                    //Update collectable animations
                    if (_mCollectableAnimation != null)
                        _mCollectableAnimation.Update((float)gameTime.ElapsedGameTime.TotalSeconds);

                    //Update hazard animations
                    if (_mHazardAnimation != null)
                        _mHazardAnimation.Update((float)gameTime.ElapsedGameTime.TotalSeconds);

                    if (_mReverseHazardAnimation != null)
                        _mReverseHazardAnimation.Update((float)gameTime.ElapsedGameTime.TotalSeconds);

                    //Check to see if we collected anything
                    if (_mRemoveCollected.Count > 0)
                    {
                        MNumCollected = MNumCollectable - (MNumCollectable - _mCollected.Count());

                        //Safely remove the collected objects
                        foreach (var g in _mRemoveCollected)
                        {
                            RemoveFromMatrix(g);
                            _mObjects.Remove(g);
                            _mCollectableLocations.Remove(g.MPosition);
                        }

                        //Then clear the list
                        _mRemoveCollected.Clear();
                    }
                    
                    //Update number of deaths occured
                    MDeaths = 5 - _mPlayer.MNumLives;

                    // Update the camera to keep the player at the center of the screen
                    // Also only update if the velocity if greater than 0.5f in either direction
                    if (!IsMainMenu && (Math.Abs(_mPlayer.ObjectVelocity.X) > 0.5f || Math.Abs(_mPlayer.ObjectVelocity.Y) > 0.5f))
                    {
                       MCam.Position = new Vector3(_mPlayer.Position.X - 275, _mPlayer.Position.Y - 175, 0);
                    }
                    else if(IsMainMenu)
                    {
                        MCam.Position = new Vector3(_mPlayer.SpawnPoint.X - 275, _mPlayer.SpawnPoint.Y - 100, 0);
                    }

                    //Pause
                    if (_mControls.IsStartPressed(false) || Guide.IsVisible)
                        if (gameState == GameStates.InGame)
                            gameState = GameStates.Pause;
                }

                else if (_mDeathState == DeathStates.Respawning)
                {
                    _mDeathState = DeathStates.Panning;
                    Respawn();
                }

                else//Pan back to player after death
                {
                    MCam.Position += _mDeathPanLength;
                    _mDeathPanUpdates++;

                    if (_mDeathPanUpdates == ScalingFactor)
                        _mDeathState = DeathStates.Playing;
                }
            }

            if (!_mPlayer.MIsAlive)
            {
                _mPlayer.StopRumble();
                if (_mControls.IsAPressed(false))// resets game after game over
                {
                    _mPlayer.MNumLives = 5;
                    _mPlayer.MIsAlive = true;
                    MNumCollected = 0;
                    MTimer = 0;

                    //Add the collected objects back to the object list
                    foreach (var collected in _mCollected)
                        _mObjects.Add(collected);

                    //Reset the collision matrix
                    PrepareCollisionMatrix();

                    //Clear the collection lists
                    _mCollected.Clear();
                    _mRemoveCollected.Clear();
                }
            }

            _collectibleEngine.Update(0);
            _wallEngine.Update(0);
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
                MCam.get_transformation() * scale);
            
            for (var i = 0; i < _backGroundParticleCount; i++)// && !IsMainMenu; i++)
                _backgroundParticles[i].Draw(spriteBatch);
            
            foreach (var trigger in _mTrigger)
                trigger.Draw(spriteBatch, gameTime);

            // Loops through all rail objects and draws the appropriate rail image.
            #region DrawRails
            foreach (var rail in _mRails)
            {
                var position = new Vector2(rail.MLocation.X * 64, rail.MLocation.Y * 64);
                var length = Convert.ToInt32(rail.MProperties["Length"]);
                var type = rail.MProperties["Rail"];
                var width = _mRailTop.Width;
                var height = _mRailTop.Height;

                for (var i = 0; i <= length; i++)
                {
                    if (type == "X")
                    {
                        if (i == 0)
                            spriteBatch.Draw(_mRailLeft, new Rectangle(Convert.ToInt32(position.X) + (i*64), Convert.ToInt32(position.Y), width, height), Color.White);
                        else if (i == length)
                            spriteBatch.Draw(_mRailRight, new Rectangle(Convert.ToInt32(position.X) + (i*64), Convert.ToInt32(position.Y), width, height), Color.White);
                        else
                            spriteBatch.Draw(_mRailHor, new Rectangle(Convert.ToInt32(position.X) + (i*64), Convert.ToInt32(position.Y), width, height), Color.White);
                    }
                    else
                    {
                        if (i == 0)
                            spriteBatch.Draw(_mRailTop, new Rectangle(Convert.ToInt32(position.X), Convert.ToInt32(position.Y) + (i*64), width, height), Color.White);
                        else if (i == length)
                            spriteBatch.Draw(_mRailBottom, new Rectangle(Convert.ToInt32(position.X), Convert.ToInt32(position.Y) + (i * 64), width, height), Color.White);
                        else
                            spriteBatch.Draw(_mRailVert, new Rectangle(Convert.ToInt32(position.X), Convert.ToInt32(position.Y) + (i * 64), width, height), Color.White); ;
                    }
                }
            }
            #endregion

            if (_mDeathState == DeathStates.Playing)
            {
                _collectibleEngine.Draw(spriteBatch);
                _wallEngine.Draw(spriteBatch);
            }

            //Draw all of our game objects
            foreach (var gObject in _mObjects)
            {
                if (gObject.CollisionType == XmlKeys.Hazardous)
                {
                    if (gObject is ReverseTile)
                    {
                        if (_mReverseHazardAnimation != null)
                        {
                            _mReverseHazardAnimation.Draw(spriteBatch, gObject.MPosition);
                        }
                    }
                    else if (gObject is MovingTile)
                    {
                        if (_mHazardAnimation != null)
                        {
                            _mHazardAnimation.Draw(spriteBatch, gObject.MPosition);
                        }
                    }
                    else
                    {
                        gObject.Draw(spriteBatch, gameTime);
                    }
                }
                else if (gObject.CollisionType != XmlKeys.Collectable)
                    gObject.Draw(spriteBatch, gameTime);

            }

            //Draw all of our active animations
            if (_shouldAnimate)
            {
                for (var i = 0; i < _mActiveAnimations.Count; i++)
                    _mActiveAnimations.ElementAt(i).Value.Draw(spriteBatch, _mActiveAnimations.ElementAt(i).Key);

                if (_mCollectableAnimation != null)
                {
                    for (var i = 0; i < _mCollectableLocations.Count; i++)
                    {
                        _mCollectableAnimation.Draw(spriteBatch, _mCollectableLocations.ElementAt(i));
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
        public void Respawn()
        {
            _mPlayer.Respawn();

            _mPlayer.StopRumble();
            _mHasRespawned = true;

            _mActiveAnimations.Clear();

            _mPhysicsEnvironment.GravityDirection = GravityDirections.Down;

            //Only play respawn noise when player is still alive
            if (_mPlayer.MNumLives > 0)
                GameSound.PlayerSoundRespawn.Play(GameSound.Volume * 0.8f, 0.0f, 0.0f);

            foreach (var gameObject in _mObjects)
                if(gameObject != _mPlayer)
                    gameObject.Respawn();
        }

        /// <summary>
        /// Preps the level to reload content
        /// </summary>
        public void Reset()
        {
            _mPhysicsEnvironment.GravityDirection = GravityDirections.Down;
            _mObjects.Clear();
            _mCollected.Clear();
            _mRemoveCollected.Clear();
            _mTrigger.Clear();
            MTimer = 0;
            if (_mPlayer != null)
            {
                _mPlayer.ResetIdle((int)MTimer, _mPhysicsEnvironment.GravityDirection);
            }

            if (_mPlayerEnd != null)
            {
                _mPlayerEnd.UpdateFace(MTimer);
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
            var collidingList = new List<GameObject>();

            var gridPos = GridSpace.GetGridCoord(physObj.MPosition);

            //Goes through the 9 possible positions for collision to see if this physics object is colliding with anything
            for (var i = -1; i < 2; i++)
            {
                if (gridPos.Y + i < 0 || gridPos.Y + i >= _mCollisionMatrix.Length) continue;//Bounds check
                for (var j = -1; j < 2; j++)
                {
                    if (gridPos.X + j < 0 || gridPos.X + j >= _mCollisionMatrix[(int)gridPos.Y + i].Length) continue;//Bounds check

                    
                    foreach (var obj in _mCollisionMatrix[(int)gridPos.Y+i][(int)gridPos.X+j])
                    {
                        var collided = false;

                        if (physObj.IsSquare && obj.IsSquare)// both squares
                        {
                            collided = physObj.IsCollidingBoxAndBox(obj);
                        }
                        else if (!physObj.IsSquare && obj.IsSquare) // phys obj is circle
                        {
                            collided = physObj.IsCollidingCircleAndBox(obj);
                        }
                        else if (physObj.IsSquare && !obj.IsSquare) //obj is circle 
                        {
                            collided = physObj.IsCollidingBoxAndCircle(obj);
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
                            _mPlayer.MCurrentTexture = PlayerFaces.FromString("Laugh");
                            _mPlayerEnd.MCurrentTexture = PlayerFaces.FromString("GirlLaugh3");

                            GameSound.StopOthersAndPlay(GameSound.LevelStageVictory);
                            _mPhysicsEnvironment.GravityDirection = GravityDirections.Down;
                            gameState = GameStates.Unlock;
                        }

                        //If player collided with a collectable object
                        if (collided && ((physObj is Player) && obj.CollisionType == XmlKeys.Collectable || (obj is Player) && physObj.CollisionType == XmlKeys.Collectable))
                        {
                            if (physObj.CollisionType == XmlKeys.Collectable)
                            {
                                _mCollected.Add(physObj);
                                _mRemoveCollected.Add(physObj);
                                _mCollectableLocations.Remove(physObj.MPosition);
                            }
                            else if (obj.CollisionType == XmlKeys.Collectable)
                            {
                                _mCollected.Add(obj);
                                _mRemoveCollected.Add(obj);
                                _mCollectableLocations.Remove(obj.MPosition);
                            }

                            GameSound.PlayerColCollectable.Play(GameSound.Volume * 0.8f, 0f, 0f);
                            _collectibleEngine.EmitterLocation = new Vector2(obj.MPosition.X + 32, obj.MPosition.Y + 32);
                            _collectibleEngine.Update(10);
                        }
                        //If player hits a hazard
                        else if (collided && ((physObj is Player) && obj.CollisionType == XmlKeys.Hazardous || (obj is Player) && physObj.CollisionType == XmlKeys.Hazardous))
                        {
                            // Particle Effects (don't work).
                            //Vector2 one = new Vector2(obj.mPosition.X + 32, obj.mPosition.Y + 32);
                            //Vector2 two = new Vector2(physObj.mPosition.X + 32, physObj.mPosition.Y + 32);
                            //Vector2 midpoint = new Vector2((one.X + two.X) / 2, (one.Y + two.Y) / 2);
                            //wallEngine.EmitterLocation = midpoint;
                            //wallEngine.Update(10);
                            GameSound.PlayerSoundDeath.Play(GameSound.Volume * 0.8f, 0.0f, 0.0f);


                            if (physObj is Player)
                            {
                                physObj.Kill();
                                _mPlayerEnd.MCurrentTexture = PlayerFaces.FromString("GirlSad");
                            }
                            else
                            {
                                ((Player)obj).Kill();
                                _mPlayerEnd.MCurrentTexture = PlayerFaces.FromString("GirlSad");
                            }

                            //Get difference of two positions
                            _mDeathPanLength = Vector3.Subtract(new Vector3(_mPlayer.SpawnPoint.X - 275, _mPlayer.SpawnPoint.Y - 100, 0), MCam.Position);
                            //Divide by scaling factor to get camera pan at each update.
                            _mDeathPanLength = Vector3.Divide(_mDeathPanLength, ScalingFactor);
                            //Set the update counter to zero
                            _mDeathPanUpdates = 0;

                            gameState = GameStates.Death;
                            _mDeathState = DeathStates.Respawning;

                            _mHasRespawned = false;

                            return;
                        }
                        
                    }

                    //Start any animations on walls we are touching
                    if (physObj is Player)
                        foreach (var cObject in collidingList)
                        {
                            if (cObject is Wall)
                            {
                                var animation = ((Wall)cObject).NearestWallPosition(physObj.MPosition);
                                if (!_mActiveAnimations.ContainsKey(animation.Key))
                                    _mActiveAnimations.Add(animation.Key, GetAnimation(animation.Value));

                                // Particle Effects.
                                //if (cObject != lastCollided[0] && cObject != lastCollided[1])
                                if (cObject != _lastCollided)
                                {
                                    var one = new Vector2(_mPlayer.Position.X + 32, _mPlayer.Position.Y + 32);
                                    var two = new Vector2(animation.Key.X + 32, animation.Key.Y + 32);
                                    var midpoint = new Vector2((one.X + two.X) / 2, (one.Y + two.Y) / 2);
                                    _wallEngine.EmitterLocation = midpoint;
                                    _wallEngine.Update(10);

                                    // play wall collision sound
                                    GameSound.PlayerColWall.Play(GameSound.Volume * 0.8f, 0f, 0f);

                                    //lastCollided[1] = lastCollided[0];
                                    //lastCollided[0] = cObject;
                                    _lastCollided = cObject;

                                }
                            }

                            else if (cObject is MovingTile && !((MovingTile)cObject).BeingAnimated && cObject.CollisionType != XmlKeys.Hazardous)
                                ((MovingTile)cObject).StartAnimation(GetAnimation(cObject.MName));
                            else if (cObject is ReverseTile && !((ReverseTile)cObject).BeingAnimated && cObject.CollisionType != XmlKeys.Hazardous)
                                ((ReverseTile)cObject).StartAnimation(GetAnimation(cObject.MName));
                            else if (cObject is StaticObject && cObject.CollisionType != XmlKeys.Collectable)
                            {
                                if (!_mActiveAnimations.ContainsKey(cObject.MPosition))
                                    _mActiveAnimations.Add(cObject.MPosition, GetAnimation(cObject.MName));

                                // Particle Effects.
                                //if (cObject != lastCollided[0] && cObject != lastCollided[1])
                                if (cObject != _lastCollided)
                                {
                                    var one = new Vector2(_mPlayer.Position.X + 32, _mPlayer.Position.Y + 32);
                                    var two = new Vector2(cObject.MPosition.X + 32, cObject.MPosition.Y + 32);
                                    var midpoint = new Vector2((one.X + two.X) / 2, (one.Y + two.Y) / 2);
                                    _wallEngine.EmitterLocation = midpoint;
                                    _wallEngine.Update(10);

                                    // play wall collision sound
                                    GameSound.PlayerColWall.Play(GameSound.Volume * 0.8f, 0f, 0f);

                                    //lastCollided[1] = lastCollided[0];
                                    //lastCollided[0] = cObject;
                                    _lastCollided = cObject;

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
            var concatName = name.Substring(name.LastIndexOf('\\') + 1);
            var newAnimation = new AnimatedSprite();

            _wallEngine.ColorScheme = concatName;

            switch (concatName)
            {
                case "Green":
                    if(_mUseVert)
                        newAnimation.Load(_mContent, "GreenVert", 4, 0.15f);
                    else newAnimation.Load(_mContent, "GreenHor", 4, 0.15f);
                    _mUseVert = !_mUseVert; 
                    break;
                case "Pink":
                    if(_mUseVert)
                        newAnimation.Load(_mContent, "PinkVert", 4, 0.15f);
                    else newAnimation.Load(_mContent, "PinkHor", 4, 0.15f);
                    _mUseVert = !_mUseVert;
                    break;
                case "Blue":
                    if(_mUseVert)
                        newAnimation.Load(_mContent, "BlueVert", 4, 0.15f);
                    else newAnimation.Load(_mContent, "BlueHor", 4, 0.15f);
                    _mUseVert = !_mUseVert;
                    break;
                case "Yellow":
                    if(_mUseVert)
                        newAnimation.Load(_mContent, "YellowVert", 4, 0.15f);
                    else newAnimation.Load(_mContent, "YellowHor", 4, 0.15f);
                    _mUseVert = !_mUseVert;
                    break;
                case "Purple":
                    if(_mUseVert)
                        newAnimation.Load(_mContent, "PurpleVert", 4, 0.15f);
                    else newAnimation.Load(_mContent, "PurpleHor", 4, 0.15f);
                    _mUseVert = !_mUseVert;
                    break;
                case "Orange":
                    if(_mUseVert)
                        newAnimation.Load(_mContent, "OrangeVert", 4, 0.15f);
                    else newAnimation.Load(_mContent, "OrangeHor", 4, 0.15f);
                    _mUseVert = !_mUseVert;
                    break;
                case "GreenDiamond":
                    newAnimation.Load(_mContent, "GreenDiamond", 3, 0.225f);
                    break;
                case "BlueDiamond":
                    newAnimation.Load(_mContent, "BlueDiamond", 3, 0.225f);
                    break;
                case "OrangeDiamond":
                    newAnimation.Load(_mContent, "OrangeDiamond", 3, 0.225f);
                    break;
                case "PinkDiamond":
                    newAnimation.Load(_mContent, "PinkDiamond", 3, 0.225f);
                    break;
                case "PurpleDiamond":
                    newAnimation.Load(_mContent, "PurpleDiamond", 3, 0.225f);
                    break;
                case "YellowDiamond":
                    newAnimation.Load(_mContent, "YellowDiamond", 3, 0.225f);
                    break;
                case "BlueGem":
                    newAnimation.Load(_mContent, "BlueGem", 6, 0.15f);
                    break;
                case "OrangeGem":
                    newAnimation.Load(_mContent, "OrangeGem", 6, 0.15f);
                    break;
                case "PinkGem":
                    newAnimation.Load(_mContent, "PinkGem", 6, 0.15f);
                    break;
                case "PurpleGem":
                    newAnimation.Load(_mContent, "PurpleGem", 6, 0.15f);
                    break;
                case "GreenGem":
                    newAnimation.Load(_mContent, "GreenGem", 6, 0.15f);
                    break;
                case "YellowGem":
                    newAnimation.Load(_mContent, "YellowGem", 6, 0.15f);
                    break;
                case "MovingHazard":
                    newAnimation.Load(_mContent, "MovingHazard", 4, 0.15f);
                    break;
                case "ReverseMovingHazard":
                    newAnimation.Load(_mContent, "ReverseMovingHazard", 4, 0.15f);
                    break;
                default:
                    newAnimation.Load(_mContent, "NoAnimation", 1, 0.5f);
                    break;
            }
            return newAnimation;
        }

        public static Level MainMenuLevel(string filepath, IControlScheme controls, Viewport viewport, Rectangle region)
        {
            var main = new Level(filepath, controls, viewport);
            main.IsMainMenu = true;
            main._shouldAnimate = false;
            main.Size = new Vector2(region.Width * 4 / 3, region.Height * 4 / 3);
            main.StartingPoint = new Vector2(region.Width * 11 / 18, region.Height / 4);
            return main;
        }
    }
}
