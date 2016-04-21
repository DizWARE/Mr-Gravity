using System.Collections;

namespace MrGravity.LevelEditor.IOperationClasses
{
    internal class AddEntity : IOperation
    {
        private readonly ArrayList _mEntities;
        private readonly Level _mLevel;

        /*
         * Redo
         *
         * Implemented function from interface IOperation.
         * Redoes a previously undone add entity operation.
         */
        public void Redo()
        {
            foreach(Entity entity in _mEntities)
                _mLevel.AddEntity(entity, entity.Location, false);
        }

        /*
         * Undo
         *
         * Implemented function from interface IOperation.
         * Undoes an add entity operation, removing the entity
         * from the level.
         */
        public void Undo()
        {
            _mLevel.RemoveEntity(_mEntities, false);
        }

        /*
         * AddEntity
         * 
         * Constructor for add entity operation. Holds all
         * data required to either undo or redo the given operation.
         * 
         * ArrayList entities: the entities that are being added to the level.
         * 
         * Level level: the level that is having the entities added.
         */
        public AddEntity(ArrayList entities, Level level)
        {
            _mEntities = entities;
            _mLevel = level;
        }

        /*
         * AddEntity
         * 
         * Constructor for add entity operation. Holds all
         * data required to either undo or redo the given operation.
         * 
         * Entity entity: the entity that is being added to the level.
         * 
         * Level level: the level that is having the entity added.
         */
        public AddEntity(Entity entity, Level level)
        {
            _mEntities = new ArrayList();
            _mEntities.Add(entity);
            _mLevel = level;
        }
    }
}
