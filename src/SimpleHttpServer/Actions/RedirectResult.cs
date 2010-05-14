namespace DDT.SimpleHttpServer.Actions
{
    public class RedirectResult : IActionResult
    {
        private readonly string url;

        public RedirectResult(string url)
        {
            this.url = url;
        }

        public void Execute(IHttpContext context)
        {
            context.Response.Redirect(url);
        }
    }
}