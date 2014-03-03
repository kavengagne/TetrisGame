using System;
using System.Linq;
using System.Text;
using System.Web.Http;
using GameModel.Contexts;
using GameModel.Models;
using GameModel.Models.Database;
using GameModel.Utils;

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
            return new User();
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
