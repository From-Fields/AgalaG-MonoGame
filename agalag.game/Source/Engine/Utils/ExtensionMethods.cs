using agalag.engine;
using Microsoft.Xna.Framework;

namespace agalag.engine
{
    public static class ExtensionMethods
    {
        public static float Angle(this Vector2 vector, Vector2 other)
        {
            return (float)((180 / System.Math.PI) * System.Math.Atan2(vector.Y - other.Y, vector.X - other.X));
        }
        public static Vector2 normalized(this Vector2 vector) {
            if(vector.Length() == 0)
                return vector;

            Vector2 newVector = new Vector2(vector.X, vector.Y);
            newVector.Normalize();

            return newVector;
        }
    }
}