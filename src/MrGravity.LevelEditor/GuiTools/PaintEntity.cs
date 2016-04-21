using System.Drawing;
using System.Windows.Forms;

namespace MrGravity.LevelEditor.GuiTools
{
    internal class PaintEntity : ITool
    {
        private Point _mPrevious;
        private bool _mPainting;

        #region ITool Members

        public void LeftMouseDown(ref EditorData data, Point gridPosition)
        {

            if (data.OnDeck == null) return;
            _mPainting = true;
            var entity = data.OnDeck.Copy();
            entity.Location = gridPosition;
            data.Level.AddEntity(entity, gridPosition, true);
            data.SelectedEntities.Clear();
            data.SelectedEntities.Add(entity);
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
            if (data.OnDeck == null || !data.OnDeck.Paintable) return;
            if (data.Level.SelectEntity(gridPosition) == null && _mPainting && !_mPrevious.Equals(gridPosition))
            {
                var entity = data.OnDeck.Copy();
                entity.Location = gridPosition;
                data.Level.AddEntity(entity, gridPosition, true);
                data.SelectedEntities.Clear();
                data.SelectedEntities.Add(entity);
                _mPrevious = gridPosition;
                panel.Invalidate(panel.DisplayRectangle);
            }
        }

        #endregion
    }
}
