using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MrGravity.Game_Objects.Physics_Objects;
using MrGravity.Import_Code;
using MrGravity.MISC_Code;

namespace MrGravity.Game_Objects.Static_Objects.Triggers
{
    internal abstract class Trigger : StaticObject
    {
        /// <summary>
        /// Construcsts a new trigger
        /// 
        /// See Static object for parameter defs
        /// </summary>
        /// <param name="width">Width of the trigger activation field</param>
        /// <param name="height">Height of the trigger activation field</param>
        public Trigger(ContentManager content, EntityInfo entity)
            : base(content, .0f, entity)
        {
            MSize = new Vector2(3, 3);
            if(entity.MProperties.ContainsKey(XmlKeys.Width)) MSize.X = int.Parse(entity.MProperties[XmlKeys.Width]);
            if (entity.MProperties.ContainsKey(XmlKeys.Height)) MSize.Y = int.Parse(entity.MProperties[XmlKeys.Height]);

            MSize = GridSpace.GetDrawingCoord(MSize);
            var boundingBox = BoundingBox;
            boundingBox.X -= (int)MSize.X / 2;
            boundingBox.Y -= (int)MSize.Y / 2;
            boundingBox.Width = (int)MSize.X;
            boundingBox.Height = (int)MSize.Y;

            BoundingBox = boundingBox;
        }

        /// <summary>
        /// Ignores drawing triggers. They should be invisible anyway
        /// </summary>
        public override void Draw(SpriteBatch canvas, GameTime gametime)
        {
        }

        /// <summary>
        /// Runs whatever the trigger should do.
        /// </summary>
        /// <param name="objects">List of all the objects in the level</param>
        /// <param name="player">The player in the game</param>
        public abstract void RunTrigger(List<GameObject> objects, Player player);
    }
}
