using System;
using System.Windows;
using GameClient.Classes.Core.PageSwitcher;

namespace GameClient.Pages
{
    public partial class MainMenuPage : ISwitchable
    {
        #region Constructor
        public MainMenuPage()
        {
            InitializeComponent();
        }
        #endregion


        #region Implementation of ISwitchable
        public void UtilizeState(object state)
        {
            throw new NotImplementedException();
        }
        #endregion


        private void ButtonPlay_OnClick(object sender, RoutedEventArgs e)
        {
            App.Instance.Game.IsRunning = true;
            Switcher.Switch(typeof(GamePage));
        }
    }
}
