using Microsoft.Xna.Framework;

namespace agalag.engine
{
    public class Transform 
    {
        //Attributes
        public Vector2 position;
        public Vector2 scale;
        public float rotation;
        public Vector2 velocity;
        public float drag = 0.5f;

        //Constructors
        public Transform(Vector2 position, Vector2 scale, float rotation) 
        {
            this.position = position;
            this.scale = scale;
            this.rotation = rotation;
        }

        //Methods
        public void ApplyVelocity()
        {
            position = Vector2.LerpPrecise(position, position + velocity, FixedUpdater.FixedFrameTime.frameTime);
            velocity = Vector2.Lerp(velocity, Vector2.Zero, FixedUpdater.FixedFrameTime.frameTime * drag);
        }
    }
}