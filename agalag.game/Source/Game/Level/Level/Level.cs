using System;
using System.Collections;
using System.Collections.Generic;
using agalag.engine.routines;
using Microsoft.Xna.Framework;

namespace agalag.game
{
    public class Level : iLevel
    {
        private Queue<WaveController> _waveQueue;

        public bool HasNextWave => (_waveQueue.Count > 0);
        
        public Level(Queue<WaveController> waves) => _waveQueue = waves;
        public Level(List<WaveController> waves) => _waveQueue = new Queue<WaveController>(waves);

        public List<WaveController> WaveList => new List<WaveController>(_waveQueue);

        public WaveController GetNextWave()
        {
            if(HasNextWave)
                return _waveQueue.Dequeue();

            return null;
        }
    }
}