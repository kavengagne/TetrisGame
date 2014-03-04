using System.Linq;
using System.Web.Http;
using GameModel.Models;

namespace GameServer.Controllers
{
    public class ErrorsController : ApiController
    {
        // GET api/errors
        [HttpPost]
        [ActionName("log")]
        public void LogError([FromBody]ClientErrorModel clientErrorModel)
        {
            
        }
    }
}
