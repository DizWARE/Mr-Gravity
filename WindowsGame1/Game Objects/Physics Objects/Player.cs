using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using GravityShift.MISC_Code;

namespace GravityShift
{
    /// <summary>
    /// Represents the player in the game
    /// </summary>
    class Player : PhysicsObject
    {
        IControlScheme mControls;

        Vector2 mSpawnPoint;
        public Vector2 SpawnPoint
        {   get { return mSpawnPoint; } }

        public int mNumLives = 5;
        public bool mIsAlive = true;

        public int NumLives
        {
            get { return mNumLives; }
        }

        //***Idle animation information***//
        private GravityDirections mPreviousDirection;
        private int mPreviousChange;
        public int PreviousChange
        {
            set { mPreviousChange = value; }
        }

        private int mCurrentTime;
        public int CurrentTime
        {
            set { mCurrentTime = value; }
        }

        //Player rotation values for outer circle (current, goal, and speed)
        private float mRotation;
        private float mGoalRotation;
        private float mRotationFactor = 0.0f;

        //Player rotation values for face (current, goal, and speed)
        private float mFaceRotation;
        private float mFaceGoalRotation;
        private float mFaceRotationFactor = (float)(Math.PI / 90.0f);

        //rotation goals for the 4 directions
        private float mRotationDown = 0.0f;

        private float mFaceRotationStraight = 0.0f;
        private float mFaceRotationRight = (float)(Math.PI / 8.0f);
        private float mFaceRotationLeft = (float)(-Math.PI / 8.0);

        public Texture2D mCurrentTexture;
        //public Texture2D mCurrentTexture1;

        //public Texture2D playerBase;

        private bool mRumble = false;
        private double elapsedTime = 0.0;

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
            mControls = controlScheme;
            mSpawnPoint = mPosition;
            mRotation = 0.0f;
            mGoalRotation = 0.0f;

            mFaceGoalRotation = 0.0f;
            mFaceRotation = 0.0f;

            ID = entity.mId;

            PlayerFaces.Load(content);

            mCurrentTexture = PlayerFaces.FromString("Smile");
            mSize = new Vector2(mCurrentTexture.Width, mCurrentTexture.Height);
            mPreviousDirection = GravityDirections.Down;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        public Texture2D CheckForIdle()
        {
            if (mPreviousDirection != mEnvironment.GravityDirection)
            {
                mPreviousDirection = mEnvironment.GravityDirection;
                mPreviousChange = mCurrentTime;
            }
            else
            {
                if ((mCurrentTime - mPreviousChange) > 7)
                {
                    return PlayerFaces.FromString("Sad");
                }

                if ((mCurrentTime - mPreviousChange) > 3)
                {
                    return PlayerFaces.FromString("Bored");
                }
            }

            return PlayerFaces.FromString("Smile");

        }

        public void ResetIdle(int mTimer, GravityDirections mDirection)
        {
            mCurrentTime = mTimer;
            mPreviousChange = mTimer;
            mPreviousDirection = mDirection;
        }


        /// <summary>
        /// Updates the player location and the player controls
        /// </summary>
        /// <param name="gametime">The current Gametime</param>
        public override void Update(GameTime gametime)
        {
            base.Update(gametime);

            if (mRumble)
                StopRumble();

            if (Math.Abs(mVelocity.X) >= 15 || Math.Abs(mVelocity.Y) >= 15)
                mCurrentTexture = PlayerFaces.FromString("Surprise");
            else if (!mRumble)
                mCurrentTexture = CheckForIdle();

            //SHIFT: Down
            if ((mControls.isAPressed(false) || mControls.isDownPressed(false)) && mEnvironment.GravityDirection != GravityDirections.Down)
            {
                GameSound.level_gravityShiftDown.Play(GameSound.volume * 0.75f, 0.0f, 0.0f);
                mEnvironment.GravityDirection = GravityDirections.Down;
                mGoalRotation = mRotationDown;
                mRotationFactor = 0.0f;
                setFaceStraight();
            }

            //SHIFT: Up
            else if ((mControls.isYPressed(false) || mControls.isUpPressed(false)) && mEnvironment.GravityDirection != GravityDirections.Up)
            {
                GameSound.level_gravityShiftUp.Play(GameSound.volume * 0.75f, 0.0f, 0.0f);
                mEnvironment.GravityDirection = GravityDirections.Up;
                mRotationFactor = 0.0f;
                setFaceStraight();
            }

            //SHIFT: Left
            else if ((mControls.isXPressed(false) || mControls.isLeftPressed(false)) && mEnvironment.GravityDirection != GravityDirections.Left)
            {
                GameSound.level_gravityShiftLeft.Play(GameSound.volume * 0.75f, 0.0f, 0.0f);
                mEnvironment.GravityDirection = GravityDirections.Left;
                mRotationFactor = (float)(-Math.PI / 60.0f);
                mFaceGoalRotation = mFaceRotationLeft;
            }

            //SHIFT: Right
            else if ((mControls.isBPressed(false) || mControls.isRightPressed(false)) && mEnvironment.GravityDirection != GravityDirections.Right)
            {
                GameSound.level_gravityShiftRight.Play(GameSound.volume * 0.75f, 0.0f, 0.0f);
                mEnvironment.GravityDirection = GravityDirections.Right;
                mRotationFactor = (float)(Math.PI / 60.0f);
                mFaceGoalRotation = mFaceRotationRight;
            }

            if (Math.Abs(mRotation) > 2.0 * Math.PI)
            {
                mRotation -= (float)(2.0 * Math.PI);
            }
            else if (Math.Abs(mRotation) < -2.0 * Math.PI)
            {
                mRotation += (float)(2.0 * Math.PI);
            }

            mRotation += mRotationFactor;

            if (Math.Abs(mFaceGoalRotation - mFaceRotation) < 0.1)
            {
                mFaceRotation = mFaceGoalRotation;
            }
            else if (mFaceRotation > mFaceGoalRotation)
            {
                mFaceRotation -= mFaceRotationFactor;
            }
            else
            {
                mFaceRotation += mFaceRotationFactor;
            }

        }

