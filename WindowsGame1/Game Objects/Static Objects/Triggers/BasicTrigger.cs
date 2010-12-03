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

        public override void RunTrigger(List<GameObject> objects, Player player)
        {
            bool isColliding = mBoundingBox.Intersects(player.mBoundingBox);

            if (!wasColliding && isColliding)
                player.Environment.GravityDirection = GravityDirections.Down;
            if (wasColliding && !isColliding)
                player.Environment.GravityDirection = GravityDirections.Down;

            wasColliding = isColliding;
        }
    }
}
