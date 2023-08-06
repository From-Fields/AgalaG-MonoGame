using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using agalag.engine;
using agalag.engine.content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace agalag.game {
    public abstract class Weapon {
        protected Vector2[] _spawnPoints;
        protected int _maxAmmunition = 999;
        protected int _currentAmmunition;
        protected float _speed = 2f;
        protected float _cooldown = 0.1f;
        protected EntityTag _shooter;

        protected int _damage = 1;
        protected bool _canShoot = true;

        protected readonly Transform _spawnerTransform;
        protected Texture2D _bulletPrefab;

        public Action onShoot;

        public virtual Sprite WeaponIcon => Prefabs.GetSprite<Bullet>();
        public virtual string AmmoToString => _currentAmmunition.ToString();

        protected Weapon(Vector2[] spawnPoints, int maxAmmunition, Transform spawnerTransform, EntityTag shooter = 0, float speed = 2f) 
            : base() 
        {
            _spawnPoints = spawnPoints;
            _speed = speed;
            _maxAmmunition = maxAmmunition;
            _currentAmmunition = _maxAmmunition;
            _shooter = shooter;
            _spawnerTransform = spawnerTransform;

            _canShoot = true;
        }

        public abstract bool isEmpty();
        public abstract void Shoot();

        protected void StartCooldown()
        {
            if (_cooldown <= 0)
                return;

            _canShoot = false;
            engine.routines.RoutineManager.Instance.CallbackTimer(this._cooldown, OnCooldownEnd);
        }

        private void OnCooldownEnd() => _canShoot = true;
    }
}
