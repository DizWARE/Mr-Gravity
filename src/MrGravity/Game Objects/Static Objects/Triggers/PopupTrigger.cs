using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MrGravity.Game_Objects.Physics_Objects;
using MrGravity.Import_Code;

namespace MrGravity.Game_Objects.Static_Objects.Triggers
{
    /// <summary>
    /// Trigger that draws text or an image
    /// </summary>
    internal class PopupTrigger : Trigger
    {
        private const string ImageDirectory = "Images\\";
        private bool _hasEntered;

        private readonly bool _isImage;
        private readonly string _mText = "";

        private readonly SpriteFont _mFont;

        /// <summary>
        /// Creates a new popup trigger
        /// </summary>
        /// <param name="content">Content manager that loads our stuff</param>
        /// <param name="entity">Information from xml about this entity</param>
        public PopupTrigger(ContentManager content, EntityInfo entity)
            : base(content, entity)
        {
            if (entity.MProperties.ContainsKey(XmlKeys.PopupType))
            {
                if (_isImage = entity.MProperties[XmlKeys.PopupType] == XmlKeys.PopupImage)
                    Load(content, ImageDirectory + entity.MProperties[XmlKeys.ImageFile]);
                else
                    _mText = entity.MProperties[XmlKeys.Text];
            }

            _mFont = content.Load<SpriteFont>("Fonts/QuartzSmall");
        }

        /// <summary>
        /// Draws the popup when the player is in range
        /// </summary>
        /// <param name="canvas">Canvas we draw to</param>
        /// <param name="gametime">Current Gametime</param>
        public override void Draw(SpriteBatch canvas, GameTime gametime)
        {
            if (_hasEntered)
            {
                if (_isImage)
                    canvas.Draw(Texture, new Vector2(MPosition.X - Texture.Width / 2, MPosition.Y - Texture.Height / 2), Color.White);
                else
                {
                    Vector2 size = _mFont.MeasureString(_mText);
                    canvas.DrawString(_mFont, _mText, new Vector2(MPosition.X - size.X / 2, MPosition.Y - size.Y / 2), Color.White);
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
            if (player.IsCollidingCircleAndBox(this) && !_hasEntered)
                _hasEntered = true;
            else if (!player.IsCollidingCircleAndBox(this))
                _hasEntered = false;
        }
    }
}
