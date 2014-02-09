using System;
using System.Collections.Generic;
using GameClient.Classes.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameClient.Classes.ParticleSystem
{
    public class ParticleEngine : ISprite
    {
        #region Fields
        private Random _random;
        private List<Particle> _particles;
        private List<Texture2D> _textures;
        #endregion


        #region Properties
        public Vector2 EmitterLocation { get; set; }
        #endregion


        #region Constructors
        public ParticleEngine(List<Texture2D> textures, Vector2 location)
        {
            EmitterLocation = location;
            _textures = textures;
            _particles = new List<Particle>();
            _random = new Random();
        }
        #endregion


        #region Internal Implementation
        private Particle GenerateNewParticle()
        {
            Texture2D texture = _textures[_random.Next(_textures.Count)];
            Vector2 position = EmitterLocation;
            var velocity = new Vector2(1f * (float)(_random.NextDouble() * 2 - 1),
                                       1f * (float)(_random.NextDouble() * 2 - 1));
            float angle = 0;
            float angularVelocity = 0.1f * (float)(_random.NextDouble() * 2 - 1);
            var color = new Color((float)_random.NextDouble(),
                                  (float)_random.NextDouble(),
                                  (float)_random.NextDouble());
            var size = (float)_random.NextDouble();
            int ttl = 20 + _random.Next(40);

            return new Particle(texture, position, velocity, angle, angularVelocity, color, size, ttl);
        }
        #endregion


        #region Implementation of ISprite
        public void Update(GameTime gameTime)
        {
            int total = 10;
            for (int i = 0; i < total; i++)
            {
                _particles.Add(GenerateNewParticle());
            }

            for (int particle = 0; particle < _particles.Count; particle++)
            {
                _particles[particle].Update(gameTime);
                if (_particles[particle].TimeToLive <= 0)
                {
                    _particles.RemoveAt(particle);
                    particle--;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Begin();
            foreach (Particle particle in _particles)
            {
                particle.Draw(spriteBatch, gameTime);
            }
            spriteBatch.End();
        }
        #endregion
    }
}
