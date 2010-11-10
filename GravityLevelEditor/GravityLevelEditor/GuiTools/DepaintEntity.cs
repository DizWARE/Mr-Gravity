using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Drawing;

namespace GravityLevelEditor.GuiTools
{
    class DepaintEntity : ITool
    {
        Point mPrevious;
        bool mPainting;

        #region ITool Members

        public void LeftMouseDown(ref EditorData data, System.Drawing.Point gridPosition)
        {
            mPrevious = gridPosition;
            mPainting = true;
            data.SelectedEntities.Clear();
            data.SelectedEntities.Add(data.Level.SelectEntity(gridPosition));
            data.Level.RemoveEntity(data.SelectedEntities);
            data.SelectedEntities.Clear();
        }

        public void LeftMouseUp(ref EditorData data, System.Drawing.Point gridPosition)
        {
            mPainting = false;
        }

        public void RightMouseDown(ref EditorData data, System.Drawing.Point gridPosition)
        {

        }

        public void RightMouseUp(ref EditorData data, System.Drawing.Point gridPosition)
        {

        }

        public void MouseMove(ref EditorData data, System.Windows.Forms.Panel panel, System.Drawing.Point gridPosition)
        {
            ArrayList topEntity = new ArrayList();
            Entity entity = data.Level.SelectEntity(gridPosition);

            if (entity != null && mPainting && !mPrevious.Equals(gridPosition))
                try
                {
                    data.SelectedEntities.Clear();
                    topEntity.Add(entity);                    
                    data.Level.RemoveEntity(topEntity);
                    mPrevious = gridPosition;
                    panel.Invalidate(panel.DisplayRectangle);
                }
                catch (Exception e)
                {
                    //If the tile is empty, fail silently
                } 
        }

        #endregion
    }
}
