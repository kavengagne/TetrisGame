namespace GameData.Models
{
    public class UserLoginModel : BaseUserModel
    {
        public UserLoginModel(string username, string password) : base(username, password)
        {
        }
    }
}