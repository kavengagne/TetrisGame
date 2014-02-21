using System;

namespace GameClient.Classes.Core.Settings
{
    public class Configuration
    {
        #region Singleton Pattern
        private static Configuration Instance { get; set; }

        public static Configuration GetInstance()
        {
            return Instance ?? (Instance = new Configuration());
        }
        #endregion


        #region Fields
        private readonly string _serverAddress;
        private readonly int _windowWidth;
        private readonly int _windowHeight;
        #endregion


        #region Properties
        public string ServerAddress
        {
            get
            {
                var settings = Properties.Settings.Default;
                return !String.IsNullOrWhiteSpace(settings.ServerAddress) ? settings.ServerAddress : _serverAddress;
            }
        }

        public int WindowWidth
        {
            get
            {
                var settings = Properties.Settings.Default;
                if (settings.WindowWidth > Defaults.Window.MinimumWidth &&
                    settings.WindowHeight > Defaults.Window.MinimumHeight)
                {
                    return settings.WindowWidth;
                }
                return _windowWidth;
            }
        }

        public int WindowHeight
        {
            get
            {
                var settings = Properties.Settings.Default;
                if (settings.WindowWidth > Defaults.Window.MinimumWidth &&
                    settings.WindowHeight > Defaults.Window.MinimumHeight)
                {
                    return settings.WindowHeight;
                }
                return _windowHeight;
            }
        }
        #endregion


        #region Constructor
        private Configuration()
        {
            _serverAddress = Defaults.Server.Address;
            _windowWidth = Defaults.Window.Width;
            _windowHeight = Defaults.Window.Height;
        }
        #endregion
    }
}
