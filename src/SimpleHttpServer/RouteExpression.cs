using System.Collections.Generic;
using DDT.SimpleHttpServer.Actions;
using DDT.SimpleHttpServer.Logging;
using DDT.SimpleHttpServer.Routing;

namespace DDT.SimpleHttpServer
{
    public class RouteExpression
    {
        private readonly string virtualPath;
        private readonly IList<IRoute> routes;
        private readonly ILogger logger;

        public RouteExpression(string virtualPath, IList<IRoute> routes, IServerInfo serverInfo)
        {
            this.virtualPath = virtualPath;
            this.routes = routes;

            logger = serverInfo.LoggerFactory.GetLogger("SimpleHttpServer.Routing");
        }

        public void To<TAction>() where TAction : IAction
        {
            logger.Info("Adding route from {0} to {1}", virtualPath, typeof (TAction));

            routes.Add(new Route(virtualPath, typeof(TAction)));
        }
    }
}