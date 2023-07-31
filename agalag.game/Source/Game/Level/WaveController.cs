using System;
using System.Collections;
using System.Collections.Generic;
using agalag.engine;
using agalag.engine.routines;
using Microsoft.Xna.Framework;

namespace agalag.game
{
    public class WaveController
    {
        private bool _isDone;
        private float _timeout;
        public Action onWaveDone;
        private List<iWaveUnit> _unitList;
        private Rectangle _levelBounds;
        private Layer _layer;
        private string _timeoutCallback;
        private string _eliminateCallback;

        public bool IsDone => _isDone;

        public WaveController(float timeout, Rectangle levelBounds, List<iWaveUnit> unitList, Layer layer = Layer.Entities)
        {
            this._isDone = false;
            this._timeout = timeout;
            this._unitList = unitList;
            this._levelBounds = levelBounds;
            this._layer = layer;
        }
        public void Initialize()
        {
            foreach (iWaveUnit unit in _unitList)
            {
                unit.onUnitReleased += RemoveUnitFromWave;
                unit.Initialize(_levelBounds, _layer);
            }
            
            _timeoutCallback = RoutineManager.Instance.CallbackTimer(_timeout, TimeOutAllUnits);
        }
        private void RemoveUnitFromWave(iWaveUnit unit)
        {
            if(_isDone)
                return;

            _unitList.Remove(unit);

            if(_unitList.Count == 0)
            {
                onWaveDone?.Invoke();
                _isDone = true;
            }
        }
        private void TimeOutAllUnits()
        {
            if(_isDone)
                return;

            int unitCount = _unitList.Count;

            for (int i = 0; i < unitCount; i++)
            {
                iWaveUnit unit = _unitList[i];
                unit.ExecuteTimeoutAction();
            }

            _eliminateCallback = RoutineManager.Instance.CallbackTimer(_timeout / 2, EliminateAllUnits);
        }
        private void EliminateAllUnits()
        {
            int unitCount = _unitList.Count;

            for (int i = 0; i < unitCount; i++)
            {
                iWaveUnit unit = _unitList[i];

                if(_isDone)
                    return;

                unit?.Reserve();
            }

            if(_isDone)
                return;

            onWaveDone?.Invoke();
        }

        public void Clear() {
            onWaveDone = null;
            ClearAllUnits();

            RoutineManager.Instance.Interrupt(_timeoutCallback);
            if(!string.IsNullOrWhiteSpace(_eliminateCallback))
                RoutineManager.Instance.Interrupt(_eliminateCallback);
            
            _isDone = true;
        }
        private void ClearAllUnits()
        {
            int unitCount = _unitList.Count;
            var units = new List<iWaveUnit>(_unitList);

            for (int i = 0; i < unitCount; i++)
            {
                iWaveUnit unit = units[i];

                unit?.Clear();
            }
        }
    }
}