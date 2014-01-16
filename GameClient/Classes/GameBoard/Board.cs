﻿using System;
using System.Linq;
using GameClient.Classes.Extensions;
using GameClient.Interfaces;
using GameConfiguration.DataObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Color = Microsoft.Xna.Framework.Color;
using Point = Microsoft.Xna.Framework.Point;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace GameClient.Classes.GameBoard
{
    public class Board : ISprite
    {
        #region Fields
        private Block[][] _grid;
        private double _delayCurrent;
        private readonly Application _application;
        #endregion


        #region Properties
        public TetrisGame Game { get; set; }
        public PieceGenerator PieceGenerator;
        public int Rows { get; set; }
        public int Columns { get; set; }
        public int UpdateDelay { get; set; }
        public Color BackgroundColor { get; set; }
        public Texture2D Texture { get; set; }
        public Rectangle Bounds { get; set; }
        public Piece CurrentPiece { get; set; }
        #endregion


        #region Constructors
        public Board(TetrisGame game, Point position)
        {
            _application = Application.Instance;
            Game = game;
            Game.Score = new Score();

            var boardInformation = _application.Configuration.Board;
            Rows = boardInformation.Rows;
            Columns = boardInformation.Columns;
            UpdateDelay = boardInformation.Speed;
            BackgroundColor = boardInformation.BackgroundColor;

            Bounds = new Rectangle(position.X, position.Y,
                                   Columns * boardInformation.BlockSize.Width,
                                   Rows * boardInformation.BlockSize.Height);
            
            Texture = CreateTexture(Game.GraphicsDevice, Bounds, BackgroundColor);

            InitializeGameGrid();
            InitializePieceGenerator(_application.Configuration.Pieces,
                                     _application.Configuration.PiecesColors,
                                     _application.Configuration.Board.BlockSize);

            CurrentPiece = GetNextPiece();
        }
        #endregion


        #region Public Methods
        public bool IsEmptyAt(Point position)
        {
            return !(position.X < 0 ||
                     position.Y < 0 ||
                     position.X >= _grid.Length ||
                     position.Y >= _grid[0].Length ||
                     _grid[position.X][position.Y] != null);
        }

        public Piece GetNextPiece()
        {
            return PieceGenerator.GetPiece();
        }
        #endregion


        #region Player Commands
        public void DropPiece()
        {
            if (!IsGameOver() && _application.IsRunning)
            {
                CurrentPiece.DropAllTheWay();
            }
        }

        public void MoveLeft()
        {
            if (!IsGameOver() && _application.IsRunning)
            {
                CurrentPiece.MoveLeft();
            }
        }

        public void MoveRight()
        {
            if (!IsGameOver() && _application.IsRunning)
            {
                CurrentPiece.MoveRight();
            }
        }

        public void RotateLeft()
        {
            if (!IsGameOver() && _application.IsRunning)
            {
                CurrentPiece.RotateLeft();
            }
        }

        public void RotateRight()
        {
            if (!IsGameOver() && _application.IsRunning)
            {
                CurrentPiece.RotateRight();
            }
        }
        #endregion


        #region ISprite Implementation
        public void Update(GameTime gameTime)
        {
            if (_application.IsRunning && IsDelayExpired(gameTime))
            {
                UpdateBoard();
                CurrentPiece.Update(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Draw(Texture, Bounds, BackgroundColor);
            foreach (Block[] column in _grid)
            {
                foreach (Block block in column.Where(block => block != null))
                {
                    block.Draw(spriteBatch, gameTime);
                }
            }
            CurrentPiece.Draw(spriteBatch, gameTime);
        }
        #endregion


        #region Internal Implementation
        private void InitializeGameGrid()
        {
            _grid = new Block[Columns][];
            for (int i = 0; i < Columns; i++)
            {
                _grid[i] = new Block[Rows];
            }
        }

        private void InitializePieceGenerator(PieceInformation[] pieces, Color[] colors, Rectangle blockSize)
        {
            PieceGenerator = new PieceGenerator(this, pieces, colors, blockSize);
        }
        
        private void UpdateBoard()
        {
            bool ran = CurrentPiece.DropByOne();
            if (!ran)
            {
                StoreCurrentPiece();
                RemoveCompletedLines();
                if (!IsGameOver())
                {
                    CurrentPiece = GetNextPiece();
                }
            }
        }

        private bool IsDelayExpired(GameTime time)
        {
            _delayCurrent += time.ElapsedGameTime.TotalMilliseconds;
            if (Math.Ceiling(_delayCurrent) >= UpdateDelay)
            {
                _delayCurrent = time.ElapsedGameTime.TotalMilliseconds;
                return true;
            }
            return false;
        }

        private bool IsGameOver()
        {
            bool ret = _grid.Any(column => column[0] != null);
            return ret;
        }

        private void StoreCurrentPiece()
        {
            var model = CurrentPiece.Model[CurrentPiece.RotationIndex];
            var position = CurrentPiece.Position;
            for (int i = 0; i < model.Length; i++)
            {
                var current = model[i];
                _grid[Math.Max(current.X + position.X, 0)][Math.Max(current.Y + position.Y, 0)] = CurrentPiece.Blocks[i];
            }
        }

        private void RemoveCompletedLines()
        {
            int removedLines = 0;
            for (int rowIndex = 1; rowIndex < _grid[0].Length; rowIndex++)
            {
                if (_grid.All(column => column[rowIndex] != null))
                {
                    DeleteLine(rowIndex);
                    DropLinesByOne(rowIndex);
                    // TODO: KG - Move Increment Value to Configuration
                    Game.Score.IncrementBy(10);
                    removedLines++;
                }
            }
            // TODO: KG - Bonus for Tetris (4 Blocks) - Refactor
            int bonusCount = removedLines / 4;
            Game.Score.IncrementBy(bonusCount * 10);
        }

        private void DropLinesByOne(int rowIndex)
        {
            for (int row = rowIndex; row > 0; row--)
            {
                foreach (Block[] column in _grid)
                {
                    column[row] = column[row - 1];
                    if (column[row] != null)
                    {
                        column[row].Y++;
                    }
                    column[row - 1] = null;
                }
            }
        }

        private void DeleteLine(int rowIndex)
        {
            foreach (Block[] column in _grid)
            {
                if (column[rowIndex] != null)
                {
                    column[rowIndex].Dispose();
                    column[rowIndex] = null;
                }
            }
        }

        private Texture2D CreateTexture(GraphicsDevice graphicsDevice, Rectangle bounds, Color color)
        {
            var texture = new Texture2D(graphicsDevice, bounds.Width, bounds.Height);
            texture.FillWithColor(color);
            texture.AddBorder(Color.Black, 1);
            return texture;
        }
        #endregion
    }
}
