using agalag.engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;

namespace agalag.test 
{
    internal class SpriteTest : MonoEntity
    {
        public SpriteTest(Texture2D sprite, Vector2 position): base(sprite, position) { }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if(this._sprite != null) {
                spriteBatch.Draw(
                    this._sprite,
                    this._transform.position,
                    Color.White
                );
            }
        }

        public override void FixedUpdate(GameTime gameTime)
        {
            //
        }

        public override void Update(GameTime gameTime)
        {
            //
        }
    }
}