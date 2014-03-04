using System;
using System.Linq;
using System.Text;
using System.Web.Http;
using GameData.Contexts;
using GameData.Models;
using GameData.Utils;
using GameModel.Models;

namespace GameServer.Controllers
{
    public class UsersController : ApiController
    {
        // GET users
        [HttpGet]
        [Authorize]
        [ActionName("get")]
        public User GetUser()
        {
            var username = RequestContext.Principal.Identity.Name;
            return UserUtils.GetUserByUsername(username);
        }

        // POST users/create
        [HttpPost]
        [ActionName("create")]
        public UserCreationResultModel CreateUser([FromBody]UserCreationModel model)
        {
            if (!ModelState.IsValid)
            {
                return new UserCreationResultModel(false, ModelState);
            }
            // Check if user exists
            using (var db = new StatisticsDbContext())
            {
                var existingUsers = db.Users.Where(user => String.Equals(user.Username, model.Username));
                if (existingUsers.Any())
                {
                    return new UserCreationResultModel(false, new[]
                    {
                        String.Format("User with name '{0}' already exists.", model.Username)
                    });
                }
                db.Users.Add(new User
                {
                    Username = model.Username,
                    Password = PasswordHash.CreateHashWithRandomSalt(model.Password),
                    Country = model.Country,
                    RegisteredDate = DateTime.Now,
                    LastLoginDate = DateTime.Now
                });
                db.SaveChanges();
                return new UserCreationResultModel(true);
            }
        }

        // GET users/stats
        [HttpGet]
        [Authorize]
        [ActionName("stats")]
        public UserStatistics GetUserStatistics()
        {
            return new UserStatistics();
        }
    }
}
