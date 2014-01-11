using System;
using System.Globalization;

namespace GameClient.Classes.GameBoard
{
    public class Score
    {
        public int Points
        {
            get;
            private set;
        }

        public Score(int initialScore = 0)
        {
            Points = initialScore;
        }

        public override string ToString()
        {
            return Points.ToString(CultureInfo.InvariantCulture);
        }

        public int IncrementBy(int value)
        {
            value = Math.Max(Math.Abs(value), 0);
            Console.WriteLine("before: {0}, after: {1}", Points, Points + value);
            return Points += value;
        }

        public int DecrementBy(int value = 1)
        {
            return Points = Math.Max(Points - Math.Abs(value), 0);
        }
    }
}