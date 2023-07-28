using System;
using System.Collections.Generic;

namespace agalag.engine.pool
{
    public class ObjectPool<T>: iObjectPool<T> where T : MonoEntity
    {
        private readonly uint _maxCapacity;
        
        private readonly List<T> _activeList;
        private readonly Queue<T> _objectQueue;

        private readonly Func<T> onCreateFunction;
		private readonly Action<T> onGetFromPoolAction;
		private readonly Action<T> onReleaseToPoolAction;

        public ObjectPool(
            Func<T> onCreate, Action<T> onGetFromPool, Action<T> onReleaseToPool, 
            uint maxCapacity = 10000000, uint initialCapacity = 0
        ) {
            onCreateFunction = onCreate;
            this.onGetFromPoolAction = onGetFromPool;
            this.onReleaseToPoolAction = onReleaseToPool;

            this._activeList = new List<T>();
            this._objectQueue = new Queue<T>();

            if(initialCapacity > maxCapacity)
                initialCapacity = maxCapacity;

            this._maxCapacity = maxCapacity;

            for (int i = 0; i < initialCapacity; i++)
                Populate();
        }

        public int CountActive => _activeList.Count;
        public int CountInactive => _objectQueue.Count;
        public int CountTotal => CountActive + CountInactive;

        public T Get()
        {
            if(_objectQueue.Count == 0)
                Populate();
 
            T obj = _objectQueue.Dequeue();
            _activeList.Add(obj);

            onGetFromPoolAction?.Invoke(obj);
            // System.Diagnostics.Debug.WriteLine("Got Unit.");
            // System.Diagnostics.Debug.WriteLine("Active/Inactive: " + CountActive.ToString() + "/" + CountInactive.ToString());
            return obj;
        }
        public void Release(T element)
        {
            if(_objectQueue.Contains(element)) {
                System.Diagnostics.Debug.WriteLine("Tried to release an element that is already released.");
                return;
            }
            

            onReleaseToPoolAction?.Invoke(element);
            _objectQueue.Enqueue(element);
            _activeList.Remove(element);
            // System.Diagnostics.Debug.WriteLine("Active/Inactive: " + CountActive.ToString() + "/" + CountInactive.ToString());
        }
    
        private void Populate()
        {
            if(CountTotal >= _maxCapacity)
                return;

            T newEntity = onCreateFunction();

            if(newEntity == null)
                throw new System.NullReferenceException("OnCreate of type " + typeof(T).ToString() + " returning null.");

            _objectQueue.Enqueue(newEntity);
            
            // System.Diagnostics.Debug.WriteLine("Generated Unit.");
        }
    }
}