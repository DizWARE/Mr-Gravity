using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace GravityLevelEditor.GuiTools
{
    class PaintEntity : ITool
    {
        Point mPrevious;
        bool mPainting;

        #region ITool Members

        public void LeftMouseDown(ref EditorData data, System.Drawing.Point gridPosition)
        {

            if (data.OnDeck == null) return;
            mPainting = true;
            Entity entity = data.OnDeck.Copy();
            entity.Location = gridPosition;
            data.Level.AddEntity(entity, gridPosition);
            data.SelectedEntities.Clear();
            data.SelectedEntities.Add(entity);
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
            if (data.OnDeck == null || !data.OnDeck.Paintable) return;
            if (data.Level.SelectEntity(gridPosition) == null && mPainting && !mPrevious.Equals(gridPosition))
            {
                Entity entity = data.OnDeck.Copy();
                entity.Location = gridPosition;
                data.Level.AddEntity(entity, gridPosition);
                data.SelectedEntities.Clear();
                data.SelectedEntities.Add(entity);
                mPrevious = gridPosition;
                panel.Invalidate(panel.DisplayRectangle);
            }
        }

        #endregion
    }
}
