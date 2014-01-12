using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameClient.Interfaces
{
    public interface ISprite
    {
        void Draw(SpriteBatch spriteBatch, GameTime gameTime);
    }
}
