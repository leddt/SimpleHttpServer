using System;
using System.IO;

namespace DDT.SimpleHttpServer.Storage
{
    public class DirectoryFileStorage : IFileStorage
    {
        private readonly DirectoryInfo baseDir;

        public DirectoryFileStorage(string baseDirectory)
        {
            baseDir = new DirectoryInfo(baseDirectory);

            if (!baseDir.Exists)
                throw new DirectoryNotFoundException(baseDirectory);
        }

        public Stream GetFile(string fileName)
        {
            if (!FileExists(fileName))
                throw new FileNotFoundException(fileName);

            return File.OpenRead(Path.Combine(baseDir.FullName, fileName));
        }

        public bool FileExists(string fileName)
        {
            var info = new FileInfo(Path.Combine(baseDir.FullName, fileName));

            return info.Exists && !String.IsNullOrEmpty(info.DirectoryName) && info.DirectoryName.StartsWith(baseDir.FullName);
        }
    }
}
