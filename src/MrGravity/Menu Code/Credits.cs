using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MrGravity.MISC_Code;

namespace MrGravity.Menu_Code
{
    internal class Credits
    {
        private readonly Dictionary<string, string[]> _mTitles;
        private Texture2D _mTitle;
        private SpriteFont _mFontBig;
        private SpriteFont _mFontSmall;
        private Texture2D _mBack;
        private Texture2D _mBackground;

        private Texture2D _mSmile;
        private Texture2D _mGirlSmile;
        private Texture2D _mSad;
        private Texture2D _mGirlSad;

        private Texture2D _mEae;
        private Texture2D _mULogo;

        private readonly Level _mBackgroundLevel;

        private readonly IControlScheme _mControls;
        private readonly GraphicsDeviceManager _mGraphics;

        private float _mTopY;

        /// <summary>
        /// Constructor for the credits screen. Sets up the list of names and categories to scrolll up
        /// </summary>
        /// <param name="controlScheme">Player's method of control</param>
        /// <param name="graphics">Graphics Manager for the game</param>
        public Credits(IControlScheme controlScheme, GraphicsDeviceManager graphics)
        {
            _mControls = controlScheme;
            _mGraphics = graphics;
            _mTopY = _mGraphics.GraphicsDevice.Viewport.TitleSafeArea.Bottom;

            _mBackgroundLevel = Level.MainMenuLevel("Content\\Levels\\MainMenu.xml", _mControls, _mGraphics.GraphicsDevice.Viewport, _mGraphics.GraphicsDevice.Viewport.Bounds);
            _mBackgroundLevel.StartingPoint = new Vector2(_mGraphics.GraphicsDevice.Viewport.Bounds.Center.X + _mGraphics.GraphicsDevice.Viewport.Bounds.Center.X * .2f,
                                                        _mGraphics.GraphicsDevice.Viewport.Bounds.Center.Y + _mGraphics.GraphicsDevice.Viewport.Bounds.Center.Y * .1f);
            //Easily add names and titles here
            _mTitles = new Dictionary<string, string[]>();
            _mTitles.Add("Angry Newton Productions", new[] { "", "Angry" });
            _mTitles.Add("Developed At", new[] { "University Of Utah; Senior EAE Capstone Class" });
            _mTitles.Add("Executive Producers", new[]{"Roger Altizer", "Dr. Bob Kessler"});
            _mTitles.Add("Original Concept & Design", new[] { "Tyler Robinson" });
            _mTitles.Add("Team Lead", new[] { "Curtis Taylor" });
            _mTitles.Add("-Programming-", new[]{ "", "Surprise"});
            _mTitles.Add("Lead Programmer", new[] { "Tyler Robinson" });
            _mTitles.Add("Programmers", new[] { "Curtis Taylor", "Nate Bradford", "Jeremy Heintz", "Casey Spencer", "Kamron Egan", "Morgan Reynolds" });
            _mTitles.Add("Xbox Tech", new[] { "Kamron Egan" });
            _mTitles.Add("-Art-", new[] { "", "Laugh" });
            _mTitles.Add("Lead Artist", new[] { "Lukas Black" });
            _mTitles.Add("Artists", new[] { "Nate Bradford", "Jeremy Heintz", "Casey Spencer" });
            _mTitles.Add("Animators", new[] { "Lukas Black", "Nate Bradford", "Jeremy Heintz", "Kamron Egan" });
            _mTitles.Add("-Design & Development-", new[] { "", "Bored" } );
            _mTitles.Add("Character Designer", new[] { "Lukas Black" });
            _mTitles.Add("Level Designers", new[]{"Nate Bradford", "Curtis Taylor", "Jeremy Heintz", "Morgan Reynolds", "Steven Doxey", "Casey Spencer"});
            _mTitles.Add("Game Mechanics", new[] { "Tyler Robinson", "Curtis Taylor", "Morgan Reynolds" });
            _mTitles.Add("-Music & Sound-", new[] { "", "Worry" });
            _mTitles.Add("Music Lead", new[] { "Steven Doxey" });
            _mTitles.Add("Musicians", new[] { "Michelle MacArt", "Cuyler Stuwe" });
            _mTitles.Add("Sound FX Lead", new[] { "Steven Doxey" });
            _mTitles.Add("Sound FX Composers", new[] { "Michelle MacArt", "Tyler Robinson", "Morgan Reynolds" });
            _mTitles.Add("-Find Us-", new[] { "", "Laugh2" });
            _mTitles.Add("Our Official Website", new[] { "http://www.mrgravity.com" });
            _mTitles.Add("Our Email(Email here for Contest)", new[] { "angrynewtondevelopers@live.com" });
            _mTitles.Add("EAE Website", new[] { "http://eae.utah.edu" });
            _mTitles.Add("Like us on Facebook", new[] { "http://www.facebook.com/", "Search Mr. Gravity" });
            _mTitles.Add("Follow us on Twitter", new[] { "http://www.twitter.com/AngryNewton", "Or @AngryNewton"});
        }

