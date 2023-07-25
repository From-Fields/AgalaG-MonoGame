using agalag.engine.content;
using agalag.game.input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace agalag.engine
{
    public class UIButton : UIElement
    {
        private UIText _text;
        private Action _onClick;
        private Vector2 _size;

        private Texture2D _buttonRect;
        private Color _color = Color.Gray;
        private Color _hoverColor = Color.Aqua;

        private bool IsSelected => UIHandler.Instance.Selected == this;

        private Vector2 _relativePosition;
        private Vector2 _relativeSize;

        private Rectangle _rect;

        public UIButton(string text, Vector2 position, Vector2 size, Action action = null, Color? hoverColor = null) : base (position)
        {
            _onClick = action;
            _size = size;
            _hoverColor = hoverColor ?? _hoverColor;

            //Position correction
            UpdateRelativePosition();

            var textPos = _relativePosition + new Vector2(size.X / 2f, size.Y / 4f);
            _text = new UIText(text, textPos, Prefabs.GetFont("Button"));
            _text.SetAlign(TextAlign.Center);

            _children.Add(_text);

            UIHandler.Instance.AddToInteractable(this);
        }

        private void UpdateRelativePosition()
        {
            _relativePosition = new Vector2(Transform.position.X - (_relativeSize.X / 2), Transform.position.Y - (_relativeSize.Y / 2));
        }

        Vector2 MousePos;

        public override void Update(GameTime gameTime)
        {
            Vector2 mousePosition = InputHandler.Instance.ScaledMousePosition;
            MousePos = InputHandler.Instance.ScaledMousePosition;

            float bbox_left = _relativePosition.X;
            float bbox_right = _relativePosition.X + (_size.X);
            float bbox_top = _relativePosition.Y;
            float bbox_bottom = _relativePosition.Y + (_size.Y);

            if (mousePosition.X >= bbox_left && mousePosition.X <= bbox_right 
                && mousePosition.Y >= bbox_top && mousePosition.Y <= bbox_bottom)
            {
                UIHandler.Instance.SetSelected(this);
            }
            //else if (UIHandler.Instance.Selected == this)
            //{
            //    UIHandler.Instance.SetSelected(null);
            //}

            if (IsSelected && InputHandler.Instance.GetMouseLeftPressed())
            {
                Debug.WriteLine("botão [" + _text + "] clicado");
                if (_onClick == null)
                {
                    Debug.WriteLine("Action was not assigned to this button.");
                    return;
                }
                _onClick.Invoke();
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            float multiplier = (IsSelected) ? 1.1f : 1f;
            var color = (IsSelected) ? _hoverColor : _color;

            _buttonRect = Utils.DrawRectangle(color);

            _relativeSize = _size * (IsSelected ? multiplier:1f);
            UpdateRelativePosition();

            //spriteBatch.DrawString(Prefabs.StandardFont, "Mouse: (" + MousePos.X + " " + MousePos.Y + ")", new Vector2(50, 50), Color.White);
            //spriteBatch.DrawString(Prefabs.StandardFont, "Button Pos: (" + _relativePosition.X + ", " + _relativePosition.Y + ")", new Vector2(50, 150 + _relativePosition.Y * .2f), Color.White);
            //spriteBatch.DrawString(Prefabs.StandardFont, "top: " + (_relativePosition.Y - 160) + ", bottom: " + (_relativePosition.Y - 80) + " size: (" + _size.X + ", " + _size.Y + ")", new Vector2(350, 150 + _relativePosition.Y * .2f), Color.White);

            var textPos = _relativePosition + new Vector2(_relativeSize.X / 2f, _relativeSize.Y / 4f);
            _text.SetPos(textPos);
            _text.SetScale(Vector2.One * (IsSelected ? 1.5f:1f));

            _rect = new Rectangle((int)(_relativePosition.X), (int)(_relativePosition.Y), (int)(_relativeSize.X), (int)(_relativeSize.Y * multiplier));
            spriteBatch.Draw(_buttonRect, _rect, Color.White);
        }
    }
}
