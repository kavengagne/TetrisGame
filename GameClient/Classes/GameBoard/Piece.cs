using System.Collections.Generic;
using GameClient.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Color = Microsoft.Xna.Framework.Color;
using Point = Microsoft.Xna.Framework.Point;

namespace GameClient.Classes.GameBoard
{
    public class Piece : PieceBase
    {
        #region Properties
        public TetrisGame Game { get; set; }
        #endregion


        #region Constructors
        public Piece(TetrisGame game, Color color, PieceModel model, int rotationIndex, Rectangle blockSize)
            : base(color, model, rotationIndex, blockSize)
        {
            Game = game;
            Position = new Point(5, 0);
            /*
            Color = color;
            Model = model;
            RotationIndex = 0;
            if (rotationIndex >= 0 && rotationIndex < model.Length)
            {
                RotationIndex = rotationIndex;
            }
            BlockSize = blockSize;
            CreateBlocks(model[RotationIndex]);
            */
        }

        public Piece(TetrisGame game, PreviewPiece previewPiece) : base(previewPiece.Color, previewPiece.Model, previewPiece.RotationIndex, previewPiece.BlockSize)
        {
            Game = game;
            Position = new Point(5, 0);
        }
        #endregion


        #region Overrides of PieceBase
        public override void Update(GameTime gameTime)
        {
            UpdateBlocksPositions(Game.Board.Bounds.Location);
        }

        protected override void UpdateBlocksPositions(Point offset)
        {
            var positions = Model[RotationIndex];
            for (int i = 0; i < Blocks.Length; i++)
            {
                Blocks[i].X = positions[i].X * Blocks[i].Bounds.Width + Position.X * Blocks[i].Bounds.Width + offset.X;
                Blocks[i].Y = positions[i].Y * Blocks[i].Bounds.Height + Position.Y * Blocks[i].Bounds.Height + offset.Y;
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
            bool droppedOnce = false;
            while (DropByOne())
            {
                droppedOnce = true;
                // TODO: KG - Move Increment Value to Configuration
                Game.ScoreBoard.IncrementScoreBy(1);
            }
            if (droppedOnce)
            {
                Game.Board.UpdateBoard();
            }
        }

        public void MoveLeft()
        {
            if (Move(-1, 0, 0))
            {
                Game.SoundManager.Play("Move",(float)0.25);
            }
        }

        public void MoveRight()
        {
            if (Move(1, 0, 0))
            {
                Game.SoundManager.Play("Move", (float)0.25);
            }
        }

        public void RotateLeft()
        {
            if (Move(0, 0, 3))
            {
                Game.SoundManager.Play("Rotate", (float)0.25);
            }
        }

        public void RotateRight()
        {
            if (Move(0, 0, 1))
            {
                Game.SoundManager.Play("Rotate", (float)0.25);
            }
        }
        #endregion


        #region Internal Implementation
        private bool Move(int deltaX, int deltaY, int deltaRotation)
        {
            bool moved = true;
            var positions = Model[(RotationIndex + deltaRotation) % Model.Length];
            foreach (var pos in positions)
            {
                var expectedPosition = new Point(pos.X + deltaX + Position.X, pos.Y + deltaY + Position.Y);
                if (!Game.Board.IsEmptyAt(expectedPosition))
                {
                    moved = false;
                }
            }
            if (moved)
            {
                Position = new Point(Position.X + deltaX, Position.Y + deltaY);
                RotationIndex = (RotationIndex + deltaRotation) % Model.Length;
                UpdateBlocksPositions(Game.Board.Bounds.Location);
            }
            return moved;
        }
        #endregion
    }
}