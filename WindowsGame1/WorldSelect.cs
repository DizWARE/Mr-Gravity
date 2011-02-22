using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Linq;
using GravityShift.Import_Code;
using System.IO;

namespace GravityShift
{
    /// <summary>
    /// Level selection menu that allows you to pick the level to start
    /// </summary>
    class WorldSelect
    {        
        private const int NAME_REGION = 0;
        private const int STARS_REGION = 1;
        private const int TIMER_REGION = 2;
        private const int DEATH_REGION = 3;

        private const int BACK = 0;
        private const int NEXT = 8;
        private const int PREVIOUS = 7;

        private const int NUM_OF_WORLDS = 8;

        IControlScheme mControls;
        GraphicsDeviceManager mGraphics;
        ContentManager mContent;

        Rectangle mScreenRect;

        Rectangle mTitleBar;
        Rectangle mBottomBar;
        Rectangle mInfoBar;
        Rectangle mLevelPanel;

        Rectangle[] mLevelRegions;
        Rectangle[] mInfoRegions;

        string[] mWorlds = 
        { "The Ropes", "Rail Shark", "Free Motion", "Two-Sides", "Old School", "Putting it Together", "Insanity", "Good Luck" };
        int mLongestName;

        List<LevelInfo> mLevels;
        XElement mLevelInfo;

        Vector2 mPadding;

        SpriteFont mFont;

        Texture2D[] mIcons;
        Texture2D[] mBack;
        Texture2D[] mPrevious;
        Texture2D[] mNext;

        Texture2D mBackground;
        Texture2D mTitle;
        Texture2D mStar;
        Texture2D mSelection;

        int mCurrentIndex = 1;
        int mCurrentWorld = 0;

        /// <summary>
        /// Constructs the menu screen that allows the player to select a level
        /// </summary>
        /// <param name="controlScheme">Controls that the player are using</param>
        public WorldSelect(IControlScheme controlScheme, GraphicsDeviceManager graphics)
        {
            mControls = controlScheme;
            mGraphics = graphics;

            CreateRegions();

            mIcons = new Texture2D[6];
            mNext = new Texture2D[2];
            mPrevious = new Texture2D[2];
            mBack = new Texture2D[2];

            mLevels = new List<LevelInfo>();
#if XBOX360
#else
            mLevelInfo = XElement.Load("..\\..\\..\\Content\\Levels\\Info\\LevelList.xml");
#endif
        }

        /// <summary>
        /// Creates all the regions for the level editor for easy placement within the Title Safe area
        /// </summary>
        private void CreateRegions()
        {
            mScreenRect = mGraphics.GraphicsDevice.Viewport.TitleSafeArea;

            mPadding = new Vector2(mScreenRect.Width / 100, mScreenRect.Height / 100);

            mLevelPanel = mInfoBar = mTitleBar = mBottomBar = new Rectangle();

            //Title bar, bottom bar, and level panel is 2/3 the x size of the title safe area
            mTitleBar.Width = mBottomBar.Width = mLevelPanel.Width = (3 * mScreenRect.Width) / 4;

            //The properties panel is 1/3 the x size of the title safe area
            mInfoBar.Width = mScreenRect.Width / 4;
            mInfoBar.Height = mScreenRect.Height;

            //Set the bottom and title bar to 1/8 of the title safe y area. 
            //Set the level selection area to be 3/4 of the title safe y area
            mLevelPanel.Height = mScreenRect.Height - 2 * (mBottomBar.Height = mTitleBar.Height = mScreenRect.Height / 8);

            //Sets the x and y location of all the regions
            mLevelPanel.X = mBottomBar.X = mTitleBar.X = mScreenRect.Left;
            mInfoBar.Y = mTitleBar.Y = mScreenRect.Top;
            mLevelPanel.Y = mTitleBar.Y + mTitleBar.Height;
            mBottomBar.Y = mLevelPanel.Y + mLevelPanel.Height;

            mInfoBar.X = mLevelPanel.X + mLevelPanel.Width;

            mLevelRegions = new Rectangle[6];
            mInfoRegions = new Rectangle[4];

            //Create 6 regions for image icons. The x direction is split into 3rds, which that region is 2/3rds of that area
            for (int i = 0; i < mLevelRegions.Length; i++)
            {
                int width = mLevelPanel.Width / 3;
                int height = mLevelPanel.Height / 2;
                mLevelRegions[i] = new Rectangle(mLevelPanel.Left + width * (i % 3), mLevelPanel.Top + height * (i / 3), width, height);
            }

            //Creates the regions for the Level information
            mInfoRegions[NAME_REGION] = new Rectangle(mInfoBar.Left, mInfoBar.Top, mInfoBar.Width, mInfoBar.Height / 8);
            for (int i = 1; i < mInfoRegions.Length; i++)
                mInfoRegions[i] = new Rectangle(mInfoBar.Left, mInfoBar.Top + mInfoRegions[NAME_REGION].Height + ((i-1) * 7 * mInfoBar.Height / 24), 
                    mInfoBar.Width, 7 * mInfoBar.Height / 24);
        }

