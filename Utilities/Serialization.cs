using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace XLAutoDeploy.Manifests.Utilities
{
    public static class Serialization
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

        // Opening a single read/write stream appends to the existing data, effectivly duplicating it, so we open 2 separate ones
        public static void AddSchemaLocationToXmlFile(string filePath, Uri schemaLocation)
        {
            XmlDocument doc = new XmlDocument();
            using (var readStream = File.Open(filePath, FileMode.Open, FileAccess.Read))
            {
                doc.Load(readStream);
            }

            var rootNode = doc.DocumentElement;
            rootNode.SetAttribute("schemaLocation", "http://www.w3.org/2001/XMLSchema-instance", rootNode.NamespaceURI + " " + schemaLocation.AbsoluteUri);

            using (var writeStream = File.Open(filePath, FileMode.Open, FileAccess.Write))
            {
                doc.Save(writeStream);
            }
        }

        public static string GetSchemaLocationFromXmlFile(string filePath)
        {
            using (var stream = File.OpenRead(filePath))
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(stream);

                var rootNode = doc.DocumentElement;

                var schemaLocationAttribute = rootNode.SelectSingleNode("//@*[local-name()='schemaLocation']");

                return schemaLocationAttribute?.Value;
            }
        }

        public static T DeserializeFromXml<T>(WebClient webClient, string url, bool validateAgainstSchema = true)
        {
            using (var stream = webClient.OpenRead(url))
            {
                return DeserializeFromXml<T>(stream, validateAgainstSchema);
            }
        }

        public static async Task<T> DeserializeFromXmlAsync<T>(WebClient webClient, string url, bool validateAgainstSchema = true)
        {
            using (var stream = await webClient.OpenReadTaskAsync(url))
            {
                return DeserializeFromXml<T>(stream, validateAgainstSchema);
            }
        }

        public static T DeserializeFromXml<T>(WebClient webClient, Uri uri, bool validateAgainstSchema = true)
        {
            using (var stream = webClient.OpenRead(uri))
            {
                return DeserializeFromXml<T>(stream, validateAgainstSchema);
            }
        }

        public static async Task<T> DeserializeFromXmlAsync<T>(WebClient webClient, Uri uri, bool validateAgainstSchema = true)
        {
            using (var stream = await webClient.OpenReadTaskAsync(uri))
            {
                return DeserializeFromXml<T>(stream, validateAgainstSchema);
            }
        }

        public static T DeserializeFromXml<T>(string filePath, bool validateAgainstSchema = true)
        {
            using (var stream = File.OpenRead(filePath))
            {
                return DeserializeFromXml<T>(stream, validateAgainstSchema); 
            }
        }

        public static T DeserializeFromXml<T>(Stream stream, bool validateAgainstSchema = true)
        {
            //ConformanceLevel.Fragment not require T to have a root namespace
            var settings = new XmlReaderSettings();
            if (validateAgainstSchema)
            {
                settings.ValidationType = ValidationType.Schema;
                settings.ValidationFlags |= XmlSchemaValidationFlags.ProcessInlineSchema;
                settings.ValidationFlags |= XmlSchemaValidationFlags.ProcessSchemaLocation;
                settings.ValidationFlags |= XmlSchemaValidationFlags.ReportValidationWarnings;
            }

            var serializer = new XmlSerializer(typeof(T));

            using (XmlReader reader = XmlReader.Create(stream, settings))
            {
                return (T)serializer.Deserialize(reader);
            }
        }
    }
}
