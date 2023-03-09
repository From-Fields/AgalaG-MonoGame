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
        private Entity _targetObject;
        private Vector2 _targetPosition;
        private Vector2 _desiredDirection = Vector2.Zero;

        //Constructors
        public MoveTowards(Vector2 targetPosition,
            float speedModifier = 1, float accelerationModifier = 1, float trackingSpeed = 1f, 
            float maximumAngle = 360, float minimumDistance = 40
        ) : this(speedModifier, accelerationModifier, trackingSpeed, maximumAngle, minimumDistance)
        {
            _targetPosition = targetPosition;
            _targetObject = null;
        }
        public MoveTowards(Entity targetObject,
            float speedModifier = 1, float accelerationModifier = 1, float trackingSpeed = 1f, 
            float maximumAngle = 360, float minimumDistance = 40
        ): this(speedModifier, accelerationModifier, trackingSpeed, maximumAngle, minimumDistance)
        {
            _targetObject = targetObject;
            _targetPosition = _targetObject.position;
        }
        public MoveTowards(
            float speedModifier, float accelerationModifier, float trackingSpeed, 
            float maximumAngle, float minimumDistance
        ) {
            _speedModifier = speedModifier;
            _accelerationModifier = accelerationModifier;
            _steeringSpeed = trackingSpeed;
            _maximumAngle = maximumAngle;
            _minimumDistance = minimumDistance;
        }

        //Methods
        private Vector2 GetSteeringVector(float speed, Vector2 currentPosition, Vector2 currentVelocity)
        {
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
            Vector2 steeringVector = (desiredVelocity - currentVelocity) * _steeringSpeed;

            Vector2 finalDirection = currentVelocity + steeringVector;
            finalDirection.Normalize();

            return finalDirection;
        }

        #region Interface Implementation
        //Behaviour ends if distance is less than the desired distance.
        //Desired direction is calculated on Update, applied on FixedUpdate.
        public bool CheckCondition(iEnemy target)
        {
            return Vector2.Distance(target.position, _targetPosition) <= _minimumDistance; 
        } 
        public void FixedUpdate(iEnemy target) 
        {
            target.Move(_desiredDirection, target.DesiredSpeed * _speedModifier, target.CurrentAcceleration * _accelerationModifier);
        }
        public void Update(iEnemy target) 
        {
            if(_targetObject != null)
                this._targetPosition = _targetObject.position;

            _desiredDirection = GetSteeringVector(target.DesiredSpeed, target.position, target.currentVelocity);
        }
        public void OnStart(iEnemy target) { return; }
        public void OnFinish(iEnemy target) 
        { 
            //target.Move(Vector2.Zero, target.currentSpeed, target.currentAcceleration); 
        }
        #endregion
    }
}