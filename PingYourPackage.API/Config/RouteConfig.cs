namespace PingYourPackage.API.Config
{
    using System.Web.Http;
    using PingYourPackage.API.Routing;

    public class RouteConfig
    {
        public static void RegisterRoutes(HttpRouteCollection routes)
        {
            routes.MapHttpRoute(
                "DefaultHttpRoute", 
                "api/{controller}/{key}", 
                defaults: new { key = RouteParameter.Optional }, 
                constraints: new { key = new GuidRouteConstraint() });
        }
    }
}