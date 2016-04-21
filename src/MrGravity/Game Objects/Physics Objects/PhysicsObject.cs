using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using MrGravity.Game_Objects.Static_Objects;
using MrGravity.Import_Code;
using MrGravity.MISC_Code;

namespace MrGravity.Game_Objects.Physics_Objects
{
    /// <summary>
    /// Represents an object that has rules based on physics
    /// </summary>
    internal abstract class PhysicsObject : GameObject
    {
        protected PhysicsEnvironment MEnvironment;
        public PhysicsEnvironment Environment
        {
            get { return MEnvironment; }
            set { MEnvironment = value; }
        }

        protected float MMass;
        public float Mass
        {
            get { return MMass; }
            set { MMass = value; }
        }

        public bool IsRail { get; }

        //only used for rails
        private readonly Vector2 _mOriginalPosition;
        private readonly float _mHiBound;
        private readonly float _mLowBound;
        private readonly float _mAxis;
        
        //All forces applied to this physicsObject
        private Vector2 _mGravityForce = new Vector2(0,0);
        private Vector2 _mResistiveForce = new Vector2(1,1);
        private Vector2 _mAdditionalForces = new Vector2(0, 0);

        /// <summary>
        /// number of pixels that the player can intersect the hazard without dying
        /// </summary>
        private const float Hazardforgiveness = 12.0f;

        /// <summary>
        /// Directional force on this object
        /// </summary>
        public Vector2 TotalForce => (Vector2.Add(_mGravityForce,_mAdditionalForces));

        /// <summary>
        /// Speed and direction of this object
        /// </summary>
        public Vector2 ObjectVelocity
        {
            get { return MVelocity;  }
            set { MVelocity = value; }
        }

        /// <summary>
        /// Constructs a PhysicsObject; Loads the required info from the content pipeline, and defines its size and location
        /// </summary>
        /// <param name="content">Content pipeline</param>
        /// <param name="spriteBatch">The drawing canvas of the game. Used for collision detection with the level</param>
        /// <param name="name">Name of the physics object so that it can be loaded</param>
        /// <param name="scalingFactors">Factor for the image resource(i.e. half the size would be (.5,.5)</param>
        /// <param name="initialPosition">Position of where this object starts in the level</param>
        public PhysicsObject(ContentManager content, ref PhysicsEnvironment environment, float friction, EntityInfo entity)
            :base(content, friction, entity)
        {
            MEnvironment = environment;
            MVelocity = new Vector2(0, 0);
            _mOriginalPosition = MOriginalInfo.MLocation;
            IsRail = MOriginalInfo.MProperties.ContainsKey(XmlKeys.Rail);

            if (IsRail)
                if (MOriginalInfo.MProperties[XmlKeys.Rail] == XmlKeys.RailX)
                {
                    _mHiBound = GridSpace.GetDrawingCoord(_mOriginalPosition).X + (int.Parse(MOriginalInfo.MProperties[XmlKeys.Length]) * 64);
                    _mLowBound = GridSpace.GetDrawingCoord(_mOriginalPosition).X;
                    _mAxis = GridSpace.GetDrawingCoord(_mOriginalPosition).Y;
                }
                else
                {
                    _mHiBound = GridSpace.GetDrawingCoord(_mOriginalPosition).Y + (int.Parse(MOriginalInfo.MProperties[XmlKeys.Length]) * 64);
                    _mLowBound = GridSpace.GetDrawingCoord(_mOriginalPosition).Y;
                    _mAxis = GridSpace.GetDrawingCoord(_mOriginalPosition).X;
                }
            else
                _mHiBound = _mLowBound = _mAxis = 0;

            UpdateBoundingBoxes();
            MMass = 1;
        }

        /// <summary>
        /// Respawns this object and stops its movement
        /// </summary>
        public override void Respawn()
        {
            base.Respawn();
            UpdateBoundingBoxes();
            MVelocity = Vector2.Zero;
        }

        /// <summary>
        /// Adds an additional force to the physics object
        /// </summary>
        /// <param name="force">Force to be added</param>
        public void AddForce(Vector2 force)
        {
            _mAdditionalForces = Vector2.Add(_mAdditionalForces, force);
        }

