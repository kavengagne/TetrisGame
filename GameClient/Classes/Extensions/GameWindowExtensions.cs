using System.Drawing;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Point = System.Drawing.Point;

namespace GameClient.Classes.Extensions
{
    public static class GameWindowExtensions
    {
        #region Public Methods
        public static void SetLocation(this GameWindow window, Point position)
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
            var form = (Form)Control.FromHandle(window.Handle);
            return form;
        }
        #endregion
    }
}
