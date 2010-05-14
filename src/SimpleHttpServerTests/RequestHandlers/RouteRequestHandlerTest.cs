using System.Collections.Generic;
using DDT.SimpleHttpServer;
using DDT.SimpleHttpServer.RequestHandlers;
using DDT.SimpleHttpServer.Routing;
using DDT.SimpleHttpServerTests.TestHelpers;
using Rhino.Mocks;
using Xunit;

namespace DDT.SimpleHttpServerTests.RequestHandlers
{
    public class RouteRequestHandlerTest
    {
        [Fact]
        public void HandleRequest_SendsRequestToFirstMatchingRoute()
        {
            var routes = new List<IRoute>
                         {
                             MockRepository.GenerateStub<IRoute>(),
                             MockRepository.GenerateStub<IRoute>(),
                             MockRepository.GenerateStub<IRoute>()
                         };

            routes[0].Stub(x => x.IsMatch("/test.html")).Return(false);
            routes[1].Stub(x => x.IsMatch("/test.html")).Return(true);
            routes[2].Stub(x => x.IsMatch("/test.html")).Return(true);

            var context = Mocked.HttpContext()
                .WithUrl("http://localhost/test.html")
                .Mock;

            context.Stub(x => x.ServerInfo).Return(MockRepository.GenerateStub<IServerInfo>());
            context.ServerInfo.Stub(x => x.Routes).Return(routes);

            var handler = new RouteRequestHandler();
            handler.HandleRequest(context);

            routes[1].AssertWasCalled(x => x.HandleRequest(context));
        }

        [Fact]
        public void HandleRequest_WithNoMatchingRoute_ReturnsFalse()
        {
            var routes = new List<IRoute> {MockRepository.GenerateStub<IRoute>()};
            
            var context = Mocked.HttpContext()
                .WithUrl("http://localhost/test.html")
                .Mock;

            context.Stub(x => x.ServerInfo).Return(MockRepository.GenerateStub<IServerInfo>());
            context.ServerInfo.Stub(x => x.Routes).Return(routes);
            
            var handler = new RouteRequestHandler();
            var result = handler.HandleRequest(context);

            Assert.False(result);
        }

        [Fact]
        public void HandleRequest_WithMatchingRouteHandlesTheRequest_ReturnsTrue()
        {
            var context = Mocked.HttpContext()
                .WithUrl("http://localhost/test.html")
                .Mock;

            var routes = new List<IRoute> { MockRepository.GenerateStub<IRoute>() };
            routes[0].Stub(x => x.IsMatch("/test.html")).Return(true);
            routes[0].Stub(x => x.HandleRequest(context)).Return(true);

            context.Stub(x => x.ServerInfo).Return(MockRepository.GenerateStub<IServerInfo>());
            context.ServerInfo.Stub(x => x.Routes).Return(routes);

            var handler = new RouteRequestHandler();
            var result = handler.HandleRequest(context);

            Assert.True(result);
        }
    }
}