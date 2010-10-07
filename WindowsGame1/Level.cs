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

namespace GravityShift
{
    /// <summary>
    /// MAJOR TODO - This class needs to be completely retooled. There is a major problem with a lot of the decisions I made here.
    /// It was only kept for the use of the demonstration
    /// </summary>
    class Level
    {
        private Texture2D colorMap;
        private Texture2D overlay;

        public Texture2D ColorMap
        { get { return colorMap; } }

        public Texture2D Overlay
        { get { return overlay; } }

        /// <summary>
        /// Constructs a new level for the game.
        /// 
        /// TODO - Figure out if this will be used
        /// </summary>
        public Level()
        {
        }

        /// <summary>
        /// Loads the level from the content manager
        /// </summary>
        /// <param name="content"></param>
        /// <param name="level"></param>
        public void Load(ContentManager content, int level)
        {
            colorMap = content.Load<Texture2D>("Level" + level.ToString());
            overlay = content.Load<Texture2D>("Level" + level.ToString() + "Overlay");
        }

        /// <summary>
        /// Checks to see if the given object is in bounds or not
        /// </summary>
        /// <param name="objectToCheck">Object we are checking for collisions with</param>
        public void CheckBounds(PhysicsObject objectToCheck)
        {
            Rectangle currentBoundingBox = objectToCheck.BoundingBox;

            Vector2 dPos = objectToCheck.ObjectVelocity;

            Color[] mapData = new Color[currentBoundingBox.Width * currentBoundingBox.Height];

            while (isTouchingBounds(new Rectangle(
                currentBoundingBox.X,currentBoundingBox.Y,
                currentBoundingBox.Width,currentBoundingBox.Height), dPos, mapData))
                    dPos = Vector2.Multiply(dPos, .75f);

            objectToCheck.ObjectVelocity = dPos;

        }

        private bool isTouchingBounds(Rectangle boundingBox, Vector2 dPos, Color[] mapData)
        {
            boundingBox.X += (int)dPos.X;
            boundingBox.Y += (int)dPos.Y;

            colorMap.GetData<Color>(0, boundingBox, mapData, 0, mapData.Length);

            foreach (Color color in mapData)
                if (color.Equals(ColorCodes.UNWALKABLE))
                    return true;

            return false;
        }


        /// <summary>
        /// Gets
        /// </summary>
        /// <returns></returns>
        public Vector2 GetStartingPoint()
        {
            int aPixels = colorMap.Width * colorMap.Height;

            Color[] myColors = new Color[aPixels];
            Vector2 startingPoint = new Vector2(25, 25);

            colorMap.GetData<Color>(myColors);

            for (int i = 0; i < myColors.Length; i++)
            {
                if (myColors[i] == ColorCodes.STARTING_POINT)
                {
                    startingPoint = new Vector2(i % colorMap.Width, i / colorMap.Width);
                    break;
                }
            }

            return startingPoint;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(colorMap, new Rectangle(0, 0, colorMap.Width, colorMap.Height), Color.White);
            spriteBatch.Draw(overlay, new Rectangle(0, 0, overlay.Width, overlay.Height), Color.White);            
        }
    }
}
