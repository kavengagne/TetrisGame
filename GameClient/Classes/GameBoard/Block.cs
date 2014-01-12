using System;
using System.Drawing;
using GameClient.Classes.Extensions;
using GameClient.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Color = Microsoft.Xna.Framework.Color;
using Point = Microsoft.Xna.Framework.Point;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace GameClient.Classes.GameBoard
{
    public class Block : ISprite, IDisposable
    {
        #region Properties
        public int X { get; set; }
        public int Y { get; set; }
        public Size Size { get; set; }
        public Piece Piece { get; set; }
        public Color Color { get; set; }
        public Texture2D Texture { get; set; }
        #endregion


        #region Constructors
        public Block(Piece piece, Point position, Size size, Color color)
        {
            Piece = piece;
            X = position.X;
            Y = position.Y;
            Size = size;
            Color = color;
            CreateBlockTexture();
        }
        #endregion


        #region Internal Implementation
        private void CreateBlockTexture()
        {
            Texture = new Texture2D(Piece.Board.Game.GraphicsDevice, Size.Width, Size.Height);
            Texture.FillWithColor(Color);
            Texture.AddBorder(Color.Black, 1);
        }
        #endregion


        #region Public Methods
        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            var newX = X * Size.Width + Piece.Position.X * Size.Width + Piece.Board.Bounds.X;
            var newY = Y * Size.Height + Piece.Position.Y * Size.Height + Piece.Board.Bounds.Y;
            spriteBatch.Draw(Texture, new Rectangle(newX, newY, Size.Width, Size.Height), Color);
        }
        #endregion


        #region IDisposable Implementation
        public void Dispose()
        {
            if (Texture != null)
            {
                Texture.Dispose();
            }
        } 
        #endregion
    }
}
