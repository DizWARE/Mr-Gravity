using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Collections;

namespace GravityLevelEditor.GuiTools
{
    class SingleSelect:ITool
    {
        Point mInitial;
        Point mPrevious;

        #region ITool Members

        /*
         * LeftMouseDown
         * 
         * Starts watching the changes when user presses the left mouse button down
         * 
         * Level level: Level we are editing
         * Point gridPosition: Position where our mouse is, currently
         */
        public void LeftMouseDown(Level level, Point gridPosition)
        {
            mInitial = gridPosition;
            mPrevious = mInitial;
        }

        /*
         * LeftMouseUp
         * 
         * If the mouse when left button released, has not moved tiles, 
         * then select the top entity at this position 
         * 
         * Level level: Level we are editing
         * Point gridPosition: Position where our mouse is, currently
         */
        public void LeftMouseUp(Level level, Point gridPosition)
        {
            ArrayList entities = level.GetSelectedEntities();
            if (mInitial.Equals(gridPosition))
                level.SelectEntities(gridPosition, gridPosition);
        }

        public void RightMouseDown(Level level, Point gridPosition)
        {
            //Implement me
        }

        public void RightMouseUp(Level level, Point gridPosition)
        {
            //Implement me
        }

        /*
         * MouseMove
         * 
         * Move the selected entities from their previous grid point to the given grid point
         * 
         * Level level: Level we are editing
         * Point gridPosition: Position where our mouse is, currently
         */
        public void MouseMove(Level level, Point gridPosition)
        {
            if (!mPrevious.Equals(gridPosition))
            {
                //Keep an eye on this. The GetSelectedEntities can return an empty list
                level.MoveEntity(level.GetSelectedEntities(),
                    new Size(Point.Subtract(gridPosition, new Size(mInitial))));
                mPrevious = gridPosition;
            }
        }

        #endregion
    }
}
