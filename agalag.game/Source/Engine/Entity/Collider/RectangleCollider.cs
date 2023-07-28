using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace agalag.engine
{
    public class RectangleCollider : iCollider
    {
        private bool _solid;
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

        public RectangleCollider(Point dimensions, bool solid = true, Vector2? anchor = null, Point? offset = null)
        {
            Point offsetValue = (offset == null) ? Point.Zero : offset.Value;
            Vector2 anchorValue = (anchor == null) ? new Vector2(0.5f, 0.5f) : anchor.Value;

            this._dimensions = dimensions;
            this.offset = offsetValue;
            this.anchor = anchorValue;
            this._solid = solid;
        }


        //Interface Implementation
        public Vector2 Origin { 
            get => _parent.position + offset.ToVector2() - (anchor * _dimensions.ToVector2() * Parent.scale);
        }
        public Vector2 Dimensions => _dimensions.ToVector2();

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
        public bool IsSolid => _solid;

        public bool HasCollided(iCollider other) => this.FlattenedPolygon.Intersects(other.FlattenedPolygon) || other.FlattenedPolygon.Intersects(this.FlattenedPolygon);

        public void Draw(SpriteBatch spriteBatch)
        {
//#if !DEBUG
//            return;
//#endif

//            Texture2D whiteRectangle = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
//            whiteRectangle.SetData(new[] { Color.White });
            
//            Color colour = new Color(Color.Blue, 20);

//            spriteBatch.Draw(whiteRectangle, Origin, _Rect, colour);
        }

        public Vector2 ClosestPoint(Vector2 position) {
            float maxX = Origin.X + _dimensions.X;
            float maxY = Origin.Y + _dimensions.Y;

            float minX = Origin.X;
            float minY = Origin.Y;

            float x = MathHelper.Max(minX, MathHelper.Min(position.X, maxX));
            float y = MathHelper.Max(minY, MathHelper.Min(position.Y, maxY));

            return new Vector2(x, y);
        }

        public void SetSolid(bool solid)
        {
            this._solid = solid;
        }
    }
}