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
        
        /* Player And Collision */
        public static SoundEffect playerCol_wall;
        public static SoundEffect playerSound_death;
        public static SoundEffect playerCol_collectable;
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

        private static SoundEffect music_level01Source;
        public static SoundEffectInstance music_level01;

        private static SoundEffect music_level02Source;
        public static SoundEffectInstance music_level02;

        private static SoundEffect music_level03Source;
        public static SoundEffectInstance music_level03;

        private static SoundEffect music_level04Source;
        public static SoundEffectInstance music_level04;

        private static SoundEffect music_level05Source;
        public static SoundEffectInstance music_level05;

        private static SoundEffect music_level06Source;
        public static SoundEffectInstance music_level06;

        private static SoundEffect music_level07Source;
        public static SoundEffectInstance music_level07;

        private static SoundEffect music_level08Source;
        public static SoundEffectInstance music_level08;

        public static SoundEffectInstance gameMusic_generic;

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
            playerSound_death = content.Load<SoundEffect>("SoundEffects\\playerSound_death");
            playerCol_collectable = content.Load<SoundEffect>("SoundEffects\\playerCol_collectable");
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
            menuMusic_titleSource = content.Load<SoundEffect>("Music\\music_level03");
            menuMusic_title = menuMusic_titleSource.CreateInstance();
            menuMusic_title.IsLooped = true;
            menuMusic_title.Volume = volume;
            
            music_level00Source = content.Load<SoundEffect>("Music\\music_level00");
            music_level00 =  music_level00Source.CreateInstance();
            music_level00.IsLooped = true;
            music_level00.Volume = volume;

            music_level01Source = content.Load<SoundEffect>("Music\\music_level01");
            music_level01 = music_level01Source.CreateInstance();
            music_level01.IsLooped = true;
            music_level01.Volume = volume;

            music_level02Source = content.Load<SoundEffect>("Music\\music_level02");
            music_level02 = music_level02Source.CreateInstance();
            music_level02.IsLooped = true;
            music_level02.Volume = volume;

            music_level03Source = content.Load<SoundEffect>("Music\\music_level03");
            music_level03 = music_level03Source.CreateInstance();
            music_level03.IsLooped = true;
            music_level03.Volume = volume;

            music_level04Source = content.Load<SoundEffect>("Music\\music_level04");
            music_level04 = music_level04Source.CreateInstance();
            music_level04.IsLooped = true;
            music_level04.Volume = volume;

            music_level05Source = content.Load<SoundEffect>("Music\\music_level05");
            music_level05 = music_level05Source.CreateInstance();
            music_level05.IsLooped = true;
            music_level05.Volume = volume;

            music_level06Source = content.Load<SoundEffect>("Music\\music_level06");
            music_level06 = music_level06Source.CreateInstance();
            music_level06.IsLooped = true;
            music_level06.Volume = volume;

            music_level07Source = content.Load<SoundEffect>("Music\\music_level07");
            music_level07 = music_level07Source.CreateInstance();
            music_level07.IsLooped = true;
            music_level07.Volume = volume;

            music_level08Source = content.Load<SoundEffect>("Music\\music_level08");
            music_level08 = music_level08Source.CreateInstance();
            music_level08.IsLooped = true;
            music_level08.Volume = volume;
        }

        public static void SetGeneric(SoundEffectInstance generic)
        {
            gameMusic_generic = generic;
        }

        private static void StopMusic()
        {
            level_stageFail.Stop();
            level_stageVictory.Stop();
            menuMusic_title.Stop();
            music_level00.Stop();
            music_level01.Stop();
            music_level02.Stop();
            music_level03.Stop();
            music_level04.Stop();
            music_level05.Stop();
            music_level06.Stop();
            music_level07.Stop();
            music_level08.Stop();

            if (gameMusic_generic != null)
                gameMusic_generic.Stop();
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
