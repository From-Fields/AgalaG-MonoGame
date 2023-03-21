using agalag.engine.utils;
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

        public void AddElement(UIElement e)
        {
            if (_elements.Contains(e)) return;

            _elements.Add(e);
        }

        public void RemoveElement(UIElement e)
        {
            _elements.Remove(e);
        }

        public void Update(GameTime gameTime)
        {
            foreach (var e in _elements)
            {
                e.Update(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var e in _elements)
            {
                e.Draw(spriteBatch);
            }
        }
    }
}
