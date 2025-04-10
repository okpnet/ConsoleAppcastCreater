using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppcastCreater
{
    public class AppcastConfig
    {
        public string AppName { get; set; } = "";
        public string Version { get; set; } = "";
        public string ApplicationZipFilePath { get; set; } = "";
        public string DownloadUrl { get; set; } = "";
        public string OutputXml { get; set; } = "appcast.xml";
        public string PrivateKeyPath { get; set; } = "dragee_key";

        public AppcastConfig()
        {
        }

        public AppcastConfig(string appName, string version, string applicationZipFilePath, string downloadUrl, string outputXml, string privateKeyPath)
        {
            AppName = appName;
            Version = version;
            ApplicationZipFilePath = applicationZipFilePath;
            DownloadUrl = downloadUrl;
            OutputXml = outputXml;
            PrivateKeyPath = privateKeyPath;
        }

        private bool IsFiles()
        {
            return System.IO.File.Exists(ApplicationZipFilePath) && System.IO.File.Exists(PrivateKeyPath);
        }
    }
}