        /// <summary>
        /// Loads the images and fonts needed for this screen
        /// </summary>
        /// <param name="content">Content manager this game is using</param>
        public void Load(ContentManager content)
        {
            _mTitle = content.Load<Texture2D>("Images\\Menu\\Mr_Gravity");
            _mBack = content.Load<Texture2D>("Images\\Menu\\Main\\BackSelected");
            _mFontBig = content.Load<SpriteFont>("Fonts\\QuartzLarge");
            _mFontSmall = content.Load<SpriteFont>("Fonts\\QuartzSmall");
            _mBackground = content.Load<Texture2D>("Images\\Menu\\backgroundSquares1");
            _mSmile = content.Load<Texture2D>("Images\\Player\\Smile");
            _mGirlSmile = content.Load<Texture2D>("Images\\Player\\GirlSmile");
            _mSad = content.Load<Texture2D>("Images\\Player\\Sad");
            _mGirlSad = content.Load<Texture2D>("Images\\Player\\GirlSad");
            _mEae = content.Load<Texture2D>("Images\\Menu\\Credits\\EAEIconRounded");
            _mULogo = content.Load<Texture2D>("Images\\Menu\\Credits\\Primary-University-Logo");
            _mBackgroundLevel.Load(content);
        }

        /// <summary>
        /// Update process of this screen. Moves the credits up one pixel
        /// </summary>
        /// <param name="gametime">Curent gametime(unnecessary)</param>
        /// <param name="states">State of the current game</param>
        public void Update(GameTime gametime, ref GameStates states)
        {
            if (_mControls.IsAPressed(false) || 
                _mControls.IsStartPressed(false) || 
                _mControls.IsBackPressed(false) ||
                _mControls.IsBPressed(false))
            {
                _mTopY = _mGraphics.GraphicsDevice.Viewport.TitleSafeArea.Bottom;
                states = GameStates.MainMenu;
            }

            _mTopY -= 1.25f;

            _mBackgroundLevel.Update(gametime, ref states);
        }

