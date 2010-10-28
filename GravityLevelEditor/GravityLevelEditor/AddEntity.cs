using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace GravityLevelEditor
{
    class AddEntity : IOperation
    {
        private Entity mEntity;
        private Level mLevel;

        /*
         * Redo
         *
         * Implemented function from interface IOperation.
         * Redoes a previously undone add entity operation.
         */
        public void Redo()
        {
            mLevel.AddEntity(mEntity, mEntity.Location);
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
            ArrayList e = new ArrayList();
            e.Add(mEntity);
            mLevel.RemoveEntity(e);
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
            mEntity = entity;
            mLevel = level;
        }
    }
}
