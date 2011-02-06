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
using GravityShift.MISC_Code;
using GravityShift.Game_Objects.Static_Objects.Triggers;
using GravityShift.Game_Objects.Static_Objects;

namespace GravityShift.Import_Code
{
    /// <summary>
    /// Imports an XML file into the game
    /// </summary>
    class Importer
    {
        ContentManager mContent;
        List<EntityInfo> mEntities;
        List<EntityInfo> mRails;

        /// <summary>
        /// Constructor for an importer object
        /// </summary>
        /// <param name="content">Content Manager</param>
        public Importer(ContentManager content)
        {
            mContent = content;
            mEntities = new List<EntityInfo>();
            mRails = new List<EntityInfo>();
        }

        /// <summary>
        /// Goes through the xml file and translates the information
        /// </summary>
        /// <param name="filename">Name of the file we are importing</param>
        /// <returns>A level object</returns>
        public Level ImportLevel(Level level)
        {

            XElement xLevel;
#if XBOX360
            xLevel = XElement.Load("Content\\Levels\\" + level.Name + ".xml");
#else
            xLevel = XElement.Load("..\\..\\..\\Content\\Levels\\" + level.Name + ".xml");
#endif
            

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

            GetPlayerStart(level);

            return level;
        }

        /// <summary>
        /// Gets the players start position
        /// </summary>
        /// <returns>A vector2 with the players start position(or -100,-100 if none is provided)</returns>
        public void GetPlayerStart(Level level)
        {
            foreach (EntityInfo entity in mEntities)
            {
                if (entity.mType == XmlKeys.PLAYER_LOCATION && entity.mName == XmlKeys.PLAYER_START)
                {
                    level.StartingPoint = GridSpace.GetDrawingCoord(entity.mLocation);
                    if (entity.mProperties.ContainsKey(XmlKeys.IDEAL_TIME))
                        level.IdealTime = int.Parse(entity.mProperties[XmlKeys.IDEAL_TIME]);
                    else
                        level.IdealTime = 400;
                }
                if (entity.mType == XmlKeys.STATIC_OBJECT && entity.mCollisionType == XmlKeys.COLLECTABLE)
                {
                    level.CollectableCount++;
                }
            }
        } 

        /// <summary>
        /// Creates and returns a PlayerEnd using the info that is in the xml file
        /// </summary>
        /// <returns>An object that represents the end of a level</returns>
        public PlayerEnd GetPlayerEnd()
        {
            foreach (EntityInfo entity in mEntities)
                if (entity.mType == XmlKeys.PLAYER_LOCATION && entity.mName == XmlKeys.PLAYER_END)
                    return new PlayerEnd(mContent, entity);

            //Need to fix ---- TODODODODODODODO
            return null;
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
                    Tile tile = new Tile(mContent, .8f, entity);
                    objects.Add(tile);

                }
                //If the object is physics, make a physics object
                if (entity.mType == XmlKeys.PHYSICS_OBJECT)
                {
                    if (entity.mProperties.ContainsKey("Rail") && entity.mProperties.ContainsKey("Length"))
                        mRails.Add(entity);

                    bool isSquare = entity.mProperties.ContainsKey("Shape") && entity.mProperties["Shape"] == "Square";
                    float mass = 1;
                    if (entity.mProperties.ContainsKey("Mass"))
                    {
                        mass = (float)Convert.ToDouble(entity.mProperties["Mass"]);
                    }
                    if (entity.mProperties.ContainsKey(XmlKeys.REVERSE))
                    {
                        ReverseTile rTile = new ReverseTile(mContent, ref environment, 0.8f, entity);
                        rTile.Mass = mass;
                        objects.Add(rTile);
                    }
                    else
                    {
                        MovingTile mTile = new MovingTile(mContent, ref environment, 0.8f, entity);
                        mTile.Mass = mass;
                        objects.Add(mTile);
                    }
                }
            }

