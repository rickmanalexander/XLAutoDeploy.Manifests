using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace XLAutoDeploy.Manifests.Utilities
{
    public static class XmlConversion
    {
        public static void SerializeToXmlFile<T>(T obj, string filePath, bool overwriteExisting = true)
        {
            //overwriteExisting == true ===> append == false
            using (var writer = new StreamWriter(filePath, !overwriteExisting))
            {
                var serializer = new XmlSerializer(typeof(T));

                serializer.Serialize(writer, obj);
            }
        }

        public static T DeserializeFromXml<T>(WebClient webClient, string url, System.Xml.ConformanceLevel readerConformanceLevel = ConformanceLevel.Document)
        {
            using (var stream = webClient.OpenRead(url))
            {
                return DeserializeFromXml<T>(stream, readerConformanceLevel);
            }
        }

        public static async Task<T> DeserializeFromXmlAsync<T>(WebClient webClient, string url, System.Xml.ConformanceLevel readerConformanceLevel = ConformanceLevel.Document)
        {
            using (var stream = await webClient.OpenReadTaskAsync(url))
            {
                return DeserializeFromXml<T>(stream, readerConformanceLevel);
            }
        }
        public static T DeserializeFromXml<T>(WebClient webClient, Uri uri, System.Xml.ConformanceLevel readerConformanceLevel = ConformanceLevel.Document)
        {
            using (var stream = webClient.OpenRead(uri))
            {
                return DeserializeFromXml<T>(stream, readerConformanceLevel);
            }
        }

        public static async Task<T> DeserializeFromXmlAsync<T>(WebClient webClient, Uri uri, System.Xml.ConformanceLevel readerConformanceLevel = ConformanceLevel.Document)
        {
            using (var stream = await webClient.OpenReadTaskAsync(uri))
            {
                return DeserializeFromXml<T>(stream, readerConformanceLevel);
            }
        }

        public static T DeserializeFromXml<T>(string filePath, System.Xml.ConformanceLevel readerConformanceLevel = ConformanceLevel.Document)
        {
            using (var stream = File.OpenRead(filePath))
            {
                return DeserializeFromXml<T>(stream, readerConformanceLevel); 
            }
        }

        public static T DeserializeFromXml<T>(Stream stream, System.Xml.ConformanceLevel readerConformanceLevel = ConformanceLevel.Document)
        {
            //ConformanceLevel.Fragment not require T to have a root namespace
            var settings = new XmlReaderSettings() { ConformanceLevel = readerConformanceLevel };

            var serializer = new XmlSerializer(typeof(T));

            using (XmlReader reader = XmlReader.Create(stream, settings))
            {
                return (T)serializer.Deserialize(reader);
            }
        }
    }
}
