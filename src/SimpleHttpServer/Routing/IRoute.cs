namespace DDT.SimpleHttpServer.Routing
{
    public interface IRoute
    {
        bool IsMatch(string virtualPath);
        bool HandleRequest(IHttpContext context);
    }
}