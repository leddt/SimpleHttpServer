using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using DDT.SimpleHttpServer.Actions;
using DDT.SimpleHttpServer.Logging;
using DDT.SimpleHttpServer.RequestHandlers;
using DDT.SimpleHttpServer.Routing;
using DDT.SimpleHttpServer.Storage;

namespace DDT.SimpleHttpServer
{
    public class Server : IServerInfo, IDisposable
    {
        private readonly IList<IRoute> routes;
        private readonly IList<IRequestHandler> handlers;
        private readonly ushort port;
        private HttpListener listener;
        private ILogger logger;

        public string DefaultPath { get; set; }
        public IMimeTypeProvider MimeTypeProvider { get; set; }
        public IActionFactory ActionFactory { get; set; }
        public IFileStorage FileStorage { get; set; }
        public IEnumerable<IRoute> Routes { get { return routes; } }

        private ILoggerFactory loggerFactory;
        public ILoggerFactory LoggerFactory
        {
            get { return loggerFactory; }
            set 
            { 
                if (listener != null)
                {
                    logger.Fatal("Do not change the logger factory after starting the server.");
                    throw new InvalidOperationException("Do not change the logger factory after starting the server.");
                }

                loggerFactory = value;
            }
        }


        public Server(IFileStorage fileStorage, ushort port)
        {
            FileStorage = fileStorage;
            this.port = port;
            routes = new List<IRoute>();
            handlers = new List<IRequestHandler>
                       {
                           new RouteRequestHandler(),
                           new FileRequestHandler(),
                           new FileNotFoundRequestHandler()
                       };

            DefaultPath = "index.htm";
            MimeTypeProvider = new DefaultMimeTypeProvider();
            LoggerFactory = new Log4netLoggerFactory();
            ActionFactory = new ActionFactory();
        }

        public void Start()
        {
            logger = LoggerFactory.GetLogger("SimpleHttpServer.Server");

            logger.Info("Server starting on port {0}", port);

            if (listener != null)
            {
                logger.Fatal("Server already started");
                throw new InvalidOperationException("Already started");
            }

            listener = new HttpListener();
            listener.Prefixes.Add(string.Format("http://*:{0}/", port));

            try
            {
                listener.Start();
            }
            catch(Exception ex)
            {
                logger.Fatal("Error starting server", ex);
                throw;
            }

            logger.Info("Server started");

            logger.Debug("Waiting for first request");
            listener.BeginGetContext(ProcessRequest, null);
        }

        public void Stop()
        {
            logger.Info("Server stopping");

            if (listener == null)
            {
                logger.Fatal("Server already stopped");
                throw new InvalidOperationException("Already stopped");
            }

            listener.Stop();
            listener.Close();
            listener = null;

            logger.Info("Server stopped");
        }

        public RouteExpression Route(string virtualPath)
        {
            return new RouteExpression(virtualPath, routes, this);
        }

        private void ProcessRequest(IAsyncResult ar)
        {
            HttpListenerContext listenerContext;
            try
            {
                logger.Debug("Retrieving request context");
                listenerContext = listener.EndGetContext(ar);
            }
            catch (Exception ex)
            {
                logger.Error("Bad request, could not retrieve context.", ex);

                logger.Debug("Waiting for next request");
                listener.BeginGetContext(ProcessRequest, null);

                return;
            }

            var httpContext = new WrappedHttpListenerContext(this, listenerContext);
            
            try
            {
                logger.Info("Incoming request: {0} - {1}", httpContext.Request.HttpMethod, httpContext.Request.Url);

                if (httpContext.Request.Url.AbsolutePath.TrimStart('/') == "")
                {
                    logger.Info("Replacing url with default path: {0}", httpContext.ServerInfo.DefaultPath);
                    httpContext.Request.ReplaceUrl(httpContext.ServerInfo.DefaultPath);
                }

                var handler = handlers.FirstOrDefault(x => x.HandleRequest(httpContext));

                if (handler == null)
                {
                    // TODO
                }
                else
                {
                    logger.Debug("Request handled by {0}", handler.ToString());
                }
            }
            catch(Exception ex)
            {
                HandleException(ex, httpContext);
            }
            finally
            {
                httpContext.Response.Dispose();

                logger.Debug("Waiting for next request");
                listener.BeginGetContext(ProcessRequest, null);
            }
        }


        private void HandleException(Exception exception, IHttpContext context)
        {
            logger.Error("Unhandled exception in request {0}", exception, context.Request.Url);

            
            // TODO (500)
        }

        public void Dispose()
        {
            if (listener == null) return;

            Stop();
        }
    }
}