using System.Collections.Generic;
using GameClient.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Color = Microsoft.Xna.Framework.Color;
using Point = Microsoft.Xna.Framework.Point;

namespace GameClient.Classes.GameBoard
{
    public class Piece : ISprite
    {
        #region Properties
        public Point Position { get; set; }
        public Color Color { get; set; }
        public Board Board { get; set; }
        public Block[] Blocks { get; set; }
        public PieceModel Model { get; private set; }
        public int RotationIndex { get; set; }
        public Rectangle BlockSize { get; set; }
        #endregion


        #region Constructors
        public Piece(Board board, Color color, PieceModel model, int rotationIndex, Rectangle blockSize)
        {
            Board = board;
            Color = color;
            Model = model;
            RotationIndex = 0;
            if (rotationIndex >= 0 && rotationIndex < model.Length)
            {
                RotationIndex = rotationIndex;
            }
            BlockSize = blockSize;
            Position = new Point(5, 0);
            CreateBlocks(model[RotationIndex]);
        }
        #endregion


        #region ISprite Implementation
        public void Update(GameTime gameTime)
        {
        }

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
            if (Move(-1, 0, 0))
            {

            }
        }

        public void MoveRight()
        {
            if (Move(1, 0, 0))
            {

            }
        }

        public void RotateLeft()
        {
            if (Move(0, 0, 3))
            {
                
            }
        }

        public void RotateRight()
        {
            if (Move(0, 0, 1))
            {
                
            }
        }
        #endregion


        #region Internal Implementation
        private void CreateBlocks(IList<Point> positions)
        {
            Blocks = new Block[positions.Count];
            for (int i = 0; i < Blocks.Length; i++)
            {
                Blocks[i] = new Block(this, positions[i], BlockSize, Color);
            }
        }

        private bool Move(int deltaX, int deltaY, int deltaRotation)
        {
            bool moved = true;
            var positions = Model[(RotationIndex + deltaRotation) % Model.Length];
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
                RotationIndex = (RotationIndex + deltaRotation) % Model.Length;
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

    public class PreviewPiece : ISprite
    {
        #region Constructor
        public PreviewPiece()
        {

        }
        #endregion


        #region Implementation of ISprite
        public void Update(GameTime gameTime)
        {
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            
        }
        #endregion
    }
}