using System;
using System.Collections.Generic;
using System.Drawing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Color = Microsoft.Xna.Framework.Color;
using Point = Microsoft.Xna.Framework.Point;

namespace GameClient.Classes.GameBoard
{
    public class Piece
    {
        #region Fields

        #endregion


        #region Properties
        public Point Position { get; set; }
        public Color Color { get; set; }
        public Board Board { get; set; }
        public Block[] Blocks { get; set; }
        public PieceModel PieceModel { get; private set; }
        public int RotationIndex { get; set; }
        public Size BlockSize { get; set; }
        #endregion


        #region Constructors
        public Piece(Board board, Color color, PieceModel pieceModel, int rotationIndex, Size blockSize)
        {
            Board = board;
            Color = color;
            PieceModel = pieceModel;
            RotationIndex = 0;
            if (rotationIndex >= 0 && rotationIndex < pieceModel.Length)
            {
                RotationIndex = rotationIndex;
            }
            BlockSize = blockSize;
            Position = new Point(5, 0);
            CreateBlocks(pieceModel[RotationIndex]);
        }

        private void CreateBlocks(IList<Point> positions)
        {
            Blocks = new Block[positions.Count];
            for (int i = 0; i < Blocks.Length; i++)
            {
                Blocks[i] = new Block(this, positions[i], BlockSize, Color);
            }
        }
        #endregion


        #region Public Methods
        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            foreach (var block in Blocks)
            {
                block.Draw(spriteBatch, gameTime);
            }
        }
        #endregion


        #region Player Commands
        public bool DropByOne()
        {
            return Move(0, 1, 0);
        }

        public void DropAllTheWay()
        {
            while (DropByOne())
            {
                // TODO: KG - Move Increment Value to Configuration
                Board.Game.Score.IncrementBy(1);
            }
        }

        public void MoveLeft()
        {
            Move(-1, 0, 0);
        }

        public void MoveRight()
        {
            Move(1, 0, 0);
        }

        internal void RotateLeft()
        {
            Move(0, 0, 3);
        }

        internal void RotateRight()
        {
            Move(0, 0, 1);
        }
        #endregion


        #region Internal Implementation
        private bool Move(int deltaX, int deltaY, int deltaRotation)
        {
            bool moved = true;
            var positions = PieceModel[(RotationIndex + deltaRotation) % PieceModel.Length];
            foreach (var pos in positions)
            {
                if (!Board.IsEmptyAt(new Point(pos.X + deltaX + Position.X, pos.Y + deltaY + Position.Y)))
                {
                    moved = false;
                }
            }
            if (moved)
            {
                Position = new Point(Position.X + deltaX, Position.Y + deltaY);
                RotationIndex = (RotationIndex + deltaRotation) % PieceModel.Length;
                for (int i = 0; i < Blocks.Length; i++)
                {
                    Blocks[i].X = positions[i].X;
                    Blocks[i].Y = positions[i].Y;
                }
            }
            return moved;
        }
        #endregion
    }
}