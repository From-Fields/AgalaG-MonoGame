using agalag.engine;
using agalag.engine.content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace agalag.game
{
    public class DefaultWeapon : Weapon
    {
        private const float speed = 15f;
        private static readonly Vector2[] _defaultSpawnPoints = new Vector2[1];

        private Vector2 _direction = new Vector2(0, -1);

        public DefaultWeapon(Transform spawnerTransform, EntityTag shooter = 0)  
            : base(_defaultSpawnPoints, maxAmmunition: 999, spawnerTransform, shooter, speed) 
        {
            _defaultSpawnPoints[0] = new Vector2(0, -15f);
            _bulletPrefab = Prefabs.GetTextureOfType<Bullet>();
            _cooldown = .3f;
        }

        public override bool isEmpty() { return false; }

        public override void Shoot()
        {
            if(!_canShoot)
                return;

            StartCooldown();

            Vector2 direction = (_shooter == EntityTag.PlayerBullet) ? _direction : new Vector2(0, 1);
            float rotation = (direction == new Vector2(0, -1)) ? 0 : MathHelper.ToRadians(180);
            _ = new Bullet(_spawnPoints[0] + _spawnerTransform.position, _damage, direction, _speed, _bulletPrefab, _shooter, rotation);
            onShoot?.Invoke();
        }

        public void SetAttributes(Vector2[] spawnPoints = null, Vector2? direction = null, int maxAmmunition = -1, float speed = -1, float cooldown = -1, int damage = -1, Texture2D bulletTexture = null) {
            this._direction = direction.HasValue ? direction.Value : new Vector2(0, -1);
            this._maxAmmunition = (maxAmmunition != -1) ? maxAmmunition : _maxAmmunition;
            this._speed = (speed != -1) ? speed : _speed;
            this._cooldown = (cooldown != -1) ? cooldown : _cooldown;
            this._damage = (damage != -1)? damage : _damage;
            this._spawnPoints = (spawnPoints != null) ? spawnPoints : new[] { Vector2.Zero };
            this._bulletPrefab = (bulletTexture != null) ? bulletTexture : _bulletPrefab;
        }
    }
}
