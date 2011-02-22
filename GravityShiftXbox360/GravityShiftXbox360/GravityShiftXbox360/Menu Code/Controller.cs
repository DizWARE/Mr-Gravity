using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace GravityShift
{
    class Controller
    {
        Texture2D mTitle;
        Texture2D mBack;
        Texture2D mBackground;
        Texture2D mXboxControl;

        IControlScheme mControls;
        GraphicsDeviceManager mGraphics;

        /// <summary>
        /// Constructor for controller screen
        /// </summary>
        /// <param name="controlScheme">Player's method of control</param>
        /// <param name="graphics">Graphics manager for the game</param>
        public Controller(IControlScheme controlScheme, GraphicsDeviceManager graphics)
        {
            mControls = controlScheme;
            mGraphics = graphics;

        }

        /// <summary>
        /// Loads images needed for this screen
        /// </summary>
        /// <param name="content">Content manager used for this game</param>
        public void Load(ContentManager content)
        {
            mTitle = content.Load<Texture2D>("Images\\Menu\\Mr_Gravity");
            mBack = content.Load<Texture2D>("Images\\Menu\\Main\\BackSelected");
            mBackground = content.Load<Texture2D>("Images\\Menu\\backgroundSquares1");
            mXboxControl = content.Load<Texture2D>("Images/Menu/Main/XboxController");
        }

        /// <summary>
        /// Update process of this screen.  Responds to player input to go back to the options screen.
        /// </summary>
        /// <param name="gametime">Current gameTime</param>
        /// <param name="states">Current state of the game</param>
        public void Update(GameTime gametime, ref GameStates states)
        {
            if (mControls.isAPressed(false) || mControls.isStartPressed(false) || mControls.isBackPressed(false) || mControls.isBPressed(false))
            {
                states = GameStates.Options;
            }

        }

        /// <summary>
        /// Draws the controller screen
        /// </summary>
        /// <param name="gametime">Current gametime</param>
        /// <param name="spriteBatch">Canvas the screen is drawing on</param>
        /// <param name="scale">scale factor</param>
        public void Draw(GameTime gametime, SpriteBatch spriteBatch, Matrix scale)
        {
            spriteBatch.Begin(SpriteSortMode.Immediate,
                BlendState.AlphaBlend,
                SamplerState.LinearClamp,
                DepthStencilState.None,
                RasterizerState.CullCounterClockwise,
                null,
                scale);

            Rectangle mScreenRect = mGraphics.GraphicsDevice.Viewport.TitleSafeArea;

            float[] mSize = new float[2] { (float)mScreenRect.Width / (float)mGraphics.GraphicsDevice.Viewport.Width, (float)mScreenRect.Height / (float)mGraphics.GraphicsDevice.Viewport.Height };

            spriteBatch.Draw(mBackground, new Rectangle(0, 0, mGraphics.GraphicsDevice.Viewport.Width, mGraphics.GraphicsDevice.Viewport.Height), Color.White);

            spriteBatch.Draw(mTitle, new Rectangle(mScreenRect.Center.X - (int)(mTitle.Width * mSize[0]) / 2, mScreenRect.Top, (int)(mTitle.Width * mSize[0]), (int)(mTitle.Height * mSize[1])), Color.White);

            //float[] mSize = new float[2]{ (float)mScreenRect.Width / (float)mGraphics.GraphicsDevice.Viewport.Width, (float)mScreenRect.Height / (float)mGraphics.GraphicsDevice.Viewport.Height };
            spriteBatch.Draw(mXboxControl, new Rectangle(mScreenRect.Center.X - (int)(mXboxControl.Width * mSize[0]) / 2, mScreenRect.Center.Y - (int)(mXboxControl.Height * mSize[1]) / 2, (int)(mXboxControl.Width * mSize[0]), (int)(mXboxControl.Height * mSize[1])), Color.White);
            //spriteBatch.Draw(mXboxControl, new Vector2(mScreenRect.Center.X - mXboxControl.Width / 2, mScreenRect.Center.Y - mXboxControl.Height / 3), Color.White);
            spriteBatch.Draw(mBack, new Rectangle(mScreenRect.Center.X - (int)(mBack.Width * mSize[0]) / 2, mScreenRect.Bottom - (int)(mBack.Height * mSize[1]), (int)(mBack.Width * mSize[0]), (int)(mBack.Height * mSize[1])), Color.White);

            spriteBatch.End();
        }
    }
}