        /// <summary>
        /// Loads all the content needed for the levels
        /// </summary>
        /// <param name="content"></param>
        public void Load(ContentManager content)
        {
            mContent = content;

            mBack[0] = content.Load<Texture2D>("Images/Menu/LevelSelect/Back");
            mBack[1] = content.Load<Texture2D>("Images/Menu/LevelSelect/BackSelect");

            mPrevious[0] = content.Load<Texture2D>("Images/Menu/LevelSelect/LeftArrow");
            mPrevious[1] = content.Load<Texture2D>("Images/Menu/LevelSelect/LeftArrowSelect");

            mNext[0] = content.Load<Texture2D>("Images/Menu/LevelSelect/RightArrow");
            mNext[1] = content.Load<Texture2D>("Images/Menu/LevelSelect/RightArrowSelect");

            mTitle = content.Load<Texture2D>("Images/Menu/SelectLevelSelected");
            mBackground = content.Load<Texture2D>("Images/Menu/backgroundSquares1");
            mStar = content.Load<Texture2D>("Images/NonHazards/YellowStar");

            mFont = content.Load<SpriteFont>("Fonts/QuartzSmaller");

            mLongestName = 0;
            foreach (string name in mWorlds)
                mLongestName = Math.Max((int)mFont.MeasureString(name).X, mLongestName);

            foreach (XElement level in mLevelInfo.Elements())
                mLevels.Add(new LevelInfo(level, content, mControls, mGraphics));
        }
        /// <summary>
        /// Handle any changes while on the level selection menu
        /// </summary>
        /// <param name="gameTime">Current time within the game</param>
        /// <param name="gameState">Current gamestate of the game</param>
        /// <param name="currentLevel">Current level of the game</param>
        public void Update(GameTime gameTime, ref GameStates gameState, ref Level currentLevel)
        {
            HandleAKey(ref gameState, ref currentLevel);
            HandleBKey(ref gameState);
            HandleDirectionKey();
        }

        /// <summary>
        /// Handles when the A key is pressed
        /// </summary>
        private void HandleAKey(ref GameStates gameState, ref Level currentLevel)
        {
            if (mControls.isAPressed(false) || mControls.isStartPressed(false))
            {
                if (mCurrentIndex == BACK)
                    Exit(ref gameState);
                else if (mCurrentIndex == PREVIOUS && mCurrentWorld > 0)
                    mCurrentWorld--;
                else if (mCurrentIndex == NEXT && mCurrentWorld < NUM_OF_WORLDS - 1)
                    mCurrentWorld++;
                else
                {
                   currentLevel = mLevels[mCurrentWorld * 6 + mCurrentIndex-1].Level;
                   currentLevel.Load(mContent);
                   gameState = GameStates.In_Game;
                }

                //Handle level select
            }
        }

        /// <summary>
        /// Handles when user presses the back key
        /// </summary>
        /// <param name="gameState"></param>
        private void HandleBKey(ref GameStates gameState)
        {
            if (mControls.isBPressed(false) || mControls.isBackPressed(false))
                Exit(ref gameState);
        }

