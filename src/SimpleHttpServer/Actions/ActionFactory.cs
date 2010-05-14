using System;

namespace DDT.SimpleHttpServer.Actions
{
    public class ActionFactory : IActionFactory
    {
        public IAction Create(Type type)
        {
            if (!typeof(IAction).IsAssignableFrom(type))
                throw new ArgumentException("Must implement IAction", "type");
            return (IAction) Activator.CreateInstance(type);
        }
    }
}