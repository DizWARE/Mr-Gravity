using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GravityLevelEditor
{
    public interface IOperation
    {
        /*
         * Redo
         * 
         * Interface method implemented by IOperation classes.
         */
        void Redo();

        /*
         * Undo
         * 
         * Interface method implemented by IOperation classes.
         */
        void Undo();
    }
}
