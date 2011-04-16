using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using GravityShift.Import_Code;

namespace GravityShift.Game_Objects.Static_Objects.Triggers
{
    class PopupTrigger : Trigger
    {
        private const string IMAGE_DIRECTORY = "Images\\";
        private bool hasEntered = false;

        private bool isImage = false;
        private string mText = "";

        private SpriteFont mFont;

        /// <summary>
        /// Creates a new popup trigger
        /// </summary>
        /// <param name="content">Content manager that loads our stuff</param>
        /// <param name="entity">Information from xml about this entity</param>
        public PopupTrigger(ContentManager content, EntityInfo entity)
            : base(content, entity)
        {
            if (entity.mProperties.ContainsKey(XmlKeys.POPUP_TYPE))
            {
                if (isImage = entity.mProperties[XmlKeys.POPUP_TYPE] == XmlKeys.POPUP_IMAGE)
                    this.Load(content, IMAGE_DIRECTORY + entity.mProperties[XmlKeys.IMAGE_FILE]);
                else
                    mText = entity.mProperties[XmlKeys.TEXT];
            }

            mFont = content.Load<SpriteFont>("Fonts/QuartzSmall");
        }

        /// <summary>
        /// Draws the popup when the player is in range
        /// </summary>
        /// <param name="canvas">Canvas we draw to</param>
        /// <param name="gametime">Current Gametime</param>
        public override void Draw(SpriteBatch canvas, GameTime gametime)
        {
            if (hasEntered)
            {
                if (isImage)
                    canvas.Draw(mTexture, new Vector2(this.mPosition.X - mTexture.Width / 2, this.mPosition.Y - mTexture.Height / 2), Color.White);
                else
                {
                    Vector2 size = mFont.MeasureString(mText);
                    canvas.DrawString(mFont, mText, new Vector2(this.mPosition.X - size.X / 2, this.mPosition.Y - size.Y / 2), Color.White);
                }
            }
        }

        /// <summary>
        /// Checks to see if the player is within range
        /// </summary>
        /// <param name="objects">Objects of the game</param>
        /// <param name="player">Player in the game</param>
        public override void RunTrigger(List<GameObject> objects, Player player)
        {
            if (player.IsCollidingCircleAndBox(this) && !hasEntered)
                hasEntered = true;
            else if (!player.IsCollidingCircleAndBox(this))
                hasEntered = false;
        }
    }
}
