using System;
using GameClient.Classes.Extensions;
using GameClient.Classes.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameClient.Classes.GameBoard
{
    public class Block : ISprite, IDisposable
    {
        #region Properties
        public int X { get; set; }
        public int Y { get; set; }
        public Rectangle Bounds { get; set; }
        public PieceBase Piece { get; set; }
        public Color BackgroundColor { get; set; }
        public Color BorderColor { get; set; }
        public Texture2D Texture { get; set; }
        #endregion


        #region Constructors
        public Block(PieceBase piece, Point position, Rectangle bounds, Color backgroundColor)
            : this(piece, position, bounds, backgroundColor, Color.Black)
        {
        }

        public Block(PieceBase piece, Point position, Rectangle bounds, Color backgroundColor, Color borderColor)
        {
            Piece = piece;
            X = position.X;
            Y = position.Y;
            Bounds = bounds;
            BackgroundColor = backgroundColor;
            BorderColor = borderColor;
            Texture = CreateTexture(Application.Instance.Game.GraphicsDevice, bounds, backgroundColor, borderColor);
        }
        #endregion


        #region Internal Implementation
        private Texture2D CreateTexture(GraphicsDevice graphicsDevice, Rectangle bounds, Color backgroundColor, Color borderColor)
        {
            var texture = new Texture2D(graphicsDevice, bounds.Width, bounds.Height);
            texture.FillWithColor(backgroundColor);
            texture.AddBorder(borderColor, 1);
            return texture;
        }
        #endregion


        #region ISprite Implementation
        public void Update(GameTime gameTime)
        {
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            //var newX = X * Bounds.Width + Piece.Position.X * Bounds.Width + Piece.Game.Board.Bounds.X;
            //var newY = Y * Bounds.Height + Piece.Position.Y * Bounds.Height + Piece.Game.Board.Bounds.Y;
            spriteBatch.Draw(Texture, Bounds, BackgroundColor);
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
