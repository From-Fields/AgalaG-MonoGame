using agalag.engine.routines;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace agalag.game
{
    public class Shoot: iEnemyAction
    {
        private float _timeout;
        private bool _done = false;

        //Constructors
        public Shoot(float timeout) => _timeout = timeout;

        #region Interface Implementation
        public bool CheckCondition(iEnemy target) => _done;
        public void FixedUpdate(iEnemy target) { return; }
        public void Update(iEnemy target) => target.Shoot();
        public void OnStart(iEnemy target) {
            _done = false;
            RoutineManager.Instance.CallbackTimer(_timeout, () => _done = true);
        } 
        public void OnFinish(iEnemy target) { _done = false; }
        #endregion
    }
}