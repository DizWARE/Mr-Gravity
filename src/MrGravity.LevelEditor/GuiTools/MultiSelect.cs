using System.Drawing;
using System.Windows.Forms;

namespace MrGravity.LevelEditor.GuiTools
{
    internal class MultiSelect:ITool
    {
        private Point _mPrevious;
        public Point Previous => _mPrevious;

        public Point Initial { get; private set; }

        public bool Selecting { get; private set; }

        #region ITool Members

        public void LeftMouseDown(ref EditorData data, Point gridPosition)
        {
            Initial = gridPosition;
            _mPrevious = gridPosition;
            data.SelectedEntities.Clear();
            Selecting = true;
        }

        public void LeftMouseUp(ref EditorData data, Point gridPosition)
        {
            Selecting = false;
        }

        public void RightMouseDown(ref EditorData data, Point gridPosition)
        {
            
        }

        public void RightMouseUp(ref EditorData data, Point gridPosition)
        {
           
        }

        public void MouseMove(ref EditorData data, Panel panel, Point gridPosition)
        {
            if (Selecting&&!_mPrevious.Equals(gridPosition))
            {
                data.SelectedEntities = data.Level.SelectEntities(Initial, gridPosition, true);
                _mPrevious = gridPosition;
                panel.Invalidate(panel.DisplayRectangle);
            }
        }

        #endregion
    }
}
