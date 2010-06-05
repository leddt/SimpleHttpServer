using System;
using System.IO;
using System.Linq;
using Microsoft.Win32;

namespace DDT.SimpleHttpServer
{
    public interface IMimeTypeProvider
    {
        string GetMimeType(string fileName);
    }

    public class DefaultMimeTypeProvider : IMimeTypeProvider
    {
        private readonly IMimeTypeProvider basic = new BasicMimeTypeProvider();
        private readonly IMimeTypeProvider registry = new RegistryMimeTypeProvider();

        public string GetMimeType(string fileName)
        {
            return basic.GetMimeType(fileName) ?? 
                   registry.GetMimeType(fileName) ?? 
                   "text/html";
        }
    }

    public class BasicMimeTypeProvider : IMimeTypeProvider
    {
        public string GetMimeType(string fileName)
        {
            var extension = Path.GetExtension(fileName).ToLower();
            
            switch (extension)
            {
                case ".html":
                case ".htm":
                    return "text/html";
                case ".css":
                    return "text/css";
                case ".js":
                    return "application/javascript";
                case ".gif":
                    return "image/gif";
                case ".jpeg":
                case ".jpg":
                    return "image/jpeg";
                case ".png":
                    return "image/png";
                default:
                    return null;
            }
        }
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

            return (from keyName in Registry.ClassesRoot.GetSubKeyNames()
                    where keyName.StartsWith(".")
                    where extension.CompareTo(keyName) == 0
                    select Registry.ClassesRoot.OpenSubKey(keyName).GetValue("Content Type") into val 
                    where val != null 
                    select val.ToString()).FirstOrDefault();
        }
    }
}