using System;
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
        public static Vector2 normalized(this Vector2 vector) 
        {
            if(vector.Length() == 0)
                return vector;

            Vector2 newVector = new Vector2(vector.X, vector.Y);
            newVector.Normalize();

            return newVector;
        }
        public static Vector2 Reflect(this Vector2 vector, Vector2 normal)
        {
            if(normal.Length() != 1)
                normal.Normalize();
            
            return vector - 2 * (Vector2.Dot(vector, normal) * normal);
        }
        public static Vector2 Abs(this Vector2 vector)
        {            
            return new Vector2(MathF.Abs(vector.X), MathF.Abs(vector.Y));
        }
        public static Vector2 Negative(this Vector2 vector)
        {            
            return -1 * new Vector2(MathF.Abs(vector.X), MathF.Abs(vector.Y));
        }


        public static float Repeat(float value, float max) 
        {
            if(value == 0)
                return 0;

            float remainder = value;

            if(max > 0) 
            {
                while(remainder < 0 || remainder > max)
                {
                    if(remainder > max)
                        remainder = 0 - (max - remainder);
                    if(remainder < 0)
                        remainder = max + remainder;
                }
            }
            else
            {
                while(remainder < 0 || remainder > max)
                {
                    if(remainder < max)
                        remainder = 0 - (max + remainder);
                    if(remainder > 0)
                        remainder = 0 - remainder;
                }
            }

            return remainder;
        }
    }
}