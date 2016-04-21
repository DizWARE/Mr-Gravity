using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using MrGravity.Import_Code;
using MrGravity.MISC_Code;

namespace MrGravity
{
    /// <summary>
    /// Level selection menu that allows you to pick the level to start
    /// </summary>
    internal class WorldSelect
    {
        public static string LevelDirectory = "..\\..\\..\\Content\\Levels\\";

        //struct needed for serializing on xbox
        public struct SaveGameData
        {
            public XElement SaveData { get; set; }
        }

        private const int NameRegion = 0;
        private const int StarsRegion = 2;
        private const int TimerRegion = 1;
        private const int DeathRegion = 3;

        private const int NumOfWorlds = 9;

        private readonly IControlScheme _mControls;
        private readonly GraphicsDeviceManager _mGraphics;
        private ContentManager _mContent;

        private Rectangle _mScreenRect;

        private Rectangle _mTitleBar;
        private Rectangle _mLevelPanel;

        private Rectangle[] _mLevelRegions;

        private Texture2D[,] _mSelected;
        private Texture2D[] _mUnselected;

        private readonly Random _rand;

        private int _number;

        private const int None = 0;
        private const int StartLoad = 1;
        private const int Loading = 2;
        private int _mLoading;

        private readonly string[] _mWorlds = { "The Ropes", "Rail Shark", "Free Motion", "Two-Sides", "Old School", "Putting it Together", "Insanity", "Good Luck", "Jamba Geoff Jam" };
        private int _mLongestName;

        private int _mStarCount;

        private bool _mWorldUnlocked;
        private int _mLatestUnlocked;
        private int _mUnlockedTimer;

        private bool _mHasBeatFinal;
        private bool _mShowCongrats;
        private Texture2D _mLastCongrats;

        private Texture2D _mUnlockedDialog;

        private readonly List<LevelInfo> _mLevels;
        private readonly XElement _mLevelInfo;

        private Vector2 _mPadding;

        private SpriteFont _mFont;
        private SpriteFont _mFontBig;

        private Texture2D[] _mIcons;

        private Texture2D[][] _mWorldBackground;
        private Texture2D[][] _mWorldTitleBox;

        private Texture2D _mBackground;
        private Texture2D _mTitle;
        private Texture2D _mStar;
        private Texture2D _mLock;
        private Texture2D _mTitleBackground;

        //TODO - MAKE OFFICIAL
        private Texture2D _mLevelInfoBg;
        private Texture2D _mLoadingBg;

        private int _mCurrentIndex;
        private int _mCurrentWorld;

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
        public bool DeviceSelected { get; set; }

        private bool _loaded;
        //private bool wasTrial;

        //store the storage device we are using, and the container within it.
        private StorageDevice _device;
        private StorageContainer _container;

        /// <summary>
        /// Constructs the menu screen that allows the player to select a level
        /// </summary>
        /// <param name="controlScheme">Controls that the player are using</param>
        public WorldSelect(IControlScheme controlScheme, GraphicsDeviceManager graphics)
        {
            _mControls = controlScheme;
            _mGraphics = graphics;

            CreateRegions();

            _mIcons = new Texture2D[6];

            _mLevels = new List<LevelInfo>();

            var levelList = "..\\..\\..\\Content\\Levels\\Info\\LevelList.xml";
#if XBOX360
            mLevelInfo = XElement.Load(LEVEL_LIST.Remove(0,8));            
#else
            _mLevelInfo = XElement.Load(levelList);
#endif
            DeviceSelected = false;

            _rand = new Random();
            _number = _rand.Next(4);

            _device = null;
            _loaded = false;


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
            var xLevels = new XElement(XmlKeys.Levels);
            foreach (var l in _mLevels)
                xLevels.Add(l.Export());
            var xDoc = new XDocument();
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

        private void SelectDevice(IAsyncResult result)
        {
            _device = StorageDevice.EndShowSelector(result);

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
                if (!DeviceSelected && !Guide.IsVisible)
                {
                    StorageDevice.BeginShowSelector(((ControllerControl)_mControls).ControllerIndex, SelectDevice, null);
                    DeviceSelected = true;
                }
            }
            catch (Exception e)
            {
                var errTemp = e.ToString();
                DeviceSelected = false;
                return false;
            }

            if (_device == null || !_device.IsConnected)
            {
            //mDeviceSelected = false;
                return false;
            }

            try
            {
                result = _device.BeginOpenContainer("Mr Gravity", null, null);
                result.AsyncWaitHandle.WaitOne();
                _container = _device.EndOpenContainer(result);
                result.AsyncWaitHandle.Close();
            }
            catch (Exception e)
            {
                var execTemp = e.ToString();
                _device = null;
                if (_container != null)
                {
                    _container.Dispose();
                }
                _container = null;
                DeviceSelected = false;
                return false;
            }

            try
            {
                if (_container.FileExists("LevelList.xml"))
                {
                    var stream = _container.OpenFile("LevelList.xml", FileMode.Open);
                    var serializer = new XmlSerializer(typeof(SaveGameData));
                    var data = (SaveGameData)serializer.Deserialize(stream);
                    stream.Close();
                    var i = 0;
                    foreach (var xLevels in data.SaveData.Elements())
                    {
                        foreach (var xLevelData in xLevels.Elements())
                        {
                            foreach (var xLevel in xLevelData.Elements())
                            {
                                if (xLevel.Name == XmlKeys.Unlocked && xLevel.Value == XmlKeys.True)
                                    _mLevels[i].Unlock();

                                if (xLevel.Name == XmlKeys.Timerstar)
                                    _mLevels[i].SetStar(LevelInfo.StarTypes.Time, Convert.ToInt32(xLevel.Value));

                                if (xLevel.Name == XmlKeys.Collectionstar)
                                    _mLevels[i].SetStar(LevelInfo.StarTypes.Collection, Convert.ToInt32(xLevel.Value));

                                if (xLevel.Name == XmlKeys.Deathstar)
                                    _mLevels[i].SetStar(LevelInfo.StarTypes.Death, Convert.ToInt32(xLevel.Value));

                            }
                            i++;
                        }
                    }

                }
            }
            catch (Exception e)
            {
                var exceTemp = e.ToString();
                if (_container != null)
                {
                    _container.Dispose();
                }
                _container = null;
                _device = null;
                DeviceSelected = false;
                return false;
            }

            if (_device.IsConnected)
            {
                _container.Dispose();
            }
            else
            {
                _device = null;
                _container = null;
                DeviceSelected = false;
            }

            UpdateStarCount();
            return true;
        }

