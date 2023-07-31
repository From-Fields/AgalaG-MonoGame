using agalag.engine.content;
using agalag.game.input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace agalag.engine
{
    class UIToggleButton : UIElement
    {
        private bool _checked = false;
        private Action<bool> _onClick;
        private Vector2 _size;

        private Texture2D _buttonRect;
        private Color _color = Color.White;
        private Color _hoverColor = Color.Gray;

        private bool IsSelected => UIHandler.Instance.Selected == this;

        public override bool HasEffect => true;

        private Vector2 _relativePosition;
        private Vector2 _relativeSize;

        private UIIcon _checkedIcon;

        public UIToggleButton(Vector2 position, Vector2 size, Action<bool> action = null, bool isChecked = false, Color? hoverColor = null) : base(position)
        {
            _onClick = action;
            _size = size;
            _checked = isChecked;
            _hoverColor = hoverColor ?? _hoverColor;

            //Position correction
            UpdateRelativePosition();

            UIHandler.Instance.AddToInteractable(this);

            var boxSize = size * .6f;

            _checkedIcon = new UIIcon(position - boxSize * .5f, boxSize, Color.Orange);
            _checkedIcon.SetActive(isChecked);
        }

        private void UpdateRelativePosition()
        {
            _relativePosition = new Vector2(Transform.position.X - (_relativeSize.X / 2), Transform.position.Y - (_relativeSize.Y / 2));
        }

        public override void Update(GameTime gameTime)
        {
            if (!_active) return;

            Vector2 mousePosition = InputHandler.Instance.ScaledMousePosition;

            float bbox_left = _relativePosition.X;
            float bbox_right = _relativePosition.X + _size.X;
            float bbox_top = _relativePosition.Y;
            float bbox_bottom = _relativePosition.Y + _size.Y;

            if (mousePosition.X >= bbox_left && mousePosition.X <= bbox_right
                && mousePosition.Y >= bbox_top && mousePosition.Y <= bbox_bottom)
            {
                UIHandler.Instance.SetSelected(this);
            }
            else if (IsSelected)
            {
                UIHandler.Instance.Desselect(this);
            }

            if (IsSelected && InputHandler.Instance.GetMouseLeftPressed())
            {
                _checked = !_checked;
                _checkedIcon.SetActive(_checked);
                if (_onClick == null)
                {
                    Debug.WriteLine("Action was not assigned to this button.");
                    return;
                }
                _onClick.Invoke(_checked);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!_active) return;

            float multiplier = 1f;
            var color = (IsSelected) ? _hoverColor : _color;

            _relativeSize = _size * (IsSelected ? multiplier : 1f);
            UpdateRelativePosition();

            _buttonRect = Utils.DrawRectangle(out var _rect, (int)(_relativePosition.X), (int)(_relativePosition.Y), (int)(_relativeSize.X), (int)(_relativeSize.Y * multiplier) , color);
            spriteBatch.Draw(_buttonRect, _rect, Color.White);
        }
    }
}
