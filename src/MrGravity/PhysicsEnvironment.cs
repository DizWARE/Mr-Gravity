using Microsoft.Xna.Framework;
using MrGravity.MISC_Code;

namespace MrGravity
{
    /// <summary>
    /// Environment where Physics objects exist
    /// </summary>
    public class PhysicsEnvironment
    {
        //Vectorized directions
        public Vector2 DirectionUp = new Vector2(0, -1f);
        public Vector2 DirectionDown = new Vector2(0, 1f);
        public Vector2 DirectionLeft = new Vector2(-1f, 0);
        public Vector2 DirectionRight = new Vector2(1f, 0);

        //Default values used for physics environments
        public const int DefaultTerminalSpeed = 20;//pixels per update MAX speed;
        public const int DefaultGravityForce = 1;//(pixels per update) per update;
        public const float DefaultErosionFactor = .99f;
        public const float DefaultDirectionalForce = .3f;

        //Gravity directions magnifiers to allow for different forces in each direction
        private float _mGravityUpMagnifier = DefaultDirectionalForce;            
        private float _mGravityDownMagnifier = DefaultDirectionalForce;
        private float _mGravityLeftMagnifier = DefaultDirectionalForce;
        private float _mGravityRightMagnifier = DefaultDirectionalForce;

        /// <summary>
        /// Gets the gravity magnifier for the given direction
        /// </summary>
        /// <param name="direction">Magnifier that we are getting</param>
        /// <returns>The magnitude of the given direction</returns>
        public float GetGravityMagnifier(GravityDirections direction)
        {
            if (direction == GravityDirections.Up) return _mGravityUpMagnifier;
            if (direction == GravityDirections.Down) return _mGravityDownMagnifier;
            if (direction == GravityDirections.Left) return _mGravityLeftMagnifier;
            return _mGravityRightMagnifier;
        }

        /// <summary>
        /// Sets the magnitude of force in the given direction
        /// </summary>
        /// <param name="direction">The direction of gravity to change</param>
        /// <param name="magnitude">Magnitude for the given direction</param>
        public void SetDirectionalMagnifier(GravityDirections direction, float magnitude)
        {
            if (direction == GravityDirections.Up) _mGravityUpMagnifier = magnitude;
            if (direction == GravityDirections.Down) _mGravityDownMagnifier = magnitude;
            if (direction == GravityDirections.Left) _mGravityLeftMagnifier = magnitude;
            if (direction == GravityDirections.Right) _mGravityRightMagnifier = magnitude;
        }

        /// <summary>
        /// Increments the force magnitude of the given direction by .01 
        /// </summary>
        /// <param name="direction">Force Direction to increment</param>
        public void IncrementDirectionalMagnifier(GravityDirections direction)
        {
            if (direction == GravityDirections.Up) _mGravityUpMagnifier += .01f;
            if (direction == GravityDirections.Down) _mGravityDownMagnifier += .01f;
            if (direction == GravityDirections.Left) _mGravityLeftMagnifier += .01f;
            if (direction == GravityDirections.Right) _mGravityRightMagnifier += .01f;
        }

        /// <summary>
        /// Decrements the force magnitude of the given direction by .01
        /// </summary>
        /// <param name="direction"></param>
        public void DecrementDirectionalMagnifier(GravityDirections direction)
        {
            if (direction == GravityDirections.Up) _mGravityUpMagnifier -= .01f;
            if (direction == GravityDirections.Down) _mGravityDownMagnifier -= .01f;
            if (direction == GravityDirections.Left) _mGravityLeftMagnifier -= .01f;
            if (direction == GravityDirections.Right) _mGravityRightMagnifier -= .01f;
        }

        public int TerminalSpeed { get; set; } = DefaultTerminalSpeed;

        public int GravityMagnitude { get; set; } = DefaultGravityForce;

        public float ErosionFactor { get; set; } = DefaultErosionFactor;

        private static GravityDirections _mGravityDirection = GravityDirections.Down;
        public GravityDirections GravityDirection
        {
            get { return _mGravityDirection; }
            set { _mGravityDirection = value; }
        }

        /// <summary>
        /// Gets the total gravity force in this environment
        /// </summary>
        public Vector2 GravityForce
        {
            get
            {
                var gravityDirection = DirectionDown;
                var gravityMagnifier = _mGravityDownMagnifier;
                if (_mGravityDirection == GravityDirections.Up)
                { gravityDirection = DirectionUp; gravityMagnifier = _mGravityUpMagnifier;}
                if (_mGravityDirection == GravityDirections.Left)
                { gravityDirection = DirectionLeft; gravityMagnifier = _mGravityLeftMagnifier;}
                if (_mGravityDirection == GravityDirections.Right)
                { gravityDirection = DirectionRight; gravityMagnifier = _mGravityRightMagnifier;}

                return Vector2.Multiply(gravityDirection, gravityMagnifier * GravityMagnitude);
            }
        }
    }
}
