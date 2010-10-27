using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Drawing;

namespace GravityLevelEditor
{
    class Level
    {
        private ArrayList mEntities;

        private bool mSaved = false;
        public bool Saved { get { return mSaved; } }

        private string mName;
        public string Name { get { return mName; } set { mName = value; } }

        private Point mSize;
        public Point Size { get { return mSize; } set { mSize = value; } }
        //TODO - Anchor for modifying level size

        private Color mColor;
        public Color Color { get { return mColor; } set { mColor = value; } }

        private Image mBackground;
        public Image Background { set { mBackground = value; } }

        private Stack<IOperation> mHistory;
        private Stack<IOperation> mUndoHistory;

        /// <summary>
        /// Create a new Level with given settings
        /// </summary>
        public Level(string name, Point size, Color color, Image background)
        {
            mEntities = new ArrayList();
            mName = name;
            mSize = size;
            mColor = color;
            mBackground = background;
        }

        //TODO - Add constructor for loading a level from an Xml file

        /// <summary>
        /// Add an entity to the level
        /// </summary>
        /// <param name="entity">Entity to be added</param>
        public void AddEntity(Entity entity)
        {
            mUndoHistory.Clear();
            mHistory.Push(new AddEntity(entity, this));
            mEntities.Add(entity);
        }

        /// <summary>
        /// Remove an entity from the level
        /// </summary>
        /// <param name="entity">Entity to be removed</param>
        public void RemoveEntity(Entity entity)
        {
            mUndoHistory.Clear();
            mHistory.Push(new RemoveEntity(entity, this));
            mEntities.Remove(entity);
        }

        /// <summary>
        /// Place an entity on the level at the given grid location
        /// </summary>
        /// <param name="entity">Entity to be placed</param>
        /// <param name="location">Grid location where entity should be pointed</param>
        public void PlaceEntity(Entity entity, Point location)
        {
            mUndoHistory.Clear();
            mHistory.Push(new PlaceEntity(entity, entity.Location));
            entity.MoveEntity(location);
        }

        /// <summary>
        /// Undo the last modification made to the level
        /// </summary>
        public void Undo()
        {
            IOperation operation = mHistory.Pop();
            operation.Undo();
            mUndoHistory.Push(operation);
        }

        /// <summary>
        /// Redo the last undo (if any)
        /// </summary>
        public void Redo()
        {
            IOperation operation = mUndoHistory.Pop();
            operation.Redo();
            mHistory.Push(operation);
        }

        /// <summary>
        /// Draw the level background, then tell all entities to draw themselves
        /// </summary>
        /// <param name="g">The Graphics Device to draw to</param>
        public void Draw(Graphics g)
        {
            //TODO - Draw background using a viewport?
            g.DrawImage(mBackground, new Point(0, 0));

            foreach (Entity e in mEntities)
            {
                e.Draw(g);
            }
        }

        //TODO - Add Load/Save functions
    }
}
