using System;
using Microsoft.Xna.Framework;

namespace GameClient.Classes.Core.Inputs
{
    internal class KeyPressedAction : KeyAction
    {
        #region Constructors
        public KeyPressedAction(Action action, bool repeatAction, int delayMilliseconds) : base(action, repeatAction, delayMilliseconds) { }
        #endregion


        #region Internal Implementation
        protected override bool IsDelayExpired(GameTime time)
        {
            if (Math.Ceiling(DelayCurrent) >= DelayMilliseconds || IsFirstTrigger())
            {
                DelayCurrent = time.ElapsedGameTime.TotalMilliseconds;
                return true;
            }
            DelayCurrent += time.ElapsedGameTime.TotalMilliseconds;
            return false;
        }

        private bool IsFirstTrigger()
        {
            return Math.Ceiling(Math.Abs(DelayCurrent)) < Double.Epsilon;
        }
        #endregion
    }
}
