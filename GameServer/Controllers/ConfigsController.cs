using System.Web.Http;

namespace GameServer.Controllers
{
    public class ConfigsController : ApiController
    {
        // GET api/configs
        public string Get()
        {
            // TODO: KG - Add error handling
            //var data = File.ReadAllText(HttpContext.Current.Server.MapPath("~/Content/config.json"));
            //return JsonConvert.DeserializeObject<Configuration>(data);
            return "Kaven";
        }
    }
}