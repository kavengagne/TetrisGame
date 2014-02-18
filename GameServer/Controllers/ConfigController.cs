using System.Web.Http;

namespace GameServer.Controllers
{
    public class ConfigController : ApiController
    {
        // GET api/values
        public string Get()
        {
            // TODO: KG - Add error handling
            //var data = File.ReadAllText(HttpContext.Current.Server.MapPath("~/Content/config.json"));
            //return JsonConvert.DeserializeObject<Configuration>(data);
            return "Kaven";
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