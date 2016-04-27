using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml.Linq;
using MrGravity.LevelEditor.IOperationClasses;
using MrGravity.LevelEditor.XML;

namespace MrGravity.LevelEditor
{
    internal class Level
    {
        private readonly ArrayList _mEntities;

        public ArrayList Clipboard { get; set; }

        public bool Saved { get; } = false;

        public string Name { get; set; }

        private Point _mSize;
        public Point Size { get { return _mSize; } set { _mSize = value; } }

        public Image Background { get; set; }

        private readonly Stack<IOperation> _mHistory = new Stack<IOperation>();
        private readonly Stack<IOperation> _mUndoHistory = new Stack<IOperation>();

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
        public Level(string name, Point size, Image background)
        {
            _mEntities = new ArrayList();
            Entity.ObjectId = 0;
            Clipboard = new ArrayList();
            Name = name;
            _mSize = size;
            Background = background;
        }

        /*
         * Level
         * 
         * Constructor for a level from a given XML file.
         * 
         * string filename: the XML file to load the level from.
         */
        public Level(string filename)
        {
            var currentDirectory = "..\\..\\..\\..\\MrGravity\\Content\\Images";
            var d = new DirectoryInfo(currentDirectory);

            _mEntities = new ArrayList();
            Clipboard = new ArrayList();

            Entity.ObjectId = 0;

            var xLevel = XElement.Load(filename);

            foreach (var el in xLevel.Elements())
            {
                if (el.Name == XmlKeys.LName)
                {
                    Name = el.Value;
                }
                if (el.Name == XmlKeys.Size)
                {
                    var xSize = new Point(Convert.ToInt32(el.Attribute(XmlKeys.LX).Value), Convert.ToInt32(el.Attribute(XmlKeys.LY).Value));
                    Size = xSize;

                }
                if (el.Name == XmlKeys.Background)
                {
                    try { Background = Image.FromFile(d.FullName + "\\" + el.Value + XmlKeys.Png); }
                    catch (Exception) { Background = Image.FromFile(d.FullName + "\\errorBG" + XmlKeys.Png); }
                    Background.Tag = el.Value;
                }
                if (el.Name == XmlKeys.Entities)
                {

                    _mEntities.Clear();

                    foreach (var entity in el.Elements())
                    {
                        _mEntities.Add(new Entity(entity));
                    }
                }
            }
        }

