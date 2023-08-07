using System;
using System.Collections;
using System.Collections.Generic;
using agalag.engine;
using agalag.engine.routines;
using Microsoft.Xna.Framework;

namespace agalag.game
{
    public class LevelController 
    {
        public static LevelController Instance { get; private set; }

        private Player _player;
        private iLevel _level;    

        private WaveController _currentWave;

        public Action onNoWave;
        public Action onGameOver;

        private int _score;

        public Player Player => _player;

        public LevelController(iLevel level, Player player = null)
        {
            Instance = this;
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
            _score = 0;
            GameplayUI.Instance.UpdateScore(_score);

            _player.onDeath += GameOver;
            _player.onLifeChange += OnLifeChange;
            _player.onShieldChange += OnShieldChange;
            _player.onWeaponShoot += OnWeaponShoot;
            _player.onNewWeapon += OnNewWeapon;
            CallNextWave();
        }

        public void UpdateScore(int score)
        {
            _score += score;
            GameplayUI.Instance.UpdateScore(_score);
        }

        private void OnLifeChange(int life)
        {
            GameplayUI.Instance.UpdateLifeHUD(life);
        }

        private void OnShieldChange(int shield)
        {
            GameplayUI.Instance.UpdateShieldHUD(shield);
        }

        private void OnWeaponShoot(string ammo)
        {
            GameplayUI.Instance.UpdateAmmoCount(ammo);
        }

        private void OnNewWeapon(Sprite icon)
        {
            GameplayUI.Instance.UpdateWeaponIcon(icon);
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
            GameOverUI.Instance.UpdateScore(_score);
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
