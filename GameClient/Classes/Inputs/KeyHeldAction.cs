using System;
using Microsoft.Xna.Framework;

namespace GameClient.Classes.Inputs
{
    internal class KeyHeldAction : KeyAction
    {
        #region Constructors
        public KeyHeldAction(Action action, bool repeatAction, int delayMilliseconds) : base(action, repeatAction, delayMilliseconds) { }
        #endregion


        #region Internal Implementation
        protected override bool IsDelayExpired(GameTime time)
        {
            DelayCurrent += time.ElapsedGameTime.TotalMilliseconds;
            if (Math.Ceiling(DelayCurrent) >= DelayMilliseconds)
            {
                DelayCurrent = time.ElapsedGameTime.TotalMilliseconds;
                return true;
            }
            return false;
        }
        #endregion
    }
}