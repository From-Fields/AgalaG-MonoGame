using agalag.engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace agalag.game
{
    public class Wall : MonoEntity
    {
        public Wall(Vector2 dimensions, Vector2 position, Layer layer = Layer.Entities, EntityTag tag = EntityTag.Wall)
            :base(layer: layer, active: true)
        {
            _transform.simulate = false;
            _transform.position = position;
            SetCollider(new RectangleCollider(new Point((int)dimensions.X, (int) dimensions.Y), anchor: Vector2.Zero));
            SetTag(tag);
        }

        public override void Draw(SpriteBatch spriteBatch) { }

        public override void OnCollision(MonoEntity other) { }
    }
}