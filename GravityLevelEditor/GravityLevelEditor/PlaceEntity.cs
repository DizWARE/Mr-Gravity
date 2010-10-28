using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace GravityLevelEditor
{
    class PlaceEntity : IOperation
    {
        private Entity mEntity;
        private Point mOldPosition;
        private Point mNewPosition;

        /*
         * Redo
         *
         * Implemented function from interface IOperation.
         * Redoes a previously undone place entity operation.
         */
        public void Redo()
        {
            mEntity.Location = mNewPosition;
        }

        /*
         * Undo
         *
         * Implemented function from interface IOperation.
         * Undoes a place entity operation, moving the entity
         * to its previous location on the level.
         */
        public void Undo()
        {
            mEntity.Location = mOldPosition;
        }

        /*
         * PlaceEntity
         * 
         * Constructor for place entity operation. Holds all
         * data required to either undo or redo the given operation.
         * 
         * Entity entity: the entity that is being placed on the level.
         * 
         * Point position: the old position of the entity.
         */
        public PlaceEntity(Entity entity, Point position)
        {
            mEntity = entity;
            mOldPosition = position;
            mNewPosition = mEntity.Location;
        }
    }
}
