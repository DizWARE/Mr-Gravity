using System.Drawing;
using System.Windows.Forms;
using MrGravity.LevelEditor.EntityCreationForm;

namespace MrGravity.LevelEditor.GuiTools
{
    internal class SingleSelect:ITool
    {
        private Point _mInitial;
        private Point _mPrevious;

        private bool _mouseDown;

        #region ITool Members

        public void LeftMouseDown(ref EditorData data, Point gridPosition)
        {
            _mPrevious = _mInitial = gridPosition;
            _mouseDown = true;
        }
        
        public void LeftMouseUp(ref EditorData data, Point gridPosition)
        {
            if(gridPosition.Equals(_mInitial))
            {
                if(!data.CtrlHeld) data.SelectedEntities.Clear();
                var selected = data.Level.SelectEntity(gridPosition);
                if (selected != null && !data.SelectedEntities.Contains(selected))
                    data.SelectedEntities.Add(selected);
                else if (selected == null && !data.CtrlHeld)
                    data.SelectedEntities.Clear();
            }
            if (data.SelectedEntities.Count > 0)
            {
                data.Level.MoveEntity(data.SelectedEntities,
                    new Size(Point.Subtract(_mInitial, new Size(gridPosition))), false);
                data.Level.MoveEntity(data.SelectedEntities,
                    new Size(Point.Subtract(gridPosition, new Size(_mInitial))), true);
            }
            _mouseDown = false;
        }


        public void RightMouseDown(ref EditorData data, Point gridPosition)
        {
            if(data.SelectedEntities.Count != 1) return;

            var properties = new AdditionalProperties(data.Level.SelectEntity(gridPosition).Properties);
            properties.Editable = false;
            if (properties.ShowDialog() == DialogResult.OK)
                data.Level.SelectEntity(gridPosition).Properties = properties.Properties;
        }

        public void RightMouseUp(ref EditorData data, Point gridPosition)
        {
            
        }

        public void MouseMove(ref EditorData data, Panel panel, Point gridPosition)
        {
            if (!_mPrevious.Equals(gridPosition) && data.SelectedEntities.Count > 0 && _mouseDown)
            {
                //Keep an eye on this. The SelectedEntities can return an empty list
                data.Level.MoveEntity(data.SelectedEntities,
                    new Size(Point.Subtract(gridPosition, new Size(_mPrevious))), false);
                _mPrevious = gridPosition;
                panel.Invalidate(panel.DisplayRectangle);
            }
        }

        #endregion
    }
}
