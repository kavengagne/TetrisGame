using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using GameServer.App_Start;
using GameServer.Authentication;

namespace GameServer
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configuration.MessageHandlers.Add(new BasicAuthMessageHandler
            {
                PrincipalProvider = new DatabasePrincipalProvider()
            });

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}