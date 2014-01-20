using GameClient.Classes.Extensions;
using GameClient.Classes.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameClient.Classes.Core
{
    public class PreviewPanel : ISprite
    {
        #region Fields
        private readonly TetrisGame _game;
        private readonly Texture2D _texture;
        private readonly Color _backgroundColor;
        #endregion


        #region Properties
        public Rectangle Bounds { get; set; }
        #endregion


        #region Constructors
        public PreviewPanel(TetrisGame game, Rectangle bounds, Color backgroundColor)
        {
            _game = game;
            Bounds = bounds;
            _backgroundColor = backgroundColor;
            _texture = CreateTexture(_game.GraphicsDevice, bounds, backgroundColor);
        }
        #endregion


        #region Public Methods
        public void Update(GameTime gameTime)
        {
            _game.PeekNextPiece().Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Draw(_texture, Bounds, _backgroundColor);
            _game.PeekNextPiece().Draw(spriteBatch, gameTime);
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