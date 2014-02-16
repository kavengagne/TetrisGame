﻿using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using GameClient.Classes.Core;
using GameClient.Classes.Extensions;
using GameConfiguration.Classes;
using GameConfiguration.DataObjects;
using Newtonsoft.Json;
using Color = Microsoft.Xna.Framework.Color;

namespace GameClient.Classes
{
    public sealed class Application
    {
        #region Singleton Pattern
        private static Application Instance { get; set; }
        
        public static Application GetInstance()
        {
            return Instance ?? (Instance = new Application());
        }
        #endregion


        #region Properties
        public ClientInformation Client { get; set; }
        public Configuration Configuration { get; set; }
        #endregion


        #region Constructor
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


        #region Public Methods
        public void Run()
        {
            try
            {
                LoadConfiguration();
                StartGame();
            }
            catch (Exception ex)
            {
                File.WriteAllText("error_log.txt", ex.ToString());
                Process.Start("error_log.txt");
            }
        }

        public static void Exit()
        {
            TetrisGame.GetInstance().Exit();
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
            var game = TetrisGame.GetInstance();

            game.Window.Title = Client.WindowName;
            game.Window.SetLocation(
                new System.Drawing.Point((Screen.PrimaryScreen.Bounds.Width - Client.WindowWidth) / 2, 20));
            game.Window.SetMinimumSize(new System.Drawing.Size(816, 639));
            game.Window.AllowUserResizing = true;

            game.Run();
        }
        #endregion
    }
}