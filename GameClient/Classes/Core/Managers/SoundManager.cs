using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;


namespace GameClient.Classes.Core.Managers
{
    public class SoundManager : IDisposable
    {
        #region Singleton Pattern
        private static SoundManager Instance { get; set; }
        
        public static SoundManager GetInstance()
        {
            return Instance ?? (Instance = new SoundManager());
        }
        #endregion


        #region Fields
        private readonly AudioEngine _audioEngine;
        private readonly SoundBank _soundBank;
        // ReSharper disable once NotAccessedField.Local
        private readonly WaveBank _waveBank;
        private Cue _currentMusic;
        private readonly Cue[] _currentSounds;
        #endregion


        #region Constructor
        private SoundManager()
        {
            _audioEngine = new AudioEngine("Content/Sounds/TetrisGame.xgs");
            _waveBank = new WaveBank(_audioEngine, "Content/Sounds/WaveBank.xwb");
            _soundBank = new SoundBank(_audioEngine, "Content/Sounds/SoundBank.xsb");
            _audioEngine.Update();
            _currentMusic = _soundBank.GetCue("m_Silence");
            _currentMusic.Stop(AudioStopOptions.Immediate);
            _currentSounds = new Cue[20];
            SetMusicVolume(100);
            SetSoundVolume(100);
        }
        #endregion


        #region Public Methods
        public void Update(GameTime gameTime)
        {
            _audioEngine.Update();
        }

        public void PlaySound(string soundName, float volume = 1)
        {
            for (int i = 0; i < 20; i++)
            {
                if (_currentSounds[i] == null || !_currentSounds[i].IsPlaying)
                {
                    _currentSounds[i] = _soundBank.GetCue(soundName);
                    _currentSounds[i].Play();
                    break;
                }
            }
        }

        public void PlayMusic(string musicName)
        {
            if (string.IsNullOrEmpty(musicName))
            {
                musicName = "m_Silence";
            }
            if (musicName != _currentMusic.Name)
            {
                _currentMusic = _soundBank.GetCue(musicName);
                _currentMusic.Play();
            }
        }

        public void SetMusicVolume(float volume)
        {
            volume = MathHelper.Clamp(volume, 0.0f, 1f);
            _audioEngine.GetCategory("Music").SetVolume(volume);
        }

        public void SetSoundVolume(float volume)
        {
            volume = MathHelper.Clamp(volume, 0.0f, 1f);
            _audioEngine.GetCategory("Default").SetVolume(volume);
        }
        #endregion


        #region Implement IDisposable Interface
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~SoundManager()
        {
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_audioEngine != null)
                {
                    _audioEngine.Dispose();
                }
                if (_soundBank != null)
                {
                    _soundBank.Dispose();
                }
                if (_waveBank != null)
                {
                    _waveBank.Dispose();
                }
            }
        }
        #endregion
    }
}