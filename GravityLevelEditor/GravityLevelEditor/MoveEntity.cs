using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Collections;

namespace GravityLevelEditor
{
    class MoveEntity : IOperation
    {
        private ArrayList mEntities;
        private Size mOffset;

        /*
         * Redo
         *
         * Implemented function from interface IOperation.
         * Redoes a previously undone place entity operation.
         */
        public void Redo()
        {
            foreach (Entity entity in mEntities)
                entity.MoveEntity(Point.Add(entity.Location, mOffset));
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
            foreach (Entity entity in mEntities)
                entity.MoveEntity(Point.Subtract(entity.Location, mOffset));
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
            mEntities = entities;
            mOffset = offset;
        }
    }
}
