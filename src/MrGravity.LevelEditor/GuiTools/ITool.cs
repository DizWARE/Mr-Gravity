using System.Drawing;
using System.Windows.Forms;

namespace MrGravity.LevelEditor.GuiTools
{
    internal interface ITool
    {
        void LeftMouseDown(ref EditorData data, Point gridPosition);
        void LeftMouseUp(ref EditorData data, Point gridPosition);
        void RightMouseDown(ref EditorData data, Point gridPosition);
        void RightMouseUp(ref EditorData data, Point gridPosition);
        void MouseMove(ref EditorData data, Panel panel, Point gridPosition);
    }
}
