using System;
using System.Collections;
using System.Collections.Generic;
using agalag.engine;
using agalag.engine.routines;

namespace agalag.game
{
    public class WaveController
    {
        private bool _isDone;
        private float _timeout;
        public Action onWaveDone;
        private List<iWaveUnit> unitList;

        public bool IsoDone => _isDone;

        public WaveController(float timeout, List<iWaveUnit> unitList)
        {
            this._isDone = false;
            this._timeout = timeout;
            this.unitList = unitList;
        }
        public void Initialize()
        {
            foreach (iWaveUnit unit in unitList)
            {
                unit.onUnitReleased += RemoveUnitFromWave;
                unit.Initialize();
            }
            
            RoutineManager.Instance.CallbackTimer(_timeout, TimeOutAllUnits);
        }
        private void RemoveUnitFromWave(iWaveUnit unit)
        {
            unitList.Remove(unit);

            if(unitList.Count == 0)
            {
                onWaveDone?.Invoke();
                _isDone = true;
            }
        }
        private void TimeOutAllUnits()
        {
            foreach (iWaveUnit unit in unitList)
                unit.ExecuteTimeoutAction();

            RoutineManager.Instance.CallbackTimer(_timeout / 2, EliminateAllUnits);
        }
        private void EliminateAllUnits()
        {
            foreach (iWaveUnit unit in unitList)
            {
                if(_isDone)
                    return;

                unit?.Reserve();
            }
        }
    }
}