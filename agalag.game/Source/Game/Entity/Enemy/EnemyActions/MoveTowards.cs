using agalag.engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace agalag.game
{
    public class MoveTowards: iEnemyAction
    {
        //Attributes
        private float _speedModifier;
        private float _accelerationModifier;
        private float _steeringSpeed;
        private float _maximumAngle;
        private float _minimumDistance;
        protected Entity _targetObject;
        protected Vector2 _targetPosition;
        private Vector2 _desiredDirection = Vector2.Zero;

        private bool _stopOnEnd = true;
        private bool _decelerate = false;
        private float _decelerationRadius = 2f;
        private float _decelerationMultiplier = 1;

        //Constructors
        public MoveTowards(Vector2 targetPosition, bool decelerate = true, float decelerationRadius = 120, bool stopOnEnd = true): 
            this(targetPosition, 1, decelerate: decelerate, decelerationRadius: decelerationRadius) { }
        public MoveTowards(Entity target, bool decelerate = false, float decelerationRadius = -1, bool stopOnEnd = false): 
            this(target, 1, decelerate: decelerate, decelerationRadius: decelerationRadius) { }
        public MoveTowards(Vector2 targetPosition,
            float speedModifier, float accelerationModifier = 1, float trackingSpeed = 1f, 
            float maximumAngle = 360, float minimumDistance = 40,
            bool decelerate = true, float decelerationRadius = 120, bool stopOnEnd = true
        ) : this(speedModifier, accelerationModifier, trackingSpeed, maximumAngle, minimumDistance, decelerate, decelerationRadius, stopOnEnd)
        {
            _targetPosition = targetPosition;
            _targetObject = null;
        }
        public MoveTowards(Entity targetObject,
            float speedModifier, float accelerationModifier = 1, float trackingSpeed = 1f, 
            float maximumAngle = 360, float minimumDistance = 40,
            bool decelerate = false, float decelerationRadius = -1, bool stopOnEnd = false
        ): this(speedModifier, accelerationModifier, trackingSpeed, maximumAngle, minimumDistance, decelerate, decelerationRadius, stopOnEnd)
        {
            _targetObject = targetObject;
            _targetPosition = _targetObject.Position;
        }
        public MoveTowards(
            float speedModifier, float accelerationModifier, float trackingSpeed, 
            float maximumAngle, float minimumDistance, bool decelerate, float decelerationRadius, bool stopOnEnd
        ) {
            _speedModifier = speedModifier;
            _accelerationModifier = accelerationModifier;
            _steeringSpeed = trackingSpeed / 10;
            _maximumAngle = maximumAngle;
            _minimumDistance = minimumDistance;
            _decelerate = decelerate;
            _decelerationRadius = decelerationRadius;
            _stopOnEnd = stopOnEnd;
            _decelerationMultiplier = 1;
        }

        //Methods
        private Vector2 GetSteeringVector(float speed, Vector2 currentPosition, Vector2 currentVelocity)
        {
            float steeringMultiplier = _steeringSpeed;

            //Calculates velocity in a straight line towards target
            Vector2 desiredVelocity = _targetPosition - currentPosition;
            desiredVelocity.Normalize();
            desiredVelocity *= speed * _speedModifier;

            //Calculates angle between velocity calculated above and current, actual velocity.
            //If this value is greater than the desired maximum angle, velocity is unaltered.
            float angle = Vector2.Normalize(currentVelocity).Angle(Vector2.Normalize(desiredVelocity));
            if(angle >= _maximumAngle)
                return Vector2.Normalize(currentVelocity);

            //Calculates the hypotenuse vector between the current and desired velocities, multiplied by the turning speed.
            //Returns this normalized value. 
            Vector2 steeringVector = (desiredVelocity - currentVelocity) * steeringMultiplier;

            if(_decelerate) {
                this._decelerationMultiplier = 1;
                float distance = Vector2.Distance(currentPosition, _targetPosition);
                if(distance <= _decelerationRadius) {
                    this._decelerationMultiplier = System.Math.Clamp(distance, 0, distance) / _decelerationRadius;
                    steeringVector *= this._decelerationMultiplier;
                }
                // System.Diagnostics.Debug.WriteLine(distance + " vector: " + _targetPosition + " Multiplier: " + decelerationMultiplier);
            }

            Vector2 finalDirection = currentVelocity + steeringVector;
            finalDirection.Normalize();

            return finalDirection;
        }

        #region Interface Implementation
        //Behaviour ends if distance is less than the desired distance.
        //Desired direction is calculated on Update, applied on FixedUpdate.
        public bool CheckCondition(iEnemy target)
        {
            return Vector2.Distance(target.Position, _targetPosition) <= _minimumDistance; 
        } 
        public virtual void FixedUpdate(iEnemy target) 
        {
            target.Move(_desiredDirection, target.DesiredSpeed * _speedModifier * _decelerationMultiplier, target.CurrentAcceleration * _accelerationModifier);
        }
        public virtual void Update(iEnemy target) 
        {
            if(_targetObject != null)
                this._targetPosition = _targetObject.Position;

            _desiredDirection = GetSteeringVector(target.DesiredSpeed, target.Position, target.CurrentVelocity);
        }
        public void OnStart(iEnemy target) { return; }
        public void OnFinish(iEnemy target) 
        { 
            if(_stopOnEnd)
                target.Stop();
        }
        #endregion
    }
}