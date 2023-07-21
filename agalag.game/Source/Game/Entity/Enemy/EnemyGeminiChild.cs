using System;
using System.Collections.Generic;
using System.Diagnostics;
using agalag.engine;
using agalag.engine.pool;
using agalag.game;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace agalag.game
{
    public class EnemyGeminiChild : Enemy<EnemyGeminiChild>
    {
        //Attributes
        private bool _wasKilled = false;
        private float _positionOffset;
        private float _velocityMultiplier = 100;
        private float _orbitMultiplier = 60;
        private float _orbitingVelocity;
        private DefaultWeapon _weapon;
        private Vector2? _desiredVelocity;
        private Vector2? _toChild, _fromChild, _tangent;

        //Health
        private int _currentHealth;
        private int _maxHealth = 1;

        //References
        private EnemyGemini _parent;
        private Texture2D _bulletTexture;
        private static Matrix? _rotationMatrix;

        //Constructors
        public EnemyGeminiChild(
            Texture2D sprite, Vector2 position, Vector2 scale, 
            float rotation = 0, iCollider collider = null, 
            Texture2D bulletTexture = null, EntityAudioManager audioManager = null
        ): 
            base(sprite, position, scale, rotation, collider, audioManager) 
        {
            _weapon = new DefaultWeapon(_transform, EntityTag.Enemy);
            _bulletTexture = bulletTexture;
        }
        public EnemyGeminiChild(EnemyGeminiChild prefab, bool active = false) : 
        this(prefab._sprite.Texture, prefab.Transform.position, prefab.Transform.scale, prefab.Transform.rotation, prefab.Collider, prefab._bulletTexture, prefab._audioManager) 
        {
            SetActive(active);
        }

        //Initialization Methods
        public void SetParent(EnemyGemini parent, float positionOffset, float orbitingVelocity) {
            this._parent = parent;
            this._positionOffset = positionOffset;
            this._orbitingVelocity = orbitingVelocity;
        }
        public void SetWeapon(float weaponCooldown, int missileDamage, float missileSpeed) {
            this._weapon.SetAttributes(damage: missileDamage, cooldown: weaponCooldown, speed: missileSpeed, direction: new Vector2(0, 1), bulletTexture: _bulletTexture);
        }

        #region InterfaceImplementation
        //iEntity
        public override int Health => 0;
        public override Vector2 CurrentVelocity => _transform.velocity;
        public override Vector2 Position => _transform.position;

        public override void Move(Vector2 direction, float speed, float acceleration)
        {
            _desiredVelocity = direction * speed;
        }
        public override void Stop() {
            _desiredVelocity = Vector2.Zero;
        }
        public override void Shoot() {
            _weapon.Shoot();
        }
        public override void TakeDamage(int damage)
        {
            damage = System.Math.Clamp(damage, 0, 1);
            _currentHealth = System.Math.Clamp(_currentHealth - damage, 0, _maxHealth);

            if(_currentHealth == 0) {
                Die();
            }
        }

        //MonoEntity
        public override void Draw(SpriteBatch spriteBatch)
        {
            _sprite?.Draw(_transform, spriteBatch);
        }

        //iPoolableEntity
        public override EnemyGeminiChild OnCreate() => new EnemyGeminiChild(EntityPool<EnemyGeminiChild>.Instance.Prefab);
        public override Action<EnemyGeminiChild> onGetFromPool => null;
        public override iObjectPool<EnemyGeminiChild> Pool => EntityPool<EnemyGeminiChild>.Instance.Pool;

        //Enemy
        protected override void SubInitialize()
        {
            this.SetCollider(new RectangleCollider(new Point(82, 84)));
            _transform.drag = 0;
            _wasKilled = false;
            _isDead = false;
            _currentHealth = _maxHealth;

            onDeath += (_) => _wasKilled = true;

            _defaultSpeed = 10f;
            _defaultAcceleration = 10f;
            
            currentSpeed = _defaultSpeed;
            currentAcceleration = _defaultAcceleration;
            _collisionDamage = _defaultCollisionDamage;

            if(!_rotationMatrix.HasValue)
                _rotationMatrix = Matrix.CreateRotationZ(MathHelper.ToRadians(90));

            _weapon.onShoot += PlayShotSound;
        }
        protected override void ReserveToPool() => Pool.Release(this);

        protected override void SubReserve() {
            base.SubReserve();

            _weapon.onShoot = null;
            
            if(_wasKilled)
                this._parent.TakeDamage(1);
        }

        protected override void SubUpdate(GameTime gameTime) {
            Vector2 fromChild = (Position - _parent.Position).normalized();
            Vector2 toChild = (_parent.Position - Position).normalized();
            Vector2 tangent = Vector2.Transform(fromChild, _rotationMatrix.Value).normalized();
            this._toChild = toChild;
            this._fromChild = fromChild;
            this._tangent = tangent;
        }
        protected override void SubFixedUpdate(GameTime gameTime, FixedFrameTime fixedFrameTime) {
            if(!_toChild.HasValue || !_fromChild.HasValue || !_tangent.HasValue)
                return;

            float distance = Vector2.Distance(Position, _parent.Position);
            float offsetMultiplier = _velocityMultiplier * (_orbitMultiplier * distance/_positionOffset);
            float acceleration = _parent.CurrentAcceleration;
            Vector2 _desiredOrbit = _velocityMultiplier * _tangent.Value * _orbitingVelocity * _orbitMultiplier * 2 * MathF.PI * _positionOffset / distance;

            _desiredOrbit += (_toChild.Value * _orbitingVelocity * offsetMultiplier);
            
            _desiredOrbit *= fixedFrameTime.frameTime;

            _transform.velocity = Vector2.Lerp(_transform.velocity, _desiredOrbit, fixedFrameTime.frameTime * currentAcceleration);

            if(_desiredVelocity.HasValue) {
                if(_desiredVelocity == Vector2.Zero)
                    _desiredVelocity = null;
                else {
                    _transform.velocity = Vector2.Lerp(_transform.velocity, _desiredVelocity.Value, fixedFrameTime.frameTime * acceleration);
                    _desiredVelocity = Vector2.Zero;
                }
            }
            // Debug.WriteLine("Tangent: " + _tangent);
            // Debug.WriteLine("ToChild: " + _tangent);
        }
        #endregion    
    }
}