using System.Collections;
using System.Drawing;

namespace MrGravity.LevelEditor.IOperationClasses
{
    internal class MoveEntity : IOperation
    {
        private readonly ArrayList _mEntities;
        private readonly Size _mOffset;

        /*
         * Redo
         *
         * Implemented function from interface IOperation.
         * Redoes a previously undone place entity operation.
         */
        public void Redo()
        {
            foreach (Entity entity in _mEntities)
                entity.MoveEntity(Point.Add(entity.Location, _mOffset));
        }

        /*
         * Undo
         *
         * Implemented function from interface IOperation.
         * Undoes a place entity operation, moving the entities
         * to their previous location on the level.
         */
        public void Undo()
        {
            foreach (Entity entity in _mEntities)
                entity.MoveEntity(Point.Subtract(entity.Location, _mOffset));
        }

        /*
         * PlaceEntity
         * 
         * Constructor for place entity operation. Holds all
         * data required to either undo or redo the given operation.
         * 
         * ArrayList entities: the entities that are being placed on the level.
         * 
         * Point offset: the difference between the entities' old and new locations.
         */
        public MoveEntity(ArrayList entities, Size offset)
        {
            _mEntities = entities;
            _mOffset = offset;
        }
    }
}
