using System;
using System.Collections.Specialized;
using System.Net;
using System.Text;

namespace DDT.SimpleHttpServer
{
    public interface IHttpRequest
    {
        Encoding ContentEncoding { get; }
        long ContentLength { get; }
        string ContentType { get; }
        CookieCollection Cookies { get; }
        NameValueCollection Headers { get; }
        string HttpMethod { get; }
        NameValueCollection PostData { get; }
        NameValueCollection QueryString { get; }
        Uri Url { get; }
        Uri UrlReferrer { get; }
        string UserAgent { get; }
        string[] UserLanguages { get; }
        void ReplaceUrl(string path);
    }
}