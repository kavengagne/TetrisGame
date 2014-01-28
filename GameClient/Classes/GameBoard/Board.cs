using System;
using System.Linq;
using GameClient.Classes.Core;
using GameClient.Classes.Extensions;
using GameClient.Classes.Interfaces;
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
        public Piece CurrentPiece { get; set; }
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

            InitializeGameGrid();

            CurrentPiece = game.GetNextPiece();
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
                    CurrentPiece = Game.GetNextPiece();
                    CurrentPiece.UpdateBlocksPositions(Bounds.Location);
                }
            }
        }
        #endregion


        #region Player Commands
        public void DropPieceAllTheWay()
        {
            if (!IsGameOver() && _application.IsRunning)
            {
                CurrentPiece.DropAllTheWay();
            }
        }

        public void DropPieceByOne()
        {
            if (!IsGameOver() && _application.IsRunning)
            {
                if (CurrentPiece.DropByOne())
                {
                    Game.ScoreBoard.IncrementPointsBy(1);
                }
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
            //if (!IsGameOver() && _application.IsRunning)
            {
                CurrentPiece.RotateLeft();
            }
        }

        public void RotateRight()
        {
            //if (!IsGameOver() && _application.IsRunning)
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
                CurrentPiece.Update(gameTime);
                UpdateBoard();
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
            for (int rowIndex = 1; rowIndex < _grid[0].Length; rowIndex++)
            {
                if (_grid.All(column => column[rowIndex] != null))
                {
                    DeleteLine(rowIndex);
                    DropLinesByOne(rowIndex);
                    // TODO: KG - Move Increment Value to Configuration
                    Game.ScoreBoard.IncrementPointsBy(10);
                    Game.ScoreBoard.IncrementLinesBy(1);
                    removedLinesCount++;
                }
            }
            if (removedLinesCount > 0)
            {
                //var pitch = (float)0.33 * (Math.Max(removedLinesCount - 1, 0));
                Game.SoundManager.Play("Remove", (float)1.0);
                
                // TODO: KG - Move bonus value to config
                if (removedLinesCount >= 4)
                {
                    Game.ScoreBoard.IncrementPointsBy(10);
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
