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
    }
}