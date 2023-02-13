using agalag.engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;

namespace agalag.test 
{
    internal class SpriteTest : MonoEntity
    {
        public SpriteTest(Texture2D sprite, Vector2 position): base(sprite, position) {
            this.SetCollider(new RectangleCollider(new Point(40, 40)));
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if(this._sprite != null) {
                this._sprite.Draw(Transform, spriteBatch);
            }
        }

        public override void FixedUpdate(GameTime gameTime, FixedFrameTime fixedFrameTime)
        {
            //
        }

        public override void OnCollision(MonoEntity other)
        {
            System.Diagnostics.Debug.WriteLine("Collided!");
        }

        public override void Update(GameTime gameTime)
        {
            //
        }
    }
}