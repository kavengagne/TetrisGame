using System.Linq;
using GameClient.Classes.Core.Randomizer;
using GameClient.Classes.Core.Settings;

namespace GameClient.Classes.GameBoard.Pieces
{
    public class PieceGenerator
    {
        #region Fields
        private readonly Board _board;
        private readonly PieceInformation[] _pieces;
        private PreviewPiece _nextPiece;
        private readonly RandomBag _randomBag;
        #endregion


        #region Constructors
        public PieceGenerator(Board board)
        {
            _board = board;
            _pieces = Defaults.Pieces;
            _randomBag = new RandomBag(_pieces.Length);

            //DeterminePiecesColors();

            _nextPiece = GetRandomPiece();
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
        //private void DeterminePiecesColors()
        //{
        //    _colors = GetShuffledColors();
        //    for (int i = 0; i < _pieces.Length; i++)
        //    {
        //        _pieces[i].Color = _colors[i];
        //    }
        //}

        //private Color[] GetShuffledColors()
        //{
        //    var randNumbers = new List<int>();
        //    foreach (var color in _colors)
        //    {
        //        int num;
        //        do
        //        {
        //            num = StaticRandom.Next(_colors.Length);
        //        }
        //        while (randNumbers.Contains(num));
        //        randNumbers.Add(num);
        //    }
        //    return randNumbers.Select(item => _colors[item]).ToArray();
        //}

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