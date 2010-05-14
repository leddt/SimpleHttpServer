namespace DDT.SimpleHttpServer.Actions
{
    public interface IActionResult
    {
        void Execute(IHttpContext context);
    }
}