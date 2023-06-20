using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace agalag.game
{
    public interface iPowerUp
    {
        public bool IsInstant { get; }
        
        public void OnPickup(Player player);
        public void OnTick(GameTime gameTime);
        public int OnTakeDamage(int damage, int playerHealth);
        public bool OnDeath();
        public void OnEnd();
    }
    
    public abstract class PowerUp: iPowerUp 
    {
        protected Player _player;
        
        public abstract Texture2D Sprite { get; }
        public abstract bool IsInstant { get; }

        protected void EndPowerUp() => OnEnd();

        public virtual void OnTick(GameTime gameTime) {}
        public virtual int OnTakeDamage(int damage, int playerHealth) => damage;
        public virtual bool OnDeath() => true;

        public abstract void OnPickup(Player player);
        public abstract void OnEnd();
    }
}