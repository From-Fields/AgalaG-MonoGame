using System;
using Microsoft.Xna.Framework;

namespace agalag.engine.routines
{
    public class WaitForSeconds: Routine
    {
        public WaitForSeconds(float seconds, bool executeOnPause = false)
        {
            this.ExecuteOnPause = executeOnPause;
            IsDone = false;
            Interval = TimeSpan.FromSeconds(seconds);
        }

        public override void Run()
        {
            _elapsed = TimeSpan.Zero;
        }

        public override void Update(GameTime gameTime)
        {
            _elapsed = _elapsed.Add(gameTime.ElapsedGameTime);

            if (_elapsed.TotalMilliseconds >= Interval.TotalMilliseconds)
                IsDone = true;
        }


        private TimeSpan _elapsed;

        public TimeSpan Interval { get; private set; }
    }
}