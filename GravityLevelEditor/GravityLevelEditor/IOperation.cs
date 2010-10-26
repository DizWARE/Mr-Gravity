using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GravityLevelEditor
{
    public interface IOperation
    {
        void Redo();
        void Undo();
    }
}
