
using System;
using agalag.engine;
using agalag.engine.pool;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace agalag.game
{

    public class PickUp : MonoEntity, iPoolableEntity<PickUp>
    {

        private bool _rotate, _doScale, _scaleUp = true;
        private float _rotationSpeed, _maximumScale, _scaleSpeed, _initialSpeed;
        private Vector2 _initialDirection = Vector2.Zero;

        // References
        private PowerUp _powerUp;

        public PickUp() => SetActive(false);

        public void Initialize(
            PowerUp powerUp, Vector2 position, Vector2 direction, float speed = 5, 
            bool rotate = true, float rotationSpeed = 100f, 
            bool doScale = true, float maximumScale = 1.3f, float scaleSpeed = 5f
        ) {
            this._rotate = rotate;
            this._doScale = doScale;
            this._rotationSpeed = rotationSpeed;
            this._maximumScale = maximumScale;
            this._scaleSpeed = scaleSpeed;

            this._powerUp = powerUp;
            this._tag = Utils.Tags[EntityTag.PickUp];
            this._sprite = new Sprite(powerUp.Sprite);
            
            _transform.position = position;
            
            ApplyMovement(direction, speed);
        }

        // Movement Methods
        private void ApplyMovement(Vector2 direction, float speed) => _transform.velocity = direction * speed;
        private void ReflectMovement(MonoEntity other) {
            Vector2 contact = other.Collider.ClosestPoint(_transform.position);
            Vector2 velocity = _transform.velocity;
            Vector2 normal = (Vector2) _transform.position - contact;
            normal.Normalize();

            Vector2 targetVelocity = velocity - 2 * (Vector2.Dot(velocity, normal) * normal);

            _transform.velocity = targetVelocity;
        }
        private void DoScale(FixedFrameTime time) {
            if(!_doScale)
                return;

            Vector2 targetScale = (_scaleUp) ? Vector2.One * _maximumScale : Vector2.One;

            _transform.scale = Vector2.Lerp(_transform.scale, targetScale, time.frameTime * _scaleSpeed);

            if(Vector2.Distance(_transform.scale, targetScale) <= 0.05f)
                _scaleUp = !_scaleUp;
        }
        private void DoRotation(FixedFrameTime time) {
            if(!_rotate)
                return;

            _transform.Rotate(_rotationSpeed * time.frameTime);
        }

        // MonoEntity Implementation
        public override void FixedUpdate(GameTime gameTime, FixedFrameTime fixedFrameTime) {
            DoRotation(fixedFrameTime);
            DoScale(fixedFrameTime);
        }
        public override void Draw(SpriteBatch spriteBatch) {
            _sprite?.Draw(_transform, spriteBatch);
        }
        public override void OnCollision(MonoEntity other) {
            Player player = (Player) other;

            if(player == null) {
                ReflectMovement(other);
                return;
            }

            player.AddPowerUp(_powerUp);

            SetActive(false);
            Pool.Release(this);
        }

        // PoolableObject Implementation
        public PickUp OnCreate() => new PickUp();
        public Action<PickUp> onGetFromPool => null;
        public Action<PickUp> onReleaseToPool => null;
        public iObjectPool<PickUp> Pool => EntityPool<PickUp>.Instance.Pool;
    }
}
