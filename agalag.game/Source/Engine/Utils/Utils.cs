using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace agalag.engine
{
    public static class Utils
    {
        public static GraphicsDeviceManager ScreenManager { get; set; }

        public static int ScreenWidth => GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
        public static int ScreenHeight => GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;

        public static Texture2D DrawRectangle(Color color)
        {
            var rect = new Texture2D(ScreenManager.GraphicsDevice, 1, 1);
            rect.SetData(new[] { color });

            return rect;
        }

        public static Texture2D DrawRectangle(out Rectangle rect, int x, int y, int width, int height, Color color)
        {
            var texture = new Texture2D(ScreenManager.GraphicsDevice, 1, 1);
            texture.SetData(new[] { color });

            rect = new Rectangle(x, y, width, height);

            return texture;
        }
    }
}