        /// <summary>
        /// Applies the force immediately. This will not be permanant
        /// </summary>
        /// <param name="force">Force to apply</param>
        public void ApplyImmediateForce(Vector2 force)
        {
            MVelocity = Vector2.Add(force, MVelocity);
        }

        /// <summary>
        /// TEMP METHOD - WILL GIVE THE PLAYER THE ABILITY TO FALL FROM ONE END OF THE SCREEN TO THE OTHER
        /// </summary>
        public void FixForBounds(int width, int height, bool isFixed)
        {
            if (!isFixed)
            {
                if (MPosition.X < 0) MPosition.X += width;
                if (MPosition.Y < 0) MPosition.Y += height;

                MPosition.X %= width;
                MPosition.Y %= height;
                return;
            }

            if (MPosition.X < 0) { MPosition.X = 0; MVelocity = new Vector2();}
            if (MPosition.Y < 0) {MPosition.Y = 0; MVelocity = new Vector2();}
            if (MPosition.X + MBoundingBox.Width > width) {MPosition.X = width - MBoundingBox.Width; MVelocity = new Vector2();}
            if (MPosition.Y + MBoundingBox.Height > height) { MPosition.Y = height - MBoundingBox.Height; MVelocity = new Vector2(); }
            
        }

        /// <summary>
        /// Reorient gravity in the given direction
        /// </summary>
        /// <param name="direction">Direction to enforce gravity on</param>
        public void ChangeGravityForceDirection(GravityDirections direction)
        {
            _mResistiveForce = new Vector2(1, 1);

            if (direction == GravityDirections.Up || 
                direction == GravityDirections.Down) 
                _mResistiveForce.X = MEnvironment.ErosionFactor;
            else 
                _mResistiveForce.Y = MEnvironment.ErosionFactor;

            _mGravityForce = MEnvironment.GravityForce;
        }

        /// <summary>
        /// Enforces a maximum speed that a force can 
        /// </summary>
        private void EnforceTerminalVelocity()
        {
            if (MVelocity.X > MEnvironment.TerminalSpeed)
                MVelocity.X = MEnvironment.TerminalSpeed;
            if (MVelocity.X < -MEnvironment.TerminalSpeed)
                MVelocity.X = -MEnvironment.TerminalSpeed;
            if (MVelocity.Y > MEnvironment.TerminalSpeed)
                MVelocity.Y = MEnvironment.TerminalSpeed;
            if (MVelocity.Y < -MEnvironment.TerminalSpeed)
                MVelocity.Y = -MEnvironment.TerminalSpeed;
        }

        /// <summary>
        /// Ensures a rail physics objects stays in its bounds
        /// </summary>
        private void EnforceRailBounds()
        {
            if (MOriginalInfo.MProperties[XmlKeys.Rail] == XmlKeys.RailX)
            {

                if (MPosition.X > _mHiBound)
                {
                    MPosition.X = _mHiBound;
                    MVelocity = Vector2.Zero;
                }
                else if (MPosition.X < _mLowBound)
                {
                    MPosition.X = _mLowBound;
                    MVelocity = Vector2.Zero;
                }
                MPosition.Y = _mAxis;
            }
            else
            {
                if (MPosition.Y > _mHiBound)
                {
                    MPosition.Y = _mHiBound;
                    MVelocity = Vector2.Zero;
                }
                else if (MPosition.Y < _mLowBound)
                {
                    MPosition.Y = _mLowBound;
                    MVelocity = Vector2.Zero;
                }
                MPosition.X = _mAxis;
            }
        }

        /// <summary>
        /// Updates the bounding box around this object
        /// </summary>
        public void UpdateBoundingBoxes()
        {
            MBoundingBox = new Rectangle((int)MPosition.X, (int)MPosition.Y, (int)MSize.X, (int)MSize.Y);
        }

