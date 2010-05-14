using DDT.SimpleHttpServer;
using DDT.SimpleHttpServer.Actions;
using DDT.SimpleHttpServer.Routing;
using DDT.SimpleHttpServerTests.TestHelpers;
using Rhino.Mocks;
using Xunit;

namespace DDT.SimpleHttpServerTests.Routing
{
    public class RouteTest
    {
        public class TestAction : IAction
        {
            public string LastMethod { get; private set; }
            public RouteContext LastContext { get; private set; }

            public IActionResult GetResult { get; set; }
            public IActionResult PostResult { get; set; }

            public IActionResult Get(RouteContext context)
            {
                LastMethod = "Get";
                LastContext = context;

                return GetResult;
            }

            public IActionResult Post(RouteContext context)
            {
                LastMethod = "Post";
                LastContext = context;

                return PostResult;
            }
        }

        [Fact]
        public void IsMatch_WithMatchingVirtualPath_ReturnsTrue()
        {
            var route = new Route("tests/([a-z]+).html", typeof(TestAction));

            var result = route.IsMatch("tests/success.html");

            Assert.True(result);
        }

        [Fact]
        public void IsMatch_WithMismatchedVirtualPath_ReturnsFalse()
        {
            var route = new Route("tests/([a-z]+).html", typeof(TestAction));

            var result = route.IsMatch("tests/fa!l.html");

            Assert.False(result);
        }

        [Fact]
        public void HandleRequest_ForGetRequest_CallsTheGetMethodOnTheAction()
        {
            var route = new Route("tests/([a-z]+).html", typeof(TestAction));
            var action = new TestAction();

            var httpContext = Mocked.HttpContext()
                                    .WithRegisteredAction(action)
                                    .WithHttpMethod("GET")
                                    .WithUrl("http://localhost/tests/request.html")
                                    .Mock;

            route.HandleRequest(httpContext);

            Assert.Equal("Get", action.LastMethod);
        }

        [Fact]
        public void HandleRequest_ForPostRequest_CallsThePostMethodOnTheAction()
        {
            var route = new Route("tests/([a-z]+).html", typeof(TestAction));
            var action = new TestAction();

            var httpContext = Mocked.HttpContext()
                                    .WithRegisteredAction(action)
                                    .WithHttpMethod("POST")
                                    .WithUrl("http://localhost/tests/request.html")
                                    .Mock;

            route.HandleRequest(httpContext);

            Assert.Equal("Post", action.LastMethod);
        }

        [Fact]
        public void HandleRequest_ForUnsupportedRequest_DoesNotCallTheAction()
        {
            var route = new Route("tests/([a-z]+).html", typeof(TestAction));
            var action = new TestAction();

            var httpContext = Mocked.HttpContext()
                                    .WithRegisteredAction(action)
                                    .WithHttpMethod("PUT")
                                    .WithUrl("http://localhost/tests/request.html")
                                    .Mock;

            route.HandleRequest(httpContext);

            Assert.Null(action.LastMethod);
        }

        [Fact]
        public void HandleRequest_WhenRequestWasHandled_ReturnsTrue()
        {
            var route = new Route("tests/([a-z]+).html", typeof(TestAction));
            var action = new TestAction
                         {
                             GetResult = MockRepository.GenerateStub<IActionResult>()
                         };

            var httpContext = Mocked.HttpContext()
                                    .WithRegisteredAction(action)
                                    .WithHttpMethod("GET")
                                    .WithUrl("http://localhost/tests/request.html")
                                    .Mock;

            var result = route.HandleRequest(httpContext);

            Assert.True(result);
        }

        [Fact]
        public void HandleRequest_WhenRequestWasNotHandled_ReturnsFalse()
        {
            var route = new Route("tests/([a-z]+).html", typeof(TestAction));
            var action = new TestAction
            {
                GetResult = null
            };

            var httpContext = Mocked.HttpContext()
                                    .WithRegisteredAction(action)
                                    .WithHttpMethod("GET")
                                    .WithUrl("http://localhost/tests/request.html")
                                    .Mock;

            var result = route.HandleRequest(httpContext);

            Assert.False(result);
        }

        [Fact]
        public void HandleRequest_WhenRequestWasHandled_ExecutesTheResult()
        {
            var route = new Route("tests/([a-z]+).html", typeof(TestAction));
            var action = new TestAction
            {
                GetResult = MockRepository.GenerateStub<IActionResult>()
            };

            var httpContext = Mocked.HttpContext()
                                    .WithRegisteredAction(action)
                                    .WithHttpMethod("GET")
                                    .WithUrl("http://localhost/tests/request.html")
                                    .Mock;

            route.HandleRequest(httpContext);

            action.GetResult.AssertWasCalled(x => x.Execute(Arg<IHttpContext>.Is.Anything));
        }

        [Fact]
        public void HandleRequest_ForGetRequest_PassesCorrectDataToAction()
        {
            var route = new Route("tests/(?<pageName>[a-z]+).html", typeof(TestAction));
            var action = new TestAction();

            var httpContext = Mocked.HttpContext()
                                    .WithRegisteredAction(action)
                                    .WithHttpMethod("GET")
                                    .WithUrl("http://localhost/tests/myRequest.html")
                                    .Mock;

            route.HandleRequest(httpContext);

            Assert.Equal("myRequest", action.LastContext.RouteData["pageName"]);
            Assert.Same(httpContext, action.LastContext.HttpContext);
        }
    }
}
