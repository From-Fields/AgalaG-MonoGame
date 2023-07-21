using agalag.engine;
using agalag.engine.content;
using Microsoft.Xna.Framework.Graphics;

namespace agalag.game
{
    public class TripleMachineGunPowerUp : WeaponPowerUp
    {
        public override Texture2D Sprite => Prefabs.GetTextureOfType<TripleMachineGunPowerUp>();

        protected override Weapon GetWeapon(Transform transform, EntityTag tag) => new TripleMachineGun(transform, tag);
    }
}