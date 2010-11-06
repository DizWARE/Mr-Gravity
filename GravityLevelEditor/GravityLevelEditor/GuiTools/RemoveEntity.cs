using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Windows.Forms;

namespace GravityLevelEditor.GuiTools
{
    class RemoveEntity : ITool
    {
        #region ITool Members

        public void LeftMouseDown(ref EditorData data, System.Drawing.Point gridPosition)
        {
          
        }

        public void LeftMouseUp(ref EditorData data, System.Drawing.Point gridPosition)
        {
            ArrayList topEntity = new ArrayList();

            try
            {
                topEntity.Add(data.Level.SelectEntity(gridPosition));
                data.SelectedEntities.Remove(topEntity[0]);
                data.Level.RemoveEntity(topEntity);
            }
            catch (Exception e)
            {
                //If the tile is empty, fail silently
            }  
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
