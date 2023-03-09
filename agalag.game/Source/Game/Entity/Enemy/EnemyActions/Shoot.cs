using agalag.engine;
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
        public bool CheckCondition(Enemy target) => _done;
        public void FixedUpdate(Enemy target) { return; }
        public void Update(Enemy target) => target.Shoot();
        public void OnStart(Enemy target) => RoutineManager.Instance.CallbackTimer(_timeout, () => _done = true);
        public void OnFinish(Enemy target) { return; }
        #endregion
    }
}