using System;
using Microsoft.Xna.Framework;

namespace agalag.game
{
    public class WaveHazard : iWaveUnit
    {
        public Action<iWaveUnit> onUnitReleased { get; set; }

        private Hazard _hazard;
        private bool _rotate;
        private float _speed, _rotationSpeed;
        private uint _damage, _health, _maxBounces;
        private Vector2 _position, _direction, _scale;
        private Rectangle _levelBounds;

        public WaveHazard(
            Hazard hazard, Vector2 position, Vector2 direction, Rectangle levelBounds,
            bool rotate = true, float speed = 750, float rotationSpeed = 1, 
            uint damage = 1, uint health = 1, uint maxBounces = 0,  
            Vector2? scale = null, Action<iWaveUnit> onUnitReleased = null)
        {
            this.onUnitReleased = onUnitReleased;
            _hazard = hazard;
            _rotate = rotate;
            _speed = speed;
            _rotationSpeed = rotationSpeed;
            _damage = damage;
            _health = health;
            _maxBounces = maxBounces;
            _position = position;
            _direction = direction;
            _scale = (scale.HasValue) ? scale.Value : new Vector2(0.6f, 0.6f);
            _levelBounds = levelBounds;
        }

        public void ExecuteTimeoutAction() { 
            onUnitReleased?.Invoke(this);
        }

        public void Initialize() => _hazard.Initialize(
            _position, _direction, _levelBounds, _speed, _damage, _health, _rotate, _rotationSpeed, _scale, _maxBounces
        );

        public void Reserve() => _hazard.ReserveToPool();
    }
}