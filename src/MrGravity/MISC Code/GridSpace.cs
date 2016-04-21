using Microsoft.Xna.Framework;

namespace MrGravity.MISC_Code
{
    internal static class GridSpace
    {
        public static Vector2 Size = new Vector2(64, 64);
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
        public static Vector2 GetDrawingCoord(Vector2 gridCoord)
        {
            return new Vector2((int)(gridCoord.X * Size.X * _scaleFactor), 
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
        public static Vector2 GetPixelCoord(Vector2 gridCoord)
        {
            return new Vector2((int)(gridCoord.X * Size.X),
                (int)(gridCoord.Y * Size.Y));
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
        public static Rectangle GetDrawingRegion(Vector2 gridCoord, Vector2 offset)
        {
            return new Rectangle((int)(GetDrawingCoord(gridCoord).X + offset.X), (int)(GetDrawingCoord(gridCoord).Y + offset.Y),
                (int)(Size.X * _scaleFactor), (int)(Size.Y * _scaleFactor));
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
        public static Vector2 GetScaledGridCoord(Vector2 pixelCoord)
        {
            return new Vector2((int)((pixelCoord.X / Size.X) / _scaleFactor),
                (int)((pixelCoord.Y / Size.Y) / _scaleFactor));
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
        public static Vector2 GetGridCoord(Vector2 pixelCoord)
        {
            return new Vector2((int)(pixelCoord.X / Size.X),
                 (int)(pixelCoord.Y / Size.Y)); 
        }
    }
}
