using System;
using System.Collections.Generic;
using System.Diagnostics;
using agalag.engine;
using agalag.engine.pool;
using agalag.game;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace agalag.test
{
    public class TestEnemy : Enemy<TestEnemy>
    {
        public TestEnemy(Texture2D sprite, Vector2 position, Vector2 scale, Entity target, Rectangle levelBounds, float rotation = 0, iCollider collider = null) : 
        base(sprite, position, scale, rotation, collider)
        {
            _defaultSpeed = 10f;
            _defaultAcceleration = 10f;
            
            currentSpeed = _defaultSpeed;
            currentAcceleration = _defaultAcceleration;

            Queue<iEnemyAction> queue = new Queue<iEnemyAction>();
            queue.Enqueue(new MoveTowards(new Vector2(350, 180), 1, maximumAngle: 180));
            queue.Enqueue(new Shoot(2));
            queue.Enqueue(new MoveTowards(target, 5, trackingSpeed: 0.5f));

            Initialize(queue, new WaitSeconds(4), new WaitSeconds(1), this.Position, levelBounds);
        }
        public TestEnemy(TestEnemy prefab):
        this(prefab._sprite.Texture, prefab.Transform.position, prefab.Transform.scale, null, new Rectangle(0 ,0, 1920, 1080), prefab.Transform.rotation, prefab.Collider)
        {}

        public override int Health => 0;

        public override Vector2 CurrentVelocity => _transform.velocity;
        public override Vector2 Position => _transform.position;

        public override void Die() => Debug.WriteLine("OMAEWA MOU SHINDEIRU");
        public override void Move(Vector2 direction, float speed, float acceleration)
        {
            speed *= 100;

            float frameTime = FixedUpdater.FixedFrameTime.frameTime;
            _transform.velocity = Vector2.Lerp(_transform.velocity, direction * speed, frameTime * acceleration);
        }
        public override void Stop() => _transform.velocity = Vector2.Zero;
        public override void Shoot() => Debug.WriteLine("FIRING MAH LAZOR");
        public override void TakeDamage(int damage )=> Debug.WriteLine("OWIE " + damage);
        public override void OnCollision(MonoEntity other) => Debug.WriteLine("COLLIDED!");

        public override void FixedUpdate(GameTime gameTime, FixedFrameTime fixedGameTime) => base.FixedUpdate(gameTime, fixedGameTime);

        public override void Draw(SpriteBatch spriteBatch) => _sprite?.Draw(_transform, spriteBatch);

        protected override void SubInitialize() { }

        public override Action<TestEnemy> onGetFromPool => null;
        public override iObjectPool<TestEnemy> Pool => EntityPool<TestEnemy>.Instance.Pool;
        protected override void ReserveToPool() => Pool.Release(this);
        public override TestEnemy OnCreate() => new TestEnemy(EntityPool<TestEnemy>.Instance.Prefab);
    }
}