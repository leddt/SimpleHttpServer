using System.Net;
using DDT.SimpleHttpServer.Logging;

namespace DDT.SimpleHttpServer.RequestHandlers
{
    public class FileNotFoundRequestHandler : IRequestHandler
    {
        private ILogger logger;

        public bool HandleRequest(IHttpContext context)
        {
            EnsureLogger(context.ServerInfo.LoggerFactory);

            logger.Info("Returning 404");

            var requestPath = context.Request.Url.AbsolutePath;

            context.Response.StatusCode = (int)HttpStatusCode.NotFound;

            var data = System.Text.Encoding.UTF8.GetBytes("Not found: " + requestPath);
            context.Response.ContentLength = data.Length;
            context.Response.OutputStream.Write(data, 0, data.Length);

            return true;
        }

        private void EnsureLogger(ILoggerFactory loggerFactory)
        {
            if (logger == null)
                logger = loggerFactory.GetLogger("SimpleHttpServer.Handlers.FileRequestHandler");
        }
    }
}