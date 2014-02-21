using System.Collections.Generic;

namespace GameClient.Classes.Core.Randomizer
{
    public class RandomBag
    {
        #region Fields
        private List<int> _bag = new List<int>();
        #endregion


        #region Properties
        public int Size { get; private set; }
        #endregion


        #region Constructor
        public RandomBag(int size)
        {
            Size = size;
            RandomizeBag();
        }
        #endregion


        #region Public Methods
        public int Next()
        {
            RandomizeBag();
            int result = _bag[0];
            _bag.RemoveAt(0);
            return result;
        }
        #endregion


        #region Internal Implementation
        private void RandomizeBag()
        {
            if (_bag.Count == 0)
            {
                _bag = GetShuffledNumbers(Size);
            }
        }

        private static List<int> GetShuffledNumbers(int count)
        {
            var randNumbers = new List<int>();
            for (int i = 0; i < count; i++)
            {
                int num;
                do
                {
                    num = StaticRandom.Next(count);
                }
                while (randNumbers.Contains(num));
                randNumbers.Add(num);
            }
            return randNumbers;
        }
        #endregion
    }
}
