using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using GravityShift.Import_Code;

namespace GravityShift.Game_Objects.Static_Objects.Triggers
{
    class FXTrigger : Trigger
    {
        SoundEffect soundByte;
        bool playing;

        public FXTrigger(ContentManager content, EntityInfo entity) 
            : base(content, entity)
        {
            if (entity.mProperties.ContainsKey(XmlKeys.SOUND_FILE))
            {
                soundByte = content.Load<SoundEffect>("SoundEffects\\" + entity.mProperties[XmlKeys.SOUND_FILE]);
            }
        }

        public override void RunTrigger(List<GameObject> objects, Player player)
        {
            if (player.IsCollidingCircleandCircle(this)&&!playing)
            {
                soundByte.Play();
                playing = true;
            }
            else if (!player.IsCollidingCircleandCircle(this))
                playing = false;
        }
    }
}
