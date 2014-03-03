using System;
using System.Web.Http;
using GameModel.Models.Database;
using Newtonsoft.Json;

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
            var mygame = new Game
            {
                BackToBack = 2,
                Doubles = 4,
                Duration = TimeSpan.FromMinutes(2.5),
                GameID = 1111,
                Level = 10,
                Lines = 100,
                Score = 10234,
                Tetris = 4,
                Tspin = 1,
            };
            Console.WriteLine(JsonConvert.SerializeObject(mygame));
            return true;
        }
    }
}