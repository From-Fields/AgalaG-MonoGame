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
    class TripleMachineGun : Weapon
    {
        private static readonly Vector2[] spawnPoints = new Vector2[3] { 
            new Vector2(0, -25f), 
            new Vector2(35f, -15f), 
            new Vector2(-35f, -15f) 
        };
        private const float speed = 21f;

        public TripleMachineGun(Transform spawnerTransform, EntityTag shooter = EntityTag.None) 
            : base(spawnPoints, maxAmmunition: 100, spawnerTransform, shooter, speed)
        {
            _bulletPrefab = Prefabs.GetTextureOfType<Bullet>();
            _cooldown = .1f;
        }

        public override void Shoot()
        {
            if (!_canShoot)
                return;

            StartCooldown();

            Vector2 direction = (_shooter == EntityTag.PlayerBullet) ? new Vector2(0, -1) : new Vector2(0, 1);
            float rotation = (direction == new Vector2(0, -1)) ? 0 : MathHelper.ToRadians(180);

            foreach (var spawnPoint in _spawnPoints)
            {
                _ = new Bullet(spawnPoint + _spawnerTransform.position, _damage, direction, 
                    _speed, _bulletPrefab, _shooter, rotation);
            }

            _currentAmmunition--;
            onShoot?.Invoke();
        }

        public override bool isEmpty()
        {
            return _currentAmmunition <= 0;
        }
    }
}
