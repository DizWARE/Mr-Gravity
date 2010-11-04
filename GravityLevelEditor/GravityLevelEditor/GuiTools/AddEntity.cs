using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GravityLevelEditor.GuiTools
{
    class AddEntity:ITool
    {

        #region ITool Members

        public void LeftMouseDown(ref EditorData data, System.Drawing.Point gridPosition)
        {
            if(data.OnDeck == null) {/*launch the Entity select dialog*/}
        }

        public void LeftMouseUp(ref EditorData data, System.Drawing.Point gridPosition)
        {
            if(data.OnDeck != null)
                data.Level.AddEntity(data.OnDeck.Copy(), gridPosition);
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
