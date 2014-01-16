using System;
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
        public Rectangle Bounds { get; set; }
        public Piece Piece { get; set; }
        public Color Color { get; set; }
        public Texture2D Texture { get; set; }
        #endregion


        #region Constructors
        public Block(Piece piece, Point position, Rectangle bounds, Color color)
        {
            Piece = piece;
            X = position.X;
            Y = position.Y;
            Bounds = bounds;
            Color = color;
            Texture = CreateTexture(piece.Board.Game.GraphicsDevice, bounds, color);
        }
        #endregion


        #region Internal Implementation
        private Texture2D CreateTexture(GraphicsDevice graphicsDevice, Rectangle bounds, Color color)
        {
            var texture = new Texture2D(graphicsDevice, bounds.Width, bounds.Height);
            texture.FillWithColor(color);
            texture.AddBorder(Color.Black, 1);
            return texture;
        }
        #endregion


        #region ISprite Implementation
        public void Update(GameTime gameTime)
        {
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            var newX = X * Bounds.Width + Piece.Position.X * Bounds.Width + Piece.Board.Bounds.X;
            var newY = Y * Bounds.Height + Piece.Position.Y * Bounds.Height + Piece.Board.Bounds.Y;
            spriteBatch.Draw(Texture, new Rectangle(newX, newY, Bounds.Width, Bounds.Height), Color);
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
