using System.IO;
using System.Linq;
using Microsoft.Win32;

namespace DDT.SimpleHttpServer
{
    public interface IMimeTypeProvider
    {
        string GetMimeType(string fileName);
    }

    public class RegistryMimeTypeProvider : IMimeTypeProvider
    {
        public string GetMimeType(string fileName)
        {
            var extension = Path.GetExtension(fileName).ToLower();
            RegistryKey contentTypeKey = Registry.ClassesRoot.OpenSubKey(@"MIME\Database\Content Type");

            foreach (string keyName in contentTypeKey.GetSubKeyNames())
                if (extension.CompareTo((string)contentTypeKey.OpenSubKey(keyName).GetValue("Extension")) == 0)
                    return keyName;

            foreach (string keyName in Registry.ClassesRoot.GetSubKeyNames().Where(x => x.StartsWith(".")))
                if (extension.CompareTo(keyName) == 0)
                    return Registry.ClassesRoot.OpenSubKey(keyName).GetValue("Content Type").ToString();

            return null;
        }
    }
}