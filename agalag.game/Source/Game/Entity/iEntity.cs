using agalag.engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace agalag.game
{
    public interface iEntity
    {
        public int Health { get; }
        public Vector2 Position { get; }
        public Vector2 CurrentVelocity { get; }

        public void Move (Vector2 direction, float speed, float acceleration);
        public void Shoot();
        public void Stop();
        public void TakeDamage (int damage);
        public void Die();
    }

    public abstract class Entity : MonoEntity, iEntity
    {
        public abstract int Health { get; }
        public abstract Vector2 Position { get; }
        public abstract Vector2 CurrentVelocity { get; }

        public abstract void Move (Vector2 direction, float speed, float acceleration);
        public abstract void Shoot();
        public abstract void Stop();
        public abstract void TakeDamage (int damage);
        public abstract void Die();
        
        public Entity(Texture2D sprite, Vector2 position, Vector2 scale, float rotation = 0f, iCollider collider = null) 
            : base(sprite, position, scale, rotation, collider, layer: Layer.Entities) { }
    }
}