using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace agalag.engine
{
    public class Sprite
    {
        //Attributes
        private bool _visible;
        private Texture2D _sprite;
        public Vector2 anchor;
        public Vector2 offset;

        //Properties
        public bool IsVisible => this._visible;
        private Vector2 _Dimensions => new Vector2(_sprite.Width, _sprite.Height);
        public Texture2D Texture => this._sprite;

        //Constructors
        public Sprite(Texture2D sprite, bool visibility = true, Vector2? _anchor = null, Vector2? _offset = null)
        {
            Vector2 anchor = (_anchor == null) ? new Vector2(0.5f, 0.5f) : _anchor.Value;
            Vector2 offset = (_offset == null) ? Vector2.Zero : _offset.Value;

            this._sprite = sprite;
            this._visible = visibility;
            this.anchor = anchor;
            this.offset = offset;
        }

        //Methods
        public void Draw(Transform transform, SpriteBatch spriteBatch)
        {
            if(this._sprite == null || !this._visible)
                return;

            Vector2 position = transform.position;
            Vector2 dimensions = _Dimensions * transform.scale;
            Vector2 positionOffset = dimensions * anchor + offset;

            position = position - positionOffset;
            Vector2 velocity = transform.velocity * FixedUpdater.FixedFrameTime.frameTime;

            Vector2 oldPos = position;
            position = Vector2.Lerp(position - velocity, position, FixedUpdater.FixedFrameTime.frameProgress);

            spriteBatch.Draw(
                _sprite,
                position,
                null,
                Color.White,
                transform.rotation,
                Vector2.Zero,
                transform.scale,
                SpriteEffects.None,
                0
            );
        }
        public void SetVisibility(bool visibility) => this._visible = visibility;
        public void ToggleVisibility() => this._visible = !this._visible;
    }
}