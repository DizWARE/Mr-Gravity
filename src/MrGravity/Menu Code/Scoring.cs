using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MrGravity.MISC_Code;

namespace MrGravity.Menu_Code
{
    internal class Scoring
    {
        #region Member Variables

        /* Spritefont */
        private SpriteFont _mKootenay;
        private SpriteFont _mQuartz;

        private Rectangle _mScreenRect;

        private ContentManager _mContent;

        private readonly IControlScheme _mControls;

        private WorldSelect _mWorldSelect;

        /* Keep track of the level */

        #endregion

        #region Art

        private Texture2D _mTitle;
        private Texture2D _mBackground;
        private Texture2D _mStar;

        #endregion

        #region Getters and Setters

        /* Getter/Setter for the apples variable */
//        public static int[] Apples
//        {
//            get { return num_apples; }
//            set { num_apples = value; }
//        }

        /* Getter/Setter for the level variable */
        public static int[,] Level { get; set; }

        /* Getter/Setter for the score variable */
        public static int[,] Score { get; set; }

        #endregion

        public Scoring(IControlScheme controls) 
        {
            _mControls = controls;
        }

        /*
         * Load
         *
         * Similar to a loadContent function. This function loads and 
         * initializes the variable and art used in the class.
         *
         * ContentManager content: the Content file used in the game.
         */
        public void Load(ContentManager content, GraphicsDevice graphics, WorldSelect worldSelect)
        {
            _mWorldSelect = worldSelect;
            _mContent = content;
            _mKootenay = content.Load<SpriteFont>("Fonts/Kootenay");
            _mQuartz = content.Load<SpriteFont>("Fonts/QuartzLarge");

            _mScreenRect = graphics.Viewport.TitleSafeArea;

            _mTitle = content.Load<Texture2D>("Images/Menu/Mr_Gravity");
            _mBackground = content.Load<Texture2D>("Images/Menu/backgroundSquares1");
            _mStar = content.Load<Texture2D>("Images/AnimatedSprites/YellowStar");
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
        public void Update(GameTime gameTime, ref GameStates gameState, ref Level level, WorldSelect worldSelect)
        {

            if (_mControls.IsStartPressed(false) || _mControls.IsAPressed(false))
            {
                gameState = GameStates.AfterScore;
//#if XBOX360
//                mSelect.Save(((ControllerControl)mControls).ControllerIndex);
//#else
//                mSelect.Save(PlayerIndex.One);
//#endif
            }            
        }
        /* GetRank
         * 
         * int time: time to complete level
         * 
         * int timeGoal: goal time
         * 
         * int collect: collectables received
         * 
         * int collectGoal: Total collectables in level
         * 
         * int deathTotal: number of deaths
         * 
         * return int[] (number of stars-- 0=Bad, 1=Okay, 2=Good, 3=Perfect): [Time, Collectables, Death]
         
        public int[] GetRank(int time, int timeGoal, int collect, int collectGoal, int deathTotal)
        {
            int[] result = new int[3];

            // TIME -- 100%+, <120%, <140%, >140% 
            if (time < timeGoal) 
            { result[0] = 3; }
            else if (((double) time / (double) timeGoal) < 1.2) { result[0] = 2; }
            else { result[0] = 1; }

            // COLLECTABLES -- 100%, >80%, >60%, <60% 
            if (collect == collectGoal) { result[1] = 3; }
            else if (((double) collect / (double) collectGoal) > 0.8) { result[1] = 2; }
            else { result[1] = 1; }

            // DEATHS -- 0, 1, 2-3, >3 //
            if (deathTotal == 0) { result[2] = 3; }
            else if (deathTotal >= 2) { result[2] = 2; }
            else { result[2] = 1; }

            return result;
        }*/

        /*
         * Draw
         *
         * This function will draw the current menu
         *
         * SpriteBatch spriteBatch: The current sprite batch used to draw
         * 
         * GraphicsDeviceManager graphics: The current graphics manager
         */
        public void Draw(SpriteBatch spriteBatch, GraphicsDeviceManager graphics, Level level, Matrix scale)
        {
            spriteBatch.Begin(SpriteSortMode.Immediate,
                BlendState.AlphaBlend,
                SamplerState.LinearClamp,
                DepthStencilState.None,
                RasterizerState.CullCounterClockwise,
                null,
                scale);

            // TODO - CHANGE TO DYNAMIC PLACEMENT

            float textLength = _mQuartz.MeasureString("Deaths:").Length();
            float bufferLength = _mQuartz.MeasureString("999/999  ").Length();

            var topPadding = _mScreenRect.Top + _mScreenRect.Height / 4;
            int[] attempt = {level.TimerStar, level.CollectionStar, level.DeathStar};
            int[] previous = { _mWorldSelect.GetLevelTime(), _mWorldSelect.GetLevelCollect(), _mWorldSelect.GetLevelDeath() };
            //attempt = GetRank((int)level.mTimer, (int)level.IdealTime, (int)GravityShift.Level.mNumCollected, (int)GravityShift.Level.mNumCollectable, (int)GravityShift.Level.mDeaths);
            spriteBatch.Draw(_mBackground, new Rectangle(0, 0, graphics.GraphicsDevice.Viewport.Width, graphics.GraphicsDevice.Viewport.Height), Color.White);
            spriteBatch.Draw(_mTitle, new Vector2(_mScreenRect.Left + (_mScreenRect.Width - _mTitle.Width) / 2, _mScreenRect.Top), Color.White);

            spriteBatch.DrawString(_mQuartz, "Time:", new Vector2(_mScreenRect.Left + (_mScreenRect.Width / 6), topPadding), Color.White);
            spriteBatch.DrawString(_mQuartz, "Time:", new Vector2(_mScreenRect.Left + (_mScreenRect.Width / 6) + 1, topPadding + 1), Color.SteelBlue);
            spriteBatch.DrawString(_mQuartz, (int)level.MTimer + " Sec", new Vector2(_mScreenRect.Left + (_mScreenRect.Width / 6) + textLength + 20, topPadding), Color.White);

            //Draw Stars
            if (attempt[0] >= 1)
                spriteBatch.Draw(_mStar, new Vector2(_mScreenRect.Left + (_mScreenRect.Width / 6) + textLength + bufferLength, topPadding), null, Color.White, 0.0f, Vector2.Zero, 0.75f, SpriteEffects.None, 0.0f);
            if (attempt[0] >= 2)
                spriteBatch.Draw(_mStar, new Vector2(_mScreenRect.Left + (_mScreenRect.Width / 6) + textLength + bufferLength + (0.5f * (_mStar.Width)), topPadding), null, Color.White, 0.0f, Vector2.Zero, 0.75f, SpriteEffects.None, 0.0f);
            if (attempt[0] == 3)
                spriteBatch.Draw(_mStar, new Vector2(_mScreenRect.Left + (_mScreenRect.Width / 6) + textLength + bufferLength + (0.5f * (2 * _mStar.Width)), topPadding), null, Color.White, 0.0f, Vector2.Zero, 0.75f, SpriteEffects.None, 0.0f);

            level.TimerStar = attempt[0];

            if (previous[0] > attempt[0])
            {
                spriteBatch.DrawString(_mQuartz, "Your Best: " + previous[0] + "/3", new Vector2(_mScreenRect.Left + (11 * _mScreenRect.Width / 16), topPadding), Color.White);
                spriteBatch.DrawString(_mQuartz, "Your Best: " + previous[0] + "/3", new Vector2(_mScreenRect.Left + (11 * _mScreenRect.Width / 16) + 1, topPadding + 1), Color.SteelBlue);

            }
            else if(attempt[0] < 3)
            {
                spriteBatch.DrawString(_mQuartz, "Best So Far!", new Vector2(_mScreenRect.Left + (11 * _mScreenRect.Width / 16), topPadding), Color.White);
                spriteBatch.DrawString(_mQuartz, "Best So Far!", new Vector2(_mScreenRect.Left + (11 * _mScreenRect.Width / 16) + 1, topPadding + 1), Color.SteelBlue);
            }

            else
            {
                spriteBatch.DrawString(_mQuartz, "Perfect!", new Vector2(_mScreenRect.Left + (11 * _mScreenRect.Width / 16), topPadding), Color.White);
                spriteBatch.DrawString(_mQuartz, "Perfect!", new Vector2(_mScreenRect.Left + (11 * _mScreenRect.Width / 16) + 1, topPadding + 1), Color.SteelBlue);
            }
            
            topPadding += 65;

            spriteBatch.DrawString(_mQuartz, "Gems:", new Vector2(_mScreenRect.Left + (_mScreenRect.Width / 6), topPadding), Color.White);
            spriteBatch.DrawString(_mQuartz, "Gems:", new Vector2(_mScreenRect.Left + (_mScreenRect.Width / 6) + 1, topPadding + 1), Color.SteelBlue);
            spriteBatch.DrawString(_mQuartz, MrGravity.Level.MNumCollected + " / " + MrGravity.Level.MNumCollectable, new Vector2(_mScreenRect.Left + (_mScreenRect.Width / 6) + textLength + 20, topPadding), Color.White);

            if (attempt[1] >= 1)
                spriteBatch.Draw(_mStar, new Vector2(_mScreenRect.Left + (_mScreenRect.Width / 6) + textLength + bufferLength, topPadding), null, Color.White, 0.0f, Vector2.Zero, 0.75f, SpriteEffects.None, 0.0f);
            if (attempt[1] >= 2)
                spriteBatch.Draw(_mStar, new Vector2(_mScreenRect.Left + (_mScreenRect.Width / 6) + textLength + bufferLength + (0.5f * (_mStar.Width)), topPadding), null, Color.White, 0.0f, Vector2.Zero, 0.75f, SpriteEffects.None, 0.0f);
            if (attempt[1] == 3)
                spriteBatch.Draw(_mStar, new Vector2(_mScreenRect.Left + (_mScreenRect.Width / 6) + textLength + bufferLength + (0.5f * (2 * _mStar.Width)), topPadding), null, Color.White, 0.0f, Vector2.Zero, 0.75f, SpriteEffects.None, 0.0f);

            level.CollectionStar = attempt[1];
            if (previous[1] > attempt[1])
            {
                spriteBatch.DrawString(_mQuartz, "Your Best: " + previous[0] + "/3", new Vector2(_mScreenRect.Left + (11 * _mScreenRect.Width / 16), topPadding), Color.White);
                spriteBatch.DrawString(_mQuartz, "Your Best: " + previous[0] + "/3", new Vector2(_mScreenRect.Left + (11 * _mScreenRect.Width / 16) + 1, topPadding + 1), Color.SteelBlue);

            }
            else if (attempt[1] < 3)
            {
                spriteBatch.DrawString(_mQuartz, "Best So Far!", new Vector2(_mScreenRect.Left + (11 * _mScreenRect.Width / 16), topPadding), Color.White);
                spriteBatch.DrawString(_mQuartz, "Best So Far!", new Vector2(_mScreenRect.Left + (11 * _mScreenRect.Width / 16) + 1, topPadding + 1), Color.SteelBlue);
            }

            else
            {
                spriteBatch.DrawString(_mQuartz, "Perfect!", new Vector2(_mScreenRect.Left + (11 * _mScreenRect.Width / 16), topPadding), Color.White);
                spriteBatch.DrawString(_mQuartz, "Perfect!", new Vector2(_mScreenRect.Left + (11 * _mScreenRect.Width / 16) + 1, topPadding + 1), Color.SteelBlue);
            }   
            
            topPadding += 65;

            spriteBatch.DrawString(_mQuartz, "Deaths:", new Vector2(_mScreenRect.Left + (_mScreenRect.Width / 6), topPadding), Color.White);
            spriteBatch.DrawString(_mQuartz, "Deaths:", new Vector2(_mScreenRect.Left + (_mScreenRect.Width / 6) + 1, topPadding + 1), Color.SteelBlue);
            spriteBatch.DrawString(_mQuartz, "" + MrGravity.Level.MDeaths, new Vector2(_mScreenRect.Left + (_mScreenRect.Width / 6) + textLength + 20, topPadding), Color.White);
           
            //Draw Stars
            if (attempt[2] >= 1)
                spriteBatch.Draw(_mStar, new Vector2(_mScreenRect.Left + (_mScreenRect.Width / 6) + textLength + bufferLength, topPadding), null, Color.White, 0.0f, Vector2.Zero, 0.75f, SpriteEffects.None, 0.0f);
            if (attempt[2] >= 2)
                spriteBatch.Draw(_mStar, new Vector2(_mScreenRect.Left + (_mScreenRect.Width / 6) + textLength + bufferLength + (0.5f * (_mStar.Width)), topPadding), null, Color.White, 0.0f, Vector2.Zero, 0.75f, SpriteEffects.None, 0.0f);
            if (attempt[2] == 3)
                spriteBatch.Draw(_mStar, new Vector2(_mScreenRect.Left + (_mScreenRect.Width / 6) + textLength + bufferLength + (0.5f * (2 * _mStar.Width)), topPadding), null, Color.White, 0.0f, Vector2.Zero, 0.75f, SpriteEffects.None, 0.0f);

            level.DeathStar = attempt[2];

            if (previous[2] > attempt[2])
            {
                spriteBatch.DrawString(_mQuartz, "Your Best: " + previous[0] + "/3", new Vector2(_mScreenRect.Left + (11 * _mScreenRect.Width / 16), topPadding), Color.White);
                spriteBatch.DrawString(_mQuartz, "Your Best: " + previous[0] + "/3", new Vector2(_mScreenRect.Left + (11 * _mScreenRect.Width / 16) + 1, topPadding + 1), Color.SteelBlue);

            }
            else if (attempt[2] < 3)
            {
                spriteBatch.DrawString(_mQuartz, "Best So Far!", new Vector2(_mScreenRect.Left + (11 * _mScreenRect.Width / 16), topPadding), Color.White);
                spriteBatch.DrawString(_mQuartz, "Best So Far!", new Vector2(_mScreenRect.Left + (11 * _mScreenRect.Width / 16) + 1, topPadding + 1), Color.SteelBlue);
            }

            else
            {
                spriteBatch.DrawString(_mQuartz, "Perfect!", new Vector2(_mScreenRect.Left + (11 * _mScreenRect.Width / 16), topPadding), Color.White);
                spriteBatch.DrawString(_mQuartz, "Perfect!", new Vector2(_mScreenRect.Left + (11 * _mScreenRect.Width / 16) + 1, topPadding + 1), Color.SteelBlue);
            }
               
            var request = "Press Start or A To Access The Menu";

            Vector2 stringSize = _mQuartz.MeasureString(request);

            spriteBatch.DrawString(_mQuartz, request, new Vector2(_mScreenRect.Center.X - (stringSize.X / 2), _mScreenRect.Bottom - (stringSize.Y) * 2), Color.SteelBlue);
            spriteBatch.DrawString(_mQuartz, request, new Vector2(_mScreenRect.Center.X - (stringSize.X / 2) + 2, _mScreenRect.Bottom - (stringSize.Y) * 2 + 2), Color.White);

            spriteBatch.End();
        }
    }
}
