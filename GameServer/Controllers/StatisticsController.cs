using System;
using System.Web.Http;
using GameModel.Contexts;
using GameModel.Models;
using GameModel.Models.Database;

namespace GameServer.Controllers
{
    public class StatisticsController : ApiController
    {
        // GET stats
        [HttpGet]
        [ActionName("leaderboard")]
        public LeaderBoard GetLeaderBoard()
        {
            using (var stats = new StatisticsDbContext())
            {
                stats.Games.Add(new Game { Duration = TimeSpan.FromMinutes(22), Score = 12035 });
                return new LeaderBoard();
            }
        }
    }
}
