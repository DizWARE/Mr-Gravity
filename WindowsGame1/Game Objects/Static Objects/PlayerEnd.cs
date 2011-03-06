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
    class PlayerEnd : StaticObject
    {
        public double mTimer;
        public Texture2D mCurrentTexture;

        public PlayerEnd(ContentManager content,EntityInfo entity)
            : base(content, .8f, entity)
        {
                mIsSquare = false;
                PlayerFaces.Load(content);
                mCurrentTexture = PlayerFaces.FromString("GirlSmile");
                mTimer = 0.0;
        }

        public override void Draw(SpriteBatch canvas, GameTime gametime)
        {

            canvas.Draw(mCurrentTexture, new Rectangle((int)mPosition.X + (int)(mSize.X / 2), (int)mPosition.Y + (int)(mSize.Y / 2), (int)mSize.X, (int)mSize.Y),
                    new Rectangle(0, 0, (int)mCurrentTexture.Width, (int)mCurrentTexture.Height), Color.White, 0.0f, new Vector2((mSize.X / 2), (mSize.Y / 2)), SpriteEffects.None, 0);

            //canvas.Draw(mCurrentTexture, Vector2.Add(mPosition, new Vector2(mBoundingBox.Width / 2, mBoundingBox.Height / 2)), null, Color.White, mRotation, new Vector2(mBoundingBox.Width / 2, mBoundingBox.Height / 2), 1.0f, SpriteEffects.None, 0);
        }

        public void UpdateFace(double time)
        {
            mTimer = time;
            if (mTimer%4 < 3.5)// Eyes open for 3.5 seconds, .5 closed (looks like a blink)
            {
                mCurrentTexture = PlayerFaces.FromString("GirlSmile");
            }
            else
            {
                mCurrentTexture = PlayerFaces.FromString("GirlCalm");
            }
        }

    }
}
