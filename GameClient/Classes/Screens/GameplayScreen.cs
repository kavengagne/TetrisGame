//-----------------------------------------------------------------------------
// GameplayScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------

using System;
using System.Threading;
using GameClient.Classes.Core;
using GameClient.Classes.Core.Inputs;
using GameClient.Classes.Core.Managers;
using GameClient.Classes.GameBoard;
using GameClient.Classes.ParticleSystem;
using GameClient.Classes.StateManager;
using GameClient.Classes.StateManager.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameClient.Classes.Screens
{
    internal class GameplayScreen : GameScreen
    {
        #region Fields
        private ContentManager _content;
        private float _pauseAlpha;
        #endregion


        #region Properties
        public SpriteBatch SpriteBatch { get; set; }
        public ParticleEngine ParticleEngine { get; set; }
        public Board Board { get; set; }
        public InputManager InputManager { get; set; }
        public Matrix SpriteScale { get; set; }
        #endregion


        #region Initialization
        public GameplayScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
        }

        public override void LoadContent()
        {
            if (_content == null)
            {
                _content = new ContentManager(ScreenManager.Game.Services, "Content");
            }

            Board = new Board();
            InputManager = new InputManager();
            RegisterUserInputs();

            // TODO: KG - Remove this when game completed.
            // Simulate loading time to show Loading Screen.
            Thread.Sleep(3000);

            // Tell the Game that we have just finished a very
            // long frame, and that it should not try to catch up.
            ScreenManager.Game.ResetElapsedTime();
        }
        
        public override void UnloadContent()
        {
            _content.Unload();
        }
        #endregion


        #region Update and Draw
        /// <summary>
        /// Updates the state of the game. This method checks the GameScreen.IsActive
        /// property, so the game will stop updating when the pause menu is active,
        /// or if you tab away to a different application.
        /// </summary>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);

            // Gradually fade in or out depending on whether we are covered by the pause screen.
            // ReSharper disable once ConvertIfStatementToConditionalTernaryExpression
            if (coveredByOtherScreen)
            {
                _pauseAlpha = Math.Min(_pauseAlpha + 1f / 32, 1);
            }
            else
            {
                _pauseAlpha = Math.Max(_pauseAlpha - 1f / 32, 0);
            }

            if (IsActive)
            {
                Board.Update(gameTime);
                InputManager.HandleInputs(gameTime);
            }
        }
        
        /// <summary>
        /// Lets the game respond to player input. Unlike the Update method,
        /// this will only be called when the gameplay screen is active.
        /// </summary>
        public override void HandleInput(InputState input)
        {
            if (input == null)
            {
                return;
            }
            if (ControllingPlayer == null)
            {
                return;
            }

            var playerIndex = (int)ControllingPlayer.Value;
            KeyboardState keyboardState = input.CurrentKeyboardStates[playerIndex];
            GamePadState gamePadState = input.CurrentGamePadStates[playerIndex];

            // The game pauses either if the user presses the pause button, or if they unplug the active gamepad.
            bool gamePadDisconnected = !gamePadState.IsConnected && input.GamePadWasConnected[playerIndex];
            if (input.IsPauseGame(ControllingPlayer) || gamePadDisconnected)
            {
                ScreenManager.AddScreen(new PauseMenuScreen(), ControllingPlayer);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            ScreenManager.GraphicsDevice.Clear(ClearOptions.Target, Color.CornflowerBlue, 0, 0);
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;

            spriteBatch.Begin();
            Board.Draw(spriteBatch, gameTime);
            spriteBatch.End();

            // If the game is transitioning on or off, fade it out to black.
            if (TransitionPosition > 0 || _pauseAlpha > 0)
            {
                float alpha = MathHelper.Lerp(1f - TransitionAlpha, 1f, _pauseAlpha / 2);
                ScreenManager.FadeBackBufferToBlack(alpha);
            }
        }
        #endregion


        #region Internal Implementation
        private void RegisterUserInputs()
        {
            InputManager.RegisterKeyPressed(Keys.LeftShift, Board.ExchangePiece);
            InputManager.RegisterKeyPressed(Keys.Space, Board.DropPieceAllTheWay);
            InputManager.RegisterKeyPressed(Keys.Up, Board.RotateLeft);
            InputManager.RegisterKeyPressed(Keys.LeftControl, Board.RotateRight);
            InputManager.RegisterKeyPressed(Keys.Down, Board.DropPieceByOne, true, 100);
            // TODO: KG - Adjust left/right speed
            InputManager.RegisterKeyPressed(Keys.Left, Board.MoveLeft, true, 120);
            InputManager.RegisterKeyPressed(Keys.Right, Board.MoveRight, true, 120);
            // TODO: KG - Restart Game (With Confirmation)
            // TODO: KG - Quit Game (With Confirmation)
            //InputManager.RegisterKeyPressed(Keys.Escape, Application.Exit);
            InputManager.RegisterKeyPressed(Keys.R, RestartGame);
            //InputManager.RegisterKeyPressed(Keys.F, _graphics.ToggleFullScreen);
            InputManager.RegisterKeyPressed(Keys.Z, Board.CurrentPiece.Debug_AddBlock, true);
        }

        private void TogglePause()
        {
            TetrisGame.GetInstance().IsRunning = !TetrisGame.GetInstance().IsRunning;
            SoundManager.GetInstance().PlaySound("Pause");
        }

        private void RestartGame()
        {
            Board.Reset();
        }
        #endregion
    }
}
