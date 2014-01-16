using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GameClient.Classes.Utilities;
using GameConfiguration.DataObjects;
using Microsoft.Xna.Framework;
using Color = Microsoft.Xna.Framework.Color;

namespace GameClient.Classes.GameBoard
{
    public class PieceGenerator
    {
        #region Fields
        private readonly Board _board;
        private readonly PieceInformation[] _pieces;
        private readonly Color[] _colors;
        private readonly Rectangle _blockSize;
        private Piece _nextPiece;
        #endregion


        #region Constructors
        public PieceGenerator(Board board, PieceInformation[] pieces, Color[] colors, Rectangle blockSize)
        {
            _board = board;
            _pieces = pieces;
            _colors = colors;
            _blockSize = blockSize;

            _nextPiece = GetRandomPiece();

            // TODO: KG - May use this in Piece Editor
            var colorNames = new Dictionary<String, Color>();
            foreach (var color in colors)
            {
                var properties = typeof(Color).GetProperties(BindingFlags.Static | BindingFlags.Public);
                var currentColor = color;
                foreach (var propertyInfo in properties)
                {
                    var col = (Color)propertyInfo.GetValue(null);
                    if (col == currentColor)
                    {
                        colorNames.Add(propertyInfo.Name, currentColor);
                        break;
                    }
                }
            }
            foreach (var colorName in colorNames)
            {
                //Console.WriteLine("{0} = {1}", colorName.Key, colorName.Value);
            }
        }
        #endregion


        #region Public Methods
        public Piece GetPiece()
        {
            return ConsumedPiece();
        }

        public Piece PeekNextPiece()
        {
            return _nextPiece;
        }
        #endregion


        #region Internal Implementation
        private Piece GetRandomPiece()
        {
            var color = _colors[StaticRandom.Next(0, _colors.Length)];
            var model = new PieceModel(_pieces[StaticRandom.Next(0, _pieces.Length)]);
            var rotationIndex = StaticRandom.Next(0, model.Length);
            return new Piece(_board, color, model, rotationIndex, _blockSize);
        }

        private Piece ConsumedPiece()
        {
            var resultPiece = _nextPiece;
            _nextPiece = GetRandomPiece();
            return resultPiece;
        }
        #endregion
    }
}