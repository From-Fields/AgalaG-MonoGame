using agalag.engine.content;
using agalag.game.input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace agalag.engine
{
    class UISlider : UIElement
    {
        private Vector2 _size;
        private Color _color = Color.Gray;
        private int _value;

        private UIIcon _indicator;
        private Color _hoverColor = Color.OrangeRed;

        private readonly Vector2 _interactableOffset = new Vector2(30f, 30f);

        private Action<int> _onClick = null;

        private bool IsSelected => UIHandler.Instance.Selected == this;

        public UISlider(Vector2 position, Vector2 size, int initialValue, Action<int> action = null) : base(position)
        {
            _size = size;
            _value = initialValue;
            _onClick = action;

            UIHandler.Instance.AddToInteractable(this);

            var indicatorSize = new Vector2(size.X * .1f, size.Y * 5f);
            var indicatorPos = position - new Vector2(- (indicatorSize.X * initialValue), indicatorSize.Y * .35f);

            _indicator = new UIIcon(indicatorPos, indicatorSize, Color.White);
        }

        public override void Update(GameTime gameTime)
        {
            if (!_active) return;

            Vector2 mousePosition = InputHandler.Instance.ScaledMousePosition;

            float bbox_left = _transform.position.X - _interactableOffset.X;
            float bbox_right = _transform.position.X + _size.X + _interactableOffset.X;
            float bbox_top = _transform.position.Y - _interactableOffset.Y;
            float bbox_bottom = _transform.position.Y + _size.Y + _interactableOffset.Y;

            if (mousePosition.X >= bbox_left && mousePosition.X <= bbox_right
                && mousePosition.Y >= bbox_top && mousePosition.Y <= bbox_bottom)
            {
                UIHandler.Instance.SetSelected(this);
            }
            else if (IsSelected)
            {
                UIHandler.Instance.Desselect(this);
            }

            if (IsSelected && InputHandler.Instance.GetMouseLeft())
            {
                _indicator.SetColor(_hoverColor);
                _indicator.Transform.position.X = MathHelper.Clamp(mousePosition.X, _transform.position.X, Transform.position.X + _size.X);

                _value = GetValueByPosition(_indicator.Transform.position.X, _transform.position.X, _size.X);

                if (_onClick == null)
                {
                    Debug.WriteLine("Action was not assigned to this button.");
                    return;
                }
                _onClick.Invoke(_value);
            }
            else
            {
                _indicator.SetColor(Color.White);
            }
        }

        private int GetValueByPosition(float position, float offset, float relativeTo)
        {
            int _value = 0;

            for (int i = 1; i <= 10; i += 1)
            {
                if (position - offset >= relativeTo * (i / 10f) && position - offset < relativeTo * ((i + 1) / 10f))
                {
                    _value = i;
                    break;
                }
            }

            return _value;
        }

        public override void SetActive(bool active)
        {
            _indicator.SetActive(active);
            base.SetActive(active);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!_active) return;

            spriteBatch.DrawString(Prefabs.GetFont("Button"), _value.ToString(), Transform.position + new Vector2(_size.X + 50f, -10f), Color.White);

            var iconRect = Utils.DrawRectangle(out var _rect,
                (int)(_transform.position.X), (int)(_transform.position.Y),
                (int)(_size.X), (int)(_size.Y),
                _color);
            spriteBatch.Draw(iconRect, _rect, Color.White);
        }
    }
}
