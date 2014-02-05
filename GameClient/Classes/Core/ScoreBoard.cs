using System;
using System.Globalization;
using GameClient.Classes.Core.Randomizer;
using GameClient.Classes.Extensions;
using GameClient.Classes.Interfaces;
using GameClient.Classes.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameClient.Classes.Core
{
    public class ScoreBoard : ISprite
    {
        #region Fields
        private readonly TetrisGame _game;
        private readonly Texture2D _texture;
        private readonly Rectangle _bounds;
        private readonly Color _backgroundColor;
        private readonly SpriteFont _font;
        #endregion


        #region Properties
        public Score Score { get; set; }
        #endregion


        #region Constructors
        public ScoreBoard(TetrisGame game, Rectangle bounds, Color backgroundColor)
        {
            _game = game;
            _bounds = bounds;
            _backgroundColor = backgroundColor;
            _texture = CreateTexture(_game.GraphicsDevice, bounds, backgroundColor);
            _font = _game.Content.Load<SpriteFont>("Fonts/ScoreBoard");
            Score = new Score();
            Score.LinesUpdated += ChangeGameBackgroundColor;
        }
        #endregion


        #region Public Methods
        public void IncrementPointsBy(int value)
        {
            Score.IncrementPointsBy(value);
        }

        public void IncrementLinesBy(int value)
        {
            Score.IncrementLinesBy(value);
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
                                   Color.Black, 0, GetTextPosition(pointsHeader, 1), 0.5f, SpriteEffects.None, 0.0f);
            var pointsText = Score.Points.ToString(CultureInfo.InvariantCulture);
            spriteBatch.DrawString(_font, pointsText, new Vector2(_bounds.Center.X, _bounds.Center.Y),
                                   Color.Black, 0, GetTextPosition(pointsText, 2), 0.5f, SpriteEffects.None, 0.0f);

            const string linesHeader = "Lines";
            spriteBatch.DrawString(_font, linesHeader, new Vector2(_bounds.Center.X, _bounds.Center.Y),
                                   Color.Black, 0, GetTextPosition(linesHeader, 3), 0.5f, SpriteEffects.None, 0.0f);
            var linesText = Score.Lines.ToString(CultureInfo.InvariantCulture);
            spriteBatch.DrawString(_font, linesText, new Vector2(_bounds.Center.X, _bounds.Center.Y),
                                   Color.Black, 0, GetTextPosition(linesText, 4), 0.5f, SpriteEffects.None, 0.0f);
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
        private void ChangeGameBackgroundColor(Score score, int updateValue)
        {
            // TODO: KG - Move to configuration.
            const int divider = 10;
            int oldValue = score.Lines - updateValue;
            int oldTenth = oldValue / divider;
            int newTenth = score.Lines / divider;
            if (newTenth > oldTenth)
            {
                Application.Instance.Configuration.Game.BackgroundColor = new Color(StaticRandom.Next(256),
                                                                                    StaticRandom.Next(256),
                                                                                    StaticRandom.Next(256));
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