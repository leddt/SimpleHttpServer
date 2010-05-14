using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;
using System.Web;

namespace DDT.SimpleHttpServer
{
    public class WrappedHttpListenerRequest : IHttpRequest
    {
        private readonly HttpListenerRequest inner;
        private Uri overridenUrl;

        public Encoding ContentEncoding
        {
            get { return inner.ContentEncoding; }
        }

        public long ContentLength
        {
            get { return inner.ContentLength64; }
        }

        public string ContentType
        {
            get { return inner.ContentType; }
        }

        public CookieCollection Cookies
        {
            get { return inner.Cookies; }
        }

        public NameValueCollection Headers
        {
            get { return inner.Headers; }
        }

        public string HttpMethod
        {
            get { return inner.HttpMethod; }
        }

        public NameValueCollection PostData { get; private set; }

        public NameValueCollection QueryString 
        { 
            get
            {
                return inner.QueryString;
            } 
        }
        
        public Uri Url
        {
            get { return overridenUrl ?? inner.Url; }
        }

        public Uri UrlReferrer
        {
            get { return inner.UrlReferrer; }
        }

        public string UserAgent
        {
            get { return inner.UserAgent; }
        }

        public string[] UserLanguages
        {
            get { return inner.UserLanguages; }
        }

        public WrappedHttpListenerRequest(HttpListenerRequest inner)
        {
            this.inner = inner;
            PostData = ReadPostData(inner);
        }

        public void ReplaceUrl(string path)
        {
            if (!path.StartsWith("/"))
                path = "/" + path;

            overridenUrl = new Uri(Url, path);
        }

        private static NameValueCollection ReadPostData(HttpListenerRequest request)
        {
            var body = request.InputStream;
            var encoding = request.ContentEncoding;
            var reader = new StreamReader(body, encoding);
            return HttpUtility.ParseQueryString(reader.ReadToEnd(), encoding);
        }
    }
}