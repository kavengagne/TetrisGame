using System;
using Microsoft.Xna.Framework.Input;

namespace GameClient.Classes.Inputs
{
    internal class KeyReleasedAction
    {
        #region Fields
        private bool _isKeyPressed;
        private readonly Action _action; 
        #endregion


        #region Constructors
        public KeyReleasedAction(Action action)
        {
            _action = action;
        } 
        #endregion


        #region Public Methods
        public void Update(Keys key)
        {
            if (Keyboard.GetState().IsKeyDown(key) && !_isKeyPressed)
            {
                _isKeyPressed = true;
            }
            if (Keyboard.GetState().IsKeyUp(key) && _isKeyPressed)
            {
                _isKeyPressed = false;
                if (_action != null)
                {
                    _action.Invoke();
                }
            }
        } 
        #endregion
    }
}