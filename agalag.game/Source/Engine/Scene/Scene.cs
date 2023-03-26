using System.Collections.Generic;
using agalag.engine.content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace agalag.engine
{
    public abstract class Scene : iParentObject, iObject
    {
        //Attributes
        public bool isLoaded { get; protected set; }
        public bool isInitialized { get; protected set; }
        protected SortedDictionary<int, SceneLayer> _layers;
        protected List<int> _orderedLayers;

        //Constructors
        public Scene(List<SceneLayer> layers = null) 
        {
            _layers = new SortedDictionary<int, SceneLayer>();
            if(layers != null)
                foreach (SceneLayer layer in layers)
                    AddLayer(layer);
        }

        //Methods
        public bool AddLayer(SceneLayer layer) 
        {
            if(_layers.ContainsValue(layer))
                return false;
            
            int zPosition = layer.zPosition;

            while(_layers.ContainsKey(zPosition))
                zPosition--;

            _layers.Add(zPosition, layer);

            return true;
        }

        public bool RemoveLayer(SceneLayer layer) 
        {
            if(!_layers.ContainsValue(layer))
                return false;

            foreach (var tuple in _layers)
            {
                if(tuple.Value == layer)
                {
                    _layers.Remove(tuple.Key);
                    return true;
                }
            }

            return false;
        }

        public bool AddElementToLayer(MonoEntity entity, Layer layer)
        {
            int key = (int)layer;
            if (_layers.Count == 0 || _layers[key] == null) return false;

            SceneLayer sceneLayer = _layers[key];

            return sceneLayer.AddEntity(entity);
        }

        public bool RemoveElementFromLayer(MonoEntity entity)
        {
            foreach (SceneLayer layer in _layers.Values)
            {
                if (layer.RemoveEntity(entity))
                    return true;
            }

            return false;
        }

        public abstract bool LoadContent(ContentManager content);
        public abstract bool UnloadContent(ContentManager content);

        public abstract void Initialize();
        public abstract void Clear();

        public T GetPrefab<T>() where T: MonoEntity => Prefabs.GetPrefabOfType<T>();

        #region Interface Implementation
        
        //iParent
        public void DrawChildren(SpriteBatch spriteBatch)
        {
            foreach(SceneLayer layer in _layers.Values) 
            {
                layer.Draw(spriteBatch);
                layer.DrawChildren(spriteBatch);
            }
        }
        public void FixedUpdateChildren(GameTime gameTime, FixedFrameTime fixedFrameTime)
        {
            List<MonoEntity> entities = new();
            foreach(SceneLayer layer in _layers.Values)
            {
                layer.FixedUpdate(gameTime, fixedFrameTime);
                layer.FixedUpdateChildren(gameTime, fixedFrameTime);
                entities.AddRange(layer.Entities);
            }

            CollisionManager.CheckCollisions(entities);
        }
        public void UpdateChildren(GameTime gameTime)
        {
            foreach (SceneLayer layer in _layers.Values)
            {
                layer.Update(gameTime);
                layer.UpdateChildren(gameTime);
            }
            UIHandler.Instance.Update(gameTime);
        }

        //iObject
        public abstract void Draw(SpriteBatch spriteBatch);
        public abstract void FixedUpdate(GameTime gameTime, FixedFrameTime fixedFrameTime);
        public abstract void Update(GameTime gameTime);

        #endregion
    }
}