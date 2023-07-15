using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
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
        protected EntityTag _tag = 0;
        private List<Collision> _collisions = new List<Collision>();

        //Properties
        public bool IsActive => this._active;
        public Transform Transform => this._transform;
        public iCollider Collider => this._collider;
        public bool HasCollider => this._active && this._collider != null;

        public EntityTag Tag => _tag;
        protected EntityTag SetTag(EntityTag tag) => _tag = tag;

        //Constructors
        #region Constructors

        public MonoEntity(Texture2D sprite = null, Layer layer = Layer.Default, Vector2? offset = null) : 
            this(sprite, Vector2.Zero, Vector2.One, 0f, layer, offset) { }
        public MonoEntity(Texture2D sprite, Vector2 position, Layer layer = Layer.Default, Vector2? offset = null): 
            this(sprite, position, Vector2.One, 0f, layer, offset) { }
        public MonoEntity(Texture2D sprite, Vector2 position, Vector2 scale, Layer layer = Layer.Default, Vector2? offset = null): 
            this(sprite, position, scale, 0f, layer, offset) { }
        public MonoEntity(Texture2D sprite, Vector2 position, Vector2 scale, float rotation, Layer layer = Layer.Default, Vector2? offset = null) 
        {
            this._sprite = new Sprite(sprite, _offset: offset);
            _transform = new Transform(position, scale, rotation);
            SetActive(true);
            SceneManager.AddToMainScene(this, layer);
        }

        public MonoEntity(Texture2D sprite, Vector2 position, Vector2 scale, iCollider collider, Layer layer = Layer.Default, Vector2? offset = null): 
            this(sprite, position, scale, 0f, collider, layer) { }
        public MonoEntity(Texture2D sprite, Vector2 position, Vector2 scale, float rotation, iCollider collider, Layer layer = Layer.Default, Vector2? offset = null) 
        {
            _collisions = new List<Collision>();
            this._sprite = new Sprite(sprite);
            _transform = new Transform(position, scale, rotation);
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
        internal void AddCollision(MonoEntity other) => _collisions.Add(new Collision(this, other));
        internal void ManageCollisions() 
        {
            if(Collider == null || Transform == null)
                return;

            for (int i = 0; i < _collisions.Count; i++)
            {
                if(_collisions[i].Other == null || !_collisions[i].Other.entity.IsActive)
                    continue;

                if(_transform.simulate && _collisions[i].IsSolid && _collider.IsSolid) {
                    Vector2 normal = _collisions[i].Normal;
                    Vector2 opposingForce = _transform.velocity;

                    float angle = normal.Angle(_transform.velocity);

                    if(angle > 90 || normal.Angle(_transform.velocity) == float.NaN) {
                        _transform.velocity += _transform.velocity * normal.Negative(); 
                        opposingForce = _transform.velocity + normal * _transform.velocity.Length();
                    }

                    _transform.velocity = Vector2.LerpPrecise(_transform.velocity, opposingForce, FixedUpdater.FixedFrameTime.frameTime);

                    // Vector2 previousPosition =  _collisions[i].Self.position + (_collisions[i].Self.velocity * normal.Negative());

                    // _transform.position = Vector2.LerpPrecise(_transform.position, previousPosition, FixedUpdater.FixedFrameTime.frameTime);
                }

                OnCollision(_collisions[i].Other.entity);
            }
            ClearCollisions();
        }
        internal void ClearCollisions() => _collisions.Clear();
        
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