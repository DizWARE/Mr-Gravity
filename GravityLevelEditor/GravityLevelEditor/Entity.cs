using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace GravityLevelEditor
{
    class Entity
    {
        static int ObjectID = 0;
        private int mID = ObjectID++;
        public int ID { get { return mID; } }

        private string mType;
        public string Type { get {return mType;}  }

        private Point mLocation;
        public Point Location { get { return mLocation; } set { mLocation = value; } }
        
        private bool mVisible;
        public bool Visible { get { return mVisible; } }

        private bool mPaintable;
        public bool Paintable { get { return mPaintable; } }

        private bool mSelected = false;
        public bool Selected { get { return mSelected; } }

        private Image mTexture;

        private Dictionary<string, string> mProperties;

        /// <summary>
        /// Creates an exact copy(different ID) of the given entity
        /// </summary>
        /// <param name="original">Initial entity to be copied</param>
        public Entity(Entity original)
        {
            mType = original.mType;
            mVisible = original.mVisible;
            mPaintable = original.mPaintable;
            mProperties = original.mProperties;
            mLocation = new Point(-100, -100);
            mTexture = original.mTexture;
        }

        /// <summary>
        /// Creates an Entity from scratch with the given properties
        /// </summary>
        /// <param name="type">Type of Entitiy(i.e. Wall, PlayerStart, etc)</param>
        /// <param name="visibility">If false, entity will not be visible in the game</param>
        /// <param name="paintable">Whether or not the entity is allowed to be painted across tiles</param>
        /// <param name="properties">Additional properties for this entity</param>
        /// <param name="texture">The image used to represent this entity in the level editor</param>
        public Entity(string type, bool visibility, 
            bool paintable, Dictionary<string, string> properties, Image texture)
        {
            mType = type;
            mLocation = new Point(-100, -100);
            mVisible = visibility;
            mPaintable = paintable;
            mProperties = properties;
            mTexture = texture;

            //Export to Entity List
        }

        //TODO - Constructor from Xml file

        /// <summary>
        /// Moves the entity to the given location
        /// </summary>
        /// <param name="where">Location to move the entity to</param>
        public void MoveEntity(Point where)
        {
            mLocation = where;
        }
        
        /// <summary>
        /// Selects or deselects this entity
        /// </summary>
        public void ToggleSelect()
        {
            mSelected = !mSelected;
        }

        /// <summary>
        /// Marks this entity as visible or invisible for in-game play
        /// </summary>
        public void ToggleVisibility()
        {
            mVisible = !mVisible;
        }

        /// <summary>
        /// Draws this entity on screen
        /// </summary>
        /// <param name="g">The Graphics Device to draw to</param>
        public void Draw(Graphics g)
        {
            //This won't work with grid space without being scaled to pixel format TODO - Fix it
            g.DrawImage(mTexture, mLocation);
            
            //Draw selected outline here
        }

        /// <summary>
        /// Copies this entity
        /// </summary>
        /// <returns>An exact copy of this object(IDs will be different)</returns>
        public Entity Copy()
        {
            return new Entity(this);
        }

        /// <summary>
        /// Checks to see if the given object is equal to this entity
        /// </summary>
        /// <param name="obj">Object we are comparing to this</param>
        /// <returns>True if they are equal, false otherwise</returns>
        public override bool Equals(object obj)
        {
            return obj is Entity && ((Entity)obj).mID == mID;
        }

        //TODO - Import/Export methods
    }
}
