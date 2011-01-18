using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using System.Text;

namespace GravityShift
{
    public class GameSound
    {
        public static float volume = 1.0f;

        /* Menu */
        public static SoundEffect menuSound_rollover;
        public static SoundEffect menuSound_select;
        
        /* Player Collision */
        public static SoundEffect playerCol_wall;
        public static SoundEffect playerCol_hazard;
        public static SoundEffect playerSound_respawn;

        /* Gravity Shifting */
        public static SoundEffect level_gravityShiftUp;
        public static SoundEffect level_gravityShiftDown;
        public static SoundEffect level_gravityShiftLeft;
        public static SoundEffect level_gravityShiftRight;

        /* Level */
        public static SoundEffect level_stageFailSource;
        public static SoundEffectInstance level_stageFail;
        public static SoundEffect level_stageVictorySource;
        public static SoundEffectInstance level_stageVictory;
        
        /* Music */
        private static SoundEffect menuMusic_titleSource;
        public static SoundEffectInstance menuMusic_title;
        private static SoundEffect music_level00Source;
        public static SoundEffectInstance music_level00; 

        public GameSound() { }

        /*
         * Load
         *
         * Similar to a loadContent function. This function loads and 
         * initializes sounds used in the class.  Called from
         * GravityShiftMain.LoadContent
         *
         * ContentManager content: the Content file used in the game.
         */
        public static void Load(ContentManager content)
        {
            /* Menu */
            menuSound_rollover = content.Load<SoundEffect>("SoundEffects\\menuSound_rollover");
            menuSound_select = content.Load<SoundEffect>("SoundEffects\\menuSound_select");
        
            /* Player Collision */
            playerCol_wall = content.Load<SoundEffect>("SoundEffects\\playerCol_wall");
            playerCol_hazard = content.Load<SoundEffect>("SoundEffects\\playerCol_hazard");
            playerSound_respawn = content.Load<SoundEffect>("SoundEffects\\playerSound_respawn");

            /* Gravity Shifting */
            level_gravityShiftUp = content.Load<SoundEffect>("SoundEffects\\level_gravityShiftUp");
            level_gravityShiftDown = content.Load<SoundEffect>("SoundEffects\\level_gravityShiftDown");
            level_gravityShiftLeft = content.Load<SoundEffect>("SoundEffects\\level_gravityShiftLeft");
            level_gravityShiftRight = content.Load<SoundEffect>("SoundEffects\\level_gravityShiftRight");

            /* Level */
            level_stageFailSource = content.Load<SoundEffect>("SoundEffects\\level_stageFail");
            level_stageFail = level_stageFailSource.CreateInstance();
            level_stageFail.Volume = volume;
            
            level_stageVictorySource = content.Load<SoundEffect>("SoundEffects\\level_stageFail");
            level_stageVictory = level_stageVictorySource.CreateInstance();
            level_stageVictory.Volume = volume;

            /* Music */
            menuMusic_titleSource = content.Load<SoundEffect>("music\\menuMusic_title");
            menuMusic_title = menuMusic_titleSource.CreateInstance();
            menuMusic_title.IsLooped = true;
            menuMusic_title.Volume = volume;
            
            music_level00Source = content.Load<SoundEffect>("music\\music_level00");
            music_level00 =  music_level00Source.CreateInstance();
            music_level00.IsLooped = true;
            music_level00.Volume = volume;
        }

        private static void StopMusic()
        {
            level_stageFail.Stop();
            level_stageVictory.Stop();
            menuMusic_title.Stop();
            music_level00.Stop();
        }
        /*
         * StopOthersAndPlay
         *
         * This will stop any music already being played and replace it
         * with the new music
         *
         */
        public static void StopOthersAndPlay(SoundEffectInstance music)
        {
            StopMusic();
            music.Volume = volume;
            music.Play();
        }

        /*
         * StopOthersAndPlay with volume multiplier
         * 
         * volumeMultiplier should be between 0.0f and 1.0f
         */
        public static void StopOthersAndPlay(SoundEffectInstance music, float volumeMultiplier)
        {
            StopMusic();
            music.Volume = volume * volumeMultiplier;
            music.Play();
        }
    }
}
