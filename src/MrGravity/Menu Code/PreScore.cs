using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MrGravity.MISC_Code;

namespace MrGravity.Menu_Code
{
    internal class PreScore
    {
        #region Member Variables

        private Rectangle _mScreenRect;

        private ContentManager _mContent;

        private readonly IControlScheme _mControls;

        #endregion

        #region Art

        private Texture2D _mTrans;
        private SpriteFont _mQuartz;

        #endregion

        private float _xCoord, _centerYCoord;
        private float _topYCoord2, _bottomYCoord2;
        private float _topYCoord3, _bottomYCoord3;

        private float _mScale;

        private bool _upToScale;
        private bool _pulse;

        private WorldSelect _mWorldSelect;

        public static  List<string> StarList;

        private double _elapsedTime;

        private string _gemString, _deathString, _timeString;

        private float _current;

        private bool _mDoOnce;

        public PreScore(IControlScheme controls)
        {
            _mControls = controls;
        }

        /*
         * Load
         *
         * Similar to a loadContent function. This function loads and 
         * initializes the variable and art used in the class.
         *
         * ContentManager content: the Content file used in the game.
         */
        public void Load(ContentManager content, GraphicsDevice graphics, WorldSelect worldSelect)
        {
            _mContent = content;

            _mScreenRect = graphics.Viewport.TitleSafeArea;

//            mCurrentLevel = new LevelInfo();
            _current = _mScreenRect.Right;

            _mTrans = content.Load<Texture2D>("Images/Menu/Pause/PausedTrans");
            _mQuartz = content.Load<SpriteFont>("Fonts/QuartzEvenLarger");

            _xCoord = _mScreenRect.Center.X - _mQuartz.MeasureString("COMPLETED DEATH CHALLENGE").X / 2;
            _centerYCoord = _mScreenRect.Center.Y - _mQuartz.MeasureString("COMPLETED TIME CHALLENGE").Y / 2;
            _topYCoord2 = _mScreenRect.Center.Y - _mQuartz.MeasureString("COMPLETED TIME CHALLENGE").Y;
            _bottomYCoord2 = _mScreenRect.Center.Y + _mQuartz.MeasureString("COMPLETED TIME CHALLENGE").Y;
            _topYCoord3 = _mScreenRect.Center.Y - _mQuartz.MeasureString("COMPLETED GEM CHALLENGE").Y * 2;
            _bottomYCoord3 = _mScreenRect.Center.Y + _mQuartz.MeasureString("COMPLETED DEATH CHALLENGE").Y;

            _mScale = 1.0f;

            _pulse = false;
            _upToScale = false;

            _gemString = "COMPLETED GEM CHALLENGE!";
            _timeString = "COMPLETED TIME CHALLENGE!";
            _deathString = "COMPLETED DEATH CHALLENGE!";

            _mDoOnce = false;

            StarList = new List<string>();
            StarList.Clear();

            _mWorldSelect = worldSelect;
        }

        /*
         * Update
         *
         * Updates the menu depending on what the user has selected.
         * It will handle the title, options, load and all other menu 
         * screens
         *
         * GameTime gameTime: The current game time variable
         */
        public void Update(GameTime gameTime, ref GameStates gameState, ref Level level)
        {
            _elapsedTime += gameTime.ElapsedGameTime.TotalSeconds;

            if (_mScale < 1 && !_upToScale)
            {
                _mScale += 0.05f;
            }
            else if (_mScale >= 1.0f && _mScale <= 1.02f && !_pulse)
            {
                _upToScale = false;
                _mScale += 0.001f;
                if (_mScale > 1.02f)
                    _pulse = true;
            }
            else if (_pulse)
            {
                _mScale -= 0.001f;
                if (_mScale <= 1.002f)
                    _pulse = false;
            }
            /* If the user selects one of the menu items */
            if (_mControls.IsAPressed(false) || _mControls.IsStartPressed(false) || _elapsedTime >= 3.0)
            {
                GameSound.MenuSoundSelect.Play(GameSound.Volume, 0.0f, 0.0f);

                /*Back To Level Selection*/
                gameState = GameStates.Score;

                Reset();
            }
        }

        private void Reset()
        {
            _pulse = false;
            _upToScale = false;

            _gemString = "COMPLETED GEM CHALLENGE!";
            _timeString = "COMPLETED TIME CHALLENGE!";
            _deathString = "COMPLETED DEATH CHALLENGE!";

            _mDoOnce = false;

            StarList.Clear();
            _current = _mScreenRect.Right;

            _elapsedTime = 0.0;
            _mScale = 1.0f;
        }

