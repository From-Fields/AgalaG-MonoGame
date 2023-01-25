using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace agalag.engine
{
    public interface iParentObject {
        public void UpdateChildren(GameTime gameTime);
        public void DrawChildren(SpriteBatch spriteBatch);
        public void FixedUpdateChildren(GameTime gameTime);
    }
}