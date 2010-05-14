using System;
using log4net;

namespace DDT.SimpleHttpServer.Logging
{
    public class Log4netLogger : ILogger
    {
        private readonly ILog log;

        public Log4netLogger(string name)
        {
            log = LogManager.GetLogger(name);
        }

        public void Debug(string message, params object[] args)
        {
            log.DebugFormat(message, args);
        }

        public void Info(string message, params object[] args)
        {
            log.InfoFormat(message, args);
        }

        public void Warn(string message, params object[] args)
        {
            log.WarnFormat(message, args);
        }

        public void Error(string message, params object[] args)
        {
            log.ErrorFormat(message, args);
        }

        public void Error(string message, Exception exception, params object[] args)
        {
            log.Error(String.Format(message, args), exception);
        }

        public void Fatal(string message, params object[] args)
        {
            log.FatalFormat(message, args);
        }

        public void Fatal(string message, Exception exception, params object[] args)
        {
            log.Error(String.Format(message, args), exception);
        }
    }
}