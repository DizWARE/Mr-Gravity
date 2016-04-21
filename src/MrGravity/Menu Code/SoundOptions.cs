using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MrGravity.MISC_Code;

namespace MrGravity.Menu_Code
{
    internal class SoundOptions
    {
        public enum MenuChoices { Mute, Back }

        private readonly Dictionary<MenuChoices, Texture2D> _mUnselected;
        private readonly Dictionary<MenuChoices, Texture2D> _mSelected;

        private Texture2D _mTitle;
        private Texture2D _mBack;
        private Texture2D _mBackground;
        private Texture2D _mMute;
        private Texture2D _mUnMute;

        private readonly IControlScheme _mControls;
        private readonly GraphicsDeviceManager _mGraphics;

        private MenuChoices _mCurrentChoice = MenuChoices.Mute;

        private bool _mMuted;

        /// <summary>
        /// Constructor for SoundOptions screen
        /// </summary>
        /// <param name="controlScheme">Players method of control</param>
        /// <param name="graphics">Graphics manager for the game</param>
        public SoundOptions(IControlScheme controlScheme, GraphicsDeviceManager graphics)
        {
            _mControls = controlScheme;
            _mGraphics = graphics;

            _mUnselected = new Dictionary<MenuChoices, Texture2D>();
            _mSelected = new Dictionary<MenuChoices, Texture2D>();

        }

        /// <summary>
        /// Loads images needed for this screen
        /// </summary>
        /// <param name="content">Content manager used for this game</param>
        public void Load(ContentManager content)
        {
            _mTitle = content.Load<Texture2D>("Images\\Menu\\Mr_Gravity");
            _mBack = content.Load<Texture2D>("Images\\Menu\\Main\\BackSelected");
            _mBackground = content.Load<Texture2D>("Images\\Menu\\backgroundSquares1");

            _mUnselected.Add(MenuChoices.Mute, content.Load<Texture2D>("Images\\Menu\\Main\\MuteUnselected"));
            _mUnselected.Add(MenuChoices.Back, content.Load<Texture2D>("Images\\Menu\\Main\\BackUnselected"));

            _mSelected.Add(MenuChoices.Mute, content.Load<Texture2D>("Images\\Menu\\Main\\MuteSelected"));
            _mSelected.Add(MenuChoices.Back, content.Load<Texture2D>("Images\\Menu\\Main\\BackSelected"));

            _mMute = content.Load<Texture2D>("Images/Menu/Main/Mute");
            _mUnMute = content.Load<Texture2D>("Images/Menu/Main/UnMute");

        }

