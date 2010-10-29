using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace GravityLevelEditor
{
    static class GridSpace
    {
        private static Point SIZE = new Point(10, 10);
        private static float SCALE_FACTOR = 1.0f;

        /*
         * ZoomIn
         * 
         * Zooms in by 25% of its original size factor.
         */
        public static void ZoomIn() {   SCALE_FACTOR += .25f;   }

        /*
         * ZoomOut
         * 
         * Zooms out by 25% of its original size factor.
         */
        public static void ZoomOut()    {  SCALE_FACTOR -= .25f;   }

        /*
         * GetDrawingCoord
         * 
         * Converts grid coordinates into scaled pixel coordinates for drawing to the screen
         * 
         * Point gridCoord: Grid coordinates that need to be converted
         * 
         * Return Value: The Point location that represents the pixel coordinates of the location
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
         * Point gridCoord: Grid coordinates that need to be converted
         * 
         * Return Value: The Point location that represents the pixel coordinates of the location
         */
        public static Point GetPixelCoord(Point gridCoord)
        {
            return new Point((int)(gridCoord.X * SIZE.X),
                (int)(gridCoord.Y * SIZE.Y));
        }

        /*
         * 
         */
        public static Rectangle GetDrawingRegion(Point gridCoord)
        {
            return new Rectangle(GetDrawingCoord(gridCoord), 
                new Size((int)(SIZE.X * SCALE_FACTOR), (int)(SIZE.Y * SCALE_FACTOR)));
        }
    }
}
