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

        /*
         * Level
         * 
         * Constructor for Level with given settings.
         * 
         * string name: name of the level.
         * 
         * Point size: tile-based size of the level.
         * 
         * Color color: color scheme of the level.
         * 
         * Image background: background image to be drawn for the level.
         */
        public Level(string name, Point size, Color color, Image background)
        {
            mEntities = new ArrayList();
            mName = name;
            mSize = size;
            mColor = color;
            mBackground = background;
        }

        //TODO - Add constructor for loading a level from an Xml file

        /*
         * AddEntity
         * 
         * Adds an entity to the current level and add the
         * add entity operation to the history.
         * 
         * Entity entity: entity to be added to the level.
         */
        public void AddEntity(Entity entity)
        {
            mUndoHistory.Clear();
            mHistory.Push(new AddEntity(entity, this));
            mEntities.Add(entity);
        }

        /*
         * RemoveEntity
         * 
         * Remove an entity from the level and add the
         * remove entity operation to the history.
         * 
         * Entity entity: entity to be removed from the level.
         */
        public void RemoveEntity(Entity entity)
        {
            mUndoHistory.Clear();
            mHistory.Push(new RemoveEntity(entity, this));
            mEntities.Remove(entity);
        }

        /*
         * PlaceEntity
         * 
         * Move an entity to another location on the given level
         * and add the place entity operation to the history.
         * 
         * Entity entity: entity to be moved.
         * 
         * Point location: new location of the entity.
         */
        public void PlaceEntity(Entity entity, Point location)
        {
            mUndoHistory.Clear();
            mHistory.Push(new PlaceEntity(entity, entity.Location));
            entity.MoveEntity(location);
        }

        /*
         * Redo
         * 
         * Redo the last undone operation (if any).
         */
        public void Redo()
        {
            IOperation operation = mUndoHistory.Pop();
            operation.Redo();
            mHistory.Push(operation);
        }

        /*
         * Undo
         * 
         * Undo the last operation to the level.
         */
        public void Undo()
        {
            IOperation operation = mHistory.Pop();
            operation.Undo();
            mUndoHistory.Push(operation);
        }

        /*
         * Draw
         * 
         * Draw the background of the level, then tell all entities to draw themselves.
         * 
         * Graphics g: the Graphics Device to draw to.
         */
        public void Draw(Graphics g)
        {
            //TODO - Draw background using a viewport?
            g.DrawImage(mBackground, new Point(0, 0));

            foreach (Entity e in mEntities)
            {
                e.Draw(g);
            }
        }

        /***
         * SelectEntities
         * 
         * Selects all entities that are within the given vector boundaries(grid coordanates)
         * 
         * Point topLeft - Top left corner of the selection rectangle
         * Point bottomRight - Bottom right corner of the selection rectangle
         */
        public ArrayList SelectEntities(Point topLeft, Point bottomRight)
        {
            Point diff = new Point(bottomRight.X - topLeft.X, bottomRight.Y - topLeft.Y);
            Rectangle selection = new Rectangle(topLeft, new Size(diff));
            ArrayList selectedEntities = new ArrayList();

            foreach(Entity entity in mEntities)
            {
                Rectangle entityLocation = new Rectangle(entity.Location, new Size(GridSpace.SIZE));
                if (selection.IntersectsWith(entityLocation))
                {
                    entity.ToggleSelect();
                    selectedEntities.Add(entity);
                }
            }
            return selectedEntities;
        }

        //TODO - Add Load/Save functions
    }
}
