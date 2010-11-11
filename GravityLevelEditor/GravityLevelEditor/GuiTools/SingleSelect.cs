using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Collections;
using System.Windows.Forms;

namespace GravityLevelEditor.GuiTools
{
    class SingleSelect:ITool
    {
        Point mInitial;
        Point mPrevious;

        bool mouseDown = false;

        #region ITool Members

        public void LeftMouseDown(ref EditorData data, Point gridPosition)
        {
            if (data.SelectedEntities.Count < 2)
            {
                data.SelectedEntities.Clear();

                Entity selected = data.Level.SelectEntity(gridPosition);
                if (selected != null)
                    data.SelectedEntities.Add(selected);
            }

            mPrevious = mInitial = gridPosition;
            mouseDown = true;
        }

        public void LeftMouseUp(ref EditorData data, Point gridPosition)
        {
            if (data.SelectedEntities.Count > 0)
            {
                data.Level.MoveEntity(data.SelectedEntities,
                    new Size(Point.Subtract(mInitial, new Size(gridPosition))), false);
                data.Level.MoveEntity(data.SelectedEntities,
                    new Size(Point.Subtract(gridPosition, new Size(mInitial))), true);
            }
            mouseDown = false;
        }


        public void RightMouseDown(ref EditorData data, Point gridPosition)
        {
            
        }

        public void RightMouseUp(ref EditorData data, Point gridPosition)
        {
            
        }

        public void MouseMove(ref EditorData data, Panel panel, Point gridPosition)
        {
            if (!mPrevious.Equals(gridPosition) && data.SelectedEntities.Count > 0 && mouseDown)
            {
                //Keep an eye on this. The SelectedEntities can return an empty list
                data.Level.MoveEntity(data.SelectedEntities,
                    new Size(Point.Subtract(gridPosition, new Size(mPrevious))), false);
                mPrevious = gridPosition;
                panel.Invalidate(panel.DisplayRectangle);
            }
        }

        #endregion
    }
}
