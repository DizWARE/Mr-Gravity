using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using MrGravity.Game_Objects.Physics_Objects;
using MrGravity.Import_Code;
using MrGravity.MISC_Code;

namespace MrGravity.Game_Objects.Static_Objects.Triggers
{
    internal class MusicTrigger : Trigger
    {
        private readonly SoundEffect _musicByte;
        private readonly SoundEffectInstance _musicByteInstance;

        public MusicTrigger(ContentManager content, EntityInfo entity) 
            : base(content, entity)
        {
            if (entity.MProperties.ContainsKey(XmlKeys.MusicFile))
            {
                _musicByte = content.Load<SoundEffect>("Music\\" + entity.MProperties[XmlKeys.MusicFile]);
                _musicByteInstance = _musicByte.CreateInstance();
                _musicByteInstance.Volume = GameSound.Volume;
                
            }
            if(entity.MProperties.ContainsKey(XmlKeys.Loop))
                _musicByteInstance.IsLooped = entity.MProperties[XmlKeys.Loop] == XmlKeys.True;
        }



        public override void RunTrigger(List<GameObject> objects, Player player)
        {
            if (player.IsCollidingCircleandCircle(this) && _musicByteInstance.State != SoundState.Playing)
            {
                GameSound.StopOthersAndPlay(_musicByteInstance);
                GameSound.SetGeneric(_musicByteInstance);
            }
        }
    }
}
