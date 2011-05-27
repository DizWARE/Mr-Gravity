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
using System.Threading;

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

        private const int NUM_OF_WORLDS = 9;

        IControlScheme mControls;
        GraphicsDeviceManager mGraphics;
        ContentManager mContent;

        Rectangle mScreenRect;

        Rectangle mTitleBar;
        Rectangle mLevelPanel;

        Rectangle[] mLevelRegions;

        Texture2D[,] mSelected;
        Texture2D[] mUnselected;

        Random rand;

        int number;

        const int NONE = 0;
        const int START_LOAD = 1;
        const int LOADING = 2;
        int mLoading = 0;

        string[] mWorlds = { "The Ropes", "Rail Shark", "Free Motion", "Two-Sides", "Old School", "Putting it Together", "Insanity", "Good Luck", "Jamba Geoff Jam" };
        int mLongestName;

        int mStarCount = 0;

        bool mWorldUnlocked = false;
        int mLatestUnlocked = 0;
        int mUnlockedTimer = 0;

        bool mHasBeatFinal = false;
        bool mShowCongrats = false;
        Texture2D mLastCongrats;

        Texture2D mUnlockedDialog;

        List<LevelInfo> mLevels;
        XElement mLevelInfo;

        Vector2 mPadding;

        SpriteFont mFont;
        SpriteFont mFontBig;

        Texture2D[] mIcons;

        Texture2D[][] mWorldBackground;
        Texture2D[][] mWorldTitleBox;

        Texture2D mBackground;
        Texture2D mTitle;
        Texture2D mStar;
        Texture2D mLock;
        Texture2D mTitleBackground;

        //TODO - MAKE OFFICIAL
        Texture2D mLevelInfoBG;
        Texture2D mLoadingBG;

        int mCurrentIndex = 0;
        int mCurrentWorld = 0;

        //bool displayUnlockDialog = false;

        /* Trial Mode Loading */
        public bool TrialMode 
        { 
            get {
#if XBOX360
                return Guide.IsTrialMode; 
#else 
                return false;
#endif
            } 
        }

        //record whether we have asked for a storage device already
        bool mDeviceSelected;
        public bool DeviceSelected { get { return mDeviceSelected; } set { mDeviceSelected = value; } }

        private bool loaded;
        //private bool wasTrial;

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

            device = null;
            loaded = false;


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
            try 
            {
                if (!mDeviceSelected)
                {
                    StorageDevice.BeginShowSelector(((ControllerControl)mControls).ControllerIndex, this.SelectDevice, null);
                    mDeviceSelected = true;
                }
            }
            catch (Exception e)
            {
                string errTemp = e.ToString();
                return;
            }

            if (device == null || !device.IsConnected)
            {
                return;
            }
            try
            {
                result = device.BeginOpenContainer("Mr Gravity", null, null);
                result.AsyncWaitHandle.WaitOne();
                container = device.EndOpenContainer(result);

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
                result.AsyncWaitHandle.Close();
            }
            catch (Exception e)
            {
                string execTemp = e.ToString();
                return;
            }
               
#else
            xDoc.Save("..\\..\\..\\Content\\Levels\\Info\\LevelList.xml");
