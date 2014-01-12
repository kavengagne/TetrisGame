using System;
using System.Linq;
using System.Windows.Forms;
using GameClient.Classes.Extensions;
using GameConfiguration.Classes;
using GameConfiguration.DataObjects;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using Color = Microsoft.Xna.Framework.Color;
using Point = Microsoft.Xna.Framework.Point;

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


        #region Fields

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
                StartGame();
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

        private void StartGame()
        {
            Game = new TetrisGame();

            Game.Window.Title = Client.WindowName;
            Game.Window.SetPosition(new Point((Screen.PrimaryScreen.Bounds.Width - Client.WindowWidth) / 2, 20));

            // ReSharper disable once UnusedVariable
            var graphics = new GraphicsDeviceManager(Game)
            {
                PreferredBackBufferWidth = Client.WindowWidth,
                PreferredBackBufferHeight = Client.WindowHeight
            };

            Game.Run();
        }
        #endregion
    }
}