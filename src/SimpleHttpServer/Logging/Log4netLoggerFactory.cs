namespace DDT.SimpleHttpServer.Logging
{
    public class Log4netLoggerFactory : ILoggerFactory
    {
        public ILogger GetLogger(string name)
        {
            return new Log4netLogger(name);
        }
    }
}