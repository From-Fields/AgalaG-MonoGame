using agalag.engine.content;
using agalag.engine.utils;
using agalag.engine.pool;
using agalag.engine;

namespace agalag.game
{    
    public class EntityPool<T> : Singleton<EntityPool<T>> where T: MonoEntity, iPoolableEntity<T>
    {
        private iObjectPool<T> _pool;
        private T _prefab;

        public iObjectPool<T> Pool {
            get {
                if(_pool == null)
                    _pool = new ObjectPool<T>(Prefab.OnCreate, Prefab.onGetFromPool, Prefab.onReleaseToPool);

                return _pool;
            }
        }

        public T Prefab {
            get {
                if(_prefab == null)
                {
                    T Prefab = Prefabs.GetPrefabOfType<T>();

                    if(Prefab is T)
                        _prefab = Prefab as T;
                }
                
                return _prefab;
            }
        }
    }
}