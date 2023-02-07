using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace agalag.engine
{
    public abstract class MonoEntity: iObject
    {
        //Attributes
        protected Sprite _sprite;
        protected iCollider _collider;
        protected Transform _transform;
        
        //Properties
        public Transform Transform => _transform;
        public iCollider Collider => _collider;
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
        public void RemoveCollider() {
            _collider = null;
        }
        public abstract void OnCollision(MonoEntity other);
        public void DrawCollider(SpriteBatch spriteBatch)
        {
            #if DEBUG
            this._collider?.Draw(spriteBatch);
            #endif
        }
        
        #region Interface Implementation

        //iObject
        public abstract void Draw(SpriteBatch spriteBatch);
        public abstract void FixedUpdate(GameTime gameTime, FixedFrameTime fixedFrameTime);
        public abstract void Update(GameTime gameTime);

        #endregion
    }
}