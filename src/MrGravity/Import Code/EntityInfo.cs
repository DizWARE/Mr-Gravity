using System.Collections.Generic;
using System.Xml.Linq;
using Microsoft.Xna.Framework;

namespace MrGravity.Import_Code
{
    /// <summary>
    /// This class represents the inbetween stage of XML and translated GameObject
    /// </summary>
    internal class EntityInfo
    {
        public int MId;

        public string MName;
        public string MType;
        public string MCollisionType;
        public string MTextureFile;

        public bool MTrigger;

        public Vector2 MLocation;

        public Dictionary<string, string> MProperties;

        /// <summary>
        /// Creates an entity out of an XElement that defiens an entity
        /// </summary>
        /// <param name="entity">The XML chunck that defines this entity</param>
        public EntityInfo(XElement entity)
        {
            MProperties = new Dictionary<string,string>();
            foreach(var item in entity.Elements())
            {
                if (item.Name == XmlKeys.Id)
                    MId = int.Parse(item.Value);
                if (item.Name == XmlKeys.Name)
                    MName = item.Value;
                if (item.Name == XmlKeys.Type)
                    MType = item.Value;
                if (item.Name == XmlKeys.CollisionType)
                    MCollisionType = item.Value;
                if (item.Name == XmlKeys.Texture)
                    MTextureFile = item.Value;
                if (item.Name == XmlKeys.Trigger)
                    MTrigger = XmlKeys.True.Equals(item.Value);
                if (item.Name == XmlKeys.Location)
                    MLocation = new Vector2(int.Parse(item.Attribute(XName.Get("X", "")).Value),
                        int.Parse(item.Attribute(XName.Get("Y", "")).Value));
                if (item.Name == XmlKeys.Properties)
                    foreach (var property in item.Elements())
                        MProperties.Add(property.Name.ToString(), property.Value);
            }
        }

        private EntityInfo(string name, Vector2 startLocation)
        {
            MId = -1;

            MName = name;
            MType = "";
            MCollisionType = "Normal";
            MTextureFile = name;
            MLocation = startLocation;
            MProperties = new Dictionary<string, string>();
        }

        public static EntityInfo CreatePlayerEndInfo(Vector2 startLocation)
        {
            var playerEnd = new EntityInfo("PlayerEnd", startLocation);
            playerEnd.MProperties.Add("Shape", "Circle");
            playerEnd.MId = -2;
            return playerEnd;
        }

        public static EntityInfo CreatePlayerInfo(Vector2 startLocation)
        {
            var player = new EntityInfo("Player", startLocation);
            player.MProperties.Add("Shape", "Circle");
            player.MProperties.Add("Mass", "1");
            player.MId = -1;
            return player;
        }

        public static EntityInfo CreateWallInfo()
        {
            var info = new EntityInfo("", new Vector2());
            info.MProperties.Add("Shape", "Square");
            return info;
        }

    }
}
