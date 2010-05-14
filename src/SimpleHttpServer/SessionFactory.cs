using System;
using System.Web;
using System.Web.Caching;

namespace DDT.SimpleHttpServer
{
    internal static class SessionFactory
    {
        private static readonly TimeSpan SESSION_TIMEOUT = TimeSpan.FromMinutes(20);
        private static readonly Cache sessionCache = HttpRuntime.Cache;

        public static ISession CreateSession(string sessionId)
        {
            var sessionKey = CreateSessionKey(sessionId);
            var session = sessionCache.Get(sessionKey) as ISession;
            
            if (session == null)
            {
                session = new Session();
                sessionCache.Add(sessionKey, session, null, Cache.NoAbsoluteExpiration, SESSION_TIMEOUT, CacheItemPriority.NotRemovable, null);
            }

            return session;
        }

        private static string CreateSessionKey(string sessionId)
        {
            return "__session_id_" + sessionId;
        }
    }
}