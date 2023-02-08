using System.Collections.Generic;
using System.Diagnostics;
using agalag.engine;
using agalag.game;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace agalag.test
{
    public class TestEnemy : Enemy
    {
        public TestEnemy(Texture2D sprite, Vector2 position, Vector2 scale, Entity target, float rotation = 0, iCollider collider = null) : 
        base(sprite, position, scale, rotation, collider)
        {
            _defaultSpeed = 10f;
            _defaultAcceleration = 10f;
            
            currentSpeed = _defaultSpeed;
            currentAcceleration = _defaultAcceleration;

            Queue<iEnemyAction> queue = new Queue<iEnemyAction>();
            queue.Enqueue(new MoveTowards(1, 1, 1f, 180, 10f, new Vector2(350, 180)));
            queue.Enqueue(new Shoot(2));
            queue.Enqueue(new MoveTowards(5, 1, 0.5f, 40, 1f, target));

            Initialize(queue, new WaitSeconds(4), new WaitSeconds(1), this.position);
        }

        public override int health => 0;

        public override Vector2 currentVelocity => _transform.velocity;
        public override Vector2 position => _transform.position;

        public override void Die()
        {
            Debug.WriteLine("OMAEWA MOU SHINDEIRU");
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
            Debug.WriteLine("OWIE " + damage);
        }

        public override void OnCollision(MonoEntity other)
        {
            Debug.WriteLine("COLLIDED!");
        }

        public override void FixedUpdate(GameTime gameTime, FixedFrameTime fixedGameTime)
        {
            base.FixedUpdate(gameTime, fixedGameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            _sprite?.Draw(_transform, spriteBatch);
        }
    }
}