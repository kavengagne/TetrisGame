using System.Web.Http;
using GameData.Contexts;
using GameData.Models;

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

                return new LeaderBoard();
            }
        }
    }
}
