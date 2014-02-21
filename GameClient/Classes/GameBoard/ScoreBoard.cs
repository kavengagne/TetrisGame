using System;
using System.Globalization;
using GameClient.Classes.Core;
using GameClient.Classes.Extensions;
using GameClient.Classes.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameClient.Classes.GameBoard
{
    public class ScoreBoard : ISprite
    {
        #region Fields
        private readonly Board _board;
        private readonly Texture2D _texture;
        private readonly Rectangle _bounds;
        private readonly Color _backgroundColor;
        private readonly SpriteFont _font;
        private readonly Score _score;
        #endregion


        #region Properties
        #endregion


        #region Constructors
        public ScoreBoard(Board board, Rectangle bounds, Color backgroundColor)
        {
            _board = board;
            _bounds = bounds;
            _backgroundColor = backgroundColor;
            _texture = CreateTexture(TetrisGame.GetInstance().GraphicsDevice, bounds, backgroundColor);
            _font = TetrisGame.GetInstance().Content.Load<SpriteFont>("Fonts/ScoreBoard");
            _score = new Score();
            _score.LinesUpdated += TetrisGame.GetInstance().ChangeGameBackgroundColor;
        }
        #endregion


        #region Public Methods
        public void IncrementPointsBy(int value)
        {
            _score.IncrementPointsBy(value);
        }

        public void IncrementLinesBy(int value)
        {
            _score.IncrementLinesBy(value);
        }
        #endregion


        #region ISprite Implementation
        public void Update(GameTime gameTime)
        {
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Draw(_texture, _bounds, _backgroundColor);

            const string pointsHeader = "Score";
            spriteBatch.DrawString(_font, pointsHeader, new Vector2(_bounds.Center.X, _bounds.Center.Y),
                                   Color.White, 0, GetTextPosition(pointsHeader, 1), 0.5f, SpriteEffects.None, 0.0f);
            var pointsText = _score.Points.ToString(CultureInfo.InvariantCulture);
            spriteBatch.DrawString(_font, pointsText, new Vector2(_bounds.Center.X, _bounds.Center.Y),
                                   Color.White, 0, GetTextPosition(pointsText, 2), 0.5f, SpriteEffects.None, 0.0f);

            const string linesHeader = "Lines";
            spriteBatch.DrawString(_font, linesHeader, new Vector2(_bounds.Center.X, _bounds.Center.Y),
                                   Color.White, 0, GetTextPosition(linesHeader, 3), 0.5f, SpriteEffects.None, 0.0f);
            var linesText = _score.Lines.ToString(CultureInfo.InvariantCulture);
            spriteBatch.DrawString(_font, linesText, new Vector2(_bounds.Center.X, _bounds.Center.Y),
                                   Color.White, 0, GetTextPosition(linesText, 4), 0.5f, SpriteEffects.None, 0.0f);
        }

        private Vector2 GetTextPosition(string text, int lineNumber, int offset = 0)
        {
            lineNumber = Math.Abs(lineNumber) - 1;
            var size = _font.MeasureString(text);
            var position = new Vector2(size.X / 2, _bounds.Height - (size.Y * lineNumber + offset));
            return position;
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
