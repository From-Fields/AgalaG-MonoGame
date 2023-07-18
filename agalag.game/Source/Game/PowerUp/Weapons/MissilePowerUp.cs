using agalag.engine;
using agalag.engine.content;
using Microsoft.Xna.Framework.Graphics;

namespace agalag.game
{
    public class MissilePowerUp : WeaponPowerUp
    {
        public override Texture2D Sprite => Prefabs.GetSprite("missile");

        protected override Weapon GetWeapon(Transform transform, EntityTag tag) => new MissileWeapon(transform, tag);
    }
}