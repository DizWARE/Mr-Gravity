using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace GravityLevelEditor.GuiTools
{
    class MultiSelect:ITool
    {
        Point mInitial;

        #region ITool Members

        public void LeftMouseDown(ref EditorData data, Point gridPosition)
        {
            mInitial = gridPosition;
        }

        public void LeftMouseUp(ref EditorData data, Point gridPosition)
        {
            data.SelectedEntities = data.Level.SelectEntities(mInitial, gridPosition);
        }

        public void RightMouseDown(ref EditorData data, Point gridPosition)
        {
            
        }

        public void RightMouseUp(ref EditorData data, Point gridPosition)
        {
           
        }

        public void MouseMove(ref EditorData data, Point gridPosition)
        {
            
        }

        #endregion
    }
}
