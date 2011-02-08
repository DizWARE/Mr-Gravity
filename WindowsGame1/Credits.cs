using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace GravityShift
{
    class Credits
    {
        Dictionary<string, string[]> titles; 
        Texture2D mTitle;
        SpriteFont mFontBig;
        SpriteFont mFontSmall;
        Texture2D mBack;

        IControlScheme mControls;
        GraphicsDeviceManager mGraphics;

        private int topY;

        public Credits(IControlScheme controlScheme, GraphicsDeviceManager graphics)
        {
            mControls = controlScheme;
            mGraphics = graphics;
            topY = mGraphics.GraphicsDevice.Viewport.TitleSafeArea.Bottom;

            titles = new Dictionary<string, string[]>();
            titles.Add("Developed At", new string[] { "University Of Utah; Senior EAE Capstone Class" });
            titles.Add("Executive Producer", new string[]{"Roger Altizer", "Dr. Bob Kessler"});
            titles.Add("Scrum Master", new string[] { "Curtis Taylor" });
            titles.Add("Content Director", new string[] { "Steven Doxey" });
            titles.Add("Technical Director", new string[] { "Tyler Robinson" });
            titles.Add("Game Tiles, Characters, and Style Design", new string[] { "Lukas Black" });
            titles.Add("Graphics Team", new string[] { "Lukas Black", "Nate Bradford", "Jeremy Heintz" });
            titles.Add("Technical Team", new string[] { "Tyler Robinson", "Casey Spencer", "Curtis Taylor", "Kameron Klegan", "Jeremy Heintz", "Nate Bradford" });
        }

        public void Load(ContentManager content)
        {
            mTitle = content.Load<Texture2D>("Images\\Menu\\Title");
            mBack = content.Load<Texture2D>("Images\\Menu\\Main\\BackSelected");
            mFontBig = content.Load<SpriteFont>("Fonts\\QuartzLarge");
            mFontSmall = content.Load<SpriteFont>("Fonts\\QuartzSmall");
        }


        public void Update(GameTime gametime, ref GameStates states)
        {
            if (mControls.isAPressed(false) || mControls.isStartPressed(false) || mControls.isBackPressed(false) || mControls.isBPressed(false))
            { topY = mGraphics.GraphicsDevice.Viewport.TitleSafeArea.Bottom; states = GameStates.Main_Menu; }

            topY -= 1;
        }

        public void Draw(GameTime gametime, SpriteBatch spriteBatch, Matrix scale)
        {
            spriteBatch.Begin();

            spriteBatch.Draw(mBack, new Vector2(mGraphics.GraphicsDevice.Viewport.TitleSafeArea.Right - 200,
                                                mGraphics.GraphicsDevice.Viewport.TitleSafeArea.Bottom - 100), Color.White);

            spriteBatch.Draw(mTitle, new Rectangle(mGraphics.GraphicsDevice.Viewport.TitleSafeArea.Center.X - mTitle.Width / 4, topY,mTitle.Width/2,mTitle.Height/2),
                Color.White);

            int top = topY+200;
            foreach (string key in titles.Keys)
            {
                spriteBatch.DrawString(mFontBig, key, new Vector2(GetTextLocation(key, true), top), Color.White);
                spriteBatch.DrawString(mFontBig, key, Vector2.Add(new Vector2(GetTextLocation(key, true), top), new Vector2(2,2)), Color.SteelBlue);
             
                foreach (string name in titles[key])
                {
                    top += 40;
                    spriteBatch.DrawString(mFontSmall, name, new Vector2(GetTextLocation(name, false), top), Color.White);
                }

                top += 100;
            }

            if (top == mGraphics.GraphicsDevice.Viewport.TitleSafeArea.Top)
                topY = mGraphics.GraphicsDevice.Viewport.TitleSafeArea.Bottom;
            spriteBatch.End();
        }

        private int GetTextLocation(string text, bool header)
        {
            Vector2 size = mFontSmall.MeasureString(text);
            if(header)
                size = mFontBig.MeasureString(text);

            return mGraphics.GraphicsDevice.Viewport.TitleSafeArea.Center.X - (int)size.X / 2;
        }

    }
}
