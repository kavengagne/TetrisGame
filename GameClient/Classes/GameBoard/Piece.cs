using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using GameClient.Classes.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameClient.Classes.GameBoard
{
    public class Piece : PieceBase
    {
        #region Properties
        public Block[] GhostBlocks { get; set; }
        #endregion


        #region Constructors
        public Piece(TetrisGame game, PreviewPiece previewPiece)
            : base(previewPiece.Color, previewPiece.Model, previewPiece.RotationIndex, previewPiece.BlockSize)
        {
            previewPiece.Dispose();
            Game = game;
            Position = new Point(5, 0);
            CreateGhostBlocks(Model[RotationIndex]);
        }

        public Piece(TetrisGame game, PreviewPiece previewPiece, Point position)
            : base(previewPiece.Color, previewPiece.Model, previewPiece.RotationIndex, previewPiece.BlockSize)
        {
            previewPiece.Dispose();
            Game = game;
            Position = position;
            CreateGhostBlocks(Model[RotationIndex]);
            Rotate(0);
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
            var ghostPositions = positions.Select(pos => new Point(pos.X, pos.Y + GetGhostDeltaY(positions))).ToList();
            UpdatePositions(Blocks, offset, positions);
            UpdatePositions(GhostBlocks, offset, ghostPositions);
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            foreach (var ghostBlock in GhostBlocks)
            {
                ghostBlock.Draw(spriteBatch, gameTime);
            }
            base.Draw(spriteBatch, gameTime);
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
                Game.ScoreBoard.IncrementPointsBy(1);
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

        private void CreateGhostBlocks(IList<Point> positions)
        {
            GhostBlocks = new Block[positions.Count];
            for (int i = 0; i < GhostBlocks.Length; i++)
            {
                GhostBlocks[i] = new Block(this, positions[i], BlockSize, Application.Instance.Configuration.Board.BackgroundColor);
            }
        }

        private void UpdatePositions(IList<Block> blocks, Point offset, IList<Point> positions)
        {
            for (int i = 0; i < blocks.Count; i++)
            {
                blocks[i].Bounds = new Rectangle(positions[i].X * BlockSize.Width + Position.X * BlockSize.Width + offset.X,
                                                 positions[i].Y * BlockSize.Height + Position.Y * BlockSize.Height + offset.Y,
                                                 BlockSize.Width, BlockSize.Height);
            }
        }

        private int GetGhostDeltaY(IList<Point> positions)
        {
            bool canMove = true;
            int deltaY = 0;
            while (canMove)
            {
                deltaY++;
                foreach (var pos in positions)
                {
                    var expectedPosition = new Point(pos.X + Position.X, pos.Y + deltaY + Position.Y);
                    if (!Game.Board.IsEmptyAt(expectedPosition))
                    {
                        canMove = false;
                    }
                }
            }
            return Math.Max(--deltaY, 0);
        }
        #endregion
    }
}