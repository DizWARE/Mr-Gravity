using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MrGravity.MISC_Code;

namespace MrGravity.Menu_Code
{
    //TODO:This will be used for our intial splash screen before the main menu
    internal class PurchaseScreenSplash
    {
        //Title Image
        private Texture2D _mTitle;
        private Texture2D _mBackground;
        private SpriteFont _mQuartz;
        private readonly GraphicsDeviceManager _mGraphics;

        /* Title Safe Area */
        private Rectangle _mScreenRect;

        /* Controls */
        private readonly IControlScheme _mControls;

        /// <summary>
        /// 
        /// </summary>
        public PurchaseScreenSplash(IControlScheme controls, GraphicsDeviceManager graphics)
        {
            _mControls = controls;
            _mGraphics = graphics;
        }

        public void Load(ContentManager content, GraphicsDevice graphics)
        {
            _mTitle = content.Load<Texture2D>("Images/Menu/Mr_Gravity");
            _mBackground = content.Load<Texture2D>("Images\\Menu\\backgroundSquares1");
            _mQuartz = content.Load<SpriteFont>("Fonts/QuartzSmall");

            _mScreenRect = graphics.Viewport.TitleSafeArea;
        }

        public void Update(GameTime gameTime, ref GameStates gameState)
        {
            if (_mControls.IsBPressed(false))
                gameState = GameStates.WaitingToExit;
            if (_mControls.IsAPressed(false))
                gameState = GameStates.TrialExit;

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

            var mSize = new float[2] { _mScreenRect.Width / (float)_mGraphics.GraphicsDevice.Viewport.Width, _mScreenRect.Height / (float)_mGraphics.GraphicsDevice.Viewport.Height };

            spriteBatch.Draw(_mBackground, new Rectangle(0, 0, _mGraphics.GraphicsDevice.Viewport.Width, _mGraphics.GraphicsDevice.Viewport.Height), Color.White);

            spriteBatch.Draw(_mTitle, new Rectangle(_mScreenRect.Center.X - (int)(_mTitle.Width * mSize[0]) / 2, _mScreenRect.Top, (int)(_mTitle.Width * mSize[0]), (int)(_mTitle.Height * mSize[1])), Color.White);

            var request = "Would you like to purchase the full version of the game?";
            var request2 = "(Requires a signed in XBOX Live profile)";
            var request3 = "Press A to bring up the Marketplace";
            var request4 = "Press B to exit without purchasing the full version";

            Vector2 stringSize = _mQuartz.MeasureString(request);
            Vector2 stringSize2 = _mQuartz.MeasureString(request2);
            Vector2 stringSize3 = _mQuartz.MeasureString(request3);
            Vector2 stringSize4 = _mQuartz.MeasureString(request4);

            spriteBatch.DrawString(_mQuartz, request, new Vector2(_mScreenRect.Center.X - (stringSize.X / 2), _mScreenRect.Center.Y - (stringSize.Y)), Color.SteelBlue);
            spriteBatch.DrawString(_mQuartz, request, new Vector2(_mScreenRect.Center.X - (stringSize.X / 2) + 2, _mScreenRect.Center.Y - (stringSize.Y) + 2), Color.White);

            spriteBatch.DrawString(_mQuartz, request2, new Vector2(_mScreenRect.Center.X - (stringSize2.X / 2), _mScreenRect.Center.Y), Color.SteelBlue);
            spriteBatch.DrawString(_mQuartz, request2, new Vector2(_mScreenRect.Center.X - (stringSize2.X / 2) + 2, _mScreenRect.Center.Y + 2), Color.White);

            spriteBatch.DrawString(_mQuartz, request3, new Vector2(_mScreenRect.Center.X - (stringSize3.X / 2), _mScreenRect.Center.Y + (stringSize3.Y)), Color.SteelBlue);
            spriteBatch.DrawString(_mQuartz, request3, new Vector2(_mScreenRect.Center.X - (stringSize3.X / 2) + 2, _mScreenRect.Center.Y + (stringSize3.Y) + 2), Color.White);

            spriteBatch.DrawString(_mQuartz, request4, new Vector2(_mScreenRect.Center.X - (stringSize4.X / 2), _mScreenRect.Center.Y + (2 * stringSize4.Y)), Color.SteelBlue);
            spriteBatch.DrawString(_mQuartz, request4, new Vector2(_mScreenRect.Center.X - (stringSize4.X / 2) + 2, _mScreenRect.Center.Y + (2 * stringSize4.Y) + 2), Color.White);
            spriteBatch.End();
        }




    }
}
