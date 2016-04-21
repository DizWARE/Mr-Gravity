using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using MrGravity.Game_Objects.Physics_Objects;
using MrGravity.Import_Code;

namespace MrGravity.Game_Objects.Static_Objects.Triggers
{
    internal class ForceTrigger : Trigger
    {
        private readonly List<PhysicsObject> _affectedObjects = new List<PhysicsObject>();
        private readonly Vector2 _mForce = new Vector2(1, 0);

        public ForceTrigger(ContentManager content, EntityInfo entity) :
            base(content, entity) 
        {
            if (entity.MProperties.ContainsKey(XmlKeys.Xforce))
                _mForce.X = float.Parse(entity.MProperties[XmlKeys.Xforce]);
            if (entity.MProperties.ContainsKey(XmlKeys.Yforce))
                _mForce.Y = float.Parse(entity.MProperties[XmlKeys.Yforce]);
        }

        /// <summary>
        /// This triggers adds a force in the x direction while the player is within its bounds and removes it
        /// when the player exits
        /// </summary>
        /// <param name="objects">List of objects in the game</param>
        /// <param name="player">Player in the game</param>
        public override void RunTrigger(List<GameObject> objects, Player player)
        {
            foreach (var gObj in objects)
            {
                if(gObj is PhysicsObject)
                {
                    var isColliding = MBoundingBox.Intersects(gObj.BoundingBox);
                    var pObj = (PhysicsObject)gObj;

                    if (!_affectedObjects.Contains(pObj) && isColliding)
                    { pObj.AddForce(_mForce); _affectedObjects.Add(pObj); }
                    else if (_affectedObjects.Contains(pObj) && !isColliding)
                    { pObj.AddForce(Vector2.Multiply(_mForce,-1)); _affectedObjects.Remove(pObj); }                    
                }
            }
        }
    }
}
