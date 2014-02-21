using System;

namespace GameClient.Classes.GameBoard
{
    public class Score
    {
        #region Properties
        public int Points { get; private set; }
        public int Lines { get; private set; }
        public event Action<Score, int> PointsUpdated;
        public event Action<Score, int> LinesUpdated;
        #endregion


        #region Constructor
        public Score(int initialPoints = 0, int initialLines = 0)
        {
            Points = initialPoints;
            Lines = initialLines;
        }
        #endregion


        #region Public Methods
        public void IncrementPointsBy(int value)
        {
            value = Math.Abs(value);
            //Console.WriteLine("before: {0}, after: {1}", Points, Points + value);
            Points += value;
            if (value > 0)
            {
                OnPointsUpdated(value);
            }
        }

        public void IncrementLinesBy(int value)
        {
            value = Math.Abs(value);
            //Console.WriteLine("before: {0}, after: {1}", Lines, Lines + value);
            Lines += value;
            if (value > 0)
            {
                OnLinesUpdated(value);
            }
        }
        #endregion


        #region Event Handling Method
        protected virtual void OnPointsUpdated(int updateValue)
        {
            var handler = PointsUpdated;
            if (handler != null)
            {
                handler(this, updateValue);
            }
        }

        protected virtual void OnLinesUpdated(int updateValue)
        {
            var handler = LinesUpdated;
            if (handler != null)
            {
                handler(this, updateValue);
            }
        }
        #endregion
    }
}
