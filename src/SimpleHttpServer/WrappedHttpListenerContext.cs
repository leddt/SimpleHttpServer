using System;
using System.Net;

namespace DDT.SimpleHttpServer
{
    public class WrappedHttpListenerContext : IHttpContext
    {
        private const string SESSION_COOKIE = "__ddt_session_id";

        public IServerInfo ServerInfo { get; set; }
        public IHttpRequest Request { get; private set; }
        public IHttpResponse Response { get; private set; }
        public ISession Session { get; private set; }

        public WrappedHttpListenerContext(IServerInfo serverInfo, HttpListenerContext listenerContext)
        {
            ServerInfo = serverInfo;
            Request = new WrappedHttpListenerRequest(listenerContext.Request);
            Response = new WrappedHttpListenerResponse(listenerContext.Response);

            InitializeSession();
        }

        public string SessionId
        {
            get
            {
                var sessionCookie = Request.Cookies[SESSION_COOKIE];

                if (sessionCookie == null)
                {
                    sessionCookie = new Cookie(SESSION_COOKIE, Guid.NewGuid().ToString()) { HttpOnly = true };

                    Request.Cookies.Add(sessionCookie);
                    Response.SetCookie(sessionCookie);
                }

                return sessionCookie.Value;
            }
        }

        private void InitializeSession()
        {
            Session = SessionFactory.CreateSession(SessionId);
        }
    }
}
