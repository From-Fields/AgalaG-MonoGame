using System;
using System.Collections.Generic;
using System.Diagnostics;
using agalag.engine;
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
        
        public Weapon currentWeapon;
        private Weapon _defaultWeapon = null;

        public float currentSpeed;
        private float _defaultSpeed = 500;
        public List<PowerUp> powerUps;

        //References
        private GameTime _gameTime;
        private GameTime _fixedGameTime;
        private InputHandler _inputHandler;

        //Properties
        public override int health => _currentHealth;

        //Constructors
        public Player(Texture2D sprite, Vector2 position, Vector2 scale, float rotation = 0f, Collider collider = null) 
            : base(sprite, position, scale, rotation, collider) 
        {
            currentSpeed = _defaultSpeed;
            currentWeapon = _defaultWeapon;
            _currentHealth = _maxHealth;
            _inputHandler = InputHandler.Instance;
        }

        //Methods
        public void SwitchWeapon(Weapon newWeapon) 
        {
            this.currentWeapon = newWeapon;
        }
        public void AddPowerUp(PowerUp newPowerUp) 
        {
            if(!this.powerUps.Contains(newPowerUp)) 
            {
                this.powerUps.Add(newPowerUp);
            }
        }
        public void RemovePowerUp(PowerUp powerUp) 
        {
            this.powerUps.Remove(powerUp);
        }
        public void Heal(int amount) 
        {
            this._currentHealth = Math.Clamp(_currentHealth + amount, 0, _maxHealth);
        }
        
        #region InterfaceImplementation
        //Entity
        public override void Move(Vector2 direction, float speed)
        {
            direction.Normalize();

            Vector2 destination = _transform.position + direction * speed;

            _transform.position = Vector2.Lerp(_transform.position, destination, (float)_gameTime.ElapsedGameTime.TotalSeconds);
        }

        public override void Shoot()
        {
            Debug.WriteLine("PEW");
        }

        public override void TakeDamage(int damage)
        {
            _currentHealth = Math.Clamp(_currentHealth - damage, 0, _maxHealth);

            if(_currentHealth == 0)
                Die();
        }
        
        public override void Die()
        {
            Debug.WriteLine("NANI");
            this.isDead = true;
        }

        //MonoEntity
        public override void Draw(SpriteBatch spriteBatch)
        {
            if(this._sprite != null) {
                spriteBatch.Draw(
                    this._sprite,
                    this._transform.position,
                    Color.White
                );
            }
        }

        public override void FixedUpdate(GameTime gameTime)
        {
            this._fixedGameTime = gameTime;
        }

        public override void Update(GameTime gameTime)
        {
            if(!isDead) {
                if(_inputHandler.HasMovement)
                    Move(_inputHandler.GetMovement(), currentSpeed);
                if(_inputHandler.GetShoot())
                    Shoot();
            }

            this._gameTime = gameTime;
        }
        #endregion
    }
}