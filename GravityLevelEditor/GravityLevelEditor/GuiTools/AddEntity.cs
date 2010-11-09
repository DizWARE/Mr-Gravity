using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GravityLevelEditor.EntityCreationForm;
using System.Windows.Forms;

namespace GravityLevelEditor.GuiTools
{
    class AddEntity:ITool
    {

        #region ITool Members

        public void LeftMouseDown(ref EditorData data, System.Drawing.Point gridPosition)
        {
        }

        public void LeftMouseUp(ref EditorData data, System.Drawing.Point gridPosition)
        {
            if (data.OnDeck == null) return;
            Entity entity = data.OnDeck.Copy();
            entity.Location = gridPosition;
            data.Level.AddEntity(entity, gridPosition);
            data.SelectedEntities.Clear();
            data.SelectedEntities.Add(entity);
        }

        public void RightMouseDown(ref EditorData data, System.Drawing.Point gridPosition)
        {
            
        }

        public void RightMouseUp(ref EditorData data, System.Drawing.Point gridPosition)
        {
            
        }

        public void MouseMove(ref EditorData data, Panel panel, System.Drawing.Point gridPosition)
        {
            
        }

        #endregion
    }
}
