using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using slim;
using slim.App_Start;

namespace Commit.Web.Angular
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}
