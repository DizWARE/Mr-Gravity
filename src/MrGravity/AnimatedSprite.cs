using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MrGravity
{
    internal class AnimatedSprite
    {
        /* Number of frames the animation has */
        public int LastFrame { get; private set; }

        public int PreviousFrame;

        /* Frames Per Second */
        private float _mFps;

        /* The current frame to show */
        public int Frame { get; private set; }

        private Texture2D _mTexture;

        /* Elapsed time */
        private float _mElapsed;

        /// <summary>
        /// Loads function - self explanatory
        /// </summary>
        /// <param name="content">The current content manager</param>
        /// <param name="name">Name of the asset - assumes the animatedSprites folder</param>
        /// <param name="frameCount">number of frames</param>
        /// <param name="fps">Frames Per Second</param>
        public void Load(ContentManager content, string name, int frameCount, float fps)
        {
            LastFrame = frameCount;
            _mTexture = content.Load<Texture2D>("Images/AnimatedSprites/" + name);
            _mFps = fps;
            Frame = 0;
            _mElapsed = 0.0f;
        }

        /// <summary>
        /// Update function - self explanatory
        /// </summary>
        /// <param name="elapsed">elapsed time - if calling in MrGravityMain use (float)gameTime.ElapsedGameTime.TotalSeconds</param>
        public void Update(float elapsed)
        {
            _mElapsed += elapsed;

            /* If enough has passed, update the frame */
            if (_mElapsed > _mFps)
            {
                PreviousFrame = Frame;

                Frame++;

                Frame = Frame % LastFrame;
                _mElapsed -= _mFps;
            }
        }

        /// <summary>
        /// Draw function - self explanatory
        /// </summary>
        /// <param name="spriteBatch">the spritebatch to draw</param>
        /// <param name="position">where you want the texture to be drawn</param>
        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            var width = _mTexture.Width / LastFrame;
            var sourcerect = new Rectangle(width * Frame, 0, width, _mTexture.Height);
            spriteBatch.Draw(_mTexture, position, sourcerect, Color.White);
        }

        /// <summary>
        /// Reset function - Resets the frames and elapsed time to zero
        /// </summary>
        public void Reset()
        {
            Frame = 0;
            _mElapsed = 0.0f;
        }
    }
}
