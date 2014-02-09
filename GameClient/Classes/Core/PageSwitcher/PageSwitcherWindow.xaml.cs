using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Controls;
using GameClient.Pages;

namespace GameClient.Classes.Core.PageSwitcher
{
    public partial class PageSwitcherWindow
    {
        public PageSwitcherWindow()
        {
            InitializeComponent();
            Switcher.PageSwitcherWindow = this;
            Switcher.PageList = CreatePagesInstances();
            Switcher.Switch(typeof(MainMenuPage));
        }

        private Dictionary<Type, UserControl> CreatePagesInstances()
        {
            var list = new Dictionary<Type, UserControl>
            {
                { typeof(MainMenuPage), new MainMenuPage() },
                { typeof(GamePage), new GamePage() }
            };

            foreach (var item in list)
            {
            }

            return list;
        }

        public void Navigate(UserControl nextPage, object state)
        {
            this.Content = nextPage;
            if (state == null) return;

            // ReSharper disable once SuspiciousTypeConversion.Global
            var s = nextPage as ISwitchable;
            if (s != null)
            {
                s.UtilizeState(state);
            }
            else
            {
                throw new ArgumentException("NextPage is not ISwitchable! " +
                                            nextPage.Name.ToString(CultureInfo.InvariantCulture));
            }
        }
    }
}
