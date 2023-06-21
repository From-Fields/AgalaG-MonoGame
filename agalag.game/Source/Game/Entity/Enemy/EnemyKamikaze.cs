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
    public class EnemyKamikaze : Enemy<EnemyKamikaze>
    {
        //Health
        private int _defaultHealth = 1;
        private int _maxHealth;
        private int _currentHealth;

        //Constructors
        public EnemyKamikaze(Texture2D sprite, Vector2 position, Vector2 scale, float rotation = 0, iCollider collider = null) : 
        base(sprite, position, scale, rotation, collider) { }
        public EnemyKamikaze(EnemyKamikaze prefab, bool active = false) : 
        this(prefab._sprite.Texture, prefab.Transform.position, prefab.Transform.scale, prefab.Transform.rotation, prefab.Collider) 
        {
            SetActive(active);
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
        public override void Shoot() { }
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
        public override EnemyKamikaze OnCreate() => new EnemyKamikaze(EntityPool<EnemyKamikaze>.Instance.Prefab);
        public override Action<EnemyKamikaze> onGetFromPool => null;
        public override iObjectPool<EnemyKamikaze> Pool => EntityPool<EnemyKamikaze>.Instance.Pool;

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

            System.Diagnostics.Debug.WriteLine(this + ": " + Tag);
        }
        public override void Reserve() => Pool.Release(this);
        #endregion    
    }
}