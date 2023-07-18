using System;
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
        public bool IsSolid { get; }

        //Methods
        public void SetSolid(bool solid);
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

        public FlattenedPolygon(Rectangle rect) 
        {
            this.minX = rect.Left;
            this.maxX = rect.Right;
            this.minY = rect.Top;
            this.maxY = rect.Bottom;
        }
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

        internal Rectangle toRectangle()
        {
            return new Rectangle((int) minX, (int) minY, (int) (maxX - minX), (int) (maxY - minY));
        }
    }
}