using agalag.engine;
using agalag.engine.content;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace agalag.game
{
    class MissileWeapon : Weapon
    {
        private const float speed = 6f;
        private static readonly Vector2[] spawnPoints = new Vector2[1];

        public MissileWeapon(Transform spawnerTransform, EntityTag shooter = EntityTag.None) 
            : base(spawnPoints, maxAmmunition: 15, spawnerTransform, shooter, speed)
        {
            spawnPoints[0] = new Vector2(0f, 0f);
            _bulletPrefab = Prefabs.GetSprite("missile");
            _damage = 2;
            _cooldown = .8f;
        }

        public override void Shoot()
        {
            if (!_canShoot)
                return;

            StartCooldown();

            Vector2 direction = (_shooter == EntityTag.PlayerBullet) ? new Vector2(0, -1) : new Vector2(0, 1);
            float rotation = (direction == new Vector2(0, -1)) ? 0 : MathHelper.ToRadians(180);
            _ = new Bullet(_spawnPoints[0] + _spawnerTransform.position, Vector2.One * 1.8f, _damage, direction, 
                _speed, _bulletPrefab, _shooter, rotation, explosion: true, life: 1.2f);
            _currentAmmunition--;
        }

        public override bool isEmpty()
        {
            return _currentAmmunition <= 0;
        }
    }
}
