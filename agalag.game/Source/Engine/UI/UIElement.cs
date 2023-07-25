using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace agalag.engine
{
    public abstract class UIElement : iObject, IDisposable
    {
        protected bool _active;
        protected Transform _transform;
        protected List<UIElement> _children;

        public Transform Transform => this._transform;
        public bool IsActive => this._active;

        public UIElement(Vector2 position)
        {
            _transform = new Transform(position, Vector2.One, 0f);
            _active = true;

            UIHandler.Instance.AddElement(this);
            _children = new List<UIElement>();
        }

        public void SetActive(bool active) 
        {
            foreach (UIElement child in _children)
                child.SetActive(active);

            this._active = active;
        }

        #region Interface Implementation

        //iObject
        public abstract void Draw(SpriteBatch spriteBatch);
        public virtual void Update(GameTime gameTime) { }
        public virtual void FixedUpdate(GameTime gameTime, FixedFrameTime fixedFrameTime) { }

        //IDisposable
        public void Dispose()
        {
            UIHandler.Instance.RemoveElement(this);
            SetActive(false);
            _transform = null;
        }

        #endregion
    }
}