        /// <summary>
        /// Updates the velocity based on the force
        /// </summary>
        protected void UpdateVelocities()
        {
            if (IsRail)
            {
                if(MOriginalInfo.MProperties[XmlKeys.Rail] == XmlKeys.RailX)
                    MVelocity.X += (MEnvironment.GravityForce.X / MMass) + _mAdditionalForces.X;
                else if (MOriginalInfo.MProperties[XmlKeys.Rail] == XmlKeys.RailY)
                    MVelocity.Y += (MEnvironment.GravityForce.Y / MMass) + _mAdditionalForces.Y;
            }
            else
            {
                MVelocity = Vector2.Add(MVelocity, Vector2.Divide(MEnvironment.GravityForce, MMass));
                MVelocity = Vector2.Add(MVelocity, _mAdditionalForces); 
            }

            //Force erosion on the resistive forces(friction/wind resistance)
            ChangeGravityForceDirection(MEnvironment.GravityDirection); 
            MVelocity = Vector2.Multiply(MVelocity, _mResistiveForce);
            EnforceTerminalVelocity();

            if (IsRail)
                EnforceRailBounds();
        }

        /// <summary>
        /// Update the physics object based on the given gametime
        /// </summary>
        /// <param name="gametime">Current gametime</param>
        public virtual void Update(GameTime gametime)
        {
            UpdateVelocities();
            MPrevPos = MPosition;
            MPosition = Vector2.Add(MPosition, MVelocity);
            UpdateBoundingBoxes();
        }

        #region Collision Code

        /// <summary>
        /// Returns true if the physics objects are colliding with each other
        /// (only good for 2 boxes)
        /// </summary>
        /// <param name="otherObject">The other object to test against</param>
        /// <returns>True if they are colliding with each other; False otherwise</returns>
        public virtual bool IsCollidingBoxAndBox(GameObject otherObject)
        {
            // if player has not collided with a hazard deeper than HAZARDFORGIVENESS pixels, do not handle the collision
            if (((this is Player) && (otherObject.CollisionType == XmlKeys.Hazardous)) ||
                ((CollisionType == XmlKeys.Hazardous) && (otherObject is Player)))
            {
                # region Find depth
                //Find Center
                float halfHeight1 = BoundingBox.Height / 2;
                float halfWidth1 = BoundingBox.Width / 2;

                //Calculate Center Position
                var center1 = new Vector2(BoundingBox.Left + halfWidth1, BoundingBox.Top + halfHeight1);

                //Find Center of otherObject
                float halfHeight2 = otherObject.BoundingBox.Height / 2;
                float halfWidth2 = otherObject.BoundingBox.Width / 2;

                //Calculate Center Position
                var center2 = new Vector2(otherObject.BoundingBox.Left + halfWidth2, otherObject.BoundingBox.Top + halfHeight2);

                //Center distances between both objects
                var distX = center1.X - center2.X;
                var distY = center1.Y - center2.Y;

                //Minimum distance 
                var minDistX = halfWidth1 + halfWidth2;
                var minDistY = halfHeight1 + halfHeight2;

                float depthX, depthY;
                if (distX > 0)
                {
                    depthX = minDistX - distX;
                }
                else
                {
                    depthX = -minDistX - distX;
                }
                if (distY > 0)
                {
                    depthY = minDistY - distY;
                }
                else
                {
                    depthY = -minDistY - distY;
                }
                depthX = Math.Abs(depthX);
                depthY = Math.Abs(depthY);
                #endregion 
                var shallow = Math.Min(depthX, depthY);
                if (shallow < Hazardforgiveness)
                    return false;
            }

            return !Equals(otherObject) && MBoundingBox.Intersects(otherObject.MBoundingBox);
        }

        /// <summary>
        /// Returns true if the physics objects are colliding with each other
        /// (only good for 2 boxes)
        /// </summary>
        /// <param name="otherObject">The other object to test against</param>
        /// <returns>True if they are colliding with each other; False otherwise</returns>
        public virtual bool IsCollidingBoxAndBoxAnimate(GameObject otherObject)
        {
            var animateArea = MBoundingBox;
            animateArea.Inflate(2, 2);
            return !Equals(otherObject) && animateArea.Intersects(otherObject.MBoundingBox);
        }

