using System.Collections.Generic;
using agalag.engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace agalag.game
{
    public abstract class Enemy : Entity
    {
        public int score;

        public float currentSpeed;
        protected float _defaultSpeed;

        public float currentAcceleration;
        protected float _defaultAcceleration;

        protected Queue<iEnemyAction> _actionQueue;
        protected iEnemyAction _startingAction;
        protected iEnemyAction _timeoutAction;
        protected iEnemyAction _currentAction;
        
        protected Enemy(Texture2D sprite, Vector2 position, Vector2 scale, float rotation = 0, iCollider collider = null) 
        : base(sprite, position, scale, rotation, collider) {
            _transform.drag = 10f;
        }
        
        
        //Methods
        public void ExecuteNextAction()
        {
            if(this._actionQueue.Count > 0)
                this.SwitchAction(this._actionQueue.Dequeue());
            else
                this.SwitchAction(null);
        }
        public void ExecuteStartingAction() => this.SwitchAction(this._startingAction);
        public void ExecuteTimeoutAction() => this.SwitchAction(this._timeoutAction);
        protected void SwitchAction(iEnemyAction action)
        {
            this._currentAction?.OnFinish(this);
            this._currentAction = action;
            this._currentAction?.OnStart(this);
        }
        public void Initialize(Queue<iEnemyAction> actionQueue, iEnemyAction startingAction, iEnemyAction timeoutAction, Vector2 startingPoint)
        {
            if(actionQueue == null || timeoutAction == null)
                throw new System.ArgumentNullException("Action queue and Timeout action may not be null");

            this._actionQueue = actionQueue;
            this._startingAction = startingAction;
            this._timeoutAction = timeoutAction;
            this._transform.position = startingPoint;

            this._sprite.SetVisibility(true);

            if(this._startingAction != null)
                this.SwitchAction(this._startingAction);
            else
                this.ExecuteNextAction();
        }
        
        public void Reserve()
        {
            this._actionQueue = null;
            this._startingAction = null;
            this._timeoutAction = null;
            this._transform.position = Vector2.Zero;
            this._sprite.SetVisibility(false);
        }

        public override void Update(GameTime gameTime)
        {
            if(_currentAction == null)
                return;

            if(_currentAction.CheckCondition(this))
                ExecuteNextAction();

            _currentAction?.Update(this);

        }
        public override void FixedUpdate(GameTime gameTime, FixedFrameTime fixedGameTime)
        {
            if(_currentAction == null)
                return;
            
            if(!_currentAction.CheckCondition(this))
                _currentAction.FixedUpdate(this);
        }
    }
}