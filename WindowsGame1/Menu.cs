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

        private SpriteFont kootenay;

        private Texture2D[] selMenuItems;
        private Texture2D[] unselMenuItems;
        private Texture2D[] menuItems;

        enum states
        {
            TITLE,
            OPTIONS,
            LOAD,
            CONTROLLER,
            SOUNDS,
            CREDITS
        };

        states state;

        private const int NUM_TITLE = 4;
        private const int NUM_LOAD = 4;
        private const int NUM_OPTIONS = 3;

        GamePadState pad_state;
        GamePadState prev_pad_state;
        KeyboardState key_state;
        KeyboardState prev_key_state;

        int current;

        #endregion

        #region Menu Art

        private Texture2D title;

        /* Title */
        private Texture2D newGameUnsel;
        private Texture2D newGameSel;
        private Texture2D loadGameUnsel;
        private Texture2D loadGameSel;
        private Texture2D optionsSel;
        private Texture2D optionsUnsel;
        private Texture2D creditsSel;
        private Texture2D creditsUnsel;

        /* Options Screen */
        private Texture2D controlUnsel;
        private Texture2D controlSel;
        private Texture2D soundUnsel;
        private Texture2D soundSel;

        /* Load Screen */
        private Texture2D backUnsel;
        private Texture2D backSel;
        private Texture2D oneUnsel;
        private Texture2D oneSel;
        private Texture2D twoUnsel;
        private Texture2D twoSel;
        private Texture2D threeUnsel;
        private Texture2D threeSel;

        /* Controller Settings */
        private Texture2D xboxControl;

        #endregion

        /*
         * Menu Contructor
         *
         * Currently does not do anything
         */
        public Menu() { }

        /*
         * Load
         *
         * Similar to a loadContent function. This function loads and 
         * initializes the variable and art used in the class.
         *
         * ContentManager content: the Content file used in the game.
         */
        public void Load(ContentManager content)
        {
            state = states.TITLE;
            current = 0;

            selMenuItems = new Texture2D[NUM_TITLE];
            unselMenuItems = new Texture2D[NUM_TITLE];
            menuItems = new Texture2D[NUM_TITLE];

            title = content.Load<Texture2D>("menu/Title");

            /* Title Screen */
            newGameSel = content.Load<Texture2D>("menu/NewGameSelected");
            newGameUnsel = content.Load<Texture2D>("menu/NewGameUnselected");
            loadGameSel = content.Load<Texture2D>("menu/LoadGameSelected");
            loadGameUnsel = content.Load<Texture2D>("menu/LoadGameUnselected");
            optionsSel = content.Load<Texture2D>("menu/OptionsSelected");
            optionsUnsel = content.Load<Texture2D>("menu/OptionsUnselected");
            creditsSel = content.Load<Texture2D>("menu/CreditsSelected");
            creditsUnsel = content.Load<Texture2D>("menu/CreditsUnselected");

            /* Options Screen */
            controlUnsel = content.Load<Texture2D>("menu/ControllerUnselected");
            controlSel = content.Load<Texture2D>("menu/ControllerSelected");
            soundUnsel = content.Load<Texture2D>("menu/SoundUnselected");
            soundSel = content.Load<Texture2D>("menu/SoundSelected");
            backUnsel = content.Load<Texture2D>("menu/BackUnselected");
            backSel = content.Load<Texture2D>("menu/BackSelected");

            /* Level Select Screen */
            oneUnsel = content.Load<Texture2D>("menu/OneUnselected");
            oneSel = content.Load<Texture2D>("menu/OneSelected");
            twoUnsel = content.Load<Texture2D>("menu/TwoUnselected");
            twoSel = content.Load<Texture2D>("menu/TwoSelected");
            threeUnsel = content.Load<Texture2D>("menu/ThreeUnselected");
            threeSel = content.Load<Texture2D>("menu/ThreeSelected");

            /* Controller */
            xboxControl = content.Load<Texture2D>("menu/XboxController");

            /* Initialize the menu item arrays */
            selMenuItems[0] = newGameSel;
            selMenuItems[1] = loadGameSel;
            selMenuItems[2] = optionsSel;
            selMenuItems[3] = creditsSel;

            unselMenuItems[0] = newGameUnsel;
            unselMenuItems[1] = loadGameUnsel;
            unselMenuItems[2] = optionsUnsel;
            unselMenuItems[3] = creditsUnsel;

            menuItems[0] = newGameSel;
            menuItems[1] = loadGameUnsel;
            menuItems[2] = optionsUnsel;
            menuItems[3] = creditsUnsel;

            /* Pad state stuff */
            prev_pad_state = GamePad.GetState(PlayerIndex.One);

            /* Load the fonts */
            kootenay = content.Load<SpriteFont>("fonts/Kootenay");
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
        public void Update(GameTime gameTime)
        {
            /* Keyboard and GamePad states */
            key_state = Keyboard.GetState();
            pad_state = GamePad.GetState(PlayerIndex.One);

            /* If we are on the title screen */
            switch (state)
            {
                case states.TITLE:
                    /* If the user hits up */
                    if (pad_state.IsButtonDown(Buttons.LeftThumbstickUp) &&
                        prev_pad_state.IsButtonUp(Buttons.LeftThumbstickUp) ||
                        key_state.IsKeyDown(Keys.Up) &&
                        prev_key_state.IsKeyUp(Keys.Up))
                    {
                        /* If we are not on the first element already */
                        if (current > 0)
                        {
                            GameSound.menuSound_rollover.Play();
                            /* Decrement current and change the images */
                            current--;
                            for (int i = 0; i < NUM_TITLE; i++)
                                menuItems[i] = unselMenuItems[i];
                            menuItems[current] = selMenuItems[current];
                        }
                    }
                    /* If the user hits the down button */
                    if (pad_state.IsButtonDown(Buttons.LeftThumbstickDown) &&
                        prev_pad_state.IsButtonUp(Buttons.LeftThumbstickDown) ||
                        key_state.IsKeyDown(Keys.Down) &&
                        prev_key_state.IsKeyUp(Keys.Down))
                    {
                        /* If we are on the last element in the menu */
                        if (current < NUM_TITLE - 1)
                        {
                            GameSound.menuSound_rollover.Play();
                            /* Increment current and update graphics */
                            current++;
                            for (int i = 0; i < NUM_TITLE; i++)
                                menuItems[i] = unselMenuItems[i];
                            menuItems[current] = selMenuItems[current];
                        }
                    }

                    /* If the user selects one of the menu items */
                    if (pad_state.IsButtonDown(Buttons.A) &&
                        prev_pad_state.IsButtonUp(Buttons.A) ||
                        key_state.IsKeyDown(Keys.Enter) &&
                        prev_key_state.IsKeyUp(Keys.Enter))
                    {
                        GameSound.menuSound_select.Play();
                        /* New Game */
                        if (current == 0)
                        {
                            /* Start the game */
                            GravityShiftMain.InMenu = false;
                            GravityShiftMain.InGame = true;
                        }
                        /* Load Game */
                        else if (current == 1)
                        {
                            /* Change to the load screen */
                            state = states.LOAD;

                            /* Initialize variables to the load menu items */
                            selMenuItems = new Texture2D[NUM_LOAD];
                            unselMenuItems = new Texture2D[NUM_LOAD];
                            menuItems = new Texture2D[NUM_LOAD];

                            selMenuItems[0] = oneSel;
                            selMenuItems[1] = twoSel;
                            selMenuItems[2] = threeSel;
                            selMenuItems[3] = backSel;

                            unselMenuItems[0] = oneUnsel;
                            unselMenuItems[1] = twoUnsel;
                            unselMenuItems[2] = threeUnsel;
                            unselMenuItems[3] = backUnsel;

                            menuItems[0] = oneSel;
                            menuItems[1] = twoUnsel;
                            menuItems[2] = threeUnsel;
                            menuItems[3] = backUnsel;

                            current = 0;
                        }
                        /* Options */
                        else if (current == 2)
                        {
                            /* Change to the options menu */
                            state = states.OPTIONS;

                            selMenuItems = new Texture2D[NUM_OPTIONS];
                            unselMenuItems = new Texture2D[NUM_OPTIONS];
                            menuItems = new Texture2D[NUM_OPTIONS];

                            selMenuItems[0] = controlSel;
                            selMenuItems[1] = soundSel;
                            selMenuItems[2] = backSel;

                            unselMenuItems[0] = controlUnsel;
                            unselMenuItems[1] = soundUnsel;
                            unselMenuItems[2] = backUnsel;

                            menuItems[0] = controlSel;
                            menuItems[1] = soundUnsel;
                            menuItems[2] = backUnsel;

                            current = 0;
                        }
                        /* Credits */
                        else if (current == 3)
                        {
                            /* Change to the credits */
                            state = states.CREDITS;

                            current = 0;
                        }
                    }
                    break;

                /* Options Menu*/
                case states.OPTIONS:
                    if (pad_state.IsButtonDown(Buttons.LeftThumbstickUp) &&
                        prev_pad_state.IsButtonUp(Buttons.LeftThumbstickUp) ||
                        key_state.IsKeyDown(Keys.Up) &&
                        prev_key_state.IsKeyUp(Keys.Up))
                    {
                        if (current > 0)
                        {
                            GameSound.menuSound_rollover.Play();
                            current--;
                            for (int i = 0; i < NUM_OPTIONS; i++)
                                menuItems[i] = unselMenuItems[i];
                            menuItems[current] = selMenuItems[current];
                        }
                    }
                    if (pad_state.IsButtonDown(Buttons.LeftThumbstickDown) &&
                        prev_pad_state.IsButtonUp(Buttons.LeftThumbstickDown) ||
                        key_state.IsKeyDown(Keys.Down) &&
                        prev_key_state.IsKeyUp(Keys.Down))
                    {
                        if (current < NUM_OPTIONS - 1)
                        {
                            GameSound.menuSound_rollover.Play();
                            current++;
                            for (int i = 0; i < NUM_OPTIONS; i++)
                                menuItems[i] = unselMenuItems[i];
                            menuItems[current] = selMenuItems[current];
                        }
                    }

                    if (pad_state.IsButtonDown(Buttons.A) &&
                        prev_pad_state.IsButtonUp(Buttons.A) ||
                        key_state.IsKeyDown(Keys.Enter) &&
                        prev_key_state.IsKeyUp(Keys.Enter))
                    {
                        GameSound.menuSound_select.Play();
                        /* Controller Settings */
                        if (current == 0)
                        {
                            state = states.CONTROLLER;
                        }
                        /* Sound Settings */
                        else if (current == 1)
                        {
                            state = states.SOUNDS;
                        }
                        /* Back */
                        else if (current == 2)
                        {
                            state = states.TITLE;

                            selMenuItems = new Texture2D[NUM_TITLE];
                            unselMenuItems = new Texture2D[NUM_TITLE];
                            menuItems = new Texture2D[NUM_TITLE];

                            selMenuItems[0] = newGameSel;
                            selMenuItems[1] = loadGameSel;
                            selMenuItems[2] = optionsSel;
                            selMenuItems[3] = creditsSel;

                            unselMenuItems[0] = newGameUnsel;
                            unselMenuItems[1] = loadGameUnsel;
                            unselMenuItems[2] = optionsUnsel;
                            unselMenuItems[3] = creditsUnsel;

                            menuItems[0] = newGameSel;
                            menuItems[1] = loadGameUnsel;
                            menuItems[2] = optionsUnsel;
                            menuItems[3] = creditsUnsel;

                            current = 0;
                        }
                    }
                    break;

                /* Load Menu */
                case states.LOAD:
                    if (pad_state.IsButtonDown(Buttons.LeftThumbstickUp) &&
                        prev_pad_state.IsButtonUp(Buttons.LeftThumbstickUp) ||
                        key_state.IsKeyDown(Keys.Up) &&
                        prev_key_state.IsKeyUp(Keys.Up))
                    {
                        if (current > 0)
                        {
                            GameSound.menuSound_rollover.Play();
                            current--;
                            for (int i = 0; i < NUM_LOAD; i++)
                                menuItems[i] = unselMenuItems[i];
                            menuItems[current] = selMenuItems[current];
                        }
                    }
                    if (pad_state.IsButtonDown(Buttons.LeftThumbstickDown) &&
                        prev_pad_state.IsButtonUp(Buttons.LeftThumbstickDown) ||
                        key_state.IsKeyDown(Keys.Down) &&
                        prev_key_state.IsKeyUp(Keys.Down))
                    {
                        if (current < NUM_LOAD - 1)
                        {
                            GameSound.menuSound_rollover.Play();
                            current++;
                            for (int i = 0; i < NUM_LOAD; i++)
                                menuItems[i] = unselMenuItems[i];
                            menuItems[current] = selMenuItems[current];
                        }
                    }

                    if (pad_state.IsButtonDown(Buttons.A) &&
                        prev_pad_state.IsButtonUp(Buttons.A) ||
                        key_state.IsKeyDown(Keys.Enter) &&
                        prev_key_state.IsKeyUp(Keys.Enter))
                    {
                        GameSound.menuSound_select.Play();
                        /* Level 1 */
                        if (current == 0)
                        {
                            /* TODO */
                        }
                        /* Level 2 */
                        else if (current == 1)
                        {
                            /* TODO */
                        }
                        /* Level 3 */
                        else if (current == 2)
                        {
                            /* TODO */
                        }
                        /* Back */
                        else if (current == 3)
                        {
                            /* Return back to the title screen */
                            state = states.TITLE;

                            selMenuItems = new Texture2D[NUM_TITLE];
                            unselMenuItems = new Texture2D[NUM_TITLE];
                            menuItems = new Texture2D[NUM_TITLE];

                            selMenuItems[0] = newGameSel;
                            selMenuItems[1] = loadGameSel;
                            selMenuItems[2] = optionsSel;
                            selMenuItems[3] = creditsSel;

                            unselMenuItems[0] = newGameUnsel;
                            unselMenuItems[1] = loadGameUnsel;
                            unselMenuItems[2] = optionsUnsel;
                            unselMenuItems[3] = creditsUnsel;

                            menuItems[0] = newGameSel;
                            menuItems[1] = loadGameUnsel;
                            menuItems[2] = optionsUnsel;
                            menuItems[3] = creditsUnsel;

                            current = 0;
                        }
                    }
                    break;

                /* Options Menu*/
                case states.CREDITS:
                    if (pad_state.IsButtonDown(Buttons.A) &&
                        prev_pad_state.IsButtonUp(Buttons.A) ||
                        key_state.IsKeyDown(Keys.Enter) &&
                        prev_key_state.IsKeyUp(Keys.Enter))
                    {
                        GameSound.menuSound_select.Play();
                        /* Back */
                        state = states.TITLE;

                        selMenuItems = new Texture2D[NUM_TITLE];
                        unselMenuItems = new Texture2D[NUM_TITLE];
                        menuItems = new Texture2D[NUM_TITLE];
                        
                        selMenuItems[0] = newGameSel;
                        selMenuItems[1] = loadGameSel;
                        selMenuItems[2] = optionsSel;
                        selMenuItems[3] = creditsSel;

                        unselMenuItems[0] = newGameUnsel;
                        unselMenuItems[1] = loadGameUnsel;
                        unselMenuItems[2] = optionsUnsel;
                        unselMenuItems[3] = creditsUnsel;

                        menuItems[0] = newGameSel;
                        menuItems[1] = loadGameUnsel;
                        menuItems[2] = optionsUnsel;
                        menuItems[3] = creditsUnsel;

                        current = 0;
                    }
                    break;

                /* Controller Settings */
                case states.CONTROLLER:
                    if (pad_state.IsButtonDown(Buttons.A) &&
                        prev_pad_state.IsButtonUp(Buttons.A) ||
                        key_state.IsKeyDown(Keys.Enter) &&
                        prev_key_state.IsKeyUp(Keys.Enter))
                    {
                        GameSound.menuSound_select.Play();

                        /* Change to the options menu */
                        state = states.OPTIONS;

                        selMenuItems = new Texture2D[NUM_OPTIONS];
                        unselMenuItems = new Texture2D[NUM_OPTIONS];
                        menuItems = new Texture2D[NUM_OPTIONS];

                        selMenuItems[0] = controlSel;
                        selMenuItems[1] = soundSel;
                        selMenuItems[2] = backSel;

                        unselMenuItems[0] = controlUnsel;
                        unselMenuItems[1] = soundUnsel;
                        unselMenuItems[2] = backUnsel;

                        menuItems[0] = controlSel;
                        menuItems[1] = soundUnsel;
                        menuItems[2] = backUnsel;

                        current = 0;
                    }

                    break;

                /* Sound Settings */
                case states.SOUNDS:
                    if (pad_state.IsButtonDown(Buttons.A) &&
                        prev_pad_state.IsButtonUp(Buttons.A) ||
                        key_state.IsKeyDown(Keys.Enter) &&
                        prev_key_state.IsKeyUp(Keys.Enter))
                    {
                        GameSound.menuSound_select.Play();
                        
                        /* Change to the options menu */
                        state = states.OPTIONS;

                        selMenuItems = new Texture2D[NUM_OPTIONS];
                        unselMenuItems = new Texture2D[NUM_OPTIONS];
                        menuItems = new Texture2D[NUM_OPTIONS];

                        selMenuItems[0] = controlSel;
                        selMenuItems[1] = soundSel;
                        selMenuItems[2] = backSel;

                        unselMenuItems[0] = controlUnsel;
                        unselMenuItems[1] = soundUnsel;
                        unselMenuItems[2] = backUnsel;

                        menuItems[0] = controlSel;
                        menuItems[1] = soundUnsel;
                        menuItems[2] = backUnsel;

                        current = 0;
                    }
                    break;
            }
            
            /* Set the previous states to the current states */
            prev_pad_state = pad_state;
            prev_key_state = key_state;
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
        public void Draw(SpriteBatch spriteBatch, GraphicsDeviceManager graphics)
        {
            spriteBatch.Begin();

            /* Draw the title of the game  and main background */
            spriteBatch.Draw(title, new Vector2(150.0f, 50.0f), Color.White);

            /* If on the title screen */
            switch (state)
            {
                case states.TITLE:
                    /* Draw the title items */
                    spriteBatch.Draw(menuItems[0], new Vector2(460.0f, 300.0f), Color.White);
                    spriteBatch.Draw(menuItems[1], new Vector2(445.0f, 400.0f), Color.White);
                    spriteBatch.Draw(menuItems[2], new Vector2(500.0f, 500.0f), Color.White);
                    spriteBatch.Draw(menuItems[3], new Vector2(510.0f, 600.0f), Color.White);
                    break;

                /* If on the load screen */
                case states.LOAD:
                    spriteBatch.Draw(menuItems[0], new Vector2(100.0f, 100.0f), Color.White);
                    spriteBatch.Draw(menuItems[1], new Vector2(100.0f, 300.0f), Color.White);
                    spriteBatch.Draw(menuItems[2], new Vector2(100.0f, 500.0f), Color.White);
                    spriteBatch.Draw(menuItems[3], new Vector2(900.0f, 700.0f), Color.White);
                    break;

                /* If on the options menu */
                case states.OPTIONS:
                    spriteBatch.Draw(menuItems[0], new Vector2(300.0f, 300.0f), Color.White);
                    spriteBatch.Draw(menuItems[1], new Vector2(300.0f, 400.0f), Color.White);
                    spriteBatch.Draw(menuItems[2], new Vector2(900.0f, 700.0f), Color.White);
                    break;

                case states.CREDITS:
                    spriteBatch.DrawString(kootenay, "Developed By:", new Vector2(400.0f, 375.0f), Color.White);
                    spriteBatch.DrawString(kootenay, "Lukas Black", new Vector2(400.0f, 425.0f), Color.White);
                    spriteBatch.DrawString(kootenay, "Nate Bradford", new Vector2(400.0f, 450.0f), Color.White);
                    spriteBatch.DrawString(kootenay, "Michael DeVico", new Vector2(400.0f, 475.0f), Color.White);
                    spriteBatch.DrawString(kootenay, "Steven Doxey", new Vector2(400.0f, 500.0f), Color.White);
                    spriteBatch.DrawString(kootenay, "Kamron Egan", new Vector2(400.0f, 525.0f), Color.White);
                    spriteBatch.DrawString(kootenay, "Jeremy Heintz", new Vector2(400.0f, 550.0f), Color.White);
                    spriteBatch.DrawString(kootenay, "Morgan Reynolds", new Vector2(400.0f, 575.0f), Color.White);
                    spriteBatch.DrawString(kootenay, "Tyler Robinson", new Vector2(400.0f, 600.0f), Color.White);
                    spriteBatch.DrawString(kootenay, "Casey Spencer", new Vector2(400.0f, 625.0f), Color.White);
                    spriteBatch.DrawString(kootenay, "Curtis Taylor", new Vector2(400.0f, 650.0f), Color.White);

                    spriteBatch.Draw(backSel, new Vector2(900.0f, 700.0f), Color.White);

                    break;

                /* If on the controller settings screen */
                case states.CONTROLLER:
                    spriteBatch.Draw(xboxControl, new Vector2(300.0f, 30.0f), Color.White);
                    spriteBatch.Draw(backSel, new Vector2(900.0f, 700.0f), Color.White);
                    /* TODO */
                    break;

                /* If on the sound settings screen */
                case states.SOUNDS:
                    spriteBatch.DrawString(kootenay, "Coming Soon", new Vector2(300.0f, 400.0f), Color.White);
                    spriteBatch.Draw(backSel, new Vector2(900.0f, 700.0f), Color.White);
                    /* TODO */
                    break;
            }
            spriteBatch.End();
        }
    }
}