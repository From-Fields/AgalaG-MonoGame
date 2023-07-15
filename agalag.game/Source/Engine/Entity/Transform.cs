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
        public bool simulate = false;

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
            if(simulate)
            {
                position = Vector2.LerpPrecise(position, position + velocity, FixedUpdater.FixedFrameTime.frameTime);
                velocity = Vector2.Lerp(velocity, Vector2.Zero, FixedUpdater.FixedFrameTime.frameTime * drag);
            }
        }

        public void Reset()
        {
            this.velocity = Vector2.Zero;
            this.scale = Vector2.One;
            this.rotation = 0f;
        }

        public void Rotate(float rotation_) {
            rotation_ += this.rotation;
            rotation_ = ExtensionMethods.Repeat(rotation_, 360);
            // System.Diagnostics.Debug.WriteLine(rotation_);

            this.rotation = rotation_;
        }
    }
}