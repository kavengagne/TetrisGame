using GameClient.Classes.Core;
using GameClient.Classes.Core.Settings;
using GameClient.Classes.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameClient.Classes.GameBoard
{
    public class Block : ISprite
    {
        #region Fields
        private readonly Texture2D _texture;
        #endregion


        #region Properties
        public int X { get; set; }
        public int Y { get; set; }
        public Rectangle Bounds { get; set; }
        public Color BackgroundColor { get; set; }
        #endregion


        #region Constructors
        public Block(Point position, Rectangle bounds, Color backgroundColor)
        {
            X = position.X;
            Y = position.Y;
            Bounds = bounds;
            BackgroundColor = backgroundColor;
            _texture = TetrisGame.GetInstance().Content.Load<Texture2D>(Constants.Textures.Block);
        }
        #endregion


        #region ISprite Implementation
        public void Update(GameTime gameTime)
        {
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Draw(_texture, Bounds, null, BackgroundColor);
        }
        #endregion
    }
}
