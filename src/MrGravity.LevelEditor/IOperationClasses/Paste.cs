using System;
using System.Collections;

namespace MrGravity.LevelEditor.IOperationClasses
{
    internal class Paste : IOperation
    {
        private ArrayList _mEntities;

        /*
         * Redo
         *
         * Implemented function from interface IOperation.
         * Repastes the entites onto the level.
         */
        public void Redo()
        {
            throw new NotImplementedException();
        }

        /*
         * Undo
         *
         * Implemented function from interface IOperation.
         * Removes the last pasted entites.
         */
        public void Undo()
        {
            throw new NotImplementedException();
        }

        /*
         * Paste
         * 
         * Constructor for paste operation. Holds all
         * data required to either undo or redo the given operation.
         * 
         * ArrayList entities: list of entities being pasted
         */
        public Paste(ArrayList entities)
        {
            _mEntities = entities;
        }
    }
}
