using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace agalag.engine 
{
    public class SceneLayer: iParentObject, iObject
    {
        //Attributes
        public int zPosition { get; protected set; }
        protected List<MonoEntity> _entities;

        public List<MonoEntity> Entities => _entities;

        //Constructors
        public SceneLayer(int zPosition, List<MonoEntity> entities = null) 
        {
            this.zPosition = zPosition;
            this._entities = (entities == null) ? new List<MonoEntity>() : new List<MonoEntity>(entities);
        }

        //Methods
        public bool AddEntity(MonoEntity entity) 
        {
            if(_entities.Contains(entity))
                return false;

            _entities.Add(entity);
            return true;
        }

        public bool RemoveEntity(MonoEntity entity) => _entities.Remove(entity);
        public void Clear()
        {
            for (int i = 0; i < _entities.Count; i++)
            {
                MonoEntity entity = _entities[i];
                entity.Dispose();
            }
        }

        #region Interface Implementation

        //iParent
        public void DrawChildren(SpriteBatch spriteBatch)
        {
            foreach(MonoEntity entity in _entities)
            {
                if(entity.IsActive)
                {
                    entity.Draw(spriteBatch);
#if DEBUG
                    entity.DrawCollider(spriteBatch);
#endif
                }
            }
        }
        public void UpdateChildren(GameTime gameTime)
        {
            foreach(MonoEntity entity in _entities.ToList()) 
            {
                if(SceneManager.Instance.IsPaused && !entity.ExecuteOnPause)
                    continue;
                if(entity.IsActive)
                    entity.DoUpdate(gameTime);
            }
        }
        public void FixedUpdateChildren(GameTime gameTime, FixedFrameTime fixedFrameTime)
        {
            var entities = _entities.ToList();
            foreach(MonoEntity entity in entities)
            {
                if(SceneManager.Instance.IsPaused && !entity.ExecuteOnPause)
                    continue;

                if(entity.IsActive)
                {
                    entity.DoFixedUpdate(gameTime, fixedFrameTime);
                    entity.ManageCollisions();
                    entity.ApplyVelocity();
                }
            }
        }

        //iObject
        public virtual void Draw(SpriteBatch spriteBatch) {}
        public virtual void Update(GameTime gameTime) {}
        public virtual void FixedUpdate(GameTime gameTime, FixedFrameTime fixedFrameTime) {}

        #endregion

    }
}