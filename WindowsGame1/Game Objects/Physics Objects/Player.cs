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

namespace GravityShift
{
    /// <summary>
    /// Represents the player in the game
    /// </summary>
    class Player : PhysicsObject
    {
        IControlScheme mControls;
        Vector2 mSpawnPoint;
        public int mNumLives = 5;
        public int mScore = 0;
        public bool mIsAlive = true;

        //Player rotation values (current, goal, and speed)
        private float mRotation;
        private float mGoalRotation;
        private float mRotationFactor = (float)(Math.PI / 60.0f);

        //rotation goals for the 4 directions
        private float mRotationDown = 0.0f;
        private float mRotationRight = (float)(3.0 * Math.PI / 2.0f);
        private float mRotationUp = (float)Math.PI;
        private float mRotationLeft = (float)(Math.PI / 2.0);
        
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
            ID = entity.mId;
        }
        /// <summary>
        /// Updates the player location and the player controls
        /// </summary>
        /// <param name="gametime">The current Gametime</param>
        public override void Update(GameTime gametime)
        {
            base.Update(gametime);

            //SHIFT: Down
            if (mControls.isDownPressed(false) && mEnvironment.GravityDirection != GravityDirections.Down)
            {
                GameSound.level_gravityShiftDown.Play(GameSound.volume * 0.75f, 0.0f, 0.0f);
                mEnvironment.GravityDirection = GravityDirections.Down;
                mGoalRotation = mRotationDown;
            }

            //SHIFT: Up
            else if (mControls.isUpPressed(false) && mEnvironment.GravityDirection != GravityDirections.Up)
            {
                GameSound.level_gravityShiftUp.Play(GameSound.volume * 0.75f, 0.0f, 0.0f);
                mEnvironment.GravityDirection = GravityDirections.Up;
                mGoalRotation = mRotationUp;
            }

            //SHIFT: Left
            else if (mControls.isLeftPressed(false) && mEnvironment.GravityDirection != GravityDirections.Left)
            {
                GameSound.level_gravityShiftLeft.Play(GameSound.volume * 0.75f, 0.0f, 0.0f);
                mEnvironment.GravityDirection = GravityDirections.Left;
                mGoalRotation = mRotationLeft;
            }

            //SHIFT: Right
            else if (mControls.isRightPressed(false) && mEnvironment.GravityDirection != GravityDirections.Right)
            {
                GameSound.level_gravityShiftRight.Play(GameSound.volume * 0.75f, 0.0f, 0.0f);
                mEnvironment.GravityDirection = GravityDirections.Right;
                mGoalRotation = mRotationRight;
            }

            if (Math.Abs(mGoalRotation - mRotation) < 0.1)
            {
                mRotation = mGoalRotation;
            }
            else if (mRotation > mGoalRotation)
            {
                mRotation -= mRotationFactor;
            }
            else
            {
                mRotation += mRotationFactor;
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
            canvas.Draw(mTexture, new Rectangle((int)mPosition.X + (int)(mSize.X / 2), (int)mPosition.Y + (int)(mSize.Y / 2), (int)mSize.X, (int)mSize.Y), 
                new Rectangle(0, 0, (int)mSize.X, (int)mSize.Y), Color.White, 0.0f, new Vector2((mSize.X / 2), (mSize.Y / 2)), SpriteEffects.None, 0);
            //canvas.Draw(mTexture, mPosition, null, Color.White, mRotation, new Vector2(mTexture.Width / 2, mTexture.Height / 2), 1.0f, SpriteEffects.None, 0);
        }

        /// <summary>
        /// Handle players death 
        /// </summary>
        public override int Kill()
        {
            // reset player to start position
            this.mPosition = mSpawnPoint;
            // remove a life
            mNumLives--;
            if (mNumLives <= 0)
                mIsAlive = false;

            return mNumLives;
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
