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
    class Scoring
    {
        #region Member Variables

        /* Spritefond */
        private SpriteFont kootenay;

        IControlScheme mControls;

        /* Equivalent of stars */
        private static Texture2D[] num_apples;
        private const int POSSIBLE_APPLES = 3;

        /* Keep track of the level */
        private static int[,] level;
        private static int[,] score;

        private Texture2D[] selItems;
        private Texture2D[] unselItems;
        private Texture2D[] Items;

        private int current;

        private const int NUM_OPTIONS = 2;

        #endregion

        #region Art

        private Texture2D apple;
        private Texture2D apple_gray;

        private Texture2D backUnsel;
        private Texture2D backSel;
        private Texture2D restartSel;
        private Texture2D restartUnsel;

        private Texture2D title;

        #endregion

        #region Getters and Setters

        /* Getter/Setter for the apples variable */
//        public static int[] Apples
//        {
//            get { return num_apples; }
//            set { num_apples = value; }
//        }

        /* Getter/Setter for the level variable */
        public static int[,] Level
        {
            get { return level; }
            set { level = value; }
        }

        /* Getter/Setter for the score variable */
        public static int[,] Score
        {
            get { return score; }
            set { score = value; }
        }

        #endregion

        public Scoring(IControlScheme controls) 
        {
            mControls = controls;
        }

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
            kootenay = content.Load<SpriteFont>("fonts/Kootenay");

            apple = content.Load<Texture2D>("scoring/apple");
            apple_gray = content.Load<Texture2D>("scoring/apple_gray");

            /* Set up with the number of worlds and levels */
            /* For now we have 1 world with 3 levels */
            level = new int[1, 3];
            score = new int[1, 3];
            num_apples = new Texture2D[3];

            num_apples[0] = apple;
            num_apples[1] = apple;
            num_apples[2] = apple;

            current = 0;

            selItems = new Texture2D[NUM_OPTIONS];
            unselItems = new Texture2D[NUM_OPTIONS];
            Items = new Texture2D[NUM_OPTIONS];

            backUnsel = content.Load<Texture2D>("menu/BackUnselected");
            backSel = content.Load<Texture2D>("menu/BackSelected");
        
            restartUnsel = content.Load<Texture2D>("menu/RestartUnselected");
            restartSel = content.Load<Texture2D>("menu/RestartSelected");

            title = content.Load<Texture2D>("menu/title");

            selItems[0] = restartSel;
            selItems[1] = backSel;

            unselItems[0] = restartUnsel;
            unselItems[1] = backUnsel;

            Items[0] = restartSel;
            Items[1] = backUnsel;
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
        public void Update(GameTime gameTime, ref GameStates gameState)
        {

            /* If the user hits up */
            if (mControls.isUpPressed(false))
            {
                /* If we are not on the first element already */
                if (current > 0)
                {
                    GameSound.menuSound_rollover.Play(GameSound.volume, 0.0f, 0.0f);
                    /* Decrement current and change the images */
                    current--;
                    for (int i = 0; i < NUM_OPTIONS; i++)
                        Items[i] = unselItems[i];
                    Items[current] = selItems[current];
                }
            }
            /* If the user hits the down button */
            if (mControls.isDownPressed(false))
            {
                /* If we are on the last element in the menu */
                if (current < NUM_OPTIONS - 1)
                {
                    GameSound.menuSound_rollover.Play(GameSound.volume, 0.0f, 0.0f);
                    /* Increment current and update graphics */
                    current++;
                    for (int i = 0; i < NUM_OPTIONS; i++)
                        Items[i] = unselItems[i];
                    Items[current] = selItems[current];
                }
            }

            /* If the user selects one of the menu items */
            if (mControls.isAPressed(false) || mControls.isStartPressed(false))
            {
                GameSound.menuSound_select.Play(GameSound.volume, 0.0f, 0.0f);
                /* Restart Game */
                if (current == 0)
                {
                    /* Start the game*/
                    gameState = GameStates.In_Game;
                    current = 0;
                }
                /* Back Game */
                else if (current == 1)
                {
                    /*Back To Level Selection*/
                    gameState = GameStates.Level_Selection;

                    current = 0;
                }
            }
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

            spriteBatch.Draw(title, new Vector2(150.0f, 50.0f), Color.White);

            //spriteBatch.DrawString(kootenay, "Time: " + (int)GravityShiftMain.Timer + " Seconds", new Vector2(350.0f, 350.0f), Color.White);
            spriteBatch.Draw(num_apples[0], new Vector2(350.0f, 400.0f), Color.White);
            spriteBatch.Draw(num_apples[1], new Vector2(425.0f, 400.0f), Color.White);
            spriteBatch.Draw(num_apples[2], new Vector2(500.0f, 400.0f), Color.White);

            spriteBatch.Draw(Items[0], new Vector2(900.0f, 600.0f), Color.White);
            spriteBatch.Draw(Items[1], new Vector2(900.0f, 675.0f), Color.White);

            spriteBatch.End();
        }
    }
}
