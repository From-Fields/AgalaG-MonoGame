using agalag.engine;

namespace agalag.game
{
    public abstract class WeaponPowerUp: PowerUp
    {
        protected abstract Weapon GetWeapon(Transform transform, EntityTag tag);
        public override bool IsInstant => true;

        public override void OnPickup(Player player) 
        {
            player.SwitchWeapon(GetWeapon(player.Transform, EntityTag.PlayerBullet));
        }
        public override void OnEnd() { }  // Do Nothing
    }
}