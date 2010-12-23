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

        /* Level */
        public static SoundEffect level_stageFail;
        public static SoundEffect level_stageVictory;
        
        /* Menu */
        public static SoundEffect menuSound_rollover;
        public static SoundEffect menuSound_select;
        
        /* Player Collision */
        public static SoundEffect playerCol_wall;
        public static SoundEffect playerCol_hazard;

        /* Gravity Shifting */
        public static SoundEffect level_gravityShiftUp;
        public static SoundEffect level_gravityShiftDown;
        public static SoundEffect level_gravityShiftLeft;
        public static SoundEffect level_gravityShiftRight;

        /* Music */

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
            /* Level */
            level_stageFail = content.Load<SoundEffect>("SoundEffects\\level_stageFail");
            level_stageVictory = content.Load<SoundEffect>("SoundEffects\\level_stageVictory");
        
            /* Menu */
            menuSound_rollover = content.Load<SoundEffect>("SoundEffects\\menuSound_rollover");
            menuSound_select = content.Load<SoundEffect>("SoundEffects\\menuSound_select");
        
            /* Player Collision */
            playerCol_wall = content.Load<SoundEffect>("SoundEffects\\playerCol_wall");
            playerCol_hazard = content.Load<SoundEffect>("SoundEffects\\playerCol_hazard");

            /* Gravity Shifting */
            level_gravityShiftUp = content.Load<SoundEffect>("SoundEffects\\level_gravityShiftUp");
            level_gravityShiftDown = content.Load<SoundEffect>("SoundEffects\\level_gravityShiftDown");
            level_gravityShiftLeft = content.Load<SoundEffect>("SoundEffects\\level_gravityShiftLeft");
            level_gravityShiftRight = content.Load<SoundEffect>("SoundEffects\\level_gravityShiftRight");
        }
    }
}
