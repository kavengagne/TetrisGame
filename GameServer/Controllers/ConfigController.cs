using System.IO;
using System.Web;
using System.Web.Http;
using GameConfiguration.DataObjects;
using Newtonsoft.Json;

namespace GameServer.Controllers
{
    public class ConfigController : ApiController
    {
        // GET api/values
        public Configuration Get()
        {
            // TODO: KG - Add error handling
            var data = File.ReadAllText(HttpContext.Current.Server.MapPath("~/Content/config.json"));
            return JsonConvert.DeserializeObject<Configuration>(data);
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}