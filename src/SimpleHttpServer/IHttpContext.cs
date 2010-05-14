
namespace DDT.SimpleHttpServer
{
    public interface IHttpContext
    {
        IHttpRequest Request { get; }
        IHttpResponse Response { get; }
        ISession Session { get; }
        string SessionId { get; }
        IServerInfo ServerInfo { get; }
    }
}