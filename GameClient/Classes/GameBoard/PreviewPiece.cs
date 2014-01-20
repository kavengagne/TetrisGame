using System.Diagnostics;
using GameClient.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameClient.Classes.GameBoard
{
    public class PreviewPiece : PieceBase
    {
        #region Fields
        private readonly TetrisGame _game;
        #endregion


        #region Constructor
        public PreviewPiece(TetrisGame game, Color color, PieceModel model, int rotationIndex, Rectangle blockSize)
            : base(color, model, rotationIndex, blockSize)
        {
            _game = game;

            SetPiecePosition();
        }
        #endregion


        #region Overrides of PieceBase
        public override void Update(GameTime gameTime)
        {
            UpdateBlocksPositions(_game.PreviewPanel.Bounds.Location);
            base.Update(gameTime);
        }

        protected override void UpdateBlocksPositions(Point offset)
        {
            var positions = Model[RotationIndex];
            for (int i = 0; i < Blocks.Length; i++)
            {
                Blocks[i].X = positions[i].X * Blocks[i].Bounds.Width + Position.X + offset.X;
                Blocks[i].Y = positions[i].Y * Blocks[i].Bounds.Height + Position.Y + offset.Y;
            }
        }
        #endregion


        #region Internal Implementation
        private void SetPiecePosition()
        {
            Debug.WriteLine("Piece");
            foreach (var block in Blocks)
            {
                Debug.WriteLine("X:{0}, Y:{1}", block.X, block.Y);
            }
            Position = new Point(30, 30);
        }
        #endregion
    }
}