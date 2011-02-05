using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using GravityShift.Import_Code;

namespace GravityShift.Game_Objects.Static_Objects.Triggers
{
    class MusicTrigger : Trigger
    {
        SoundEffect musicByte;
        SoundEffectInstance musicByteInstance;

        public MusicTrigger(ContentManager content, EntityInfo entity) 
            : base(content, entity)
        {
            if (entity.mProperties.ContainsKey(XmlKeys.MUSIC_FILE))
            {
                musicByte = content.Load<SoundEffect>("Music\\" + entity.mProperties[XmlKeys.MUSIC_FILE]);
                musicByteInstance = musicByte.CreateInstance();
                musicByteInstance.Volume = GameSound.volume;
                
            }
            if(entity.mProperties.ContainsKey(XmlKeys.LOOP))
                musicByteInstance.IsLooped = entity.mProperties[XmlKeys.LOOP] == XmlKeys.TRUE;
        }



        public override void RunTrigger(List<GameObject> objects, Player player)
        {
            if (player.IsCollidingCircleandCircle(this) && musicByteInstance.State != SoundState.Playing)
            {
                GameSound.StopOthersAndPlay(musicByteInstance);
                GameSound.SetGeneric(musicByteInstance);
            }
        }
    }
}
