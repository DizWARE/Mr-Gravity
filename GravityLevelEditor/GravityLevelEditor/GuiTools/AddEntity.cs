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
            if (data.OnDeck == null) 
            {
                CreateEntity entityDialog = new CreateEntity();
                if (entityDialog.ShowDialog() == DialogResult.OK)
                    data.OnDeck = entityDialog.SelectedEntity;

            }
        }

        public void LeftMouseUp(ref EditorData data, System.Drawing.Point gridPosition)
        {
            if (data.OnDeck == null) return;
            Entity entity = data.OnDeck.Copy();
            entity.Location = gridPosition;
            data.Level.AddEntity(entity, gridPosition);
        }

        public void RightMouseDown(ref EditorData data, System.Drawing.Point gridPosition)
        {
            
        }

        public void RightMouseUp(ref EditorData data, System.Drawing.Point gridPosition)
        {
            
        }

        public void MouseMove(ref EditorData data, System.Drawing.Point gridPosition)
        {
            
        }

        #endregion
    }
}
