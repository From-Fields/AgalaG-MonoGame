
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace agalag.engine
{
    public class UISprite : UIElement
    {
        private Sprite _sprite;
        private Color _color;
        private float _opacity;
        private Rectangle? _area;

        public UISprite(Sprite sprite, Vector2 position, Color color, float opacity = 1, Rectangle? area = null) : base(position)
        {
            this._sprite = sprite;
            this._color = color;
            this._opacity = opacity;
            this._area = area;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            _sprite.Draw(_transform, spriteBatch, tint: _color, opacity: _opacity, area: _area);
        }
    }
}