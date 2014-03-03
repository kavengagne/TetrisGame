namespace GameModel.Models
{
    public class UserCreationModel : BaseUserModel
    {
        #region Properties
        public string Country { get; private set; }
        #endregion


        #region Constructor
        public UserCreationModel(string username, string password, string country) : base(username, password)
        {
            Country = country;
        }
        #endregion
    }
}