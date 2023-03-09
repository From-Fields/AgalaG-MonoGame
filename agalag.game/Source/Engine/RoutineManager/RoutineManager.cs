using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using agalag.engine.utils;
using Microsoft.Xna.Framework;

namespace agalag.engine
{
    public class RoutineManager: Singleton<RoutineManager>
    {
        private readonly List<RoutineRunner> _routines = new List<RoutineRunner>();

        public void Update(GameTime gameTime) 
        {
            _routines.ForEach((routine) => routine.Update(gameTime));

            foreach(var runner in _routines.Where((runner) => runner.IsDone).ToList())
                _routines.Remove(runner);
        }

        public void CallbackTimer(float timeout, Action callback = null) => Run(SetTimer(timeout, callback));
        private IEnumerable SetTimer(float timeout, Action callback = null)
        {
            yield return new WaitForSeconds(timeout);

            callback?.Invoke();
        }

        public void Run(IEnumerable routine)
        {
            RoutineRunner handler = new RoutineRunner(routine);
            _routines.Add(handler);
            handler.Step();
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

            if (routine == null || routine.IsDone)
                Step();
            else 
                routine.Update(gameTime);
        }

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

        public virtual void Update(GameTime gameTime) { }
        public abstract void Run();
    }
}