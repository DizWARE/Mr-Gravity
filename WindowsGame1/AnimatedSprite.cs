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
using System.Text;

namespace GravityShift
{
    class AnimatedSprite
    {
        private int mFrameCount;
        private float mFPS;
        private int frame;
        private Texture2D mTexture;
        private float mElapsed;

        public AnimatedSprite() { }

        public void Load(ContentManager content, string name, int frameCount, float FPS)
        {
            mFrameCount = frameCount;
            mTexture = content.Load<Texture2D>("animatedSprites/" + name);
            mFPS = FPS;
            frame = 0;
            mElapsed = 0.0f;
        }

        public void Update(float elapsed)
        {
            mElapsed += elapsed;
            if (mElapsed > mFPS)
            {
                frame++;

                frame = frame % mFrameCount;
                mElapsed -= mFPS;
            }
        }

        public void Draw(SpriteBatch mSpriteBatch, Vector2 position)
        {
            int width = mTexture.Width / mFrameCount;
            Rectangle sourcerect = new Rectangle(width * frame, 0, width, mTexture.Height);
            mSpriteBatch.Draw(mTexture, position, sourcerect, Color.White);
        }

        public void Reset()
        {
            frame = 0;
            mElapsed = 0.0f;
        }
    }
}
