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
    //TODO:This will be used for our intial splash screen before the main menu
    class WorldPurchaseSplash
    {
        //Title Image
        private Texture2D mTitle;
        private Texture2D mBackground;
        private SpriteFont mQuartz;
        private GraphicsDeviceManager mGraphics;

        /* Title Safe Area */
        Rectangle mScreenRect;

        /* Controls */
        IControlScheme mControls;

        /// <summary>
        /// 
        /// </summary>
        public WorldPurchaseSplash(IControlScheme controls, GraphicsDeviceManager graphics)
        {
            mControls = controls;
            mGraphics = graphics;
        }

        public void Load(ContentManager content, GraphicsDevice graphics)
        {
            mTitle = content.Load<Texture2D>("Images/Menu/Mr_Gravity");
            mBackground = content.Load<Texture2D>("Images\\Menu\\backgroundSquares1");
            mQuartz = content.Load<SpriteFont>("Fonts/QuartzSmall");

            mScreenRect = graphics.Viewport.TitleSafeArea;
        }

        public void Update(GameTime gameTime, ref GameStates gameState)
        {
            if (mControls.isBPressed(false))
                gameState = GameStates.Level_Selection;
            if (mControls.isAPressed(false))
                gameState = GameStates.WorldPurchase;

        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime, Matrix scale)
        {
            spriteBatch.Begin(SpriteSortMode.Immediate,
                BlendState.AlphaBlend,
                SamplerState.LinearClamp,
                DepthStencilState.None,
                RasterizerState.CullCounterClockwise,
                null,
                scale);

            float[] mSize = new float[2] { (float)mScreenRect.Width / (float)mGraphics.GraphicsDevice.Viewport.Width, (float)mScreenRect.Height / (float)mGraphics.GraphicsDevice.Viewport.Height };

            spriteBatch.Draw(mBackground, new Rectangle(0, 0, mGraphics.GraphicsDevice.Viewport.Width, mGraphics.GraphicsDevice.Viewport.Height), Color.White);

            spriteBatch.Draw(mTitle, new Rectangle(mScreenRect.Center.X - (int)(mTitle.Width * mSize[0]) / 2, mScreenRect.Top, (int)(mTitle.Width * mSize[0]), (int)(mTitle.Height * mSize[1])), Color.White);

            string request = "Would you like to purchase the full version of the game?";
            string request2 = "(Requires a signed in XBOX Live profile)";
            string request3 = "Press A to bring up the Marketplace";
            string request4 = "Press B to return to the world select screen";

            Vector2 stringSize = mQuartz.MeasureString(request);
            Vector2 stringSize2 = mQuartz.MeasureString(request2);
            Vector2 stringSize3 = mQuartz.MeasureString(request3);
            Vector2 stringSize4 = mQuartz.MeasureString(request4);

            spriteBatch.DrawString(mQuartz, request, new Vector2(mScreenRect.Center.X - (stringSize.X / 2), mScreenRect.Center.Y - (stringSize.Y)), Color.SteelBlue);
            spriteBatch.DrawString(mQuartz, request, new Vector2(mScreenRect.Center.X - (stringSize.X / 2) + 2, mScreenRect.Center.Y - (stringSize.Y) + 2), Color.White);

            spriteBatch.DrawString(mQuartz, request2, new Vector2(mScreenRect.Center.X - (stringSize2.X / 2), mScreenRect.Center.Y), Color.SteelBlue);
            spriteBatch.DrawString(mQuartz, request2, new Vector2(mScreenRect.Center.X - (stringSize2.X / 2) + 2, mScreenRect.Center.Y + 2), Color.White);

            spriteBatch.DrawString(mQuartz, request3, new Vector2(mScreenRect.Center.X - (stringSize3.X / 2), mScreenRect.Center.Y + (stringSize3.Y)), Color.SteelBlue);
            spriteBatch.DrawString(mQuartz, request3, new Vector2(mScreenRect.Center.X - (stringSize3.X / 2) + 2, mScreenRect.Center.Y + (stringSize3.Y) + 2), Color.White);

            spriteBatch.DrawString(mQuartz, request4, new Vector2(mScreenRect.Center.X - (stringSize4.X / 2), mScreenRect.Center.Y + (2 * stringSize4.Y)), Color.SteelBlue);
            spriteBatch.DrawString(mQuartz, request4, new Vector2(mScreenRect.Center.X - (stringSize4.X / 2) + 2, mScreenRect.Center.Y + (2 * stringSize4.Y) + 2), Color.White);
            spriteBatch.End();
        }




    }
}
