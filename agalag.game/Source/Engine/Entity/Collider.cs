using Microsoft.Xna.Framework;

namespace agalag.engine
{
    public class Collider 
    {
        //Attributes
        public Vector2 origin;
        public Vector2 dimensions;
        private Transform _parent;

        //Properties
        public Vector2 Position => _parent.position + origin;

        //Constructors
        public Collider() 
        {
            this.origin = Vector2.Zero;
            this.dimensions = Vector2.One;
        }
        public Collider(Vector2 origin, Vector2 dimensions) 
        {
            this.origin = origin;
            this.dimensions = dimensions;
        }
        public Collider(Vector2 origin, Vector2 dimensions, Transform parent) 
        {
            this.origin = origin;
            this.dimensions = dimensions;
            this._parent = parent;
        }

        //Methods
        public bool SetParent(Transform parent) 
        {   
            if(parent == null)
                return false;

            this._parent = parent;
            return true;
        }
    }
}