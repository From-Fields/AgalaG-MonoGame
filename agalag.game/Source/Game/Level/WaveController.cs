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

        public bool IsDone => _isDone;

        public WaveController(float timeout, Rectangle levelBounds, List<iWaveUnit> unitList)
        {
            this._isDone = false;
            this._timeout = timeout;
            this._unitList = unitList;
            this._levelBounds = levelBounds;
        }
        public void Initialize()
        {
            foreach (iWaveUnit unit in _unitList)
            {
                unit.onUnitReleased += RemoveUnitFromWave;
                unit.Initialize(_levelBounds);
            }
            
            RoutineManager.Instance.CallbackTimer(_timeout, TimeOutAllUnits);
        }
        private void RemoveUnitFromWave(iWaveUnit unit)
        {
            _unitList.Remove(unit);

            if(_unitList.Count == 0)
            {
                onWaveDone?.Invoke();
                _isDone = true;
            }
        }
        private void TimeOutAllUnits()
        {
            int unitCount = _unitList.Count;

            for (int i = 0; i < unitCount; i++)
            {
                iWaveUnit unit = _unitList[i];
                unit.ExecuteTimeoutAction();
            }

            RoutineManager.Instance.CallbackTimer(_timeout / 2, EliminateAllUnits);
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
    }
}