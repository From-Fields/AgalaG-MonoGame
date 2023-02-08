using agalag.engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace agalag.game
{
    public interface iEntity
    {
        public int health { get; }
        public Vector2 position { get; }
        public Vector2 currentVelocity { get; }

        public void Move (Vector2 direction, float speed, float acceleration);
        public void Shoot();
        public void TakeDamage (int damage);
        public void Die();
    }

    public abstract class Entity : MonoEntity, iEntity
    {
        public abstract int health { get; }
        public abstract Vector2 position { get; }
        public abstract Vector2 currentVelocity { get; }

        public abstract void Move (Vector2 direction, float speed, float acceleration);
        public abstract void Shoot();
        public abstract void TakeDamage (int damage);
        public abstract void Die();
        
        public Entity(Texture2D sprite, Vector2 position, Vector2 scale, float rotation = 0f, iCollider collider = null) 
            : base(sprite, position, scale, rotation, collider) { }
    }
}