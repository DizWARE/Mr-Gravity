using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace GravityLevelEditor
{
    class RemoveEntity : IOperation
    {
        private ArrayList mEntities;
        private Level mLevel;

        /*
         * Redo
         *
         * Implemented function from interface IOperation.
         * Redoes a previously undone remove entity operation.
         */
        public void Redo()
        {
            mLevel.RemoveEntity(mEntities, true);
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
            mLevel.AddEntities(mEntities, false);
            //foreach (Entity e in mEntities)
                //mLevel.AddEntity(e, e.Location, false);
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
            mEntities = entities;
            mLevel = level;
        }
    }
}
