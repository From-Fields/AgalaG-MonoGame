using agalag.engine;
using agalag.engine.content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace agalag.game
{
    class Explosion : MonoEntity
    {
        private int _damage = 1;
        private int _animationSpeed = 4; // frames por second?
        private int timePassed = 0;
        private int _currentFrame = 0;

        private EntityAudioManager _audioManager;

        private static readonly Texture2D _texture = Prefabs.GetTextureOfType<Explosion>();

        private Vector2 _offset = new Vector2(852, 0);

        public Explosion(Vector2 position) : base (_texture, position, scale: Vector2.One * .75f, 
            new RectangleCollider(new Point(284, 284), solid: false), layer: Layer.Objects)
        {
            _audioManager = new EntityAudioManager(deathSound: Prefabs.GetSoundOfType<Explosion>());
            _audioManager.PlaySound(EntitySoundType.Death);
            Transform.simulate = false;
        }

        private void DestroySelf()
        {
            Dispose();
        }

        #region Interface Implementation
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (_sprite == null) return;

            _sprite.Draw(_transform.position + _offset ,_transform, spriteBatch, new Rectangle(_currentFrame * 284, 0, 284, 284));
        }

        public override void Update(GameTime gameTime)
        {
            _transform.scale += Vector2.One * .03f;
            _offset.X += 34f;

            timePassed++;
            if (timePassed > _animationSpeed)
            {
                _currentFrame++;
                if (_currentFrame >= 9)
                {
                    DestroySelf();
                }

                timePassed = 0;
            }
        }
        public override void OnCollision(MonoEntity other)
        {
            if (other is Entity entity)
            {
                if (other.Tag == EntityTag.Enemy)
                {
                    entity.TakeDamage(_damage);
                }
            }
        }
        #endregion
    }
}
