using System;
using System.Collections;
using System.Collections.Generic;
using agalag.engine.routines;
using Microsoft.Xna.Framework;

namespace agalag.game
{
    public interface iLevel 
    {
        public bool HasNextWave { get; }    
        public List<WaveController> WaveList { get; }
        public WaveController GetNextWave();
    }

}