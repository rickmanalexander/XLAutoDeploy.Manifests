using System;
using System.Xml.Serialization;

namespace XLAutoDeploy.Manifests
{
    /// <summary>
    /// Information about the location on which a <see cref="Deployment"/> and its associated 
    /// <see cref="AddIn"/> is stored.
    /// </summary>
    public sealed class FileHost
    {
        public FileHost() { }

        /// <inheritdoc cref = "FileHostType"/>
        /// <remarks>
        /// Xml Node Type: Element <br/>
        /// Xml Required: Y 
        /// </remarks>  
        [XmlElement("HostType", typeof(FileHostType), IsNullable = false)]
        public FileHostType HostType { get; set; }

        /// <summary>
        /// Indicates if authentication is required to access the <see cref="FileHostType"/>.
        /// </summary>
        /// <remarks>
        /// Xml Node Type: Element <br/>
        /// Xml Required: Y 
        /// </remarks>  
        [XmlElement("RequiresAuthentication", typeof(bool), IsNullable = false)]
        public bool RequiresAuthentication { get; set; }
    }

    /// <summary>
    /// Represents the kind of host on which a <see cref="Deployment"/> and its associated 
    /// <see cref="AddIn"/> are stored.
    /// </summary>
    [Serializable]
    public enum FileHostType
    {
        [XmlEnum("fileserver")]
        FileServer,
        [XmlEnum("webserver")]
        WebServer
    }
}