        /// <summary>
        /// Returns true if the physics objects are colliding with each other
        /// Circle = this
        /// Currently not used
        /// </summary>
        /// <param name="otherObject">The other object to test against</param>
        /// <returns>True if they are colliding with each other; False otherwise</returns>
        public virtual bool IsCollidingCircleAndBox(GameObject otherObject)
        {   
            var test1 = new BoundingSphere(
                new Vector3(BoundingBox.Center.X,BoundingBox.Center.Y,0f),(BoundingBox.Width/2)-1);

            var hazTest = new BoundingSphere(
                new Vector3(BoundingBox.Center.X,BoundingBox.Center.Y,0f),(BoundingBox.Width/2)-Hazardforgiveness);

            var test2 = new BoundingBox(
                new Vector3(otherObject.BoundingBox.X, otherObject.BoundingBox.Y, 0f),
                new Vector3(otherObject.BoundingBox.X + otherObject.BoundingBox.Width, otherObject.BoundingBox.Y + otherObject.BoundingBox.Height, 0f));
            
            // if player has not collided with a hazard deeper than HAZARDFORGIVENESS pixels, do not handle the collision
            if (((this is Player) && (otherObject.CollisionType == XmlKeys.Hazardous)) ||
                ((CollisionType == XmlKeys.Hazardous) && (otherObject is Player)))
            {
                return hazTest.Intersects(test2);
            }

            return test1.Intersects(test2);
            
        }
        /// <summary>
        /// Returns true if the physics objects are colliding with each other
        /// square = this
        /// Currently not used
        /// </summary>
        /// <param name="otherObject">The other object to test against</param>
        /// <returns>True if they are colliding with each other; False otherwise</returns>
        public virtual bool IsCollidingBoxAndCircle(GameObject otherObject)
        {
            var test1 = new BoundingSphere(
                new Vector3(otherObject.BoundingBox.Center.X, otherObject.BoundingBox.Center.Y, 0f), (otherObject.BoundingBox.Width / 2)-1);

            var hazTest = new BoundingSphere(
                new Vector3(otherObject.BoundingBox.Center.X, otherObject.BoundingBox.Center.Y, 0f), (otherObject.BoundingBox.Width / 2)- Hazardforgiveness);

            var test2 = new BoundingBox(
                new Vector3(BoundingBox.X, BoundingBox.Y, 0f),
                new Vector3(BoundingBox.X + BoundingBox.Width, BoundingBox.Y + BoundingBox.Height, 0f));

            // if player has not collided with a hazard deeper than HAZARDFORGIVENESS pixels, do not handle the collision
            if (((this is Player) && (otherObject.CollisionType == XmlKeys.Hazardous)) ||
                ((CollisionType == XmlKeys.Hazardous) && (otherObject is Player)))
            {
                return hazTest.Intersects(test2);
            }

            return test1.Intersects(test2);

        }
        /// <summary>
        /// Returns true if the physics objects are colliding with each other
        /// </summary>
        /// <param name="otherObject">The other object to test against</param>
        /// <returns>True if they are colliding with each other; False otherwise</returns>
        public virtual bool IsCollidingCircleandCircle(GameObject otherObject)
        {
            var radiusA = MBoundingBox.Width/2;
            var radiusB = otherObject.MBoundingBox.Width/2;
            Point centerPosA = MBoundingBox.Center;
            Point centerPosB = otherObject.MBoundingBox.Center;
            var centers = new Vector2(centerPosA.X - centerPosB.X, centerPosA.Y - centerPosB.Y);

             // if player has not collided with a hazard deeper than HAZARDFORGIVENESS pixels, do not handle the collision
            if (((this is Player) && (otherObject.CollisionType == XmlKeys.Hazardous)) ||
                ((CollisionType == XmlKeys.Hazardous) && (otherObject is Player)))
            {
                if ((centers.Length() + Hazardforgiveness) > (radiusA+radiusB))
                    return false;
            }

            return !Equals(otherObject) && (centers.Length()<(radiusA+radiusB));
        }

        /// <summary>
        /// Decides on the collision detection method for this and the given object
        /// </summary>)
        /// <param name="obj">object we are testing</param>
        public bool HandleCollisions(GameObject obj)
        {
            if (!obj.IsSquare ^ !MIsSquare)
                return HandleCollideCircleAndBox(obj) == 1;
            if (obj.IsSquare & MIsSquare)
                return HandleCollideBoxAndBox(obj) == 1;
            return HandleCollideCircleAndCircle(obj) == 1;
        }

