﻿using System;
using Microsoft.Xna.Framework;

// Taken from http://projectdrake.net/blog/2013/03/31/tutorial-setting-window-position-in-xnamonogame/
// Thank you Eniko :)
namespace GameClient.Classes.Extensions
{
    public static class GameWindowExtensions
    {
        #region Public Methods
        public static void SetPosition(this GameWindow window, Point position)
        {
            OpenTK.GameWindow otkWindow = GetForm(window);
            if (otkWindow != null)
            {
                otkWindow.X = position.X;
                otkWindow.Y = position.Y;
            }
        }

        public static OpenTK.GameWindow GetForm(this GameWindow gameWindow)
        {
            Type type = typeof(OpenTKGameWindow);
            System.Reflection.FieldInfo field = type.GetField("window", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (field != null)
                return field.GetValue(gameWindow) as OpenTK.GameWindow;
            return null;
        } 
        #endregion
    }
}
