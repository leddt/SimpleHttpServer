using System.Linq;
using DDT.SimpleHttpServer.Logging;
using DDT.SimpleHttpServer.Routing;

namespace DDT.SimpleHttpServer.RequestHandlers
{
    public class RouteRequestHandler : IRequestHandler
    {
        private ILogger logger;

        public bool HandleRequest(IHttpContext context)
        {
            EnsureLogger(context.ServerInfo.LoggerFactory);

            var requestPath = context.Request.Url.AbsolutePath;
            var route = GetMatchingRoute(requestPath, context);

            if (route == null)
            {
                logger.Debug("No matching route for {0}", requestPath);
                return false;
            }

            logger.Debug("Found matching route for {0}", requestPath);
            logger.Info("Dispatching request to route {0}", route);
            return route.HandleRequest(context);
        }

        private void EnsureLogger(ILoggerFactory loggerFactory)
        {
            if (logger == null)
                logger = loggerFactory.GetLogger("SimpleHttpServer.Handlers.RouteRequestHandler");
        }

        private static IRoute GetMatchingRoute(string virtualPath, IHttpContext context)
        {
            return context.ServerInfo.Routes.FirstOrDefault(x => x.IsMatch(virtualPath));
        }
    }
}