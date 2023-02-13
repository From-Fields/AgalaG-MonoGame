using agalag.engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace agalag.game
{
    public class Shoot: iEnemyAction
    {
        //Attributes
        private float _startingGameTime;
        private float _timeout;

        //Constructors
        public Shoot(float timeout)
        {
            this._timeout = timeout;
        }

        #region Interface Implementation
        public bool CheckCondition(Enemy target)
        {
            float gameDelta = FixedUpdater.GameTime.TotalGameTime.Seconds - this._startingGameTime;
            return gameDelta >= _timeout;
        }
        public void FixedUpdate(Enemy target) { return; }
        public void Update(Enemy target) => target.Shoot();
        public void OnStart(Enemy target)
        {
            this._startingGameTime = FixedUpdater.GameTime.TotalGameTime.Seconds;
        }
        public void OnFinish(Enemy target) { return; }
        #endregion
    }
}