using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace DDT.SimpleHttpServer.Storage
{
    public class EmbeddedFileStorage : IFileStorage
    {
        private readonly Assembly assembly;
        private readonly string baseNamespace;
        private Dictionary<string, string> nameMap;

        public EmbeddedFileStorage(Assembly assembly, string baseNamespace)
        {
            this.assembly = assembly;
            this.baseNamespace = baseNamespace;
            CreateNameMap();
        }

        public Stream GetFile(string fileName)
        {
            if (!FileExists(fileName))
                throw new FileNotFoundException(fileName);

            return assembly.GetManifestResourceStream(GetResourceName(fileName));
        }

        public bool FileExists(string fileName)
        {
            var info = assembly.GetManifestResourceInfo(GetResourceName(fileName));
            return info != null;
        }

        private string GetResourceName(string fileName)
        {
            var name = baseNamespace + "." + fileName.Replace('\\', '.').Replace('/', '.');
            return nameMap.ContainsKey(name.ToLowerInvariant())
                       ? nameMap[name.ToLowerInvariant()]
                       : name;
        }

        private void CreateNameMap()
        {
            var allResources = assembly.GetManifestResourceNames().Where(x => x.StartsWith(baseNamespace));

            nameMap = new Dictionary<string, string>();

            foreach (var resource in allResources)
                nameMap.Add(resource.ToLowerInvariant(), resource);
        }
    }
}