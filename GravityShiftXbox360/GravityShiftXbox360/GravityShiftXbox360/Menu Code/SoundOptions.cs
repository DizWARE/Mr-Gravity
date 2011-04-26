using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace GravityShift
{
    class SoundOptions
    {
        public enum MenuChoices { Mute, Back }

        Dictionary<MenuChoices, Texture2D> mUnselected;
        Dictionary<MenuChoices, Texture2D> mSelected;

        Texture2D mTitle;
        Texture2D mBack;
        Texture2D mBackground;
        Texture2D mMute;
        Texture2D mUnMute;

        IControlScheme mControls;
        GraphicsDeviceManager mGraphics;

        MenuChoices mCurrentChoice = MenuChoices.Mute;

        bool mMuted = false;

        /// <summary>
        /// Constructor for SoundOptions screen
        /// </summary>
        /// <param name="controlScheme">Players method of control</param>
        /// <param name="graphics">Graphics manager for the game</param>
        public SoundOptions(IControlScheme controlScheme, GraphicsDeviceManager graphics)
        {
            mControls = controlScheme;
            mGraphics = graphics;

            mUnselected = new Dictionary<MenuChoices, Texture2D>();
            mSelected = new Dictionary<MenuChoices, Texture2D>();

        }

        /// <summary>
        /// Loads images needed for this screen
        /// </summary>
        /// <param name="content">Content manager used for this game</param>
        public void Load(ContentManager content)
        {
            mTitle = content.Load<Texture2D>("Images\\Menu\\Mr_Gravity");
            mBack = content.Load<Texture2D>("Images\\Menu\\Main\\BackSelected");
            mBackground = content.Load<Texture2D>("Images\\Menu\\backgroundSquares1");

            mUnselected.Add(MenuChoices.Mute, content.Load<Texture2D>("Images\\Menu\\Main\\MuteUnselected"));
            mUnselected.Add(MenuChoices.Back, content.Load<Texture2D>("Images\\Menu\\Main\\BackUnselected"));

            mSelected.Add(MenuChoices.Mute, content.Load<Texture2D>("Images\\Menu\\Main\\MuteSelected"));
            mSelected.Add(MenuChoices.Back, content.Load<Texture2D>("Images\\Menu\\Main\\BackSelected"));

            mMute = content.Load<Texture2D>("Images/Menu/Main/Mute");
            mUnMute = content.Load<Texture2D>("Images/Menu/Main/UnMute");

        }

        /// <summary>
        /// Update process of this screen.  Responds to player input to go back to the options screen.
        /// </summary>
        /// <param name="gametime">Current gameTime</param>
        /// <param name="states">Current state of the game</param>
        public void Update(GameTime gametime, ref GameStates states)
        {

            if (mControls.isBackPressed(false))
                states = GameStates.Options;
            if (mControls.isAPressed(false) || mControls.isStartPressed(false))
            {
                if (mCurrentChoice == MenuChoices.Back)
                {
                    states = GameStates.Options;
                }
                if (mCurrentChoice == MenuChoices.Mute)
                {
                    mMuted = !mMuted;
                }
            }
            if (mControls.isBPressed(false) || mControls.isBackPressed(false))
                states = GameStates.Options;

            if (mMuted)
            {
                GameSound.volume = GameSound.menuMusic_title.Volume = 0.0f;
            }
            else
            {
                GameSound.volume = GameSound.menuMusic_title.Volume = 1.0f;
            }

            if (mControls.isUpPressed(false))
            {
                if (mCurrentChoice != MenuChoices.Mute)
                {
                    GameSound.menuSound_rollover.Play(GameSound.volume, 0.0f, 0.0f);
                    mCurrentChoice = MenuChoices.Mute;
                }
                
            }

            if (mControls.isDownPressed(false))
            {
                if (mCurrentChoice != MenuChoices.Back)
                {
                    GameSound.menuSound_rollover.Play(GameSound.volume, 0.0f, 0.0f);
                    mCurrentChoice = MenuChoices.Back;
                }
                
            }

        }

        /// <summary>
        /// Draws the controller screen
        /// </summary>
        /// <param name="gametime">Current gametime</param>
        /// <param name="spriteBatch">Canvas the screen is drawing on</param>
        /// <param name="scale">scale factor</param>
        public void Draw(GameTime gametime, SpriteBatch spriteBatch, Matrix scale)
        {
            spriteBatch.Begin(SpriteSortMode.Immediate,
                BlendState.AlphaBlend,
                SamplerState.LinearClamp,
                DepthStencilState.None,
                RasterizerState.CullCounterClockwise,
                null,
                scale);

            Rectangle mScreenRect = mGraphics.GraphicsDevice.Viewport.TitleSafeArea;

            float[] mSize = new float[2] { (float)mScreenRect.Width / (float)mGraphics.GraphicsDevice.Viewport.Width, (float)mScreenRect.Height / (float)mGraphics.GraphicsDevice.Viewport.Height };

#if XBOX360
            Point center = mGraphics.GraphicsDevice.Viewport.TitleSafeArea.Center;
            spriteBatch.Draw(mBackground, new Rectangle(0, 0, mGraphics.GraphicsDevice.Viewport.Width, mGraphics.GraphicsDevice.Viewport.Height), Color.White);
            spriteBatch.Draw(mTitle, new Rectangle(center.X - (int)(mTitle.Width * mSize[0]) / 2, mGraphics.GraphicsDevice.Viewport.TitleSafeArea.Top, (int)(mTitle.Width * mSize[0]), (int)(mTitle.Height * mSize[1])), Color.White);

            for (int i = 0; i < 2; i++)
            {
                MenuChoices choice = (MenuChoices)i;
                Rectangle mChoiceRectangle;
                Texture2D mTexture;
                if (choice == mCurrentChoice)
                {
                    mTexture = mSelected[choice];
                    mChoiceRectangle = GetRegion(choice, mTexture);
                    spriteBatch.Draw(mSelected[choice], mChoiceRectangle, Color.White);
                }
                else
                {
                    mTexture = mUnselected[choice];
                    mChoiceRectangle = GetRegion(choice, mTexture);
                    spriteBatch.Draw(mUnselected[choice], mChoiceRectangle, Color.White);

                }

                if (choice == MenuChoices.Mute)
                {
                    if (mMuted)
                    {
                        spriteBatch.Draw(mMute, new Rectangle(center.X + (int)(mTexture.Width * mSize[0]) / 2, center.Y - (int)(mMute.Height * mSize[1]) - (int)(mTexture.Height * mSize[1] - mMute.Height * mSize[1]) / 2, (int)(mMute.Width * mSize[0]), (int)(mMute.Height * mSize[1])), Color.White);
                    }
                    else
                    {
                        spriteBatch.Draw(mUnMute, new Rectangle(center.X + (int)(mTexture.Width * mSize[0]) / 2, center.Y - (int)(mUnMute.Height * mSize[1]) - (int)(mTexture.Height * mSize[1] - mUnMute.Height * mSize[1]) / 2, (int)(mUnMute.Width * mSize[0]), (int)(mUnMute.Height * mSize[1])), Color.White);
                    }
                }
            }
#else

            Point center = mGraphics.GraphicsDevice.Viewport.TitleSafeArea.Center;
            spriteBatch.Draw(mBackground, new Rectangle(0, 0, mGraphics.GraphicsDevice.Viewport.Width, mGraphics.GraphicsDevice.Viewport.Height), Color.White);
            spriteBatch.Draw(mTitle, new Rectangle(center.X - (int)(mTitle.Width * mSize[0]) / 2, mGraphics.GraphicsDevice.Viewport.TitleSafeArea.Top, (int)(mTitle.Width * mSize[0]), (int)(mTitle.Height * mSize[1])), Color.White);

            foreach (MenuChoices choice in Enum.GetValues(typeof(MenuChoices)))
            {
                Rectangle mChoiceRectangle;
                Texture2D mTexture;
                if (choice == mCurrentChoice)
                {
                    mTexture = mSelected[choice];
                    mChoiceRectangle = GetRegion(choice, mTexture);
                    spriteBatch.Draw(mSelected[choice], mChoiceRectangle, Color.White);
                }

                else
                {
                    mTexture = mUnselected[choice];
                    mChoiceRectangle = GetRegion(choice, mTexture);
                    spriteBatch.Draw(mUnselected[choice], mChoiceRectangle, Color.White);

                }

                if (choice == MenuChoices.Mute)
                {
                    if (mMuted)
                    {
                        spriteBatch.Draw(mMute, new Rectangle(center.X + (int)(mTexture.Width * mSize[0]) / 2, center.Y - (int)(mMute.Height * mSize[1]) - (int)(mTexture.Height * mSize[1] - mMute.Height * mSize[1]) / 2, (int)(mMute.Width * mSize[0]), (int)(mMute.Height * mSize[1])), Color.White);
                    }
                    else
                    {
                        spriteBatch.Draw(mUnMute, new Rectangle(center.X + (int)(mTexture.Width * mSize[0]) / 2, center.Y - (int)(mUnMute.Height * mSize[1]) - (int)(mTexture.Height * mSize[1] - mUnMute.Height * mSize[1]) / 2, (int)(mUnMute.Width * mSize[0]), (int)(mUnMute.Height * mSize[1])), Color.White);
                    }
                }
                    

            }

#endif


            //spriteBatch.Draw(mBack, new Rectangle(mScreenRect.Center.X - (int)(mBack.Width * mSize[0]) / 2, mScreenRect.Bottom - (int)(mBack.Height * mSize[1]), (int)(mBack.Width * mSize[0]), (int)(mBack.Height * mSize[1])), Color.White);

            spriteBatch.End();
        }

        public Rectangle GetRegion(MenuChoices choice, Texture2D texture)
        {
            Viewport viewport = mGraphics.GraphicsDevice.Viewport;

            float[] mSize = new float[2] { (float)viewport.TitleSafeArea.Width / (float)mGraphics.GraphicsDevice.Viewport.Width, (float)viewport.TitleSafeArea.Height / (float)mGraphics.GraphicsDevice.Viewport.Height };

            if (choice == MenuChoices.Mute)
                return new Rectangle(viewport.TitleSafeArea.Center.X - ((int)(texture.Width * mSize[0]) / 2) - ((int)(mMute.Width * mSize[0]) / 2),
                    viewport.TitleSafeArea.Center.Y - (int)(texture.Height * mSize[1]), (int)(texture.Width * mSize[0]), (int)(texture.Height * mSize[1]));
            if (choice == MenuChoices.Back)
                return new Rectangle(viewport.TitleSafeArea.Center.X - ((int)(texture.Width * mSize[0]) / 2),
                    viewport.TitleSafeArea.Center.Y + (int)(texture.Height * mSize[1]), (int)(texture.Width * mSize[0]), (int)(texture.Height * mSize[1]));
            return new Rectangle();
        }
    }
}
