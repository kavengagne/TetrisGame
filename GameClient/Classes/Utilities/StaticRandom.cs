using System;
using System.Security.Cryptography;

namespace GameClient.Classes.Utilities
{
    public static class StaticRandom
    {
        #region Fields
        private static readonly RNGCryptoServiceProvider RandGenerator = new RNGCryptoServiceProvider();
        #endregion


        #region Constructor
        static StaticRandom()
        {
        }
        #endregion


        #region Public Methods
        public static int Next(int to)
        {
            return RandomNext(to);
        }

        private static int RandomNext(int maxValue)
        {
            return GetRandomNumber((byte)maxValue);
        }
        #endregion


        #region Internal Implementation
        public static byte GetRandomNumber(byte numberSides)
        {
            var randomNumber = new byte[1];
            do
            {
                RandGenerator.GetBytes(randomNumber);
            }
            while (!IsFairRoll(randomNumber[0], numberSides));
            return (byte)((randomNumber[0] % numberSides));
        }

        // Taken from: http://msdn.microsoft.com/en-us/library/system.security.cryptography.rngcryptoserviceprovider.aspx
        private static bool IsFairRoll(byte roll, byte numSides)
        {
            // There are MaxValue / numSides full sets of numbers that can come up 
            // in a single byte. For instance, if we have a 6 sided dice, there are 
            // 42 full sets of 1-6 that come up. The 43rd set is incomplete. 
            int fullSetsOfValues = Byte.MaxValue / numSides;

            // If the roll is within this range of fair values, then we let it continue. 
            // In the 6 sided dice case, a roll between 0 and 251 is allowed. (We use 
            // < rather than <= since the = portion allows through an extra 0 value). 
            // 252 through 255 would provide an extra 0, 1, 2, 3 so they are not fair 
            // maxValue use. 
            return roll < numSides * fullSetsOfValues;
        }
        #endregion
    }
}
