using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using DDT.SimpleHttpServer.Actions;
using DDT.SimpleHttpServer.Logging;

namespace DDT.SimpleHttpServer.Routing
{
    public class Route : IRoute
    {
        private readonly Regex pathRegex;
        private readonly string path;
        private readonly Type actionType;
        private ILogger logger;

        public Route(string virtualPath, Type actionType)
        {
            path = virtualPath;
            pathRegex = new Regex("^/?" + virtualPath + "$", RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture | RegexOptions.Compiled);
            this.actionType = actionType;
        }

        public bool IsMatch(string virtualPath)
        {
            return pathRegex.IsMatch(virtualPath);
        }

        public bool HandleRequest(IHttpContext context)
        {
            EnsureLogger(context.ServerInfo.LoggerFactory);

            var action = context.ServerInfo.ActionFactory.Create(actionType);
            var routeContext = new RouteContext(context, GetRouteData(context.Request.Url.AbsolutePath));

            IActionResult actionResult = null;
            switch (context.Request.HttpMethod.ToUpper())
            {
                case "GET":
                    logger.Debug("Dispatching GET request to {0}", action);
                    actionResult = action.Get(routeContext);
                    break;
                case "POST":
                    logger.Debug("Dispatching POST request to {0}", action);
                    actionResult = action.Post(routeContext);
                    break;
            }

            if (actionResult == null)
            {
                logger.Info("Action {0} did not handle the request", action);
                return false;
            }

            logger.Info("Request handled by action {0}, executing result...", action);
            actionResult.Execute(context);
            return true;
        }

        private void EnsureLogger(ILoggerFactory loggerFactory)
        {
            if (logger == null)
                logger = loggerFactory.GetLogger("SimpleHttpServer.Routing");
        }

        private IDictionary<string, string> GetRouteData(string virtualPath)
        {
            var names = pathRegex.GetGroupNames();
            var match = pathRegex.Match(virtualPath);
            var results = new Dictionary<string, string>();
            
            foreach (var name in names)
            {
                var g = match.Groups[name];
                if (g != null)
                    results.Add(name, g.Value);
            }

            return results;
        }

        public override string ToString()
        {
            return path;
        }
    }
}