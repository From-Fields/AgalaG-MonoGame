using agalag.engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace agalag.game
{
    public class MoveAndShoot: MoveTowards
    {
        //Constructors
        public MoveAndShoot(Vector2 targetPosition, bool decelerate = true, float decelerationRadius = 120, bool stopOnEnd = true): 
            this(targetPosition, 1, decelerate: decelerate, decelerationRadius: decelerationRadius) { }
        public MoveAndShoot(Entity target, bool decelerate = false, float decelerationRadius = -1, bool stopOnEnd = false): 
            this(target, 1, decelerate: decelerate, decelerationRadius: decelerationRadius) { }
        public MoveAndShoot(Vector2 targetPosition,
            float speedModifier, float accelerationModifier = 1, float trackingSpeed = 1f, 
            float maximumAngle = 360, float minimumDistance = 40,
            bool decelerate = true, float decelerationRadius = 120, bool stopOnEnd = true
        ) : base(speedModifier, accelerationModifier, trackingSpeed, maximumAngle, minimumDistance, decelerate, decelerationRadius, stopOnEnd)
        {
            _targetPosition = targetPosition;
            _targetObject = null;
        }
        public MoveAndShoot(Entity targetObject,
            float speedModifier, float accelerationModifier = 1, float trackingSpeed = 1f, 
            float maximumAngle = 360, float minimumDistance = 40,
            bool decelerate = false, float decelerationRadius = -1, bool stopOnEnd = false
        ): base(speedModifier, accelerationModifier, trackingSpeed, maximumAngle, minimumDistance, decelerate, decelerationRadius, stopOnEnd)
        {
            _targetObject = targetObject;
            _targetPosition = _targetObject.Position;
        }

        #region Interface Implementation
        public override void Update(iEnemy target) {
            base.Update(target);
            target.Shoot();
        }
        #endregion
    }
}