using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GameClient.Classes.GameBoard;
using GameClient.Classes.Utilities;
using GameConfiguration.DataObjects;
using Microsoft.Xna.Framework;

namespace GameClient.Classes.Core
{
    public class PieceGenerator
    {
        #region Fields
        private readonly TetrisGame _game;
        private readonly PieceInformation[] _pieces;
        private Color[] _colors;
        private readonly Rectangle _blockSize;
        private PreviewPiece _nextPiece;
        #endregion


        #region Constructors
        public PieceGenerator(TetrisGame game, PieceInformation[] pieces, Color[] colors, Rectangle blockSize)
        {
            _game = game;
            _pieces = pieces;
            _colors = colors;
            _blockSize = blockSize;

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
            return ConsumedPiece();
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
            //var color = _colors[StaticRandom.Next(0, _colors.Length)];
            var modelIndex = StaticRandom.Next(_pieces.Length);
            //var modelIndex = 2;
            var model = new PieceModel(_pieces[modelIndex]);
            var rotationIndex = StaticRandom.Next(model.Length);
            return new PreviewPiece(_game, _pieces[modelIndex].Color, model, rotationIndex, _blockSize);
        }

        private Piece ConsumedPiece()
        {
            var resultPiece = _nextPiece;
            _nextPiece = GetRandomPiece();
            return new Piece(_game, resultPiece);
        }
        #endregion
    }
}