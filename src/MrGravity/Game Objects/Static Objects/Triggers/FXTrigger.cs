using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using MrGravity.Game_Objects.Physics_Objects;
using MrGravity.Import_Code;

namespace MrGravity.Game_Objects.Static_Objects.Triggers
{
    internal class FxTrigger : Trigger
    {
        private readonly SoundEffect _soundByte;
        private bool _playing;

        /// <summary>
        /// Constructs a trigger that will play a sound effect
        /// </summary>
        /// <param name="content">Content to load from</param>
        /// <param name="entity">Entity information</param>
        public FxTrigger(ContentManager content, EntityInfo entity) 
            : base(content, entity)
        {
            if (entity.MProperties.ContainsKey(XmlKeys.SoundFile))
                _soundByte = content.Load<SoundEffect>("SoundEffects\\" + entity.MProperties[XmlKeys.SoundFile]);
        }

        /// <summary>
        /// Runs the sound effect
        /// </summary>
        /// <param name="objects">List of objects in the game</param>
        /// <param name="player">Player</param>
        public override void RunTrigger(List<GameObject> objects, Player player)
        {
            if (player.IsCollidingCircleandCircle(this)&&!_playing)
            {
                _soundByte.Play();
                _playing = true;
            }
            else if (!player.IsCollidingCircleandCircle(this))
                _playing = false;
        }
    }
}
