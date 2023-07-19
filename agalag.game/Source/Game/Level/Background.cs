using agalag.engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace agalag.game
{
    public class Background : MonoEntity
    {

        private Vector2 _speed, _currentOffset;
        private Rectangle _target;

        public Background(Texture2D sprite, Rectangle target, Vector2? speed = null)
            :base(active: true, layer: Layer.Default)
        {
            this._sprite = new Sprite(sprite);
            this._target = target;
            this._speed = (speed.HasValue) ? speed.Value : new Vector2(0, -1);
            _currentOffset = Vector2.Zero;
        }

        public void setSpeed(Vector2 newSpeed) => _speed = newSpeed;

        public override void Update(GameTime gameTime)
        {
            _currentOffset = Vector2.Lerp(_currentOffset, _currentOffset + _speed, (float) gameTime.ElapsedGameTime.TotalSeconds * 100);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            Rectangle source = new Rectangle((int) _currentOffset.X, (int)_currentOffset.Y, _sprite.Texture.Width, _sprite.Texture.Height);
            spriteBatch.Draw(_sprite.Texture, _target, source, Color.White);
        }

        public override void OnCollision(MonoEntity other) { } // Do Nothing
    }
}