        /// <summary>
        /// Allows for only one collision to be done. 
        /// Whichever object this is colliding with deepest, handle only that collision. 
        /// </summary>
        /// <param name="objList">list of objects that this is colliding with</param>
        /// <returns></returns>
        public void HandleCollisionList(List<GameObject> objList)
        {
            if (objList.Count < 1)
                return;// short circuit

            if (objList.Count == 1)
            {
                HandleCollisions(objList.First());
                return;
            }
            
            var maxCollisionDepth = 0.0f;
            GameObject maxObject = null;

            var wallList = new List<StaticObject>();

            foreach (var gameObj in objList)
            {
                if (gameObj.CollisionType == XmlKeys.Collectable)
                    continue;

                if (gameObj is StaticObject)
                {
                    wallList.Add((StaticObject)gameObj);
                }

                if (gameObj is StaticObject)
                {
                    // 1st Priority
                    var depth = GetCollitionDepth(gameObj);
                    var shallowDepth = Math.Min(Math.Abs(depth.X), Math.Abs(depth.Y));
                    if ((shallowDepth >= maxCollisionDepth) ||
                        (maxObject == null) ||
                        (maxObject is PhysicsObject))
                    {
                        maxCollisionDepth = shallowDepth;// new deepest depth
                        maxObject = gameObj; // new top object
                    }

                }
                else if (gameObj is PhysicsObject)
                {
                    var physObj = (PhysicsObject)gameObj;
                    if (physObj.IsRail)
                    {
                        //2nd Priority
                        var depth = GetCollitionDepth(gameObj);
                        var shallowDepth = Math.Min(Math.Abs(depth.X), Math.Abs(depth.Y));
                        if (shallowDepth >= maxCollisionDepth)
                        {
                            if ((maxObject == null) ||
                                (!(maxObject is StaticObject)))
                            {
                                maxCollisionDepth = shallowDepth;// new deepest depth
                                maxObject = physObj; // new top object
                            }
                        }

                    }
                    else
                    {
                        //3rd priority
                        var depth = GetCollitionDepth(gameObj);
                        var shallowDepth = Math.Min(Math.Abs(depth.X), Math.Abs(depth.Y));
                        if (shallowDepth >= maxCollisionDepth)
                        {
                            if (maxObject == null)
                            {
                                maxCollisionDepth = shallowDepth;// new deepest depth
                                maxObject = physObj; // new top object
                            }
                            else if (maxObject is PhysicsObject)// Not Static
                            {
                                var maxPhys = (PhysicsObject)maxObject;
                                if (!maxPhys.IsRail)
                                {
                                    maxCollisionDepth = shallowDepth;// new deepest depth
                                    maxObject = physObj; // new top object
                                }
                            }
                        }

                    }
                }
            }//End foreach
            if (wallList.Count > 1)
            {
                HandleCollideBoxAndBox(maxObject);
                return;
            }
            if (maxObject != null)
            {
                HandleCollisions(maxObject);
            }
        }

