using agalag.engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace agalag.game
{
    public class WaitSeconds: iEnemyAction
    {
        //Attributes
        private float _startingGameTime;
        private float _timeout;

        //Constructors
        public WaitSeconds(float timeout)
        {
            this._timeout = timeout;
        }

        #region Interface Implementation
        public bool CheckCondition(Enemy target)
        {
            float gameDelta = FixedUpdater.GameTime.TotalGameTime.Seconds - this._startingGameTime;
            //System.Diagnostics.Debug.WriteLine(gameDelta);
            return gameDelta >= _timeout;
        }
        public void FixedUpdate(Enemy target) { return; }
        public void Update(Enemy target) { return; }
        public void OnStart(Enemy target)
        {
            //System.Diagnostics.Debug.WriteLine("Waiting");
            this._startingGameTime = FixedUpdater.GameTime.TotalGameTime.Seconds;
        }
        public void OnFinish(Enemy target) 
        {
            //System.Diagnostics.Debug.WriteLine("Done Waiting");
        }
        #endregion
    }
}