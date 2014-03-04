using System;
using System.Diagnostics;
using System.Linq;
using System.Web.Http;
using GameData.Contexts;
using GameData.Utils;
using GameModel.Models;

namespace GameServer.Controllers
{
    public class GamesController : ApiController
    {
        // POST games
        [HttpPost]
        [Authorize]
        [ActionName("games")]
        public bool AddGame([FromBody]Game game)
        {
            if (!ModelState.IsValid)
            {
                return false;
            }
            using (var db = new StatisticsDbContext())
            {
                var username = RequestContext.Principal.Identity.Name;
                var user = UserUtils.GetUserByUsername(username);
                game.GameID = 0;
                game.UserID = user.UserID;
                game.PlayedDate = DateTime.Now;
                var newGame = db.Games.Add(game);
                db.SaveChanges();
                Debug.WriteLine("GameID:{0}, UserID:{1}", newGame.GameID, newGame.UserID);
            }
            return true;
        }
    }
}