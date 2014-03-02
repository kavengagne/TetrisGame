using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Web.Http;
using GameModel.Models;
using GameModel.Models.Database;
using GameModel.Utils;
using Newtonsoft.Json;

namespace GameServer.Controllers
{
    public class UsersController : ApiController
    {
        // GET users/{sessionID}
        [HttpGet]
        [ActionName("get")]
        public User GetUserBySessionID(int sessionID)
        {
            return new User();
        }

        // POST users/create
        [HttpPost]
        [ActionName("create")]
        public User CreateUser([FromBody]UserCreationModel model)
        {
            var user = new UserCreationModel("kaven", "password", "canada");
            var randomHash = PasswordHash.CreateHashWithRandomSalt(model.Password);
            Debug.WriteLine(PasswordHash.ValidatePassword(model.Password, randomHash));
            return new User();
        }

        // POST users/login
        [HttpPost]
        [ActionName("login")]
        public Session LoginUser([FromBody]UserLoginModel model)
        {
            return new Session();
        }

        // POST users/logout
        [HttpPost]
        [ActionName("logout")]
        public void LogoutUser([FromBody]int sessionID)
        {
        }

        // GET users/stats
        [HttpGet]
        [ActionName("stats")]
        public UserStatistics GetUserStatistics(int sessionID)
        {
            return new UserStatistics();
        }
    }
}
