using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;

namespace MrGravity.LevelEditor.GuiTools
{
    internal class DepaintEntity : ITool
    {
        private Point _mPrevious;
        private bool _mPainting;

        #region ITool Members

        public void LeftMouseDown(ref EditorData data, Point gridPosition)
        {
            _mPrevious = gridPosition;
            _mPainting = true;
            data.SelectedEntities.Clear();
            data.SelectedEntities.Add(data.Level.SelectEntity(gridPosition));
            data.Level.RemoveEntity(data.SelectedEntities, true);
            data.SelectedEntities.Clear();
        }

        public void LeftMouseUp(ref EditorData data, Point gridPosition)
        {
            _mPainting = false;
        }

        public void RightMouseDown(ref EditorData data, Point gridPosition)
        {

        }

        public void RightMouseUp(ref EditorData data, Point gridPosition)
        {

        }

        public void MouseMove(ref EditorData data, Panel panel, Point gridPosition)
        {
            var topEntity = new ArrayList();
            var entity = data.Level.SelectEntity(gridPosition);

            if (entity != null && _mPainting && !_mPrevious.Equals(gridPosition))
                try
                {
                    data.SelectedEntities.Clear();
                    topEntity.Add(entity);                    
                    data.Level.RemoveEntity(topEntity, true);
                    _mPrevious = gridPosition;
                    panel.Invalidate(panel.DisplayRectangle);
                }
                catch (Exception)
                {
                    //If the tile is empty, fail silently
                } 
        }

        #endregion
    }
}
