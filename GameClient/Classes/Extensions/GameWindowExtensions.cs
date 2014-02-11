
// Taken from http://projectdrake.net/blog/2013/03/31/tutorial-setting-window-position-in-xnamonogame/
// Thank you Eniko :)


using System;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Xna.Framework;

namespace GameClient.Classes.Extensions
{
    public static class GameWindowExtensions
    {
        #region Public Methods
        public static void SetLocation(this GameWindow window, System.Drawing.Point position)
        {
            var form = GetForm(window);
            form.Location = position;
        }

        public static void SetMinimumSize(this GameWindow window, Size minimumSize)
        {
            var form = GetForm(window);
            form.MinimumSize = minimumSize;
        }

        private static Form GetForm(GameWindow window)
        {
            var form = (System.Windows.Forms.Form)System.Windows.Forms.Control.FromHandle(window.Handle);
            return form;
        }
        #endregion
    }
}
