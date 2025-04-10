using NSec.Cryptography;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml;

namespace AppcastCreater
{
    public static class AppcastHelper
    {

        public static AppcastConfig? ReadAppcastConfig(string path)
        {
            if(path is (null or "") || !System.IO.File.Exists(path))
            {
                return null;
            }
            using var stream=new StreamReader(path, Encoding.UTF8);
            var buffer=stream.ReadToEnd();
            stream.Close();
            return JsonSerializer.Deserialize<AppcastConfig>(buffer);
        }

        public static void CreateAppcast(AppcastConfig config)
        {
            byte[] fileBytes = File.ReadAllBytes(config.ApplicationZipFilePath);
            byte[] privateKey = File.ReadAllBytes(config.PrivateKeyPath);
            byte[] signatureBytes = SignWithEd25519(fileBytes, privateKey);
            string signatureBase64 = Convert.ToBase64String(signatureBytes);

            long fileSize = new FileInfo(config.ApplicationZipFilePath).Length;
            string pubDate = DateTime.UtcNow.ToString("r");

            var settings = new XmlWriterSettings { Indent = true, Encoding = Encoding.UTF8 };
            using var writer = XmlWriter.Create(config.OutputXml, settings);

            writer.WriteStartDocument();
            writer.WriteStartElement("rss");
            writer.WriteAttributeString("version", "2.0");
            writer.WriteAttributeString("xmlns:sparkle", "http://www.andymatuschak.org/xml-namespaces/sparkle");

            writer.WriteStartElement("channel");
            writer.WriteElementString("title", $"{config.AppName} Updates");
            writer.WriteElementString("link", config.DownloadUrl);
            writer.WriteElementString("description", $"Appcast feed for {config.AppName}");

            writer.WriteStartElement("item");
            writer.WriteElementString("title", $"Version {config.Version}");
            writer.WriteElementString("pubDate", pubDate);

            writer.WriteStartElement("enclosure");
            writer.WriteAttributeString("url", config.DownloadUrl);
            writer.WriteAttributeString("sparkle:version", config.Version);
            writer.WriteAttributeString("length", fileSize.ToString());
            writer.WriteAttributeString("type", "application/octet-stream");
            writer.WriteAttributeString("sparkle:edSignature", signatureBase64);
            writer.WriteEndElement(); // enclosure

            writer.WriteEndElement(); // item
            writer.WriteEndElement(); // channel
            writer.WriteEndElement(); // rss
            writer.WriteEndDocument();
        }

        static byte[] SignWithEd25519(byte[] data, byte[] privateKeyBytes)
        {
            var algorithm = SignatureAlgorithm.Ed25519;
            // 秘密鍵のインポート（NetSparkle の形式に対応させる）
            var privateKey = Key.Import(algorithm, privateKeyBytes, KeyBlobFormat.RawPrivateKey);
            return algorithm.Sign(privateKey, data);
        }
    }
}
