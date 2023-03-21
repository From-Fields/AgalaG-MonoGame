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

        protected float currentSpeed;
        protected float _defaultSpeed;

        protected float currentAcceleration;
        protected float _defaultAcceleration;

        protected bool _isDead;

        protected Queue<iEnemyAction> _actionQueue;
        protected iEnemyAction _startingAction;
        protected iEnemyAction _timeoutAction;
        protected iEnemyAction _currentAction;
        
        protected Enemy(Texture2D sprite, Vector2 position, Vector2 scale, float rotation = 0, iCollider collider = null) 
        : base(sprite, position, scale, rotation, collider) {
            SetTag("Enemy");
            _transform.drag = 10f;
            _transform.simulate = true;
        }
        
        public float DesiredSpeed => currentSpeed;
        public float CurrentAcceleration => currentAcceleration;

        //Properties
        public bool IsDead => this._isDead;

        //Events
        public Action<int> onDeath;
        public Action onRelease;
        
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
        public void Initialize(Queue<iEnemyAction> actionQueue, iEnemyAction startingAction, iEnemyAction timeoutAction, Vector2 startingPoint)
        {
            if(actionQueue == null || timeoutAction == null)
                throw new System.ArgumentNullException("Action queue and Timeout action may not be null");

            SubInitialize();

            this._isDead = false;
            this._actionQueue = actionQueue;
            this._startingAction = startingAction;
            this._timeoutAction = timeoutAction;
            this._transform.position = startingPoint;

            this.SetActive(true);

            if(this._startingAction != null)
                this.SwitchAction(this._startingAction);
            else
                this.ExecuteNextAction();
        }
        
        public void OnReserve()
        {
            this.onRelease?.Invoke();

            this._actionQueue = null;
            this._startingAction = null;
            this._timeoutAction = null;

            this.onDeath = null;
            this.onRelease = null;

            this._isDead = true;
            this._transform.Reset();
            this.SetActive(false);
        }

        public override void Update(GameTime gameTime)
        {
            if(_isDead || _currentAction == null)
                return;

            if(_currentAction.CheckCondition(this))
                ExecuteNextAction();

            _currentAction?.Update(this);

        }
        public override void FixedUpdate(GameTime gameTime, FixedFrameTime fixedGameTime)
        {
            if(_isDead || _currentAction == null)
                return;
            
            if(!_currentAction.CheckCondition(this))
                _currentAction.FixedUpdate(this);
        }

        //Abstract Methods
        protected abstract void SubInitialize();
        public abstract void Reserve();

        #region InterfaceImplementation

        //iEntity
        public override void Die()
        {
            onDeath?.Invoke(this.score);
            Reserve();
        }

        //iPoolableEntity
        public abstract T OnCreate();
        public abstract Action<T> onGetFromPool { get; }
        public virtual Action<T> onReleaseToPool => (obj) => obj.OnReserve();
        public abstract iObjectPool<T> Pool { get; }
        #endregion
    }
}