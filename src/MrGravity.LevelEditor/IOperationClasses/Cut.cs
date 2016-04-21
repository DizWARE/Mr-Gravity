using System.Collections;

namespace MrGravity.LevelEditor.IOperationClasses
{
    internal class Cut : IOperation
    {
        private readonly ArrayList _mEntities;
        private readonly ArrayList _mOldClipboard;
        private readonly Level _mLevel;

        /*
         * Redo
         *
         * Implemented function from interface IOperation.
         * Recuts selected entities and places them on the clipboard.
         */
        public void Redo()
        {
            _mLevel.Clipboard.Clear();
            foreach (Entity entity in _mEntities)
                _mLevel.Clipboard.Add(entity);
            _mLevel.RemoveEntity(_mEntities, false);
        }

        /*
         * Undo
         *
         * Implemented function from interface IOperation.
         * Re-adds previously cut entities and restores the clipboard.
         */
        public void Undo()
        {
            _mLevel.Clipboard = _mOldClipboard;
            _mLevel.AddEntities(_mEntities, false); //mEntities.Count = 0 wtf??
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
            _mEntities = entities;
            _mOldClipboard = level.Clipboard;
            _mLevel = level;
        }
    }
}
