using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MrGravity.Import_Code;
using MrGravity.MISC_Code;

namespace MrGravity.Game_Objects.Static_Objects
{
    internal class PlayerEnd : StaticObject
    {
        public double MTimer;
        public Texture2D MCurrentTexture;

        public PlayerEnd(ContentManager content,EntityInfo entity)
            : base(content, .8f, entity)
        {
                MIsSquare = false;
                PlayerFaces.Load(content);
                MCurrentTexture = PlayerFaces.FromString("GirlSmile");
                MTimer = 0.0;
        }

        public override void Draw(SpriteBatch canvas, GameTime gametime)
        {

            canvas.Draw(MCurrentTexture, new Rectangle((int)MPosition.X + (int)(MSize.X / 2), (int)MPosition.Y + (int)(MSize.Y / 2), (int)MSize.X, (int)MSize.Y),
                    new Rectangle(0, 0, MCurrentTexture.Width, MCurrentTexture.Height), Color.White, 0.0f, new Vector2((MSize.X / 2), (MSize.Y / 2)), SpriteEffects.None, 0);

            //canvas.Draw(mCurrentTexture, Vector2.Add(mPosition, new Vector2(mBoundingBox.Width / 2, mBoundingBox.Height / 2)), null, Color.White, mRotation, new Vector2(mBoundingBox.Width / 2, mBoundingBox.Height / 2), 1.0f, SpriteEffects.None, 0);
        }

        public void UpdateFace(double time)
        {
            MTimer = time;
            if (MTimer%4 < 2.5)// Eyes open for 3.5 seconds, .5 closed 
            {
                MCurrentTexture = PlayerFaces.FromString("GirlSmile2");
            }
            else
            {
                MCurrentTexture = PlayerFaces.FromString("GirlSmile");
            }
        }

    }
}
