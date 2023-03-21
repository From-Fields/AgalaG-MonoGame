using System;
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
        private string _shooter;

        private readonly float _destroyTime = 2f;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        /// <param name="damage"></param>
        /// <param name="direction"></param>
        /// <param name="speed"></param>
        /// <param name="shooter"></param>
        /// <param name="sprite"></param>
        /// <param name="rotation"></param>
        /// <param name="collider"></param>

        public Bullet(Vector2 position, int damage, Vector2 direction, float speed, Texture2D sprite, string shooter = null, float rotation = 0f, iCollider collider = null)
            : this(position, Vector2.One, damage, direction, speed, sprite, 
                shooter, rotation, 
                collider ?? new RectangleCollider(new Point(32, 60), null, new Point(0, 4))
        ) { }

        public Bullet(Vector2 position, Vector2 scale, int damage, Vector2 direction, float speed, Texture2D sprite, string shooter = null, float rotation = 0f, iCollider collider = null) 
            : base(sprite, position, scale, rotation, 
                collider ?? new RectangleCollider(new Point(32, 60), null, new Point(0, 4)), 
                layer: Layer.Objects
        ) {
            if (direction != Vector2.Zero)
                direction.Normalize();
         
            // _speed = speed;
            // _direction = direction;

            _damage = damage;
            _shooter = shooter ?? "unknown";
            
            _transform.drag = 0;
            _transform.velocity = (direction * speed * 100);
            _transform.simulate = true;
            
            RoutineManager.Instance.CallbackTimer(_destroyTime, DestroySelf);
        }

        // public void Move(Vector2 direction, float speed, float acceleration = 10f)
        // {
        //     if (direction != Vector2.Zero)
        //         direction.Normalize();

        //     _transform.position += direction * speed;
        // }

        private void DestroySelf() {
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
            //Debug.WriteLine("bullet: Colisão!");

            if (other is Entity entity)
            {
                //Debug.WriteLine("Entidade encontrada!");
                if (!string.IsNullOrEmpty(entity.Tag) && string.Compare(entity.Tag, _shooter) != 0)
                {
                    //Debug.WriteLine("alvo encontrado!");
                    entity.TakeDamage(_damage);
                    DestroySelf();
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            // Move(_direction, _speed);
        }
        #endregion
    }
}
