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
    public class EnemyGemini : Enemy<EnemyGemini>
    {
        //Attributes
        private float _geminiPositionOffset = 100f;
        private float _orbitingVelocity = 1f;
        private int _geminiMissileDamage = 1;
        private float _weaponCooldown = 1f;
        private float _missileSpeed = 12f;

        //Health
        private int _defaultHealth = 2;
        private int _currentHealth;
        private int _maxHealth;

        //References
        private List<EnemyGeminiChild> _children = new List<EnemyGeminiChild>();
        public iObjectPool<EnemyGeminiChild> _childPool => EntityPool<EnemyGeminiChild>.Instance.Pool;

        //Constructors
        public EnemyGemini(Texture2D sprite, Vector2 position, Vector2 scale, float rotation = 0, iCollider collider = null) : 
        base(sprite, position, scale, rotation, collider) { }
        public EnemyGemini(EnemyGemini prefab, bool active = false) : 
        this(prefab._sprite.Texture, prefab.Transform.position, prefab.Transform.scale, prefab.Transform.rotation, prefab.Collider) 
        {
            SetActive(active);
        }

        #region InterfaceImplementation
        //iEntity
        public override int Health => 0;
        public override Vector2 CurrentVelocity => _transform.velocity;
        public override Vector2 Position => _transform.position;

        public override void Move(Vector2 direction, float speed, float acceleration)
        {
            speed *= 100;

            float frameTime = FixedUpdater.FixedFrameTime.frameTime;
            _transform.velocity = Vector2.Lerp(_transform.velocity, direction * speed, frameTime * acceleration);
            
            Debug.WriteLine("Parent: " + _transform.velocity);

            foreach(var child in _children) {
                child.Move(direction, speed, frameTime * acceleration);
            }
        }
        public override void Stop() =>
            _transform.velocity = Vector2.Lerp(_transform.velocity, Vector2.Zero, 0.99f);
        public override void Shoot() {
            foreach (var child in _children) {
                child.Shoot();
            }
        }
        public override void TakeDamage(int damage)
        {
            damage = System.Math.Clamp(damage, 0, 1);
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
        public override EnemyGemini OnCreate() => new EnemyGemini(EntityPool<EnemyGemini>.Instance.Prefab);
        public override Action<EnemyGemini> onGetFromPool => null;
        public override iObjectPool<EnemyGemini> Pool => EntityPool<EnemyGemini>.Instance.Pool;

        //Enemy
        protected override void SubInitialize()
        {
            this.SetCollider(new RectangleCollider(new Point(1, 1)));
            _isDead = false;
            _maxHealth = _defaultHealth;
            _currentHealth = _defaultHealth;

            _defaultSpeed = 10f;
            _defaultAcceleration = 10f;
            
            currentSpeed = _defaultSpeed;
            currentAcceleration = _defaultAcceleration;
            _collisionDamage = _defaultCollisionDamage;

            for (int i = 0; i < _defaultHealth; i++) {
                this._children.Add(_childPool.Get());
                var child = this._children[i];
                float yOffset = (i < 1) ? -1 * this._geminiPositionOffset : this._geminiPositionOffset;
                Vector2 position = new Vector2(this.Position.X, this.Position.Y + yOffset);

                child.Initialize(new Queue<iEnemyAction>(), null, new WaitSeconds(200), position);
                child.SetParent(this, _geminiPositionOffset, _orbitingVelocity);
                child.SetWeapon(_weaponCooldown, _geminiMissileDamage, _missileSpeed);
            }
        }
        public override void Reserve() => Pool.Release(this);

        protected override void SubReserve() {
            base.SubReserve();

            int childCount = _children.Count;

            for (int i = 0; i < childCount; i++)
            {
                EnemyGeminiChild child = _children[i];
                
                if(!child.IsDead)
                    child.Reserve();
            }

            _children.Clear();
        }
        public override void OnCollision(MonoEntity other)
        {
            //Do Nothing
        }
        #endregion    
    }
}