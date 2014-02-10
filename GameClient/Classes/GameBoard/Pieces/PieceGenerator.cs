using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GameClient.Classes.Core;
using GameClient.Classes.Core.Randomizer;
using GameConfiguration.DataObjects;
using Microsoft.Xna.Framework;

namespace GameClient.Classes.GameBoard.Pieces
{
    public class PieceGenerator
    {
        #region Fields
        private readonly Board _board;
        private readonly PieceInformation[] _pieces;
        private Color[] _colors;
        private PreviewPiece _nextPiece;
        private readonly RandomBag _randomBag;
        #endregion


        #region Constructors
        public PieceGenerator(Board board, PieceInformation[] pieces, Color[] colors)
        {
            _board = board;
            _pieces = pieces;
            _colors = colors;
            _randomBag = new RandomBag(pieces.Length);

            DeterminePiecesColors();

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
                Console.WriteLine("{0} = {1}", colorName.Key, colorName.Value);
            }
        }
        #endregion


        #region Public Methods
        public Piece GetPiece()
        {
            _board.CanExchangePiece = true;
            return ConsumedPiece();
        }

        public void SetNextPiece(PreviewPiece piece)
        {
            _nextPiece = piece;
        }

        public PreviewPiece PeekNextPiece()
        {
            return _nextPiece;
        }
        #endregion


        #region Internal Implementation
        private void DeterminePiecesColors()
        {
            _colors = GetShuffledColors();
            for (int i = 0; i < _pieces.Length; i++)
            {
                _pieces[i].Color = _colors[i];
            }
        }

        private Color[] GetShuffledColors()
        {
            var randNumbers = new List<int>();
            foreach (var color in _colors)
            {
                int num;
                do
                {
                    num = StaticRandom.Next(_colors.Length);
                }
                while (randNumbers.Contains(num));
                randNumbers.Add(num);
            }
            return randNumbers.Select(item => _colors[item]).ToArray();
        }

        private PreviewPiece GetRandomPiece()
        {
            var modelIndex = _randomBag.Next();
            var model = new PieceModel(_pieces[modelIndex]);
            return new PreviewPiece(_board, _pieces[modelIndex].Color, model, rotationIndex: 0);
        }

        private Piece ConsumedPiece()
        {
            var resultPiece = _nextPiece;
            _nextPiece = GetRandomPiece();
            return new Piece(_board, resultPiece);
        }
        #endregion
    }
}