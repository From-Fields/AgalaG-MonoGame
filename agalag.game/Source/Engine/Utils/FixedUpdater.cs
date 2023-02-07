using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace agalag.engine 
{

    public class FixedUpdater
    {
        private float _fixedUpdateDelta;
        private float _previousTime;
        private float _frameAccumulator;
        private float _maxFrameTime;
        private float _frameProgress;

        public FixedUpdater(float fixedUpdateDelta = -1, float maxFrameTime = 250)
        {
            fixedUpdateDelta = (fixedUpdateDelta < 0.15f) ? (int)(1000 / (float)30) : fixedUpdateDelta;
            maxFrameTime = (maxFrameTime < 250) ? 250 : fixedUpdateDelta;

            this._previousTime = 0f;
            this._frameAccumulator = 0f;
            this._frameProgress = 0;
            this._maxFrameTime = maxFrameTime;
            this._fixedUpdateDelta = fixedUpdateDelta;
        }

        public void ExecuteFixedUpdate(GameTime gameTime, System.Action<GameTime, FixedFrameTime> callback)
        {
            
            if (_previousTime == 0)
            {
                _previousTime = (float)gameTime.TotalGameTime.TotalMilliseconds;
            }
        
            float now = (float)gameTime.TotalGameTime.TotalMilliseconds;
            float frameTime = now - _previousTime;
            if (frameTime > _maxFrameTime)
            {
                frameTime = _maxFrameTime;
            }
                
            _previousTime = now;
        
            _frameAccumulator += frameTime;
        
            while (_frameAccumulator >= _fixedUpdateDelta)
            {
                callback.Invoke(gameTime, new FixedFrameTime(_fixedUpdateDelta, frameTime, _maxFrameTime, ref _frameProgress));
                _frameAccumulator -= _fixedUpdateDelta;
            }
            _frameProgress = (_frameAccumulator / _fixedUpdateDelta);
        }
    }

    public struct FixedFrameTime
    {
        public float fixedUpdateDelta;
        public float frameTime;
        public float maxFrameTime;
        public float frameProgress;

        public FixedFrameTime(float fixedUpdateDelta, float frameTime, float maxFrameTime, ref float frameProgress)
        {
            this.fixedUpdateDelta = fixedUpdateDelta;
            this.frameTime = frameTime;
            this.maxFrameTime = maxFrameTime;
            this.frameProgress = frameProgress;
        }
    }

}