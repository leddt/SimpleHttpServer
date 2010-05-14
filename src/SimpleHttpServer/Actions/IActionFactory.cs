using System;

namespace DDT.SimpleHttpServer.Actions
{
    public interface IActionFactory
    {
        IAction Create(Type type);
    }
}