        /// <summary>
        /// Draws the credits on screen
        /// </summary>
        /// <param name="gametime">Curent gametime(unnecessary)</param>
        /// <param name="spriteBatch">Canvas the screen is drawing on</param>
        /// <param name="scale">Not sure yet :)</param>
        public void Draw(GameTime gametime, SpriteBatch spriteBatch, Matrix scale)
        {
            spriteBatch.Begin(SpriteSortMode.Immediate,
                BlendState.AlphaBlend,
                SamplerState.LinearClamp,
                DepthStencilState.None,
                RasterizerState.CullCounterClockwise,
                null,
                scale);

            spriteBatch.Draw(_mBackground, new Rectangle(0, 0, _mGraphics.GraphicsDevice.Viewport.Width, _mGraphics.GraphicsDevice.Viewport.Height), Color.White);
       
            var mSize = new float[2] { _mGraphics.GraphicsDevice.Viewport.TitleSafeArea.Width / (float)_mGraphics.GraphicsDevice.Viewport.Width, _mGraphics.GraphicsDevice.Viewport.TitleSafeArea.Height / (float)_mGraphics.GraphicsDevice.Viewport.Height };
            //Draws the back button. TODO - Better back button and probably better placement
            spriteBatch.Draw(_mBack, new Rectangle(_mGraphics.GraphicsDevice.Viewport.TitleSafeArea.Right - (int)(_mBack.Width * mSize[0]) / 2,
                                                _mGraphics.GraphicsDevice.Viewport.TitleSafeArea.Bottom - (int)(_mBack.Height * mSize[1]) / 2, (int)(_mBack.Width * mSize[0]) / 2, (int)(_mBack.Height * mSize[1]) / 2), Color.White);

            spriteBatch.Draw(_mSmile, new Rectangle(_mGraphics.GraphicsDevice.Viewport.TitleSafeArea.Center.X - _mTitle.Width / 4 - _mSmile.Width, (int)_mTopY - _mSmile.Height / 2, _mSmile.Width, _mSmile.Height), Color.White);
            spriteBatch.Draw(_mGirlSmile, new Rectangle(_mGraphics.GraphicsDevice.Viewport.TitleSafeArea.Center.X + _mTitle.Width / 4, (int)_mTopY - _mGirlSmile.Height / 2,_mGirlSmile.Width, _mGirlSmile.Height), Color.White);
            //Draw the game title before the words. Scrolls too
            spriteBatch.Draw(_mTitle, new Rectangle(_mGraphics.GraphicsDevice.Viewport.TitleSafeArea.Center.X - _mTitle.Width / 4, (int)_mTopY - _mTitle.Height / 4,_mTitle.Width/2,_mTitle.Height/2),
                Color.White);

            //Make room for the game title
            var top = (int)_mTopY+200;

            //Goes through all the headers
            foreach (var key in _mTitles.Keys)
            {
                //Base and highlight for the header of the names under it
                spriteBatch.DrawString(_mFontBig, key, new Vector2(GetTextXLocation(key, true), top), Color.White);
                spriteBatch.DrawString(_mFontBig, key, 
                    Vector2.Add(new Vector2(GetTextXLocation(key, true), top), new Vector2(2,2)), Color.SteelBlue);

                //If the first item under this key is empty, then this is an image block. 
                if (_mTitles[key][0] == "")
                {
                    spriteBatch.Draw(PlayerFaces.FromString(_mTitles[key][1]), new Rectangle(GetTextXLocation(key, true) - 3*_mSad.Width/2, top, _mSad.Width, _mSad.Height), Color.White);
                    spriteBatch.Draw(PlayerFaces.FromString("Girl" + _mTitles[key][1]), new Rectangle((int)(GetTextXLocation(key, true) + _mFontBig.MeasureString(key).X) + _mSad.Width/2, top, _mSad.Width, _mSad.Height), Color.White);
                    top += 100;
                    continue;
                }

                //Goes through all the titles under that header and draws it
                foreach (var name in _mTitles[key])
                {
                    top += 40;
                    spriteBatch.DrawString(_mFontSmall, name, new Vector2(GetTextXLocation(name, false), top), Color.White);
                    spriteBatch.DrawString(_mFontSmall, name, new Vector2(GetTextXLocation(name, false)+2, top+2), Color.DimGray);
                }
                //Clear spacing between headers
                top += 100;
                
            }

            spriteBatch.Draw(_mULogo, new Rectangle(_mGraphics.GraphicsDevice.Viewport.TitleSafeArea.Center.X - 175, top, 125, 100),
                 Color.White);

            spriteBatch.Draw(_mEae, new Rectangle(_mGraphics.GraphicsDevice.Viewport.TitleSafeArea.Center.X-25, top, 200, 100),
                Color.White);
            top += 200;

            spriteBatch.Draw(_mSad, new Rectangle(_mGraphics.GraphicsDevice.Viewport.TitleSafeArea.Center.X - _mSad.Width, top, _mSad.Width, _mSad.Height), Color.White);
            spriteBatch.Draw(_mGirlSad, new Rectangle(_mGraphics.GraphicsDevice.Viewport.TitleSafeArea.Center.X, top, _mGirlSad.Width, _mGirlSad.Height), Color.White);

            
            

            //If bottom has been reached, reset to the top
            if (top+_mSad.Height <= _mGraphics.GraphicsDevice.Viewport.TitleSafeArea.Top)
                _mTopY = _mGraphics.GraphicsDevice.Viewport.TitleSafeArea.Bottom;
            spriteBatch.End();
        }

        public void DrawLevel(SpriteBatch spriteBatch, GameTime gameTime, Matrix scale)
        {
            _mBackgroundLevel.Draw(spriteBatch, gameTime, scale);
        }

        /// <summary>
        /// Gets the x location of the text so that is exactly center for the string that is given
        /// </summary>
        /// <param name="text">The text to center</param>
        /// <param name="header">If it is a header(bigger text) or not</param>
        /// <returns></returns>
        private int GetTextXLocation(string text, bool header)
        {
            Vector2 size = _mFontSmall.MeasureString(text);
            if(header)
                size = _mFontBig.MeasureString(text);

            return _mGraphics.GraphicsDevice.Viewport.TitleSafeArea.Center.X - (int)size.X / 2;
        }

    }
}