        /*
         * Draw
         *
         * This function will draw the current menu
         *
         * SpriteBatch spriteBatch: The current sprite batch used to draw
         * 
         * GraphicsDeviceManager graphics: The current graphics manager
         */
        public void Draw(SpriteBatch spriteBatch, GraphicsDeviceManager graphics, Matrix scale, Level currentLevel)
        {
            spriteBatch.Begin(SpriteSortMode.Immediate,
                BlendState.AlphaBlend,
                SamplerState.LinearClamp,
                DepthStencilState.None,
                RasterizerState.CullCounterClockwise,
                null,
                scale);

            var mSize = new float[2] { _mScreenRect.Width / (float)graphics.GraphicsDevice.Viewport.Width, _mScreenRect.Height / (float)graphics.GraphicsDevice.Viewport.Height };
            spriteBatch.Draw(_mTrans, graphics.GraphicsDevice.Viewport.Bounds, Color.White);

            if (!_mDoOnce)
            {
                if (currentLevel.CollectionStar == 3 && (_mWorldSelect.GetLevelCollect()) != 3)
                {
                    StarList.Add(_gemString);
                }
                if (currentLevel.TimerStar == 3 && (_mWorldSelect.GetLevelTime()) != 3)
                {
                    StarList.Add(_timeString);
                }
                if (currentLevel.DeathStar == 3 && (_mWorldSelect.GetLevelDeath()) != 3)
                {
                    StarList.Add(_deathString);
                }
                GameSound.MenuSoundWoosh.Play(GameSound.Volume, 0.0f, 0.0f);
                _mDoOnce = true;
            }

            if (StarList.Count == 1)
            {
                if (_current >= _xCoord)
                {
                    _current-= 100;
                }
                spriteBatch.DrawString(_mQuartz, StarList[0], new Vector2(_current, _centerYCoord), Color.White, 0.0f, Vector2.Zero, _mScale, SpriteEffects.None, 0.0f);
                spriteBatch.DrawString(_mQuartz, StarList[0], new Vector2(_current + 4, _centerYCoord + 4), Color.SteelBlue, 0.0f, Vector2.Zero, _mScale, SpriteEffects.None, 0.0f);
            }
            else if (StarList.Count == 2)
            {
                if (_current >= _xCoord)
                {
                    _current -= 66;
                }

                spriteBatch.DrawString(_mQuartz, StarList[0], new Vector2(_current - 33, _topYCoord2), Color.White, 0.0f, Vector2.Zero, _mScale, SpriteEffects.None, 0.0f);
                spriteBatch.DrawString(_mQuartz, StarList[0], new Vector2(_current - 33 + 4, _topYCoord2 + 4), Color.SteelBlue, 0.0f, Vector2.Zero, _mScale, SpriteEffects.None, 0.0f);

                spriteBatch.DrawString(_mQuartz, StarList[1], new Vector2(_current, _bottomYCoord2), Color.White, 0.0f, Vector2.Zero, _mScale, SpriteEffects.None, 0.0f);
                spriteBatch.DrawString(_mQuartz, StarList[1], new Vector2(_current + 4, _bottomYCoord2 + 4), Color.SteelBlue, 0.0f, Vector2.Zero, _mScale, SpriteEffects.None, 0.0f);
            }
            else if (StarList.Count == 3)
            {
                if (_current >= _xCoord)
                {
                    _current -= 33;
                }

                spriteBatch.DrawString(_mQuartz, StarList[0], new Vector2(_current - 66, _topYCoord3), Color.White, 0.0f, Vector2.Zero, _mScale, SpriteEffects.None, 0.0f);
                spriteBatch.DrawString(_mQuartz, StarList[0], new Vector2(_current - 66 + 4, _topYCoord3 + 4), Color.SteelBlue, 0.0f, Vector2.Zero, _mScale, SpriteEffects.None, 0.0f);

                spriteBatch.DrawString(_mQuartz, StarList[1], new Vector2(_current - 33, _centerYCoord), Color.White, 0.0f, Vector2.Zero, _mScale, SpriteEffects.None, 0.0f);
                spriteBatch.DrawString(_mQuartz, StarList[1], new Vector2(_current - 33 + 4, _centerYCoord + 4), Color.SteelBlue, 0.0f, Vector2.Zero, _mScale, SpriteEffects.None, 0.0f);

                spriteBatch.DrawString(_mQuartz, StarList[2], new Vector2(_current, _bottomYCoord3), Color.White, 0.0f, Vector2.Zero, _mScale, SpriteEffects.None, 0.0f);
                spriteBatch.DrawString(_mQuartz, StarList[2], new Vector2(_current + 4, _bottomYCoord3 + 4), Color.SteelBlue, 0.0f, Vector2.Zero, _mScale, SpriteEffects.None, 0.0f);
            }

            spriteBatch.End();
        }
    }
}
