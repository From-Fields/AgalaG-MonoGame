using System;

namespace agalag.engine.pool
{
    public interface iObjectPool<T> where T : MonoEntity
    {
        public int CountActive { get; }
        public int CountInactive { get; }
        public int CountTotal { get; }

        public T Get();
        public void Release(T element);
    }
}