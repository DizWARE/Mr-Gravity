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
    class LevelSelect
    {
        public static string LEVEL_DIRECTORY = "..\\..\\..\\Content\\Levels\\";
        public static string LEVEL_THUMBS_DIRECTORY = LEVEL_DIRECTORY + "Thumbnail\\";
        public static string LEVEL_LIST = LEVEL_DIRECTORY + "Info\\LevelList.xml";

        IControlScheme mControls;

        List<LevelChoice> mLevels;

        Texture2D mSelectBox;

        XElement mLevelInfo;

        int mCurrentIndex = 0;

        ContentManager mContent;

        public LevelSelect(IControlScheme controlScheme)
        {
            mControls = controlScheme;
            mLevels = new List<LevelChoice>();
            mLevelInfo = XElement.Load(LEVEL_LIST);
        }

        public void Load(ContentManager content, GraphicsDevice graphics)
        {
            foreach (XElement level in mLevelInfo.Elements())
                mLevels.Add(new LevelChoice(level,mControls,graphics));

            mContent = content;
            mSelectBox = content.Load<Texture2D>("menu/selectbox");
        }

        public void Update(GameTime gameTime, ref GameStates gameState, ref Level currentLevel)
        {
            if (mControls.isLeftPressed(false))
                mCurrentIndex = (mCurrentIndex - 1);
            else if (mControls.isRightPressed(false))
                mCurrentIndex = (mCurrentIndex + 1) % mLevels.Count;
            else if(mControls.isUpPressed(false))
                mCurrentIndex = (mCurrentIndex - 5);
            else if(mControls.isDownPressed(false))
                mCurrentIndex = (mCurrentIndex + 5) % mLevels.Count;

            if (mCurrentIndex < 0) mCurrentIndex += mLevels.Count;

            if(mControls.isAPressed(false))
            {
                currentLevel = mLevels[mCurrentIndex].Level;
                currentLevel.Load(mContent);
                gameState = GameStates.In_Game;
                mCurrentIndex = 0;
            }

            if (mControls.isBackPressed(false))
            {
                gameState = GameStates.Main_Menu;
                mCurrentIndex = 0;
            }
        }

        public void Draw(SpriteBatch spriteBatch, GraphicsDeviceManager graphics)
        {
            spriteBatch.Begin();
            Vector2 padding = new Vector2(20,50);
            Vector2 currentLocation = padding;
            Vector2 size = new Vector2(200, 200);

            int index = 0;

            foreach (LevelChoice levelChoice in mLevels)
            {
                if (currentLocation.X + size.X + padding.X >= graphics.GraphicsDevice.Viewport.Width)
                {
                    currentLocation.X = padding.X;
                    currentLocation.Y += padding.Y + size.Y;
                }
                currentLocation.X += padding.X;
                Rectangle rect = new Rectangle((int)currentLocation.X, (int)currentLocation.Y, (int)size.X, (int)size.Y);

                spriteBatch.Draw(levelChoice.Thumbnail, rect, Color.White);
                if (index == mCurrentIndex) spriteBatch.Draw(mSelectBox, rect, Color.White);

                currentLocation.X += size.X + padding.X;
                index++;
            }

            spriteBatch.End();
        }
    }

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
    }
}
