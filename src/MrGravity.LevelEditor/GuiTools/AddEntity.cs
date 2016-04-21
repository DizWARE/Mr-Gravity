using System.Drawing;
using System.Windows.Forms;

namespace MrGravity.LevelEditor.GuiTools
{
    internal class AddEntity:ITool
    {

        #region ITool Members

        public void LeftMouseDown(ref EditorData data, Point gridPosition)
        {
        }

        public void LeftMouseUp(ref EditorData data, Point gridPosition)
        {
            if (data.OnDeck == null) return;
            var entity = data.OnDeck.Copy();
            entity.Location = gridPosition;
            data.Level.AddEntity(entity, gridPosition, true);
            data.SelectedEntities.Clear();
            data.SelectedEntities.Add(entity);
        }

        public void RightMouseDown(ref EditorData data, Point gridPosition)
        {
            
        }

        public void RightMouseUp(ref EditorData data, Point gridPosition)
        {
            
        }

        public void MouseMove(ref EditorData data, Panel panel, Point gridPosition)
        {
            
        }

        #endregion
    }
}
