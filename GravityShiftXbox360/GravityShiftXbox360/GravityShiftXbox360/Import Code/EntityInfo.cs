using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Microsoft.Xna.Framework;

namespace GravityShift.Import_Code
{
    /// <summary>
    /// This class represents the inbetween stage of XML and translated GameObject
    /// </summary>
    class EntityInfo
    {
        public int mId;

        public string mName;
        public string mType;
        public string mCollisionType;
        public string mTextureFile;

        public bool mTrigger;

        public Vector2 mLocation = new Vector2();

        public Dictionary<string, string> mProperties;

        /// <summary>
        /// Creates an entity out of an XElement that defiens an entity
        /// </summary>
        /// <param name="entity">The XML chunck that defines this entity</param>
        public EntityInfo(XElement entity)
        {
            mProperties = new Dictionary<string,string>();
            foreach(XElement item in entity.Elements())
            {
                if (item.Name == XmlKeys.ID)
                    mId = int.Parse(item.Value);
                if (item.Name == XmlKeys.NAME)
                    mName = item.Value;
                if (item.Name == XmlKeys.TYPE)
                    mType = item.Value;
                if (item.Name == XmlKeys.COLLISION_TYPE)
                    mCollisionType = item.Value;
                if (item.Name == XmlKeys.TEXTURE)
                    mTextureFile = item.Value;
                if (item.Name == XmlKeys.TRIGGER)
                    mTrigger = XmlKeys.TRUE.Equals(item.Value);
                if (item.Name == XmlKeys.LOCATION)
                    mLocation = new Vector2(int.Parse(item.Attribute(XName.Get("X", "")).Value),
                        int.Parse(item.Attribute(XName.Get("Y", "")).Value));
                if (item.Name == XmlKeys.PROPERTIES)
                    foreach (XElement property in item.Elements())
                        mProperties.Add(property.Name.ToString(), property.Value);
            }
        }

        private EntityInfo(string name, Vector2 startLocation)
        {
            mId = -1;

            mName = name;
            mType = "";
            mCollisionType = "Normal";
            mTextureFile = name;
            mLocation = startLocation;
            mProperties = new Dictionary<string, string>();
        }

        public static EntityInfo CreatePlayerEndInfo(Vector2 startLocation)
        {
            EntityInfo playerEnd = new EntityInfo("PlayerEnd", startLocation);
            playerEnd.mProperties.Add("Shape", "Circle");
            playerEnd.mId = -2;
            return playerEnd;
        }

        public static EntityInfo CreatePlayerInfo(Vector2 startLocation)
        {
            EntityInfo player = new EntityInfo("Player", startLocation);
            player.mProperties.Add("Shape", "Circle");
            player.mProperties.Add("Mass", "1");
            player.mId = -1;
            return player;
        }

        public static EntityInfo CreateWallInfo()
        {
            EntityInfo info = new EntityInfo("", new Vector2());
            info.mProperties.Add("Shape", "Square");
            return info;
        }

    }
}
