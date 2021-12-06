using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;

namespace event_client_app.Configure
{
    public static class MvcOptionExtensions
    {
        // "this" extension method 
        public static void UseCentralRouterPrefix(this MvcOptions opts, IRouteTemplateProvider routeAttribute)
        {
            opts.Conventions.Insert(0, new RouteConvention(routeAttribute));
        }
        
    }
}