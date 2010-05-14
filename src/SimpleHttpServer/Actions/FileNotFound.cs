using System;
using System.Net;
using DDT.SimpleHttpServer.Routing;

namespace DDT.SimpleHttpServer.Actions
{
    public class FileNotFound : IAction
    {
        public IActionResult Get(RouteContext context)
        {
            return new FileNotFoundResult();
        }

        public IActionResult Post(RouteContext context)
        {
            return new FileNotFoundResult();
        }

        public class FileNotFoundResult : IActionResult
        {
            public void Execute(IHttpContext context)
            {
                var requestPath = context.Request.Url.AbsolutePath;

                context.Response.StatusCode = (int)HttpStatusCode.NotFound;

                var data = System.Text.Encoding.UTF8.GetBytes("Not found: " + requestPath);
                context.Response.ContentLength = data.Length;
                context.Response.OutputStream.Write(data, 0, data.Length);
            }
        }
    }
}
