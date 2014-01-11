using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GameConfiguration.DataObjects;
using Microsoft.Xna.Framework;

namespace GameClient.Classes.GameBoard
{
    public class PieceModel : IEnumerable<Point[]>
    {
        #region Fields
        private readonly IEnumerable<Point[]> _model;
        #endregion


        #region Properties
        public int Length
        {
            get { return _model.Count(); }
        }

        public Point[] this[int index]
        {
            get { return _model.Skip(index).First(); }
        }
        #endregion


        #region Constructors
        public PieceModel(PieceInformation pieceInformation)
        {
            _model = GetPieceModel(pieceInformation.Positions, pieceInformation.RotationsCount);
        }
        #endregion


        #region Internal Implementation
        private static IEnumerable<Point[]> GetPieceModel(Point[] points, int rotationsCount)
        {
            var list = new List<Point[]> { points };
            if (rotationsCount >= 2)
            {
                list.Add(points.Select(p => new Point(-p.Y, p.X)).ToArray());
            }
            if (rotationsCount >= 3)
            {
                list.Add(points.Select(p => new Point(-p.X, -p.Y)).ToArray());
            }
            if (rotationsCount >= 4)
            {
                list.Add(points.Select(p => new Point(p.Y, -p.X)).ToArray());
            }
            return list;
        }
        #endregion


        #region IEnumerable Interface
        public IEnumerator<Point[]> GetEnumerator()
        {
            return _model.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion
    }
}