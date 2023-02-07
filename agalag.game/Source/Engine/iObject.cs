using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace agalag.engine
{
    public interface iObject {
        public void Update(GameTime gameTime);
        public void Draw(SpriteBatch spriteBatch);
        public void FixedUpdate(GameTime gameTime, FixedFrameTime fixedFrameTime);
    }
}