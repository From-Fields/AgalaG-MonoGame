using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace agalag.engine
{
    public interface iCollider 
    {
        //Attributes
        public Vector2 Origin { get; }
        public Transform Parent { get; set; }
        
        public FlattenedPolygon FlattenedPolygon { get; }

        //Methods
        public bool HasCollided(iCollider other);
        public void Draw(SpriteBatch spriteBatch);
    }

    public class FlattenedPolygon
    {
        public Vector2 xDelta;
        public Vector2 yDelta;

        public FlattenedPolygon(Vector2 xDelta, Vector2 yDelta) 
        {
            this.xDelta = xDelta;
            this.yDelta = yDelta;
        }

        public bool Intersects(FlattenedPolygon other)
        {
            bool xIntersectsA = (this.xDelta.X <= other.xDelta.X && other.xDelta.X <= this.xDelta.Y);
            bool xIntersectsB = (this.xDelta.X <= other.xDelta.Y && other.xDelta.Y <= this.xDelta.Y);

            bool xIntersects = xIntersectsA || xIntersectsB;

            bool yIntersectsA = (this.yDelta.X <= other.yDelta.X && other.yDelta.X <= this.yDelta.Y);
            bool yIntersectsB = (this.yDelta.X <= other.yDelta.Y && other.yDelta.Y <= this.yDelta.Y);

            bool yIntersects = yIntersectsA || yIntersectsB;

            return xIntersects && yIntersects;
        }
    }
}