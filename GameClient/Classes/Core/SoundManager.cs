using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

namespace GameClient.Classes.Core
{
    public class SoundManager
    {
        #region Fields
        private readonly Dictionary<string, SoundEffect> _effects;
        #endregion


        #region Constructor
        public SoundManager(ContentManager contentManager)
        {
            _effects = LoadSoundEffects(contentManager);
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
                throw new ArgumentException("Parameter soundName is not valid. Value \"{0}\" does not exist.", "soundName");
            }
        }
        #endregion


        #region Internal Implementation
        private static Dictionary<string, SoundEffect> LoadSoundEffects(ContentManager contentManager)
        {
            var list = new Dictionary<string, SoundEffect>();
            var soundList = Directory.EnumerateFiles(Directory.GetCurrentDirectory() + @"\Content\Sounds", "*.xnb",
                                                     SearchOption.AllDirectories);
            foreach (var fileName in soundList)
            {
                var pathParts = fileName.Split(new[] { '\\', '/' });
                var pathList = pathParts.SkipWhile(p => !p.Equals("Content")).Skip(1)
                                        .Select(Path.GetFileNameWithoutExtension).ToList();
                var assetName = Path.GetFileNameWithoutExtension(pathList.Last());
                var assetPath = String.Join("/", pathList);
                var effect = contentManager.Load<SoundEffect>(assetPath);
                if (assetName != null)
                {
                    list.Add(assetName, effect);
                }
            }

            return list;
        }
        #endregion
    }
}