using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace agalag.engine
{
    public abstract class MonoEntity: iObject
    {
        //Attributes
        protected bool _active;
        protected Sprite _sprite;
        protected iCollider _collider;
        protected Transform _transform;
        
        //Properties
        public bool IsActive => this._active;
        public Transform Transform => this._transform;
        public iCollider Collider => this._collider;
        public bool HasCollider => this._collider != null;

        //Constructors
        #region Constructors

        public MonoEntity(Texture2D sprite = null): 
            this(sprite, Vector2.Zero, Vector2.One, 0f) { }
        public MonoEntity(Texture2D sprite, Vector2 position): 
            this(sprite, position, Vector2.One, 0f) { }
        public MonoEntity(Texture2D sprite, Vector2 position, Vector2 scale): 
            this(sprite, position, scale, 0f) { }
        public MonoEntity(Texture2D sprite, Vector2 position, Vector2 scale, float rotation) 
        {
            this._sprite = new Sprite(sprite);
            _transform = new Transform(position, scale, rotation);
        }
        public MonoEntity(Texture2D sprite, Vector2 position, Vector2 scale, iCollider collider): 
            this(sprite, position, scale, 0f, collider) { }
        public MonoEntity(Texture2D sprite, Vector2 position, Vector2 scale, float rotation, iCollider collider) 
        {
            this._sprite = new Sprite(sprite);
            _transform = new Transform(position, scale, 0f);
            SetCollider(collider);
            SetActive(true);
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
        public void DrawCollider(SpriteBatch spriteBatch)
        {
            #if DEBUG
            this._collider?.Draw(spriteBatch);
            #endif
        }
        public void ApplyVelocity() => _transform.ApplyVelocity();

        #region Interface Implementation

        //iObject
        public abstract void Draw(SpriteBatch spriteBatch);
        public abstract void FixedUpdate(GameTime gameTime, FixedFrameTime fixedFrameTime);
        public abstract void Update(GameTime gameTime);

        #endregion
    }
}