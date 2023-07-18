using System.Diagnostics;
using agalag.engine.utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace agalag.game.input
{
    public struct KeyboardScheme 
    {
        public Keys up, down, left, right, shoot, pause;

        public KeyboardScheme() 
        {
            up = Keys.W;
            down = Keys.S;
            left = Keys.A;
            right = Keys.D;
            shoot = Keys.J;
            pause = Keys.Escape;
        }
    }

    public class KeyboardHandler: Singleton<KeyboardHandler>, iInputHandler
    {
        //Attributes
        public KeyboardScheme inputScheme { get; private set; } = new KeyboardScheme();

        private KeyboardState? _oldState = null;
        private KeyboardState? _currentState = null;

        //Methods
         private int KeyToInt(Keys key) 
        {
            return _currentState.Value.IsKeyDown(key) ? 1 : 0;
        }
        
		private bool GetKeyTap(Keys key)
        {
            if(_oldState != null && _currentState != null) 
            {
                return(_oldState.Value.IsKeyUp(key) && _currentState.Value.IsKeyDown(key));
            }

            return false;
        }
        
		private bool GetKeyRelease(Keys key)
        {
            if(_oldState != null && _currentState != null) 
            {
                return(_oldState.Value.IsKeyDown(key) && _currentState.Value.IsKeyUp(key));
            }

            return false;
        }
        
		private bool GetKeyDown(Keys key)
        {
            return (_currentState != null && _currentState.Value.IsKeyDown(inputScheme.shoot));
        }

        #region InterfaceImplementation
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
            return GetKeyTap(inputScheme.pause);
        }
		
        public bool GetShoot()
        {
            return KeyToInt(inputScheme.shoot) > 0;
        }
		
		public bool UpPressed()
        {
            return GetKeyTap(inputScheme.up);
        }

        public bool DownPressed()
        {
            return GetKeyTap(inputScheme.down);
        }

        public bool PressTestButton(Keys key)
        {
            return GetKeyTap(key);
        }

        public void Update()
        {
            _oldState = _currentState;
            _currentState = Keyboard.GetState();
        }
        #endregion
    }
}