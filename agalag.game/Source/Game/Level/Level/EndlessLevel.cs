using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using agalag.engine.routines;
using Microsoft.Xna.Framework;

namespace agalag.game
{
    public class EndlessLevel : iLevel
    {
        private List<WaveController> _waveList;
        private Random _seed;
        private Queue<WaveController> _waveQueue;
        private WaveController _previousWave, _currentWave;

        public bool HasNextWave => true;
        
        public EndlessLevel(Queue<WaveController> waves) 
            :this(new List<WaveController>(waves)) { }
        public EndlessLevel(List<WaveController> waves) 
        {
            _waveList = waves;
            _seed = new Random();
            ShuffleWaves();
        }

        private void ShuffleWaves()
        {
            IOrderedEnumerable<WaveController> randomizedList = _waveList.OrderBy(item => _seed.Next());
            _waveQueue = new Queue<WaveController>(randomizedList);
        }

        public List<WaveController> WaveList => new List<WaveController>(_waveList);

        public WaveController GetNextWave()
        {
            _previousWave = _currentWave;

            if(_waveQueue.Count == 0)
            {
                do {
                    ShuffleWaves();
                    _currentWave = _waveQueue.Dequeue();
                } while(_currentWave == _previousWave);
            }
            else
            {
                _currentWave = _waveQueue.Dequeue();
            }

            return _currentWave;
        }
        public void Clear()
        {
            foreach (WaveController wave in new List<WaveController>(_waveList))
                wave.Clear();

            _waveQueue = null;
            _waveList = null;
        }
    }
}