        /// <summary>
        /// Creates all the regions for the level editor for easy placement within the Title Safe area
        /// </summary>
        private void CreateRegions()
        {
            _mScreenRect = _mGraphics.GraphicsDevice.Viewport.TitleSafeArea;

            _mPadding = new Vector2(_mScreenRect.Width / 100, _mScreenRect.Height / 100);

            _mLevelPanel = _mTitleBar = new Rectangle();

            //Title bar, bottom bar, and level panel is 2/3 the x size of the title safe area
            _mTitleBar.Width = _mLevelPanel.Width = _mScreenRect.Width;

            //Set the bottom and title bar to 1/8 of the title safe y area. 
            //Set the level selection area to be 3/4 of the title safe y area
            _mLevelPanel.Height = _mScreenRect.Height - (_mTitleBar.Height = _mScreenRect.Height / 8);

            //Sets the x and y location of all the regions
            _mLevelPanel.X = _mTitleBar.X = _mScreenRect.Left;
            _mTitleBar.Y = _mScreenRect.Top;
            _mLevelPanel.Y = _mTitleBar.Y + _mTitleBar.Height;

            _mLevelRegions = new Rectangle[49];

            _mSelected = new Texture2D[6, 4];
            _mUnselected = new Texture2D[6];

            //Create 6 regions for image icons. The x direction is split into 3rds, which that region is 2/3rds of that area
            for (var i = 0; i < _mLevelRegions.Length; i++)
            {
                var width = _mLevelPanel.Width / 7;
                var height = _mLevelPanel.Height / 4;
                var xpadding = (i % 6) * width / 6;
                var ypadding = ((i / 6) + 1) * height / 5;

                _mLevelRegions[i] = new Rectangle(_mLevelPanel.Left + width * (i % 6) + xpadding, _mLevelPanel.Top + height * (i / 6) + ypadding, width, height);
            }
        }

        /// <summary>
        /// Resets the world system
        /// </summary>
        public void Reset()
        {
            foreach (var level in _mLevels)
                level.Reset();

            _mCurrentWorld = 0;
            _mCurrentIndex = 1;
            _mLatestUnlocked = 0;

            UnlockWorld(0);

            UpdateStarCount();
        }

        public int GetLevelTime()
        {
            return _mLevels[_mCurrentWorld * 6 + _mCurrentIndex].TimerStar;
        }

        public int GetLevelCollect()
        {
            return _mLevels[_mCurrentWorld * 6 + _mCurrentIndex].CollectStar;
        }

        public int GetLevelDeath()
        {
            return _mLevels[_mCurrentWorld * 6 + _mCurrentIndex].DeathStar;
        }

        /// <summary>
        /// Unlocks the given world
        /// </summary>
        /// <param name="world">The world.</param>
        public void UnlockWorld(int world)
        {
            if (world >= NumOfWorlds) return;

            if (world == NumOfWorlds - 1 && _mStarCount >= 480)
            {
                _mLevels[48].Unlock();
                return;
            }
            if (world == NumOfWorlds - 1)
                return;
#if XBOX360
            //if(!this.TrialMode || world < 2)
#endif
            for (var i = 0; i < (world * 6 + 6); i++)
                _mLevels[i].Unlock();
        }

