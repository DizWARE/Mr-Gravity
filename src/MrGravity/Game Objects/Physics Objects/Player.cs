using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MrGravity.Import_Code;
using MrGravity.MISC_Code;

namespace MrGravity.Game_Objects.Physics_Objects
{
    /// <summary>
    /// Represents the player in the game
    /// </summary>
    internal class Player : PhysicsObject
    {
        private readonly IControlScheme _mControls;

        public Vector2 SpawnPoint { get; }

        public int MNumLives = 5;
        public bool MIsAlive = true;

        public int NumLives => MNumLives;

        //***Idle animation information***//
        private GravityDirections _mPreviousDirection;
        private int _mPreviousChange;
        public int PreviousChange
        {
            set { _mPreviousChange = value; }
        }

        private int _mCurrentTime;
        public int CurrentTime
        {
            set { _mCurrentTime = value; }
        }

        //Player rotation values for outer circle (current, goal, and speed)
        private float _mRotation;
        private float _mGoalRotation;
        private float _mRotationFactor;

        //Player rotation values for face (current, goal, and speed)
        private float _mFaceRotation;
        private float _mFaceGoalRotation;
        private readonly float _mFaceRotationFactor = (float)(Math.PI / 90.0f);

        //rotation goals for the 4 directions
        private readonly float _mRotationDown = 0.0f;

        private readonly float _mFaceRotationStraight = 0.0f;
        private readonly float _mFaceRotationRight = (float)(Math.PI / 8.0f);
        private readonly float _mFaceRotationLeft = (float)(-Math.PI / 8.0);

        public Texture2D MCurrentTexture;
        //public Texture2D mCurrentTexture1;

        //public Texture2D playerBase;

        private bool _mRumble;
        private double _elapsedTime;

        /// <summary>
        /// Construcs a player object, that can live in a physical realm
        /// </summary>
        /// <param name="content">Content manager for the game</param>
        /// <param name="name">Name of the image resource for the player</param>
        /// <param name="initialPosition">Initial posisition in the level</param>
        /// <param name="controlScheme">Controller scheme for the player(Controller or keyboard)</param>
        public Player(ContentManager content, ref PhysicsEnvironment environment, IControlScheme controlScheme, float friction, EntityInfo entity) 
            : base(content, ref environment,friction, entity)
        {
            _mControls = controlScheme;
            SpawnPoint = MPosition;
            _mRotation = 0.0f;
            _mGoalRotation = 0.0f;

            _mFaceGoalRotation = 0.0f;
            _mFaceRotation = 0.0f;

            Id = entity.MId;

            PlayerFaces.Load(content);

            MCurrentTexture = PlayerFaces.FromString("Smile");
            MSize = new Vector2(MCurrentTexture.Width, MCurrentTexture.Height);
            _mPreviousDirection = GravityDirections.Down;
        }

        /// <summary>
        /// Checks to see if the player is idle if it is, change his face
        /// </summary>
        public Texture2D CheckForIdle()
        {
            if (_mPreviousDirection != MEnvironment.GravityDirection)
            {
                _mPreviousDirection = MEnvironment.GravityDirection;
                _mPreviousChange = _mCurrentTime;
            }
            else
            {
                if ((_mCurrentTime - _mPreviousChange) > 7)
                {
                    return PlayerFaces.FromString("Sad");
                }

                if ((_mCurrentTime - _mPreviousChange) > 3)
                {
                    return PlayerFaces.FromString("Bored");
                }
            }

            return PlayerFaces.FromString("Smile");

        }

        public void ResetIdle(int mTimer, GravityDirections mDirection)
        {
            _mCurrentTime = mTimer;
            _mPreviousChange = mTimer;
            _mPreviousDirection = mDirection;
        }


        /// <summary>
        /// Updates the player location and the player controls
        /// </summary>
        /// <param name="gametime">The current Gametime</param>
        public override void Update(GameTime gametime)
        {
            base.Update(gametime);

            if (_mRumble)
                StopRumble();

            if (Math.Abs(MVelocity.X) >= 15 || Math.Abs(MVelocity.Y) >= 15)
                MCurrentTexture = PlayerFaces.FromString("Surprise");
            else if (!_mRumble)
                MCurrentTexture = CheckForIdle();

            //SHIFT: Down
            if ((_mControls.IsAPressed(false) || _mControls.IsDownPressed(false)) && MEnvironment.GravityDirection != GravityDirections.Down)
            {
                GameSound.LevelGravityShiftDown.Play(GameSound.Volume * 0.75f, 0.0f, 0.0f);
                MEnvironment.GravityDirection = GravityDirections.Down;
                _mGoalRotation = _mRotationDown;
                _mRotationFactor = 0.0f;
                SetFaceStraight();
            }

            //SHIFT: Up
            else if ((_mControls.IsYPressed(false) || _mControls.IsUpPressed(false)) && MEnvironment.GravityDirection != GravityDirections.Up)
            {
                GameSound.LevelGravityShiftUp.Play(GameSound.Volume * 0.75f, 0.0f, 0.0f);
                MEnvironment.GravityDirection = GravityDirections.Up;
                _mRotationFactor = 0.0f;
                SetFaceStraight();
            }

            //SHIFT: Left
            else if ((_mControls.IsXPressed(false) || _mControls.IsLeftPressed(false)) && MEnvironment.GravityDirection != GravityDirections.Left)
            {
                GameSound.LevelGravityShiftLeft.Play(GameSound.Volume * 0.75f, 0.0f, 0.0f);
                MEnvironment.GravityDirection = GravityDirections.Left;
                _mRotationFactor = (float)(-Math.PI / 60.0f);
                _mFaceGoalRotation = _mFaceRotationLeft;
            }

            //SHIFT: Right
            else if ((_mControls.IsBPressed(false) || _mControls.IsRightPressed(false)) && MEnvironment.GravityDirection != GravityDirections.Right)
            {
                GameSound.LevelGravityShiftRight.Play(GameSound.Volume * 0.75f, 0.0f, 0.0f);
                MEnvironment.GravityDirection = GravityDirections.Right;
                _mRotationFactor = (float)(Math.PI / 60.0f);
                _mFaceGoalRotation = _mFaceRotationRight;
            }

            if (Math.Abs(_mRotation) > 2.0 * Math.PI)
            {
                _mRotation -= (float)(2.0 * Math.PI);
            }
            else if (Math.Abs(_mRotation) < -2.0 * Math.PI)
            {
                _mRotation += (float)(2.0 * Math.PI);
            }

            _mRotation += _mRotationFactor;

            if (Math.Abs(_mFaceGoalRotation - _mFaceRotation) < 0.1)
            {
                _mFaceRotation = _mFaceGoalRotation;
            }
            else if (_mFaceRotation > _mFaceGoalRotation)
            {
                _mFaceRotation -= _mFaceRotationFactor;
            }
            else
            {
                _mFaceRotation += _mFaceRotationFactor;
            }

        }

