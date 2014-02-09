using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace GameClient.Classes.Core.PageSwitcher
{
  	public static class Switcher
  	{
    	public static PageSwitcherWindow PageSwitcherWindow;
        public static Dictionary<Type, UserControl> PageList;

    	public static void Switch(UserControl newPage, object state = null)
    	{
      		PageSwitcherWindow.Navigate(newPage, state);
    	}
        public static void Switch(Type pageType, object state = null)
        {
            var page = PageList[pageType]; //.First(x => x.GetType() == pageType);
            if (page != null)
            {
                PageSwitcherWindow.Navigate(page, state);
            }
        }
  	}
}
