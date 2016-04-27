using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;

namespace MrGravity.LevelEditor.GuiTools
{
    internal class RemoveEntity : ITool
    {
        #region ITool Members

        public void LeftMouseDown(ref EditorData data, Point gridPosition)
        {
          
        }

        public void LeftMouseUp(ref EditorData data, Point gridPosition)
        {
            var topEntity = new ArrayList();

            try
            {
                if (data.SelectedEntities.Count == 0)
                    data.SelectedEntities.Add(data.Level.SelectEntity(gridPosition));
                data.Level.RemoveEntity(data.SelectedEntities, true);
                data.SelectedEntities.Clear();
            }
            catch (Exception)
            {
                //If the tile is empty, fail silently
            }  
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
