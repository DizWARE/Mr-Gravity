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

        public static string LEVEL_DIRECTORY = "..\\..\\..\\Content\\Levels\\";

        //struct needed for serializing on xbox
        public struct SaveGameData
        {
            private XElement saveData;
            public XElement SaveData
            {
                get { return saveData; }
                set { saveData = value; }
            }
        }

        private const int NAME_REGION = 0;
        private const int STARS_REGION = 2;
        private const int TIMER_REGION = 1;
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

        Texture2D[,] mSelected;
        Texture2D[] mUnselected;

        Random rand;

        int number;

        string[] mWorlds = 
        { "The Ropes", "Rail Shark", "Free Motion", "Two-Sides", "Old School", "Putting it Together", "Insanity", "Good Luck" };
        int mLongestName;

        int mStarCount = 0;

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
        Texture2D mLock;
        //Texture2D mSelection;

        int mCurrentIndex = 1;
        int mCurrentWorld = 0;

        /* Trial Mode Loading */
        public bool TrialMode { get { return Guide.IsTrialMode; } }

        //record whether we have asked for a storage device already
        bool mDeviceSelected;
        public bool DeviceSelected { get { return mDeviceSelected; } set { mDeviceSelected = value; } }

        //store the storage device we are using, and the container within it.
        StorageDevice device;
        StorageContainer container;

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

            string LEVEL_LIST = "..\\..\\..\\Content\\Levels\\Info\\LevelList.xml";
#if XBOX360
            mLevelInfo = XElement.Load(LEVEL_LIST.Remove(0,8));
#else
            mLevelInfo = XElement.Load(LEVEL_LIST);
