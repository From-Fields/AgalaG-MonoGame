﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using agalag.engine;
using agalag.engine.routines;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace agalag.game {
    public class Bullet : MonoEntity {
        private int _damage;
        // private Vector2 _direction;
        // private float _speed;

        private readonly float _destroyTime = 2f;
        private readonly bool _explosion;

        private bool isDestroying = false;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        /// <param name="damage"></param>
        /// <param name="direction"></param>
        /// <param name="speed"></param>
        /// <param name="sprite"></param>
        /// <param name="shooter"></param>
        /// <param name="rotation"></param>
        /// <param name="collider"></param>
        /// <param name="life"></param>
        /// <param name="explosion"></param>
        public Bullet(Vector2 position, int damage, Vector2 direction, float speed, Texture2D sprite, EntityTag shooter = 0, float rotation = 0f, iCollider collider = null, float life = 2f, bool explosion = false)
            : this(position, Vector2.One, damage, direction, speed, sprite, 
                shooter, rotation, 
                collider ?? new RectangleCollider(new Point(32, 60), solid: false, offset: new Point(0, 4))
        ) { }

        public Bullet(Vector2 position, Vector2 scale, int damage, Vector2 direction, float speed, Texture2D sprite, EntityTag shooter = 0, float rotation = 0f, iCollider collider = null, float life = 2f, bool explosion = false) 
            : base(sprite, position, scale, rotation, 
                collider ?? new RectangleCollider(new Point(32, 60), solid: false, offset: new Point(0, 4)), 
                layer: Layer.Objects
            ) 
        {
            if (direction != Vector2.Zero)
                direction.Normalize();
         
            // _speed = speed;
            // _direction = direction;

            _damage = damage;
            _destroyTime = life;
            _explosion = explosion;
            SetTag(shooter);
            
            _transform.drag = 0;
            _transform.velocity = (direction * speed * 100);
            _transform.simulate = true;
            
            RoutineManager.Instance.CallbackTimer(_destroyTime, DestroySelf);
        }

        private void DestroySelf() {
            if (isDestroying) return;

            isDestroying = true;
            if (_explosion)
            {
                Vector2 pos = new(_transform.position.X, _transform.position.Y);
                _ = new Explosion(pos);
            }
            Dispose();
        }

        #region MonoEntity
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (_sprite == null) return;

            _sprite.Draw(Transform, spriteBatch);
        }

        public override void OnCollision(MonoEntity other)
        {
            if (other is Entity entity)
            {
                entity.TakeDamage(_damage);
                DestroySelf();
            }
        }

        public override void Update(GameTime gameTime)
        {
            // Move(_direction, _speed);
        }

        protected override void SubDispose()
        {
            base.SubDispose();
            this.DisposeCompletely();
        }
        #endregion
    }
}