        /// <summary>
        /// Handles collision for two boxes (this, and other)
        /// </summary>
        /// <param name="otherObject">object to do collision on(box)</param>
        /// <returns>1 if collided; 0 if no collision</returns>
        public virtual int HandleCollideBoxAndBox(GameObject otherObject)
        {
            if (!IsCollidingBoxAndBox(otherObject))
            {
                return 0;
            }
            
            //Player collided with collectable
            if (otherObject.CollisionType == XmlKeys.Collectable || CollisionType == XmlKeys.Collectable && !(otherObject is StaticObject))
                return 1;

            var colDepth = GetCollitionDepth(otherObject);

            // handle the shallowest collision
            if (Math.Abs(colDepth.X) > Math.Abs(colDepth.Y))// colliding top or bottom
            {

                //Reset Y Velocity to 0
                MVelocity.Y = 0;

                if (!IsRail || (IsRail && !(MOriginalInfo.MProperties[XmlKeys.Rail] == XmlKeys.RailY)))
                    // reduce x velocity for friction
                    MVelocity.X *= otherObject.MFriction;

                // place the Y pos just so it is not colliding.
                MPosition.Y += colDepth.Y;
            }
            else// colliding left or right
            {

                //Reset X Velocity to 0
                MVelocity.X = 0;

                if (!IsRail || (IsRail && !(MOriginalInfo.MProperties[XmlKeys.Rail] == XmlKeys.RailX)))
                    // reduce Y velocity for friction
                    MVelocity.Y *= otherObject.MFriction;

                // place the X pos just so it is not colliding.
                MPosition.X += colDepth.X;
            }
            
            UpdateBoundingBoxes();
            return 1;// handled collision 
        }
        /// <summary>
        /// Handles collision for circle and circle
        /// </summary>
        /// <param name="otherObject">object to do collision on(circle)</param>
        /// <returns>1 if collided; 0 if no collision</returns>
        public virtual int HandleCollideCircleAndCircle(GameObject otherObject)
        {
            if (!IsCollidingCircleandCircle(otherObject))
            {
                return 0;
            }

            //Player collided with collectable
            if (otherObject.CollisionType == XmlKeys.Collectable || CollisionType == XmlKeys.Collectable && !(otherObject is StaticObject))
                return 1;
            
            Point centerA = MBoundingBox.Center;
            Point centerB = otherObject.BoundingBox.Center;

            var colDepth = GetCollitionDepth(otherObject);

            var centerDiff = new Vector2(centerA.X - centerB.X, centerA.Y - centerB.Y);

            float radiusA = MBoundingBox.Width / 2;
            float radiusB = otherObject.MBoundingBox.Width / 2;

            var delta = (radiusA + radiusB) - centerDiff.Length();
            centerDiff.Normalize();
            Vector2 add = Vector2.Multiply(centerDiff, delta);

            HandleVelocitiesAfterCollision(otherObject, centerDiff);

            // place it just so it is not colliding. 
            if (otherObject is PhysicsObject)
            {
                if (!IsRail)
                    MPosition += add / 2;
                else if (MOriginalInfo.MProperties[XmlKeys.Rail] == XmlKeys.RailX)
                    MPosition.X += add.X / 2;
                else
                    MPosition.Y += add.Y / 2;

                if(!((PhysicsObject)otherObject).IsRail)
                    ((PhysicsObject)otherObject).MPosition -= add / 2;
                else if (otherObject.OriginalInfo.MProperties[XmlKeys.Rail] == XmlKeys.RailX)
                    ((PhysicsObject)otherObject).MPosition.X -= add.X / 2;
                else
                    ((PhysicsObject)otherObject).MPosition.Y -= add.Y / 2;
            }
            else
            {
                // do not move a static object
                if(!IsRail)
                    MPosition += add;
                else if (MOriginalInfo.MProperties[XmlKeys.Rail] == XmlKeys.RailX)
                    MPosition.X += add.X;
                else
                    MPosition.Y += add.Y;
            }

            UpdateBoundingBoxes();
            if (add.Length() > 1.0f)
            {
                return 1; // changed enough to call a collision
            }
            return 0;
        }
        /// <summary>
        /// Handles collision for circle and Box (circle =this)
        /// </summary>
        /// <param name="otherObject">object to do collision on(box)</param>
        /// <returns>1 if collided; 0 if no collision</returns>
        public virtual int HandleCollideCircleAndBox(GameObject otherObject)
        {
            if (!IsCollidingBoxAndBox(otherObject))
            {
                return 0;// no collision
            }
                
            //Player collided with collectable
            if (otherObject.CollisionType == XmlKeys.Collectable || CollisionType == XmlKeys.Collectable && !(otherObject is StaticObject))
                return 1;

            // get points of square
            var p = new Point[4];
            // top left
            p[0] = new Point(otherObject.MBoundingBox.X,otherObject.MBoundingBox.Y);
            // top right
            p[1] = new Point(otherObject.MBoundingBox.X + otherObject.MBoundingBox.Width, p[0].Y);
            // bottom right
            p[2] = new Point(p[1].X, otherObject.MBoundingBox.Y + otherObject.MBoundingBox.Height);
            // bottom left
            p[3] = new Point(p[0].X, p[2].Y);

            Point center = BoundingBox.Center;
            // if not going to collide with a corner
            if (((center.X >= p[0].X) && (center.X <= p[1].X)) // top/bottom side
             || ((center.Y >= p[1].Y) && (center.Y <= p[2].Y)))// right/left side
            {
                // then treat like a square /square
                return HandleCollideBoxAndBox(otherObject);
            }
            // treat like circle/point collision
            Point centerA = MBoundingBox.Center;
            var centerB = new Point();
            if ((center.X < p[0].X) && (center.Y < p[0].Y))// top left corner
            {
                centerB = p[0];
            }
            else if ((center.X > p[1].X) && (center.Y < p[1].Y))// top right corner
            {
                centerB = p[1];
            }
            else if ((center.X > p[2].X) && (center.Y > p[2].Y))// bottom right corner
            {
                centerB = p[2];
            }
            else if ((center.X < p[3].X) && (center.Y > p[3].Y))// bottom left corner
            {
                centerB = p[3];
            }

            var centerDiff = new Vector2(centerA.X - centerB.X, centerA.Y - centerB.Y);

            float radiusA = MBoundingBox.Width / 2;

            var delta = (radiusA) - centerDiff.Length();
            centerDiff.Normalize();
            Vector2 add = Vector2.Multiply(centerDiff, delta);

            //Do not change velocity because we want an inelastic collision on corners.
            #region Corner magic
            // the best I could do for the constraints i am given.
            // Screw you people.
            // Love, 
            //  Curtis Taylor
            if ((Environment.GravityDirection == GravityDirections.Down) ||
                (Environment.GravityDirection == GravityDirections.Up))
            {
                MVelocity.X = 0f;
                MVelocity.Y *= .96f;
            }
            else
            {
                MVelocity.X *= .96f;
                MVelocity.Y = 0f;
            }
            #endregion
            // place the Y pos just so it is not colliding.
            if (!IsRail)
                MPosition += add;
            else if (MOriginalInfo.MProperties[XmlKeys.Rail] == XmlKeys.RailX)
                MPosition.X += add.X;
            else
                MPosition.Y += add.Y;
                
            UpdateBoundingBoxes();
            if (add.Length() > 0.5f)
            {
                return 1; // changed enough to call a collision
            }
            return 0; // did not collide
        }

