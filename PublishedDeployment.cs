using XLAutoDeploy.Manifests.Utilities;

using System;
using System.ComponentModel;
using System.Xml;
using System.Xml.Serialization;

namespace XLAutoDeploy.Manifests
{
    /// <summary>
    /// Represents the location of a <see cref="Deployment"/> manifest.
    /// </summary>  
    [Serializable]
    public sealed class PublishedDeployment
    {
        /// <summary>
        /// Specifies the Uri of a <see cref="Deployment"/> manifest.
        /// </summary>  
        /// <remarks>
        /// Xml Node Type: Attribute <br/>
        /// Xml Required: Y <br/>
        /// </remarks> 
        //using "shim" property b/c System.Uri is not serializable
        [XmlIgnore]
        public Uri ManifestUri { get; set; }

        [XmlAttribute("ManifestUri")]
        [EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
        public string ManifestUriString
        {
            get
            {
                return ManifestUri?.AsString();
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                    ManifestUri = new Uri(value);
            }
        }

        /// <summary>
        /// Information about the location on which a <see cref="Deployment"/> and its associated 
        /// <see cref="AddIn"/> is stored.
        /// </summary>
        /// <remarks>
        /// Xml Node Type: Element <br/>
        /// Xml Required: Y 
        /// </remarks>  
        [XmlElement("FileHost", typeof(FileHost), IsNullable = false)]
        public FileHost FileHost { get; set; }
    }
}
