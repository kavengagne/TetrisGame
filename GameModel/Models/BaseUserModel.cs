using System.ComponentModel.DataAnnotations;

namespace GameModel.Models
{
    public abstract class BaseUserModel
    {
        #region Properties
        [Required]
        [StringLength(30, ErrorMessage = "{0} must be between {2} and {1} characters length.", MinimumLength = 5)]
        public string Username { get; private set; }

        [Required]
        [StringLength(30, ErrorMessage = "{0} must be between {2} and {1} characters length.", MinimumLength = 5)]
        public string Password { get; private set; }
        #endregion


        #region Constructor
        protected BaseUserModel(string username, string password)
        {
            Username = username;
            Password = password;
        }
        #endregion
    }
}