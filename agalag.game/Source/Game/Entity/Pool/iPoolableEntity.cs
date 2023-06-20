using System;
using agalag.engine;
using agalag.engine.pool;

namespace agalag.game
{
    public interface iPoolableEntity<T> where T: MonoEntity, iPoolableEntity<T>
	{
		public T OnCreate();
		public Action<T> onGetFromPool { get; }
		public Action<T> onReleaseToPool { get; }
		public iObjectPool<T> Pool { get; }
	}
}