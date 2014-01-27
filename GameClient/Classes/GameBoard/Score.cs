using System;
using System.Globalization;

namespace GameClient.Classes.GameBoard
{
    public class Score
    {
        #region Properties
        public int Points { get; private set; }
        public int Lines { get; private set; }
        #endregion


        #region Constructor
        public Score(int initialPoints = 0, int initialLines = 0)
        {
            Points = initialPoints;
            Lines = initialLines;
        }
        #endregion


        #region Public Methods
        public int IncrementPointsBy(int value)
        {
            value = Math.Abs(value);
            //Console.WriteLine("before: {0}, after: {1}", Points, Points + value);
            return Points += value;
        }

        public int IncrementLinesBy(int value)
        {
            value = Math.Abs(value);
            //Console.WriteLine("before: {0}, after: {1}", Lines, Lines + value);
            return Lines += value;
        }
        #endregion
    }
}