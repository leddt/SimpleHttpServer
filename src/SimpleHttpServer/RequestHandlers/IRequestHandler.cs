namespace DDT.SimpleHttpServer.RequestHandlers
{
    public interface IRequestHandler
    {
        bool HandleRequest(IHttpContext context);
    }
}