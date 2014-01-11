using System.Net.Mime;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameClient.Classes.Extensions
{
    public static class Texture2DExtensions
    {
        #region Public Methods
        public static void FillWithColor(this Texture2D texture, Color color)
        {
            var textureColors = new Color[texture.Width * texture.Height];
            for (int i = 0; i < textureColors.Length; i++)
            {
                textureColors[i] = color;
            }
            texture.SetData(textureColors);
        }

        public static void AddBorder(this Texture2D texture, Color color, int thickness)
        {
            var oldColors = new Color[texture.Width * texture.Height];
            texture.GetData(oldColors);
            var newColors = new Color[texture.Width * texture.Height];
            for (int i = 0; i < newColors.Length; i++)
            {
                int row = i / texture.Width;
                var col = i % texture.Width;
                // Keep Current Colors
                newColors[i] = oldColors[i];
                // Top Border
                if (row < thickness)
                {
                    newColors[i] = color;
                }
                // Bottom Border
                if (row >= texture.Height - thickness)
                {
                    newColors[i] = color;
                }
                // Left Border
                if (col < thickness)
                {
                    newColors[i] = color;
                }
                // Right Border
                if (col >= texture.Width - thickness)
                {
                    newColors[i] = color;
                }
            }
            texture.SetData(newColors);
        }
        #endregion
    }
}
