using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Runtime.CompilerServices;

namespace agalag.engine
{
    public abstract class MonoEntity: iObject, IDisposable
    {
        //Attributes
        protected bool _active;
        protected Sprite _sprite;
        protected iCollider _collider;
        protected Transform _transform;
        protected string _tag = null;
        
        //Properties
        public bool IsActive => this._active;
        public Transform Transform => this._transform;
        public iCollider Collider => this._collider;
        public bool HasCollider => this._collider != null;

        public string Tag => _tag;
        protected string SetTag(string tag) => _tag = tag;

        //Constructors
        #region Constructors

        public MonoEntity(Texture2D sprite = null, Layer layer = Layer.Default): 
            this(sprite, Vector2.Zero, Vector2.One, 0f) { }
        public MonoEntity(Texture2D sprite, Vector2 position, Layer layer = Layer.Default): 
            this(sprite, position, Vector2.One, 0f) { }
        public MonoEntity(Texture2D sprite, Vector2 position, Vector2 scale, Layer layer = Layer.Default): 
            this(sprite, position, scale, 0f) { }
        public MonoEntity(Texture2D sprite, Vector2 position, Vector2 scale, float rotation, Layer layer = Layer.Default) 
        {
            this._sprite = new Sprite(sprite);
            _transform = new Transform(position, scale, rotation);
        }
        public MonoEntity(Texture2D sprite, Vector2 position, Vector2 scale, iCollider collider, Layer layer = Layer.Default): 
            this(sprite, position, scale, 0f, collider) { }
        public MonoEntity(Texture2D sprite, Vector2 position, Vector2 scale, float rotation, iCollider collider, Layer layer = Layer.Default) 
        {
            this._sprite = new Sprite(sprite);
            _transform = new Transform(position, scale, 0f);
            SetCollider(collider);
            SetActive(true);
            SceneManager.AddToMainScene(this, layer);
        }
        
        #endregion

        //Methods
        public bool SetCollider(iCollider collider) {
            if(collider == null)
                return false;
            
            _collider = collider;
            _collider.Parent = this._transform;
            return true;
        }

        public void RemoveCollider() => _collider = null;
        public void SetActive(bool active) => this._active = active;
        public abstract void OnCollision(MonoEntity other);
        
        public void ApplyVelocity() => _transform?.ApplyVelocity();

        public void Dispose()
        {
            RemoveCollider();
            SetActive(false);
            _sprite = null;
            _transform = null;
            SceneManager.RemoveFromScene(this);
        }

        #region Interface Implementation

        //iObject
        public abstract void Draw(SpriteBatch spriteBatch);
        public virtual void FixedUpdate(GameTime gameTime, FixedFrameTime fixedFrameTime) { }
        public virtual void Update(GameTime gameTime) { }

        #endregion

#if DEBUG
        public void DrawCollider(SpriteBatch spriteBatch)
        {
            this._collider?.Draw(spriteBatch);
        }
#endif
    }
}