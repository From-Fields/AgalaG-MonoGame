using agalag.engine.content;
using Microsoft.Xna.Framework.Graphics;

namespace agalag.game
{
    public class ShieldPowerUp : PowerUp
    {
        public override Texture2D Sprite => Prefabs.GetTextureOfType<ShieldPowerUp>();
        public override bool IsInstant => false;

        public override void OnPickup(Player player) {
            this._player = player;
        }
        public override int OnTakeDamage(int damage, int playerHealth) {
            if(damage <= 0)
                return damage;

            EndPowerUp();
            return damage - 1;
        }
        public override void OnEnd() {
            _player.PlaySoundOneShot(Prefabs.GetSoundOfType<ShieldPowerUp>().CreateInstance(), AudioGroup.SFX);
            _player.RemovePowerUp(this);
        }
    }
}