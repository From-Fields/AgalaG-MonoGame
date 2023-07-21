using System;
using System.Collections.Generic;
using System.Diagnostics;
using agalag.engine;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace agalag.engine.content {
    public static class Prefabs {
        private static ContentManager _content;
		
		private static Dictionary<string, SpriteFont> _fonts = new();
		private static Dictionary<Shapes, Texture2D> _basicShapes = new();

        private static Dictionary<Type, Prefab> _prefabs = new Dictionary<Type, Prefab>();
        private static Dictionary<Type, Texture2D> _prefabTextures = new Dictionary<Type, Texture2D>();
        private static Dictionary<Type, SoundEffect> _prefabSounds = new Dictionary<Type, SoundEffect>();

        private static Dictionary<string, Texture2D> _sprites = new();
		
		public static void DefineContent(ContentManager content) => _content = content;
		public static SpriteFont StandardFont => GetFont("Standard");
		public static Texture2D GetShape(Shapes key) => _basicShapes[key];

        // Fonts
		public static void DefineStandardFont(SpriteFont font)
        {
            AddFont("Standard", font);
        }
        public static void AddFont(string key, SpriteFont font)
        {
            _fonts.Add(key, font);
        }
        public static SpriteFont GetFont(string key) => _fonts[key];

        // Prefabs
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

        // Sprites
        public static void AddSprite(string key, Texture2D texture)
        {
            _sprites.Add(key, texture);
        }
        public static Texture2D GetSprite(string key) => _sprites[key];

        // Textures
        public static void AddTexture<T>(Texture2D texture)
        {
            Type type = typeof(T);
            if(_prefabTextures.ContainsKey(type) || texture == null)
                return;

            _prefabTextures.Add(type, texture);
        }
        public static Texture2D GetTextureOfType<T>()
        {
            if(!_prefabTextures.ContainsKey(typeof(T)))
                return null;

            return _prefabTextures[typeof(T)];
        }
		
        // Sounds
        public static void AddSound<T>(SoundEffect sound)
        {
            Type type = typeof(T);
            if(_prefabSounds.ContainsKey(type) || sound == null)
                return;

            _prefabSounds.Add(type, sound);
        }
        public static SoundEffect GetSoundOfType<T>()
        {
            if(!_prefabSounds.ContainsKey(typeof(T)))
                return null;


            return _prefabSounds[typeof(T)];
        }

        // Shapes
		public static void AddShape(Texture2D texture, Shapes shape)
        {
            _basicShapes.Add(shape, texture);
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
	
	public enum Shapes
    {
        Rectangle,
    }
}
