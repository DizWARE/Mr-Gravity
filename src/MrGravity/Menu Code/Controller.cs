using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MrGravity.MISC_Code;

namespace MrGravity.Menu_Code
{
    internal class Controller
    {
        private Texture2D _mTitle;
        private Texture2D _mBack;
        private Texture2D _mBackground;
        private Texture2D _mXboxControl;

        private readonly IControlScheme _mControls;
        private readonly GraphicsDeviceManager _mGraphics;

        /// <summary>
        /// Constructor for controller screen
        /// </summary>
        /// <param name="controlScheme">Player's method of control</param>
        /// <param name="graphics">Graphics manager for the game</param>
        public Controller(IControlScheme controlScheme, GraphicsDeviceManager graphics)
        {
            _mControls = controlScheme;
            _mGraphics = graphics;

        }

        /// <summary>
        /// Loads images needed for this screen
        /// </summary>
        /// <param name="content">Content manager used for this game</param>
        public void Load(ContentManager content)
        {
            _mTitle = content.Load<Texture2D>("Images\\Menu\\Mr_Gravity");
            _mBack = content.Load<Texture2D>("Images\\Menu\\Main\\BackSelected");
            _mBackground = content.Load<Texture2D>("Images\\Menu\\backgroundSquares1");
            _mXboxControl = content.Load<Texture2D>("Images/Menu/Main/XboxController");
        }

        /// <summary>
        /// Update process of this screen.  Responds to player input to go back to the options screen.
        /// </summary>
        /// <param name="gametime">Current gameTime</param>
        /// <param name="states">Current state of the game</param>
        public void Update(GameTime gametime, ref GameStates states)
        {
            if (_mControls.IsAPressed(false) || _mControls.IsStartPressed(false) || _mControls.IsBackPressed(false) || _mControls.IsBPressed(false))
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

            Rectangle mScreenRect = _mGraphics.GraphicsDevice.Viewport.TitleSafeArea;

            var mSize = new float[2] { mScreenRect.Width / (float)_mGraphics.GraphicsDevice.Viewport.Width, mScreenRect.Height / (float)_mGraphics.GraphicsDevice.Viewport.Height };

            spriteBatch.Draw(_mBackground, new Rectangle(0, 0, _mGraphics.GraphicsDevice.Viewport.Width, _mGraphics.GraphicsDevice.Viewport.Height), Color.White);

            spriteBatch.Draw(_mTitle, new Rectangle(mScreenRect.Center.X - (int)(_mTitle.Width * mSize[0]) / 2, mScreenRect.Top, (int)(_mTitle.Width * mSize[0]), (int)(_mTitle.Height * mSize[1])), Color.White);

            //float[] mSize = new float[2]{ (float)mScreenRect.Width / (float)mGraphics.GraphicsDevice.Viewport.Width, (float)mScreenRect.Height / (float)mGraphics.GraphicsDevice.Viewport.Height };
            spriteBatch.Draw(_mXboxControl, new Rectangle(mScreenRect.Center.X - (int)(_mXboxControl.Width * mSize[0]) / 2, mScreenRect.Center.Y - (int)(_mXboxControl.Height * mSize[1]) / 2, (int)(_mXboxControl.Width * mSize[0]), (int)(_mXboxControl.Height * mSize[1])), Color.White);
            //spriteBatch.Draw(mXboxControl, new Vector2(mScreenRect.Center.X - mXboxControl.Width / 2, mScreenRect.Center.Y - mXboxControl.Height / 3), Color.White);
            spriteBatch.Draw(_mBack, new Rectangle(mScreenRect.Center.X - (int)(_mBack.Width * mSize[0]) / 2, mScreenRect.Bottom - (int)(_mBack.Height * mSize[1]), (int)(_mBack.Width * mSize[0]), (int)(_mBack.Height * mSize[1])), Color.White);

            spriteBatch.End();
        }
    }
}
