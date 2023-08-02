using System;
using System.Collections;
using System.Collections.Generic;
using agalag.engine;
using agalag.engine.routines;
using Microsoft.Xna.Framework;

namespace agalag.game
{
    public class WaveUnit<T>: iWaveUnit where T: Enemy<T> 
    {
        private T _enemy;

        private Vector2 _startingPoint;

        private iEnemyAction _startingAction, _timeoutAction;
        private Queue<iEnemyAction> _actions;
        
        private iPowerUp _drop;
        
        private bool _hasTimedOut;
        private float _timeout;
        private string _callback;

        private Action _onRelease;
        private Action<int> _onDeath;

        public Action<iWaveUnit> onUnitReleased { get; set; }

        public WaveUnit(
            Vector2 startingPoint, iEnemyAction startingAction, iEnemyAction timeoutAction, Queue<iEnemyAction> actions, 
            Action<int> onDeath = null, Action onRelease = null, float timeout = -1, iPowerUp drop = null
        ) {
            _startingPoint = startingPoint;
            
            _startingAction = startingAction;
            _timeoutAction = timeoutAction;
            _actions = actions;

            _onDeath += onDeath;
            _onRelease += onRelease;

            _drop = drop;

            _timeout = timeout;
        }

        public void Initialize(Rectangle levelBounds, Layer layer = Layer.Entities)
        {
            _hasTimedOut = false;

            _enemy = EntityPool<T>.Instance.Pool.Get();
            _enemy.Initialize(new Queue<iEnemyAction>(_actions), _startingAction, _timeoutAction, _startingPoint, levelBounds, drop: _drop);
            SceneManager.AddToMainScene(_enemy, layer);

            _enemy.onDeath += _onDeath;
            _enemy.onRelease += _onRelease;
            _enemy.onRelease += OnUnitReleased;

            if(_timeout > 0)
                _callback = RoutineManager.Instance.CallbackTimer(_timeout, ExecuteTimeoutAction);
        }
        public void ExecuteTimeoutAction()
        {
            if(_hasTimedOut)
                return;
            _hasTimedOut = true;
            _enemy.ExecuteTimeoutAction();
        }
        public void Reserve() => _enemy.Reserve();
        private void OnUnitReleased()
        {
            _enemy.onRelease -= OnUnitReleased;
            onUnitReleased?.Invoke(this);
        }

        public void Clear() {
            OnUnitReleased();
            if(!string.IsNullOrWhiteSpace(_callback))
                RoutineManager.Instance.Interrupt(_callback);
        }
    }
}