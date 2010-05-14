namespace DDT.SimpleHttpServer.Logging
{
    public interface ILoggerFactory
    {
        ILogger GetLogger(string name);
    }
}