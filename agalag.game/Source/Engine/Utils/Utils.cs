using Microsoft.Xna.Framework.Graphics;

namespace agalag.engine
{
    public static class Utils
    {
        public static int ScreenWidth => GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
        public static int ScreenHeight => GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
    }
}
