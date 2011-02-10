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
    class LevelSelect
    {
        public struct SaveGameData
        {
            public XElement savedata;
        }

        public static string LEVEL_DIRECTORY = "..\\..\\..\\Content\\Levels\\";
        public static string LEVEL_THUMBS_DIRECTORY = LEVEL_DIRECTORY + "Thumbnail\\";
        public static string LEVEL_LIST = LEVEL_DIRECTORY + "Info\\LevelList.xml";
        public static string TRIAL_LEVEL_LIST = LEVEL_DIRECTORY + "Info\\TrialLevelList.xml";

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
        Texture2D mLocked;
        Texture2D mStar;

        int mCurrentIndex = 1;
        int mPageCount;
        int mCurrentPage = 0;

        Rectangle mScreenRect;

        ContentManager mContent;

        /* SpriteFont */
        SpriteFont mKootenay;

        /* Trial Mode Loading */
        bool mTrialMode;
        public bool TrialMode { get { return mTrialMode; } set { mTrialMode = value; } }

        bool mDeviceSelected;
        public bool DeviceSelected { get { return mDeviceSelected; } set { mDeviceSelected = value; } }

        StorageDevice device;
        StorageContainer container;

        PlayerIndex playerIndex;

        int frame = 0;

        /// <summary>
        /// Constructs the menu screen that allows the player to select a level
        /// </summary>
        /// <param name="controlScheme">Controls that the player are using</param>
        public LevelSelect(IControlScheme controlScheme)
        {
            mControls = controlScheme;
            mLevels = new List<LevelChoice>();
#if XBOX360
            LEVEL_LIST = LEVEL_LIST.Remove(0, 8);
            TRIAL_LEVEL_LIST = TRIAL_LEVEL_LIST.Remove(0, 8);
            if (Guide.IsTrialMode)
                mLevelInfo = XElement.Load(TRIAL_LEVEL_LIST);
            else
#endif

            mLevelInfo = XElement.Load(LEVEL_LIST);

            TrialMode = Guide.IsTrialMode;

            mDeviceSelected = false;
        }

        public void Reload(PlayerIndex player)
        {
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

            if (TrialMode)
            {
                if (container.FileExists("TrialLevelList.xml"))
                {
                    Stream stream = container.OpenFile("TrialLevelList.xml", FileMode.Open);
                    XmlSerializer serializer = new XmlSerializer(typeof(SaveGameData));
                    SaveGameData data = (SaveGameData)serializer.Deserialize(stream);
                    stream.Close();
                    mLevelInfo = data.savedata;
                }
                else
                {

                    mLevelInfo = XElement.Load(TRIAL_LEVEL_LIST);
                }
            }
            else
            {
                if (container.FileExists("LevelList.xml"))
                {
                    Stream stream = container.OpenFile("LevelList.xml", FileMode.Open);
                    XmlSerializer serializer = new XmlSerializer(typeof(SaveGameData));
                    SaveGameData data = (SaveGameData)serializer.Deserialize(stream);
                    stream.Close();
                    mLevelInfo = data.savedata;
                }
                else
                {
                    mLevelInfo = XElement.Load(LEVEL_LIST);
                }
            }
            container.Dispose();


#else
            if (TrialMode)
            {
                mLevelInfo = XElement.Load(TRIAL_LEVEL_LIST);
            }
            else
                mLevelInfo = XElement.Load(LEVEL_LIST);

#endif
        }

        /// <summary>
        /// Saves level unlock and scoring information
        /// </summary>
        /// 
        public void Save(PlayerIndex player) 
        {
            playerIndex = player;
            XElement xLevels = new XElement(XmlKeys.LEVELS);
            foreach (LevelChoice l in mLevels) 
            {
                xLevels.Add(l.Export());
            }
            XDocument xDoc = new XDocument();
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
            if (TrialMode)
            {
                if (container.FileExists("TrialLevelList.xml"))
                {
                    container.DeleteFile("TrialLevelList.xml");
                }
                stream = container.CreateFile("TrialLevelList.xml");
                XmlSerializer serializer = new XmlSerializer(typeof(SaveGameData));
                SaveGameData data = new SaveGameData();
                data.savedata = xLevels;
                serializer.Serialize(stream, data);
                stream.Close();
            }

            else
            {
                {
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
                }
            }
            container.Dispose();
               
#else
                xDoc.Save(LEVEL_LIST);
#endif

        }

        /// <summary>
        /// Load the data that is needed to show the Level selection screen
        /// </summary>
        /// <param name="content">Access to the content of the project</param>
        /// <param name="graphics">Graphics that draws to the screen</param>
        public void Load(ContentManager content, GraphicsDevice graphics)
        {
            foreach (XElement level in mLevelInfo.Elements())
                mLevels.Add(new LevelChoice(level,mControls, content, graphics));

            mContent = content;
            mSelectBox = content.Load<Texture2D>("Images/Menu/LevelSelect/SelectBox");
            mKootenay = content.Load<SpriteFont>("Fonts/Kootenay");

            mBackground = content.Load<Texture2D>("Images/Backgrounds/Stars");

            /*TODO - REMOVE THIS WHEN REAL ART COMES*/
            mPrevious = new Texture2D[2];
            mPrevious[0] = content.Load<Texture2D>("Images/Menu/LevelSelect/LeftArrow");
            mPrevious[1] = content.Load<Texture2D>("Images/Menu/LevelSelect/LeftArrowSelect");

            mNext = new Texture2D[2];
            mNext[0] = content.Load<Texture2D>("Images/Menu/LevelSelect/RightArrow");
            mNext[1] = content.Load<Texture2D>("Images/Menu/LevelSelect/RightArrowSelect");

            mBack = new Texture2D[2];
            mBack[0] = content.Load<Texture2D>("Images/Menu/LevelSelect/Back");
            mBack[1] = content.Load<Texture2D>("Images/Menu/LevelSelect/BackSelect");

            mStar = content.Load<Texture2D>("Images/NonHazards/Star"); ;

            mLocked = content.Load<Texture2D>("Images/Lock/locked1a");

            mPageCount = mLevels.Count / 12 +1;

            mScreenRect = graphics.Viewport.TitleSafeArea;
        }

        /// <summary>
        /// Resets all levels to be locked (except the first level) and resets all scores to 0
        /// </summary>
        /// <returns>The first level</returns>
        public Level Reset()
        {
            foreach (LevelChoice l in mLevels)
            {
                l.Reset(false);
            }

            mLevels[0].Reset(true);

            return mLevels[0].Level;
        }

        /// <summary>
        /// Handle any changes while on the level selection menu
        /// </summary>
        /// <param name="gameTime">Current time within the game</param>
        /// <param name="gameState">Current gamestate of the game</param>
        /// <param name="currentLevel">Current level of the game</param>
        public void Update(GameTime gameTime, ref GameStates gameState, ref Level currentLevel)
        {
            frame++;
            if (frame >= 60)
            {
                this.Save(playerIndex);
                frame = 0;
            }
            HandleDirectionKeys();          

            if(mControls.isAPressed(false)||mControls.isStartPressed(false))
                HandleAPressed(ref gameState,ref currentLevel);

            if (mControls.isBackPressed(false) || mControls.isBPressed(false))
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
        public void Draw(SpriteBatch spriteBatch, GraphicsDeviceManager graphics, Matrix scale)
        {
            spriteBatch.Begin(SpriteSortMode.Immediate,
                BlendState.AlphaBlend,
                SamplerState.LinearClamp,
                DepthStencilState.None,
                RasterizerState.CullCounterClockwise,
                null,
                scale);

            //spriteBatch.Draw(mBackground, mScreenRect, Color.White);

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
                if (currentLocation.X + size.X + padding.X >= graphics.GraphicsDevice.Viewport.TitleSafeArea.Width)
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

                if (!mLevels[i + 12 * mCurrentPage].Unlocked)
                {
                    //DRAW LOCKED SYMBOL
                    spriteBatch.Draw(mLocked, new Vector2(rect.Center.X - (mLocked.Width / 2 * 0.25f), rect.Center.Y - (mLocked.Height / 2 * 0.25f)),
                        null, Color.White, 0.0f, Vector2.Zero, 0.25f, SpriteEffects.None, 0.0f);
                }
                else
                {
                    int time = mLevels[i + 12 * mCurrentPage].TimerStar;
                    int collect = mLevels[i + 12 * mCurrentPage].CollectionStar;
                    int death = mLevels[i + 12 * mCurrentPage].DeathStar;
                    float starScale = 0.5f;

                    //DUBUG//
                    //time = collect = death = 3;
                    //END DEBUG//

                    //TIME SCORE
                    if (time >= 1)
                        spriteBatch.Draw(mStar, new Vector2(rect.Left, rect.Bottom - (mStar.Height * 0.8f)),
                            null, Color.White, 0.0f, Vector2.Zero, starScale, SpriteEffects.None, 0.0f);
                    if (time >= 2)
                        spriteBatch.Draw(mStar, new Vector2(rect.Left, rect.Bottom - (mStar.Height * 0.4f)),
                            null, Color.White, 0.0f, Vector2.Zero, starScale, SpriteEffects.None, 0.0f);
                    if (time == 3)
                        spriteBatch.Draw(mStar, new Vector2(rect.Left + (mStar.Width * 0.4f), rect.Bottom - (mStar.Height * 0.6f)),
                            null, Color.White, 0.0f, Vector2.Zero, starScale, SpriteEffects.None, 0.0f);

                    //COLLECTABLES SCORE
                    if (collect >= 1)
                        spriteBatch.Draw(mStar, new Vector2(rect.Center.X - (mStar.Width * starScale), rect.Bottom - (mStar.Height * 0.8f)),
                            null, Color.White, 0.0f, Vector2.Zero, starScale, SpriteEffects.None, 0.0f);
                    if (collect >= 2)
                        spriteBatch.Draw(mStar, new Vector2(rect.Center.X - (mStar.Width * starScale), rect.Bottom - (mStar.Height * 0.4f)),
                            null, Color.White, 0.0f, Vector2.Zero, starScale, SpriteEffects.None, 0.0f);
                    if (collect == 3)
                        spriteBatch.Draw(mStar, new Vector2(rect.Center.X - (mStar.Width * starScale) + (mStar.Width * 0.4f), rect.Bottom - (mStar.Height * 0.6f)),
                            null, Color.White, 0.0f, Vector2.Zero, starScale, SpriteEffects.None, 0.0f);

                    //DEATH SCORE
                    if (death >= 1)
                        spriteBatch.Draw(mStar, new Vector2(rect.Right - (2 * mStar.Width * starScale), rect.Bottom - (mStar.Height * 0.8f)),
                            null, Color.White, 0.0f, Vector2.Zero, starScale, SpriteEffects.None, 0.0f);
                    if (death >= 2)
                        spriteBatch.Draw(mStar, new Vector2(rect.Right - (2 * mStar.Width * starScale), rect.Bottom - (mStar.Height * 0.4f)),
                            null, Color.White, 0.0f, Vector2.Zero, starScale, SpriteEffects.None, 0.0f);
                    if (death == 3)
                        spriteBatch.Draw(mStar, new Vector2(rect.Right - (2 * mStar.Width * starScale) + (mStar.Width * 0.4f), rect.Bottom - (mStar.Height * 0.6f)),
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
        private Level mLevel;
        private Texture2D mThumbnail;
        private bool mUnlocked = false;
        private int mTimerStar;
        private int mCollectionStar;
        private int mDeathStar;

        public Level Level
        { get { return mLevel; } }
        public Texture2D Thumbnail
        { get { return mThumbnail; } }
        public bool Unlocked
        { get { return mUnlocked; } }
        public int TimerStar
        { get { return mTimerStar; } }
        public int CollectionStar
        { get { return mCollectionStar; } }
        public int DeathStar
        { get { return mDeathStar; } }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="levelInfo"></param>
        /// <param name="controls"></param>
        /// <param name="graphics"></param>
        public LevelChoice(XElement levelInfo, IControlScheme controls, ContentManager content, GraphicsDevice graphics)
        {
            foreach(XElement element in levelInfo.Elements())
            {
                if (element.Name == XmlKeys.LEVEL_NAME)
                {
                    mLevel = new Level(LevelSelect.LEVEL_DIRECTORY + element.Value.ToString() + ".xml", controls, graphics.Viewport);
                    
#if XBOX360
                    mThumbnail = content.Load<Texture2D>("Levels\\Thumbnail\\" + element.Value.ToString());
#else
                    FileStream filestream;
                    try
                    {
                        filestream = new FileStream(LevelSelect.LEVEL_THUMBS_DIRECTORY + element.Value.ToString() + ".png", FileMode.Open);                       
                    }
                    catch (IOException e)
                    {
                        filestream = new FileStream(LevelSelect.LEVEL_THUMBS_DIRECTORY + "..\\..\\..\\Content\\Images\\Error.png", FileMode.Open);
                    }
                    mThumbnail = Texture2D.FromStream(graphics, filestream);
                    filestream.Close();
#endif
                }
                if (element.Name == XmlKeys.UNLOCKED)
                    mUnlocked = element.Value == Import_Code.XmlKeys.TRUE;

                if (element.Name == XmlKeys.TIMERSTAR)
                    mTimerStar = mLevel.TimerStar = Convert.ToInt32(element.Value);

                if (element.Name == XmlKeys.COLLECTIONSTAR)
                    mCollectionStar = mLevel.CollectionStar = Convert.ToInt32(element.Value);

                if (element.Name == XmlKeys.DEATHSTAR)
                    mDeathStar = mLevel.DeathStar = Convert.ToInt32(element.Value);
            }
        }

        /// <summary>
        /// Resets this level choice to unlocked/locked depending on rUnlock, and resets scores to 0.
        /// </summary>
        /// <param name="rUnlock">True if level is locked, false otherwise</param>
        public void Reset(bool rUnlock)
        {
            mUnlocked = rUnlock;
            mTimerStar = mCollectionStar = mDeathStar = 0;
            mLevel.ResetScores();
        }

        /// <summary>
        /// Export an XElement of this level choice
        /// </summary>
        /// 
        public XElement Export()
        {
            SubmitScore(mLevel.TimerStar, mLevel.CollectionStar, mLevel.DeathStar);
            string xUnlock = XmlKeys.FALSE;
            if (mUnlocked)
                xUnlock = XmlKeys.TRUE;

            XElement xLevel = new XElement(XmlKeys.LEVEL_DATA,
                new XElement(XmlKeys.LEVEL_NAME, mLevel.Name),
                new XElement(XmlKeys.UNLOCKED, xUnlock),
                new XElement(XmlKeys.TIMERSTAR, mTimerStar.ToString()),
                new XElement(XmlKeys.COLLECTIONSTAR, mCollectionStar.ToString()),
                new XElement(XmlKeys.DEATHSTAR, mDeathStar.ToString()));

            return xLevel;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Unlock()
        {
            mUnlocked = true;
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
            if (timerStar > mTimerStar)
            {
                if (timerStar > 3)
                    mTimerStar = 3;
                else if (timerStar < 0)
                    mTimerStar = 0;
                else
                    mTimerStar = timerStar;
            }

            if (collectionStar > mCollectionStar)
            {
                if (collectionStar > 3)
                    mCollectionStar = 3;
                else if (collectionStar < 0)
                    mCollectionStar = 0;
                else
                    mCollectionStar = collectionStar;
            }

            if (deathStar > mDeathStar)
            {
                if (deathStar > 3)
                    mDeathStar = 3;
                else if (deathStar < 0)
                    mDeathStar = 0;
                else
                    mDeathStar = deathStar;
            }

        }
    }
}
