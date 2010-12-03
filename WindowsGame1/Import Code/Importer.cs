using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using System.IO;
using System.Collections;
using System.Xml.Linq;
using GravityLevelEditor;
using GravityShift.Game_Objects.Static_Objects.Triggers;

namespace GravityShift.Import_Code
{
    /// <summary>
    /// Imports an XML file into the game
    /// </summary>
    class Importer
    {
        ContentManager mContent;
        List<EntityInfo> mEntities;

        /// <summary>
        /// Constructor for an importer object
        /// </summary>
        /// <param name="content">Content Manager</param>
        public Importer(ContentManager content)
        {
            mContent = content;
            mEntities = new List<EntityInfo>();
        }

        /// <summary>
        /// Goes through the xml file and translates the information
        /// </summary>
        /// <param name="filename">Name of the file we are importing</param>
        /// <returns>A level object</returns>
        public Level ImportLevel(string filename)
        {
            XElement xLevel = XElement.Load(filename);

            Level level = new Level();

            //Gets all the information for a level and places it into the level object
            foreach (XElement item in xLevel.Elements())
            {
                if (item.Name == XmlKeys.NAME)
                    level.Name = item.Value;
                if (item.Name == XmlKeys.SIZE)
                    level.Size = GridSpace.GetDrawingCoord(new Vector2(int.Parse(item.Attribute(XName.Get("X", "")).Value),
                        int.Parse(item.Attribute(XName.Get("Y", "")).Value)));
                if (item.Name == XmlKeys.BACKGROUND)
                    level.Load(mContent, item.Value);
                if (item.Name == "Entities")
                    foreach (XElement element in item.Elements())
                        mEntities.Add(new EntityInfo(element));
            }

            level.StartingPoint = GetPlayerStart();

            return level;
        }

        /// <summary>
        /// Gets the players start position
        /// </summary>
        /// <returns>A vector2 with the players start position(or -100,-100 if none is provided)</returns>
        public Vector2 GetPlayerStart()
        {
            foreach (EntityInfo entity in mEntities)
                if (entity.mType == XmlKeys.PLAYER_LOCATION && entity.mName == XmlKeys.PLAYER_START)
                    return GridSpace.GetDrawingCoord(entity.mLocation);

            return new Vector2(-100, -100);
        } 

        /// <summary>
        /// Creates and returns a PlayerEnd using the info that is in the xml file
        /// </summary>
        /// <returns>An object that represents the end of a level</returns>
        public PlayerEnd GetPlayerEnd()
        {
            foreach (EntityInfo entity in mEntities)
                if (entity.mType == XmlKeys.PLAYER_LOCATION && entity.mName == XmlKeys.PLAYER_END)
                    return new PlayerEnd(mContent, "Images\\" + entity.mTextureFile, GridSpace.GetDrawingCoord(entity.mLocation));

            return new PlayerEnd(mContent, "Images\\Level Positions\\player_end", Vector2.Add(GetPlayerStart(),GridSpace.SIZE));
        }

        /// <summary>
        /// Goes through all the entities and finds all of the ones that are Static, or physics(Dynamic maybe in the future)
        /// Creates and returns them using the data found in the xml file
        /// </summary>
        /// <param name="environment">Environment that these items exist in</param>
        /// <returns>A list of objects that are in the game</returns>
        public List<GameObject> GetObjects(ref PhysicsEnvironment environment)
        {
            List<GameObject> objects = new List<GameObject>();
            foreach (EntityInfo entity in mEntities)
            {
                //If the object is static, make a static object
                if (entity.mType == XmlKeys.STATIC_OBJECT)
                {
                    bool isSquare = entity.mProperties.Count == 0 || entity.mProperties["Shape"] == "Square";
                    Tile tile = new Tile(mContent, "Images\\" + entity.mTextureFile,
                        GridSpace.GetDrawingCoord(entity.mLocation), .8f, isSquare, entity.mCollisionType);
                    tile.ID = entity.mId;
                    objects.Add(tile);

                }
                //If the object is physics, make a physics object
                if (entity.mType == XmlKeys.PHYSICS_OBJECT)
                {
                    bool isSquare = entity.mProperties.ContainsKey("Shape") && entity.mProperties["Shape"] == "Square";
                    MovingTile tile = new MovingTile(mContent, "Images\\" + entity.mTextureFile, new Vector2(1, 1),
                        GridSpace.GetDrawingCoord(entity.mLocation), ref environment, .8f, isSquare, entity.mCollisionType);
                    tile.ID = entity.mId;
                    objects.Add(tile);
                }
            }

            return objects;
        }

        /// <summary>
        /// UNUSED FOR NOW. MAY IN THE FUTURE, GATHER ALL THE INFO UP AND MAKE BIGGER BOUNDING BOXES FOR ENTIRE WALLS
        /// </summary>
        /// <returns></returns>
        public List<Tile> GetWalls()
        {
            return null;
        }

        /// <summary>
        /// Gets all the triggers in the level
        /// </summary>
        /// <returns>All the triggers in the xml file</returns>
        public List<Trigger> GetTriggers()
        {
            List<Trigger> triggers = new List<Trigger>();
            foreach (EntityInfo entity in mEntities)
            {
                if (entity.mType == XmlKeys.TRIGGER)
                {
                    bool isSquare = entity.mProperties.ContainsKey(XmlKeys.SHAPE) && entity.mProperties[XmlKeys.SHAPE] == XmlKeys.SQUARE;

                    if(!entity.mProperties.ContainsKey(XmlKeys.WIDTH) || !entity.mProperties.ContainsKey(XmlKeys.HEIGHT))
                        continue;

                    if(entity.mName == "Basic")
                        triggers.Add(new BasicTrigger(mContent,entity.mName,
                            entity.mLocation,isSquare,int.Parse(entity.mProperties[XmlKeys.WIDTH]),int.Parse(entity.mProperties[XmlKeys.HEIGHT])));
                    //Add trigger by name
                }
            }
            return triggers;
        }
    }
}
