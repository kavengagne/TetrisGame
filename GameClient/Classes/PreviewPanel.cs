using GameClient.Classes.Extensions;
using GameClient.Classes.GameBoard;
using GameClient.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameClient.Classes
{
    public class PreviewPanel : ISprite
    {
        #region Fields
        private readonly TetrisGame _game;
        private readonly Board _board;
        private readonly Texture2D _texture;
        private readonly Rectangle _bounds;
        private readonly Color _backgroundColor;
        #endregion


        #region Constructors
        public PreviewPanel(TetrisGame game, Board board, Rectangle bounds, Color backgroundColor)
        {
            _game = game;
            _board = board;
            _bounds = bounds;
            _backgroundColor = backgroundColor;
            _texture = CreateTexture(_game.GraphicsDevice, bounds, backgroundColor);
        }
        #endregion


        #region Public Methods
        public void Update(GameTime gameTime)
        {
            
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            _board.PieceGenerator.PeekNextPiece().Draw(spriteBatch, gameTime);
            spriteBatch.Draw(_texture, _bounds, _backgroundColor);
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
    }
}