        /*
         * AddEntity
         * 
         * Adds an entity to the current level and add the
         * add entity operation to the history.
         * 
         * Entity entity: entity to be added to the level.
         * 
         * Point location: location where we are placing this Entity
         * 
         * bool addToHistory: whether or not to add the operation to the undo history
         */
        public ArrayList AddEntity(Entity entity, Point location, bool addToHistory)
        {
            if (addToHistory)
            {
                _mUndoHistory.Clear();
                _mHistory.Push(new AddEntity(entity, this));
            }

            _mEntities.Add(entity);
            entity.MoveEntity(location);

            return SelectEntities(location, location, false);
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
         * 
         * bool addToHistory: whether or not to add the operation to the undo history
         */
        public ArrayList AddEntities(ArrayList entities, bool addToHistory)
        {
            if (addToHistory)
            {
                _mUndoHistory.Clear();
                _mHistory.Push(new AddEntity(entities, this));
            }
            foreach (Entity entity in entities)
                _mEntities.Add(entity);            

            return entities;
        }

        /*
         * RemoveEntity
         * 
         * Remove a list of entities from the level and add the
         * remove entity operation to the history.
         * 
         * ArrayList entities: entities to be removed from the level.
         * 
         * bool addToHistory: whether or not to add the operation to the undo history
         */
        public void RemoveEntity(ArrayList entities, bool addToHistory)
        {
            if(addToHistory)
            {
                _mHistory.Push(new RemoveEntity(entities, this));
                _mUndoHistory.Clear();
            }
            foreach (Entity entity in entities)
                _mEntities.Remove(entity);
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
         * 
         * bool addToHistory: whether or not to add the operation to the undo history
         */
        public void MoveEntity(ArrayList entities, Size offset, bool addToHistory)
        {
            if (addToHistory)
            {
                _mUndoHistory.Clear();
                _mHistory.Push(new MoveEntity(entities, offset));
            }

            foreach(Entity entity in entities)
                if(entity != null)
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
            _mSize.X = cols;
            _mSize.Y = rows;
        }

        /*
         * Redo
         * 
         * Redo the last undone operation (if any).
         */
        public void Redo()
        {
            if (_mUndoHistory.Count > 0)
            {
                var operation = _mUndoHistory.Pop();
                operation.Redo();
                _mHistory.Push(operation);
            }
        }

        /*
         * Undo
         * 
         * Undo the last operation to the level.
         */
        public void Undo()
        {
            if (_mHistory.Count > 0)
            {
                var operation = _mHistory.Pop();
                operation.Undo();
                _mUndoHistory.Push(operation);
            }
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
            Clipboard.Clear();
            foreach (Entity entity in entities)
                Clipboard.Add(entity.Copy());
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
            if (entities.Count == 0) return;

            RemoveEntity(entities, false);
            Copy(entities);

            _mUndoHistory.Clear();
            _mHistory.Push(new Cut(entities, this));
        }

        /*
         * Paste
         * 
         * Pastes the entities on the screen. Currently, they drop down starting at the upper left corner
         */
        public ArrayList Paste()
        {
            var minPoint = new Point(1,1);

            foreach (Entity entity in Clipboard)
                entity.Location = Point.Add(entity.Location, new Size(minPoint));

            AddEntities(Clipboard, true);
            return Clipboard;
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
            var tile = new ArrayList();
            foreach (Entity entity in _mEntities)
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
            var inTile = InTile(gridLocation);
            if (inTile.Count > 0)
            {
                var selectedEntity = (Entity)inTile[0];
                return selectedEntity;
            }
            return null;
        }

        /*
         * Draw
         * 
         * Draw the background of the level, then tell all entities to draw themselves.
         * 
         * Graphics g: the Graphics Device to draw to.
         * 
         * Point offset: the offset that the level editor is at.
         */
        public void Draw(Graphics g, Point offset)
        {            
            g.DrawImage(Background, new Rectangle(offset,
                new Size(GridSpace.GetDrawingCoord(_mSize))));

            foreach (Entity entity in _mEntities)
                entity.Draw(g, offset);
        }

        /// <summary>
        /// Draws the level in its unzoomed location
        /// </summary>
        /// <param name="g">Graphics to draw to</param>
        public void Draw(Graphics g)
        {
            g.DrawImage(Background, new Rectangle(new Point(),
                new Size(GridSpace.GetPixelCoord(_mSize))));

            foreach (Entity entity in _mEntities)
                entity.Draw(g);
        }

        /***
         * SelectEntities
         * 
         * Selects all entities that are within the given vector boundaries(grid coordinates)
         * 
         * Point topLeft: Top left corner of the selection rectangle
         * 
         * Point bottomRight: Bottom right corner of the selection rectangle
         * 
         * bool addToHistory: Determines whether or not we will add this operation to the history
         */
        public ArrayList SelectEntities(Point firstPoint, Point secondPoint, bool addToHistory)
        {
            var min = new Point(Math.Min(firstPoint.X,secondPoint.X),
                                    Math.Min(firstPoint.Y,secondPoint.Y));

            var max = new Point(Math.Max(firstPoint.X, secondPoint.X)+1,
                                    Math.Max(firstPoint.Y, secondPoint.Y)+1);
            var diff = new Point(max.X - min.X, max.Y - min.Y);
            var selection = new Rectangle(min, new Size(diff));
            var selected = new ArrayList();

            //For every entity, check if it is within the selection bounds. 
                //If it is, select it, and add it to the selection list
            foreach(Entity entity in _mEntities)
            {               
                if (selection.IntersectsWith(new Rectangle(entity.Location, new Size(1,1))))
                {
                    selected.Add(entity);
                }
            }

            if (addToHistory)
            {
                _mHistory.Push(new SelectEntity(selected));
                _mUndoHistory.Clear();
            }

            return selected;
        }

        /*
         * Save
         * 
         * Saves the current level as an serialized XML Document into the folder
         * .../Levels/.  The name of the file is given by the name variable stored
         * in this level as <name>.xml.
         * 
         */
        public void Save()
        {
            var currentDirectory = "..\\..\\..\\..\\MrGravity\\Content\\Levels\\";
            var d = new DirectoryInfo(currentDirectory);

            var thumbnail = new DirectoryInfo(currentDirectory + "\\Thumbnail\\");

            if (!d.Exists)
                Directory.CreateDirectory(currentDirectory);

            if (!thumbnail.Exists)
                Directory.CreateDirectory(currentDirectory + "\\Thumbnail\\");

            if (Background.Tag == null)
            { MessageBox.Show("Failed to save \"" + Name + "\". Invalid background image."); return; }

            var entityTree = new XElement(XmlKeys.Entities);
            foreach (Entity entity in _mEntities)
            {
                if (entity.Type == "Walls")
                    CheckForInvalidWalls(entity);

                entityTree.Add(entity.Export());
            }
            var xDoc = new XDocument(
                new XElement(XmlKeys.Level,
                    new XElement(XmlKeys.LName, Name),
                    new XElement(XmlKeys.Size,
                        new XAttribute(XmlKeys.LX, Size.X),
                        new XAttribute(XmlKeys.LY, Size.Y)),
                    new XElement(XmlKeys.Background, Background.Tag.ToString()),
                    entityTree));

            xDoc.Save(currentDirectory + Name + XmlKeys.Xml);

            MessageBox.Show(Name + ".xml saved correctly");
        }

        private void CheckForInvalidWalls(Entity entity)
        {
            foreach (Entity ent in _mEntities)
            {
                if (ent.Equals(entity)) break;

                if (ent.Type == "Walls" && entity.Location.X == ent.Location.X && entity.Location.Y == ent.Location.Y)
                    MessageBox.Show("Warning! Level has overlapping walls at position: " + ent.Location.X + ", " + ent.Location.Y);
            }
        }
    }
}
