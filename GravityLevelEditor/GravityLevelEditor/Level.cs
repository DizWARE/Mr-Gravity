using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Drawing;
using System.Xml;
using System.Xml.Linq;
using System.Windows.Forms;
using System.Drawing.Imaging;
using GravityLevelEditor.XML;

namespace GravityLevelEditor
{
    class Level
    {
        private ArrayList mEntities;
        
        private ArrayList mClipboard;
        public ArrayList Clipboard { get { return mClipboard; } set { mClipboard = value; } }

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
            Entity.objectID = 0;
            mClipboard = new ArrayList();
            mName = name;
            mSize = size;
            mColor = color;
            mBackground = background;
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
            string currentDirectory = "..\\..\\..\\..\\WindowsGame1\\Content\\Images";
            DirectoryInfo d = new DirectoryInfo(currentDirectory);

            mEntities = new ArrayList();
            mClipboard = new ArrayList();

            Entity.objectID = 0;

            XElement xLevel = XElement.Load(filename);

            foreach (XElement el in xLevel.Elements())
            {
                if (el.Name == XmlKeys.L_NAME)
                {
                    this.Name = el.Value.ToString();
                }
                if (el.Name == XmlKeys.SIZE)
                {
                    Point xSize = new Point(Convert.ToInt32(el.Attribute(XmlKeys.L_X).Value.ToString()), Convert.ToInt32(el.Attribute(XmlKeys.L_Y).Value.ToString()));
                    this.Size = xSize;

                }
                if (el.Name == XmlKeys.BACKGROUND)
                {
                    try { this.Background = Image.FromFile(d.FullName + "\\" + el.Value + XmlKeys.PNG); }
                    catch (Exception ex) { this.Background = Image.FromFile(d.FullName + "\\errorBG" + XmlKeys.PNG); }
                    this.Background.Tag = el.Value;
                }
                if (el.Name == XmlKeys.COLOR)
                {
                    this.Color = Color.FromName(el.Name.ToString());
                }
                if (el.Name == XmlKeys.ENTITIES)
                {

                    mEntities.Clear();

                    foreach (XElement entity in el.Elements())
                    {
                        mEntities.Add(new Entity(entity));
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
                mUndoHistory.Clear();
                mHistory.Push(new AddEntity(entity, this));
            }

            mEntities.Add(entity);
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
                mUndoHistory.Clear();
                mHistory.Push(new AddEntity(entities, this));
            }
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
         * 
         * bool addToHistory: whether or not to add the operation to the undo history
         */
        public void RemoveEntity(ArrayList entities, bool addToHistory)
        {
            if(addToHistory)
            {
                mHistory.Push(new RemoveEntity(entities, this));
                mUndoHistory.Clear();
            }
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
         * 
         * bool addToHistory: whether or not to add the operation to the undo history
         */
        public void MoveEntity(ArrayList entities, Size offset, bool addToHistory)
        {
            if (addToHistory)
            {
                mUndoHistory.Clear();
                mHistory.Push(new MoveEntity(entities, offset));
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
            mSize.X = cols;
            mSize.Y = rows;
        }

        /*
         * Redo
         * 
         * Redo the last undone operation (if any).
         */
        public void Redo()
        {
            if (mUndoHistory.Count > 0)
            {
                IOperation operation = mUndoHistory.Pop();
                operation.Redo();
                mHistory.Push(operation);
            }
        }

        /*
         * Undo
         * 
         * Undo the last operation to the level.
         */
        public void Undo()
        {
            if (mHistory.Count > 0)
            {
                IOperation operation = mHistory.Pop();
                operation.Undo();
                mUndoHistory.Push(operation);
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
            if (entities.Count == 0) return;

            RemoveEntity(entities, false);
            Copy(entities);

            mUndoHistory.Clear();
            mHistory.Push(new Cut(entities, this));
        }

        /*
         * Paste
         * 
         * Pastes the entities on the screen. Currently, they drop down starting at the upper left corner
         */
        public ArrayList Paste()
        {
            Point minPoint = new Point(1,1);

            foreach (Entity entity in mClipboard)
                entity.Location = Point.Add(entity.Location, new Size(minPoint));

            AddEntities(mClipboard, true);
            return mClipboard;
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
            ArrayList inTile = InTile(gridLocation);
            if (inTile.Count > 0)
            {
                Entity selectedEntity = (Entity)inTile[0];
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
            g.DrawImage(mBackground, new Rectangle(offset,
                new Size(GridSpace.GetDrawingCoord(this.mSize))));

            foreach (Entity entity in mEntities)
                entity.Draw(g, offset);
        }

        /// <summary>
        /// Draws the level in its unzoomed location
        /// </summary>
        /// <param name="g">Graphics to draw to</param>
        public void Draw(Graphics g)
        {
            g.DrawImage(mBackground, new Rectangle(new Point(),
                new Size(GridSpace.GetPixelCoord(this.mSize))));

            foreach (Entity entity in mEntities)
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
            Point min = new Point(Math.Min(firstPoint.X,secondPoint.X),
                                    Math.Min(firstPoint.Y,secondPoint.Y));

            Point max = new Point(Math.Max(firstPoint.X, secondPoint.X)+1,
                                    Math.Max(firstPoint.Y, secondPoint.Y)+1);
            Point diff = new Point(max.X - min.X, max.Y - min.Y);
            Rectangle selection = new Rectangle(min, new Size(diff));
            ArrayList selected = new ArrayList();

            //For every entity, check if it is within the selection bounds. 
                //If it is, select it, and add it to the selection list
            foreach(Entity entity in mEntities)
            {               
                if (selection.IntersectsWith(new Rectangle(entity.Location, new Size(1,1))))
                {
                    selected.Add(entity);
                }
            }

            if (addToHistory)
            {
                mHistory.Push(new SelectEntity(selected));
                mUndoHistory.Clear();
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
        public void Save(Bitmap image)
        {
            string currentDirectory = "..\\..\\..\\..\\WindowsGame1\\Content\\Levels\\";
            DirectoryInfo d = new DirectoryInfo(currentDirectory);

            DirectoryInfo thumbnail = new DirectoryInfo(currentDirectory + "\\Thumbnail\\");

            if (!d.Exists)
                System.IO.Directory.CreateDirectory(currentDirectory);

            if (!thumbnail.Exists)
                System.IO.Directory.CreateDirectory(currentDirectory + "\\Thumbnail\\");

            if (mBackground.Tag == null)
            { MessageBox.Show("Failed to save \"" + Name + "\". Invalid background image."); return; }

            XElement entityTree = new XElement(XmlKeys.ENTITIES);
            foreach (Entity entity in mEntities)
            {
                if (entity.Type == "Walls")
                    CheckForInvalidWalls(entity);

                entityTree.Add(entity.Export());
            }
            XDocument xDoc = new XDocument(
                new XElement(XmlKeys.LEVEL,
                    new XElement(XmlKeys.L_NAME, this.Name),
                    new XElement(XmlKeys.SIZE,
                        new XAttribute(XmlKeys.L_X, this.Size.X),
                        new XAttribute(XmlKeys.L_Y, this.Size.Y)),
                    new XElement(XmlKeys.BACKGROUND, this.Background.Tag.ToString()),
                    new XElement(XmlKeys.COLOR, this.Color.ToString()),
                    entityTree));

            xDoc.Save(currentDirectory + this.Name + XmlKeys.XML);


            image.Save(currentDirectory + "\\Thumbnail\\" + this.Name + ".png");
            image.Dispose();
            MessageBox.Show(this.Name + ".xml saved correctly");
        }

        void CheckForInvalidWalls(Entity entity)
        {
            foreach (Entity ent in mEntities)
            {
                if (ent.Equals(entity)) break;

                if (ent.Type == "Walls" && entity.Location.X == ent.Location.X && entity.Location.Y == ent.Location.Y)
                    MessageBox.Show("Warning! Level has overlapping walls at position: " + ent.Location.X.ToString() + ", " + ent.Location.Y.ToString());
            }
        }
    }
}
