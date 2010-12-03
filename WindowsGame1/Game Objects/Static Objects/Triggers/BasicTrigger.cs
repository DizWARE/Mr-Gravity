using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace GravityShift.Game_Objects.Static_Objects.Triggers
{
    class BasicTrigger : Trigger
    {
        bool wasColliding = false;

        public BasicTrigger(ContentManager content, String name, Vector2 initialPosition, bool isSquare, int width, int height) :
            base(content, name, initialPosition, isSquare, width, height) { }

        /// <summary>
        /// This triggers adds a force in the x direction while the player is within its bounds and removes it
        /// when the player exits
        /// </summary>
        /// <param name="objects">List of objects in the game</param>
        /// <param name="player">Player in the game</param>
        public override void RunTrigger(List<GameObject> objects, Player player)
        {
            bool isColliding = (mIsSquare && player.IsCollidingBoxAndBox(this)) || (!IsSquare && player.IsCollidingCircleandCircle(this));

            if (!wasColliding && isColliding)
                player.AddForce(new Vector2(.5f, 0));
            if (wasColliding && !isColliding)
                player.AddForce(new Vector2(-.5f, 0));

            wasColliding = isColliding;
        }
    }
}
