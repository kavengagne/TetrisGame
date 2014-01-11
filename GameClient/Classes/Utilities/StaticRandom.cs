using System;

namespace GameClient.Classes.Utilities
{
    public static class StaticRandom
    {
        #region Fields
        private static readonly Random Rand = new Random(); 
        #endregion


        #region Public Methods
        public static int Next(int from, int to)
        {
            return RandomNext(from, to);
        } 
        #endregion


        #region Internal Implementation
        private static int RandomNext(int from, int to)
        {
            if (to == 0 || to < from)
            {
                return 0;
            }
            return Rand.Next(from, to);
        } 
        #endregion
    }
}
