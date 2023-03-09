using System;
using System.Collections.Generic;
using System.Diagnostics;
using agalag.engine;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace agalag.engine.content {
    public static class Prefabs {
        private static ContentManager _content;

        public static void DefineContent(ContentManager content) => _content = content;

        private static Dictionary<Type, Prefab> _prefabs = new Dictionary<Type, Prefab>();
        private static Dictionary<Type, Texture2D> _prefabTextures = new Dictionary<Type, Texture2D>();

        public static void AddPrefab<T>(T entity, Texture2D texture = null)
            where T: MonoEntity
        {
            Type type = typeof(T);
            if(_prefabs.ContainsKey(type) || entity == null)
                return;
            
            Prefab prefab = new Prefab(entity, texture, type);
            _prefabs.Add(type, prefab);
        }

        public static T GetPrefabOfType<T>()
            where T: MonoEntity
        {
            if(!_prefabs.ContainsKey(typeof(T)))
                return null;

            return _prefabs[typeof(T)].Entity as T;
        }

        public static void AddPrefab<T>(Texture2D texture)
            where T: MonoEntity
        {
            Type type = typeof(T);
            if(_prefabTextures.ContainsKey(type) || texture == null)
                return;

            _prefabTextures.Add(type, texture);
        }

        public static Texture2D GetTextureOfType<T>()
            where T: MonoEntity
        {
            if(!_prefabTextures.ContainsKey(typeof(T)))
                return null;


            return _prefabTextures[typeof(T)];
        }
    }

    public struct Prefab
    {
        public MonoEntity Entity;
        public Texture2D Texture;
        public Type Type;

        public Prefab(MonoEntity entity, Texture2D texture, Type type)
        {
            Entity = entity;
            Texture = texture;
            Type = type;
        }
    }
}