        /// <summary>
        /// Updates the star count.
        /// </summary>
        public void UpdateStarCount()
        {
            _mStarCount = 0;
            foreach (var level in _mLevels)
                _mStarCount += level.StarCount();

            if (!TrialMode || _mLatestUnlocked < 1)
            {
                if (!_mHasBeatFinal && _mStarCount > 480)
                {
                    _mHasBeatFinal = true;
                    _mShowCongrats = true;
                }

                if (_mStarCount < 30)
                {
                    if (_loaded == false)
                        _loaded = true;
                    UnlockWorld(0);
                    return;
                }
                if (!_loaded)
                {
                    _mLatestUnlocked = Math.Max(_mLatestUnlocked, Math.Min(_mStarCount / 30, 7));

                    if (!TrialMode || _mLatestUnlocked < 1)
                        UnlockWorld(_mLatestUnlocked);
                    _loaded = true;
                }
                else if (_mLatestUnlocked < 7)
                {
                    if (_loaded && _mLatestUnlocked < _mStarCount / 30 && (_mLatestUnlocked = Math.Max(_mLatestUnlocked, Math.Min(_mStarCount / 30, 7))) < NumOfWorlds - 1)
                    {

                        _mWorldUnlocked = true;
                        UnlockWorld(_mLatestUnlocked);
                        if (_mLatestUnlocked == 8)
                            return;
                    }
                }
                else if (_mStarCount >= 480 && _mLatestUnlocked < NumOfWorlds - 1)
                {
                    _mWorldUnlocked = true;
                    _mLatestUnlocked = NumOfWorlds - 1;
                    UnlockWorld(_mLatestUnlocked);
                }
            }

        }

        /// <summary>
        /// Loads all the content needed for the levels
        /// </summary>
        /// <param name="content"></param>
        public void Load(ContentManager content)
        {
            _mContent = content;

            _mTitle = content.Load<Texture2D>("Images/Menu/SelectLevelSelected");
            _mBackground = content.Load<Texture2D>("Images/Menu/backgroundSquares1");
            _mStar = content.Load<Texture2D>("Images/NonHazards/YellowStar");
            _mLock = content.Load<Texture2D>("Images/Menu/LevelSelect/LockedLevel");
            _mTitleBackground = content.Load<Texture2D>("Images/Menu/LevelSelect/WorldTitle");

            _mLevelInfoBg = content.Load<Texture2D>("Images/Menu/LevelSelect/LevelMenu");

            _mLoadingBg = content.Load<Texture2D>("Images/Menu/LevelSelect/LoadingMenu");
            _mUnlockedDialog = content.Load<Texture2D>("Images/Menu/LevelSelect/WorldUnlocked");
            _mLastCongrats = content.Load<Texture2D>("Images/Menu/LevelSelect/LastLevelCongrats");

            _mWorldBackground = new Texture2D[9][];
            _mWorldTitleBox = new Texture2D[9][];
            for (var i = 0; i < _mWorldBackground.Length; i++)
            {
                _mWorldBackground[i] = new Texture2D[2];
                _mWorldTitleBox[i] = new Texture2D[2];

                _mWorldBackground[i][0] = content.Load<Texture2D>("Images/Menu/LevelSelect/World" + (i + 1) + "Selected");
                _mWorldBackground[i][1] = content.Load<Texture2D>("Images/Menu/LevelSelect/World" + (i + 1) + "Unselected");

                _mWorldTitleBox[i][0] = content.Load<Texture2D>("Images/Menu/LevelSelect/World" + (i + 1) + "TitleSelected");
                _mWorldTitleBox[i][1] = content.Load<Texture2D>("Images/Menu/LevelSelect/World" + (i + 1) + "TitleUnselected");
            }

            _mFont = content.Load<SpriteFont>("Fonts/QuartzSmaller");
            _mFontBig = content.Load<SpriteFont>("Fonts/QuartzLarge");

            _mSelected[0, 0] = content.Load<Texture2D>("Images/Menu/LevelSelect/1Blue");
            _mSelected[0, 1] = content.Load<Texture2D>("Images/Menu/LevelSelect/1Green");
            _mSelected[0, 2] = content.Load<Texture2D>("Images/Menu/LevelSelect/1Orange");
            _mSelected[0, 3] = content.Load<Texture2D>("Images/Menu/LevelSelect/1Purple");
            _mSelected[1, 0] = content.Load<Texture2D>("Images/Menu/LevelSelect/2Blue");
            _mSelected[1, 1] = content.Load<Texture2D>("Images/Menu/LevelSelect/2Green");
            _mSelected[1, 2] = content.Load<Texture2D>("Images/Menu/LevelSelect/2Orange");
            _mSelected[1, 3] = content.Load<Texture2D>("Images/Menu/LevelSelect/2Purple");
            _mSelected[2, 0] = content.Load<Texture2D>("Images/Menu/LevelSelect/3Blue");
            _mSelected[2, 1] = content.Load<Texture2D>("Images/Menu/LevelSelect/3Green");
            _mSelected[2, 2] = content.Load<Texture2D>("Images/Menu/LevelSelect/3Orange");
            _mSelected[2, 3] = content.Load<Texture2D>("Images/Menu/LevelSelect/3Purple");
            _mSelected[3, 0] = content.Load<Texture2D>("Images/Menu/LevelSelect/4Blue");
            _mSelected[3, 1] = content.Load<Texture2D>("Images/Menu/LevelSelect/4Green");
            _mSelected[3, 2] = content.Load<Texture2D>("Images/Menu/LevelSelect/4Orange");
            _mSelected[3, 3] = content.Load<Texture2D>("Images/Menu/LevelSelect/4Purple");
            _mSelected[4, 0] = content.Load<Texture2D>("Images/Menu/LevelSelect/5Blue");
            _mSelected[4, 1] = content.Load<Texture2D>("Images/Menu/LevelSelect/5Green");
            _mSelected[4, 2] = content.Load<Texture2D>("Images/Menu/LevelSelect/5Orange");
            _mSelected[4, 3] = content.Load<Texture2D>("Images/Menu/LevelSelect/5Purple");
            _mSelected[5, 0] = content.Load<Texture2D>("Images/Menu/LevelSelect/6Blue");
            _mSelected[5, 1] = content.Load<Texture2D>("Images/Menu/LevelSelect/6Green");
            _mSelected[5, 2] = content.Load<Texture2D>("Images/Menu/LevelSelect/6Orange");
            _mSelected[5, 3] = content.Load<Texture2D>("Images/Menu/LevelSelect/6Purple");

            _mUnselected[0] = content.Load<Texture2D>("Images/Menu/LevelSelect/1Unselected");
            _mUnselected[1] = content.Load<Texture2D>("Images/Menu/LevelSelect/2Unselected");
            _mUnselected[2] = content.Load<Texture2D>("Images/Menu/LevelSelect/3Unselected");
            _mUnselected[3] = content.Load<Texture2D>("Images/Menu/LevelSelect/4Unselected");
            _mUnselected[4] = content.Load<Texture2D>("Images/Menu/LevelSelect/5Unselected");
            _mUnselected[5] = content.Load<Texture2D>("Images/Menu/LevelSelect/6Unselected");

            _mLongestName = 0;
            foreach (var name in _mWorlds)
                _mLongestName = Math.Max((int)_mFont.MeasureString(name).X, _mLongestName);

            foreach (var level in _mLevelInfo.Elements())
                _mLevels.Add(new LevelInfo(level, content, _mControls, _mGraphics));

            for(var i = 0; i < 8; i++)
                if(!_mLevels[i*6].Unlocked)
                {   _mLatestUnlocked = i - 1; break; }

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
            if (_mLoading == Loading)
            {
                _mLoading = None;
                if (currentLevel != null) currentLevel.Dispose();
                currentLevel = _mLevels[_mCurrentWorld * 6 + _mCurrentIndex].Level;
                currentLevel.Load(_mContent);

                currentLevel.IdealTime = _mLevels[_mCurrentWorld * 6 + _mCurrentIndex].GetGoal(LevelInfo.StarTypes.Time);
                currentLevel.CollectableCount = _mLevels[_mCurrentWorld * 6 + _mCurrentIndex].GetGoal(LevelInfo.StarTypes.Collection);

                currentLevel.TimerStar = _mLevels[_mCurrentWorld * 6 + _mCurrentIndex].GetStar(LevelInfo.StarTypes.Time);
                currentLevel.CollectionStar = _mLevels[_mCurrentWorld * 6 + _mCurrentIndex].GetStar(LevelInfo.StarTypes.Collection);
                currentLevel.DeathStar = _mLevels[_mCurrentWorld * 6 + _mCurrentIndex].GetStar(LevelInfo.StarTypes.Death);

                gameState = GameStates.StartLevelSplash;
            }

            HandleAKey(ref gameState, ref currentLevel);
            HandleBKey(ref gameState);
            HandleDirectionKey();

            UpdateStarCount();
        }