            return objects;
        }

        /// <summary>
        /// UNUSED FOR NOW. MAY IN THE FUTURE, GATHER ALL THE INFO UP AND MAKE BIGGER BOUNDING BOXES FOR ENTIRE WALLS
        /// </summary>
        /// <returns></returns>
        public List<StaticObject> GetWalls(Level level)
        {
            Vector2 gridSize = GridSpace.GetGridCoord(level.Size);

            List<StaticObject> final = new List<StaticObject>();

            List<StaticObject> walls = new List<StaticObject>();
            List<StaticObject>[] rows = new List<StaticObject>[(int)gridSize.Y];
            List<StaticObject>[] cols = new List<StaticObject>[(int)gridSize.X];
            foreach (EntityInfo entity in mEntities)
                if (entity.mType == XmlKeys.WALLS)
                    walls.Add(new Tile(mContent, .8f, entity));

            CreateSortedIndicies(walls, rows, false);
            for(int i = 0; i < rows.Length; i++)
            {
                int next = -1;
                string collisionType = "";
                List<StaticObject> wall = new List<StaticObject>();
                foreach (StaticObject obj in rows[i])
                {
                    Vector2 gridLoc = GridSpace.GetGridCoord(obj.mPosition);
                    if ((gridLoc.X != next || !collisionType.Equals(obj.CollisionType)) && wall.Count > 0)
                    {
                        if (wall.Count > 1)
                            final.Add(new Wall(.8f, wall));
                        else
                            walls.Add(wall[0]);

                        next = -1;
                        collisionType = "";
                        wall.Clear();
                    }

                    if(wall.Count == 0)
                    {
                        walls.Remove(obj);
                        wall.Add(obj);
                        next = (int)gridLoc.X + 1;
                        collisionType = obj.CollisionType;
                    }
                    else if (gridLoc.X == next && collisionType.Equals(obj.CollisionType))
                    {
                        walls.Remove(obj);
                        wall.Add(obj);
                        next++;
                    }
                }
                if (wall.Count > 0)
                {
                    if (wall.Count > 1)
                        final.Add(new Wall(.8f, wall));
                    else
                        walls.Add(wall[0]);

                    wall.Clear();
                }
            }

            CreateSortedIndicies(walls, cols,true);
            for (int i = 0; i < cols.Length; i++)
            {
                int next = -1;
                string collisionType = "";
                List<StaticObject> wall = new List<StaticObject>();
                foreach (StaticObject obj in cols[i])
                {
                    Vector2 gridLoc = GridSpace.GetGridCoord(obj.mPosition);
                    if ((gridLoc.Y != next || !collisionType.Equals(obj.CollisionType)) && wall.Count > 0)
                    {
                        if (wall.Count > 1)
                            final.Add(new Wall(.8f, wall));
                        else
                            walls.Add(wall[0]);

                        next = -1;
                        wall.Clear();
                    }

                    if (wall.Count == 0)
                    {
                        walls.Remove(obj);
                        wall.Add(obj);
                        next = (int)gridLoc.Y + 1;
                        collisionType = obj.CollisionType;
                    }
                    else if (gridLoc.Y == next && collisionType.Equals(obj.CollisionType))
                    {
                        walls.Remove(obj);
                        wall.Add(obj);
                        next++;
                    }
                }

                if (wall.Count > 0)
                {
                    if (wall.Count > 1)
                        final.Add(new Wall(.8f, wall));
                    else
                        walls.Add(wall[0]);

                    wall.Clear();
                }
            }

            final.AddRange(walls);

            return final;
        }

        /// <summary>
        /// Creates the sorted indicies.
        /// </summary>
        /// <param name="walls">The walls.</param>
        /// <param name="indicies">The indicies.</param>
        private void CreateSortedIndicies(List<StaticObject> walls, List<StaticObject>[] indicies, bool cols)
        {
            for (int i = 0; i < indicies.Length; i++) indicies[i] = new List<StaticObject>();
            foreach (StaticObject obj in walls)
            {
                int i = 0;
                Vector2 gridLoc = GridSpace.GetGridCoord(obj.mPosition);
                int index = (int)gridLoc.Y;
                float compNum = gridLoc.X;

                if (cols) { index = (int)gridLoc.X; compNum = gridLoc.Y; }
                for (; i < indicies[index].Count; i++)
                {
                    Vector2 otherGridLoc = GridSpace.GetGridCoord(indicies[index][i].mPosition);

                    float otherComp = otherGridLoc.X;
                    if (cols) otherComp = otherGridLoc.Y;
                    

                    if (compNum < otherComp)
                        break;
                }

                if (i < indicies[index].Count)
                    indicies[index].Insert(i, obj);
                else
                    indicies[index].Add(obj);
            }
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

                    if (entity.mName == "Force")
                    {
                        ForceTrigger trigger = new ForceTrigger(mContent, entity);
                        triggers.Add(trigger);
                    }
                    else if (entity.mName == "Music")
                    {
                        MusicTrigger trigger = new MusicTrigger(mContent, entity);
                        triggers.Add(trigger);
                    }
                    else if (entity.mName == "SFX")
                    {
                        FXTrigger trigger = new FXTrigger(mContent, entity);
                        triggers.Add(trigger);
                    }
                    else if (entity.mName == "BlackHole")
                    {
                        BlackHoleTrigger trigger = new BlackHoleTrigger(mContent, entity);
                        triggers.Add(trigger);
                    }
                    else if (entity.mName == "PlayerFace")
                    {
                        PlayerFaceTrigger trigger = new PlayerFaceTrigger(mContent, entity);
                        triggers.Add(trigger);
                    }
                    //Add trigger by name
                }
            }
            return triggers;
        }

        public List<EntityInfo> GetRails()
        {
            return mRails;
        }
    }
}
