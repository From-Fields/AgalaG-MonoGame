using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using agalag.engine;
using Microsoft.Xna.Framework;

namespace agalag.game {
    public abstract class Weapon {
        protected Vector2[] _spawnPoints;
        protected int _maxAmmunition = 999;
        protected int _currentAmmunition;
        protected float _speed = 2f;
        protected float _cooldown = 0.1f;
        protected EntityTag _shooter;

        protected Weapon(Vector2[] spawnPoints, int maxAmmunition, EntityTag shooter = 0, float speed = 2f) : base() {
            _spawnPoints = spawnPoints;
            _speed = speed;
            _maxAmmunition = maxAmmunition;
            _shooter = shooter;
        }

        protected abstract void isEmpty();
        public abstract void Shoot();
    }
}