        /// <summary>
        /// finds how deep they are intersecting (That is what she said!)
        /// </summary>
        /// <returns>vector decribing depth</returns>
        public Vector2 GetCollitionDepth(GameObject otherObject)
        {
            //Find Center
            float halfHeight1 = BoundingBox.Height / 2;
            float halfWidth1 = BoundingBox.Width / 2;

            //Calculate Center Position
            var center1 = new Vector2(BoundingBox.Left + halfWidth1, BoundingBox.Top + halfHeight1);
            
            //Find Center of otherObject
            float halfHeight2 = otherObject.BoundingBox.Height / 2;
            float halfWidth2 = otherObject.BoundingBox.Width / 2;

            //Calculate Center Position
            var center2 = new Vector2(otherObject.BoundingBox.Left + halfWidth2, otherObject.BoundingBox.Top + halfHeight2);
            
            //Center distances between both objects
            var distX = center1.X - center2.X;
            var distY = center1.Y - center2.Y;

            //Minimum distance 
            var minDistX = halfWidth1 + halfWidth2;
            var minDistY = halfHeight1 + halfHeight2;

            if (!IsCollidingBoxAndBox(otherObject))
            {
                return Vector2.Zero;
            }

            float depthX, depthY;
            if (distX > 0)
            {
                depthX = minDistX - distX;
            }
            else
            {
                depthX = -minDistX - distX;
            }
            if (distY > 0)
            {
                depthY = minDistY - distY;
            }
            else
            {
                depthY = -minDistY - distY;
            }

            return new Vector2(depthX, depthY);
        }

