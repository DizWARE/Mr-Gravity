using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MrGravity.MISC_Code;

namespace MrGravity.Menu_Code
{
    /// <summary>
    /// 
    /// </summary>
    internal class MainMenu
    {
        public enum MenuChoices { StartGame, Options, Exit, Credits }

        private readonly Dictionary<MenuChoices, Texture2D> _mUnselected;
        private readonly Dictionary<MenuChoices, Texture2D> _mSelected;

        private Texture2D _mTitle;
        private Texture2D _mBackground;

        private readonly IControlScheme _mControls;
        private readonly GraphicsDeviceManager _mGraphics;

        private MenuChoices _mCurrentChoice = MenuChoices.StartGame;

        public MainMenu(IControlScheme controlScheme, GraphicsDeviceManager graphics)
        {
            _mControls = controlScheme;
            _mGraphics = graphics;

            _mUnselected = new Dictionary<MenuChoices, Texture2D>();
            _mSelected = new Dictionary<MenuChoices, Texture2D>();
        }

        public void Load(ContentManager content)
        {
            _mUnselected.Add(MenuChoices.StartGame, content.Load<Texture2D>("Images\\Menu\\Main\\PlayUnselected"));
            _mUnselected.Add(MenuChoices.Exit, content.Load<Texture2D>("Images\\Menu\\Main\\ExitUnselected"));
            _mUnselected.Add(MenuChoices.Options, content.Load<Texture2D>("Images\\Menu\\Main\\OptionsUnselected"));
            _mUnselected.Add(MenuChoices.Credits, content.Load<Texture2D>("Images\\Menu\\Main\\CreditsUnselected"));

            _mSelected.Add(MenuChoices.StartGame, content.Load<Texture2D>("Images\\Menu\\Main\\PlaySelected"));
            _mSelected.Add(MenuChoices.Exit, content.Load<Texture2D>("Images\\Menu\\Main\\ExitSelected"));
            _mSelected.Add(MenuChoices.Options, content.Load<Texture2D>("Images\\Menu\\Main\\OptionsSelected"));
            _mSelected.Add(MenuChoices.Credits, content.Load<Texture2D>("Images\\Menu\\Main\\CreditsSelected"));

            _mTitle = content.Load<Texture2D>("Images\\Menu\\Mr_Gravity");
            _mBackground = content.Load<Texture2D>("Images\\Menu\\backgroundSquares1");

        }


        public void Update(GameTime gametime, ref GameStates states, Level mainMenuLevel)
        {
            var env = mainMenuLevel.Environment;
            if (_mControls.IsBackPressed(false))
                states = GameStates.Exit;
            if (_mControls.IsAPressed(false) || _mControls.IsStartPressed(false))
            {
                if (_mCurrentChoice == MenuChoices.StartGame)
                {
                    states = GameStates.LevelSelection;
                    mainMenuLevel.Reload();
                }
                if (_mCurrentChoice == MenuChoices.Exit)
                    states = GameStates.Exit;
                if (_mCurrentChoice == MenuChoices.Options)
                    states = GameStates.Options;
                if (_mCurrentChoice == MenuChoices.Credits)
                {
                    states = GameStates.Credits;
                    mainMenuLevel.Reload();
                }
                env.GravityDirection = GravityDirections.Down;
            }

            if (env.GravityDirection == GravityDirections.Down)
                _mCurrentChoice = MenuChoices.StartGame;
            if (env.GravityDirection == GravityDirections.Left)
                _mCurrentChoice = MenuChoices.Credits;
            if (env.GravityDirection == GravityDirections.Right)
                _mCurrentChoice = MenuChoices.Options;
            if (env.GravityDirection == GravityDirections.Up)
                _mCurrentChoice = MenuChoices.Exit;
        }

