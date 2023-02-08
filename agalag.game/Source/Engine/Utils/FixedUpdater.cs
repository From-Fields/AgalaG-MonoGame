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

        private static FixedFrameTime _frameTime;
        public static FixedFrameTime FixedFrameTime => _frameTime;

        private static GameTime _gameTime;
        public static GameTime GameTime => _gameTime;

        public FixedUpdater(float fixedUpdateDelta = -1, float maxFrameTime = 250)
        {
            fixedUpdateDelta = (fixedUpdateDelta < 0.15f) ? (int)(1000 / (float)30) : fixedUpdateDelta;
            maxFrameTime = (maxFrameTime < 250) ? 250 : fixedUpdateDelta;

            this._previousTime = 0f;
            this._frameAccumulator = 0f;
            this._frameProgress = 0;
            this._maxFrameTime = maxFrameTime;
            this._fixedUpdateDelta = fixedUpdateDelta;
            _frameTime = new FixedFrameTime(_fixedUpdateDelta, 0, _maxFrameTime/1000, _frameProgress);
            _gameTime = new GameTime();
        }

        public void ExecuteFixedUpdate(GameTime gameTime, System.Action<GameTime, FixedFrameTime> callback)
        {
            _gameTime = gameTime;
            
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
                callback.Invoke(gameTime, new FixedFrameTime(_fixedUpdateDelta, frameTime/1000, _maxFrameTime/1000, _frameProgress));
                _frameAccumulator -= _fixedUpdateDelta;
            }
            _frameProgress = (_frameAccumulator / _fixedUpdateDelta);

            _frameTime = new FixedFrameTime(_fixedUpdateDelta, frameTime/1000, _maxFrameTime/1000, _frameProgress);
        }
    }

    public struct FixedFrameTime
    {
        public float DesiredDelta;
        public float frameTime;
        public float maxFrameTime;
        public float frameProgress;

        public FixedFrameTime(float DesiredDelta, float frameTime, float maxFrameTime, float frameProgress)
        {
            this.DesiredDelta = DesiredDelta;
            this.frameTime = frameTime;
            this.maxFrameTime = maxFrameTime;
            this.frameProgress = frameProgress;
        }
    }

}