        private void HandleVelocitiesAfterCollision(GameObject otherObject,Vector2 normal)
        {
            // Thanks to Dr. Bob of UofU SoC

            // normal of the collision
            var n = normal;
            n.Normalize();

            // tangent of the collision
            var T = new Vector2(n.Y, -n.X);

            var e = 0.9f;//0.9f; // elasticity of the collision

            var vain = Vector2.Dot(MVelocity, n);
            var vait = Vector2.Dot(MVelocity, T);

            float vbin, vbit;
            if (otherObject is PhysicsObject)
            {
                vbin = Vector2.Dot(((PhysicsObject)otherObject).MVelocity, n);
                vbit = Vector2.Dot(((PhysicsObject)otherObject).MVelocity, T);
            }
            else
            {
                vbin = Vector2.Dot(Vector2.Zero, n);
                vbit = Vector2.Dot(Vector2.Zero, T);
            }

            var vafn = ((e + 1.0f) * vbin + vain * (1 - e)) / 2;
            var vbfn = ((e + 1.0f) * vain - vbin * (1 - e)) / 2;
            var vaft = vait;
            var vbft = vbit;

            if (!IsRail)
            {
                MVelocity.X = vafn * n.X + vaft * T.X;
                MVelocity.Y = vafn * n.Y + vaft * T.Y;
            }
            else if (MOriginalInfo.MProperties[XmlKeys.Rail] == XmlKeys.RailX)
                MVelocity.X = vafn * n.X + vaft * T.X;
            else
                MVelocity.Y = vafn * n.Y + vaft * T.Y;


            if (otherObject is PhysicsObject)
            {
                if (!((PhysicsObject)otherObject).IsRail)
                {
                    ((PhysicsObject)otherObject).MVelocity.X = vbfn * n.X + vbft * T.X;
                    ((PhysicsObject)otherObject).MVelocity.Y = vbfn * n.Y + vbft * T.Y;
                }
                else if (otherObject.OriginalInfo.MProperties[XmlKeys.Rail] == XmlKeys.RailX)
                    ((PhysicsObject)otherObject).MVelocity.X = vbfn * n.X + vbft * T.X;
                else
                    ((PhysicsObject)otherObject).MVelocity.Y = vbfn * n.Y + vbft * T.Y;
            }
        }

        #endregion

        public abstract int Kill();
        public abstract override string ToString();
    }
}

/*
            //// is colliding
            //if (otherObject is PhysicsObject)
            //{
            //    PhysicsObject PhysObj = (PhysicsObject)otherObject;

            //    // seperate balls (Tee hee!)
            //    // back the balls up along their previous path until they're not colliding

            //    // what percentage to increment moving back. .01 = 1% 
            //    float increment = .001f; //(Lower = more accurate; Higher = better performance
            //    float t = increment;
            //    while (IsCollidingCircleandCircle(PhysObj))
            //    {
            //        this.mPosition = this.mPrevPos;
            //        PhysObj.mPosition = PhysObj.mPrevPos;
            //        UpdateBoundingBoxes();
            //        t += increment;
            //    }

            //    // normal of the collision
            //    float n_x = this.mPosition.X - PhysObj.mPosition.X;
            //    float n_y = this.mPosition.Y - PhysObj.mPosition.Y;
            //    float n_length = (float)Math.Sqrt((n_x * n_x) + (n_y * n_y));
            //    // normalize n
            //    if (n_length > 0)
            //    {
            //        n_x /= n_length;
            //        n_y /= n_length;
            //    }

            //    // tangent of the collision
            //    float t_x = n_y;
            //    float t_y = -n_x;

            //    float e = 0.99f; // elasticity of the collision

            //    float vain = (this.mVelocity.X * n_x) + (this.mVelocity.Y * n_y); //Vector2.Dot(ball1.Velocity, N);
            //    float vait = (this.mVelocity.X * t_x) + (this.mVelocity.Y * t_y); //Vector2.Dot(ball1.Velocity, T);
            //    float vbin = (PhysObj.mVelocity.X * n_x) + (PhysObj.mVelocity.Y * n_y); //Vector2.Dot(ball2.Velocity, N);
            //    float vbit = (PhysObj.mVelocity.X * t_x) + (PhysObj.mVelocity.Y * t_y); //Vector2.Dot(ball2.Velocity, T);

            //    float vafn = ((e + 1.0f) * vbin + vain * (1 - e)) / 2;
            //    float vbfn = ((e + 1.0f) * vain - vbin * (1 - e)) / 2;
            //    float vaft = vait;
            //    float vbft = vbit;

            //    this.mVelocity.X = vafn * n_x + vaft * t_x;
            //    this.mVelocity.Y = vafn * n_y + vaft * t_y;
            //    PhysObj.mVelocity.X = vbfn * n_x + vbft * t_x;
            //    PhysObj.mVelocity.Y = vbfn * n_y + vbft * t_y;

            //}
            //else
            //{
            //    //just move "this"
            //    this.mVelocity = Vector2.Zero;
            //}
            */