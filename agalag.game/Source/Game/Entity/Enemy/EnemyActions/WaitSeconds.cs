using agalag.engine;
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
        public bool CheckCondition(Enemy target) => _done;
        public void FixedUpdate(Enemy target) { return; }
        public void Update(Enemy target) { return; }
        public void OnStart(Enemy target) => RoutineManager.Instance.CallbackTimer(_timeout, () => _done = true);
        public void OnFinish(Enemy target) 
        {
            //System.Diagnostics.Debug.WriteLine("Done Waiting");
        }
        #endregion
    }
}