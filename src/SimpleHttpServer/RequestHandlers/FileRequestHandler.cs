using System.IO;
using System.Net;
using DDT.SimpleHttpServer.Extensions;
using DDT.SimpleHttpServer.Logging;

namespace DDT.SimpleHttpServer.RequestHandlers
{
    public class FileRequestHandler : IRequestHandler
    {
        private ILogger logger;

        public bool HandleRequest(IHttpContext context)
        {
            EnsureLogger(context.ServerInfo.LoggerFactory);

            var requestPath = context.Request.Url.AbsolutePath;
            var filePath = requestPath.TrimStart('/').Replace('/', '\\');

            if (!context.ServerInfo.FileStorage.FileExists(filePath))
            {
                logger.Debug("No file found for request {0} (filename: {1})", requestPath, filePath);
                return false;
            }

            logger.Debug("Found file for request {0} (filename: {1})", requestPath, filePath);
            ServeFile(filePath, context.ServerInfo.FileStorage.GetFile(filePath), context);
            return true;
        }

        private void EnsureLogger(ILoggerFactory loggerFactory)
        {
            if (logger == null)
                logger = loggerFactory.GetLogger("SimpleHttpServer.Handlers.FileRequestHandler");
        }

        private void ServeFile(string file, Stream stream, IHttpContext context)
        {
            context.Response.StatusCode = (int)HttpStatusCode.OK;
            context.Response.ContentType = context.ServerInfo.MimeTypeProvider.GetMimeType(file) ?? "text/html";

            logger.Info("Serving file {0} [{1}]", file, context.Response.ContentType);

            stream.CopyTo(context.Response.OutputStream, 64*1024);
        }
    }
}