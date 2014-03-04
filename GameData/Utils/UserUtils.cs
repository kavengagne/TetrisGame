using System;
using System.Linq;
using System.Text;
using GameData.Contexts;
using GameModel.Models;

namespace GameData.Utils
{
    public class UserUtils
    {
        public static User GetUserByUsername(string username)
        {
            var db = new StatisticsDbContext();
            {
                var currentUser = db.Users.Where(u => String.Equals(u.Username, username));
                if (!currentUser.Any())
                {
                    return null;
                }
                var user = currentUser.ToList().First();
                user.Password = String.Empty;
                return user;
            }
        }
    }
}
