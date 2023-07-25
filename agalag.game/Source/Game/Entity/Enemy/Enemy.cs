using System;
using System.Collections.Generic;
using agalag.engine;
using agalag.engine.pool;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace agalag.game
{
    public abstract class Enemy<T> : Entity, iEnemy, iPoolableEntity<T> where T: Enemy<T>, iEnemy, iPoolableEntity<T>
    {
        public int score;

        //Damage
        protected int _defaultCollisionDamage = 1;
        protected int _collisionDamage;

        //Speed
        protected float currentSpeed;
        protected float _defaultSpeed;

        //Acceleration
        protected float currentAcceleration;
        protected float _defaultAcceleration;

        protected bool _isReleased = false;
        protected bool _isDead;

        protected Queue<iEnemyAction> _actionQueue;
        protected iEnemyAction _startingAction;
        protected iEnemyAction _timeoutAction;
        protected iEnemyAction _currentAction;
        
        protected Rectangle _levelBounds;

        protected EntityAudioManager _audioManager;

        protected Enemy(Texture2D sprite, Vector2 position, Vector2 scale, float rotation = 0, iCollider collider = null, EntityAudioManager audioManager = null) 
        : base(sprite, position, scale, rotation, collider) {
            SetTag(EntityTag.Enemy);
            _transform.drag = 10f;
            _transform.simulate = true;
            _audioManager = audioManager;
        }
        
        public float DesiredSpeed => currentSpeed;
        public float CurrentAcceleration => currentAcceleration;

        //Properties
        public bool IsDead => this._isDead;

        //Events
        public Action<int> onDeath;
        public Action onRelease;
        private iPowerUp _droppedItem;

        //Methods
        public void ExecuteNextAction()
        {
            if(this._actionQueue?.Count > 0)
                this.SwitchAction(this._actionQueue.Dequeue());
            else if(_currentAction != _timeoutAction)
                this.SwitchAction(_timeoutAction);
            else
                this.SwitchAction(null);
        }
        public void ExecuteStartingAction() => this.SwitchAction(this._startingAction);
        public void ExecuteTimeoutAction() => this.SwitchAction(this._timeoutAction);
        protected void SwitchAction(iEnemyAction action)
        {
            if(_isDead)
                return;

            this._currentAction?.OnFinish(this);
            this._currentAction = action;
            this._currentAction?.OnStart(this);

            if(_currentAction == null)
                Reserve();
        }
        public void Initialize(Queue<iEnemyAction> actionQueue, iEnemyAction startingAction, iEnemyAction timeoutAction, Vector2 startingPoint, Rectangle levelBounds, iPowerUp drop = null)
        {
            if(actionQueue == null || timeoutAction == null)
                throw new System.ArgumentNullException("Action queue and Timeout action may not be null");

            this._isDead = false;
            this._isReleased = false;
            this._actionQueue = actionQueue;
            this._startingAction = startingAction;
            this._timeoutAction = timeoutAction;
            this._transform.position = startingPoint;
            this._levelBounds = levelBounds;

            this._droppedItem = drop;

            SubInitialize();

            this.SetActive(true);

            if(this._startingAction != null)
                this.SwitchAction(this._startingAction);
            else
                this.ExecuteNextAction();
        }
        
        public void Reserve()
        {
            if(this._isReleased) return;

            this._actionQueue = null;
            this._startingAction = null;
            this._timeoutAction = null;

            this.onDeath = null;

            this._isDead = true;
            this._transform.Reset();
            this.SetActive(false);
            this.SubReserve();
            this.ReserveToPool();
            
            this._isReleased = true;
            this.onRelease?.Invoke();
        }

        public override void Update(GameTime gameTime)
        {
            if(_isDead || _currentAction == null)
                return;

            if(_currentAction.CheckCondition(this))
                ExecuteNextAction();

            _currentAction?.Update(this);
            SubUpdate(gameTime);
        }
        public override void FixedUpdate(GameTime gameTime, FixedFrameTime fixedGameTime)
        {
            if(_isDead || _currentAction == null)
                return;
            
            if(!_currentAction.CheckCondition(this))
                _currentAction.FixedUpdate(this);
            SubFixedUpdate(gameTime, fixedGameTime);
        }

        protected override void SubDispose() => Reserve();

        // Sound
        protected void PlayShotSound() => _audioManager.PlaySound(EntitySoundType.Shot);

        //Abstract Methods
        protected abstract void SubInitialize();
        protected abstract void ReserveToPool();
        
        //Virtual Methods
        protected virtual void SubReserve() { }
        protected virtual void SubUpdate(GameTime gameTime) { }
        protected virtual void SubFixedUpdate(GameTime gameTime, FixedFrameTime fixedGameTime) { }
        public override void OnCollision(MonoEntity other) {
            if(_isDead)
                return;

            Entity otherEntity = other as Entity;
            if(otherEntity != null) 
            {
                Die();
                otherEntity.TakeDamage(_collisionDamage);
            }
        }

        #region InterfaceImplementation

        //iEntity
        public override void Die()
        {
            onDeath?.Invoke(this.score);
        
            if(_droppedItem != null) {
                double randomX = Random.Shared.NextDouble() * 2 - 1;
                double randomY = Random.Shared.NextDouble() * 2 - 1;

                Vector2 randomDirection = new Vector2((float) randomX, (float) randomY);
                randomDirection.Normalize();

                EntityPool<PickUp>.Instance.Pool.Get().Initialize(_droppedItem, _transform.position, randomDirection, _levelBounds);
            }

            _audioManager.PlaySound(EntitySoundType.Death);
            _audioManager.StopSound(EntitySoundType.Movement);

            Reserve();
        }

        //iPoolableEntity
        public abstract T OnCreate();
        public abstract Action<T> onGetFromPool { get; }
        public virtual Action<T> onReleaseToPool { get; }
        public abstract iObjectPool<T> Pool { get; }
        #endregion
    }
}