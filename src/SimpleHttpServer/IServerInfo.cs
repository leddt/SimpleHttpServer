using System.Collections.Generic;
using DDT.SimpleHttpServer.Actions;
using DDT.SimpleHttpServer.Logging;
using DDT.SimpleHttpServer.Routing;
using DDT.SimpleHttpServer.Storage;

namespace DDT.SimpleHttpServer
{
    public interface IServerInfo
    {
        string DefaultPath { get; set; }
        IMimeTypeProvider MimeTypeProvider { get; set; }
        ILoggerFactory LoggerFactory { get; set; }
        IActionFactory ActionFactory { get; set; }
        IFileStorage FileStorage { get; set; }
        IEnumerable<IRoute> Routes { get; }
    }
}