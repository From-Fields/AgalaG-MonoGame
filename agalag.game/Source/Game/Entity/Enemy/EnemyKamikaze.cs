using System.Collections.Generic;
using System.Diagnostics;
using agalag.engine;
using agalag.game;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace agalag.game
{
    public class EnemyKamikaze : Enemy
    {
        private int _suicideDamage = 1;

        private int _maxHealth = 1;
        private int _health;

        public EnemyKamikaze(Texture2D sprite, Vector2 position, Vector2 scale, Entity target,float rotation = 0, iCollider collider = null) : 
        base(sprite, position, scale, rotation, collider)
        { }

        public override int health => 0;

        public override Vector2 currentVelocity => _transform.velocity;
        public override Vector2 position => _transform.position;

        public override void Die()
        {
            this.Reserve();
        }

        public override void Move(Vector2 direction, float speed, float acceleration)
        {
            speed *= 100;

            float frameTime = FixedUpdater.FixedFrameTime.frameTime;
            _transform.velocity = Vector2.Lerp(_transform.velocity, direction * speed, frameTime * acceleration);
        }

        public override void Shoot()
        {
            Debug.WriteLine("FIRING MAH LAZOR");
        }

        public override void TakeDamage(int damage)
        {
            _health = System.Math.Clamp(_health - damage, 0, _maxHealth);

            if(_health == 0)
                Die();
        }

        public override void OnCollision(MonoEntity other)
        {
            if(!_isDead)
            {
                Entity otherEntity = other as Entity;
                if(otherEntity != null) 
                {
                    Die();
                    otherEntity.TakeDamage(_suicideDamage);
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void FixedUpdate(GameTime gameTime, FixedFrameTime fixedGameTime)
        {
            base.FixedUpdate(gameTime, fixedGameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            _sprite?.Draw(_transform, spriteBatch);
        }

        protected override void SubInitialize()
        {
            this.SetCollider(new RectangleCollider(new Point(82, 84)));
            _health = _maxHealth;

            _defaultSpeed = 10f;
            _defaultAcceleration = 10f;
            
            currentSpeed = _defaultSpeed;
            currentAcceleration = _defaultAcceleration;
        }
    }
}