        /// <summary>
        /// Draw the player, with rotation due to gravity taken into affect
        /// </summary>
        /// <param name="canvas">Canvas SpriteBatch</param>
        /// <param name="gametime">Current gametime</param>
        public override void Draw(SpriteBatch canvas, GameTime gametime)
        {
            //TODO: put rotation back in later
//            canvas.Draw(playerBase, new Rectangle((int)mPosition.X + (int)(mSize.X / 2), (int)mPosition.Y + (int)(mSize.Y / 2), (int)mSize.X, (int)mSize.Y),
//                new Rectangle(0, 0, (int)mCurrentTexture1.Width, (int)mCurrentTexture1.Height), Color.White, mRotation, new Vector2((mSize.X / 2), (mSize.Y / 2)), SpriteEffects.None, 0);
//            canvas.Draw(mCurrentTexture1, new Rectangle((int)mPosition.X + (int)(mSize.X / 2), (int)mPosition.Y + (int)(mSize.Y / 2), (int)mSize.X, (int)mSize.Y),
//                new Rectangle(0, 0, (int)mCurrentTexture1.Width, (int)mCurrentTexture1.Height), Color.White, mFaceRotation, new Vector2((mSize.X / 2), (mSize.Y / 2)), SpriteEffects.None, 0);
//            canvas.Draw(playerBase, new Rectangle((int)mPosition.X + (int)(mSize.X / 2), (int)mPosition.Y + (int)(mSize.Y / 2), (int)mSize.X, (int)mSize.Y),
//                new Rectangle(0, 0, (int)mCurrentTexture.Width, (int)mCurrentTexture.Height), Color.White, mRotation, new Vector2((mSize.X / 2), (mSize.Y / 2)), SpriteEffects.None, 0);

            canvas.Draw(mCurrentTexture, new Rectangle((int)mPosition.X + (int)(mSize.X / 2), (int)mPosition.Y + (int)(mSize.Y / 2), (int)mSize.X, (int)mSize.Y),
                new Rectangle(0, 0, (int)mCurrentTexture.Width, (int)mCurrentTexture.Height), Color.White, mFaceRotation, new Vector2((mSize.X / 2), (mSize.Y / 2)), SpriteEffects.None, 0);

            //canvas.Draw(mTexture, Vector2.Add(mPosition, new Vector2(mBoundingBox.Width / 2, mBoundingBox.Height / 2)), null, Color.White, mRotation, new Vector2(mBoundingBox.Width / 2, mBoundingBox.Height / 2), 1.0f, SpriteEffects.None, 0);
        }

        /// <summary>
        /// Handle players death 
        /// </summary>
        public override int Kill()
        {
            mRumble = true;

            setFaceStraight();
            mCurrentTexture = PlayerFaces.FromString("Dead2");

            StartRumble();

            // reset player to start position
            //this.mPosition = mSpawnPoint;

            // remove a life
            mNumLives--;
            if (mNumLives <= 0)
                mIsAlive = false;

            return mNumLives;
        }

        public void setFaceStraight()
        {
            mFaceGoalRotation = mFaceRotationStraight;
        }

        public bool isFaceStraight()
        {
            return mFaceGoalRotation == mFaceRotationStraight;
        }

        public void StartRumble()
        {
            mRumble = true;
            for (int i = 0; i < 4; i++)
            {
                PlayerIndex current = (PlayerIndex)Enum.ToObject(typeof(PlayerIndex), i);

                GamePad.SetVibration(current, .25f, .25f);
            }
        }
        public void StopRumble()
        {
            for (int i = 0; i < 4; i++)
            {
                PlayerIndex current = (PlayerIndex)Enum.ToObject(typeof(PlayerIndex), i);
                mRumble = false;
                GamePad.SetVibration(current, 0.0f, 0.0f);
            }
        }

        /// <summary>
        /// Sets the controller to rumble for the amount of time passed
        /// </summary>
        /// <param name="time">The amount of time to rumble</param>
        /// <param name="gameTime">The current gameTime</param>
        public void rumble(double time, GameTime gameTime)
        {
            double mTime = time;

            for (int i = 0; i < 4; i++)
            {
                PlayerIndex current = (PlayerIndex)Enum.ToObject(typeof(PlayerIndex), i);

                GamePad.SetVibration(current, 1.0f, 1.0f);

                if ((elapsedTime += gameTime.ElapsedGameTime.TotalSeconds) >= mTime)
                {
                    mRumble = false;
                    GamePad.SetVibration(current, 0.0f, 0.0f);
                    elapsedTime = 0.0;
                    mCurrentTexture = PlayerFaces.FromString("Smile");
                }
            }
        }

        public override void Respawn()
        {
            base.Respawn();

            mRotation = mRotationDown;
            mGoalRotation = mRotation;
            mCurrentTexture = PlayerFaces.FromString("Smile");
        }

        /// <summary>
        /// Gets the position of the player
        /// </summary>
        /// <returns>A vector2 with the players position</returns>
        public Vector2 Position
        {
            get { return this.mPosition; }
            set { this.mPosition = value; }
        }

        /// <summary>
        /// Gets the Unique identifier for this object. Will be used for logging
        /// </summary>
        /// <returns>A string with the Object ID and the object type</returns>
        public override string ToString()
        {
            return  "Object ID:" + this.ObjectID + " Type: Player";
        }
    }
}
