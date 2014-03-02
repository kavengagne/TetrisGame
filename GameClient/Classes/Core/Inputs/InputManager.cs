using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GameClient.Classes.Core.Inputs
{
    /// <summary>
    /// InputManager is an XNA/MonoGame utility class that allows you to register inputs
    /// to be triggered in a more event-driven way than the game loop.
    /// 
    /// TODO: KG - Add Player Management (Player1, Player2, Computer)
    /// TODO: KG - Add Input Selection (Keyboard, GamePad, Mouse)
    /// </summary>
    public class InputManager
    {
        #region Fields
        private readonly Dictionary<Keys, KeyPressedAction> _keyPressed = new Dictionary<Keys, KeyPressedAction>();
        private readonly Dictionary<Keys, KeyReleasedAction> _keyReleased = new Dictionary<Keys, KeyReleasedAction>();
        private readonly Dictionary<Keys, KeyHeldAction> _keyHeld = new Dictionary<Keys, KeyHeldAction>();
        #endregion


        #region Constructor

        #endregion


        #region Input Registration Methods
        /// <summary>
        /// Registers a KeyPress handler that will trigger the given action
        /// and then will, optionaly, repeat this same action until you release the key.
        /// </summary>
        /// <param name="key">The key that will trigger the action.</param>
        /// <param name="action">The action to trigger.</param>
        /// <param name="repeatAction">Should the action be repeated? Default is false.</param>
        /// <param name="delayMilliseconds">The repeat delay.</param>
        public void RegisterKeyPressed(Keys key, Action action, bool repeatAction = false, int delayMilliseconds = 0)
        {
            _keyPressed[key] = new KeyPressedAction(action, repeatAction, delayMilliseconds);
        }

        /// <summary>
        /// Registers a KeyRelease handler that will trigger the
        /// given action only after pressing and releasing the key.
        /// </summary>
        /// <param name="key">The key that will trigger the action.</param>
        /// <param name="action">The action to trigger.</param>
        public void RegisterKeyReleased(Keys key, Action action)
        {
            _keyReleased[key] = new KeyReleasedAction(action);
        }

        /// <summary>
        /// Registers a KeyPress handler that will trigger the given action
        /// when the key is held for the specified delay.
        /// </summary>
        /// <param name="key">The key that will trigger the action.</param>
        /// <param name="action">The action to trigger when the key is held.</param>
        /// <param name="delayMilliseconds">The delay to wait before triggering the action.</param>
        /// <param name="repeatAction">Should the action be repeated? Default is false.</param>
        public void RegisterKeyHeld(Keys key, Action action, bool repeatAction = false, int delayMilliseconds = 0)
        {
            _keyHeld[key] = new KeyHeldAction(action, repeatAction, delayMilliseconds);
        }
        #endregion


        #region Public Methods
        public void HandleInputs(GameTime gameTime)
        {
            HandleKeyPressed(gameTime);
            HandleKeyReleased();
            HandleKeyHeld(gameTime);
        }
        #endregion


        #region Internal Implementation
        private void HandleKeyPressed(GameTime gameTime)
        {
            foreach (var handler in _keyPressed)
            {
                handler.Value.Update(gameTime, handler.Key);
            }
        }

        private void HandleKeyReleased()
        {
            foreach (var handler in _keyReleased)
            {
                handler.Value.Update(handler.Key);
            }
        }

        private void HandleKeyHeld(GameTime gameTime)
        {
            foreach (var handler in _keyHeld)
            {
                handler.Value.Update(gameTime, handler.Key);
            }
        }
        #endregion
    }
}