using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace GravityLevelEditor
{
    static class GridSpace
    {
        public static Point SIZE = new Point(64, 64);
        private static float SCALE_FACTOR = 1.0f;

        /*
         * ZoomIn
         * 
         * Zooms in by 25% of its original size factor.
         */
        public static void ZoomIn() {   SCALE_FACTOR += .10f;   }

        /*
         * ZoomOut
         * 
         * Zooms out by 25% of its original size factor.
         */
        public static void ZoomOut()    {  if((SCALE_FACTOR - .10) > 0) SCALE_FACTOR -= .10f;   }

        /*
         * GetDrawingCoord
         * 
         * Converts grid coordinates into scaled pixel coordinates for drawing to the screen.
         * 
         * Point gridCoord: Grid coordinates that need to be converted.
         * 
         * Return Value: The Point location that represents the pixel coordinates of the location.
         */
        public static Point GetDrawingCoord(Point gridCoord)
        {
            return new Point((int)(gridCoord.X * SIZE.X * SCALE_FACTOR), 
                (int)(gridCoord.Y * SIZE.Y * SCALE_FACTOR));
        }

        /*
         * GetPixelCoord
         * 
         * Converts grid coordinates into pixel coordinates for exporting to file.
         * --No scaling factor is used here.
         * 
         * Point gridCoord: Grid coordinates that need to be converted.
         * 
         * Return Value: The Point location that represents the pixel coordinates of the location.
         */
        public static Point GetPixelCoord(Point gridCoord)
        {
            return new Point((int)(gridCoord.X * SIZE.X),
                (int)(gridCoord.Y * SIZE.Y));
        }

        /*
         * GetDrawingRegion
         * 
         * Gets the drawing rectangle (pixel based) for the given coordinate.
         * 
         * Point gridCoord: Grid coordinates that need to be converted.
         * 
         * Point offset: level panel scroll value.
         * 
         * Return Value: The pixel based rectangle where the entity at gridCoord
         *               should be drawn.
         */
        public static Rectangle GetDrawingRegion(Point gridCoord, Point offset)
        {
            return new Rectangle(new Point(GetDrawingCoord(gridCoord).X + offset.X, GetDrawingCoord(gridCoord).Y + offset.Y), 
                new Size((int)(SIZE.X * SCALE_FACTOR), (int)(SIZE.Y * SCALE_FACTOR)));
        }

        /*
         * GetScaledGridCoord
         * 
         * Gets the row and column that the pixel coord represents. The pixel coordanates should
         * be based off of the Zoomed In or Zoomed Out location on the grid(Otherwise you will
         * get incorrect results)
         * 
         * Point pixelCoord: The Pixel Coordinate that we want converted
         * 
         * Return Value: The Grid Coordinates for this location
         */
        public static Point GetScaledGridCoord(Point pixelCoord)
        {
            return new Point((int)(((float)pixelCoord.X / SIZE.X) / SCALE_FACTOR),
                (int)(((float)pixelCoord.Y / SIZE.Y) / SCALE_FACTOR));
        }

        /*
         * GetGridCoord
         * 
         * Converts a pixel coordinate into Grid Coordinates. The pixel coordinates should be 
         * based off the unmodified zoom factor(100%). Use GetScaledGridCoord if the scale factor
         * plays affect to the results.
         * 
         * Point pixelCoord: Coordinates we plan on converting
         * 
         * Return Value: Gets the Grid coordinates on the level
         */
        public static Point GetGridCoord(Point pixelCoord)
        {
            return new Point((int)(pixelCoord.X / SIZE.X),
                 (int)(pixelCoord.Y / SIZE.Y)); 
        }
    }
}
