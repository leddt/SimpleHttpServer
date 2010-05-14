using System.Collections.Generic;

namespace DDT.SimpleHttpServer.Routing
{
    public class RouteContext
    {
        public IHttpContext HttpContext { get; set; }
        public IDictionary<string, string> RouteData { get; set; }

        public RouteContext(IHttpContext context, IDictionary<string, string> routeData)
        {
            HttpContext = context;
            RouteData = routeData;
        }
    }
}