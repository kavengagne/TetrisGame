using System;
using System.Linq;
using System.Windows.Forms;
using GameClient.Classes.Core;
using GameClient.Classes.Extensions;
using GameClient.Classes.Utilities;
using GameConfiguration.Classes;
using GameConfiguration.DataObjects;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;

namespace GameClient.Classes
{
    public sealed class Application
    {
        #region Singleton Pattern
        private static readonly Application ApplicationInstance = new Application();

        public static Application Instance
        {
            get { return ApplicationInstance; }
        }

        static Application()
        {
        }

        private Application()
        {
            // TODO: KG - Remove Mocking Data Here
            var col = new[]
            {
                Color.Red, Color.LightBlue, Color.White, Color.Yellow,
                Color.Orange, Color.LightGreen, Color.Pink, Color.Violet
            };
            Console.WriteLine(JsonConvert.SerializeObject(col));
        }
        #endregion


        #region Properties
        public ClientInformation Client { get; set; }
        public Configuration Configuration { get; set; }
        public bool IsRunning { get; set; }
        public TetrisGame Game { get; set; }
        #endregion


        #region Public Methods
        public void Run()
        {
            if (!IsRunning)
            {
                LoadConfiguration();
                //if (IsGameVersionOutdated())
                //{
                //    StartUpdater();
                //}
                //else
                //{
                StartGame();
                //}
            }
        }

        public static void Exit()
        {
            Instance.Game.Exit();
        }
        #endregion


        #region Internal Implementation
        private void LoadConfiguration()
        {
            Client = ConfigurationLoader.GetClientConfiguration();
            Configuration = ConfigurationLoader.GetServerConfiguration(Client);
        }

        private bool IsGameVersionOutdated()
        {
            var versionComparer = new VersionComparer(Client.Version, Configuration.Game.ClientVersion);
            return versionComparer.Result == VersionComparerResult.Outdated
                || versionComparer.Result == VersionComparerResult.Invalid;
        }

        private void StartUpdater()
        {
            // TODO: KG - Call external process to handle software update.
            MessageBox.Show("Call Updater Here");
        }

        private void StartGame()
        {
            Game = new TetrisGame();

            // ReSharper disable once UnusedVariable
            var graphics = new GraphicsDeviceManager(Game)
            {
                PreferredBackBufferWidth = Client.WindowWidth,
                PreferredBackBufferHeight = Client.WindowHeight
            };

            Game.Window.Title = Client.WindowName;
            Game.Window.SetPosition(new Point((Screen.PrimaryScreen.Bounds.Width - Client.WindowWidth) / 2, 20));

            Game.Run();
        }
        #endregion
    }
}