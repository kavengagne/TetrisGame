﻿using GameClient.Classes.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameClient.Classes.ParticleSystem
{
    public class Particle : ISprite
    {
        #region Properties
        public Texture2D Texture { get; set; } // The texture that will be drawn to represent the particle
        public Vector2 Position { get; set; } // The current position of the particle        
        public Vector2 Velocity { get; set; } // The speed of the particle at the current instance
        public float Angle { get; set; } // The current angle of rotation of the particle
        public float AngularVelocity { get; set; } // The speed that the angle is changing
        public Color Color { get; set; } // The color of the particle
        public float Size { get; set; } // The size of the particle
        public int TimeToLive { get; set; } // The 'time to live' of the particle
        #endregion


        #region Constructors
        public Particle(Texture2D texture, Vector2 position, Vector2 velocity,
                        float angle, float angularVelocity, Color color, float size, int timeToLive)
        {
            Texture = texture;
            Position = position;
            Velocity = velocity;
            Angle = angle;
            AngularVelocity = angularVelocity;
            Color = color;
            Size = size;
            TimeToLive = timeToLive;
        }
        #endregion


        #region Implementation of ISprite
        public void Update(GameTime gameTime)
        {
            TimeToLive--;
            Position += Velocity;
            Angle += AngularVelocity;
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            var sourceRectangle = new Rectangle(0, 0, Texture.Width, Texture.Height);
            var origin = new Vector2(Texture.Width / 2.0f, Texture.Height / 2.0f);

            spriteBatch.Draw(Texture, Position, sourceRectangle, Color,
                             Angle, origin, Size, SpriteEffects.None, 0f);
        }
        #endregion
    }
}
