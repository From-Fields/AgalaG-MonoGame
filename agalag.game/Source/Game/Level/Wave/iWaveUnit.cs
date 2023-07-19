using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace agalag.game
{
    public interface iWaveUnit
    {
        public Action<iWaveUnit> onUnitReleased { get; set; }
        public void Initialize(Rectangle levelBounds);
        public void ExecuteTimeoutAction();
        public void Reserve();
    }
}