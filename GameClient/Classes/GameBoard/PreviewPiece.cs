﻿using System;
using System.Linq;
using GameClient.Classes.Core;
using Microsoft.Xna.Framework;

namespace GameClient.Classes.GameBoard
{
    public class PreviewPiece : PieceBase, IDisposable
    {
        #region Constructor
        public PreviewPiece(TetrisGame game, Color color, PieceModel model, int rotationIndex)
            : base(color, model, rotationIndex, Application.Instance.Configuration.Board.PreviewBlockSize)
        {
            Game = game;
        }

        public PreviewPiece(TetrisGame game, Color color, PieceModel model, int rotationIndex, Point position)
            : base(color, model, rotationIndex, Application.Instance.Configuration.Board.PreviewBlockSize)
        {
            Game = game;
            Position = position;
        }
        #endregion


        #region Overrides of PieceBase
        public override void Update(GameTime gameTime)
        {
            SetPiecePosition();
            UpdateBlocksPositions(Game.PreviewPanel.Bounds.Location);
            base.Update(gameTime);
        }

        public override void UpdateBlocksPositions(Point offset)
        {
            var positions = Model[RotationIndex];
            for (int i = 0; i < Blocks.Length; i++)
            {
                Blocks[i].Bounds = new Rectangle(positions[i].X * BlockSize.Width + Position.X + offset.X,
                                                 positions[i].Y * BlockSize.Height + Position.Y + offset.Y,
                                                 BlockSize.Width, BlockSize.Height);
            }
        }
        #endregion


        #region Implementation of IDisposable
        public void Dispose()
        {
            foreach (var block in Blocks)
            {
                if (block != null)
                {
                    block.Dispose();
                }
            }
        }
        #endregion


        #region Internal Implementation
        private void SetPiecePosition()
        {
            int spanLeft = Math.Abs(Blocks.Min(block => block.X));
            int spanTop = Math.Abs(Blocks.Min(block => block.Y));
            int width = Blocks.GroupBy(block => block.X).Count() * BlockSize.Width;
            int height = Blocks.GroupBy(block => block.Y).Count() * BlockSize.Width;
            var newX = (Game.PreviewPanel.Bounds.Width - width) / 2 + spanLeft * BlockSize.Width;
            var newY = (Game.PreviewPanel.Bounds.Height - height) / 2 + spanTop * BlockSize.Height;
            Position = new Point(newX, newY);
        }
        #endregion
    }
}