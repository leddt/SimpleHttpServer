using System.IO;
using System.Net;

namespace DDT.SimpleHttpServer
{
    public class WrappedHttpListenerResponse : IHttpResponse
    {
        private readonly HttpListenerResponse inner;

        public WrappedHttpListenerResponse(HttpListenerResponse inner)
        {
            this.inner = inner;
        }

        public long ContentLength
        {
            get { return inner.ContentLength64; }
            set { inner.ContentLength64 = value; }
        }

        public string ContentType
        {
            get { return inner.ContentType; }
            set { inner.ContentType = value; }
        }

        public int StatusCode
        {
            get { return inner.StatusCode; }
            set { inner.StatusCode = value; }
        }

        public Stream OutputStream
        {
            get { return inner.OutputStream; }
        }

        public void AddHeader(string name, string value)
        {
            inner.AddHeader(name, value);
        }

        public void Redirect(string url)
        {
            inner.Redirect(url);
        }

        public void SetCookie(Cookie cookie)
        {
            inner.SetCookie(cookie);
        }


        public void Dispose()
        {
            inner.Close();
        }
    }
}