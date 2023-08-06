using agalag.engine;
using agalag.engine.content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace agalag.engine
{
    public class UIText : UIElement
    {
        private string _text;
        private SpriteFont _font;
        private Color _color;

        private TextAlign _align = TextAlign.Left;

        public string Text { 
            get { return _text; }
            set { _text = value; }
        }

        public UIText(string text, Vector2 position, SpriteFont font = null) :
            this(text, position, Color.White, font) { }

        public UIText(string text, Vector2 position, Color color, SpriteFont font = null) : base(position)
        {
            _text = text;
            _font = (font != null) ? font : Prefabs.StandardFont;
            _color = color;
        }

        public void SetText(string text) => _text = text;
        public void SetFont(SpriteFont font) => _font = font;
        public void SetColor(Color color) => _color = color;
        public void SetPos(Vector2 pos) => Transform.position = pos;
        public void SetScale(Vector2 scale) => Transform.scale = scale;
        public void SetAlign(TextAlign align) => _align = align;

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!_active) return;

            Vector2 size = _font.MeasureString(_text);

            Vector2 textPos = _transform.position;
            
            switch (_align)
            {
                case TextAlign.Center:
                    textPos.X -= (size.Length() / 2) * _transform.scale.X;
                    break;
                case TextAlign.Right:
                    textPos.X -= size.X;
                    break;
            }

            spriteBatch.DrawString(_font, _text, textPos, _color, 0f, Vector2.Zero, _transform.scale, SpriteEffects.None, 0f);
            //spriteBatch.DrawString(_font, _text, textPos, _color);
        }
    }

    public enum TextAlign
    {
        Left,
        Center,
        Right
    }
}