        /// <summary>
        /// Update process of this screen.  Responds to player input to go back to the options screen.
        /// </summary>
        /// <param name="gametime">Current gameTime</param>
        /// <param name="states">Current state of the game</param>
        public void Update(GameTime gametime, ref GameStates states)
        {

            if (_mControls.IsBackPressed(false))
                states = GameStates.Options;
            if (_mControls.IsAPressed(false) || _mControls.IsStartPressed(false))
            {
                if (_mCurrentChoice == MenuChoices.Back)
                {
                    states = GameStates.Options;
                }
                if (_mCurrentChoice == MenuChoices.Mute)
                {
                    _mMuted = !_mMuted;
                }
            }
            if (_mControls.IsBPressed(false) || _mControls.IsBackPressed(false))
                states = GameStates.Options;

            if (_mMuted)
            {
                GameSound.Volume = GameSound.MenuMusicTitle.Volume = 0.0f;
            }
            else
            {
                GameSound.Volume = GameSound.MenuMusicTitle.Volume = 1.0f;
            }

            if (_mControls.IsUpPressed(false))
            {
                if (_mCurrentChoice != MenuChoices.Mute)
                {
                    GameSound.MenuSoundRollover.Play(GameSound.Volume, 0.0f, 0.0f);
                    _mCurrentChoice = MenuChoices.Mute;
                }
                
            }

            if (_mControls.IsDownPressed(false))
            {
                if (_mCurrentChoice != MenuChoices.Back)
                {
                    GameSound.MenuSoundRollover.Play(GameSound.Volume, 0.0f, 0.0f);
                    _mCurrentChoice = MenuChoices.Back;
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

            Rectangle mScreenRect = _mGraphics.GraphicsDevice.Viewport.TitleSafeArea;

            var mSize = new float[2] { mScreenRect.Width / (float)_mGraphics.GraphicsDevice.Viewport.Width, mScreenRect.Height / (float)_mGraphics.GraphicsDevice.Viewport.Height };

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

            Point center = _mGraphics.GraphicsDevice.Viewport.TitleSafeArea.Center;
            spriteBatch.Draw(_mBackground, new Rectangle(0, 0, _mGraphics.GraphicsDevice.Viewport.Width, _mGraphics.GraphicsDevice.Viewport.Height), Color.White);
            spriteBatch.Draw(_mTitle, new Rectangle(center.X - (int)(_mTitle.Width * mSize[0]) / 2, _mGraphics.GraphicsDevice.Viewport.TitleSafeArea.Top, (int)(_mTitle.Width * mSize[0]), (int)(_mTitle.Height * mSize[1])), Color.White);

            foreach (MenuChoices choice in Enum.GetValues(typeof(MenuChoices)))
            {
                Rectangle mChoiceRectangle;
                Texture2D mTexture;
                if (choice == _mCurrentChoice)
                {
                    mTexture = _mSelected[choice];
                    mChoiceRectangle = GetRegion(choice, mTexture);
                    spriteBatch.Draw(_mSelected[choice], mChoiceRectangle, Color.White);
                }

                else
                {
                    mTexture = _mUnselected[choice];
                    mChoiceRectangle = GetRegion(choice, mTexture);
                    spriteBatch.Draw(_mUnselected[choice], mChoiceRectangle, Color.White);

                }

                if (choice == MenuChoices.Mute)
                {
                    if (_mMuted)
                    {
                        spriteBatch.Draw(_mMute, new Rectangle(center.X + (int)(mTexture.Width * mSize[0]) / 2, center.Y - (int)(_mMute.Height * mSize[1]) - (int)(mTexture.Height * mSize[1] - _mMute.Height * mSize[1]) / 2, (int)(_mMute.Width * mSize[0]), (int)(_mMute.Height * mSize[1])), Color.White);
                    }
                    else
                    {
                        spriteBatch.Draw(_mUnMute, new Rectangle(center.X + (int)(mTexture.Width * mSize[0]) / 2, center.Y - (int)(_mUnMute.Height * mSize[1]) - (int)(mTexture.Height * mSize[1] - _mUnMute.Height * mSize[1]) / 2, (int)(_mUnMute.Width * mSize[0]), (int)(_mUnMute.Height * mSize[1])), Color.White);
                    }
                }
                    

            }

#endif


            //spriteBatch.Draw(mBack, new Rectangle(mScreenRect.Center.X - (int)(mBack.Width * mSize[0]) / 2, mScreenRect.Bottom - (int)(mBack.Height * mSize[1]), (int)(mBack.Width * mSize[0]), (int)(mBack.Height * mSize[1])), Color.White);

            spriteBatch.End();
        }

        public Rectangle GetRegion(MenuChoices choice, Texture2D texture)
        {
            Viewport viewport = _mGraphics.GraphicsDevice.Viewport;

            var mSize = new float[2] { viewport.TitleSafeArea.Width / (float)_mGraphics.GraphicsDevice.Viewport.Width, viewport.TitleSafeArea.Height / (float)_mGraphics.GraphicsDevice.Viewport.Height };

            if (choice == MenuChoices.Mute)
                return new Rectangle(viewport.TitleSafeArea.Center.X - ((int)(texture.Width * mSize[0]) / 2) - ((int)(_mMute.Width * mSize[0]) / 2),
                    viewport.TitleSafeArea.Center.Y - (int)(texture.Height * mSize[1]), (int)(texture.Width * mSize[0]), (int)(texture.Height * mSize[1]));
            if (choice == MenuChoices.Back)
                return new Rectangle(viewport.TitleSafeArea.Center.X - ((int)(texture.Width * mSize[0]) / 2),
                    viewport.TitleSafeArea.Center.Y + (int)(texture.Height * mSize[1]), (int)(texture.Width * mSize[0]), (int)(texture.Height * mSize[1]));
            return new Rectangle();
        }
    }
}
