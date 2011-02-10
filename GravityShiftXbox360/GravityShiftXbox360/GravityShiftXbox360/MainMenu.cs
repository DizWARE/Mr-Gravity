using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace GravityShift
{
    /// <summary>
    /// 
    /// </summary>
    class MainMenu
    {
        public enum MenuChoices { StartGame, Options, Exit, Credits }

        Dictionary<MenuChoices, Texture2D> mUnselected;
        Dictionary<MenuChoices, Texture2D> mSelected;

        Texture2D mTitle;

        IControlScheme mControls;
        GraphicsDeviceManager mGraphics;

        MenuChoices mCurrentChoice = MenuChoices.StartGame;

        public MainMenu(IControlScheme controlScheme, GraphicsDeviceManager graphics)
        {
            mControls = controlScheme;
            mGraphics = graphics;

            mUnselected = new Dictionary<MenuChoices, Texture2D>();
            mSelected = new Dictionary<MenuChoices, Texture2D>();
        }

        public void Load(ContentManager content)
        {
            mUnselected.Add(MenuChoices.StartGame, content.Load<Texture2D>("Images\\Menu\\Main\\LoadGameUnselected"));
            mUnselected.Add(MenuChoices.Exit, content.Load<Texture2D>("Images\\Menu\\Main\\BackUnselected"));
            mUnselected.Add(MenuChoices.Options, content.Load<Texture2D>("Images\\Menu\\Main\\OptionsUnselected"));
            mUnselected.Add(MenuChoices.Credits, content.Load<Texture2D>("Images\\Menu\\Main\\CreditsUnselected"));

            mSelected.Add(MenuChoices.StartGame, content.Load<Texture2D>("Images\\Menu\\Main\\LoadGameSelected"));
            mSelected.Add(MenuChoices.Exit, content.Load<Texture2D>("Images\\Menu\\Main\\BackSelected"));
            mSelected.Add(MenuChoices.Options, content.Load<Texture2D>("Images\\Menu\\Main\\OptionsSelected"));
            mSelected.Add(MenuChoices.Credits, content.Load<Texture2D>("Images\\Menu\\Main\\CreditsSelected"));

            mTitle = content.Load<Texture2D>("Images\\Menu\\Title");
        }


        public void Update(GameTime gametime, ref GameStates states, PhysicsEnvironment env)
        {
            if (mControls.isBackPressed(false))
                states = GameStates.Exit;
            if (mControls.isAPressed(false) || mControls.isStartPressed(false))
            {
                if (mCurrentChoice == MenuChoices.StartGame)
                    states = GameStates.Level_Selection;
                if (mCurrentChoice == MenuChoices.Exit)
                    states = GameStates.Exit;
                if (mCurrentChoice == MenuChoices.Options)
                    states = GameStates.Options;
                if (mCurrentChoice == MenuChoices.Credits)
                    states = GameStates.Credits;
            }

            if (env.GravityDirection == GravityDirections.Down)
                mCurrentChoice = MenuChoices.StartGame;
            if (env.GravityDirection == GravityDirections.Left)
                mCurrentChoice = MenuChoices.Credits;
            if (env.GravityDirection == GravityDirections.Right)
                mCurrentChoice = MenuChoices.Options;
            if (env.GravityDirection == GravityDirections.Up)
                mCurrentChoice = MenuChoices.Exit;
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

            if (choice == MenuChoices.StartGame)
                return new Rectangle(viewport.TitleSafeArea.Center.X - (texture.Width / 2),
                    viewport.TitleSafeArea.Bottom - texture.Height, texture.Width, texture.Height);
            if (choice == MenuChoices.Exit)
                return new Rectangle(viewport.TitleSafeArea.Center.X - (texture.Width / 2),
                    viewport.TitleSafeArea.Top, texture.Width, texture.Height);
            if (choice == MenuChoices.Options)
                return new Rectangle(viewport.TitleSafeArea.Right - (texture.Width),
                    viewport.TitleSafeArea.Center.Y - (texture.Height / 2), texture.Width, texture.Height);
            if (choice == MenuChoices.Credits)
                return new Rectangle(viewport.TitleSafeArea.Left,
                    viewport.TitleSafeArea.Center.Y - (texture.Height / 2), texture.Width, texture.Height);
            return new Rectangle();
        }
    }
}