        /// <summary>
        /// Draw the player, with rotation due to gravity taken into affect
        /// </summary>
        /// <param name="canvas">Canvas SpriteBatch</param>
        /// <param name="gametime">Current gametime</param>
        public override void Draw(SpriteBatch canvas, GameTime gametime)
        {
            canvas.Draw(MCurrentTexture, new Rectangle((int)MPosition.X + (int)(MSize.X / 2), (int)MPosition.Y + (int)(MSize.Y / 2), (int)MSize.X, (int)MSize.Y),
                new Rectangle(0, 0, MCurrentTexture.Width, MCurrentTexture.Height), Color.White, _mFaceRotation, new Vector2((MSize.X / 2), (MSize.Y / 2)), SpriteEffects.None, 0);
        }

        /// <summary>
        /// Handle players death 
        /// </summary>
        public override int Kill()
        {
            _mRumble = true;

            SetFaceStraight();
            MCurrentTexture = PlayerFaces.FromString("Dead2");

            StartRumble();

            // reset player to start position
            //this.mPosition = mSpawnPoint;

            // remove a life
            MNumLives--;
            if (MNumLives <= 0)
                MIsAlive = false;

            return MNumLives;
        }

        /// <summary>
        /// Sets the player face to straight
        /// </summary>
        public void SetFaceStraight()
        {
            _mFaceGoalRotation = _mFaceRotationStraight;
        }

        /// <summary>
        /// Checks to see if the face is straight
        /// </summary>
        /// <returns>True if the rotation is straight</returns>
        public bool IsFaceStraight()
        {
            return _mFaceGoalRotation == _mFaceRotationStraight;
        }

        /// <summary>
        /// Starts controller rumble
        /// </summary>
        public void StartRumble()
        {
            if (_mControls is ControllerControl)
            {
                GamePad.SetVibration(((ControllerControl)_mControls).ControllerIndex, .25f, .25f);
                _mRumble = true;
            }
        }

        /// <summary>
        /// Stop controller rumble
        /// </summary>
        public void StopRumble()
        {
            if(_mControls is ControllerControl)
            {
                GamePad.SetVibration(((ControllerControl)_mControls).ControllerIndex, 0.0f, 0.0f);
                
            }
            _mRumble = false;
        }

        /// <summary>
        /// Sets the controller to rumble for the amount of time passed
        /// </summary>
        /// <param name="time">The amount of time to rumble</param>
        /// <param name="gameTime">The current gameTime</param>
        public void Rumble(double time, GameTime gameTime)
        {
            var mTime = time;

            for (var i = 0; i < 4; i++)
            {
                var current = (PlayerIndex)Enum.ToObject(typeof(PlayerIndex), i);

                GamePad.SetVibration(current, 1.0f, 1.0f);

                if ((_elapsedTime += gameTime.ElapsedGameTime.TotalSeconds) >= mTime)
                {
                    _mRumble = false;
                    GamePad.SetVibration(current, 0.0f, 0.0f);
                    _elapsedTime = 0.0;
                    MCurrentTexture = PlayerFaces.FromString("Smile");
                }
            }
        }

        public override void Respawn()
        {
            base.Respawn();

            _mRotation = _mRotationDown;
            _mGoalRotation = _mRotation;
            MCurrentTexture = PlayerFaces.FromString("Smile");
        }

        /// <summary>
        /// Gets the position of the player
        /// </summary>
        /// <returns>A vector2 with the players position</returns>
        public Vector2 Position
        {
            get { return MPosition; }
            set { MPosition = value; }
        }

        /// <summary>
        /// Gets the Unique identifier for this object. Will be used for logging
        /// </summary>
        /// <returns>A string with the Object ID and the object type</returns>
        public override string ToString()
        {
            return  "Object ID:" + ObjectId + " Type: Player";
        }
    }
}
