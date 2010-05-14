using System.IO;

namespace DDT.SimpleHttpServer.Storage
{
    public interface IFileStorage
    {
        Stream GetFile(string fileName);
        bool FileExists(string fileName);
    }
}