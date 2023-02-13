using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace agalag.engine 
{
    public class SceneLayer: iParentObject, iObject
    {
        //Attributes
        public int zPosition { get; protected set; }
        protected List<MonoEntity> _entities;

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

        #region Interface Implementation

        //iParent
        public void DrawChildren(SpriteBatch spriteBatch)
        {
            foreach(MonoEntity entity in _entities)
            {
                if(entity.IsActive)
                {
                    entity.Draw(spriteBatch);
                    entity.DrawCollider(spriteBatch);
                }
            }
        }
        public void UpdateChildren(GameTime gameTime)
        {
            foreach(MonoEntity entity in _entities)
                if(entity.IsActive)
                    entity.Update(gameTime);
        }
        public void FixedUpdateChildren(GameTime gameTime, FixedFrameTime fixedFrameTime)
        {
            foreach(MonoEntity entity in _entities)
            {
                if(entity.IsActive)
                {
                    entity.FixedUpdate(gameTime, fixedFrameTime);
                    entity.ApplyVelocity();
                }
            }

            CollisionManager.CheckCollisions(_entities);
        }

        //iObject
        public virtual void Draw(SpriteBatch spriteBatch) {}
        public virtual void Update(GameTime gameTime) {}
        public virtual void FixedUpdate(GameTime gameTime, FixedFrameTime fixedFrameTime) {}

        #endregion

    }
}