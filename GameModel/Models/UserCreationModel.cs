using GameModel.Utils;

namespace GameModel.Models
{
    public class UserCreationModel
    {
        #region Fields
        private string _password;
        #endregion


        #region Properties
        public string Username { get; private set; }

        public string Password
        {
            get { return _password; }
            private set
            {
                _password = PasswordHash.CreateHashWithClientSalt(value);
            }
        }

        public string Country { get; set; }
        #endregion


        #region Constructor
        public UserCreationModel(string username, string password, string country)
        {
            Username = username;
            Password = password;
            Country = country;
        }
        #endregion
    }
}