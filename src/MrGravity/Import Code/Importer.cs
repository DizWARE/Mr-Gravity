using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using MrGravity.Game_Objects;
using MrGravity.Game_Objects.Physics_Objects;
using MrGravity.Game_Objects.Static_Objects;
using MrGravity.Game_Objects.Static_Objects.Triggers;
using MrGravity.MISC_Code;

namespace MrGravity.Import_Code
{
    /// <summary>
    /// Imports an XML file into the game
    /// </summary>
    internal class Importer
    {
        private readonly ContentManager _mContent;
        private readonly List<EntityInfo> _mEntities;
        private readonly List<EntityInfo> _mRails;

        /// <summary>
        /// Constructor for an importer object
        /// </summary>
        /// <param name="content">Content Manager</param>
        public Importer(ContentManager content)
        {
            _mContent = content;
            _mEntities = new List<EntityInfo>();
            _mRails = new List<EntityInfo>();
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
            foreach (var item in xLevel.Elements())
            {
                if (item.Name == XmlKeys.Name)
                    level.Name = item.Value;
                if (item.Name == XmlKeys.Size && !level.IsMainMenu)
                    level.Size = GridSpace.GetDrawingCoord(new Vector2(int.Parse(item.Attribute(XName.Get("X", "")).Value),
                        int.Parse(item.Attribute(XName.Get("Y", "")).Value)));
                if (item.Name == XmlKeys.Background)
                    level.Load(_mContent, item.Value);
                if (item.Name == "Entities")
                    foreach (var element in item.Elements())
                        _mEntities.Add(new EntityInfo(element));
            }

            if(!level.IsMainMenu)
                GetPlayerStart(level);

            return level;
        }

        /// <summary>
        /// Gets the players start position
        /// </summary>
        /// <returns>A vector2 with the players start position(or -100,-100 if none is provided)</returns>
        public void GetPlayerStart(Level level)
        {
            level.CollectableCount = 0;
            foreach (var entity in _mEntities)
            {
                if (entity.MType == XmlKeys.PlayerLocation && entity.MName == XmlKeys.PlayerStart)
                    level.StartingPoint = GridSpace.GetDrawingCoord(entity.MLocation);
            }
        } 

        /// <summary>
        /// Creates and returns a PlayerEnd using the info that is in the xml file
        /// </summary>
        /// <returns>An object that represents the end of a level</returns>
        public PlayerEnd GetPlayerEnd()
        {
            foreach (var entity in _mEntities)
                if (entity.MType == XmlKeys.PlayerLocation && entity.MName == XmlKeys.PlayerEnd)
                    return new PlayerEnd(_mContent, entity);

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
            var objects = new List<GameObject>();
            foreach (var entity in _mEntities)
            {
                //If the object is static, make a static object
                if (entity.MType == XmlKeys.StaticObject)
                {
                    var tile = new Tile(_mContent, .8f, entity);
                    objects.Add(tile);

                }
                //If the object is physics, make a physics object
                if (entity.MType == XmlKeys.PhysicsObject)
                {
                    if (entity.MProperties.ContainsKey("Rail") && entity.MProperties.ContainsKey("Length"))
                        _mRails.Add(entity);

                    var isSquare = entity.MProperties.ContainsKey("Shape") && entity.MProperties["Shape"] == "Square";
                    float mass = 1;
                    if (entity.MProperties.ContainsKey("Mass"))
                    {
                        mass = (float)Convert.ToDouble(entity.MProperties["Mass"]);
                    }
                    if (entity.MProperties.ContainsKey(XmlKeys.Reverse))
                    {
                        var rTile = new ReverseTile(_mContent, ref environment, 0.8f, entity);
                        rTile.Mass = mass;
                        objects.Add(rTile);
                    }
                    else
                    {
                        var mTile = new MovingTile(_mContent, ref environment, 0.8f, entity);
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
            var gridSize = GridSpace.GetGridCoord(level.Size);

            var final = new List<StaticObject>();

            var walls = new List<StaticObject>();
            var rows = new List<StaticObject>[(int)gridSize.Y];
            var cols = new List<StaticObject>[(int)gridSize.X];
            foreach (var entity in _mEntities)
                if (entity.MType == XmlKeys.Walls)
                    walls.Add(new Tile(_mContent, .8f, entity));

            CreateSortedIndicies(walls, rows, false);
            for(var i = 0; i < rows.Length; i++)
            {
                var next = -1;
                var collisionType = "";
                var wall = new List<StaticObject>();
                foreach (var obj in rows[i])
                {
                    var gridLoc = GridSpace.GetGridCoord(obj.MPosition);
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
            for (var i = 0; i < cols.Length; i++)
            {
                var next = -1;
                var collisionType = "";
                var wall = new List<StaticObject>();
                foreach (var obj in cols[i])
                {
                    var gridLoc = GridSpace.GetGridCoord(obj.MPosition);
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
            for (var i = 0; i < indicies.Length; i++) indicies[i] = new List<StaticObject>();
            foreach (var obj in walls)
            {
                var i = 0;
                var gridLoc = GridSpace.GetGridCoord(obj.MPosition);
                var index = (int)gridLoc.Y;
                var compNum = gridLoc.X;

                if (cols) { index = (int)gridLoc.X; compNum = gridLoc.Y; }
                for (; i < indicies[index].Count; i++)
                {
                    var otherGridLoc = GridSpace.GetGridCoord(indicies[index][i].MPosition);

                    var otherComp = otherGridLoc.X;
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
            var triggers = new List<Trigger>();
            foreach (var entity in _mEntities)
            {
                if (entity.MType == XmlKeys.Trigger)
                {
                    var isSquare = entity.MProperties.ContainsKey(XmlKeys.Shape) && entity.MProperties[XmlKeys.Shape] == XmlKeys.Square;

                    if(!entity.MProperties.ContainsKey(XmlKeys.Width) || !entity.MProperties.ContainsKey(XmlKeys.Height))
                        continue;

                    if (entity.MName == "Force")
                    {
                        var trigger = new ForceTrigger(_mContent, entity);
                        triggers.Add(trigger);
                    }
                    else if (entity.MName == "Music")
                    {
                        var trigger = new MusicTrigger(_mContent, entity);
                        triggers.Add(trigger);
                    }
                    else if (entity.MName == "SFX")
                    {
                        var trigger = new FxTrigger(_mContent, entity);
                        triggers.Add(trigger);
                    }
                    else if (entity.MName == "BlackHole")
                    {
                        var trigger = new BlackHoleTrigger(_mContent, entity);
                        triggers.Add(trigger);
                    }
                    else if (entity.MName == "PlayerFace")
                    {
                        var trigger = new PlayerFaceTrigger(_mContent, entity);
                        triggers.Add(trigger);
                    }
                    else if (entity.MName == "PopUp")
                    {
                        var trigger = new PopupTrigger(_mContent, entity);
                        triggers.Add(trigger);
                    }
                    //Add trigger by name
                }
            }
            return triggers;
        }

        public List<EntityInfo> GetRails()
        {
            return _mRails;
        }
    }
}
