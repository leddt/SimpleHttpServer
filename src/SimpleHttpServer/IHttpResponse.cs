using System;
using System.IO;
using System.Net;

namespace DDT.SimpleHttpServer
{
    public interface IHttpResponse : IDisposable
    {
        long ContentLength { get; set; }
        string ContentType { get; set; }
        int StatusCode { get; set; }
        Stream OutputStream { get; }
        void AddHeader(string name, string value);
        void Redirect(string url);
        void SetCookie(Cookie cookie);
    }
}