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
    public class Menu
    {
        #region Member Variables

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private SpriteFont kootenay;

        private Texture2D[] selMenuItems;
        private Texture2D[] unselMenuItems;
        private Texture2D[] menuItems;

        private const int MENU_OPTIONS = 4;

        GamePadState pad_state;
        GamePadState prev_pad_state;
        KeyboardState key_state;
        KeyboardState prev_key_state;

        bool isMenu;
        bool start;

        int current;
        int timer;

        #endregion

        #region Menu Art

        private Texture2D newGameUnsel;
        private Texture2D newGameSel;
        private Texture2D loadGameUnsel;
        private Texture2D loadGameSel;
        private Texture2D exitSel;
        private Texture2D exitUnsel;
        private Texture2D optionsSel;
        private Texture2D optionsUnsel;

        #endregion

        public Menu()
        {
        }

        public void Load(ContentManager content)
        {
            current = 0;

            selMenuItems = new Texture2D[MENU_OPTIONS];
            unselMenuItems = new Texture2D[MENU_OPTIONS];
            menuItems = new Texture2D[MENU_OPTIONS];

            newGameSel = content.Load<Texture2D>("menu/NewGameSelected");
            newGameUnsel = content.Load<Texture2D>("menu/NewGameUnselected");
            loadGameSel = content.Load<Texture2D>("menu/LoadGameSelected");
            loadGameUnsel = content.Load<Texture2D>("menu/LoadGameUnselected");
            exitSel = content.Load<Texture2D>("menu/ExitSelected");
            exitUnsel = content.Load<Texture2D>("menu/ExitUnselected");
            optionsSel = content.Load<Texture2D>("menu/OptionSelected");
            optionsUnsel = content.Load<Texture2D>("menu/OptionUnselected");

            selMenuItems[0] = newGameSel;
            selMenuItems[1] = loadGameSel;
            selMenuItems[2] = optionsSel;
            selMenuItems[3] = exitSel;

            unselMenuItems[0] = newGameUnsel;
            unselMenuItems[1] = loadGameUnsel;
            unselMenuItems[2] = optionsUnsel;
            unselMenuItems[3] = exitUnsel;

            menuItems[0] = newGameSel;
            menuItems[1] = loadGameUnsel;
            menuItems[2] = optionsUnsel;
            menuItems[3] = exitUnsel;

            prev_pad_state = GamePad.GetState(PlayerIndex.One);

            start = false;

            timer = 0;

            kootenay = content.Load<SpriteFont>("fonts/Kootenay");
        }

        public void Update(GameTime gameTime)
        {
            key_state = Keyboard.GetState();
            pad_state = GamePad.GetState(PlayerIndex.One);

            if (pad_state.IsButtonDown(Buttons.LeftThumbstickUp) &&
                prev_pad_state.IsButtonUp(Buttons.LeftThumbstickUp) ||
                key_state.IsKeyDown(Keys.Up) &&
                prev_key_state.IsKeyUp(Keys.Up))
            {
                if (current > 0)
                {
                    current--;
                    for (int i = 0; i < MENU_OPTIONS; i++)
                        menuItems[i] = unselMenuItems[i];
                    menuItems[current] = selMenuItems[current];
                }
            }
            if (pad_state.IsButtonDown(Buttons.LeftThumbstickDown) &&
                prev_pad_state.IsButtonUp(Buttons.LeftThumbstickDown) ||
                key_state.IsKeyDown(Keys.Down) &&
                prev_key_state.IsKeyUp(Keys.Down))
            {
                if (current < MENU_OPTIONS-1)
                {
                    current++;
                    for (int i = 0; i < MENU_OPTIONS; i++)
                        menuItems[i] = unselMenuItems[i];
                    menuItems[current] = selMenuItems[current];
                }
            }

            if (pad_state.IsButtonDown(Buttons.A) &&
                prev_pad_state.IsButtonUp(Buttons.A) ||
                key_state.IsKeyDown(Keys.Enter) &&
                prev_key_state.IsKeyUp(Keys.Enter))
            {
                isMenu = false;
                /* New Game */
                if (current == 0)
                {
                    GravityShiftMain.InMenu = false;
                    GravityShiftMain.InGame = true;
                }
                /* Load Game */
                else if (current == 1)
                {

                }
                /* Options */
                else if (current == 2)
                {

                }
                /* Exit */
                else if (current == 3)
                {
                }
            }

            prev_pad_state = pad_state;
            prev_key_state = key_state;
        }

        public void Draw(SpriteBatch spriteBatch, GraphicsDeviceManager graphics)
        {
            spriteBatch.Begin();

            spriteBatch.DrawString(kootenay, "Gravity Shift",
                                   new Vector2(150.0f, 0.0f), Color.Black);
            spriteBatch.Draw(menuItems[0], new Vector2(300.0f, 200.0f), Color.White);
            spriteBatch.Draw(menuItems[1], new Vector2(300.0f, 300.0f), Color.White);
            spriteBatch.Draw(menuItems[2], new Vector2(300.0f, 400.0f), Color.White);
            spriteBatch.Draw(menuItems[3], new Vector2(300.0f, 500.0f), Color.White);

            spriteBatch.End();
        }
    }
}