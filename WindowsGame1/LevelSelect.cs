using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using System.Xml.Linq;
using GravityShift.Import_Code;
using System.IO;

namespace GravityShift
{
    /// <summary>
    /// Level selection menu that allows you to pick the level to start
    /// </summary>
    class LevelSelect
    {
        public static string LEVEL_DIRECTORY = "..\\..\\..\\Content\\Levels\\";
        public static string LEVEL_THUMBS_DIRECTORY = LEVEL_DIRECTORY + "Thumbnail\\";
        public static string LEVEL_LIST = LEVEL_DIRECTORY + "Info\\LevelList.xml";

        public static int BACK = 0;
        public static int PREVIOUS = 13;
        public static int NEXT = 14;

        IControlScheme mControls;

        XElement mLevelInfo;

        List<LevelChoice> mLevels;

        Texture2D mSelectBox;
        Texture2D[] mPrevious;
        Texture2D[] mNext;
        Texture2D[] mBack;
        Texture2D mBackground;
        
        int mCurrentIndex = 1;
        int mPageCount;
        int mCurrentPage = 0;

        Rectangle mScreenRect;

        ContentManager mContent;

        /* SpriteFont */
        SpriteFont mKootenay;

        /// <summary>
        /// Constructs the menu screen that allows the player to select a level
        /// </summary>
        /// <param name="controlScheme">Controls that the player are using</param>
        public LevelSelect(IControlScheme controlScheme)
        {
            mControls = controlScheme;
            mLevels = new List<LevelChoice>();
            mLevelInfo = XElement.Load(LEVEL_LIST);
        }

        /// <summary>
        /// Load the data that is needed to show the Level selection screen
        /// </summary>
        /// <param name="content">Access to the content of the project</param>
        /// <param name="graphics">Graphics that draws to the screen</param>
        public void Load(ContentManager content, GraphicsDevice graphics)
        {
            foreach (XElement level in mLevelInfo.Elements())
                mLevels.Add(new LevelChoice(level,mControls,graphics));

            mContent = content;
            mSelectBox = content.Load<Texture2D>("menu/selectbox");
            mKootenay = content.Load<SpriteFont>("fonts/kootenay");

            mBackground = content.Load<Texture2D>("Images/Backgrounds/Background5Resize");

            /*TODO - REMOVE THIS WHEN REAL ART COMES*/
            mPrevious = new Texture2D[2];
            mPrevious[0] = content.Load<Texture2D>("Images/LeftArrow");
            mPrevious[1] = content.Load<Texture2D>("Images/LeftArrowSelect");

            mNext = new Texture2D[2];
            mNext[0] = content.Load<Texture2D>("Images/RightArrow");
            mNext[1] = content.Load<Texture2D>("Images/RightArrowSelect");

            mBack = new Texture2D[2];
            mBack[0] = content.Load<Texture2D>("Images/Back");
            mBack[1] = content.Load<Texture2D>("Images/BackSelect");

            mPageCount = mLevels.Count / 12 +1;

            mScreenRect = graphics.Viewport.TitleSafeArea;
        }

        /// <summary>
        /// Handle any changes while on the level selection menu
        /// </summary>
        /// <param name="gameTime">Current time within the game</param>
        /// <param name="gameState">Current gamestate of the game</param>
        /// <param name="currentLevel">Current level of the game</param>
        public void Update(GameTime gameTime, ref GameStates gameState, ref Level currentLevel)
        {
            HandleDirectionKeys();          

            if(mControls.isAPressed(false)||mControls.isStartPressed(false))
                HandleAPressed(ref gameState,ref currentLevel);

            if (mControls.isBackPressed(false))
            {
                gameState = GameStates.Main_Menu;
                mCurrentPage = 0;
                mCurrentIndex = 1;
            }
        }

        /// <summary>
        /// Unlocks the next level in the game(if it is already unlocked than it won't do anything)
        /// </summary>
        public void UnlockNextLevel()
        {
            if (mCurrentIndex + 12 * mCurrentPage < mLevels.Count)
                mLevels[mCurrentIndex + 12 * mCurrentPage].Unlock();
        }

