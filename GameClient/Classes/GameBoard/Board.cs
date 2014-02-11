using System;
using System.Linq;
using GameClient.Classes.Core;
using GameClient.Classes.Extensions;
using GameClient.Classes.GameBoard.Pieces;
using GameClient.Classes.Interfaces;
using GameConfiguration.DataObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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
        public int Rows { get; set; }
        public int Columns { get; set; }
        public int UpdateDelay { get; set; }
        public Color BackgroundColor { get; set; }
        public Texture2D Texture { get; set; }
        public Rectangle Bounds { get; set; }
        public PieceGenerator PieceGenerator { get; set; }
        public PreviewPanel PreviewPanel { get; set; }
        public ScoreBoard ScoreBoard { get; set; }
        public Piece CurrentPiece { get; set; }
        public bool CanExchangePiece { get; set; }
        #endregion


        #region Constructors
        public Board(TetrisGame game, Point position)
        {
            _application = Application.Instance;
            Game = game;

            var boardInformation = _application.Configuration.Board;
            Rows = boardInformation.Rows;
            Columns = boardInformation.Columns;
            UpdateDelay = boardInformation.Speed;
            BackgroundColor = boardInformation.BackgroundColor;

            Bounds = new Rectangle(position.X, position.Y,
                                   Columns * boardInformation.BlockSize.Width,
                                   Rows * boardInformation.BlockSize.Height);
            
            Texture = CreateTexture(Game.GraphicsDevice, Bounds, BackgroundColor);

            Reset();
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

        public void StoreCurrentPiece()
        {
            var model = CurrentPiece.Model[CurrentPiece.RotationIndex];
            var position = CurrentPiece.Position;
            for (int i = 0; i < model.Length; i++)
            {
                var current = model[i];
                _grid[Math.Max(current.X + position.X, 0)][Math.Max(current.Y + position.Y, 0)] = CurrentPiece.Blocks[i];
            }
        }

        public void UpdateBoard()
        {
            bool ran = CurrentPiece.DropByOne();
            if (!ran)
            {
                StoreCurrentPiece();
                RemoveCompletedLines();
                if (!IsGameOver())
                {
                    Game.SoundManager.Play("Drop", (float)0.5);
                    CurrentPiece = GetNextPiece();
                    CurrentPiece.UpdateBlocksPositions(Bounds.Location);
                }
            }
        }
        #endregion


        #region Player Commands
        public void Reset()
        {
            InitializeGameGrid();
            InitializePreviewPanel();
            InitializePieceGenerator(_application.Configuration.Pieces,
                                     _application.Configuration.PiecesColors);
            InitializeScoreBoard();
            CurrentPiece = GetNextPiece();
            CurrentPiece.UpdateBlocksPositions(Bounds.Location);
        }

        public void ExchangePiece()
        {
            if (_application.Game.IsRunning && CanExchangePiece)
            {
                // Save Board.CurrentPiece in TemporaryPiece.
                var tempPiece = new PreviewPiece(this, CurrentPiece.Color, CurrentPiece.Model,
                                                 CurrentPiece.RotationIndex, CurrentPiece.Position);
                // Save NextPiece to CurrentPiece.
                CurrentPiece = new Piece(this, PieceGenerator.PeekNextPiece(), CurrentPiece.Position);
                // Save TemporaryPiece to NextPiece.
                PieceGenerator.SetNextPiece(tempPiece);
                // Set Flag to Prevent Another Exchange.
                CanExchangePiece = false;
            }
        }

        public void DropPieceAllTheWay()
        {
            if (!IsGameOver() && _application.Game.IsRunning)
            {
                CurrentPiece.DropAllTheWay();
            }
        }

        public void DropPieceByOne()
        {
            if (!IsGameOver() && _application.Game.IsRunning)
            {
                if (CurrentPiece.DropByOne())
                {
                    ScoreBoard.IncrementPointsBy(1);
                }
            }
        }

        public void MoveLeft()
        {
            if (!IsGameOver() && _application.Game.IsRunning)
            {
                CurrentPiece.MoveLeft();
            }
        }

        public void MoveRight()
        {
            if (!IsGameOver() && _application.Game.IsRunning)
            {
                CurrentPiece.MoveRight();
            }
        }

        public void RotateLeft()
        {
            if (!IsGameOver() && _application.Game.IsRunning)
            {
                CurrentPiece.RotateLeft();
            }
        }

        public void RotateRight()
        {
            if (!IsGameOver() && _application.Game.IsRunning)
            {
                CurrentPiece.RotateRight();
            }
        }
        #endregion


        #region ISprite Implementation
        public void Update(GameTime gameTime)
        {
            if (_application.Game.IsRunning && IsDelayExpired(gameTime))
            {
                CurrentPiece.Update(gameTime);
                UpdateBoard();
                PreviewPanel.Update(gameTime);
                ScoreBoard.Update(gameTime);
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
            PreviewPanel.Draw(spriteBatch, gameTime);
            ScoreBoard.Draw(spriteBatch, gameTime);
        }
        #endregion


        #region Internal Implementation
        private void InitializePieceGenerator(PieceInformation[] pieces, Color[] colors)
        {
            PieceGenerator = new PieceGenerator(this, pieces, colors);
        }

        private void InitializePreviewPanel()
        {
            // TODO: KG - Move to config or something
            var bounds = new Rectangle(Bounds.X + Bounds.Width + 5, 40, 100, 100);
            PreviewPanel = new PreviewPanel(this, bounds, _application.Configuration.Board.BackgroundColor);
        }

        private void InitializeScoreBoard()
        {
            // TODO: KG - Move to config or something
            var bounds = new Rectangle(Bounds.X + Bounds.Width + 5, 40 + 100 + 5, 100, 110);
            ScoreBoard = new ScoreBoard(this, bounds, _application.Configuration.Board.BackgroundColor);
        }

        private void InitializeGameGrid()
        {
            _grid = new Block[Columns][];
            for (int i = 0; i < Columns; i++)
            {
                _grid[i] = new Block[Rows];
            }
        }

        private Piece GetNextPiece()
        {
            return PieceGenerator.GetPiece();
        }

        public PreviewPiece PeekNextPiece()
        {
            return PieceGenerator.PeekNextPiece();
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

        private void RemoveCompletedLines()
        {
            int removedLinesCount = 0;
            int gridRowsCount = _grid[0].Length;
            for (int rowIndex = 1; rowIndex < gridRowsCount; rowIndex++)
            {
                if (_grid.All(column => column[rowIndex] != null))
                {
                    DeleteLine(rowIndex);
                    DropLinesByOne(rowIndex);
                    // TODO: KG - Move Increment Value to Configuration
                    ScoreBoard.IncrementPointsBy(10);
                    ScoreBoard.IncrementLinesBy(1);
                    removedLinesCount++;
                }
            }
            if (removedLinesCount > 0)
            {
                //var pitch = (float)0.33 * (Math.Max(removedLinesCount - 1, 0));
                Game.SoundManager.Play("Remove");
                
                // TODO: KG - Move bonus value to config
                if (removedLinesCount >= 4)
                {
                    ScoreBoard.IncrementPointsBy(10);
                }
            }
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
                        column[row].Bounds = new Rectangle(column[row].Bounds.X,
                                                           column[row].Bounds.Y + _application.Configuration.Board.BlockSize.Height,
                                                           column[row].Bounds.Width,
                                                           column[row].Bounds.Height);
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
