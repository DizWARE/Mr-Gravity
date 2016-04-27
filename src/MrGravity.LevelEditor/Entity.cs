using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml.Linq;
using MrGravity.LevelEditor.XML;

namespace MrGravity.LevelEditor
{
    internal class Entity
    {
        public static int ObjectId;
        public int Id { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        public string CollisionType { get; set; }

        public Point Location { get; set; }

        public bool Paintable { get; set; }

        public Image Texture { get; set; }

        public Dictionary<string, string> Properties { get; set; }

        /*
         * Entity
         * 
         * Constructor to create an exact copy of the given entity.
         * 
         * Entity original: Initial entity to be copied.
         */
        public Entity(Entity original)
        {
            Type = original.Type;
            Name = original.Name;
            Id = ObjectId++;
            CollisionType = original.CollisionType;
            Paintable = original.Paintable;
            Properties = original.Properties;
            Location = original.Location;
            Texture = original.Texture;
        }

        /*
         * Entity
         * 
         * Constructor that creates an entity from with the given properties
         * 
         * string type: type of entity (i.e. Wall, PlayerStart, etc).
         * string name: identifier for entity
         * int objectID: ID of the entity
         * string collisionType: type of collision for an entity.
         * bool paintable: whether or not the given entity is paintable.
         * Dictionary<string, string> properties: additional properties for this entity.
         * Image texture: image used to represent this entity in the level editor.
         */
        public Entity(string type, string name, int objectId, string collisionType,
            bool paintable, Dictionary<string, string> properties, Image texture)
        {
            Type = type;
            Name = name;
            Id = objectId++;
            CollisionType = collisionType;
            Location = new Point(-100, -100);
            Paintable = paintable;
            Properties = properties;
            Texture = texture;
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
            var currentDirectory = "..\\..\\..\\..\\MrGravity\\Content\\Images";

            var d = new DirectoryInfo(currentDirectory);

            Properties = new Dictionary<string, string>();

            foreach (var el in ent.Elements())
            {
                if (el.Name == XmlKeys.Id)
                {
                    Id = Convert.ToInt32(el.Value);
                    if (Id < ObjectId)
                        Id = ObjectId+1;
                    
                    ObjectId = Id+1;
                }
                if (el.Name == XmlKeys.EName)
                {
                    Name = el.Value;
                }
                if (el.Name == XmlKeys.Type)
                {
                    Type = el.Value;
                }
                if (el.Name == XmlKeys.Collisiontype)
                {
                    CollisionType = el.Value;
                }
                if (el.Name == XmlKeys.Location)
                {
                    var xLoc = new Point(Convert.ToInt32(el.Attribute(XmlKeys.EX).Value), Convert.ToInt32(el.Attribute(XmlKeys.EY).Value));
                    Location = xLoc;
                }
                if (el.Name == XmlKeys.Paintable)
                {
                    if (el.Value == XmlKeys.True)
                    {
                        Paintable = true;
                    }
                    else Paintable = false;
                }
                if (el.Name == XmlKeys.Texture)
                {
                    currentDirectory = d.FullName + "\\" + el.Value + XmlKeys.Png;
                    try { Texture = Image.FromFile(currentDirectory); Texture.Tag = el.Value; }
                    catch (Exception) { MessageBox.Show("File " + currentDirectory + " could not be found."); }
                    
                }
                if (el.Name == XmlKeys.Properties)
                {
                    foreach (var property in el.Elements())
                    {
                        Properties.Add(property.Name.ToString(), property.Value);
                    }
                }
            }
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
            Location = where;
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
            var drawLocation = GridSpace.GetDrawingRegion(Location, offset);
            
            g.DrawImage(Texture, drawLocation);
        }

        /// <summary>
        /// Draws this entity in a non-zoomed location
        /// </summary>
        /// <param name="g">Graphics object to draw to</param>
        public void Draw(Graphics g)
        {
            var drawLocation = new Rectangle(GridSpace.GetPixelCoord(Location), new Size(GridSpace.Size));

            g.DrawImage(Texture, drawLocation);
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
            return obj is Entity && ((Entity)obj).Id == Id && ((Entity)obj).ToString() == ToString();
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
            var hash = 0;
            foreach (var c in (Type + "/" + Name))
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
            return Type + "/" + Name;
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
            if (Texture == null || Texture.Tag == null)
            { MessageBox.Show("Failed to save " + ToString() + Id + ". Invalid image."); return null; }

            var propertiesTree = new XElement(XmlKeys.Properties);

            foreach (var key in Properties.Keys)
                propertiesTree.Add(new XElement(key, Properties[key]));

            var entityTree = new XElement(XmlKeys.Entity,
                new XElement(XmlKeys.Id, Id),
                new XElement(XmlKeys.EName, Name),
                new XElement(XmlKeys.Type, Type),
                new XElement(XmlKeys.Location,
                    new XAttribute(XmlKeys.EX, Location.X),
                    new XAttribute(XmlKeys.EY, Location.Y)),
                new XElement(XmlKeys.Collisiontype, CollisionType),
                new XElement(XmlKeys.Paintable, Paintable.ToString()),
                new XElement(XmlKeys.Texture, Texture.Tag.ToString()),
                propertiesTree);

            return entityTree;
        }
    }
}
