using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace agalag.engine
{
    public abstract class MonoEntity: iObject
    {
        //Attributes
        protected Texture2D _sprite;
        protected Collider _collider;
        protected Transform _transform;
        
        //Properties
        public Transform Transform => _transform;
        public Collider Collider => _collider;
        public bool HasCollider => this._collider != null;

        //Constructors
        #region Constructors

        public MonoEntity(Texture2D sprite = null) 
        {
            this._sprite = sprite;
            _transform = new Transform(Vector2.Zero, Vector2.One, 0f);
        }
        public MonoEntity(Texture2D sprite, Vector2 position) 
        {
            this._sprite = sprite;
            _transform = new Transform(position, Vector2.One, 0f);
        }
        public MonoEntity(Texture2D sprite, Vector2 position, Vector2 scale) 
        {
            this._sprite = sprite;
            _transform = new Transform(position, scale, 0f);
        }
        public MonoEntity(Texture2D sprite, Vector2 position, Vector2 scale, float rotation) 
        {
            this._sprite = sprite;
            _transform = new Transform(position, scale, rotation);
        }
        public MonoEntity(Texture2D sprite, Vector2 position, Vector2 scale, Collider collider) 
        {
            this._sprite = sprite;
            _transform = new Transform(position, scale, 0f);
            _collider = collider;
        }
        public MonoEntity(Texture2D sprite, Vector2 position, Vector2 scale, float rotation, Collider collider) 
        {
            this._sprite = sprite;
            _transform = new Transform(position, scale, 0f);
            _collider = collider;
        }
        
        #endregion

        //Methods
        public bool SetCollider(Collider collider) {
            if(collider == null)
                return false;
            
            _collider = collider;
            _collider.SetParent(this._transform);
            return true;
        }
        public void RemoveCollider() {
            _collider = null;
        }

        #region Interface Implementation

        //iObject
        public abstract void Draw(SpriteBatch spriteBatch);
        public abstract void FixedUpdate(GameTime gameTime);
        public abstract void Update(GameTime gameTime);

        #endregion
    }
}