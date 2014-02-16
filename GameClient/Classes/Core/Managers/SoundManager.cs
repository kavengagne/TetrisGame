using System;
using System.Collections.Generic;
using System.Linq;
using GameClient.Classes.Extensions;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

namespace GameClient.Classes.Core.Managers
{
    public class SoundManager
    {
        #region Singleton
        private static SoundManager Instance { get; set; }
        
        public static SoundManager GetInstance()
        {
            return Instance ?? (Instance = new SoundManager());
        }
        #endregion


        #region Fields
        private readonly Dictionary<string, SoundEffect> _effects;
        #endregion


        #region Constructor
        private SoundManager()
        {
            _effects = LoadSoundEffects(new ContentManager(TetrisGame.GetInstance().Services, "Content"));
        }
        #endregion


        #region Public Methods
        public void Play(string soundName, float volume = 1, float pitch = 0, float pan = 0)
        {
            SoundEffect effect;
            if (_effects.TryGetValue(soundName, out effect))
            {
                int callCount = 1;
                if (volume > 1)
                {
                    callCount = (int)volume;
                }
                for (int i = 0; i < callCount; i++)
                {
                    effect.Play(volume, pitch, pan);
                }
            }
            else
            {
                throw new ArgumentException("Parameter soundName is not valid. Value \"{0}\" does not exist.", soundName);
            }
        }
        #endregion


        #region Internal Implementation
        private static Dictionary<string, SoundEffect> LoadSoundEffects(ContentManager contentManager)
        {
            return contentManager.LoadContentFolder<SoundEffect>("Sounds");
        }
        #endregion
    }
}