        public void Draw(GameTime gametime, SpriteBatch spriteBatch, Matrix scale)
        {
            spriteBatch.Begin(SpriteSortMode.Immediate,
                BlendState.AlphaBlend,
                SamplerState.LinearClamp,
                DepthStencilState.None,
                RasterizerState.CullCounterClockwise,
                null,
                scale);

            var mSize = new float[2] { _mGraphics.GraphicsDevice.Viewport.TitleSafeArea.Width / (float)_mGraphics.GraphicsDevice.Viewport.Width, _mGraphics.GraphicsDevice.Viewport.TitleSafeArea.Height / (float)_mGraphics.GraphicsDevice.Viewport.Height };
#if XBOX360
            Point center = mGraphics.GraphicsDevice.Viewport.TitleSafeArea.Center;
            spriteBatch.Draw(mBackground, new Rectangle(0, 0, mGraphics.GraphicsDevice.Viewport.Width, mGraphics.GraphicsDevice.Viewport.Height), Color.White);
            spriteBatch.Draw(mTitle, new Rectangle(center.X - (int)(mTitle.Width * mSize[0]) / 2, mGraphics.GraphicsDevice.Viewport.TitleSafeArea.Top, (int)(mTitle.Width * mSize[0]), (int)(mTitle.Height * mSize[1])), Color.White);

            for (int i = 0; i < 4; i++)
            {
                MenuChoices choice = (MenuChoices)i;
                if (choice == mCurrentChoice)
                    spriteBatch.Draw(mSelected[choice], GetRegion(choice, mSelected[choice]), Color.White);
                else
                    spriteBatch.Draw(mUnselected[choice], GetRegion(choice, mUnselected[choice]), Color.White);
            }
#else
            Point center = _mGraphics.GraphicsDevice.Viewport.TitleSafeArea.Center;
            spriteBatch.Draw(_mBackground, new Rectangle(0, 0, _mGraphics.GraphicsDevice.Viewport.Width, _mGraphics.GraphicsDevice.Viewport.Height), Color.White);
            spriteBatch.Draw(_mTitle, new Rectangle(center.X - (int)(_mTitle.Width * mSize[0]) / 2, _mGraphics.GraphicsDevice.Viewport.TitleSafeArea.Top, (int)(_mTitle.Width * mSize[0]), (int)(_mTitle.Height * mSize[1])), Color.White);

            foreach (MenuChoices choice in Enum.GetValues(typeof(MenuChoices)))
                if (choice == _mCurrentChoice)
                    spriteBatch.Draw(_mSelected[choice], GetRegion(choice, _mSelected[choice]), Color.White);
                else
                    spriteBatch.Draw(_mUnselected[choice], GetRegion(choice, _mUnselected[choice]), Color.White);
#endif

            spriteBatch.End();
        }

        public Rectangle GetRegion(MenuChoices choice, Texture2D texture)
        {
            Viewport viewport = _mGraphics.GraphicsDevice.Viewport;

            var mSize = new float[2] { viewport.TitleSafeArea.Width / (float)_mGraphics.GraphicsDevice.Viewport.Width, viewport.TitleSafeArea.Height / (float)_mGraphics.GraphicsDevice.Viewport.Height };

            if (choice == MenuChoices.StartGame)
                return new Rectangle(viewport.TitleSafeArea.Center.X - ((int)(texture.Width * mSize[0]) / 2),
                    viewport.TitleSafeArea.Bottom - (int)(texture.Height * mSize[1]), (int)(texture.Width * mSize[0]), (int)(texture.Height * mSize[1]));
            if (choice == MenuChoices.Exit)
                return new Rectangle(viewport.TitleSafeArea.Center.X - ((int)(texture.Width * mSize[0]) / 2),
                    viewport.TitleSafeArea.Top + (int)(_mTitle.Height * mSize[1]), (int)(texture.Width * mSize[0]), (int)(texture.Height * mSize[1]));
            if (choice == MenuChoices.Options)
                return new Rectangle(viewport.TitleSafeArea.Right - ((int)(texture.Width * mSize[0])) - (int)(_mTitle.Height * mSize[1]),
                    viewport.TitleSafeArea.Center.Y + (int)(_mTitle.Height * mSize[1]) / 2 - ((int)(texture.Height * mSize[1]) / 2), (int)(texture.Width * mSize[0]), (int)(texture.Height * mSize[1]));
            if (choice == MenuChoices.Credits)
                return new Rectangle(viewport.TitleSafeArea.Left + (int)(_mTitle.Height * mSize[1]),
                    viewport.TitleSafeArea.Center.Y + (int)(_mTitle.Height * mSize[1]) / 2 - ((int)(texture.Height * mSize[1]) / 2), (int)(texture.Width * mSize[0]), (int)(texture.Height * mSize[1]));
            return new Rectangle();
        }

        /// <summary>
        /// Gets the rectangle between the menu options
        /// </summary>
        /// <returns>Rectangle between menu choices</returns>
        public Rectangle GetInnerRegion()
        {
            var topRectangle = GetRegion(MenuChoices.Exit, _mSelected[MenuChoices.Exit]);
            var leftRectangle = GetRegion(MenuChoices.Credits, _mSelected[MenuChoices.Credits]);
            var rightRectangle = GetRegion(MenuChoices.Options, _mSelected[MenuChoices.Options]);
            var bottomRectangle = GetRegion(MenuChoices.StartGame, _mSelected[MenuChoices.StartGame]);

            var region = new Rectangle(leftRectangle.Right, topRectangle.Bottom, 
                rightRectangle.Left - leftRectangle.Right,bottomRectangle.Top - topRectangle.Bottom);
            return region;           
        }
    }
}
