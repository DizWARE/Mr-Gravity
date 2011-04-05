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
    class PreScore
    {
        #region Member Variables

        private Rectangle mScreenRect;

        ContentManager mContent;

        IControlScheme mControls;

        #endregion

        #region Art

        private Texture2D mTrans;

        #endregion

        public PreScore(IControlScheme controls)
        {
            mControls = controls;
        }

        /*
         * Load
         *
         * Similar to a loadContent function. This function loads and 
         * initializes the variable and art used in the class.
         *
         * ContentManager content: the Content file used in the game.
         */
        public void Load(ContentManager content, GraphicsDevice graphics)
        {
            mContent = content;

            mScreenRect = graphics.Viewport.TitleSafeArea;

            mTrans = content.Load<Texture2D>("Images/Menu/Pause/PausedTrans");
        }

        /*
         * Update
         *
         * Updates the menu depending on what the user has selected.
         * It will handle the title, options, load and all other menu 
         * screens
         *
         * GameTime gameTime: The current game time variable
         */
        public void Update(GameTime gameTime, ref GameStates gameState, ref Level level)
        {
            /* If the user selects one of the menu items */
            if (mControls.isAPressed(false) || mControls.isStartPressed(false))
            {
                level.mTimer = 0;
                GameSound.menuSound_select.Play(GameSound.volume, 0.0f, 0.0f);

                /*Back To Level Selection*/
                gameState = GameStates.Score;
            }
        }

        /*
         * Draw
         *
         * This function will draw the current menu
         *
         * SpriteBatch spriteBatch: The current sprite batch used to draw
         * 
         * GraphicsDeviceManager graphics: The current graphics manager
         */
        public void Draw(SpriteBatch spriteBatch, GraphicsDeviceManager graphics, Matrix scale)
        {
            spriteBatch.Begin(SpriteSortMode.Immediate,
                BlendState.AlphaBlend,
                SamplerState.LinearClamp,
                DepthStencilState.None,
                RasterizerState.CullCounterClockwise,
                null,
                scale);

            float[] mSize = new float[2] { (float)mScreenRect.Width / (float)graphics.GraphicsDevice.Viewport.Width, (float)mScreenRect.Height / (float)graphics.GraphicsDevice.Viewport.Height };

            spriteBatch.Draw(mTrans, new Rectangle(mScreenRect.Left, mScreenRect.Top, mScreenRect.Width, mScreenRect.Height), Color.White);

            spriteBatch.End();
        }
    }
}
