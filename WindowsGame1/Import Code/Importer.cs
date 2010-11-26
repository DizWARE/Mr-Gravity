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

namespace GravityShift.Import_Code
{
    /// <summary>
    /// Imports an XML file into the game
    /// </summary>
    class Importer
    {
        ContentManager mContent;
        List<EntityInfo> mEntities;
        public Importer(ContentManager content)
        {
            mContent = content;
            mEntities = new List<EntityInfo>();
        }

        public Level ImportLevel(string filename)
        {
            XElement xLevel = XElement.Load(filename);

            Level level = new Level();

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


        public Vector2 GetPlayerStart()
        {
            foreach (EntityInfo entity in mEntities)
                if (entity.mType == XmlKeys.PLAYER_LOCATION && entity.mName == XmlKeys.PLAYER_START)
                    return GridSpace.GetDrawingCoord(entity.mLocation);

            return new Vector2(-100, -100);
        } 

        public PlayerEnd GetPlayerEnd()
        {
            foreach (EntityInfo entity in mEntities)
                if (entity.mType == XmlKeys.PLAYER_LOCATION && entity.mName == XmlKeys.PLAYER_END)
                    return new PlayerEnd(mContent, GridSpace.GetDrawingCoord(entity.mLocation));

            return null;
        }

        public List<GameObject> GetObjects(ref PhysicsEnvironment environment)
        {
            List<GameObject> objects = new List<GameObject>();
            foreach (EntityInfo entity in mEntities)
            {
                if (entity.mType == XmlKeys.STATIC_OBJECT)
                {
                    if (entity.mHazardous)
                    {
                        bool isSquare = entity.mProperties.Count == 0 || entity.mProperties["Shape"] == "Square";
                        objects.Add(new HazardTile(mContent, "Images\\" + entity.mTextureFile,
                            GridSpace.GetDrawingCoord(entity.mLocation), .8f, isSquare));
                    }
                    else
                    {
                        bool isSquare = entity.mProperties.Count == 0 || entity.mProperties["Shape"] == "Square";
                        objects.Add(new Tile(mContent, "Images\\" + entity.mTextureFile,
                            GridSpace.GetDrawingCoord(entity.mLocation), .8f, isSquare));
                    }
                }
                if (entity.mType == XmlKeys.PHYSICS_OBJECT)
                {
                    if (entity.mHazardous)
                    {
                        bool isSquare = entity.mProperties.Count == 0 || entity.mProperties["Shape"] == "Square";
                        objects.Add(new HazardousMovingTile(mContent, "Images\\" + entity.mTextureFile, new Vector2(1, 1),
                            GridSpace.GetDrawingCoord(entity.mLocation), ref environment, .8f, isSquare));
                    }
                    else
                    {
                        bool isSquare = entity.mProperties.Count == 0 || entity.mProperties["Shape"] == "Square";
                        objects.Add(new MovingTile(mContent, "Images\\" + entity.mTextureFile, new Vector2(1, 1),
                            GridSpace.GetDrawingCoord(entity.mLocation), ref environment, .8f, isSquare));
                    }
                }
            }

            return objects;
        }
        public List<Tile> GetWalls()
        {
            return null;
        }
    }
}