        /// <summary>
        /// Exits the level selection
        /// </summary>
        /// <param name="gameState"></param>
        private void Exit(ref GameStates gameState)
        {
            mCurrentIndex = 1;
            gameState = GameStates.Main_Menu;
        }

        /// <summary>
        /// Handles the directional keys
        /// </summary>
        private void HandleDirectionKey()
        {
            //Down Button
            if (mControls.isDownPressed(false))
            {
                if (mCurrentIndex > BACK && mCurrentIndex < 4)
                    mCurrentIndex += 3;
                else if (mCurrentIndex == BACK)
                    mCurrentIndex = 1;
                else if (mCurrentIndex > 6)
                    mCurrentIndex = BACK;
                else
                    mCurrentIndex = PREVIOUS;
            }

            //Up Button
            if (mControls.isUpPressed(false))
            {
                if (mCurrentIndex > 3)
                    mCurrentIndex -= 3;
                else if (mCurrentIndex == BACK)
                    mCurrentIndex = PREVIOUS;
                else
                    mCurrentIndex = BACK;
            }

            //Left Pressed
            if (mControls.isLeftPressed(false))
            {
                mCurrentIndex--;
                if (mCurrentIndex == PREVIOUS && mCurrentWorld == 0)
                    mCurrentIndex--;
            }

            //Right Pressed
            if (mControls.isRightPressed(false))
            {
                mCurrentIndex++;
                if (mCurrentIndex == NEXT && mCurrentWorld == NUM_OF_WORLDS - 1)
                    mCurrentIndex++;
            }

            //Special cases
            if (mCurrentIndex < BACK || (mCurrentIndex == PREVIOUS && mCurrentWorld == 0))
                mCurrentIndex = NEXT;
            if (mCurrentIndex == NEXT && mCurrentWorld == NUM_OF_WORLDS - 1)
                mCurrentIndex = PREVIOUS;
            if (mCurrentIndex > NEXT)
                mCurrentIndex = BACK;

            //Page flipping
            if (mControls.isLeftShoulderPressed(false) && mCurrentWorld > 0)
                mCurrentWorld--;
            if (mControls.isRightShoulderPressed(false) && mCurrentWorld < NUM_OF_WORLDS - 1)
                mCurrentWorld++;
        }

        /// <summary>
        /// Draws the menu on the screen
        /// </summary>
        /// <param name="spriteBatch">Canvas we are drawing to</param>
        /// <param name="graphics">Information on the device's graphics</param>
        public void Draw(SpriteBatch spriteBatch, Matrix scale)
        {
            spriteBatch.Begin(SpriteSortMode.Immediate,
                BlendState.AlphaBlend,
                SamplerState.LinearClamp,
                DepthStencilState.None,
                RasterizerState.CullCounterClockwise,
                null,
                scale);

            spriteBatch.Draw(mBackground, mScreenRect, Color.White);

            DrawTitleBar(spriteBatch);
            DrawInfoBar(spriteBatch);
            DrawBottomBar(spriteBatch);
            DrawLevelPanel(spriteBatch);

            spriteBatch.End();
        }

        /// <summary>
        /// Draw the title bar to the screen
        /// </summary>
        /// <param name="spriteBatch"></param>
        private void DrawTitleBar(SpriteBatch spriteBatch)
        {
            float sizeRatio = (float)mBack[0].Width / mBack[0].Height;

            Rectangle backButtonRegion = new Rectangle(mTitleBar.Left + (int)mPadding.X,mTitleBar.Top + (int)mPadding.Y,
               (int)((mTitleBar.Height - 2 * mPadding.Y) * sizeRatio), (int)(mTitleBar.Height - 2 * mPadding.Y));
            
            
            Rectangle titleRegion = new Rectangle(mTitleBar.Center.X - mTitleBar.Width/4, mTitleBar.Top + (int)mPadding.Y,
                (int)((mTitleBar.Width/2 - 2 * mPadding.Y) ), (int)(mTitleBar.Height - 2 * mPadding.Y));

            spriteBatch.Draw(mBack[Convert.ToInt32(mCurrentIndex == BACK)], backButtonRegion, Color.White);
            spriteBatch.Draw(mTitle, titleRegion, Color.White);
        }

