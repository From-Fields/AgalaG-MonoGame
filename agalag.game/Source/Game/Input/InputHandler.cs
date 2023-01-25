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

        public bool SwitchInputMethod(InputMethods method)
        {
            iInputHandler handler = GetInputMethod(method);

            if(handler == null)
                return false;
            
            _inputMethod = handler;
            return true;
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