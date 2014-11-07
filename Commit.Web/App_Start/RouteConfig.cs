using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Commit.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "School",
                url: "School/{name}",
                defaults: new { controller = "School", action = "Info", name = UrlParameter.Optional }
            ).RouteHandler = new SchoolRouteHandler();

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional });

        }
    }

    public class SchoolRouteHandler : MvcRouteHandler
    {
        protected override IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            var url = requestContext.HttpContext.Request.Path.TrimStart('/');

            var schoolName = url.Split(new[] {'/'})[1];

            if (!string.IsNullOrEmpty(url))
            {
                FillRequest("School", "Info", schoolName, requestContext);
            }

            return base.GetHttpHandler(requestContext);
        }

        private static void FillRequest(string controller, string action, string name, RequestContext requestContext)
        {
            if (requestContext == null)
            {
                throw new ArgumentNullException("requestContext");
            }

            requestContext.RouteData.Values["controller"] = controller;
            requestContext.RouteData.Values["action"] = action;
            requestContext.RouteData.Values["name"] = name;
        }
    }
}
