using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace GravityLevelEditor
{
    class Cut : IOperation
    {
        private ArrayList mEntities;
        private ArrayList mOldClipboard;
        private Level mLevel;

        /*
         * Redo
         *
         * Implemented function from interface IOperation.
         * Recuts selected entities and places them on the clipboard.
         */
        public void Redo()
        {
            mLevel.Clipboard.Clear();
            foreach (Entity entity in mEntities)
                mLevel.Clipboard.Add(entity);
            mLevel.RemoveEntity(mEntities, false);
        }

        /*
         * Undo
         *
         * Implemented function from interface IOperation.
         * Re-adds previously cut entities and restores the clipboard.
         */
        public void Undo()
        {
            mLevel.Clipboard = mOldClipboard;
            mLevel.AddEntities(mEntities, false); //mEntities.Count = 0 wtf??
        }

        /*
         * Cut
         * 
         * Constructor for cut operation. Holds all
         * data required to either undo or redo the given operation.
         * 
         * ArrayList entities: list of entities being cut
         * 
         * Level level: the current working level
         */
        public Cut(ArrayList entities, Level level)
        {
            mEntities = entities;
            mOldClipboard = level.Clipboard;
            mLevel = level;
        }
    }
}