#endif

            mDeviceSelected = false;

            rand = new Random();
            number = rand.Next(4);

        }

        /// <summary>
        /// Saves level unlock and scoring information
        /// 
        /// PC saving is straightforward - but xDoc.Save() will not work on xbox360.
        /// Instead we use the built in XmlSerializer class to serialize an element out to an xml file.
        /// We build our Xelement like normal - but instead of saving that directly using XDocument.Save()
        /// we place this XElement into a struct, and use XmlSerializer to serialize the data out into a storage
        /// device on the xbox.
        /// </summary>
        /// 
        public void Save()
        {
            XElement xLevels = new XElement(XmlKeys.LEVELS);
            foreach (LevelInfo l in mLevels)
                xLevels.Add(l.Export());
            XDocument xDoc = new XDocument();
            xDoc.Add(xLevels);

#if XBOX360
            IAsyncResult result;
            if (!mDeviceSelected)
            {
                result = StorageDevice.BeginShowSelector(((ControllerControl)mControls).ControllerIndex, null, null);
                result.AsyncWaitHandle.WaitOne();
                device = StorageDevice.EndShowSelector(result);
                result.AsyncWaitHandle.Close();
                mDeviceSelected = true;
            }

            result = device.BeginOpenContainer("Mr Gravity", null, null);
            result.AsyncWaitHandle.WaitOne();
            container = device.EndOpenContainer(result);
            result.AsyncWaitHandle.Close();
            //container.DeleteFile("TrialLevelList.xml");
            //container.DeleteFile("LevelList.xml");

            Stream stream;
            if (container.FileExists("LevelList.xml"))
            {
                container.DeleteFile("LevelList.xml");
            }
            stream = container.CreateFile("LevelList.xml");
            XmlSerializer serializer = new XmlSerializer(typeof(SaveGameData));
            SaveGameData data = new SaveGameData();
            data.SaveData = xLevels;
            serializer.Serialize(stream, data);
            stream.Close();
            container.Dispose();
               
#else
            xDoc.Save("..\\..\\..\\Content\\Levels\\Info\\LevelList.xml");
#endif

        }

        /// <summary>
        /// Checks if a save file for the game already exists - and loads it if so.
        /// Meant to be used solely on XBOX360.  Should be used as soon as we know what
        /// PlayerIndex the "gamer" is using (IE right after the title screen).
        /// 
        /// Note: if a save game does not exist the constructor for this class should have
        /// filled in the default values (0 stars, all but the first locked - on xbox our default xml file
        /// should always have the default values - we cannot save information to that file as it is a binary xnb file
        /// that can't be changed at run time)
        /// 
        /// Do not call this until we know the PlayerIndex the player is using!
        /// </summary>
        public void CheckForSave()
        {
            IAsyncResult result;
            if (!mDeviceSelected)
            {
                result = StorageDevice.BeginShowSelector(((ControllerControl)mControls).ControllerIndex, null, null);
                result.AsyncWaitHandle.WaitOne();
                device = StorageDevice.EndShowSelector(result);
                result.AsyncWaitHandle.Close();
                mDeviceSelected = true;
            }

            result = device.BeginOpenContainer("Mr Gravity", null, null);
            result.AsyncWaitHandle.WaitOne();
            container = device.EndOpenContainer(result);
            result.AsyncWaitHandle.Close();

            if (container.FileExists("LevelList.xml"))
            {
                Stream stream = container.OpenFile("LevelList.xml", FileMode.Open);
                XmlSerializer serializer = new XmlSerializer(typeof(SaveGameData));
                SaveGameData data = (SaveGameData)serializer.Deserialize(stream);
                stream.Close();
                int i = 0;
                foreach (XElement xLevels in data.SaveData.Elements())
                {
                    foreach (XElement xLevelData in xLevels.Elements())
                    {
                        foreach (XElement xLevel in xLevelData.Elements())
                        {
                            if (xLevel.Name == XmlKeys.UNLOCKED && xLevel.Value == XmlKeys.TRUE)
                                mLevels[i].Unlock();

                            if (xLevel.Name == XmlKeys.TIMERSTAR)
                                mLevels[i].SetStar(LevelInfo.StarTypes.Time, Convert.ToInt32(xLevel.Value));

                            if (xLevel.Name == XmlKeys.COLLECTIONSTAR)
                                mLevels[i].SetStar(LevelInfo.StarTypes.Collection, Convert.ToInt32(xLevel.Value));

                            if (xLevel.Name == XmlKeys.DEATHSTAR)
                                mLevels[i].SetStar(LevelInfo.StarTypes.Death, Convert.ToInt32(xLevel.Value));

                        }
                        i++;
                    }
                }

            }
            container.Dispose();
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

            mSelected = new Texture2D[6, 4];
            mUnselected = new Texture2D[6];

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
        /// Resets the world system
        /// </summary>
        public void Reset()
        {
            foreach (LevelInfo level in mLevels)
                level.Reset();

            mCurrentWorld = 0;
            mCurrentIndex = 1;

            UnlockWorld(0);
            UpdateStarCount();
        }

        public Level NextLevel()
        {
            if (++mCurrentIndex > 6 && mLevels[mCurrentWorld * 6 + mCurrentIndex - 1].Unlocked)
            {
                mCurrentIndex = 1;
                mCurrentWorld = (mCurrentWorld + 1) % 8;
            }
            else if (!mLevels[mCurrentWorld * 6 + mCurrentIndex - 1].Unlocked)
                mCurrentIndex--;

            return mLevels[mCurrentWorld * 6 + mCurrentIndex - 1].Level;
        }

        /// <summary>
        /// Unlocks the given world
        /// </summary>
        /// <param name="world">The world.</param>
        public void UnlockWorld(int world)
        {
#if XBOX360
            if(!this.TrialMode || world == 0)
#endif
            for (int i = 0; i < 6; i++)
                mLevels[world * 6 + i].Unlock();
        }

        /// <summary>
        /// Updates the star count.
        /// </summary>
        public void UpdateStarCount()
        {
            mStarCount = 0;
            foreach (LevelInfo level in mLevels)
                mStarCount += level.StarCount();

            UnlockWorld(mStarCount / 45);
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
            mLock = content.Load<Texture2D>("Images/Lock/locked1a");

            mFont = content.Load<SpriteFont>("Fonts/QuartzSmaller");

            mSelected[0, 0] = content.Load<Texture2D>("Images/Menu/LevelSelect/1Blue");
            mSelected[0, 1] = content.Load<Texture2D>("Images/Menu/LevelSelect/1Green");
            mSelected[0, 2] = content.Load<Texture2D>("Images/Menu/LevelSelect/1Orange");
            mSelected[0, 3] = content.Load<Texture2D>("Images/Menu/LevelSelect/1Purple");
            mSelected[1, 0] = content.Load<Texture2D>("Images/Menu/LevelSelect/2Blue");
            mSelected[1, 1] = content.Load<Texture2D>("Images/Menu/LevelSelect/2Green");
            mSelected[1, 2] = content.Load<Texture2D>("Images/Menu/LevelSelect/2Orange");
            mSelected[1, 3] = content.Load<Texture2D>("Images/Menu/LevelSelect/2Purple");
            mSelected[2, 0] = content.Load<Texture2D>("Images/Menu/LevelSelect/3Blue");
            mSelected[2, 1] = content.Load<Texture2D>("Images/Menu/LevelSelect/3Green");
            mSelected[2, 2] = content.Load<Texture2D>("Images/Menu/LevelSelect/3Orange");
            mSelected[2, 3] = content.Load<Texture2D>("Images/Menu/LevelSelect/3Purple");
            mSelected[3, 0] = content.Load<Texture2D>("Images/Menu/LevelSelect/4Blue");
            mSelected[3, 1] = content.Load<Texture2D>("Images/Menu/LevelSelect/4Green");
            mSelected[3, 2] = content.Load<Texture2D>("Images/Menu/LevelSelect/4Orange");
            mSelected[3, 3] = content.Load<Texture2D>("Images/Menu/LevelSelect/4Purple");
            mSelected[4, 0] = content.Load<Texture2D>("Images/Menu/LevelSelect/5Blue");
            mSelected[4, 1] = content.Load<Texture2D>("Images/Menu/LevelSelect/5Green");
            mSelected[4, 2] = content.Load<Texture2D>("Images/Menu/LevelSelect/5Orange");
            mSelected[4, 3] = content.Load<Texture2D>("Images/Menu/LevelSelect/5Purple");
            mSelected[5, 0] = content.Load<Texture2D>("Images/Menu/LevelSelect/6Blue");
            mSelected[5, 1] = content.Load<Texture2D>("Images/Menu/LevelSelect/6Green");
            mSelected[5, 2] = content.Load<Texture2D>("Images/Menu/LevelSelect/6Orange");
            mSelected[5, 3] = content.Load<Texture2D>("Images/Menu/LevelSelect/6Purple");
            
            mUnselected[0] = content.Load<Texture2D>("Images/Menu/LevelSelect/1Unselected");
            mUnselected[1] = content.Load<Texture2D>("Images/Menu/LevelSelect/2Unselected");
            mUnselected[2] = content.Load<Texture2D>("Images/Menu/LevelSelect/3Unselected");
            mUnselected[3] = content.Load<Texture2D>("Images/Menu/LevelSelect/4Unselected");
            mUnselected[4] = content.Load<Texture2D>("Images/Menu/LevelSelect/5Unselected");
            mUnselected[5] = content.Load<Texture2D>("Images/Menu/LevelSelect/6Unselected");

            mLongestName = 0;
            foreach (string name in mWorlds)
                mLongestName = Math.Max((int)mFont.MeasureString(name).X, mLongestName);

            foreach (XElement level in mLevelInfo.Elements())
                mLevels.Add(new LevelInfo(level, content, mControls, mGraphics));

            UnlockWorld(0);
            UpdateStarCount();
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
                else if(mLevels[mCurrentWorld * 6 + mCurrentIndex - 1].Unlocked)
                {
                   currentLevel = mLevels[mCurrentWorld * 6 + mCurrentIndex-1].Level;
                   currentLevel.Load(mContent);
                   currentLevel.IdealTime = mLevels[mCurrentWorld * 6 + mCurrentIndex - 1].GetGoal(LevelInfo.StarTypes.Time);
                   currentLevel.CollectableCount = mLevels[mCurrentWorld * 6 + mCurrentIndex - 1].GetGoal(LevelInfo.StarTypes.Collection);
                   
                   currentLevel.TimerStar = mLevels[mCurrentWorld * 6 + mCurrentIndex - 1].GetStar(LevelInfo.StarTypes.Time);
                   currentLevel.CollectionStar = mLevels[mCurrentWorld * 6 + mCurrentIndex - 1].GetStar(LevelInfo.StarTypes.Collection);
                   currentLevel.DeathStar = mLevels[mCurrentWorld * 6 + mCurrentIndex - 1].GetStar(LevelInfo.StarTypes.Death);

                   gameState = GameStates.StartLevelSplash;
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
                number = rand.Next(4);

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
                number = rand.Next(4);

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
                number = rand.Next(4);

                mCurrentIndex--;
                if (mCurrentIndex == PREVIOUS && mCurrentWorld == 0)
                    mCurrentIndex--;
            }

            //Right Pressed
            if (mControls.isRightPressed(false))
            {
                number = rand.Next(4);

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

            spriteBatch.Draw(mBackground, mGraphics.GraphicsDevice.Viewport.Bounds, Color.White);

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
                spriteBatch.DrawString(mFont, "Goal: " + goalCollectables + " Collectables", new Vector2(mInfoRegions[STARS_REGION].Center.X - size.X / 2, mInfoRegions[STARS_REGION].Top + mPadding.Y + 2 * size.Y), Color.White);

                for (int i = 0; i < starsCollectables; i++)
                    spriteBatch.Draw(mStar, new Vector2(mInfoRegions[STARS_REGION].Center.X - starWidth / 2 + i * mStar.Width, mInfoRegions[STARS_REGION].Top + mPadding.Y + 4 * size.Y), Color.White);


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
                for (int i = 0; i < starsDeaths; i++)
                    spriteBatch.Draw(mStar, new Vector2(mInfoRegions[DEATH_REGION].Center.X - starWidth / 2 + i * mStar.Width, mInfoRegions[DEATH_REGION].Top + mPadding.Y + 4 * size.Y), Color.White);
            }
            else
            {
                size = mFont.MeasureString("Name");
                Vector2 position = new Vector2(mInfoRegions[NAME_REGION].Center.X - size.X / 2, mInfoRegions[NAME_REGION].Center.Y - size.Y / 2);

                spriteBatch.DrawString(mFont, "Name", Vector2.Add(position, new Vector2(2, 2)), Color.LightBlue);
                spriteBatch.DrawString(mFont, "Name", position, Color.White);

                size = mFont.MeasureString(mWorlds[mCurrentWorld]);
                position = Vector2.Add(position, new Vector2(0, size.Y + mPadding.Y));
                position.X = mInfoRegions[NAME_REGION].Center.X - size.X / 2;

                spriteBatch.DrawString(mFont, mWorlds[mCurrentWorld], position, Color.White);

                if (mLevels[0 + mCurrentWorld * 6].Unlocked)
                {
                    size = mFont.MeasureString("UNLOCKED");
                    position = Vector2.Add(position, new Vector2(0, size.Y + 2*mPadding.Y));
                    position.X = mInfoRegions[NAME_REGION].Center.X - size.X / 2;
                    spriteBatch.DrawString(mFont, "UNLOCKED", position, Color.White);
                    spriteBatch.DrawString(mFont, "UNLOCKED", Vector2.Add(position, new Vector2(2, 2)), Color.Green);
                   
                }
                else
                {
                    size = mFont.MeasureString("LOCKED");
                    position = Vector2.Add(position, new Vector2(0, size.Y + 2 * mPadding.Y));
                    position.X = mInfoRegions[NAME_REGION].Center.X - size.X / 2;
                    spriteBatch.DrawString(mFont, "LOCKED", position, Color.White);
                    spriteBatch.DrawString(mFont, "LOCKED", Vector2.Add(position, new Vector2(2, 2)), Color.Red);
                    
                    position = Vector2.Add(position, new Vector2(0, size.Y + 2 * mPadding.Y));
                    size = mFont.MeasureString("You need");
                    position.X = mInfoRegions[NAME_REGION].Center.X - size.X / 2;
                    spriteBatch.DrawString(mFont, "You need", position, Color.White);

                    position = Vector2.Add(position, new Vector2(0, size.Y));
                    size = mFont.MeasureString((45 * mCurrentWorld - mStarCount) + "");
                    position.X = mInfoRegions[NAME_REGION].Center.X - size.X / 2;
                    spriteBatch.DrawString(mFont, (45 * mCurrentWorld - mStarCount) + "", position, Color.Red);

                    position = Vector2.Add(position, new Vector2(0, size.Y));
                    size = mFont.MeasureString("to unlock");
                    position.X = mInfoRegions[NAME_REGION].Center.X - size.X / 2;
                    spriteBatch.DrawString(mFont, "to unlock", position, Color.White);
                    
                }
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


            stringSize = mFont.MeasureString("Star Count: " + mStarCount);
            spriteBatch.DrawString(mFont, "Star Count: " + mStarCount, new Vector2(mBottomBar.Right - stringSize.X, mBottomBar.Center.Y - stringSize.Y / 2), Color.White);
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
                Vector2 size = mFont.MeasureString(mLevels[i + 6 * mCurrentWorld].Name);
                if (i + 1 != mCurrentIndex)
                    spriteBatch.Draw(mUnselected[i], rect, Color.White);
                else
                    spriteBatch.Draw(mSelected[i, number], rect, Color.White);
                spriteBatch.DrawString(mFont, mLevels[(i++) + 6 * mCurrentWorld].Name, new Vector2(rect.Center.X - size.X / 2, rect.Center.Y - size.Y / 2), Color.White);

                if (!mLevels[i - 1 + 6 * mCurrentWorld].Unlocked)
                    spriteBatch.Draw(mLock, rect, Color.White);

                if (mLevels[i - 1 + 6 * mCurrentWorld].TenthStar())
                    spriteBatch.Draw(mStar, new Vector2(rect.Left, rect.Top), Color.Green);
            }
        }
    }

    /// <summary>
    /// Information for the level that can be accessed easily
    /// </summary>
    public class LevelInfo
    {
        /// <summary>
        /// Gets the name.
        /// </summary>
        public string Name
        { get { return mLevel.Name; } }

        private Level mLevel;

        /// <summary>
        /// Gets the level.
        /// </summary>
        public Level Level
        { get { return mLevel; } }

        private bool mUnlocked;

        /// <summary>
        /// Gets a value indicating whether this <see cref="LevelInfo"/> is unlocked.
        /// </summary>
        /// <value>
        ///   <c>true</c> if unlocked; otherwise, <c>false</c>.
        /// </value>
        public bool Unlocked
        { get { return mUnlocked; } }

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
                    mLevel = new Level(WorldSelect.LEVEL_DIRECTORY + element.Value.ToString() + ".xml",
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

                if (element.Name == XmlKeys.UNLOCKED)
                    mUnlocked = element.Value == XmlKeys.TRUE;
            }
            mContent = content;
            mGraphics = graphics;
            mControls = controls;
        }

        /// <summary>
        /// Unlocks this level
        /// </summary>
        public void Unlock()
        {
            mUnlocked = true;
        }

        /// <summary>
        /// Gets if there is a tenth star or not
        /// </summary>
        /// <returns></returns>
        public bool TenthStar()
        {
            return 9 == GetStar(StarTypes.Collection) + GetStar(StarTypes.Death) + GetStar(StarTypes.Time);
        }

        /// <summary>
        /// Gets the star count
        /// </summary>
        /// <returns></returns>
        public int StarCount()
        {
            return GetStar(StarTypes.Collection) + GetStar(StarTypes.Death) + GetStar(StarTypes.Time) + Convert.ToInt32(TenthStar());
        }

        /// <summary>
        /// Gets the star.
        /// </summary>
        /// <param name="starType">Type of the star.</param>
        /// <returns></returns>
        public int GetStar(StarTypes starType)
        {
            int starCount = 0;

            if (starType == StarTypes.Collection)
                starCount = mCollectableStars = Math.Max(mCollectableStars, Level.CollectionStar);
            else if (starType == StarTypes.Death)
                starCount = mDeathStars = Math.Max(mDeathStars, Level.DeathStar);
            else
                starCount = mTimeStars =Math.Max(mTimeStars, Level.TimerStar);

            return starCount;
        }

        /// <summary>
        /// Gets the goal.
        /// </summary>
        /// <param name="starType">Type of the star.</param>
        /// <returns></returns>
        public int GetGoal(StarTypes starType)
        {
            if (starType == StarTypes.Collection)
                return mGoalCollectable;
            else if (starType == StarTypes.Death)
                return 0;
            else
                return mGoalTime;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="starType"></param>
        /// <param name="value"></param>
        public void SetStar(StarTypes starType, int value)
        {
            if (starType == StarTypes.Collection)
                mCollectableStars = value;
            else if (starType == StarTypes.Death)
                mDeathStars = value;
            else
                mTimeStars = value;
        }

        /// <summary>
        /// Resets this level
        /// </summary>
        public void Reset()
        {
            mLevel.ResetScores();
            mCollectableStars = 0;
            mTimeStars = 0;
            mDeathStars = 0;
            mUnlocked = false;
        }

        /// <summary>
        /// Exports this level to a level info
        /// </summary>
        /// <returns>The higher level of xml</returns>
        public XElement Export()
        {
            string xUnlock = XmlKeys.FALSE;
            if (mUnlocked)
                xUnlock = XmlKeys.TRUE;

            XElement xLevel = new XElement(XmlKeys.LEVEL_DATA,
                new XElement(XmlKeys.LEVEL_NAME, this.Name),
                new XElement(XmlKeys.UNLOCKED, xUnlock),
                new XElement(XmlKeys.TIMERSTAR, mTimeStars.ToString()),
                new XElement(XmlKeys.COLLECTIONSTAR, mCollectableStars.ToString()),
                new XElement(XmlKeys.DEATHSTAR, mDeathStars.ToString()),
                new XElement(XmlKeys.GOALCOLLECTABLE, mGoalCollectable.ToString()),
                new XElement(XmlKeys.GOALTIME, mGoalTime));

            return xLevel;
        }
    }
}
