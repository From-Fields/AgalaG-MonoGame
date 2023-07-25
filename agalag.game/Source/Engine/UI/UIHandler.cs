using agalag.engine.utils;
using agalag.game.input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace agalag.engine
{
    public class UIHandler : Singleton<UIHandler>
    {
        private List<UIElement> _elements = new();

        private LinkedListNode<UIElement> _selected = null;
        private LinkedList<UIElement> _interactable = new();

        public UIElement Selected => _selected?.Value;

        public void AddElement(UIElement e)
        {
            if (_elements.Contains(e)) return;

            _elements.Add(e);
        }

        public void RemoveElement(UIElement e)
        {
            _elements.Remove(e);
        }

        public void AddToInteractable(UIElement e)
        {
            if (!_elements.Contains(e) || _interactable.Contains(e)) return;

            _interactable.AddLast(new LinkedListNode<UIElement>(e));
        }

        public UIElement PreviousInteractable()
        {
            if (_selected == null || _selected.Previous == null)
            {
                _selected = _interactable.Last;
            }
            else
            {
                _selected = _selected.Previous;
            }

            return _selected?.Value;
        }

        public UIElement NextInteractable()
        {
            if (_selected == null || _selected.Next == null)
            {
                _selected = _interactable.First;
            }
            else
            {
                _selected = _selected.Next;
            }

            return _selected?.Value;
        }

        public void SetSelected(UIElement e)
        {
            LinkedListNode<UIElement> item = null;
            if (e != null)
            {
                item = _interactable.Find(e);
            }
            _selected = item;
        }

        public void Clean()
        {
            _elements.Clear();
            _interactable.Clear();
            _selected = null;
        }

        public void Update(GameTime gameTime)
        {
            if (InputHandler.Instance.PressedUp())
                PreviousInteractable();
            if (InputHandler.Instance.PressedDown())
                NextInteractable();

            var elements = new List<UIElement>(_elements); // evitar bug de alteração de elementos durante iteração
            foreach (var e in elements)
            {
                if(e.IsActive)
                    e.Update(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            var elements = new List<UIElement>(_elements);
            foreach (var e in elements)
            {
                if(e.IsActive)
                    e.Draw(spriteBatch);
            }
        }
    }
}
