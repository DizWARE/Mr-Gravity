using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

namespace MrGravity.MISC_Code
{
    public class GameSound
    {
        public static float Volume = 1.0f;

        /* Menu */
        public static SoundEffect MenuSoundRollover;
        public static SoundEffect MenuSoundSelect;
        public static SoundEffect MenuSoundWoosh;
        
        /* Player And Collision */
        public static SoundEffect PlayerColWall;
        public static SoundEffect PlayerSoundDeath;
        public static SoundEffect PlayerColCollectable;
        public static SoundEffect PlayerSoundRespawn;

        /* Gravity Shifting */
        public static SoundEffect LevelGravityShiftUp;
        public static SoundEffect LevelGravityShiftDown;
        public static SoundEffect LevelGravityShiftLeft;
        public static SoundEffect LevelGravityShiftRight;

        /* Level */
        public static SoundEffect LevelStageFailSource;
        public static SoundEffectInstance LevelStageFail;
        public static SoundEffect LevelStageVictorySource;
        public static SoundEffectInstance LevelStageVictory;
        
        /* Music */
        private static SoundEffect _menuMusicTitleSource;
        public static SoundEffectInstance MenuMusicTitle;

        public static SoundEffectInstance MusicLevel00;

        private static SoundEffect _musicLevel01Source;
        public static SoundEffectInstance MusicLevel01;

        private static SoundEffect _musicLevel02Source;
        public static SoundEffectInstance MusicLevel02;

        private static SoundEffect _musicLevel03Source;
        public static SoundEffectInstance MusicLevel03;

        private static SoundEffect _musicLevel04Source;
        public static SoundEffectInstance MusicLevel04;

        private static SoundEffect _musicLevel05Source;
        public static SoundEffectInstance MusicLevel05;

        private static SoundEffect _musicLevel06Source;
        public static SoundEffectInstance MusicLevel06;

        private static SoundEffect _musicLevel07Source;
        public static SoundEffectInstance MusicLevel07;

        private static SoundEffect _musicLevel08Source;
        public static SoundEffectInstance MusicLevel08;

        private static SoundEffect _musicLevel09Source;
        public static SoundEffectInstance MusicLevel09;

        public static SoundEffectInstance GameMusicGeneric;

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
            MenuSoundRollover = content.Load<SoundEffect>("SoundEffects\\menuSound_rollover");
            MenuSoundSelect = content.Load<SoundEffect>("SoundEffects\\menuSound_select");
            MenuSoundWoosh = content.Load<SoundEffect>("SoundEffects\\menuSound_woosh");


        
            /* Player Collision */
            PlayerColWall = content.Load<SoundEffect>("SoundEffects\\playerCol_wall");
            PlayerSoundDeath = content.Load<SoundEffect>("SoundEffects\\playerSound_death");
            PlayerColCollectable = content.Load<SoundEffect>("SoundEffects\\playerCol_collectable");
            PlayerSoundRespawn = content.Load<SoundEffect>("SoundEffects\\playerSound_respawn");

            /* Gravity Shifting */
            LevelGravityShiftUp = content.Load<SoundEffect>("SoundEffects\\level_gravityShiftUp");
            LevelGravityShiftDown = content.Load<SoundEffect>("SoundEffects\\level_gravityShiftDown");
            LevelGravityShiftLeft = content.Load<SoundEffect>("SoundEffects\\level_gravityShiftLeft");
            LevelGravityShiftRight = content.Load<SoundEffect>("SoundEffects\\level_gravityShiftRight");

            /* Level */
            LevelStageFailSource = content.Load<SoundEffect>("SoundEffects\\level_stageFail");
            LevelStageFail = LevelStageFailSource.CreateInstance();
            LevelStageFail.Volume = Volume;
            
            LevelStageVictorySource = content.Load<SoundEffect>("SoundEffects\\level_stageFail");
            LevelStageVictory = LevelStageVictorySource.CreateInstance();
            LevelStageVictory.Volume = Volume;

            /* Music */
            _menuMusicTitleSource = content.Load<SoundEffect>("Music\\music_menu");
            MenuMusicTitle = _menuMusicTitleSource.CreateInstance();
            MenuMusicTitle.IsLooped = true;
            MenuMusicTitle.Volume = Volume;
            

            _musicLevel01Source = content.Load<SoundEffect>("Music\\music_level01");
            MusicLevel01 = _musicLevel01Source.CreateInstance();
            MusicLevel01.IsLooped = true;
            MusicLevel01.Volume = Volume;

            _musicLevel02Source = content.Load<SoundEffect>("Music\\music_level02");
            MusicLevel02 = _musicLevel02Source.CreateInstance();
            MusicLevel02.IsLooped = true;
            MusicLevel02.Volume = Volume;

            _musicLevel03Source = content.Load<SoundEffect>("Music\\music_level03");
            MusicLevel03 = _musicLevel03Source.CreateInstance();
            MusicLevel03.IsLooped = true;
            MusicLevel03.Volume = Volume;

            _musicLevel04Source = content.Load<SoundEffect>("Music\\music_level04");
            MusicLevel04 = _musicLevel04Source.CreateInstance();
            MusicLevel04.IsLooped = true;
            MusicLevel04.Volume = Volume;

            _musicLevel05Source = content.Load<SoundEffect>("Music\\music_level05");
            MusicLevel05 = _musicLevel05Source.CreateInstance();
            MusicLevel05.IsLooped = true;
            MusicLevel05.Volume = Volume;
            
            _musicLevel06Source = content.Load<SoundEffect>("Music\\music_level06");
            MusicLevel06 = _musicLevel06Source.CreateInstance();
            MusicLevel06.IsLooped = true;
            MusicLevel06.Volume = Volume;
            
            _musicLevel07Source = content.Load<SoundEffect>("Music\\music_level07");
            MusicLevel07 = _musicLevel07Source.CreateInstance();
            MusicLevel07.IsLooped = true;
            MusicLevel07.Volume = Volume;

            _musicLevel08Source = content.Load<SoundEffect>("Music\\music_level08");
            MusicLevel08 = _musicLevel08Source.CreateInstance();
            MusicLevel08.IsLooped = true;
            MusicLevel08.Volume = Volume;

            _musicLevel09Source = content.Load<SoundEffect>("Music\\music_level09");
            MusicLevel09 = _musicLevel08Source.CreateInstance();
            MusicLevel09.IsLooped = true;
            MusicLevel09.Volume = Volume;
        }

        public static void SetGeneric(SoundEffectInstance generic)
        {
            GameMusicGeneric = generic;
        }

        private static void StopMusic()
        {
            LevelStageFail.Stop();
            LevelStageVictory.Stop();
            MenuMusicTitle.Stop();

            MusicLevel01.Stop();
            MusicLevel02.Stop();
            MusicLevel03.Stop();
            MusicLevel04.Stop();
            MusicLevel05.Stop();
            MusicLevel06.Stop();
            MusicLevel07.Stop();
            MusicLevel08.Stop();
            MusicLevel09.Stop();

            if (GameMusicGeneric != null)
                GameMusicGeneric.Stop();
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
            music.Volume = Volume;
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
            music.Volume = Volume * volumeMultiplier;
            music.Play();
        }
    }
}
