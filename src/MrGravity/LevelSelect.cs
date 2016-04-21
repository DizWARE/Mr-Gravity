using System;
using System.Collections.Generic;
using System.IO;
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
    internal class LevelSelect
    {
        //struct needed for serializing on xbox
        public struct SaveGameData
        {
            public XElement Savedata;
        }

        public static string LevelDirectory = "..\\..\\..\\Content\\Levels\\";
        public static string LevelThumbsDirectory = LevelDirectory + "Thumbnail\\";
        public static string LevelList = LevelDirectory + "Info\\LevelList.xml";
        public static string TrialLevelList = LevelDirectory + "Info\\TrialLevelList.xml";

        public static int Back = 0;
        public static int Previous = 13;
        public static int Next = 14;

        private readonly IControlScheme _mControls;

        private readonly XElement _mLevelInfo;

        private readonly List<LevelChoice> _mLevels;

        private Texture2D _mSelectBox;
        private Texture2D[] _mPrevious;
        private Texture2D[] _mNext;
        private Texture2D[] _mBack;
        private Texture2D _mBackground;
        private Texture2D _mLocked;
        private Texture2D _mStar;

        private int _mCurrentIndex = 1;
        private int _mPageCount;
        private int _mCurrentPage;

        private Rectangle _mScreenRect;

        private ContentManager _mContent;

        public GraphicsDevice Graphics { get; private set; }

        /* SpriteFont */
        private SpriteFont _mQuartz;

        /* Trial Mode Loading */
        public bool TrialMode { get; set; }

        //record whether we have asked for a storage device already
        public bool DeviceSelected { get; set; }

        //store the storage device we are using, and the container within it.
        private StorageDevice _device;
        private StorageContainer _container;

        private PlayerIndex _playerIndex;

        /// <summary>
        /// Constructs the menu screen that allows the player to select a level
        /// </summary>
        /// <param name="controlScheme">Controls that the player are using</param>
        public LevelSelect(IControlScheme controlScheme)
        {
            _mControls = controlScheme;
            _mLevels = new List<LevelChoice>();
#if XBOX360
            LEVEL_LIST = LEVEL_LIST.Remove(0, 8);
#endif

            _mLevelInfo = XElement.Load(LevelList);

            TrialMode = Guide.IsTrialMode;

            DeviceSelected = false;
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
        public void CheckForSave(PlayerIndex player)
        {

            IAsyncResult result;
            if (!DeviceSelected)
            {
                result = StorageDevice.BeginShowSelector(player, null, null);
                result.AsyncWaitHandle.WaitOne();
                _device = StorageDevice.EndShowSelector(result);
                result.AsyncWaitHandle.Close();
                DeviceSelected = true;
            }

            result = _device.BeginOpenContainer("Mr Gravity", null, null);
            result.AsyncWaitHandle.WaitOne();
            _container = _device.EndOpenContainer(result);
            result.AsyncWaitHandle.Close();

            if (_container.FileExists("LevelList.xml"))
            {
                var stream = _container.OpenFile("LevelList.xml", FileMode.Open);
                var serializer = new XmlSerializer(typeof(SaveGameData));
                var data = (SaveGameData)serializer.Deserialize(stream);
                stream.Close();
                var i = 0;
                foreach (var xLevels in data.Savedata.Elements())
                {

                    foreach (var xLevelData in xLevels.Elements())
                    {
                        foreach (var xLevel in xLevelData.Elements())
                        {
                            if (xLevel.Name == XmlKeys.Unlocked)
                                _mLevels[i].Unlocked = xLevel.Value == XmlKeys.True;

                            if (xLevel.Name == XmlKeys.Timerstar)
                                _mLevels[i].TimerStar = Convert.ToInt32(xLevel.Value);

                            if (xLevel.Name == XmlKeys.Collectionstar)
                                _mLevels[i].CollectionStar = Convert.ToInt32(xLevel.Value);

                            if (xLevel.Name == XmlKeys.Deathstar)
                                _mLevels[i].DeathStar = Convert.ToInt32(xLevel.Value);

                        }
                        i++;
                    }
                }

            }
            _container.Dispose();


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
        public void Save(PlayerIndex player) 
        {
            _playerIndex = player;
            var xLevels = new XElement(XmlKeys.Levels);
            foreach (var l in _mLevels) 
            {
                xLevels.Add(l.Export());
            }
            var xDoc = new XDocument();
            xDoc.Add(xLevels);
            //Debug.WriteLine(xDoc.ToString());
            

#if XBOX360
            IAsyncResult result;
            if (!mDeviceSelected)
            {
                result = StorageDevice.BeginShowSelector(player, null, null);
                result.AsyncWaitHandle.WaitOne();
                device = StorageDevice.EndShowSelector(result);
                result.AsyncWaitHandle.Close();
                mDeviceSelected = true;
            }

            result = device.BeginOpenContainer("GravityShift", null, null);
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
            data.savedata = xLevels;
            serializer.Serialize(stream, data);
            stream.Close();
            container.Dispose();
               
#else
                xDoc.Save(LevelList);
#endif

        }

        /// <summary>
        /// Load the data that is needed to show the Level selection screen
        /// </summary>
        /// <param name="content">Access to the content of the project</param>
        /// <param name="graphics">Graphics that draws to the screen</param>
        public void Load(ContentManager content, GraphicsDevice graphics)
        {
            foreach (var level in _mLevelInfo.Elements())
                _mLevels.Add(new LevelChoice(level,_mControls, content, graphics));

            _mContent = content;
            Graphics = graphics;

            _mSelectBox = content.Load<Texture2D>("Images/Menu/LevelSelect/SelectBox");
            _mQuartz = content.Load<SpriteFont>("Fonts/QuartzSmaller");

            _mBackground = content.Load<Texture2D>("Images\\Menu\\backgroundSquares1");

            /*TODO - REMOVE THIS WHEN REAL ART COMES*/
            _mPrevious = new Texture2D[2];
            _mPrevious[0] = content.Load<Texture2D>("Images/Menu/LevelSelect/LeftArrow");
            _mPrevious[1] = content.Load<Texture2D>("Images/Menu/LevelSelect/LeftArrowSelect");

            _mNext = new Texture2D[2];
            _mNext[0] = content.Load<Texture2D>("Images/Menu/LevelSelect/RightArrow");
            _mNext[1] = content.Load<Texture2D>("Images/Menu/LevelSelect/RightArrowSelect");

            _mBack = new Texture2D[2];
            _mBack[0] = content.Load<Texture2D>("Images/Menu/LevelSelect/Back");
            _mBack[1] = content.Load<Texture2D>("Images/Menu/LevelSelect/BackSelect");

            _mStar = content.Load<Texture2D>("Images/NonHazards/Star"); ;

            _mLocked = content.Load<Texture2D>("Images/Lock/locked1a");

            _mPageCount = _mLevels.Count / 12 +1;

            _mScreenRect = graphics.Viewport.TitleSafeArea;
        }

        /// <summary>
        /// Resets all levels to be locked (except the first level) and resets all scores to 0
        /// 
        /// This should be changed a bit for our new world structure.
        /// 
        /// Perhaps on xbox360 we can simply just reload the default xml file here (which is guaranteed to have
        /// default values within it if including it in the project in the proper state), and then save the game
        /// so the storage container contains the reset values as well.
        /// 
        /// Additionally... I am not sure why this returns a level....
        /// </summary>
        /// <returns>The first level</returns>
        public Level Reset()
        {
            foreach (var l in _mLevels)
                l.Reset(false);

            _mLevels[0].Reset(true);

            return _mLevels[0].Level;
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

            if(_mControls.IsAPressed(false)||_mControls.IsStartPressed(false))
                HandleAPressed(ref gameState,ref currentLevel);

            if (_mControls.IsBackPressed(false) || _mControls.IsBPressed(false))
            {
                gameState = GameStates.MainMenu;
                _mCurrentPage = 0;
                _mCurrentIndex = 1;
            }
        }

        /// <summary>
        /// Unlocks the next level in the game(if it is already unlocked than it won't do anything)
        /// </summary>
        public void UnlockNextLevel()
        {
            if (_mCurrentIndex + 12 * _mCurrentPage < _mLevels.Count)
                _mLevels[_mCurrentIndex + 12 * _mCurrentPage].Unlock();
        }

        /// <summary>
        /// Gets the next level in the game
        /// </summary>
        /// <returns>The next level in the game, or null if there is none</returns>
        public Level GetNextLevel()
        {
            if (_mCurrentIndex + 12 * _mCurrentPage < _mLevels.Count)
            {
                if (++_mCurrentIndex >= Previous) { _mCurrentIndex = 1; _mCurrentPage++; }
                return _mLevels[_mCurrentIndex - 1 + 12 * _mCurrentPage].Level;
            }

            return null;
        }

        /// <summary>
        /// Handle actions for each direction the player may press on their controller
        /// </summary>
        private void HandleDirectionKeys()
        {
            var max = new Vector2(10, 11);

            var countOnPage = 11;
            if (_mCurrentPage + 1 == _mPageCount)
                countOnPage = (_mLevels.Count - 1) % 12;

            var row = countOnPage / 4;
            var col = countOnPage % 4;

            max.X = max.Y = row * 4 + Math.Min(2,col);
            if (col == 1) max.Y = ++max.X;
            if(col >= 2) max.Y++;

            if (_mControls.IsLeftPressed(false))
            {
                _mCurrentIndex = (_mCurrentIndex - 1);
                if (_mCurrentIndex + 12 * _mCurrentPage > _mLevels.Count && _mCurrentIndex < Previous) _mCurrentIndex = _mLevels.Count % 12;
            }
            else if (_mControls.IsRightPressed(false))
            {
                _mCurrentIndex = (_mCurrentIndex + 1) % 15;
                if (_mCurrentIndex + 12 * _mCurrentPage > _mLevels.Count && _mCurrentIndex < Previous) _mCurrentIndex = Previous;
            }
            else if (_mControls.IsUpPressed(false))
            {
                if (_mCurrentIndex > Back && _mCurrentIndex <= 4) _mCurrentIndex = Back;
                else if (_mCurrentIndex == Back) _mCurrentIndex = Next;
                else if (_mCurrentIndex == Next) _mCurrentIndex = (int)max.Y;
                else if (_mCurrentIndex == Previous) _mCurrentIndex = (int)max.X;
                else _mCurrentIndex = (_mCurrentIndex - 4);
            }
            else if (_mControls.IsDownPressed(false))
            {
                if (_mCurrentIndex < countOnPage + 2 && _mCurrentIndex > max.X) _mCurrentIndex = Next;
                else if (_mCurrentIndex < max.X + 1 && _mCurrentIndex > row * 4) _mCurrentIndex = Previous;
                else if (_mCurrentIndex == Back) _mCurrentIndex = 1;
                else if (_mCurrentIndex == Next || _mCurrentIndex == Previous) _mCurrentIndex = Back;
                else _mCurrentIndex = (_mCurrentIndex+ 4);

                if (_mCurrentIndex > row * 4 + col + 1 && _mCurrentIndex < Previous) _mCurrentIndex = row * 4 + col + 1;
 
            }

            if (_mControls.IsLeftShoulderPressed(false))
                if (--_mCurrentPage < 0) _mCurrentPage = 0;
            if (_mControls.IsRightShoulderPressed(false))
                if (++_mCurrentPage == _mPageCount) _mCurrentPage = _mPageCount - 1;

            if (_mCurrentIndex < 0) _mCurrentIndex += 15;  
        }

        /// <summary>
        /// Handle what happens when the player presses A for all options
        /// </summary>
        /// <param name="gameState">State of the game - Reference so that it can be changed for the main game class to handle</param>
        /// <param name="currentLevel">Current level in the main game - Reference so that this can be changed for the main game class to handle</param>
        private void HandleAPressed(ref GameStates gameState, ref Level currentLevel)
        {
            if (_mCurrentIndex == Back)
            {
                gameState = GameStates.MainMenu;
                _mCurrentPage = 0;
                _mCurrentIndex = 1;
            }
            else if (_mCurrentIndex == Previous)
            {
                if (--_mCurrentPage < 0) _mCurrentPage = 0;
            }
            else if (_mCurrentIndex == Next)
            {
                if (++_mCurrentPage == _mPageCount) _mCurrentPage = _mPageCount - 1;
            }
            else if(_mLevels[_mCurrentIndex - 1 + 12 * _mCurrentPage].Unlocked)
            {
                currentLevel = _mLevels[_mCurrentIndex - 1 + 12 * _mCurrentPage].Level;
                currentLevel.Load(_mContent);
                gameState = GameStates.InGame;
            }
        }

        /// <summary>
        /// Draws the menu on the screen
        /// </summary>
        /// <param name="spriteBatch">Canvas we are drawing to</param>
        /// <param name="graphics">Information on the device's graphics</param>
        public void Draw(SpriteBatch spriteBatch, GraphicsDeviceManager graphics, Matrix scale)
        {
            spriteBatch.Begin(SpriteSortMode.Immediate,
                BlendState.AlphaBlend,
                SamplerState.LinearClamp,
                DepthStencilState.None,
                RasterizerState.CullCounterClockwise,
                null,
                scale);

            spriteBatch.Draw(_mBackground, new Rectangle(0, 0, graphics.GraphicsDevice.Viewport.Width, graphics.GraphicsDevice.Viewport.Height), Color.White);

            var size = new Vector2(_mScreenRect.Width / 4, _mScreenRect.Height / 3);
            var padding = new Vector2(size.X * .25f, size.Y * .25f);

            Vector2 stringLoc = _mQuartz.MeasureString((_mCurrentPage + 1) + "/" + _mPageCount);

            spriteBatch.Draw(_mBack[Convert.ToInt32(_mCurrentIndex == Back)] , new Rectangle(_mScreenRect.Left, _mScreenRect.Top, 70,70), Color.White);
            spriteBatch.Draw(_mPrevious[Convert.ToInt32(_mCurrentIndex == Previous)], new Rectangle(_mScreenRect.Center.X - 85, _mScreenRect.Bottom - 75, 75, 50), Color.White);
            spriteBatch.DrawString(_mQuartz, (_mCurrentPage + 1) + "/" + _mPageCount, new Vector2(_mScreenRect.Center.X + 10,_mScreenRect.Bottom - 60), Color.White);
            spriteBatch.Draw(_mNext[Convert.ToInt32(_mCurrentIndex == Next)], new Rectangle(_mScreenRect.Center.X + 75, _mScreenRect.Bottom - 75, 75, 50), Color.White);

            size.X -= 2*padding.X;
            size.Y -= 2*padding.Y;

            var currentLocation = new Vector2(_mScreenRect.X + 70, _mScreenRect.Top + 70);
            var index = 0;

            for (var i = 0; i < 12 && i + 12 * _mCurrentPage < _mLevels.Count; i++)
            {
                if (currentLocation.X + size.X + (padding.X / 4) >= graphics.GraphicsDevice.Viewport.TitleSafeArea.Width)
                {
                    currentLocation.X = _mScreenRect.X + 70;
                    currentLocation.Y += 1.5f * padding.Y + size.Y;
                }
                currentLocation.X += (padding.X / 4);
                var rect = new Rectangle((int)currentLocation.X, (int)currentLocation.Y, (int)size.X, (int)size.Y);

                spriteBatch.Draw(_mLevels[i + 12 * _mCurrentPage].Thumbnail, rect, Color.White);

                Vector2 stringSize = _mQuartz.MeasureString(_mLevels[i + 12 * _mCurrentPage].Level.Name);
                var stringLocation = new Vector2(rect.Center.X - stringSize.X/2, rect.Top - stringSize.Y);
                spriteBatch.DrawString(_mQuartz, _mLevels[i + 12 * _mCurrentPage].Level.Name, stringLocation, Color.White);
                if (index == _mCurrentIndex - 1) spriteBatch.Draw(_mSelectBox, rect, Color.White);

                if (!_mLevels[i + 12 * _mCurrentPage].Unlocked)
                {
                    //DRAW LOCKED SYMBOL
                    spriteBatch.Draw(_mLocked, new Vector2(rect.Center.X - (_mLocked.Width / 2 * 0.25f), rect.Center.Y - (_mLocked.Height / 2 * 0.25f)),
                        null, Color.White, 0.0f, Vector2.Zero, 0.25f, SpriteEffects.None, 0.0f);
                }
                else
                {
                    var time = _mLevels[i + 12 * _mCurrentPage].TimerStar;
                    var collect = _mLevels[i + 12 * _mCurrentPage].CollectionStar;
                    var death = _mLevels[i + 12 * _mCurrentPage].DeathStar;
                    var starScale = 0.5f;

                    //DUBUG//
                    //time = collect = death = 3;
                    //END DEBUG//

                    //TIME SCORE
                    if (time >= 1)
                        spriteBatch.Draw(_mStar, new Vector2(rect.Left, rect.Bottom - (_mStar.Height * 0.8f)),
                            null, Color.White, 0.0f, Vector2.Zero, starScale, SpriteEffects.None, 0.0f);
                    if (time >= 2)
                        spriteBatch.Draw(_mStar, new Vector2(rect.Left, rect.Bottom - (_mStar.Height * 0.4f)),
                            null, Color.White, 0.0f, Vector2.Zero, starScale, SpriteEffects.None, 0.0f);
                    if (time == 3)
                        spriteBatch.Draw(_mStar, new Vector2(rect.Left + (_mStar.Width * 0.4f), rect.Bottom - (_mStar.Height * 0.6f)),
                            null, Color.White, 0.0f, Vector2.Zero, starScale, SpriteEffects.None, 0.0f);

                    //COLLECTABLES SCORE
                    if (collect >= 1)
                        spriteBatch.Draw(_mStar, new Vector2(rect.Center.X - (_mStar.Width * starScale), rect.Bottom - (_mStar.Height * 0.8f)),
                            null, Color.White, 0.0f, Vector2.Zero, starScale, SpriteEffects.None, 0.0f);
                    if (collect >= 2)
                        spriteBatch.Draw(_mStar, new Vector2(rect.Center.X - (_mStar.Width * starScale), rect.Bottom - (_mStar.Height * 0.4f)),
                            null, Color.White, 0.0f, Vector2.Zero, starScale, SpriteEffects.None, 0.0f);
                    if (collect == 3)
                        spriteBatch.Draw(_mStar, new Vector2(rect.Center.X - (_mStar.Width * starScale) + (_mStar.Width * 0.4f), rect.Bottom - (_mStar.Height * 0.6f)),
                            null, Color.White, 0.0f, Vector2.Zero, starScale, SpriteEffects.None, 0.0f);

                    //DEATH SCORE
                    if (death >= 1)
                        spriteBatch.Draw(_mStar, new Vector2(rect.Right - (2 * _mStar.Width * starScale), rect.Bottom - (_mStar.Height * 0.8f)),
                            null, Color.White, 0.0f, Vector2.Zero, starScale, SpriteEffects.None, 0.0f);
                    if (death >= 2)
                        spriteBatch.Draw(_mStar, new Vector2(rect.Right - (2 * _mStar.Width * starScale), rect.Bottom - (_mStar.Height * 0.4f)),
                            null, Color.White, 0.0f, Vector2.Zero, starScale, SpriteEffects.None, 0.0f);
                    if (death == 3)
                        spriteBatch.Draw(_mStar, new Vector2(rect.Right - (2 * _mStar.Width * starScale) + (_mStar.Width * 0.4f), rect.Bottom - (_mStar.Height * 0.6f)),
                            null, Color.White, 0.0f, Vector2.Zero, starScale, SpriteEffects.None, 0.0f);
                }

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
        public Level Level { get; }

        public Texture2D Thumbnail { get; }

        public bool Unlocked { get; set; }

        public int TimerStar { get; set; }

        public int CollectionStar { get; set; }

        public int DeathStar { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="levelInfo"></param>
        /// <param name="controls"></param>
        /// <param name="graphics"></param>
        public LevelChoice(XElement levelInfo, IControlScheme controls, ContentManager content, GraphicsDevice graphics)
        {
            foreach(var element in levelInfo.Elements())
            {
                if (element.Name == XmlKeys.LevelName)
                {
                    Level = new Level(LevelSelect.LevelDirectory + element.Value + ".xml", controls, graphics.Viewport);
                    
#if XBOX360
                    mThumbnail = content.Load<Texture2D>("Levels\\Thumbnail\\" + element.Value.ToString());
#else
                    FileStream filestream;
                    try
                    {
                        filestream = new FileStream(LevelSelect.LevelThumbsDirectory + element.Value + ".png", FileMode.Open);                       
                    }
                    catch (IOException e)
                    {
                        var err = e.ToString();
                        filestream = new FileStream(LevelSelect.LevelThumbsDirectory + "..\\..\\..\\Content\\Images\\Error.png", FileMode.Open);
                    }
                    Thumbnail = Texture2D.FromStream(graphics, filestream);
                    filestream.Close();
#endif
                }
                if (element.Name == XmlKeys.Unlocked)
                    Unlocked = element.Value == XmlKeys.True;

                if (element.Name == XmlKeys.Timerstar)
                    TimerStar = Level.TimerStar = Convert.ToInt32(element.Value);

                if (element.Name == XmlKeys.Collectionstar)
                    CollectionStar = Level.CollectionStar = Convert.ToInt32(element.Value);

                if (element.Name == XmlKeys.Deathstar)
                    DeathStar = Level.DeathStar = Convert.ToInt32(element.Value);
            }
        }

        /// <summary>
        /// Resets this level choice to unlocked/locked depending on rUnlock, and resets scores to 0.
        /// </summary>
        /// <param name="rUnlock">True if level is locked, false otherwise</param>
        public void Reset(bool rUnlock)
        {
            Unlocked = rUnlock;
            TimerStar = CollectionStar = DeathStar = 0;
            Level.ResetScores();
        }

        /// <summary>
        /// Export an XElement of this level choice
        /// 
        /// TODO: change additional XElements for new worldSelect
        /// </summary>
        /// 
        public XElement Export()
        {
            SubmitScore(Level.TimerStar, Level.CollectionStar, Level.DeathStar);
            var xUnlock = XmlKeys.False;
            if (Unlocked)
                xUnlock = XmlKeys.True;

            var xLevel = new XElement(XmlKeys.LevelData,
                new XElement(XmlKeys.LevelName, Level.Name),
                new XElement(XmlKeys.Unlocked, xUnlock),
                new XElement(XmlKeys.Timerstar, TimerStar.ToString()),
                new XElement(XmlKeys.Collectionstar, CollectionStar.ToString()),
                new XElement(XmlKeys.Deathstar, DeathStar.ToString()));

            return xLevel;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Unlock()
        {
            Unlocked = true;
        }

        /// <summary>
        /// Submits new scores for the 3 scoring factors for this level, and if the new score is higher than
        /// previously recorded it makes note of this.  Scores should be between values of 0 and 3.
        /// </summary>
        /// <param name="timerStar">High Score for the Timer stars</param>
        /// <param name="collectionStar">High Score for the Collection stars</param>
        /// <param name="deathStar">High Score for the Death stars</param>
        public void SubmitScore(int timerStar, int collectionStar, int deathStar)
        {
            if (timerStar > TimerStar)
            {
                if (timerStar > 3)
                    TimerStar = 3;
                else if (timerStar < 0)
                    TimerStar = 0;
                else
                    TimerStar = timerStar;
            }

            if (collectionStar > CollectionStar)
            {
                if (collectionStar > 3)
                    CollectionStar = 3;
                else if (collectionStar < 0)
                    CollectionStar = 0;
                else
                    CollectionStar = collectionStar;
            }

            if (deathStar > DeathStar)
            {
                if (deathStar > 3)
                    DeathStar = 3;
                else if (deathStar < 0)
                    DeathStar = 0;
                else
                    DeathStar = deathStar;
            }

        }
    }
}
