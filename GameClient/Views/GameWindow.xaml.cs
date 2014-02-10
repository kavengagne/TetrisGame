using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using GameClient.Models;

namespace GameClient.Views
{
    public partial class GameWindow
    {
        public GameWindow()
        {
            InitializeComponent();
            DataContext = new GamePageModel();
        }

        private void MenuItemConfiguration_OnClick(object sender, RoutedEventArgs e)
        {
            //Console.WriteLine("ww:{0}, wh:{1}", this.Width, this.Height);
            var item = sender as MenuItem;
            if (item != null && item.IsChecked)
            {
                Debug.WriteLine("Checked");
            }
        }
    }
}
