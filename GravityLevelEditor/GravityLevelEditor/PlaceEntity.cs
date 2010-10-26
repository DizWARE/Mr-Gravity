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

        public void Redo()
        {
            mEntity.Location = mNewPosition;
        }

        public void Undo()
        {
            mEntity.Location = mOldPosition;
        }

        public PlaceEntity(Entity entity, Point position)
        {
            mEntity = entity;
            mOldPosition = position;
            mNewPosition = mEntity.Location;
        }
    }
}