        /// <summary>
        /// Handles when the A key is pressed
        /// </summary>
        private void HandleAKey(ref GameStates gameState, ref Level currentLevel)
        {
            if (_mControls.IsAPressed(false) || _mControls.IsStartPressed(false))
            {
                if (_mShowCongrats)
                { _mShowCongrats = false; return; }

                if (GameSound.Volume != 0)
                    GameSound.MenuSoundSelect.Play();

                if (_mLevels[_mCurrentWorld * 6 + _mCurrentIndex].Unlocked)
                    _mLoading = StartLoad;
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
            if (_mControls.IsBPressed(false) || _mControls.IsBackPressed(false))
            {
                if (_mShowCongrats)
                { _mShowCongrats = false; return; }

                Exit(ref gameState);
            }
        }


        /// <summary>
        /// Exits the level selection
        /// </summary>
        /// <param name="gameState"></param>
        private void Exit(ref GameStates gameState)
        {
            _mCurrentIndex = 1;
            gameState = GameStates.MainMenu;
        }

        /// <summary>
        /// Handles the directional keys
        /// </summary>
        private void HandleDirectionKey()
        {
            if (_mShowCongrats)
            { return; }

            //Down Button
            if (_mControls.IsDownPressed(false))
            {
                _number = _rand.Next(4);

                if (_mCurrentWorld < NumOfWorlds - 1)
                    _mCurrentWorld++;

                if (GameSound.Volume != 0)
                    GameSound.MenuSoundRollover.Play();
            }

            //Up Button
            if (_mControls.IsUpPressed(false))
            {
                _number = _rand.Next(4);

                if (_mCurrentWorld > 0)
                    _mCurrentWorld--;

                if (GameSound.Volume != 0)
                    GameSound.MenuSoundRollover.Play();
            }

            //Left Pressed
            if (_mControls.IsLeftPressed(false))
            {
                _number = _rand.Next(4);

                if (_mCurrentIndex > 0)
                    _mCurrentIndex--;

                if (GameSound.Volume != 0)
                    GameSound.MenuSoundRollover.Play();
            }

            //Right Pressed
            if (_mControls.IsRightPressed(false))
            {
                _number = _rand.Next(4);

                if (_mCurrentIndex < 5)
                    _mCurrentIndex++;

                if (GameSound.Volume != 0)
                    GameSound.MenuSoundRollover.Play();
            }

            if (_mCurrentWorld == 8) _mCurrentIndex = 0;
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

            spriteBatch.Draw(_mBackground, _mGraphics.GraphicsDevice.Viewport.Bounds, Color.White);

            DrawLevelPanel(spriteBatch);
            DrawTitleBar(spriteBatch);

            if (_mShowCongrats)
            {
                _mWorldUnlocked = false;
                spriteBatch.Draw(_mLastCongrats, new Vector2(_mScreenRect.Center.X - _mLastCongrats.Width / 2, _mScreenRect.Center.Y - _mLastCongrats.Height / 2), Color.White);
            }

            if (_mWorldUnlocked && _mUnlockedTimer < 45 && !TrialMode)
            {
                Vector2 size = _mFontBig.MeasureString("New World Unlocked");
                spriteBatch.Draw(_mUnlockedDialog, new Rectangle((int)(_mScreenRect.Center.X - size.X / 2 - size.X / 4), (int)(_mScreenRect.Center.Y - 3*size.Y/2),
                    (int)(size.X + size.X / 2), (int)(3*size.Y)), Color.White);
                _mUnlockedTimer++;
            }
            else if (_mUnlockedTimer >= 45)
            {
                _mUnlockedTimer = 0;
                _mWorldUnlocked = false;
            }

            //Draw loading screen
            if (_mLoading == StartLoad)
            {
                spriteBatch.Draw(_mLoadingBg, new Vector2(_mScreenRect.Center.X - _mLoadingBg.Width / 2,
                    _mScreenRect.Center.Y - _mLoadingBg.Height / 2), Color.White);

                _mLoading = Loading;
            }
            spriteBatch.End();
        }

        /// <summary>
        /// Draw the title bar to the screen
        /// </summary>
        /// <param name="spriteBatch"></param>
        private void DrawTitleBar(SpriteBatch spriteBatch)
        {
            var titleRegion = new Rectangle(_mTitleBar.Center.X - _mTitleBar.Width / 4, _mTitleBar.Top + (int)_mPadding.Y,
                (int)((_mTitleBar.Width / 2 - 2 * _mPadding.Y)), (int)(_mTitleBar.Height - 2 * _mPadding.Y));

            var titleBackgroundRegion = new Rectangle(_mGraphics.GraphicsDevice.Viewport.Bounds.X,
                _mGraphics.GraphicsDevice.Viewport.Y, _mGraphics.GraphicsDevice.Viewport.Width, _mTitleBar.Bottom - _mGraphics.GraphicsDevice.Viewport.Y);

            //Draw the title and the title background
            spriteBatch.Draw(_mTitleBackground, titleBackgroundRegion, Color.White);
            spriteBatch.Draw(_mTitle, titleRegion, Color.White);

            //Draw the star count
            Vector2 size = _mFontBig.MeasureString(_mStarCount + "");
            spriteBatch.Draw(_mStar, new Rectangle((int)(_mTitleBar.Right - size.Y - _mTitleBar.Width / 32), (int)(_mTitleBar.Bottom - size.Y * 1.25f), (int)size.Y, (int)size.Y), Color.White);
            spriteBatch.DrawString(_mFont, "x", new Vector2(_mTitleBar.Right - size.Y * 1.25f - _mTitleBar.Width / 32, _mTitleBar.Bottom - size.Y), Color.White);
            spriteBatch.DrawString(_mFontBig, _mStarCount + "", new Vector2(_mTitleBar.Right - size.X - size.Y * 1.25f - _mTitleBar.Width / 32, _mTitleBar.Bottom - size.Y * 1.25f), Color.White);

            //Draw B to go back
            size = _mFont.MeasureString("Press B to go Back");
            spriteBatch.DrawString(_mFont, "Press B to go Back", new Vector2(_mTitleBar.Left + _mTitleBar.Width / 128, _mTitleBar.Bottom - size.Y - _mTitleBar.Height / 6), Color.White);
        }

        /// <summary>
        /// Draw info bar on the right
        /// </summary>
        /// <param name="spriteBatch"></param>
        private void DrawInfoBar(SpriteBatch spriteBatch, int shiftValue)
        {
            //If the world is locked, do not display the level info
            if (!_mLevels[_mCurrentWorld * 6 + _mCurrentIndex].Unlocked) return;

            //Region where the infobar goes; Shift with the scrolling worlds and slightly up to hide some lines
            var infoBarLoc = _mLevelRegions[_mCurrentWorld * 6 + _mCurrentIndex];
            infoBarLoc.Offset(0, -shiftValue - (int)(infoBarLoc.Height * .04));

            //Draw the info bg
            spriteBatch.Draw(_mLevelInfoBg, infoBarLoc, Color.White);

            //Measure the size of the level's name
            var name = _mLevels[_mCurrentWorld * 6 + _mCurrentIndex].Name;
            Vector2 size = _mFont.MeasureString(name);

            //If the size is too big, we need to arrange characters so that it looks pleasing
            if (size.X > infoBarLoc.Width * 15 / 16 && name.Contains(' '))
            {
                var spaceIndex = name.LastIndexOf(' ');

                //Draw the string from beginning to the last space
                size = _mFont.MeasureString(name.Substring(0, spaceIndex));
                spriteBatch.DrawString(_mFont, name.Substring(0, spaceIndex),
                    new Vector2(infoBarLoc.Center.X - size.X / 2, infoBarLoc.Top + infoBarLoc.Height / 8 - size.Y * 11 / 16), Color.White);

                //Draw the string from the last space to the end
                size = _mFont.MeasureString(name.Substring(spaceIndex + 1));
                spriteBatch.DrawString(_mFont, name.Substring(spaceIndex + 1),
                    new Vector2(infoBarLoc.Center.X - size.X / 2, infoBarLoc.Top + infoBarLoc.Height / 8 - size.Y * 1 / 16), Color.White);
            }
            else
                //Otherwise just draw it normally
                spriteBatch.DrawString(_mFont, name,
                    new Vector2(infoBarLoc.Center.X - size.X / 2, infoBarLoc.Top + infoBarLoc.Height / 8 - size.Y * 5 / 16), Color.White);

            //Draw the acheivment data on the info bar, as long as they have stars but not all of them
            if (_mLevels[_mCurrentWorld * 6 + _mCurrentIndex].StarCount() > 0 && !_mLevels[_mCurrentWorld * 6 + _mCurrentIndex].TenthStar())
            {
                size = _mFont.MeasureString("Time:");
                spriteBatch.DrawString(_mFont, "Time:",
                        new Vector2(infoBarLoc.Left + infoBarLoc.Width / 16, infoBarLoc.Top + infoBarLoc.Height * 5 / 16 - size.Y * 5 / 16), Color.White);

                size = _mFont.MeasureString("Gems:");
                spriteBatch.DrawString(_mFont, "Gems:",
                        new Vector2(infoBarLoc.Left + infoBarLoc.Width / 16, infoBarLoc.Top + infoBarLoc.Height / 2 - size.Y * 5 / 16), Color.White);

                size = _mFont.MeasureString("Deaths:");
                spriteBatch.DrawString(_mFont, "Deaths:",
                        new Vector2(infoBarLoc.Left + infoBarLoc.Width / 16, infoBarLoc.Top + infoBarLoc.Height * 11 / 16 - size.Y * 5 / 16), Color.White);

                //This will align the stars together
                double startXPos = infoBarLoc.Left + infoBarLoc.Width / 16 + size.X;

                //Stars for time
                for (var i = 0; i < _mLevels[_mCurrentWorld * 6 + _mCurrentIndex].GetStar(LevelInfo.StarTypes.Time); i++)
                    spriteBatch.Draw(_mStar, new Rectangle((int)(startXPos + 3 * size.Y / 4 * i),
                        (int)(infoBarLoc.Top + infoBarLoc.Height * 5 / 16 - size.Y * 3 / 16),
                        3 * (int)size.Y / 4, 3 * (int)size.Y / 4), Color.White);

                //Stars for gems
                for (var i = 0; i < _mLevels[_mCurrentWorld * 6 + _mCurrentIndex].GetStar(LevelInfo.StarTypes.Collection); i++)
                    spriteBatch.Draw(_mStar, new Rectangle((int)(startXPos + 3 * size.Y / 4 * i),
                        (int)(infoBarLoc.Top + infoBarLoc.Height / 2 - size.Y * 3 / 16),
                        3 * (int)size.Y / 4, 3 * (int)size.Y / 4), Color.White);

                //Stars for death
                for (var i = 0; i < _mLevels[_mCurrentWorld * 6 + _mCurrentIndex].GetStar(LevelInfo.StarTypes.Death); i++)
                    spriteBatch.Draw(_mStar, new Rectangle((int)(startXPos + 3 * size.Y / 4 * i),
                        (int)(infoBarLoc.Top + infoBarLoc.Height * 11 / 16 - size.Y * 3 / 16),
                        3 * (int)size.Y / 4, 3 * (int)size.Y / 4), Color.White);
            }

            //If it does have all 10
            else if (_mLevels[_mCurrentWorld * 6 + _mCurrentIndex].TenthStar())
            {
                size = _mFont.MeasureString("All 10 stars");
                spriteBatch.DrawString(_mFont, "All 10 stars",
                       new Vector2(infoBarLoc.Center.X - size.X / 2, infoBarLoc.Center.Y - size.Y / 2), Color.White);

                //Draw 10 stars in 2 rows of 5
                for (var i = 0; i < 2; i++)
                    for (var j = 0; j < 5; j++)
                        spriteBatch.Draw(_mStar, new Rectangle(infoBarLoc.Left + 5*infoBarLoc.Width / 16 + j * infoBarLoc.Width / 12,
                            (int)(infoBarLoc.Center.Y- size.Y + size.Y/5 - i * infoBarLoc.Height / 12), infoBarLoc.Width / 12, infoBarLoc.Height / 12), Color.White);


                size = _mFont.MeasureString("collected");
                spriteBatch.DrawString(_mFont, "collected",
                       new Vector2(infoBarLoc.Center.X - size.X / 2, infoBarLoc.Center.Y + size.Y / 2), Color.White);
            }

            //Otherwise, let the user know they have no stars
            else
            {
                size = _mFont.MeasureString("No Stars");
                spriteBatch.DrawString(_mFont, "No Stars",
                       new Vector2(infoBarLoc.Center.X - size.X / 2, infoBarLoc.Center.Y - size.Y / 2), Color.White);
                size = _mFont.MeasureString("collected");
                spriteBatch.DrawString(_mFont, "collected",
                       new Vector2(infoBarLoc.Center.X - size.X / 2, infoBarLoc.Center.Y + size.Y / 2), Color.White);
            }
        }

        /// <summary>
        /// Draws the levels in each of its panels
        /// </summary>
        /// <param name="spriteBatch"></param>
        private void DrawLevelPanel(SpriteBatch spriteBatch)
        {
            var i = 0;

            var shiftValue = 0;
            var drawNumbers = true;

            //Find how much to shift to keep everything on screen correctly
            while (_mLevelRegions[_mCurrentWorld * 6].Bottom - shiftValue > 15 * (_mLevelPanel.Top + _mLevelPanel.Height) / 16)
                shiftValue += _mLevelRegions[_mCurrentWorld * 6].Height;

            foreach (var rect in _mLevelRegions)
            {
                rect.Offset(0, -shiftValue);

                //Draws the background box and world title for this world if the current item drawing is the first item in the world
                if (i % 6 == 0)
                {
                    //Draws background
                    var background = new Rectangle(rect.Left, rect.Top, _mLevelPanel.Right - rect.Left, rect.Bottom - rect.Top - (int)((rect.Bottom - rect.Top) * .2f));
                    spriteBatch.Draw(_mWorldBackground[i / 6][Convert.ToInt32(i / 6 != _mCurrentWorld)], background, Color.White);

                    //Draws world name background and text
                    Vector2 worldText = _mFont.MeasureString(_mWorlds[i / 6]);
                    var textBox = new Rectangle(background.Center.X - _mLongestName / 2 - _mLongestName / 16, (int)(background.Top - worldText.Y - worldText.Y / 16), _mLongestName + _mLongestName / 8, (int)(worldText.Y + worldText.Y / 8));
                    spriteBatch.Draw(_mWorldTitleBox[i / 6][Convert.ToInt32(i / 6 != _mCurrentWorld)], textBox, Color.White);
                    spriteBatch.DrawString(_mFont, _mWorlds[i / 6], new Vector2(textBox.Center.X - worldText.X / 2, textBox.Center.Y - worldText.Y / 2), Color.White);
                    if (TrialMode && i > 6)
                    {
                        spriteBatch.Draw(_mLock, background, Color.White);

                        drawNumbers = false;
                        worldText = _mFontBig.MeasureString("World Locked: You need to buy the game to play");
                        spriteBatch.DrawString(_mFontBig, "World Locked: You need to buy the game to play",
                            new Vector2(background.Center.X - worldText.X / 2, background.Center.Y - worldText.Y / 2), Color.White);
                    }
                    //If the world is not unlocked, than cover it up with the lock
                    else if (!_mLevels[i].Unlocked)
                    {
                        spriteBatch.Draw(_mLock, background, Color.White);

                        drawNumbers = false;
                        if (i / 6 != 8)
                        {
                            worldText = _mFontBig.MeasureString("World Locked: You need " + (i / 6 * 30 - _mStarCount) + " more Stars to Unlock");
                            spriteBatch.DrawString(_mFontBig, "World Locked: You need " + (i / 6 * 30 - _mStarCount) + " more Stars to Unlock",
                                new Vector2(background.Center.X - worldText.X / 2, background.Center.Y - worldText.Y / 2), Color.White);
                        }
                        else
                        {
                            worldText = _mFontBig.MeasureString("World Locked: You need " + (480 - _mStarCount) + " more Stars to Unlock");
                            spriteBatch.DrawString(_mFontBig, "World Locked: You need " + (480 - _mStarCount) + " more Stars to Unlock",
                                new Vector2(background.Center.X - worldText.X / 2, background.Center.Y - worldText.Y / 2), Color.White);
                        }
                    }

                }

                //Means this world is locked so don't draw the numbers
                if (!drawNumbers) { i++; continue; }

                //Draw numbers
                Vector2 size = _mFont.MeasureString(_mLevels[i].Name);
                if (i % 6 != _mCurrentIndex || i / 6 != _mCurrentWorld)
                    spriteBatch.Draw(_mUnselected[i % 6], rect, Color.White);
                else
                    spriteBatch.Draw(_mSelected[i % 6, _number], rect, Color.White);

                //Draw 10th star
                if (_mLevels[i].TenthStar())
                    spriteBatch.Draw(_mStar, new Vector2(rect.Right - _mStar.Width, rect.Top), Color.White);

                if (i == 48)
                {
                    size = _mFontBig.MeasureString("Congrats Travis, for winning the Naming Contest!!");
                    spriteBatch.DrawString(_mFontBig, "Congrats Travis, for winning the Naming Contest!!",
                        new Vector2(rect.Right + size.X/32, rect.Center.Y - 2*size.Y / 3), Color.White);
                    spriteBatch.DrawString(_mFontBig, "Congrats Travis, for winning the Naming Contest!!",
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
        public string Name => Level.Name;

        /// <summary>
        /// Gets the level.
        /// </summary>
        public Level Level { get; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="LevelInfo"/> is unlocked.
        /// </summary>
        /// <value>
        ///   <c>true</c> if unlocked; otherwise, <c>false</c>.
        /// </value>
        public bool Unlocked { get; private set; }

        private readonly int _mGoalTime;
        private readonly int _mGoalCollectable;

        public int TimerStar { get; private set; }

        public int CollectStar { get; private set; }

        public int DeathStar { get; private set; }


        public enum StarTypes { Death, Time, Collection }

        private ContentManager _mContent;
        private IControlScheme _mControls;
        private GraphicsDeviceManager _mGraphics;

        public LevelInfo(XElement levelInfo, ContentManager content, IControlScheme controls, GraphicsDeviceManager graphics)
        {
            foreach (var element in levelInfo.Elements())
            {
                if (element.Name == XmlKeys.LevelName)
                    Level = new Level(WorldSelect.LevelDirectory + element.Value + ".xml",
                        controls, graphics.GraphicsDevice.Viewport);
                if (element.Name == XmlKeys.Timerstar)
                    TimerStar = Level.TimerStar = Convert.ToInt32(element.Value);

                if (element.Name == XmlKeys.Collectionstar)
                    CollectStar = Level.CollectionStar = Convert.ToInt32(element.Value);

                if (element.Name == XmlKeys.Deathstar)
                    DeathStar = Level.DeathStar = Convert.ToInt32(element.Value);

                if (element.Name == XmlKeys.Goaltime)
                    _mGoalTime = Convert.ToInt32(element.Value);

                if (element.Name == XmlKeys.Goalcollectable)
                    _mGoalCollectable = Convert.ToInt32(element.Value);

                if (element.Name == XmlKeys.Unlocked)
                    Unlocked = element.Value == XmlKeys.True;
            }
            _mContent = content;
            _mGraphics = graphics;
            _mControls = controls;
        }

        /// <summary>
        /// Unlocks this level
        /// </summary>
        public void Unlock()
        {
            Unlocked = true;
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
            var starCount = 0;

            if (starType == StarTypes.Collection)
                starCount = CollectStar = Math.Max(CollectStar, Level.CollectionStar);
            else if (starType == StarTypes.Death)
                starCount = DeathStar = Math.Max(DeathStar, Level.DeathStar);
            else
                starCount = TimerStar = Math.Max(TimerStar, Level.TimerStar);

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
                return _mGoalCollectable;
            if (starType == StarTypes.Death)
                return 0;
            return _mGoalTime;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="starType"></param>
        /// <param name="value"></param>
        public void SetStar(StarTypes starType, int value)
        {
            if (starType == StarTypes.Collection)
                CollectStar = value;
            else if (starType == StarTypes.Death)
                DeathStar = value;
            else
                TimerStar = value;
        }

        /// <summary>
        /// Resets this level
        /// </summary>
        public void Reset()
        {
            Level.ResetScores();
            CollectStar = 0;
            TimerStar = 0;
            DeathStar = 0;
            Unlocked = false;
        }

        /// <summary>
        /// Exports this level to a level info
        /// </summary>
        /// <returns>The higher level of xml</returns>
        public XElement Export()
        {
            var xUnlock = XmlKeys.False;
            if (Unlocked)
                xUnlock = XmlKeys.True;

            var xLevel = new XElement(XmlKeys.LevelData,
                new XElement(XmlKeys.LevelName, Name),
                new XElement(XmlKeys.Unlocked, xUnlock),
                new XElement(XmlKeys.Timerstar, TimerStar.ToString()),
                new XElement(XmlKeys.Collectionstar, CollectStar.ToString()),
                new XElement(XmlKeys.Deathstar, DeathStar.ToString()),
                new XElement(XmlKeys.Goalcollectable, _mGoalCollectable.ToString()),
                new XElement(XmlKeys.Goaltime, _mGoalTime));

            return xLevel;
        }
    }
}
