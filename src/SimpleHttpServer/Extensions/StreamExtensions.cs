using System.IO;

namespace DDT.SimpleHttpServer.Extensions
{
    internal static class StreamExtensions
    {
        public static void CopyTo(this Stream inputStream, Stream outputStream, int bufferSize)
        {
            var buffer = new byte[bufferSize];
            int read;

            while ((read = inputStream.Read(buffer, 0, buffer.Length)) > 0)
                outputStream.Write(buffer, 0, read);
        }
    }
}