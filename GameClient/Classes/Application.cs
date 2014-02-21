using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using GameClient.Classes.Core;
using GameClient.Classes.Core.Settings;
using GameClient.Classes.Extensions;

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


        #region Constructor
        private Application()
        {
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
            Configuration.GetInstance();
        }

        private void StartGame()
        {
            var game = TetrisGame.GetInstance();

            game.Window.Title = Defaults.Window.Name;
            game.Window.SetLocation(
                new Point((Screen.PrimaryScreen.Bounds.Width - Configuration.GetInstance().WindowWidth) / 2, 20));
            game.Window.SetMinimumSize(new Size(816, 639));
            game.Window.AllowUserResizing = true;

            game.Run();
        }
        #endregion
    }
}
