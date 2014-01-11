using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using GameClient.Classes.Utilities;
using GameConfiguration.DataObjects;
using Color = Microsoft.Xna.Framework.Color;

namespace GameClient.Classes.GameBoard
{
    public class PieceGenerator
    {
        #region Fields
        private readonly Board _board;
        private readonly PieceInformation[] _pieces;
        private readonly Color[] _colors;
        private readonly Size _blockSize;
        #endregion


        #region Constructors
        public PieceGenerator(Board board, PieceInformation[] pieces, Color[] colors, Size blockSize)
        {
            _board = board;
            _pieces = pieces;
            _colors = colors;
            _blockSize = blockSize;

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
            var color = _colors[StaticRandom.Next(0, _colors.Length)];
            var model = new PieceModel(_pieces[StaticRandom.Next(0, _pieces.Length)]);
            var rotationIndex = StaticRandom.Next(0, model.Length);
            return new Piece(_board, color, model, rotationIndex, _blockSize);
        }
        #endregion


        #region Internal Implementation
        
        #endregion
    }
}