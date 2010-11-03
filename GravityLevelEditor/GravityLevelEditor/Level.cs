using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Drawing;
using System.Xml;
using System.Xml.Linq;

namespace GravityLevelEditor
{
    class Level
    {
        private ArrayList mEntities;
        private ArrayList mClipboard;

        private bool mSaved = false;
        public bool Saved { get { return mSaved; } }

        private string mName;
        public string Name { get { return mName; } set { mName = value; } }

        private Point mSize;
        public Point Size { get { return mSize; }}
        //TODO - Anchor for modifying level size

        private Color mColor;
        public Color Color { get { return mColor; } set { mColor = value; } }

        private Image mBackground;
        public Image Background { get { return mBackground; } set { mBackground = value; } }

        private Stack<IOperation> mHistory = new Stack<IOperation>();
        private Stack<IOperation> mUndoHistory = new Stack<IOperation>();

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
            mClipboard = new ArrayList();
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
         * 
         * Point location: location where we are placing this Entity
         */
        public ArrayList AddEntity(Entity entity, Point location)
        {
            mUndoHistory.Clear();
            mHistory.Push(new AddEntity(entity, this));
            mEntities.Add(entity);

            entity.MoveEntity(location);

            return SelectEntities(location, location);
        }

        /*
         * AddEntities
         * 
         * Add a list of entities to the level(i.e. a paste of a group of entities)
         * All entities in the list are required to have locations already preset
         * -Adds an IOperation onto the history stack
         * 
         * ArrayList entities: List of entities. Must have preset locations
         * 
         * Return Value: Rereturns the list for editor selection
         */
        public ArrayList AddEntities(ArrayList entities)
        {
            mUndoHistory.Clear();
            mHistory.Push(new AddEntity(entities, this));
            foreach (Entity entity in entities)
                mEntities.Add(entity);            

            return entities;
        }

        /*
         * RemoveEntity
         * 
         * Remove a list of entities from the level and add the
         * remove entity operation to the history.
         * 
         * ArrayList entities: entities to be removed from the level.
         */
        public void RemoveEntity(ArrayList entities)
        {
            mUndoHistory.Clear();
            mHistory.Push(new RemoveEntity(entities, this));
            foreach (Entity entity in entities)
                mEntities.Remove(entity);
        }

        /*
         * MoveEntity
         * 
         * Offset a list of entities to another location on the level
         * and add the move entity operation to the history.
         * 
         * ArrayList entities: entities to be moved.
         * 
         * Size offset: The difference that needs to be added to the entities current location
         */
        public void MoveEntity(ArrayList entities, Size offset)
        {
            mUndoHistory.Clear();
            mHistory.Push(new MoveEntity(entities, offset));

            foreach(Entity entity in entities)
                entity.MoveEntity(Point.Add(entity.Location, offset));
        }

        /*
         * Resize
         * 
         * Resizes the level.
         * 
         * int rows: new row size.
         * 
         * int cols: new column size.
         */
        public void Resize(int rows, int cols)
        {
            mSize.X = rows;
            mSize.Y = cols;
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
         * Copy
         * 
         * Copies a list of entities
         * 
         * ArrayList entities: List of selected entities
         */
        public void Copy(ArrayList entities)
        {
            mClipboard.Clear();
            foreach (Entity entity in entities)
                mClipboard.Add(entity.Copy());
        }

        /*
         * Cut
         * 
         * Copies and remove a list of entities
         * 
         * ArrayList entities: List of selected entities
         */
        public void Cut(ArrayList entities)
        {
            RemoveEntity(entities);
            Copy(entities);
        }

        /*
         * Paste
         * 
         * Pastes the entities on the screen. Currently, they drop down starting at the upper left corner
         */
        public void Paste()
        {
            Point minPoint = ((Entity)mClipboard[0]).Location;
            foreach(Entity entity in mClipboard)
                if (minPoint.X >= entity.Location.X && minPoint.Y >= entity.Location.Y)
                    minPoint = entity.Location;

            foreach (Entity entity in mClipboard)
                entity.Location = Point.Subtract(entity.Location, new Size(minPoint));

            AddEntities(mClipboard);
        }

        /*
         * InTile
         * 
         * Gets all the entities in the current Grid Tile
         * 
         * Point gridLocation: The location that we want all the entities
         * 
         * Return Value: Return a list of all the Entities at the given grid tile. Will return 
         * a list that is in top down order(The entity that is drawn on top is at the top of the list
         */
        public ArrayList InTile(Point gridLocation)
        {
            ArrayList tile = new ArrayList();
            foreach (Entity entity in mEntities)
                if (entity.Location.Equals(gridLocation))
                    tile.Insert(0, entity);

            return tile;
        }

        /*
         * SelectEntity
         * 
         * Select the top most entity at this grid location
         * 
         * Point gridLocation: Location we want to select
         * 
         * Return Value: The entity that is at the top
         */
        public Entity SelectEntity(Point gridLocation)
        {
            return (Entity)InTile(gridLocation)[0];
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

            foreach (Entity entity in mEntities)
            {
                entity.Draw(g);
            }
        }

        /***
         * SelectEntities
         * 
         * Selects all entities that are within the given vector boundaries(grid coordinates)
         * 
         * Point topLeft - Top left corner of the selection rectangle
         * Point bottomRight - Bottom right corner of the selection rectangle
         */
        public ArrayList SelectEntities(Point firstPoint, Point secondPoint)
        {
            Point diff = new Point(secondPoint.X - firstPoint.X, secondPoint.Y - firstPoint.Y);
            Rectangle selection = new Rectangle(firstPoint, new Size(diff));
            ArrayList selectedEntities = new ArrayList();

            //For every entity, check if it is within the selection bounds. 
                //If it is, select it, and add it to the selection list
            foreach(Entity entity in mEntities)
            {
                if (selection.IntersectsWith(GridSpace.GetDrawingRegion(entity.Location)))
                {
                    entity.ToggleSelect();
                    selectedEntities.Add(entity);
                }
            }

            mHistory.Push(new SelectEntity(selectedEntities));
            return selectedEntities;
        }

        //TODO - Add Load/Save functions

        public void Save(string filename)
        {

            XElement entityTree = new XElement("Entities");
            foreach (Entity entity in mEntities) {
                entityTree.Add(entity.Export());
            }
            XDocument xDoc = new XDocument(
                new XElement("Level",
                    new XElement("Name", this.Name),
                    new XElement("Size",
                        new XAttribute("X", this.Size.X),
                        new XAttribute("Y", this.Size.Y)),
                    new XElement("Background", this.Background.ToString()),
                    new XElement("Color", this.Color.ToString()),
                    entityTree));

            xDoc.Save(filename);
        }
    }
}
