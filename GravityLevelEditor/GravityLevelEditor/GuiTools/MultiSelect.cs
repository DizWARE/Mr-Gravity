using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace GravityLevelEditor.GuiTools
{
    class MultiSelect:ITool
    {
        Point mInitial;
        Point mPrevious;
        bool mouseDown = false;

        #region ITool Members

        public void LeftMouseDown(ref EditorData data, Point gridPosition)
        {
            mInitial = gridPosition;
            mPrevious = gridPosition;
            data.SelectedEntities.Clear();
            mouseDown = true;
        }

        public void LeftMouseUp(ref EditorData data, Point gridPosition)
        {
            //data.SelectedEntities = data.Level.SelectEntities(mInitial, gridPosition);
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
            if (mouseDown&&!mPrevious.Equals(gridPosition))
            {
                data.SelectedEntities = data.Level.SelectEntities(mInitial, gridPosition);
                mPrevious = gridPosition;
                panel.Refresh();
            }
        }

        #endregion
    }
}
