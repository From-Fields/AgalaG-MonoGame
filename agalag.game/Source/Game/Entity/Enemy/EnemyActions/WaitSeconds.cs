using agalag.engine.routines;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace agalag.game
{
    public class WaitSeconds: iEnemyAction
    {
        //Attributes
        private float _timeout;

        private bool _done = false;

        //Constructors
        public WaitSeconds(float timeout) => _timeout = timeout;

        #region Interface Implementation
        public bool CheckCondition(iEnemy target) => _done;
        public void FixedUpdate(iEnemy target) { return; }
        public void Update(iEnemy target) { return; }
        public void OnStart(iEnemy target) => RoutineManager.Instance.CallbackTimer(_timeout, () => _done = true);
        public void OnFinish(iEnemy target) 
        {
            //System.Diagnostics.Debug.WriteLine("Done Waiting");
        }
        #endregion
    }
}