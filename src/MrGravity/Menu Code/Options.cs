using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MrGravity.MISC_Code;

namespace MrGravity.Menu_Code
{
    internal class Options
    {
        public enum MenuChoices { Volume, Controls, Reset, Back }

        private readonly Dictionary<MenuChoices, Texture2D> _mUnselected;
        private readonly Dictionary<MenuChoices, Texture2D> _mSelected;

        private Texture2D _mTitle;
        private Texture2D _mBackground;

        private readonly IControlScheme _mControls;
        private readonly GraphicsDeviceManager _mGraphics;

        private MenuChoices _mCurrentChoice = MenuChoices.Controls;

        public Options(IControlScheme controlScheme, GraphicsDeviceManager graphics)
        {
            _mControls = controlScheme;
            _mGraphics = graphics;

            _mUnselected = new Dictionary<MenuChoices, Texture2D>();
            _mSelected = new Dictionary<MenuChoices, Texture2D>();
        }

        public void Load(ContentManager content)
        {
            _mSelected.Add(MenuChoices.Back, content.Load<Texture2D>("Images\\Menu\\Main\\BackSelected"));
            _mSelected.Add(MenuChoices.Volume, content.Load<Texture2D>("Images\\Menu\\Main\\SoundSelected"));
            _mSelected.Add(MenuChoices.Controls, content.Load<Texture2D>("Images\\Menu\\Main\\ControllerSelected"));
            _mSelected.Add(MenuChoices.Reset, content.Load<Texture2D>("Images\\Menu\\Main\\ResetSelected"));

            _mUnselected.Add(MenuChoices.Back, content.Load<Texture2D>("Images\\Menu\\Main\\BackUnselected"));
            _mUnselected.Add(MenuChoices.Volume, content.Load<Texture2D>("Images\\Menu\\Main\\SoundUnselected"));
            _mUnselected.Add(MenuChoices.Controls, content.Load<Texture2D>("Images\\Menu\\Main\\ControllerUnselected"));
            _mUnselected.Add(MenuChoices.Reset, content.Load<Texture2D>("Images\\Menu\\Main\\ResetUnselected"));

            _mTitle = content.Load<Texture2D>("Images\\Menu\\Mr_Gravity");
            _mBackground = content.Load<Texture2D>("Images\\Menu\\backgroundSquares1");
        }


        public void Update(GameTime gametime, ref GameStates states, Level mainMenuLevel)
        {
            var env = mainMenuLevel.Environment;
            if (_mControls.IsBackPressed(false) || _mControls.IsBPressed(false))
                states = GameStates.MainMenu;

            if (_mControls.IsAPressed(false) || _mControls.IsStartPressed(false))
            {
                if (_mCurrentChoice == MenuChoices.Volume)
                {
                    states = GameStates.SoundOptions;
                    mainMenuLevel.Reload();
                }
                if (_mCurrentChoice == MenuChoices.Controls)
                {
                    states = GameStates.Controls;
                    mainMenuLevel.Reload();
                }
                if (_mCurrentChoice == MenuChoices.Reset)
                {
                    states = GameStates.ResetConfirm;
                    mainMenuLevel.Reload();
                }
                if (_mCurrentChoice == MenuChoices.Back)
                    states = GameStates.MainMenu;

                env.GravityDirection = GravityDirections.Down;
            }

            if (env.GravityDirection == GravityDirections.Down)
                _mCurrentChoice = MenuChoices.Controls;
            if (env.GravityDirection == GravityDirections.Left)
                _mCurrentChoice = MenuChoices.Reset;
            if (env.GravityDirection == GravityDirections.Right)
                _mCurrentChoice = MenuChoices.Volume;
            if (env.GravityDirection == GravityDirections.Up)
                _mCurrentChoice = MenuChoices.Back;
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

            Point center = _mGraphics.GraphicsDevice.Viewport.TitleSafeArea.Center;
            spriteBatch.Draw(_mBackground, new Rectangle(0, 0, _mGraphics.GraphicsDevice.Viewport.Width, _mGraphics.GraphicsDevice.Viewport.Height), Color.White);
            spriteBatch.Draw(_mTitle, new Rectangle(center.X - (int)(_mTitle.Width * mSize[0]) / 2, _mGraphics.GraphicsDevice.Viewport.TitleSafeArea.Top, (int)(_mTitle.Width * mSize[0]), (int)(_mTitle.Height * mSize[1])), Color.White);
#if XBOX360
            for (int i = 0; i < 4; i++)
            {
                MenuChoices choice = (MenuChoices)i;
                if (choice == mCurrentChoice)
                    spriteBatch.Draw(mSelected[choice], GetRegion(choice, mSelected[choice]), Color.White);
                else
                    spriteBatch.Draw(mUnselected[choice], GetRegion(choice, mUnselected[choice]), Color.White);
            }
#else

            foreach (MenuChoices choice in Enum.GetValues(typeof(MenuChoices)))
                if (choice == _mCurrentChoice)
                    spriteBatch.Draw(_mSelected[choice], GetRegion(choice, _mSelected[choice]), Color.White);
                else
                    spriteBatch.Draw(_mUnselected[choice], GetRegion(choice, _mUnselected[choice]), Color.White);
#endif
            //Point center = mGraphics.GraphicsDevice.Viewport.TitleSafeArea.Center;
            //spriteBatch.Draw(mTitle, new Rectangle(center.X + 30 - mTitle.Width / 2, center.Y - mTitle.Height / 2, mTitle.Width, mTitle.Height), Color.White);

            spriteBatch.End();
        }

        public Rectangle GetRegion(MenuChoices choice, Texture2D texture)
        {
            Viewport viewport = _mGraphics.GraphicsDevice.Viewport;

            var mSize = new float[2] { _mGraphics.GraphicsDevice.Viewport.TitleSafeArea.Width / (float)_mGraphics.GraphicsDevice.Viewport.Width, _mGraphics.GraphicsDevice.Viewport.TitleSafeArea.Height / (float)_mGraphics.GraphicsDevice.Viewport.Height };

            if (choice == MenuChoices.Controls)
                return new Rectangle(viewport.TitleSafeArea.Center.X - ((int)(texture.Width * mSize[0]) / 2),
                    viewport.TitleSafeArea.Bottom - (int)(texture.Height * mSize[1]), (int)(texture.Width * mSize[0]), (int)(texture.Height * mSize[1]));
            if (choice == MenuChoices.Back)
                return new Rectangle(viewport.TitleSafeArea.Center.X - ((int)(texture.Width * mSize[0]) / 2),
                    viewport.TitleSafeArea.Top + (int)(_mTitle.Height * mSize[1]), (int)(texture.Width * mSize[0]), (int)(texture.Height * mSize[1]));
            if (choice == MenuChoices.Volume)
                return new Rectangle(viewport.TitleSafeArea.Right - ((int)(texture.Width * mSize[0])) - (int)(_mTitle.Height * mSize[1]),
                    viewport.TitleSafeArea.Center.Y + (int)(_mTitle.Height * mSize[1]) / 2 - ((int)(texture.Height * mSize[1]) / 2), (int)(texture.Width * mSize[0]), (int)(texture.Height * mSize[1]));
            if (choice == MenuChoices.Reset)
                return new Rectangle(viewport.TitleSafeArea.Left + (int)(_mTitle.Height * mSize[1]),
                    viewport.TitleSafeArea.Center.Y + (int)(_mTitle.Height * mSize[1]) / 2 - ((int)(texture.Height * mSize[1]) / 2), (int)(texture.Width * mSize[0]), (int)(texture.Height * mSize[1]));
            return new Rectangle();
        }
    }
}
