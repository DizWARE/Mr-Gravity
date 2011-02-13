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
    class Title
    {
        //Title Image
        private Texture2D mTitle;
        private SpriteFont mQuartz;

        /* Title Safe Area */
        Rectangle mScreenRect;

        /* Controls */
        IControlScheme mControls;

        /// <summary>
        /// 
        /// </summary>
        public Title(IControlScheme controls) 
        {
            mControls = controls;
        }

        public void Load(ContentManager content, GraphicsDevice graphics)
        {
            mTitle = content.Load<Texture2D>("Images/Menu/Mr_Gravity");

            mQuartz = content.Load<SpriteFont>("Fonts/QuartzLarge");

            mScreenRect = graphics.Viewport.TitleSafeArea;
        }

        public void Update(GameTime gameTime, ref GameStates gameState)
        {
            if (mControls.isBackPressed(false))
                gameState = GameStates.Exit;
            if (mControls.isStartPressed(false) || mControls.isAPressed(false))
                gameState = GameStates.Main_Menu;

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

         
            spriteBatch.Draw(mTitle, new Vector2(mScreenRect.Left + (mScreenRect.Width - mTitle.Width) / 2, mScreenRect.Top), Color.White);

            string request = "Press Start Or A To Begin";

            Vector2 stringSize = mQuartz.MeasureString(request);

            spriteBatch.DrawString(mQuartz, request, new Vector2(mScreenRect.Center.X - (stringSize.X / 2), mScreenRect.Center.Y - (stringSize.Y / 2)), Color.SteelBlue);
            spriteBatch.DrawString(mQuartz, request, new Vector2(mScreenRect.Center.X - (stringSize.X / 2) + 2, mScreenRect.Center.Y - (stringSize.Y / 2) + 2), Color.White);

            spriteBatch.End();
        }




    }
}
