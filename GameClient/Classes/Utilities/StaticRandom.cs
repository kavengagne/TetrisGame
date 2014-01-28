using System;

namespace GameClient.Classes.Utilities
{
    public static class StaticRandom
    {
        #region Fields
        private static readonly Random RandGenerator = new Random();
        #endregion


        #region Public Methods
        public static int Next(int to)
        {
            return Next(0, to);
        }
        public static int Next(int from, int to)
        {
            return RandomNext(from, to);
        }
        #endregion


        #region Intenal Implementation
        private static int RandomNext(int minValue, int maxValue)
        {
            return RandGenerator.Next(minValue, maxValue);
        }
        #endregion
    }
}
