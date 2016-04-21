using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MrGravity.Import_Code;

namespace MrGravity.Game_Objects.Physics_Objects
{
    /// <summary>
    /// Moving tile
    /// </summary>
    internal class MovingTile : PhysicsObject
    {
        public bool BeingAnimated { get; private set; }

        private AnimatedSprite _mAnimationTexture;

        /// <summary>
        /// Constructs a tile that is capable of moving around the screen
        /// </summary>
        /// <param name="content">The games content manager</param>
        /// <param name="name">Name of the Object("Images/{Type}/{Name}"</param>
        /// <param name="initialPosition">Starting position</param>
        /// <param name="friction">Friction that reacts to physics objects</param>
        /// <param name="isSquare">True if the object should behave like a square</param>
        /// <param name="isHazardous">True if the object should kill the player when touched</param>
        public MovingTile(ContentManager content, ref PhysicsEnvironment environment, float friction, EntityInfo entity) :
            base(content, ref environment, friction, entity)
        {
            BeingAnimated = false;
            _mAnimationTexture = new AnimatedSprite();
        }

        public void StartAnimation(AnimatedSprite sprite)
        {
            _mAnimationTexture = sprite;
            BeingAnimated = true;
        }

        public override void Update(GameTime gametime)
        {
            base.Update(gametime);
            if (BeingAnimated)
            {
                _mAnimationTexture.Update((float)gametime.ElapsedGameTime.TotalSeconds);
                if (_mAnimationTexture.Frame == 0 && _mAnimationTexture.PreviousFrame == _mAnimationTexture.LastFrame - 1)
                    BeingAnimated = false;
            }
        }

        public override void Draw(SpriteBatch canvas, GameTime gametime)
        {
            base.Draw(canvas, gametime);
            if (BeingAnimated)
                _mAnimationTexture.Draw(canvas, MPosition);
        }

        public override int Kill()
        {
            // probably not needed
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            throw new NotImplementedException();
        }
    }
}
