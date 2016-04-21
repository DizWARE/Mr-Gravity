using System.Collections;

namespace MrGravity.LevelEditor.IOperationClasses
{
    internal class RemoveEntity : IOperation
    {
        private readonly ArrayList _mEntities;
        private readonly Level _mLevel;

        /*
         * Redo
         *
         * Implemented function from interface IOperation.
         * Redoes a previously undone remove entity operation.
         */
        public void Redo()
        {
            _mLevel.RemoveEntity(_mEntities, true);
        }

        /*
         * Undo
         *
         * Implemented function from interface IOperation.
         * Undoes a remove entity operation, adding the entities
         * back onto the level.
         */
        public void Undo()
        {
            _mLevel.AddEntities(_mEntities, false);
        }

        /*
         * RemoveEntity
         * 
         * Constructor for remove entity operation. Holds all
         * data required to either undo or redo the given operation.
         * 
         * ArrayList entities: the entities that are being removed from the level.
         * 
         * Level level: the level that is having the entity removed.
         */
        public RemoveEntity(ArrayList entities, Level level)
        {
            _mEntities = entities;
            _mLevel = level;
        }
    }
}
