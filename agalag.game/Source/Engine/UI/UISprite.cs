
using System;
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
            if(sprite != null)
                this._sprite = sprite;
            this._color = color;
            this._opacity = opacity;
            this._area = area;

            UIHandler.Instance.AddElement(this);
        }

        private void DrawRectangle(SpriteBatch spriteBatch)
        {
            Texture2D whiteRectangle = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
            whiteRectangle.SetData(new[] { Color.White });

            spriteBatch.Draw(whiteRectangle, _transform.position, _area, _color * _opacity);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if(_sprite != null)
                _sprite.Draw(_transform, spriteBatch, tint: _color, opacity: _opacity, area: _area);
            else if(_area != null)
                DrawRectangle(spriteBatch);
        }
    }
}