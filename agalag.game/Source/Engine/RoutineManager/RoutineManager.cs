using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using agalag.engine.utils;
using Microsoft.Xna.Framework;

namespace agalag.engine.routines
{
    public class RoutineManager: Singleton<RoutineManager>
    {
        private readonly Dictionary<string, RoutineRunner> _routines = new Dictionary<string, RoutineRunner>();

        public void Update(GameTime gameTime) 
        {
            _routines.Values.ToList().ForEach((routine) => routine.Update(gameTime));

            foreach(string runnerId in _routines.Where((runner) => runner.Value.IsDone).Select((runner) => runner.Key).ToList())
                _routines.Remove(runnerId);
        }

        public string CallbackTimer(float timeout, Action callback = null) => Run(SetTimer(timeout, callback));
        private IEnumerable SetTimer(float timeout, Action callback = null)
        {
            yield return new WaitForSeconds(timeout);

            callback?.Invoke();
        }

        public string Run(IEnumerable routine)
        {
            RoutineRunner handler = new RoutineRunner(routine);
            _routines.Add(handler.GetHashCode().ToString(), handler);
            handler.Step();

            return handler.GetHashCode().ToString();
        }
        public bool Interrupt(string id)
        {
            if(string.IsNullOrWhiteSpace(id))
                return false;
            if(!_routines.ContainsKey(id))
                return false;

            _routines[id].Interrupt();
            return true;
        }
    }

    internal class RoutineRunner 
    {
        internal bool IsDone { get; private set; }

        private readonly IEnumerator _enumerator;

        internal RoutineRunner(IEnumerable routine)
        {
            _enumerator = routine.GetEnumerator();
            IsDone = false;
        }

        internal void Update(GameTime gameTime) 
        {
            Routine routine = _enumerator.Current as Routine;

            if(SceneManager.Instance.IsPaused && !routine.ExecuteOnPause)
                return;

            if (routine == null || routine.IsDone)
                Step();
            else 
                routine.Update(gameTime);
        }

        internal void Interrupt() => IsDone = true;

        internal void Step()
        {
            if (_enumerator.MoveNext())
            {
                Routine routine = _enumerator.Current as Routine;

                if(routine != null)
                    routine.Run();
            }
            else
            {
                IsDone = true;
            }
        }
    }

    public abstract class Routine
    {
        public bool IsDone { get; protected set; }
        public bool ExecuteOnPause { get; protected set; }

        public virtual void Update(GameTime gameTime) { }
        public abstract void Run();
    }
}