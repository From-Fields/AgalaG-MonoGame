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
        private readonly Transform _spawnerTransform;
        private static readonly Vector2[] spawnPoints = new Vector2[1];
        Texture2D _bulletPrefab;

        public DefaultWeapon(Transform spawnerTransform, string shooter = null)  
            : base(spawnPoints, 999, shooter, speed) 
        {
            _spawnerTransform = spawnerTransform;
            spawnPoints[0] = new Vector2(0, 10f);
            _bulletPrefab = Prefabs.GetTextureOfType<Bullet>();
        }

        protected override void isEmpty() { }

        public override void Shoot()
        {
            Vector2 direction = (string.Compare(_shooter, "Player") == 0) ? new Vector2(0, -1) : new Vector2(0, 1);
            _ = new Bullet(spawnPoints[0] + _spawnerTransform.position, 1, direction, _speed, _bulletPrefab, _shooter);
        }
    }
}
