using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace GravityLevelEditor
{
    class SelectEntity : IOperation
    {
        private ArrayList mEntities;

        /*
         * Redo
         *
         * Implemented function from interface IOperation.
         * Reselects previously unselected entities.
         */
        public void Redo()
        {
            //TODO - FIGURE OUT SOMETHING ELSE
            //foreach (Entity e in mEntities)
            //    e.ToggleSelect();
        }

        /*
         * Undo
         *
         * Implemented function from interface IOperation.
         * Deselects last selected entities.
         */
        public void Undo()
        {
            //TODO - Find a new way to do this with new system
            //foreach (Entity e in mEntities)
            //    e.ToggleSelect();
        }

        /*
         * SelectEntity
         * 
         * Constructor for select entity operation. Holds all
         * data required to either undo or redo the given operation.
         * 
         * ArrayList entities: list of entities being selected
         */
        public SelectEntity(ArrayList entities)
        {
            mEntities = entities;
        }
    }
}