        /// <summary>
        /// Draw info bar on the right
        /// </summary>
        /// <param name="spriteBatch"></param>
        private void DrawInfoBar(SpriteBatch spriteBatch)
        {
            Vector2 size;

            if (mCurrentIndex > BACK && mCurrentIndex < PREVIOUS)
            {
                size = mFont.MeasureString("Name");
                //Draw Level Name - JUST TEMPLATE FOR NOW...NEED TO FIGURE OUT THE WHOLE NAME BUISNESS FIRST
                spriteBatch.DrawString(mFont, "Name", new Vector2(mInfoRegions[NAME_REGION].Center.X - size.X / 2 + 2, mInfoRegions[NAME_REGION].Center.Y - size.Y / 2 + 2), Color.LightBlue);
                spriteBatch.DrawString(mFont, "Name", new Vector2(mInfoRegions[NAME_REGION].Center.X - size.X / 2, mInfoRegions[NAME_REGION].Center.Y - size.Y / 2), Color.White);


                size = mFont.MeasureString(mLevels[mCurrentWorld * 6 + mCurrentIndex - 1].Name);
                spriteBatch.DrawString(mFont, mLevels[mCurrentWorld * 6 + mCurrentIndex - 1].Name, new Vector2(mInfoRegions[NAME_REGION].Center.X - size.X / 2, mInfoRegions[NAME_REGION].Bottom - size.Y), Color.White);


                int goalCollectables = mLevels[mCurrentWorld * 6 + mCurrentIndex - 1].GetGoal(LevelInfo.StarTypes.Collection);
                int goalTime = mLevels[mCurrentWorld * 6 + mCurrentIndex - 1].GetGoal(LevelInfo.StarTypes.Time);
                int goalDeaths = mLevels[mCurrentWorld * 6 + mCurrentIndex - 1].GetGoal(LevelInfo.StarTypes.Death);

                int starsCollectables = mLevels[mCurrentWorld * 6 + mCurrentIndex - 1].GetStar(LevelInfo.StarTypes.Collection);
                int starsTime = mLevels[mCurrentWorld * 6 + mCurrentIndex - 1].GetStar(LevelInfo.StarTypes.Time);
                int starsDeaths = mLevels[mCurrentWorld * 6 + mCurrentIndex - 1].GetStar(LevelInfo.StarTypes.Death);

                int starWidth = mStar.Width * 3;

                //Draw Stars Info
                size = mFont.MeasureString("Collectables");
                spriteBatch.DrawString(mFont, "Collectables", new Vector2(mInfoRegions[STARS_REGION].Center.X - size.X / 2 + 2, mInfoRegions[STARS_REGION].Top + mPadding.Y + 2), Color.LightBlue);
                spriteBatch.DrawString(mFont, "Collectables", new Vector2(mInfoRegions[STARS_REGION].Center.X - size.X / 2, mInfoRegions[STARS_REGION].Top + mPadding.Y), Color.White);

                size = mFont.MeasureString("Goal: " + goalCollectables + " Collectables");
                spriteBatch.DrawString(mFont, "Goal: " + goalCollectables + " Collectables", new Vector2(mInfoRegions[STARS_REGION].Center.X - size.X / 2, mInfoRegions[STARS_REGION].Top + mPadding.Y + 2*size.Y), Color.White);

                for (int i = 0; i < starsCollectables; i++)
                    spriteBatch.Draw(mStar, new Vector2(mInfoRegions[STARS_REGION].Center.X - starWidth/2 + i * mStar.Width, mInfoRegions[STARS_REGION].Top + mPadding.Y + 4 * size.Y), Color.White);
            

                //Draw Time Info
                size = mFont.MeasureString("Timer");
                spriteBatch.DrawString(mFont, "Timer", new Vector2(mInfoRegions[TIMER_REGION].Center.X - size.X / 2 + 2, mInfoRegions[TIMER_REGION].Top + mPadding.Y + 2), Color.LightBlue);
                spriteBatch.DrawString(mFont, "Timer", new Vector2(mInfoRegions[TIMER_REGION].Center.X - size.X / 2, mInfoRegions[TIMER_REGION].Top + mPadding.Y), Color.White);

                size = mFont.MeasureString("Goal: " + goalTime + " Seconds");
                spriteBatch.DrawString(mFont, "Goal: " + goalTime + " Seconds", new Vector2(mInfoRegions[TIMER_REGION].Center.X - size.X / 2, mInfoRegions[TIMER_REGION].Top + mPadding.Y + 2 * size.Y), Color.White);

                for (int i = 0; i < starsTime; i++)
                    spriteBatch.Draw(mStar, new Vector2(mInfoRegions[TIMER_REGION].Center.X - starWidth / 2 + i * mStar.Width, mInfoRegions[TIMER_REGION].Top + mPadding.Y + 4 * size.Y), Color.White);
            

                //Draw Death Info
                size = mFont.MeasureString("Death");
                spriteBatch.DrawString(mFont, "Death", new Vector2(mInfoRegions[DEATH_REGION].Center.X - size.X / 2 + 2, mInfoRegions[DEATH_REGION].Top + mPadding.Y + 2), Color.LightBlue);
                spriteBatch.DrawString(mFont, "Death", new Vector2(mInfoRegions[DEATH_REGION].Center.X - size.X / 2, mInfoRegions[DEATH_REGION].Top + mPadding.Y), Color.White);

                size = mFont.MeasureString("Goal: " + goalDeaths + " Deaths");
                spriteBatch.DrawString(mFont, "Goal: " + goalDeaths + " Deaths", new Vector2(mInfoRegions[DEATH_REGION].Center.X - size.X / 2, mInfoRegions[DEATH_REGION].Top + mPadding.Y + 2 * size.Y), Color.White);

                //size = mFont.MeasureString("You have " + starsDeaths + " stars");
                //spriteBatch.DrawString(mFont, "You have " + starsDeaths + " stars", new Vector2(mInfoRegions[DEATH_REGION].Center.X - size.X / 2, mInfoRegions[DEATH_REGION].Top + mPadding.Y + 4 * size.Y), Color.White);
                for(int i = 0; i < starsDeaths; i ++)
                    spriteBatch.Draw(mStar, new Vector2(mInfoRegions[DEATH_REGION].Center.X - starWidth / 2 + i * mStar.Width, mInfoRegions[DEATH_REGION].Top + mPadding.Y + 4 * size.Y), Color.White);
            }
        }

