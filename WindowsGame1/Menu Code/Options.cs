using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace GravityShift
{
    class Options
    {
        public enum MenuChoices { Volume, Controls, Reset, Back }

        Dictionary<MenuChoices, Texture2D> mUnselected;
        Dictionary<MenuChoices, Texture2D> mSelected;

        Texture2D mTitle;
        Texture2D mBackground;

        IControlScheme mControls;
        GraphicsDeviceManager mGraphics;

        MenuChoices mCurrentChoice = MenuChoices.Controls;

        public Options(IControlScheme controlScheme, GraphicsDeviceManager graphics)
        {
            mControls = controlScheme;
            mGraphics = graphics;

            mUnselected = new Dictionary<MenuChoices, Texture2D>();
            mSelected = new Dictionary<MenuChoices, Texture2D>();
        }

        public void Load(ContentManager content)
        {
            mSelected.Add(MenuChoices.Back, content.Load<Texture2D>("Images\\Menu\\Main\\BackSelected"));
            mSelected.Add(MenuChoices.Volume, content.Load<Texture2D>("Images\\Menu\\Main\\SoundSelected"));
            mSelected.Add(MenuChoices.Controls, content.Load<Texture2D>("Images\\Menu\\Main\\ControllerSelected"));
            mSelected.Add(MenuChoices.Reset, content.Load<Texture2D>("Images\\Menu\\Main\\ResetSelected"));

            mUnselected.Add(MenuChoices.Back, content.Load<Texture2D>("Images\\Menu\\Main\\BackUnselected"));
            mUnselected.Add(MenuChoices.Volume, content.Load<Texture2D>("Images\\Menu\\Main\\SoundUnselected"));
            mUnselected.Add(MenuChoices.Controls, content.Load<Texture2D>("Images\\Menu\\Main\\ControllerUnselected"));
            mUnselected.Add(MenuChoices.Reset, content.Load<Texture2D>("Images\\Menu\\Main\\ResetUnselected"));

            mTitle = content.Load<Texture2D>("Images\\Menu\\Mr_Gravity");
            mBackground = content.Load<Texture2D>("Images\\Menu\\backgroundSquares1");
        }


        public void Update(GameTime gametime, ref GameStates states, PhysicsEnvironment env)
        {
            if (mControls.isBackPressed(false) || mControls.isBPressed(false))
                states = GameStates.Main_Menu;

            if (mControls.isAPressed(false) || mControls.isStartPressed(false))
            {
                if (mCurrentChoice == MenuChoices.Volume) 
                    states = GameStates.SoundOptions;
                if (mCurrentChoice == MenuChoices.Controls) 
                    states = GameStates.Controls;
                if (mCurrentChoice == MenuChoices.Reset)
                    states = GameStates.ResetConfirm;
                if (mCurrentChoice == MenuChoices.Back)
                    states = GameStates.Main_Menu;

                env.GravityDirection = GravityDirections.Down;
            }

            if (env.GravityDirection == GravityDirections.Down)
                mCurrentChoice = MenuChoices.Controls;
            if (env.GravityDirection == GravityDirections.Left)
                mCurrentChoice = MenuChoices.Reset;
            if (env.GravityDirection == GravityDirections.Right)
                mCurrentChoice = MenuChoices.Volume;
            if (env.GravityDirection == GravityDirections.Up)
                mCurrentChoice = MenuChoices.Back;
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

            float[] mSize = new float[2] { (float)mGraphics.GraphicsDevice.Viewport.TitleSafeArea.Width / (float)mGraphics.GraphicsDevice.Viewport.Width, (float)mGraphics.GraphicsDevice.Viewport.TitleSafeArea.Height / (float)mGraphics.GraphicsDevice.Viewport.Height };

            Point center = mGraphics.GraphicsDevice.Viewport.TitleSafeArea.Center;
            spriteBatch.Draw(mBackground, new Rectangle(0, 0, mGraphics.GraphicsDevice.Viewport.Width, mGraphics.GraphicsDevice.Viewport.Height), Color.White);
            spriteBatch.Draw(mTitle, new Rectangle(center.X - (int)(mTitle.Width * mSize[0]) / 2, mGraphics.GraphicsDevice.Viewport.TitleSafeArea.Top, (int)(mTitle.Width * mSize[0]), (int)(mTitle.Height * mSize[1])), Color.White);
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
                if (choice == mCurrentChoice)
                    spriteBatch.Draw(mSelected[choice], GetRegion(choice, mSelected[choice]), Color.White);
                else
                    spriteBatch.Draw(mUnselected[choice], GetRegion(choice, mUnselected[choice]), Color.White);
#endif
            //Point center = mGraphics.GraphicsDevice.Viewport.TitleSafeArea.Center;
            //spriteBatch.Draw(mTitle, new Rectangle(center.X + 30 - mTitle.Width / 2, center.Y - mTitle.Height / 2, mTitle.Width, mTitle.Height), Color.White);

            spriteBatch.End();
        }

        public Rectangle GetRegion(MenuChoices choice, Texture2D texture)
        {
            Viewport viewport = mGraphics.GraphicsDevice.Viewport;

            float[] mSize = new float[2] { (float)mGraphics.GraphicsDevice.Viewport.TitleSafeArea.Width / (float)mGraphics.GraphicsDevice.Viewport.Width, (float)mGraphics.GraphicsDevice.Viewport.TitleSafeArea.Height / (float)mGraphics.GraphicsDevice.Viewport.Height };

            if (choice == MenuChoices.Controls)
                return new Rectangle(viewport.TitleSafeArea.Center.X - ((int)(texture.Width * mSize[0]) / 2),
                    viewport.TitleSafeArea.Bottom - (int)(texture.Height * mSize[1]), (int)(texture.Width * mSize[0]), (int)(texture.Height * mSize[1]));
            if (choice == MenuChoices.Back)
                return new Rectangle(viewport.TitleSafeArea.Center.X - ((int)(texture.Width * mSize[0]) / 2),
                    viewport.TitleSafeArea.Top + (int)(mTitle.Height * mSize[1]), (int)(texture.Width * mSize[0]), (int)(texture.Height * mSize[1]));
            if (choice == MenuChoices.Volume)
                return new Rectangle(viewport.TitleSafeArea.Right - ((int)(texture.Width * mSize[0])) - (int)(mTitle.Height * mSize[1]),
                    viewport.TitleSafeArea.Center.Y + (int)(mTitle.Height * mSize[1]) / 2 - ((int)(texture.Height * mSize[1]) / 2), (int)(texture.Width * mSize[0]), (int)(texture.Height * mSize[1]));
            if (choice == MenuChoices.Reset)
                return new Rectangle(viewport.TitleSafeArea.Left + (int)(mTitle.Height * mSize[1]),
                    viewport.TitleSafeArea.Center.Y + (int)(mTitle.Height * mSize[1]) / 2 - ((int)(texture.Height * mSize[1]) / 2), (int)(texture.Width * mSize[0]), (int)(texture.Height * mSize[1]));
            return new Rectangle();
        }
    }
}
