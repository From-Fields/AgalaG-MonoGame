using System;
using System.Collections;
using System.Collections.Generic;
using agalag.engine.routines;
using Microsoft.Xna.Framework;

namespace agalag.game
{
    public class LevelController 
    {
        private Player _player;
        private iLevel _level;    

        private WaveController _currentWave;

        public Action onNoWave;
        public Action onGameOver;

        public Player Player => _player;

        public LevelController(iLevel level, Player player = null)
        {
            this._player = player;
            this._level = level;
        }

        public void SetPlayer(Player player)
        {
            if(_player == null)
                _player = player;
        }

        public void Initialize()
        {
            _player.onDeath += GameOver;
            CallNextWave();
        }

        private void CallNextWave() {
            if(_currentWave != null) {
                _currentWave.onWaveDone -= CallNextWave;
                _currentWave = null;
            }

            if(!_level.HasNextWave) {
                EndLevel();
                return;
            }

            _currentWave = _level.GetNextWave();
            _currentWave.onWaveDone += CallNextWave;
            _currentWave.Initialize();
        }
        private void EndLevel() {
            onNoWave?.Invoke();
            ClearEvents();
        } 
        private void GameOver() {
            onGameOver?.Invoke();
            ClearEvents();
        } 

        private void ClearEvents() {
            onNoWave = null;
            onGameOver = null;
            _player.onDeath -= GameOver;
        }

        public void Clear() => _level.Clear();
    }
}