        /// <summary>
        /// Gets the next level in the game
        /// </summary>
        /// <returns>The next level in the game, or null if there is none</returns>
        public Level GetNextLevel()
        {
            if (mCurrentIndex + 12 * mCurrentPage < mLevels.Count)
            {
                if (++mCurrentIndex >= PREVIOUS) { mCurrentIndex = 1; mCurrentPage++; }
                return mLevels[mCurrentIndex - 1 + 12 * mCurrentPage].Level;
            }

            return null;
        }

        /// <summary>
        /// Handle actions for each direction the player may press on their controller
        /// </summary>
        private void HandleDirectionKeys()
        {
            Vector2 max = new Vector2(10, 11);

            int countOnPage = 11;
            if (mCurrentPage + 1 == mPageCount)
                countOnPage = (mLevels.Count - 1) % 12;

            int row = countOnPage / 4;
            int col = countOnPage % 4;

            max.X = max.Y = row * 4 + Math.Min(2,col);
            if (col == 1) max.Y = ++max.X;
            if(col >= 2) max.Y++;

            if (mControls.isLeftPressed(false))
            {
                mCurrentIndex = (mCurrentIndex - 1);
                if (mCurrentIndex + 12 * mCurrentPage > mLevels.Count && mCurrentIndex < PREVIOUS) mCurrentIndex = mLevels.Count % 12;
            }
            else if (mControls.isRightPressed(false))
            {
                mCurrentIndex = (mCurrentIndex + 1) % 15;
                if (mCurrentIndex + 12 * mCurrentPage > mLevels.Count && mCurrentIndex < PREVIOUS) mCurrentIndex = PREVIOUS;
            }
            else if (mControls.isUpPressed(false))
            {
                if (mCurrentIndex > BACK && mCurrentIndex <= 4) mCurrentIndex = BACK;
                else if (mCurrentIndex == BACK) mCurrentIndex = NEXT;
                else if (mCurrentIndex == NEXT) mCurrentIndex = (int)max.Y;
                else if (mCurrentIndex == PREVIOUS) mCurrentIndex = (int)max.X;
                else mCurrentIndex = (mCurrentIndex - 4);
            }
            else if (mControls.isDownPressed(false))
            {
                if (mCurrentIndex < countOnPage + 2 && mCurrentIndex > max.X) mCurrentIndex = NEXT;
                else if (mCurrentIndex < max.X + 1 && mCurrentIndex > row * 4) mCurrentIndex = PREVIOUS;
                else if (mCurrentIndex == BACK) mCurrentIndex = 1;
                else if (mCurrentIndex == NEXT || mCurrentIndex == PREVIOUS) mCurrentIndex = BACK;
                else mCurrentIndex = (mCurrentIndex+ 4);

                if (mCurrentIndex > row * 4 + col + 1 && mCurrentIndex < PREVIOUS) mCurrentIndex = row * 4 + col + 1;
 
            }

            if (mCurrentIndex < 0) mCurrentIndex += 15;  
        }

        /// <summary>
        /// Handle what happens when the player presses A for all options
        /// </summary>
        /// <param name="gameState">State of the game - Reference so that it can be changed for the main game class to handle</param>
        /// <param name="currentLevel">Current level in the main game - Reference so that this can be changed for the main game class to handle</param>
        private void HandleAPressed(ref GameStates gameState, ref Level currentLevel)
        {
            if (mCurrentIndex == BACK)
            {
                gameState = GameStates.Main_Menu;
                mCurrentPage = 0;
                mCurrentIndex = 1;
            }
            else if (mCurrentIndex == PREVIOUS)
            {
                if (--mCurrentPage < 0) mCurrentPage = 0;
            }
            else if (mCurrentIndex == NEXT)
            {
                if (++mCurrentPage == mPageCount) mCurrentPage = mPageCount - 1;
            }
            else if(mLevels[mCurrentIndex - 1 + 12 * mCurrentPage].Unlocked)
            {
                currentLevel = mLevels[mCurrentIndex - 1 + 12 * mCurrentPage].Level;
                currentLevel.Load(mContent);
                gameState = GameStates.In_Game;
            }
        }

