using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MrGravity.Game_Objects.Physics_Objects;
using MrGravity.Import_Code;
using MrGravity.MISC_Code;

namespace MrGravity.Game_Objects.Static_Objects.Triggers
{
    internal class PlayerFaceTrigger : Trigger
    {
        private readonly Texture2D _face;
        public PlayerFaceTrigger(ContentManager content, EntityInfo entity) 
            :base(content, entity)
        {
            if (entity.MProperties.ContainsKey(XmlKeys.PlayerFace))
                _face = PlayerFaces.FromString(entity.MProperties[XmlKeys.PlayerFace]);
        }
        public override void RunTrigger(List<GameObject> objects, Player player)
        {
            if (player.IsCollidingBoxAndBox(this) && player.MCurrentTexture != PlayerFaces.FromString("Dead2"))
                player.MCurrentTexture = _face;
        }
    }
}
