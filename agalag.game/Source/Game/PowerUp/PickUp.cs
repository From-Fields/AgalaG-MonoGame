
using System;
using agalag.engine;
using agalag.engine.content;
using agalag.engine.pool;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace agalag.game
{

    public class PickUp : MonoEntity, iPoolableEntity<PickUp>
    {

        private bool _rotate, _doScale, _scaleUp = true;
        private float _rotationSpeed, _maximumScale, _scaleSpeed;
        private Vector2 _initialDirection = Vector2.Zero;

        // References
        private iPowerUp _powerUp;
        private EntityAudioManager _audioManager;

        public PickUp(Layer layer = Layer.Default, EntityAudioManager audioManager = null) : base(layer: layer, active: false) 
        { 
            _audioManager = audioManager;
        } 

        public void Initialize(
            iPowerUp powerUp, Vector2 position, Vector2 direction, Rectangle levelBounds, float speed = 750, 
            bool rotate = true, float rotationSpeed = 10f, 
            bool doScale = true, float maximumScale = 1.3f, float scaleSpeed = 5f
        ) {
            SetCollider(new RectangleCollider(new Point(40, 40), solid: false));
            this._rotate = rotate;
            this._doScale = doScale;
            this._rotationSpeed = rotationSpeed;
            this._maximumScale = maximumScale;
            this._scaleSpeed = scaleSpeed;

            this._powerUp = powerUp;
            this._tag = EntityTag.PickUp;
            this._sprite = new Sprite(powerUp.Sprite);
            
            _transform.position = SetStartingPosition(position, levelBounds);
            _transform.simulate = true;
            _transform.drag = 0;
            SetActive(true);
            
            ApplyMovement(direction, speed);
        }

        private Vector2 SetStartingPosition(Vector2 desiredPosition, Rectangle bounds)
        {
            Vector2 position = desiredPosition;

            position.X = (desiredPosition.X < Collider.Dimensions.X) ? Collider.Dimensions.X 
                : (desiredPosition.X > bounds.Width - Collider.Dimensions.X) ? bounds.Width - Collider.Dimensions.X
                : position.X;

            position.Y = (desiredPosition.Y < Collider.Dimensions.Y) ? Collider.Dimensions.Y
                : (desiredPosition.Y > bounds.Height - Collider.Dimensions.Y) ? bounds.Height - Collider.Dimensions.Y
                : position.Y;

            return position;
        }

        // Movement Methods
        private void ApplyMovement(Vector2 direction, float speed) => _transform.velocity = direction * speed;
        private void ReflectMovement(MonoEntity other) {
            Vector2 contact = other.Collider.ClosestPoint(_transform.position);
            Vector2 velocity = _transform.velocity;
            Vector2 normal = _transform.position - contact;

            Vector2 targetVelocity = velocity.Reflect(normal.normalized());

            _transform.velocity = targetVelocity;
            _audioManager?.PlaySound(EntitySoundType.Bounce);
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
            Player player = other as Player;

            if(player == null) {
                Wall wall = other as Wall;
                if(wall != null)
                    ReflectMovement(other);
                return;
            }

            player.AddPowerUp(_powerUp);

            SetActive(false);
            Pool.Release(this);
        }

        // PoolableObject Implementation
        public PickUp OnCreate() => new PickUp(audioManager: Prefabs.GetPrefabOfType<PickUp>()._audioManager);
        public Action<PickUp> onGetFromPool => null;
        public Action<PickUp> onReleaseToPool => null;
        public iObjectPool<PickUp> Pool => EntityPool<PickUp>.Instance.Pool;
    }
}
