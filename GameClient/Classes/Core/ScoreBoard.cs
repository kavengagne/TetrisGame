﻿using System.Diagnostics;
using GameClient.Classes.Extensions;
using GameClient.Classes.GameBoard;
using GameClient.Classes.Interfaces;
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
        private Vector2 _textPosition;
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
            _textPosition = new Vector2(bounds.X + 15, bounds.Y + 20);
            Score = new Score();
        }
        #endregion


        #region Public Methods
        public void IncrementScoreBy(int value)
        {
            Score.IncrementBy(value);
        }
        #endregion


        #region ISprite Implementation
        public void Update(GameTime gameTime)
        {
            var textSize = _font.MeasureString(Score.ToString());
            _textPosition = new Vector2(textSize.X / 2, textSize.Y / 2);
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Draw(_texture, _bounds, _backgroundColor);
            spriteBatch.DrawString(_font, Score.ToString(),
                                   new Vector2(_bounds.Center.X, _bounds.Center.Y),
                                   Color.Black, 0, _textPosition,
                                   0.5f, SpriteEffects.None, 0.0f);
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