using GameModel.Utils;

namespace GameModel.Models
{
    public class UserLoginModel
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
        #endregion


        #region Constructor
        public UserLoginModel(string username, string password)
        {
            Username = username;
            Password = password;
        }
        #endregion
    }
}