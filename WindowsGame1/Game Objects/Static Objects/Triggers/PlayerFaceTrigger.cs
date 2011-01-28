using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using GravityShift.Import_Code;
using Microsoft.Xna.Framework.Graphics;
using GravityShift.MISC_Code;

namespace GravityShift.Game_Objects.Static_Objects.Triggers
{
    class PlayerFaceTrigger : Trigger
    {
        Texture2D face;
        public PlayerFaceTrigger(ContentManager content, EntityInfo entity) 
            :base(content, entity)
        {
            if (entity.mProperties.ContainsKey(XmlKeys.PLAYER_FACE))
                face = PlayerFaces.FromString(entity.mProperties[XmlKeys.PLAYER_FACE]);
        }
        public override void RunTrigger(List<GameObject> objects, Player player)
        {
            if (player.IsCollidingBoxAndBox(this) && player.mCurrentTexture == PlayerFaces.SMILE)
                player.mCurrentTexture = face;
        }
    }
}
