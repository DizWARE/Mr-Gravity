using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Xml;
using System.Xml.Linq;
using System.Drawing.Imaging;
using System.Windows.Forms;
using GravityLevelEditor.XML;

namespace GravityLevelEditor
{
    class Entity
    {
        public static int ObjectID = 0;
        private int mID = ObjectID++;
        public int ID { get { return mID; } }

        private string mName;
        public string Name { get { return mName; } set { mName = value; } }

        private string mType;
        public string Type { get { return mType; } set { mType = value; } }

        private string mCollisionType;
        public string CollisionType { get { return mCollisionType; } set { mCollisionType = value; } }

        private Point mLocation;
        public Point Location { get { return mLocation; } set { mLocation = value; } }

        private bool mPaintable;
        public bool Paintable { get { return mPaintable; } set { mPaintable = value; } }

        private Image mTexture;
        public Image Texture { get { return mTexture; } set { mTexture = value; } }

        private Dictionary<string, string> mProperties;
        public Dictionary<string, string> Properties { get { return mProperties; } set { mProperties = value; } } 

        /*
         * Entity
         * 
         * Constructor to create an exact copy of the given entity.
         * 
         * Entity original: Initial entity to be copied.
         */
        public Entity(Entity original)
        {
            mType = original.mType;
            mName = original.mName;
            mCollisionType = original.mCollisionType;
            mPaintable = original.mPaintable;
            mProperties = original.mProperties;
            mLocation = original.mLocation;
            mTexture = original.mTexture;
        }

        /*
         * Entity
         * 
         * Constructor that creates an entity from with the given properties
         * 
         * string type: type of entity (i.e. Wall, PlayerStart, etc).
         * string name: identifier for entity
         * string collisionType: type of collision for an entity.
         * bool paintable: whether or not the given entity is paintable.
         * Dictionary<string, string> properties: additional properties for this entity.
         * Image texture: image used to represent this entity in the level editor.
         */
        public Entity(string type, string name, string collisionType,
            bool paintable, Dictionary<string, string> properties, Image texture)
        {
            mType = type;
            mName = name;
            mCollisionType = collisionType;
            mLocation = new Point(-100, -100);
            mPaintable = paintable;
            mProperties = properties;
            mTexture = texture;
        }

        /*
         * Entity
         *
         * Constructor that creates an entity from a given Xelement.
         * 
         * XElement ent : the XML XElement that contains information for this entity.
         */
        public Entity(XElement ent)
        {
            string currentDirectory = "..\\..\\..\\..\\WindowsGame1\\Content\\Images";

            DirectoryInfo d = new DirectoryInfo(currentDirectory);

            mProperties = new Dictionary<string, string>();

            int maxID = ObjectID;

            foreach (XElement el in ent.Elements())
            {
                if (el.Name == XmlKeys.ID)
                {
                    mID = Convert.ToInt32(el.Value.ToString());

                    if (mID > maxID) maxID = mID;
                }
                if (el.Name == XmlKeys.E_NAME)
                {
                    mName = el.Value.ToString();
                }
                if (el.Name == XmlKeys.TYPE)
                {
                    mType = el.Value.ToString();
                }
                if (el.Name == XmlKeys.COLLISIONTYPE)
                {
                    mCollisionType = el.Value.ToString();
                }
                if (el.Name == XmlKeys.LOCATION)
                {
                    Point xLoc = new Point(Convert.ToInt32(el.Attribute(XmlKeys.E_X).Value.ToString()), Convert.ToInt32(el.Attribute(XmlKeys.E_Y).Value.ToString()));
                    mLocation = xLoc;
                }
                if (el.Name == XmlKeys.PAINTABLE)
                {
                    if (el.Value == XmlKeys.TRUE)
                    {
                        mPaintable = true;
                    }
                    else mPaintable = false;
                }
                if (el.Name == XmlKeys.TEXTURE)
                {
                    currentDirectory = d.FullName + "\\" + el.Value + XmlKeys.PNG;
                    try { mTexture = Image.FromFile(currentDirectory); mTexture.Tag = el.Value; }
                    catch (Exception ex) { MessageBox.Show("File " + currentDirectory + " could not be found."); }
                    
                }
                if (el.Name == XmlKeys.PROPERTIES)
                {
                    foreach (XElement property in el.Elements())
                    {
                        mProperties.Add(property.Name.ToString(), property.Value.ToString());
                    }
                }
            }

            mID = maxID;
            Entity.ObjectID = maxID;
        }

        /*
         * MoveEntity
         * 
         * Moves the entity to the given location.
         * 
         * Point where: location to move the entity to.
         */
        public void MoveEntity(Point where)
        {
            mLocation = where;
        }

        /*
         * Draw
         * 
         * Draws this entity in the editor.
         * 
         * Graphics g: the Graphics Device to draw to.
         * 
         * Point offset: the offset that the level editor is at.
         */
        public void Draw(Graphics g, Point offset)
        {
            Rectangle drawLocation = GridSpace.GetDrawingRegion(mLocation, offset);
            
            g.DrawImage(mTexture, drawLocation);
        }

        /*
         * Copy
         * 
         * Copies this entity, creating a clone with the same properties.
         * 
         * Return Value: copied entity.
         */
        public Entity Copy()
        {
            return new Entity(this);
        }

        /*
         * Equals
         * 
         * Checks to see if the given object is equal to this entity.
         * 
         * object obj: object we are comparing against.
         * 
         * Return Value: true if equal, false otherwise.
         */
        public override bool Equals(object obj)
        {
            return obj is Entity && ((Entity)obj).mID == mID && ((Entity)obj).ToString() == ToString();
        }

        /*
         * GetHashCode
         * 
         * Gets a hash representation of this object.
         * 
         * Return Value: An integer representing this objects hash code. 
         */
        public override int GetHashCode()
        {
            int hash = 0;
            foreach (char c in (mType + "/" + mName))
                hash += c * 71;

            return base.GetHashCode() + hash;
        }

        /*
         * ToString
         * 
         * Gets the string representation of this Entity.
         * 
         * Return Value: The string representation fo this entity.
         */
        public override string ToString()
        {
            return mType + "/" + mName;
        }

        //TODO - Import/Export methods

        /*
         * Export
         * 
         * Creates an XML XElement representation of this entity.
         *
         */
        public XElement Export()
        {

            if (mTexture.Tag == null)
            { MessageBox.Show("Failed to save " + ToString() + ID + ". Invalid image."); return null; }

            XElement propertiesTree = new XElement(XmlKeys.PROPERTIES);

            foreach (string key in mProperties.Keys)
                propertiesTree.Add(new XElement(key, mProperties[key]));

            XElement entityTree = new XElement(XmlKeys.ENTITY,
                new XElement(XmlKeys.ID, this.ID),
                new XElement(XmlKeys.E_NAME, this.Name),
                new XElement(XmlKeys.TYPE, this.Type),
                new XElement(XmlKeys.LOCATION,
                    new XAttribute(XmlKeys.E_X, this.Location.X),
                    new XAttribute(XmlKeys.E_Y, this.Location.Y)),
                new XElement(XmlKeys.COLLISIONTYPE, this.CollisionType),
                new XElement(XmlKeys.PAINTABLE, this.Paintable.ToString()),
                new XElement(XmlKeys.TEXTURE, this.mTexture.Tag.ToString()),
                propertiesTree);

            return entityTree;
        }
    }
}
