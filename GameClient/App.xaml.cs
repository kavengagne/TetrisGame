using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using GameClient.Classes.Core;
using GameClient.Classes.Core.PageSwitcher;
using GameClient.Models;
using GameClient.Pages;
using GameConfiguration.Classes;
using GameConfiguration.DataObjects;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;


namespace GameClient
{
    public partial class App
    {
        #region Properties
        public static App Instance { get; private set; }
        #endregion


        #region Singleton Pattern
        private App()
        {
            // TODO: KG - Remove Mocking Data Here
            var col = new[]
            {
                Color.Red, Color.LightBlue, Color.White, Color.Yellow,
                Color.Orange, Color.LightGreen, Color.Pink, Color.Violet
            };
            Console.WriteLine(JsonConvert.SerializeObject(col));

            Instance = this;
            ExecuteApp();
        }
        #endregion


        #region Properties
        public ClientInformation Client { get; set; }
        public Configuration Configuration { get; set; }
        public TetrisGame Game { get; set; }
        #endregion


        #region Public Methods
        public static new void Exit()
        {
            // TODO: KG - Maybe Exit the Application, or Show the Pause Menu.
            //Instance.Exit();
        }
        #endregion


        #region Internal Implementation
        private void ExecuteApp()
        {
            try
            {
                LoadConfiguration();
            }
            catch (Exception ex)
            {
                File.WriteAllText("error_log.txt", ex.ToString());
                Process.Start("error_log.txt");
            }
        }

        private void LoadConfiguration()
        {
            Client = ConfigurationLoader.GetClientConfiguration();
            Configuration = ConfigurationLoader.GetServerConfiguration(Client);
        }
        #endregion
    }
}
