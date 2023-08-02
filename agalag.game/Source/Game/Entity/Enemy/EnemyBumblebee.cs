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
    public class EnemyBumblebee : Enemy<EnemyBumblebee>
    {
        //Attributes
        private int _weaponDamage = 1;
        private float _weaponCooldown = 0.5f;
        private float _missileSpeed = 15f;
        private DefaultWeapon _weapon;

        //Health
        private int _defaultHealth = 1;
        private int _maxHealth;
        private int _currentHealth;

        //References
        private Texture2D _bulletTexture;

        //Constructors
        public EnemyBumblebee(
            Texture2D sprite, Vector2 position, Vector2 scale, 
            float rotation = 0, iCollider collider = null, 
            Texture2D bulletTexture = null, EntityAudioManager audioManager = null
        ) : 
            base(sprite, position, scale, rotation, collider, audioManager) 
        { 
            _weapon = new DefaultWeapon(_transform, EntityTag.Enemy);
            _bulletTexture = bulletTexture;
        }
        public EnemyBumblebee(EnemyBumblebee prefab, bool active = false) : 
        this(prefab._sprite.Texture, prefab.Transform.position, prefab.Transform.scale, prefab.Transform.rotation, prefab.Collider, prefab._bulletTexture, prefab._audioManager) 
        {
            SetActive(active);
        }
        public void SetWeapon(float weaponCooldown, int missileDamage, float missileSpeed) {
            this._weapon.SetAttributes(damage: missileDamage, cooldown: weaponCooldown, speed: missileSpeed, direction: new Vector2(0, 1), bulletTexture: _bulletTexture, spawnPoints: new[] {new Vector2(0, 15)});
        }

        #region InterfaceImplementation
        //iEntity
        public override int Health => _currentHealth;
        public override Vector2 CurrentVelocity => _transform.velocity;
        public override Vector2 Position => _transform.position;

        public override void Move(Vector2 direction, float speed, float acceleration)
        {
            speed *= 100;

            float frameTime = FixedUpdater.FixedFrameTime.frameTime;
            _transform.velocity = Vector2.Lerp(_transform.velocity, direction * speed, frameTime * acceleration);
        }
        public override void Stop() =>
            _transform.velocity = Vector2.Lerp(_transform.velocity, Vector2.Zero, 0.99f);
        public override void Shoot() {
            _weapon.Shoot();
        }
        public override void TakeDamage(int damage)
        {
            _currentHealth = System.Math.Clamp(_currentHealth - damage, 0, _maxHealth);

            if(_currentHealth == 0)
                Die();
        }

        //MonoEntity
        public override void Draw(SpriteBatch spriteBatch)
        {
            _sprite?.Draw(_transform, spriteBatch);
        }

        //iPoolableEntity
        public override EnemyBumblebee OnCreate() => new EnemyBumblebee(EntityPool<EnemyBumblebee>.Instance.Prefab);
        public override Action<EnemyBumblebee> onGetFromPool => null;
        public override iObjectPool<EnemyBumblebee> Pool => EntityPool<EnemyBumblebee>.Instance.Pool;

        //Enemy
        protected override void SubInitialize()
        {
            this.SetCollider(new RectangleCollider(new Point(82, 84)));
            _maxHealth = _defaultHealth;
            _currentHealth = _defaultHealth;

            _defaultSpeed = 10f;
            _defaultAcceleration = 10f;
            
            currentSpeed = _defaultSpeed;
            currentAcceleration = _defaultAcceleration;

            _collisionDamage = _defaultCollisionDamage;

            this.SetWeapon(_weaponCooldown, _weaponDamage, _missileSpeed);

            _weapon.onShoot += PlayShotSound;
            _audioManager.PlaySound(EntitySoundType.Movement, looping: true);
        }
        protected override void ReserveToPool() {
            _weapon.onShoot = null;
            Pool.Release(this);
        } 
        #endregion    
    }
}