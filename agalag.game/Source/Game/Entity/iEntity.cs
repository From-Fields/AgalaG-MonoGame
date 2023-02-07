using agalag.engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace agalag.game
{
    public interface iEntity
    {
        public int health { get; }

        public void Move (Vector2 direction, float speed);
        public void Shoot();
        public void TakeDamage (int damage);
        public void Die();
    }

    public abstract class Entity : MonoEntity, iEntity
    {
        public abstract int health { get; }

        public abstract void Move (Vector2 direction, float speed);
        public abstract void Shoot();
        public abstract void TakeDamage (int damage);
        public abstract void Die();
        
        public Entity(Texture2D sprite, Vector2 position, Vector2 scale, float rotation = 0f, iCollider collider = null) 
            : base(sprite, position, scale, rotation, collider) { }
    }
}