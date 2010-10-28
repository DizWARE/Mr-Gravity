using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GravityLevelEditor
{
    class RemoveEntity : IOperation
    {
        private Entity mEntity;
        private Level mLevel;

        /*
         * Redo
         *
         * Implemented function from interface IOperation.
         * Redoes a previously undone remove entity operation.
         */
        public void Redo()
        {
            mLevel.RemoveEntity(mEntity);
        }

        /*
         * Undo
         *
         * Implemented function from interface IOperation.
         * Undoes a remove entity operation, adding the entity
         * back onto the level.
         */
        public void Undo()
        {
            mLevel.AddEntity(mEntity);
        }

        /*
         * RemoveEntity
         * 
         * Constructor for remove entity operation. Holds all
         * data required to either undo or redo the given operation.
         * 
         * Entity entity: the entity that is being removed from the level.
         * 
         * Level level: the level that is having the entity removed.
         */
        public RemoveEntity(Entity entity, Level level)
        {
            mEntity = entity;
            mLevel = level;
        }
    }
}
