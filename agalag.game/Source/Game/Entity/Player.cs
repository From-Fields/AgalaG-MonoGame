using System;
using System.Collections.Generic;
using System.Diagnostics;
using agalag.engine;
using agalag.engine.routines;
using agalag.game.input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace agalag.game
{
    public class Player : Entity
    {
        //Attributes
        public bool isDead { get; private set; } = false;
        private int _maxHealth = 3;
        private int _currentHealth;
        private bool _isInvulnerable = false;
        private float _frameAccumulator;
        private int _shieldCount = 0;

        public int Shield
        {
            get { return _shieldCount; }
            set { _shieldCount = value; }
        }
        
        public Weapon _currentWeapon;
        private Weapon _defaultWeapon = null;

        public float currentSpeed;
        private float _defaultSpeed = 10;

        public float currentAcceleration;
        private float _defaultAcceleration = 10;

        public List<iPowerUp> powerUps = new List<iPowerUp>();

        public Action<int> onLifeChange;
        public Action<int> onShieldChange;
        public Action<string> onWeaponShoot;
        public Action<Sprite> onNewWeapon;
        public Action onDeath;

        //References
        private GameTime _gameTime;
        private FixedFrameTime _fixedGameTime;
        private InputHandler _inputHandler;

        //Input Variables
        private Vector2 _movement;
        private EntityAudioManager _audioManager;

        public int MaxHealth => _maxHealth;
        
        //Constructors
        public Player(Player player, Vector2 position, EntityAudioManager audioManager = null, bool active = false):
            this(player._sprite.Texture, position, player.Transform.scale, player.Transform.rotation, player.Collider, player._audioManager, active) { }
        public Player(Texture2D sprite, Vector2 position, EntityAudioManager audioManager = null, bool active = false): 
            this(sprite, position, Vector2.One, 0, new RectangleCollider(new Point(72, 64), offset: new Point(0, 4)), audioManager, active) { }
        
        public Player(
            Texture2D sprite, Vector2 position, Vector2 scale, 
            float rotation = 0f, iCollider collider = null, EntityAudioManager audioManager = null, bool active = false
        ) 
            : base(sprite, position, scale, rotation, collider) 
        {
            SetTag(EntityTag.Player);
            _transform.simulate = true;
            _movement = Vector2.Zero;
            _defaultWeapon = new DefaultWeapon(_transform, EntityTag.PlayerBullet);
            SwitchToDefaultWeapon();

            _isInvulnerable = false;
            _frameAccumulator = 0;

            //Updating Current Stuff
            currentAcceleration = _defaultAcceleration;
            currentSpeed = _defaultSpeed;
            _currentWeapon = _defaultWeapon;
            _currentHealth = _maxHealth;
            //Controls
            _inputHandler = InputHandler.Instance;

            _audioManager = audioManager;
            
            if(active)
                PlaySound(EntitySoundType.Movement, looping: true);

            this.SetActive(active);
        }

        //Methods
        public void SwitchWeapon(Weapon newWeapon) 
        {
            this._currentWeapon = newWeapon;
            this._currentWeapon.onShoot += PlayShotSound;
            onNewWeapon?.Invoke(_currentWeapon.WeaponIcon);
            onWeaponShoot?.Invoke(_currentWeapon.AmmoToString);
        }

        public void SwitchToDefaultWeapon()
        {
            SwitchWeapon(_defaultWeapon);
        }

        public void AddPowerUp(iPowerUp newPowerUp) 
        {
            newPowerUp.OnPickup(this);

            if(newPowerUp.IsInstant) {
                PlaySound(EntitySoundType.PowerUp);
                return;
            }

            if(!this.powerUps.Contains(newPowerUp)) 
            {
                this.powerUps.Add(newPowerUp);
                PlaySound(EntitySoundType.PowerUp);
            }

            onShieldChange?.Invoke(Shield);
        }
        public void RemovePowerUp(iPowerUp powerUp) 
        {
            this.powerUps.Remove(powerUp);
        }

        public void Heal(int amount) 
        {
            this._currentHealth = Math.Clamp(_currentHealth + amount, 0, _maxHealth);
            onLifeChange?.Invoke(_currentHealth);
        }

        private void SetInvulnerability(bool invulnerable)
        {
            _isInvulnerable = invulnerable;

            if(invulnerable)
                RoutineManager.Instance.CallbackTimer(1.5f, () => SetInvulnerability(false));
        }

        // Sound
        private void PlaySound(EntitySoundType soundType, Vector2? position = null, AudioListener listener = null, bool looping = false) 
            => _audioManager.PlaySound(soundType, position, listener, looping);
        private void PlayShotSound() => _audioManager.PlaySound(EntitySoundType.Shot);
        private void StopSound(EntitySoundType soundType) => _audioManager.StopSound(soundType);
        public void PlaySoundOneShot(SoundEffectInstance instance, AudioGroup audioGroup, Vector2? position = null, AudioListener listener = null) 
            => _audioManager.PlaySoundOneShot(instance, audioGroup, position, listener);
        
        #region InterfaceImplementation
        //Entity
        public override int Health => _currentHealth;
        public override Vector2 Position => _transform.position;
        public override Vector2 CurrentVelocity => _transform.velocity;

        public override void Move(Vector2 direction, float speed, float acceleration)
        {
            if(direction != Vector2.Zero)
                direction.Normalize();

            speed *= 100;

            Vector2 movementVector = direction * speed;

            _transform.velocity = Vector2.Lerp(_transform.velocity, movementVector, _fixedGameTime.frameTime * acceleration);
        }
        public override void Stop() => _transform.velocity = Vector2.Zero;
        public override void Shoot()
        {
            if (_currentWeapon == null) 
                throw new Exception("Current Weapon cannot be null");

            _currentWeapon.Shoot();
            onWeaponShoot?.Invoke(_currentWeapon.AmmoToString);
            if (_currentWeapon.isEmpty())
            {
                SwitchToDefaultWeapon();
            }
        }
        
        public override void TakeDamage(int damage)
        {
            if(_isInvulnerable)
                return;

            int _damage = damage;

            for (int i = 0; i < powerUps.Count; i++) {
                _damage = powerUps[i].OnTakeDamage(_damage, _currentHealth); 
            }
            onShieldChange?.Invoke(Shield);

            if(_damage == 0)
                return;

            PlaySound(EntitySoundType.Damage);

            this._currentHealth = Math.Clamp(_currentHealth - _damage, 0, _maxHealth);
            onLifeChange?.Invoke(_currentHealth);

            //Debug.WriteLine((_currentHealth + damage) + "-" + damage + "=" + _currentHealth);
            if(_currentHealth == 0)
                Die();
            SetInvulnerability(true);
        }        
        
        public override void Die()
        {
            //Debug.WriteLine("NANI");
            bool die = true;    

            for (int i = 0; i < powerUps.Count; i++)
                die = powerUps[i].OnDeath();

            if(!die)
                return;

            PlaySound(EntitySoundType.Death);
            StopSound(EntitySoundType.Movement);

            this.SetActive(false);
            this.isDead = true;
            this.onDeath?.Invoke();
        }

        //MonoEntity
        public override void Draw(SpriteBatch spriteBatch)
        {
            if(this._sprite != null) {
                float alpha = _sprite.Opacity;

                if(_isInvulnerable) {
                    if(_frameAccumulator >= 5) {
                        _frameAccumulator = 0;
                        alpha = (alpha == 1) ? 0.5f : 1;
                    }
                    _frameAccumulator++;
                }
                else
                    alpha = 1;

                this._sprite.Draw(Transform, spriteBatch, opacity: alpha);
            }
        }
        
        public override void FixedUpdate(GameTime gameTime, FixedFrameTime fixedFrameTime)
        {
            this._fixedGameTime = fixedFrameTime;
                        
            Move(_movement, currentSpeed, currentAcceleration);
        }
        
        public override void Update(GameTime gameTime)
        {
            if(isDead)
                return;

            for (int i = 0; i < powerUps.Count; i++)
                powerUps[i].OnTick(gameTime);

            _movement = (_inputHandler.HasMovement) ? _inputHandler.GetMovement() : Vector2.Zero;
            if(_inputHandler.GetShoot())
                Shoot();

            if (_inputHandler.PressF2())
            {
                SwitchWeapon(new MissileWeapon(_transform, EntityTag.PlayerBullet));
            }
            else if (_inputHandler.PressF3())
            {
                SwitchWeapon(new TripleMachineGun(_transform, EntityTag.PlayerBullet));
            }

            this._gameTime = gameTime;
        }
        public override void OnCollision(MonoEntity other)  { }
        protected override void SubDispose()
        {
            base.SubDispose();
            _audioManager.Clear();
        }
        #endregion
    }
}   