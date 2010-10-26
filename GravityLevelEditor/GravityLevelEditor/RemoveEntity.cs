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

        public void Redo()
        {
            mLevel.RemoveEntity(mEntity);
        }

        public void Undo()
        {
            mLevel.AddEntity(mEntity);
        }

        public RemoveEntity(Entity entity, Level level)
        {
            mEntity = entity;
            mLevel = level;
        }
    }
}
