using System;
using GameClient.Classes.Core.PageSwitcher;
using GameClient.Models;

namespace GameClient.Pages
{
    public partial class GamePage : ISwitchable
    {
        #region Properties

        #endregion


        #region Constructor
        public GamePage()
        {
            InitializeComponent();
            DataContext = new GamePageModel();
        }
        #endregion


        #region Implementation of ISwitchable
        public void UtilizeState(object state)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