        /// <summary>
        /// Draws the bottom control bar
        /// </summary>
        /// <param name="spriteBatch"></param>
        private void DrawBottomBar(SpriteBatch spriteBatch)
        {            
            /***Subject to change :)***/
            Vector2 stringSize = mFont.MeasureString("-");
            Rectangle worldRegion = new Rectangle(mBottomBar.Center.X - (int)(mLongestName/2),mBottomBar.Center.Y - (int)(stringSize.Y/2),
                mLongestName, (int)stringSize.Y);

            float sizeRatio = (float)mPrevious[0].Width/mPrevious[0].Height;
            int height = (int)(5*mBottomBar.Height / 8 - mPadding.Y);

            Rectangle previousRegion = new Rectangle(worldRegion.Left - (int)mPadding.X - (int)(height * sizeRatio), 
                mBottomBar.Center.Y - height/2, (int)(height * sizeRatio),height);
            Rectangle nextRegion = new Rectangle(worldRegion.Right + (int)mPadding.X, 
                mBottomBar.Center.Y - height/2, (int)(height * sizeRatio),height);

            spriteBatch.DrawString(mFont, mWorlds[mCurrentWorld],
                new Vector2(worldRegion.Center.X - mFont.MeasureString(mWorlds[mCurrentWorld]).X / 2, worldRegion.Top), Color.White);

            if(mCurrentWorld > 0)
                spriteBatch.Draw(mPrevious[Convert.ToInt32(mCurrentIndex == PREVIOUS)], previousRegion, Color.White);
            else
                spriteBatch.Draw(mPrevious[Convert.ToInt32(mCurrentIndex == PREVIOUS)], previousRegion, Color.Gray);
            if(mCurrentWorld == NUM_OF_WORLDS - 1)
                spriteBatch.Draw(mNext[Convert.ToInt32(mCurrentIndex == NEXT)], nextRegion, Color.Gray);
            else
                spriteBatch.Draw(mNext[Convert.ToInt32(mCurrentIndex == NEXT)], nextRegion, Color.White);
        }

