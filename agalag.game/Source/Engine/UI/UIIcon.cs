using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace agalag.engine
{
    internal class UIIcon : UIElement
    {
        private Vector2 _size;
        private Color _color = Color.White;

        public UIIcon(Vector2 position, Vector2 size, Color? color = null) : base(position)
        {
            _size = size;
            if (color != null) _color = (Color)color;
        }

        public void SetColor(Color color)
        {
            _color = color;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!_active) return;

            var iconRect = Utils.DrawRectangle(out var _rect, 
                (int)(_transform.position.X), (int)(_transform.position.Y), 
                (int)(_size.X), (int)(_size.Y), 
                _color);
            spriteBatch.Draw(iconRect, _rect, Color.White);
        }
    }
}
