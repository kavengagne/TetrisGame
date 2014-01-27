using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Reflection.Emit;
using GameClient.Classes.Core;
using Microsoft.Xna.Framework;
using Point = Microsoft.Xna.Framework.Point;

namespace GameClient.Classes.GameBoard
{
    public class Piece : PieceBase
    {
        #region Constructors
        public Piece(TetrisGame game, PreviewPiece previewPiece)
            : base(previewPiece.Color, previewPiece.Model, previewPiece.RotationIndex, previewPiece.BlockSize)
        {
            previewPiece.Dispose();
            Game = game;
            Position = new Point(5, 0);
        }
        #endregion


        #region Overrides of PieceBase
        public override void Update(GameTime gameTime)
        {
            UpdateBlocksPositions(Game.Board.Bounds.Location);
        }

        public override void UpdateBlocksPositions(Point offset)
        {
            var positions = Model[RotationIndex];
            for (int i = 0; i < Blocks.Length; i++)
            {
                Blocks[i].Bounds = new Rectangle(positions[i].X * BlockSize.Width + Position.X * BlockSize.Width + offset.X,
                                                 positions[i].Y * BlockSize.Height + Position.Y * BlockSize.Height + offset.Y,
                                                 BlockSize.Width, BlockSize.Height);
            }
        }
        #endregion


        #region Player Commands
        public bool DropByOne()
        {
            return Move(0, 1);
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
            if (Move(-1, 0))
            {
                Game.SoundManager.Play("Move",(float)0.25);
            }
        }

        public void MoveRight()
        {
            if (Move(1, 0))
            {
                Game.SoundManager.Play("Move", (float)0.25);
            }
        }

        public void RotateLeft()
        {
            if (Rotate(deltaRotation: 3))
            {
                Game.SoundManager.Play("Rotate", (float)0.25);
            }
        }

        public void RotateRight()
        {
            if (Rotate(deltaRotation: 1))
            {
                Game.SoundManager.Play("Rotate", (float)0.25);
            }
        }
        #endregion


        #region Internal Implementation
        //private bool Move(int deltaX, int deltaY, int deltaRotation)
        //{
        //    bool moved = true;
        //    var positions = Model[(RotationIndex + deltaRotation) % Model.Length];
        //    foreach (var pos in positions)
        //    {
        //        var expectedPosition = new Point(pos.X + deltaX + Position.X, pos.Y + deltaY + Position.Y);
        //        if (!Game.Board.IsEmptyAt(expectedPosition))
        //        {
        //            moved = false;
        //        }
        //    }
        //    if (moved)
        //    {
        //        Position = new Point(Position.X + deltaX, Position.Y + deltaY);
        //        RotationIndex = (RotationIndex + deltaRotation) % Model.Length;
        //        UpdateBlocksPositions(Game.Board.Bounds.Location);
        //    }
        //    return moved;
        //}
        
        private bool Move(int deltaX, int deltaY)
        {
            bool moved = true;
            var positions = Model[RotationIndex];
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
                UpdateBlocksPositions(Game.Board.Bounds.Location);
            }
            return moved;
        }

        private bool Rotate(int deltaRotation)
        {
            var positions = Model[(RotationIndex + deltaRotation) % Model.Length];
            var enumerable = positions.Select(pos => new Point(pos.X + Position.X, pos.Y + Position.Y)).ToArray();
            var deltaLeft = GetDeltaLeft(enumerable);
            var deltaRight = GetDeltaRight(enumerable, Game.Board.Columns);
            var realPositions = enumerable.Select(pos => new Point(pos.X + deltaLeft - deltaRight, pos.Y));
            if (realPositions.Any(pos => !Game.Board.IsEmptyAt(pos)))
            {
                return false;
            }
            Position = new Point(Position.X + deltaLeft - deltaRight, Position.Y);
            RotationIndex = (RotationIndex + deltaRotation) % Model.Length;
            UpdateBlocksPositions(Game.Board.Bounds.Location);
            return true;
        }

        private static int GetDeltaLeft(IEnumerable<Point> enumerable)
        {
            int deltaLeft = enumerable.Min(pos => pos.X);
            return deltaLeft > 0 ? 0 : Math.Abs(deltaLeft);
        }

        private static int GetDeltaRight(IEnumerable<Point> enumerable, int columnsCount)
        {
            int deltaRight = enumerable.Max(pos => pos.X) - (columnsCount - 1);
            return deltaRight < 0 ? 0 : deltaRight;
        }
        #endregion
    }
}