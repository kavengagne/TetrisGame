using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GameClient.Classes.Inputs
{
    internal abstract class KeyAction
    {
        #region Fields
        protected readonly Action Action;
        protected readonly int DelayMilliseconds;
        protected readonly bool RepeatAction;

        protected double DelayCurrent;
        protected bool IsAlreadyTriggered; 
        #endregion


        #region Constructors
        protected KeyAction(Action action, bool repeatAction, int delayMilliseconds)
        {
            Action = action;
            RepeatAction = repeatAction;
            DelayMilliseconds = delayMilliseconds;
        } 
        #endregion


        #region Public Methods
        public void Update(GameTime time, Keys key)
        {
            if (Keyboard.GetState().IsKeyDown(key) && !IsAlreadyTriggered)
            {
                if (IsDelayExpired(time))
                {
                    if (!RepeatAction)
                    {
                        IsAlreadyTriggered = true;
                    }
                    if (Action != null)
                    {
                        Action.Invoke();
                    }
                }
            }
            if (Keyboard.GetState().IsKeyUp(key))
            {
                DelayCurrent = 0;
                IsAlreadyTriggered = false;
            }
        } 
        #endregion


        #region Abstract Methods
        protected abstract bool IsDelayExpired(GameTime time);
        #endregion
    }
}
