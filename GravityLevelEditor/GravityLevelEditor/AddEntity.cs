using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GravityLevelEditor
{
    class AddEntity : IOperation
    {
        private Entity mEntity;
        private Level mLevel;

        public void Redo()
        {
            mLevel.AddEntity(mEntity);
        }

        public void Undo()
        {
            mLevel.RemoveEntity(mEntity);
        }

        public AddEntity(Entity entity, Level level)
        {
            mEntity = entity;
            mLevel = level;
        }
    }
}