        /// <summary>
        /// Draws the levels in each of its panels
        /// </summary>
        /// <param name="spriteBatch"></param>
        private void DrawLevelPanel(SpriteBatch spriteBatch)
        {
            int i = 0;
            foreach (Rectangle rect in mLevelRegions)
            {
                if (i + 6 * mCurrentWorld >= mLevels.Count)
                    i = - mCurrentWorld * 6;
                Vector2 size = mFont.MeasureString(mLevels[i + 6 * mCurrentWorld].Name);
                if(i + 1 != mCurrentIndex)
                    spriteBatch.Draw(mBackground, rect, Color.Green);
                else
                    spriteBatch.Draw(mBackground, rect, Color.Blue);
                spriteBatch.DrawString(mFont, mLevels[(i++) + 6 * mCurrentWorld].Name, new Vector2(rect.Center.X - size.X / 2, rect.Center.Y - size.Y / 2), Color.White);
            }
        }
    }

    /// <summary>
    /// Information for the level that can be accessed easily
    /// </summary>
    public class LevelInfo
    {
        public string Name
        { get { return mLevel.Name; } }

        private Level mLevel;
        public Level Level
        { get { return mLevel; } }

        private bool mLoaded = false;

        private int mTimeStars;
        private int mCollectableStars;
        private int mDeathStars;
        private int mGoalTime;
        private int mGoalCollectable;

        public enum StarTypes { Death, Time, Collection }

        ContentManager mContent;
        IControlScheme mControls;
        GraphicsDeviceManager mGraphics;

        public LevelInfo(XElement levelInfo, ContentManager content, IControlScheme controls, GraphicsDeviceManager graphics)
        {
            foreach (XElement element in levelInfo.Elements())
            {
                if (element.Name == XmlKeys.LEVEL_NAME)
                    mLevel = new Level(LevelSelect.LEVEL_DIRECTORY + element.Value.ToString() + ".xml",
                        controls, graphics.GraphicsDevice.Viewport);
                if (element.Name == XmlKeys.TIMERSTAR)
                    mTimeStars = mLevel.TimerStar = Convert.ToInt32(element.Value);

                if (element.Name == XmlKeys.COLLECTIONSTAR)
                    mCollectableStars = mLevel.CollectionStar = Convert.ToInt32(element.Value);

                if (element.Name == XmlKeys.DEATHSTAR)
                    mDeathStars = mLevel.DeathStar = Convert.ToInt32(element.Value);

                if (element.Name == XmlKeys.GOALTIME)
                    mGoalTime = Convert.ToInt32(element.Value);

                if (element.Name == XmlKeys.GOALCOLLECTABLE)
                    mGoalCollectable = Convert.ToInt32(element.Value);
            }
            mContent = content;
            mGraphics = graphics;
            mControls = controls;
        }

        private void Load()
        {
            if(!mLoaded)
                mLevel.Load(mContent);
            mLoaded = true;
        }

        public int GetStar(StarTypes starType)
        {
            if (starType == StarTypes.Collection)
                return Math.Max(mCollectableStars, Level.CollectionStar);
            else if (starType == StarTypes.Death)
                return Math.Max(mDeathStars, Level.DeathStar);
            else
                return Math.Max(mTimeStars,Level.TimerStar);
        }

        public int GetGoal(StarTypes starType)
        {
            if (starType == StarTypes.Collection)
                return mGoalCollectable;
            else if (starType == StarTypes.Death)
                return 0;
            else
                return mGoalTime;
        }
    }
}
