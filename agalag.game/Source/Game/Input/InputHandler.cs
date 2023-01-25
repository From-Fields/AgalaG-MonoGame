using System.Diagnostics;
using agalag.engine.utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace agalag.game
{
    public struct BasicInput 
    {
        public Keys up, down, left, right, shoot, pause;

        public BasicInput() 
        {
            up = Keys.W;
            down = Keys.S;
            left = Keys.A;
            right = Keys.D;
            shoot = Keys.J;
            pause = Keys.Escape;
        }
    }

    public class InputHandler: Singleton<InputHandler>
    {
        public BasicInput inputScheme { get; private set; } = new BasicInput();
        public bool usingGamepad = false;

        private KeyboardState? _oldState = null;
        private KeyboardState? _currentState = null;

        public bool HasMovement => (GetMovement() != Vector2.Zero);
        public Vector2 GetMovement() 
        {
            if(_currentState != null)
            {
                int x = KeyToInt(inputScheme.right) - KeyToInt(inputScheme.left);
                int y = KeyToInt(inputScheme.down) - KeyToInt(inputScheme.up);
                
                return new Vector2(x, y);
            }

            return Vector2.Zero;
        }

        public bool GetPause() 
        {
            if(_oldState != null && _currentState != null) 
            {
                return(_oldState.Value.IsKeyUp(inputScheme.pause) && !_oldState.Value.IsKeyDown(inputScheme.pause));
            }

            return false;
        }

        public bool GetShoot()
        {
            return (_currentState != null && _currentState.Value.IsKeyDown(inputScheme.shoot));
        }

        private int KeyToInt(Keys key) 
        {
            return _currentState.Value.IsKeyDown(key) ? 1 : 0;
        }

        public void Update()
        {
            if(!usingGamepad)
            {
                _oldState = _currentState;
                _currentState = Keyboard.GetState();
            }
        }
    }
}