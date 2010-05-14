using DDT.SimpleHttpServer.Routing;

namespace DDT.SimpleHttpServer.Actions
{
    public interface IAction
    {
        IActionResult Get(RouteContext context);
        IActionResult Post(RouteContext context);
    }
}