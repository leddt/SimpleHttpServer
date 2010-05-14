using DDT.SimpleHttpServer.Routing;

namespace DDT.SimpleHttpServer.Actions
{
    public abstract class Action : IAction
    {
        public virtual IActionResult Get(RouteContext context)
        {
            return null;
        }

        public virtual IActionResult Post(RouteContext context)
        {
            return null;
        }
    }
}