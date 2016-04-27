using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using MrGravity.Import_Code;

namespace MrGravity.Game_Objects.Static_Objects
{
    /// <summary>
    /// This class represents objects that don't move
    /// </summary>
    internal abstract class StaticObject : GameObject
    {
        /// <summary>
        /// Constructs a new static object
        /// </summary>
        /// <param name="content">The games content manager</param>
        /// <param name="name">Name of the Object("Images/{Type}/{Name}"</param>
        /// <param name="initialPosition">Starting position</param>
        /// <param name="friction">Friction that reacts to physics objects</param>
        /// <param name="isSquare">True if the object should behave like a square</param>
        /// <param name="isHazardous">True if the object should kill the player when touched</param>
        public StaticObject(ContentManager content, float friction, EntityInfo entity)
            : base(content, friction, entity)
        {
            Velocity = Vector2.Zero;
        }

        /// <summary>
        ///     Default Constructor.
        /// </summary>
        public StaticObject()
        {
            
        }
    }
}
