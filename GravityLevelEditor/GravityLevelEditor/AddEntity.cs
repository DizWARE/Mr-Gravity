using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace GravityLevelEditor
{
    class AddEntity : IOperation
    {
        private ArrayList mEntities;
        private Level mLevel;

        /*
         * Redo
         *
         * Implemented function from interface IOperation.
         * Redoes a previously undone add entity operation.
         */
        public void Redo()
        {
            foreach(Entity entity in mEntities)
                mLevel.AddEntity(entity, entity.Location);
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
            mLevel.RemoveEntity(mEntities);
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
            mEntities = entities;
            mLevel = level;
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
            mEntities = new ArrayList();
            mEntities.Add(entity);
            mLevel = level;
        }
    }
}
