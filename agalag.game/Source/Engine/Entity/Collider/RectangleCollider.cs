using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace agalag.engine
{
    public class RectangleCollider : iCollider
    {
        private Transform _parent;
        private Point _dimensions;
        public Vector2 anchor; 
        public Point offset; 
                
        private Rectangle _Rect 
        {
            get
            {
                Point dimensions = _dimensions + _parent.scale.ToPoint(); 

                return new Rectangle((int)Origin.X, (int)Origin.Y, dimensions.X, dimensions.Y);
            }
        }

        public RectangleCollider(Point dimensions, Vector2? _anchor = null, Point? _offset = null)
        {
            Point offsetValue = (_offset == null) ? Point.Zero : _offset.Value;
            Vector2 anchorValue = (_anchor == null) ? new Vector2(0.5f, 0.5f) : _anchor.Value;

            this._dimensions = dimensions;
            this.offset = offsetValue;
            this.anchor = anchorValue;
        }


        //Interface Implementation
        public Vector2 Origin { 
            get => _parent.position + offset.ToVector2() - (anchor * _dimensions.ToVector2() * Parent.scale);
        }

        public FlattenedPolygon FlattenedPolygon 
        {
            get => new FlattenedPolygon(
                _Rect.Left, _Rect.Right, _Rect.Top, _Rect.Bottom
            );
        }

        public Transform Parent 
        { 
            get => this._parent;
            
            set
            {
                if (value != null)
                    this._parent = value;
            } 
        }

        public bool HasCollided(iCollider other) => this.FlattenedPolygon.Intersects(other.FlattenedPolygon) || other.FlattenedPolygon.Intersects(this.FlattenedPolygon);

        public void Draw(SpriteBatch spriteBatch)
        {
            Texture2D whiteRectangle = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
            whiteRectangle.SetData(new[] { Color.White });
            
            Color colour = new Color(Color.Blue, 20);

            spriteBatch.Draw(whiteRectangle, Origin, _Rect, colour);
        }
    }
}