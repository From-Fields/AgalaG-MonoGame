using System.Diagnostics;
using agalag.engine.utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace agalag.game.input
{
    internal interface iInputHandler
    {
        public Vector2 GetMovement();
        public bool GetPause();
        public bool GetShoot();
        public void Update();
		public bool DownPressed();
        public bool UpPressed();
        public bool PressTestButton(Keys key);
    }

    public enum InputMethods 
    {
        Keyboard,
        Gamepad
    }

    public class InputHandler: Singleton<InputHandler>
    {
        public bool usingGamepad = false;
        private iInputHandler _inputMethod = new KeyboardHandler();

        public bool HasMovement => (GetMovement() != Vector2.Zero);
        public Vector2 GetMovement() => _inputMethod.GetMovement();

        public bool GetPause() => _inputMethod.GetPause();
        public bool GetShoot() => _inputMethod.GetShoot();
		
		public bool PressedDown() => _inputMethod.DownPressed();
        public bool PressedUp() => _inputMethod.UpPressed();

        public bool PressF2() => _inputMethod.PressTestButton(Keys.F2);
        public bool PressF3() => _inputMethod.PressTestButton(Keys.F3);


        public Vector2 GetMousePosition()
        {
            MouseState mouseState = Mouse.GetState();
            return new Vector2(mouseState.Position.X, mouseState.Position.Y);
        }

        public bool GetMouseLeftPressed()
        {
            MouseState mouseState = Mouse.GetState();
            return mouseState.LeftButton == ButtonState.Pressed;
        }

        public bool SwitchInputMethod(InputMethods method)
        {
            iInputHandler handler = GetInputMethod(method);

            if(handler == null)
                return false;
            
            _inputMethod = handler;
            return true;
        }
		
		public Vector2 ScaledMousePosition {
            get {
                Matrix scalingMatrix = engine.ResolutionScaler.ResolutionMatrix;
                MouseState mouseState = Mouse.GetState();

                Vector2 mousePosition = new Vector2(mouseState.X, mouseState.Y);
                Vector2 scaledMousePosition = Vector2.Transform(mousePosition, Matrix.Invert(scalingMatrix));

                return scaledMousePosition;
            }
        }

        private iInputHandler GetInputMethod(InputMethods method)
        {
            switch(method)
            {
                case (InputMethods.Keyboard):
                    return KeyboardHandler.Instance;
            }

            return null;
        }

        public void Update()
        {
            _inputMethod.Update();
        }
    }
}