#endif

        }

        void SelectDevice(IAsyncResult result)
        {
            device = StorageDevice.EndShowSelector(result);

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
        public bool CheckForSave()
        {
            IAsyncResult result;
            try
            {
                if (!mDeviceSelected && !Guide.IsVisible)
                {
                    StorageDevice.BeginShowSelector(((ControllerControl)mControls).ControllerIndex, this.SelectDevice, null);
                    mDeviceSelected = true;
                }
            }
            catch (Exception e)
            {
                string errTemp = e.ToString();
                mDeviceSelected = false;
                return false;
            }

            if (device == null || !device.IsConnected)
            {
            //mDeviceSelected = false;
                return false;
            }

            try
            {
                result = device.BeginOpenContainer("Mr Gravity", null, null);
                result.AsyncWaitHandle.WaitOne();
                container = device.EndOpenContainer(result);
                result.AsyncWaitHandle.Close();
            }
            catch (Exception e)
            {
                string execTemp = e.ToString();
                device = null;
                if (container != null)
                {
                    container.Dispose();
                }
                container = null;
                mDeviceSelected = false;
                return false;
            }

            try
            {
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
            }
            catch (Exception e)
            {
                string exceTemp = e.ToString();
                if (container != null)
                {
                    container.Dispose();
                }
                container = null;
                device = null;
                mDeviceSelected = false;
                return false;
            }

            if (device.IsConnected)
            {
                container.Dispose();
            }
            else
            {
                device = null;
                container = null;
                mDeviceSelected = false;
            }

            this.UpdateStarCount();
            return true;
        }

        /// <summary>
        /// Creates all the regions for the level editor for easy placement within the Title Safe area
        /// </summary>
        private void CreateRegions()
        {
            mScreenRect = mGraphics.GraphicsDevice.Viewport.TitleSafeArea;

            mPadding = new Vector2(mScreenRect.Width / 100, mScreenRect.Height / 100);

            mLevelPanel = mTitleBar = new Rectangle();

            //Title bar, bottom bar, and level panel is 2/3 the x size of the title safe area
            mTitleBar.Width = mLevelPanel.Width = mScreenRect.Width;

            //Set the bottom and title bar to 1/8 of the title safe y area. 
            //Set the level selection area to be 3/4 of the title safe y area
            mLevelPanel.Height = mScreenRect.Height - (mTitleBar.Height = mScreenRect.Height / 8);

            //Sets the x and y location of all the regions
            mLevelPanel.X = mTitleBar.X = mScreenRect.Left;
            mTitleBar.Y = mScreenRect.Top;
            mLevelPanel.Y = mTitleBar.Y + mTitleBar.Height;

            mLevelRegions = new Rectangle[49];

            mSelected = new Texture2D[6, 4];
            mUnselected = new Texture2D[6];

            //Create 6 regions for image icons. The x direction is split into 3rds, which that region is 2/3rds of that area
            for (int i = 0; i < mLevelRegions.Length; i++)
            {
                int width = mLevelPanel.Width / 7;
                int height = mLevelPanel.Height / 4;
                int xpadding = (i % 6) * width / 6;
                int ypadding = ((i / 6) + 1) * height / 5;

                mLevelRegions[i] = new Rectangle(mLevelPanel.Left + width * (i % 6) + xpadding, mLevelPanel.Top + height * (i / 6) + ypadding, width, height);
            }
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
            mLatestUnlocked = 0;

            UnlockWorld(0);

            UpdateStarCount();
        }

        public int getLevelTime()
        {
            return mLevels[mCurrentWorld * 6 + mCurrentIndex].TimerStar;
        }

        public int getLevelCollect()
        {
            return mLevels[mCurrentWorld * 6 + mCurrentIndex].CollectStar;
        }

        public int getLevelDeath()
        {
            return mLevels[mCurrentWorld * 6 + mCurrentIndex].DeathStar;
        }

        /// <summary>
        /// Unlocks the given world
        /// </summary>
        /// <param name="world">The world.</param>
        public void UnlockWorld(int world)
        {
            if (world >= NUM_OF_WORLDS) return;

            if (world == NUM_OF_WORLDS - 1 && mStarCount >= 480)
            {
                mLevels[48].Unlock();
                return;
            }
            else if (world == NUM_OF_WORLDS - 1)
                return;
#if XBOX360
            //if(!this.TrialMode || world < 2)
#endif
            for (int i = 0; i < (world * 6 + 6); i++)
                mLevels[i].Unlock();
        }

        /// <summary>
        /// Updates the star count.
        /// </summary>
        public void UpdateStarCount()
        {
            mStarCount = 0;
            foreach (LevelInfo level in mLevels)
                mStarCount += level.StarCount();

            if (!this.TrialMode || mLatestUnlocked < 1)
            {
                if (!mHasBeatFinal && mStarCount > 480)
                {
                    mHasBeatFinal = true;
                    mShowCongrats = true;
                }

                if (mStarCount < 30)
                {
                    if (loaded == false)
                        loaded = true;
                    UnlockWorld(0);
                    return;
                }
                if (!loaded)
                {
                    mLatestUnlocked = Math.Max(mLatestUnlocked, Math.Min(mStarCount / 30, 7));

                    UnlockWorld(mLatestUnlocked);
                    loaded = true;
                }
                else if (mLatestUnlocked < 7)
                {
                    if (loaded && mLatestUnlocked < mStarCount / 30 && (mLatestUnlocked = Math.Max(mLatestUnlocked, Math.Min(mStarCount / 30, 7))) < NUM_OF_WORLDS - 1)
                    {

                        mWorldUnlocked = true;
                        UnlockWorld(mLatestUnlocked);
                        if (mLatestUnlocked == 8)
                            return;
                    }
                }
                else if (mStarCount >= 480 && mLatestUnlocked < NUM_OF_WORLDS - 1)
                {
                    mWorldUnlocked = true;
                    mLatestUnlocked = NUM_OF_WORLDS - 1;
                    UnlockWorld(mLatestUnlocked);
                }
            }

        }

        /// <summary>
        /// Loads all the content needed for the levels
        /// </summary>
        /// <param name="content"></param>
        public void Load(ContentManager content)
        {
            mContent = content;

            mTitle = content.Load<Texture2D>("Images/Menu/SelectLevelSelected");
            mBackground = content.Load<Texture2D>("Images/Menu/backgroundSquares1");
            mStar = content.Load<Texture2D>("Images/NonHazards/YellowStar");
            mLock = content.Load<Texture2D>("Images/Menu/LevelSelect/LockedLevel");
            mTitleBackground = content.Load<Texture2D>("Images/Menu/LevelSelect/WorldTitle");

            mLevelInfoBG = content.Load<Texture2D>("Images/Menu/LevelSelect/LevelMenu");

            mLoadingBG = content.Load<Texture2D>("Images/Menu/LevelSelect/LoadingMenu");
            mUnlockedDialog = content.Load<Texture2D>("Images/Menu/LevelSelect/WorldUnlocked");
            mLastCongrats = content.Load<Texture2D>("Images/Menu/LevelSelect/LastLevelCongrats");

            mWorldBackground = new Texture2D[9][];
            mWorldTitleBox = new Texture2D[9][];
            for (int i = 0; i < mWorldBackground.Length; i++)
            {
                mWorldBackground[i] = new Texture2D[2];
                mWorldTitleBox[i] = new Texture2D[2];

                mWorldBackground[i][0] = content.Load<Texture2D>("Images/Menu/LevelSelect/World" + (i + 1) + "Selected");
                mWorldBackground[i][1] = content.Load<Texture2D>("Images/Menu/LevelSelect/World" + (i + 1) + "Unselected");

                mWorldTitleBox[i][0] = content.Load<Texture2D>("Images/Menu/LevelSelect/World" + (i + 1) + "TitleSelected");
                mWorldTitleBox[i][1] = content.Load<Texture2D>("Images/Menu/LevelSelect/World" + (i + 1) + "TitleUnselected");
            }

            mFont = content.Load<SpriteFont>("Fonts/QuartzSmaller");
            mFontBig = content.Load<SpriteFont>("Fonts/QuartzLarge");

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

            for(int i = 0; i < 8; i++)
                if(!mLevels[i*6].Unlocked)
                {   mLatestUnlocked = i - 1; break; }

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
            //Handle loading after loading screen has been drawn
            if (mLoading == LOADING)
            {
                mLoading = NONE;
                if (currentLevel != null) currentLevel.Dispose();
                currentLevel = mLevels[mCurrentWorld * 6 + mCurrentIndex].Level;
                currentLevel.Load(mContent);

                currentLevel.IdealTime = mLevels[mCurrentWorld * 6 + mCurrentIndex].GetGoal(LevelInfo.StarTypes.Time);
                currentLevel.CollectableCount = mLevels[mCurrentWorld * 6 + mCurrentIndex].GetGoal(LevelInfo.StarTypes.Collection);

                currentLevel.TimerStar = mLevels[mCurrentWorld * 6 + mCurrentIndex].GetStar(LevelInfo.StarTypes.Time);
                currentLevel.CollectionStar = mLevels[mCurrentWorld * 6 + mCurrentIndex].GetStar(LevelInfo.StarTypes.Collection);
                currentLevel.DeathStar = mLevels[mCurrentWorld * 6 + mCurrentIndex].GetStar(LevelInfo.StarTypes.Death);

                gameState = GameStates.StartLevelSplash;
            }

            HandleAKey(ref gameState, ref currentLevel);
            HandleBKey(ref gameState);
            HandleDirectionKey();

            this.UpdateStarCount();
        }

        /// <summary>
        /// Handles when the A key is pressed
        /// </summary>
        private void HandleAKey(ref GameStates gameState, ref Level currentLevel)
        {
            if (mControls.isAPressed(false) || mControls.isStartPressed(false))
            {
                if (mShowCongrats)
                { mShowCongrats = false; return; }

                if (GameSound.volume != 0)
                    GameSound.menuSound_select.Play();

                if (mLevels[mCurrentWorld * 6 + mCurrentIndex].Unlocked)
                    mLoading = START_LOAD;
#if XBOX360
                else if (TrialMode)
                {
                    gameState = GameStates.WorldPurchaseScreen;
                }
#endif


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
            {
                if (mShowCongrats)
                { mShowCongrats = false; return; }

                Exit(ref gameState);
            }
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
            if (mShowCongrats)
            { return; }

            //Down Button
            if (mControls.isDownPressed(false))
            {
                number = rand.Next(4);

                if (mCurrentWorld < NUM_OF_WORLDS - 1)
                    mCurrentWorld++;

                if (GameSound.volume != 0)
                    GameSound.menuSound_rollover.Play();
            }

            //Up Button
            if (mControls.isUpPressed(false))
            {
                number = rand.Next(4);

                if (mCurrentWorld > 0)
                    mCurrentWorld--;

                if (GameSound.volume != 0)
                    GameSound.menuSound_rollover.Play();
            }

            //Left Pressed
            if (mControls.isLeftPressed(false))
            {
                number = rand.Next(4);

                if (mCurrentIndex > 0)
                    mCurrentIndex--;

                if (GameSound.volume != 0)
                    GameSound.menuSound_rollover.Play();
            }

            //Right Pressed
            if (mControls.isRightPressed(false))
            {
                number = rand.Next(4);

                if (mCurrentIndex < 5)
                    mCurrentIndex++;

                if (GameSound.volume != 0)
                    GameSound.menuSound_rollover.Play();
            }

            if (mCurrentWorld == 8) mCurrentIndex = 0;
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

            DrawLevelPanel(spriteBatch);
            DrawTitleBar(spriteBatch);

            if (mShowCongrats)
            {
                mWorldUnlocked = false;
                spriteBatch.Draw(mLastCongrats, new Vector2(mScreenRect.Center.X - mLastCongrats.Width / 2, mScreenRect.Center.Y - mLastCongrats.Height / 2), Color.White);
            }

            if (mWorldUnlocked && mUnlockedTimer < 45 && !TrialMode)
            {
                Vector2 size = mFontBig.MeasureString("New World Unlocked");
                spriteBatch.Draw(mUnlockedDialog, new Rectangle((int)(mScreenRect.Center.X - size.X / 2 - size.X / 4), (int)(mScreenRect.Center.Y - 3*size.Y/2),
                    (int)(size.X + size.X / 2), (int)(3*size.Y)), Color.White);
                mUnlockedTimer++;
            }
            else if (mUnlockedTimer >= 45)
            {
                mUnlockedTimer = 0;
                mWorldUnlocked = false;
            }

            //Draw loading screen
            if (mLoading == START_LOAD)
            {
                spriteBatch.Draw(mLoadingBG, new Vector2(mScreenRect.Center.X - mLoadingBG.Width / 2,
                    mScreenRect.Center.Y - mLoadingBG.Height / 2), Color.White);

                mLoading = LOADING;
            }
            spriteBatch.End();
        }

        /// <summary>
        /// Draw the title bar to the screen
        /// </summary>
        /// <param name="spriteBatch"></param>
        private void DrawTitleBar(SpriteBatch spriteBatch)
        {
            Rectangle titleRegion = new Rectangle(mTitleBar.Center.X - mTitleBar.Width / 4, mTitleBar.Top + (int)mPadding.Y,
                (int)((mTitleBar.Width / 2 - 2 * mPadding.Y)), (int)(mTitleBar.Height - 2 * mPadding.Y));

            Rectangle titleBackgroundRegion = new Rectangle(mGraphics.GraphicsDevice.Viewport.Bounds.X,
                mGraphics.GraphicsDevice.Viewport.Y, mGraphics.GraphicsDevice.Viewport.Width, mTitleBar.Bottom - mGraphics.GraphicsDevice.Viewport.Y);

            //Draw the title and the title background
            spriteBatch.Draw(mTitleBackground, titleBackgroundRegion, Color.White);
            spriteBatch.Draw(mTitle, titleRegion, Color.White);

            //Draw the star count
            Vector2 size = mFontBig.MeasureString(mStarCount + "");
            spriteBatch.Draw(mStar, new Rectangle((int)(mTitleBar.Right - size.Y - mTitleBar.Width / 32), (int)(mTitleBar.Bottom - size.Y * 1.25f), (int)size.Y, (int)size.Y), Color.White);
            spriteBatch.DrawString(mFont, "x", new Vector2(mTitleBar.Right - size.Y * 1.25f - mTitleBar.Width / 32, mTitleBar.Bottom - size.Y), Color.White);
            spriteBatch.DrawString(mFontBig, mStarCount + "", new Vector2(mTitleBar.Right - size.X - size.Y * 1.25f - mTitleBar.Width / 32, mTitleBar.Bottom - size.Y * 1.25f), Color.White);

            //Draw B to go back
            size = mFont.MeasureString("Press B to go Back");
            spriteBatch.DrawString(mFont, "Press B to go Back", new Vector2(mTitleBar.Left + mTitleBar.Width / 128, mTitleBar.Bottom - size.Y - mTitleBar.Height / 6), Color.White);
        }

        /// <summary>
        /// Draw info bar on the right
        /// </summary>
        /// <param name="spriteBatch"></param>
        private void DrawInfoBar(SpriteBatch spriteBatch, int shiftValue)
        {
            //If the world is locked, do not display the level info
            if (!mLevels[mCurrentWorld * 6 + mCurrentIndex].Unlocked) return;

            //Region where the infobar goes; Shift with the scrolling worlds and slightly up to hide some lines
            Rectangle infoBarLoc = mLevelRegions[mCurrentWorld * 6 + mCurrentIndex];
            infoBarLoc.Offset(0, -shiftValue - (int)(infoBarLoc.Height * .04));

            //Draw the info bg
            spriteBatch.Draw(mLevelInfoBG, infoBarLoc, Color.White);

            //Measure the size of the level's name
            string name = mLevels[mCurrentWorld * 6 + mCurrentIndex].Name;
            Vector2 size = mFont.MeasureString(name);

            //If the size is too big, we need to arrange characters so that it looks pleasing
            if (size.X > infoBarLoc.Width * 15 / 16 && name.Contains(' '))
            {
                int spaceIndex = name.LastIndexOf(' ');

                //Draw the string from beginning to the last space
                size = mFont.MeasureString(name.Substring(0, spaceIndex));
                spriteBatch.DrawString(mFont, name.Substring(0, spaceIndex),
                    new Vector2(infoBarLoc.Center.X - size.X / 2, infoBarLoc.Top + infoBarLoc.Height / 8 - size.Y * 11 / 16), Color.White);

                //Draw the string from the last space to the end
                size = mFont.MeasureString(name.Substring(spaceIndex + 1));
                spriteBatch.DrawString(mFont, name.Substring(spaceIndex + 1),
                    new Vector2(infoBarLoc.Center.X - size.X / 2, infoBarLoc.Top + infoBarLoc.Height / 8 - size.Y * 1 / 16), Color.White);
            }
            else
                //Otherwise just draw it normally
                spriteBatch.DrawString(mFont, name,
                    new Vector2(infoBarLoc.Center.X - size.X / 2, infoBarLoc.Top + infoBarLoc.Height / 8 - size.Y * 5 / 16), Color.White);

            //Draw the acheivment data on the info bar, as long as they have stars but not all of them
            if (mLevels[mCurrentWorld * 6 + mCurrentIndex].StarCount() > 0 && !mLevels[mCurrentWorld * 6 + mCurrentIndex].TenthStar())
            {
                size = mFont.MeasureString("Time:");
                spriteBatch.DrawString(mFont, "Time:",
                        new Vector2(infoBarLoc.Left + infoBarLoc.Width / 16, infoBarLoc.Top + infoBarLoc.Height * 5 / 16 - size.Y * 5 / 16), Color.White);

                size = mFont.MeasureString("Gems:");
                spriteBatch.DrawString(mFont, "Gems:",
                        new Vector2(infoBarLoc.Left + infoBarLoc.Width / 16, infoBarLoc.Top + infoBarLoc.Height / 2 - size.Y * 5 / 16), Color.White);

                size = mFont.MeasureString("Deaths:");
                spriteBatch.DrawString(mFont, "Deaths:",
                        new Vector2(infoBarLoc.Left + infoBarLoc.Width / 16, infoBarLoc.Top + infoBarLoc.Height * 11 / 16 - size.Y * 5 / 16), Color.White);

                //This will align the stars together
                double startXPos = infoBarLoc.Left + infoBarLoc.Width / 16 + size.X;

                //Stars for time
                for (int i = 0; i < mLevels[mCurrentWorld * 6 + mCurrentIndex].GetStar(LevelInfo.StarTypes.Time); i++)
                    spriteBatch.Draw(mStar, new Rectangle((int)(startXPos + 3 * size.Y / 4 * i),
                        (int)(infoBarLoc.Top + infoBarLoc.Height * 5 / 16 - size.Y * 3 / 16),
                        3 * (int)size.Y / 4, 3 * (int)size.Y / 4), Color.White);

                //Stars for gems
                for (int i = 0; i < mLevels[mCurrentWorld * 6 + mCurrentIndex].GetStar(LevelInfo.StarTypes.Collection); i++)
                    spriteBatch.Draw(mStar, new Rectangle((int)(startXPos + 3 * size.Y / 4 * i),
                        (int)(infoBarLoc.Top + infoBarLoc.Height / 2 - size.Y * 3 / 16),
                        3 * (int)size.Y / 4, 3 * (int)size.Y / 4), Color.White);

                //Stars for death
                for (int i = 0; i < mLevels[mCurrentWorld * 6 + mCurrentIndex].GetStar(LevelInfo.StarTypes.Death); i++)
                    spriteBatch.Draw(mStar, new Rectangle((int)(startXPos + 3 * size.Y / 4 * i),
                        (int)(infoBarLoc.Top + infoBarLoc.Height * 11 / 16 - size.Y * 3 / 16),
                        3 * (int)size.Y / 4, 3 * (int)size.Y / 4), Color.White);
            }

            //If it does have all 10
            else if (mLevels[mCurrentWorld * 6 + mCurrentIndex].TenthStar())
            {
                size = mFont.MeasureString("All 10 stars");
                spriteBatch.DrawString(mFont, "All 10 stars",
                       new Vector2(infoBarLoc.Center.X - size.X / 2, infoBarLoc.Center.Y - size.Y / 2), Color.White);

                //Draw 10 stars in 2 rows of 5
                for (int i = 0; i < 2; i++)
                    for (int j = 0; j < 5; j++)
                        spriteBatch.Draw(mStar, new Rectangle(infoBarLoc.Left + 5*infoBarLoc.Width / 16 + j * infoBarLoc.Width / 12,
                            (int)(infoBarLoc.Center.Y- size.Y + size.Y/5 - i * infoBarLoc.Height / 12), infoBarLoc.Width / 12, infoBarLoc.Height / 12), Color.White);


                size = mFont.MeasureString("collected");
                spriteBatch.DrawString(mFont, "collected",
                       new Vector2(infoBarLoc.Center.X - size.X / 2, infoBarLoc.Center.Y + size.Y / 2), Color.White);
            }

            //Otherwise, let the user know they have no stars
            else
            {
                size = mFont.MeasureString("No Stars");
                spriteBatch.DrawString(mFont, "No Stars",
                       new Vector2(infoBarLoc.Center.X - size.X / 2, infoBarLoc.Center.Y - size.Y / 2), Color.White);
                size = mFont.MeasureString("collected");
                spriteBatch.DrawString(mFont, "collected",
                       new Vector2(infoBarLoc.Center.X - size.X / 2, infoBarLoc.Center.Y + size.Y / 2), Color.White);
            }
        }

        /// <summary>
        /// Draws the levels in each of its panels
        /// </summary>
        /// <param name="spriteBatch"></param>
        private void DrawLevelPanel(SpriteBatch spriteBatch)
        {
            int i = 0;

            int shiftValue = 0;
            bool drawNumbers = true;

            //Find how much to shift to keep everything on screen correctly
            while (mLevelRegions[mCurrentWorld * 6].Bottom - shiftValue > 15 * (mLevelPanel.Top + mLevelPanel.Height) / 16)
                shiftValue += mLevelRegions[mCurrentWorld * 6].Height;

            foreach (Rectangle rect in mLevelRegions)
            {
                rect.Offset(0, -shiftValue);

                //Draws the background box and world title for this world if the current item drawing is the first item in the world
                if (i % 6 == 0)
                {
                    //Draws background
                    Rectangle background = new Rectangle(rect.Left, rect.Top, mLevelPanel.Right - rect.Left, rect.Bottom - rect.Top - (int)((rect.Bottom - rect.Top) * .2f));
                    spriteBatch.Draw(mWorldBackground[i / 6][Convert.ToInt32(i / 6 != mCurrentWorld)], background, Color.White);

                    //Draws world name background and text
                    Vector2 worldText = mFont.MeasureString(mWorlds[i / 6]);
                    Rectangle textBox = new Rectangle((int)(background.Center.X - mLongestName / 2 - mLongestName / 16), (int)(background.Top - worldText.Y - worldText.Y / 16), (int)(mLongestName + mLongestName / 8), (int)(worldText.Y + worldText.Y / 8));
                    spriteBatch.Draw(mWorldTitleBox[i / 6][Convert.ToInt32(i / 6 != mCurrentWorld)], textBox, Color.White);
                    spriteBatch.DrawString(mFont, mWorlds[i / 6], new Vector2(textBox.Center.X - worldText.X / 2, textBox.Center.Y - worldText.Y / 2), Color.White);
                    if (this.TrialMode && i > 6)
                    {
                        spriteBatch.Draw(mLock, background, Color.White);

                        drawNumbers = false;
                        worldText = mFontBig.MeasureString("World Locked: You need to buy the game to play");
                        spriteBatch.DrawString(mFontBig, "World Locked: You need to buy the game to play",
                            new Vector2(background.Center.X - worldText.X / 2, background.Center.Y - worldText.Y / 2), Color.White);
                    }
                    //If the world is not unlocked, than cover it up with the lock
                    else if (!mLevels[i].Unlocked)
                    {
                        spriteBatch.Draw(mLock, background, Color.White);

                        drawNumbers = false;
                        if (i / 6 != 8)
                        {
                            worldText = mFontBig.MeasureString("World Locked: You need " + (i / 6 * 30 - mStarCount) + " more Stars to Unlock");
                            spriteBatch.DrawString(mFontBig, "World Locked: You need " + (i / 6 * 30 - mStarCount) + " more Stars to Unlock",
                                new Vector2(background.Center.X - worldText.X / 2, background.Center.Y - worldText.Y / 2), Color.White);
                        }
                        else
                        {
                            worldText = mFontBig.MeasureString("World Locked: You need " + (480 - mStarCount) + " more Stars to Unlock");
                            spriteBatch.DrawString(mFontBig, "World Locked: You need " + (480 - mStarCount) + " more Stars to Unlock",
                                new Vector2(background.Center.X - worldText.X / 2, background.Center.Y - worldText.Y / 2), Color.White);
                        }
                    }

                }

                //Means this world is locked so don't draw the numbers
                if (!drawNumbers) { i++; continue; }

                //Draw numbers
                Vector2 size = mFont.MeasureString(mLevels[i].Name);
                if (i % 6 != mCurrentIndex || i / 6 != mCurrentWorld)
                    spriteBatch.Draw(mUnselected[i % 6], rect, Color.White);
                else
                    spriteBatch.Draw(mSelected[i % 6, number], rect, Color.White);

                //Draw 10th star
                if (mLevels[i].TenthStar())
                    spriteBatch.Draw(mStar, new Vector2(rect.Right - mStar.Width, rect.Top), Color.White);

                if (i == 48)
                {
                    size = mFontBig.MeasureString("Congrats Travis, for winning the Naming Contest!!");
                    spriteBatch.DrawString(mFontBig, "Congrats Travis, for winning the Naming Contest!!",
                        new Vector2(rect.Right + size.X/32, rect.Center.Y - 2*size.Y / 3), Color.White);
                    spriteBatch.DrawString(mFontBig, "Congrats Travis, for winning the Naming Contest!!",
                       new Vector2(rect.Right + size.X / 32 + 2, rect.Center.Y - 2 * size.Y / 3 + 2), Color.CornflowerBlue);
                }

                i++;
            }

            DrawInfoBar(spriteBatch, shiftValue);
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

        public int TimerStar
        {
            get
            {
                return mTimeStars;
            }
        }

        public int CollectStar
        {
            get
            {
                return mCollectableStars;
            }
        }

        public int DeathStar
        {
            get
            {
                return mDeathStars;
            }
        }


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
                starCount = mTimeStars = Math.Max(mTimeStars, Level.TimerStar);

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
