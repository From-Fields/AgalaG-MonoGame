
using System;
using agalag.engine;
using agalag.engine.content;
using agalag.engine.pool;
using agalag.engine.routines;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace agalag.game
{

    public class Hazard : Entity, iPoolableEntity<Hazard>
    {
        private uint _maxBounces = 0, _currentBounces = 0, _damage = 1, _health = 1;
        private bool _rotate, _canBounce, _isWithinBounds;
        private float _rotationSpeed;
        private Vector2 _initialDirection = Vector2.Zero;
        private Rectangle _levelBounds;
        private EntityAudioManager _audioManager;

        public Hazard(Hazard original) : this(original._sprite.Texture, original._audioManager) { } 
        public Hazard(Texture2D sprite, EntityAudioManager audioManager = null) : base(sprite: sprite, Vector2.Zero, Vector2.One) 
        {
            this._audioManager = audioManager;
        } 

        public void Initialize(
            Vector2 position, Vector2 direction, Rectangle levelBounds,
            float speed = 750, uint damage = 1, uint health = 1,
            bool rotate = true, float rotationSpeed = 1f,  Vector2? scale = null,
            uint maxBounces = 0
        ) {
            scale = (scale.HasValue) ? scale.Value : new Vector2(0.6f, 0.6f);
            this.Transform.scale = scale.Value;

            SetCollider(new RectangleCollider(new Point(40, 40), solid: false));
            this._rotate = rotate;
            this._rotationSpeed = rotationSpeed;

            this.SetTag(EntityTag.Hazard);
            
            _transform.position = position;
            _transform.simulate = true;
            _transform.drag = 0;
            SetActive(true);

            this._levelBounds = levelBounds;
            this._isWithinBounds = false;
            this._canBounce = (maxBounces != 0);
            this._maxBounces = maxBounces;
            this._currentBounces = 0;

            this._damage = damage;
            this._health = health;
            
            Move(direction, speed, 0f);
        }

        // Movement Methods
        private bool WillBounce() => (_canBounce && _isWithinBounds && _currentBounces < _maxBounces);
        private void ReflectMovement(MonoEntity other) 
        {
            Vector2 contact = other.Collider.ClosestPoint(_transform.position);
            Vector2 velocity = _transform.velocity;
            Vector2 normal = _transform.position - contact;

            Vector2 targetVelocity = velocity.Reflect(normal.normalized());

            _transform.velocity = targetVelocity;         
            this._currentBounces++;
            _audioManager?.PlaySound(EntitySoundType.Bounce);
        }
        private void DoRotation(FixedFrameTime time) 
        {
            if(!_rotate)
                return;

            _transform.Rotate(_rotationSpeed * time.frameTime);
        }
        private void CheckPosition()
        {
            if(_isWithinBounds)
                return;

            if(_levelBounds.Contains(_collider.FlattenedPolygon.toRectangle())) {
                this._isWithinBounds = true;
            }
        }

        // MonoEntity Implementation
        public override void FixedUpdate(GameTime gameTime, FixedFrameTime fixedFrameTime) 
        {
            CheckPosition();
            DoRotation(fixedFrameTime);
        }
        public override void Draw(SpriteBatch spriteBatch) {
            _sprite?.Draw(_transform, spriteBatch);
        }
        public override void OnCollision(MonoEntity other) {
            Player player = other as Player;

            if(player == null) {
                if(other is Wall) {
                    if(WillBounce())
                        ReflectMovement(other);
                    else if(_isWithinBounds) {
                        RoutineManager.Instance.CallbackTimer(1.5f, ReserveToPool);
                    }
                }

                return;
            }
            player.TakeDamage((int) _damage);
            ReserveToPool();
        }

        // PoolableObject Implementation
        public Hazard OnCreate() => new Hazard(EntityPool<Hazard>.Instance.Prefab);
        public Action<Hazard> onGetFromPool => null;
        public Action<Hazard> onReleaseToPool => null;
        public iObjectPool<Hazard> Pool => EntityPool<Hazard>.Instance.Pool;

        public void ReserveToPool() 
        {
            if(_active == false)
                return;

            SetActive(false);
            Pool.Release(this);
        }

        public override int Health => (int) this._health;
        public override Vector2 Position => Transform.position;
        public override Vector2 CurrentVelocity => Transform.velocity;
        public override void Move(Vector2 direction, float speed, float acceleration) => _transform.velocity = direction * speed;
        public override void Shoot() {  } //Do Nothing
        public override void Stop() => _transform.velocity = Vector2.Zero;

        public override void TakeDamage(int damage)
        {
            _health = (uint) Math.Clamp(_health - damage, 0, _health);

            if(_health <= 0)
                Die();
        }
        public override void Die(){
            _audioManager?.PlaySound(EntitySoundType.Death);
            ReserveToPool();
        } 
    }
}
