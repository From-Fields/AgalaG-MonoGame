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
        private int _damage = 1;
        private const float speed = 15f;
        private readonly Transform _spawnerTransform;
        private static readonly Vector2[] spawnPoints = new Vector2[1];
        Texture2D _bulletPrefab;

        private bool _canShoot = true;
        private Vector2 _direction = new Vector2(0, -1);

        public DefaultWeapon(Transform spawnerTransform, string shooter = null)  
            : base(spawnPoints, 999, shooter, speed) 
        {
            _spawnerTransform = spawnerTransform;
            spawnPoints[0] = new Vector2(0, 10f);
            _bulletPrefab = Prefabs.GetTextureOfType<Bullet>();
            _canShoot = true;
        }

        protected override void isEmpty() { }

        public override void Shoot()
        {
            if(!_canShoot)
                return;

            StartCooldown();

            Vector2 direction = (string.Compare(_shooter, "Player") == 0) ? new Vector2(0, -1) : new Vector2(0, 1);
            float rotation = (direction == new Vector2(0, -1)) ? 0 : 180;
            _ = new Bullet(spawnPoints[0] + _spawnerTransform.position, _damage, direction, _speed, _bulletPrefab, _shooter);
        }

         private void StartCooldown()  {
            if(_cooldown <= 0)
                return;

            _canShoot = false;
            engine.routines.RoutineManager.Instance.CallbackTimer(this._cooldown, OnCooldownEnd);
        }
        private void OnCooldownEnd() => _canShoot = true;

        public void SetAttributes(Vector2? direction = null, int maxAmmunition = -1, float speed = -1, float cooldown = -1, int damage = -1, Texture2D bulletTexture = null) {
            this._direction = direction.HasValue ? direction.Value : new Vector2(0, -1);
            this._maxAmmunition = (maxAmmunition != -1) ? maxAmmunition : _maxAmmunition;
            this._speed = (speed != -1) ? speed : _speed;
            this._cooldown = (cooldown != -1) ? cooldown : _cooldown;
            this._damage = (damage != -1)? damage : _damage;
            this._bulletPrefab = (bulletTexture != null) ? bulletTexture : _bulletPrefab;
        }
    }
}
