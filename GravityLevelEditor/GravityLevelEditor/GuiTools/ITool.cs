using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace GravityLevelEditor.GuiTools
{
    interface ITool
    {
        void LeftMouseDown(Level level, Point gridPosition);
        void LeftMouseUp(Level level, Point gridPosition);
        void RightMouseDown(Level level, Point gridPosition);
        void RightMouseUp(Level level, Point gridPosition);
        void MouseMove(Level level, Point gridPosition);
    }
}
