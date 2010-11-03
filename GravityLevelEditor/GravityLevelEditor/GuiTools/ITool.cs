using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace GravityLevelEditor.GuiTools
{
    interface ITool
    {
        void LeftMouseDown(ref EditorData data, Point gridPosition);
        void LeftMouseUp(ref EditorData data, Point gridPosition);
        void RightMouseDown(ref EditorData data, Point gridPosition);
        void RightMouseUp(ref EditorData data, Point gridPosition);
        void MouseMove(ref EditorData data, Point gridPosition);
    }
}
