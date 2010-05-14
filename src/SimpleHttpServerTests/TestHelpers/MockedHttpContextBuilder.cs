using System;
using DDT.SimpleHttpServer;
using DDT.SimpleHttpServer.Actions;
using DDT.SimpleHttpServer.Logging;
using Rhino.Mocks;

namespace DDT.SimpleHttpServerTests.TestHelpers
{
    public class MockedHttpContextBuilder : MockBuilder<IHttpContext>
    {
        public MockedHttpContextBuilder()
        {
            Mock.Stub(x => x.ServerInfo).Return(MockRepository.GenerateMock<IServerInfo>());
            Mock.ServerInfo.Stub(x => x.LoggerFactory).Return(MockRepository.GenerateMock<ILoggerFactory>());
            Mock.ServerInfo.LoggerFactory.Stub(x => x.GetLogger(Arg<string>.Is.Anything)).Return(MockRepository.GenerateMock<ILogger>());
            Mock.ServerInfo.Stub(x => x.ActionFactory).Return(MockRepository.GenerateMock<IActionFactory>());
            Mock.ServerInfo.Stub(x => x.MimeTypeProvider).Return(MockRepository.GenerateMock<IMimeTypeProvider>());

            Mock.Stub(x => x.Request).Return(MockRepository.GenerateMock<IHttpRequest>());
            Mock.Stub(x => x.Response).Return(MockRepository.GenerateMock<IHttpResponse>());
        }

        public MockedHttpContextBuilder WithRegisteredAction<TAction>(TAction action) where TAction : IAction
        {
            Mock.ServerInfo.ActionFactory.Stub(x => x.Create(typeof (TAction))).Return(action);
            
            return this;
        }

        public MockedHttpContextBuilder WithUrl(string url)
        {
            Mock.Request.Stub(x => x.Url).Return(new Uri(url));

            return this;
        }

        public MockedHttpContextBuilder WithHttpMethod(string httpMethod)
        {
            Mock.Request.Stub(x => x.HttpMethod).Return(httpMethod);

            return this;
        }
    }
}