        /// <summary>
        /// Draws the menu on the screen
        /// </summary>
        /// <param name="spriteBatch">Canvas we are drawing to</param>
        /// <param name="graphics">Information on the device's graphics</param>
        public void Draw(SpriteBatch spriteBatch, GraphicsDeviceManager graphics)
        {
            spriteBatch.Begin();

            spriteBatch.Draw(mBackground, mScreenRect, Color.White);

            Vector2 size = new Vector2(this.mScreenRect.Width / 4, this.mScreenRect.Height / 3);
            Vector2 padding = new Vector2(size.X * .20f, size.Y * .20f);

            Vector2 stringLoc = mKootenay.MeasureString((mCurrentPage + 1) + "/" + mPageCount);

            spriteBatch.Draw(mBack[Convert.ToInt32(mCurrentIndex == BACK)] , new Vector2(mScreenRect.Left, mScreenRect.Top), Color.White);
            spriteBatch.Draw(mPrevious[Convert.ToInt32(mCurrentIndex == PREVIOUS)], new Vector2(mScreenRect.Center.X - 75, mScreenRect.Bottom - 75), Color.White);
            spriteBatch.DrawString(mKootenay, (mCurrentPage + 1) + "/" + mPageCount, new Vector2(mScreenRect.Center.X + 10,mScreenRect.Bottom - 60), Color.White);
            spriteBatch.Draw(mNext[Convert.ToInt32(mCurrentIndex == NEXT)], new Vector2(mScreenRect.Center.X + 75, mScreenRect.Bottom - 75), Color.White);

            size.X -= 2*padding.X;
            size.Y -= 2*padding.Y;
            
            Vector2 currentLocation = new Vector2(mScreenRect.X, 2*padding.Y);
            int index = 0;

            for (int i = 0; i < 12 && i + 12 * mCurrentPage < mLevels.Count; i++)
            {
                if (currentLocation.X + size.X + padding.X >= graphics.GraphicsDevice.Viewport.Width)
                {
                    currentLocation.X = 0;
                    currentLocation.Y += padding.Y + size.Y;
                }
                currentLocation.X += padding.X;
                Rectangle rect = new Rectangle((int)currentLocation.X, (int)currentLocation.Y, (int)size.X, (int)size.Y);

                spriteBatch.Draw(mLevels[i + 12 * mCurrentPage].Thumbnail, rect, Color.White);

                Vector2 stringSize = mKootenay.MeasureString(mLevels[i + 12 * mCurrentPage].Level.Name);
                Vector2 stringLocation = new Vector2(rect.Center.X - stringSize.X/2, rect.Top - stringSize.Y);
                spriteBatch.DrawString(mKootenay, mLevels[i + 12 * mCurrentPage].Level.Name, stringLocation, Color.White);
                if (index == mCurrentIndex - 1) spriteBatch.Draw(mSelectBox, rect, Color.White);

                stringSize = mKootenay.MeasureString("Locked");
                if (!mLevels[i + 12 * mCurrentPage].Unlocked) spriteBatch.DrawString(mKootenay,"Locked",
                    new Vector2(rect.Center.X - stringSize.X/2,rect.Center.Y - stringSize.Y/2),Color.White);//DRAW LOCKED SYMBOL

                currentLocation.X += size.X + padding.X;
                index++;
            }

            spriteBatch.End();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class LevelChoice
    {
        private Level mLevel;
        private Texture2D mThumbnail;
        private bool mUnlocked = false;

        public Level Level
        { get { return mLevel; } }
        public Texture2D Thumbnail
        { get { return mThumbnail; } }
        public bool Unlocked
        { get { return mUnlocked; } }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="levelInfo"></param>
        /// <param name="controls"></param>
        /// <param name="graphics"></param>
        public LevelChoice(XElement levelInfo, IControlScheme controls, GraphicsDevice graphics)
        {
            foreach(XElement element in levelInfo.Elements())
            {
                if (element.Name == "name")
                {
                    mLevel = new Level(LevelSelect.LEVEL_DIRECTORY + element.Value.ToString() + ".xml", controls, graphics.Viewport);
                    
                    FileStream filestream = new FileStream(LevelSelect.LEVEL_THUMBS_DIRECTORY + element.Value.ToString() + ".png", FileMode.Open);

                    mThumbnail = Texture2D.FromStream(graphics,filestream);
                    filestream.Close();
                }
                if (element.Name == "unlocked")
                    mUnlocked = element.Value == Import_Code.XmlKeys.TRUE;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Unlock()
        {
            mUnlocked = true;
        }
    }
}
