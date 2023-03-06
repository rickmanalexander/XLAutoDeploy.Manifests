using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
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

        public static T DeserializeFromXml<T>(WebClient webClient, string url)
        {
            using (var stream = webClient.OpenRead(url))
            {
                return DeserializeFromXml<T>(stream);
            }
        }

        public static async Task<T> DeserializeFromXmlAsync<T>(WebClient webClient, string url)
        {
            using (var stream = await webClient.OpenReadTaskAsync(url))
            {
                return DeserializeFromXml<T>(stream);
            }
        }
        public static T DeserializeFromXml<T>(WebClient webClient, Uri uri)
        {
            using (var stream = webClient.OpenRead(uri))
            {
                return DeserializeFromXml<T>(stream);
            }
        }

        public static async Task<T> DeserializeFromXmlAsync<T>(WebClient webClient, Uri uri)
        {
            using (var stream = await webClient.OpenReadTaskAsync(uri))
            {
                return DeserializeFromXml<T>(stream);
            }
        }

        public static T DeserializeFromXml<T>(string filePath)
        {
            using (var stream = File.OpenRead(filePath))
            {
                return DeserializeFromXml<T>(stream); 
            }
        }

        public static T DeserializeFromXml<T>(Stream stream)
        {
            //ConformanceLevel.Fragment not require T to have a root namespace
            var settings = new XmlReaderSettings();
            settings.ValidationType = ValidationType.Schema;
            settings.ValidationFlags |= XmlSchemaValidationFlags.ProcessInlineSchema;
            settings.ValidationFlags |= XmlSchemaValidationFlags.ProcessSchemaLocation;
            settings.ValidationFlags |= XmlSchemaValidationFlags.ReportValidationWarnings;

            var serializer = new XmlSerializer(typeof(T));

            using (XmlReader reader = XmlReader.Create(stream, settings))
            {
                return (T)serializer.Deserialize(reader);
            }
        }
    }
}
