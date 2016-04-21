using System.Drawing;

namespace MrGravity.LevelEditor
{
    internal static class GridSpace
    {
        public static Point Size = new Point(64, 64);
        private static float _scaleFactor = 1.0f;

        /*
         * ZoomIn
         * 
         * Zooms in by 25% of its original size factor.
         */
        public static void ZoomIn() {   _scaleFactor += .10f;   }

        /*
         * ZoomOut
         * 
         * Zooms out by 25% of its original size factor.
         */
        public static void ZoomOut()    {  if((_scaleFactor - .10) > 0) _scaleFactor -= .10f;   }

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
            return new Point((int)(gridCoord.X * Size.X * _scaleFactor), 
                (int)(gridCoord.Y * Size.Y * _scaleFactor));
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
            return new Point(gridCoord.X * Size.X,
                gridCoord.Y * Size.Y);
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
                new Size((int)(Size.X * _scaleFactor), (int)(Size.Y * _scaleFactor)));
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
            return new Point((int)(((float)pixelCoord.X / Size.X) / _scaleFactor),
                (int)(((float)pixelCoord.Y / Size.Y) / _scaleFactor));
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
            return new Point(pixelCoord.X / Size.X,
                 pixelCoord.Y / Size.Y); 
        }
    }
}
