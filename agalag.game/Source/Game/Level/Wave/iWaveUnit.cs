using System;
using System.Collections.Generic;
using agalag.engine;
using Microsoft.Xna.Framework;

namespace agalag.game
{
    public interface iWaveUnit
    {
        public Action<iWaveUnit> onUnitReleased { get; set; }
        public void Initialize(Rectangle levelBounds, Layer layer = Layer.Entities);
        public void ExecuteTimeoutAction();
        public void Reserve();
        public void Clear();
    }
}