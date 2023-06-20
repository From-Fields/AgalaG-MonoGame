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
        public Vector2 ClosestPoint(Vector2 position);
    }

    public class FlattenedPolygon
    {
        public float minX;
        public float maxX;
        public float minY;
        public float maxY;

        public FlattenedPolygon(float minX, float maxX, float minY, float maxY) 
        {
            this.minX = minX;
            this.maxX = maxX;
            this.minY = minY;
            this.maxY = maxY;
        }

        public bool Intersects(FlattenedPolygon other)
        {
            bool xIntersectsA = (other.minX >= this.minX && other.minX <= this.maxX);
            bool xIntersectsB = (other.maxX >= this.minX && other.maxX <= this.maxX);

            bool xIntersects = xIntersectsA || xIntersectsB;

            bool yIntersectsA = (other.minY >= this.minY && other.minY <= this.maxY);
            bool yIntersectsB = (other.maxY >= this.minY && other.maxY <= this.maxY);

            bool yIntersects = yIntersectsA || yIntersectsB;

            return xIntersects && yIntersects;
        }
    }
}