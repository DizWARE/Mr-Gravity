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
            mSelected.Add(MenuChoices.Volume, content.Load<Texture2D>("Images\\Menu\\Main\\MuteSelected"));
            mSelected.Add(MenuChoices.Controls, content.Load<Texture2D>("Images\\Menu\\Main\\ControllerSelected"));
            mSelected.Add(MenuChoices.Reset, content.Load<Texture2D>("Images\\Menu\\Main\\ResetSelected"));

            mUnselected.Add(MenuChoices.Back, content.Load<Texture2D>("Images\\Menu\\Main\\BackUnselected"));
            mUnselected.Add(MenuChoices.Volume, content.Load<Texture2D>("Images\\Menu\\Main\\MuteUnselected"));
            mUnselected.Add(MenuChoices.Controls, content.Load<Texture2D>("Images\\Menu\\Main\\ControllerUnselected"));
            mUnselected.Add(MenuChoices.Reset, content.Load<Texture2D>("Images\\Menu\\Main\\ResetUnselected"));

            mTitle = content.Load<Texture2D>("Images\\Menu\\Title");
        }


        public void Update(GameTime gametime, ref GameStates states, PhysicsEnvironment env)
        {
            if (mControls.isBackPressed(false) || mControls.isBPressed(false))
                states = GameStates.Main_Menu;

            if (mControls.isAPressed(false) || mControls.isStartPressed(false))
            {
                if (mCurrentChoice == MenuChoices.Volume) ;
                if (mCurrentChoice == MenuChoices.Controls) ;
                if (mCurrentChoice == MenuChoices.Reset) ;
                if (mCurrentChoice == MenuChoices.Back)
                    states = GameStates.Main_Menu;
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
            spriteBatch.Begin();
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
            Point center = mGraphics.GraphicsDevice.Viewport.TitleSafeArea.Center;
            //spriteBatch.Draw(mTitle, new Rectangle(center.X + 30 - mTitle.Width / 2, center.Y - mTitle.Height / 2, mTitle.Width, mTitle.Height), Color.White);

            spriteBatch.End();
        }

        public Rectangle GetRegion(MenuChoices choice, Texture2D texture)
        {
            Viewport viewport = mGraphics.GraphicsDevice.Viewport;

            if (choice == MenuChoices.Controls)
                return new Rectangle(viewport.TitleSafeArea.Center.X - (texture.Width / 2),
                    viewport.TitleSafeArea.Bottom - 25 - texture.Height, texture.Width, texture.Height);
            if (choice == MenuChoices.Back)
                return new Rectangle(viewport.TitleSafeArea.Center.X - (texture.Width / 2),
                    viewport.TitleSafeArea.Top + texture.Height, texture.Width, texture.Height);
            if (choice == MenuChoices.Volume)
                return new Rectangle(viewport.TitleSafeArea.Right - (texture.Width),
                    viewport.TitleSafeArea.Center.Y - (texture.Height / 2), texture.Width, texture.Height);
            if (choice == MenuChoices.Reset)
                return new Rectangle(viewport.TitleSafeArea.Left + (texture.Width),
                    viewport.TitleSafeArea.Center.Y - (texture.Height / 2), texture.Width, texture.Height);
            return new Rectangle();
        }
    }
}
