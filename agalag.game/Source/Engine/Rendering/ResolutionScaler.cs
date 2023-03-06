
using Microsoft.Xna.Framework;

namespace agalag.engine
{
    public static class ResolutionScaler
    {
        //Attributes
        private static int _internalWidth, _internalHeight;
        private static int _finalWidth, _finalHeight;
        private static Matrix _resolutionScaler;

        //Properties
        public static Matrix ResolutionMatrix => _resolutionScaler;
        public static Resolution InternalResolution => new Resolution(_internalWidth, _internalHeight);
        public static Resolution ExternalResolution => new Resolution(_finalWidth, _finalHeight);

        //Methods
        private static void SetAllResolutions(int internalWidth, int internalHeight, int width, int height)
        {
            SetInternalResolution(internalWidth, internalHeight);
            SetExternalResolution(width, height);
        }
        private static void SetInternalResolution(int width, int height)
        {
            _internalHeight = height;
            _internalWidth = width;
        }
        private static void SetExternalResolution(int width, int height)
        {
            _finalWidth = width;
            _finalHeight = height;
            
            float scaleX = (float)width / _internalWidth;
            float scaleY = (float)height / _internalHeight;

            _resolutionScaler = Matrix.CreateScale(scaleX, scaleY, 1.0f);
        }

        public static void SetResolution(
            GraphicsDeviceManager _graphics, int internalWidth, int internalHeight, 
            int width, int height, bool fullscreen = false
        ) {
            SetAllResolutions(internalWidth, internalHeight, width, height);

            _graphics.PreferredBackBufferWidth = width;
            _graphics.PreferredBackBufferHeight = height;
            _graphics.IsFullScreen = fullscreen;
            _graphics.ApplyChanges();
        }
    }

    public struct Resolution
    {
        public int width;
        public int height;

        public Resolution(int width, int height)
        {
            this.width = width;
            this.height = height;
        }
    }
}