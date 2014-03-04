using System.Web.Http;

namespace GameServer.App_Start
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Formatters.JsonFormatter
                  .SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;

            config.Routes.MapHttpRoute(
                "ErrorsApi",
                "errors",
                new { controller = "Errors", action = "log" }
            );

            config.Routes.MapHttpRoute(
                "GameStatsApi",
                "games",
                new { controller = "Games", action = "games" }
            );

            config.Routes.MapHttpRoute(
                "UserManagementApi",
                "users/{action}",
                new { controller = "Users", action = "get" }
            );

            config.Routes.MapHttpRoute(
                "StatisticsApi",
                "stats/{action}",
                new { controller = "Statistics" }
            );

            config.Routes.MapHttpRoute(
                "DefaultApi",
                "tetris/{controller}/{id}",
                new { id = RouteParameter.Optional }
            );

            config.Formatters.Remove(config.Formatters.XmlFormatter);

            // Uncomment the following line of code to enable query support for actions with an IQueryable or IQueryable<T> return type.
            // To avoid processing unexpected or malicious queries, use the validation settings on QueryableAttribute to validate incoming queries.
            // For more information, visit http://go.microsoft.com/fwlink/?LinkId=279712.
            //config.EnableQuerySupport();

            // To disable tracing in your application, please comment out or remove the following line of code
            // For more information, refer to: http://www.asp.net/web-api
            config.EnableSystemDiagnosticsTracing();
        }
    }
}
