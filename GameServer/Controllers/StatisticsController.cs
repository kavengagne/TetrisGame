using System.Collections.Generic;
using System.Web.Http;

namespace GameServer.Controllers
{
    public class StatisticsController : ApiController
    {
        // GET api/statistics
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/statistics/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/statistics
        public void Post([FromBody]string value)
        {
        }

        // PUT api/statistics/5
        public void Put(int id, [FromBody]string value)
        {
        }
    }
}
