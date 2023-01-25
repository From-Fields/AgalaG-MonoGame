using Microsoft.Xna.Framework;

namespace agalag.engine
{
    public class Transform 
    {
        //Attributes
        public Vector2 position;
        public Vector2 scale;
        public float rotation;

        //Constructors
        public Transform(Vector2 position, Vector2 scale, float rotation) 
        {
            this.position = position;
            this.scale = scale;
            this.rotation = rotation;
        }
    }
}