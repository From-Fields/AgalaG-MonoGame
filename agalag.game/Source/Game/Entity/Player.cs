using System;
using System.Collections.Generic;
using System.Diagnostics;
using agalag.engine;
using agalag.game.input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace agalag.game
{
    public class Player : Entity
    {
        //Attributes
        public bool isDead { get; private set; } = false;
        private int _maxHealth = 3;
        private int _currentHealth;
        
        public Weapon _currentWeapon;
        private Weapon _defaultWeapon = null;

        public float currentSpeed;
        private float _defaultSpeed = 10;

        public float currentAcceleration;
        private float _defaultAcceleration = 10;

        public List<iPowerUp> powerUps = new List<iPowerUp>();

        public Action onDeath;

        //References
        private GameTime _gameTime;
        private FixedFrameTime _fixedGameTime;
        private InputHandler _inputHandler;

        //Input Variables
        private Vector2 _movement;

        public int MaxHealth => _maxHealth;
        
        //Constructors
        public Player(Player player, Vector2 position):
            this(player._sprite.Texture, position, player.Transform.scale, player.Transform.rotation, player.Collider) { }
        public Player(Texture2D sprite, Vector2 position): 
            this(sprite, position, Vector2.One, 0, new RectangleCollider(new Point(72, 64), offset: new Point(0, 4))) { }
        
        public Player(Texture2D sprite, Vector2 position, Vector2 scale, float rotation = 0f, iCollider collider = null) 
            : base(sprite, position, scale, rotation, collider) 
        {
            SetTag(EntityTag.Player);
            _transform.simulate = true;
            _movement = Vector2.Zero;
            _defaultWeapon = new DefaultWeapon(_transform, EntityTag.PlayerBullet);

            //Updating Current Stuff
            currentAcceleration = _defaultAcceleration;
            currentSpeed = _defaultSpeed;
            _currentWeapon = _defaultWeapon;
            _currentHealth = _maxHealth;
            //Controls
            _inputHandler = InputHandler.Instance;
        }

        //Methods
        public void SwitchWeapon(Weapon newWeapon) 
        {
            this._currentWeapon = newWeapon;
        }

        public void SwitchToDefaultWeapon()
        {
            SwitchWeapon(_defaultWeapon);
        }

        public void AddPowerUp(iPowerUp newPowerUp) 
        {
            newPowerUp.OnPickup(this);

            if(newPowerUp.IsInstant)
                return;

            if(!this.powerUps.Contains(newPowerUp)) 
            {
                this.powerUps.Add(newPowerUp);
            }
        }
        public void RemovePowerUp(iPowerUp powerUp) 
        {
            this.powerUps.Remove(powerUp);
        }
        public void Heal(int amount) 
        {
            this._currentHealth = Math.Clamp(_currentHealth + amount, 0, _maxHealth);
        }
        
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
            if (_currentWeapon.isEmpty())
            {
                SwitchToDefaultWeapon();
            }
        }
        
        public override void TakeDamage(int damage)
        {
            int _damage = damage;

            for (int i = 0; i < powerUps.Count; i++) {
                _damage = powerUps[i].OnTakeDamage(_damage, _currentHealth); 
            }

            this._currentHealth = Math.Clamp(_currentHealth - _damage, 0, _maxHealth);

            //Debug.WriteLine((_currentHealth + damage) + "-" + damage + "=" + _currentHealth);
            if(_currentHealth == 0)
                Die();
        }        
        
        public override void Die()
        {
            //Debug.WriteLine("NANI");
            bool die = true;    

            for (int i = 0; i < powerUps.Count; i++)
                die = powerUps[i].OnDeath();

            if(!die)
                return;

            this.SetActive(false);
            this.isDead = true;
            this.onDeath?.Invoke();
        }

        //MonoEntity
        public override void Draw(SpriteBatch spriteBatch)
        {
            if(this._sprite != null) {
                this._sprite.Draw(Transform, spriteBatch);
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
        #endregion
    }
}