using System;
using System.ComponentModel;
using System.Xml;
using System.Xml.Serialization;

using XLAutoDeploy.Manifests.Utilities;

namespace XLAutoDeploy.Manifests
{
    /// <summary>
    /// Identifies a nonassembly file to be downloaded and used by the application.
    /// </summary>  
    [Serializable]
    public sealed class AssetFile
    {
        public AssetFile() { }

        /// <summary>
        /// Specifies the Uri of the file.
        /// </summary>  
        /// <remarks>
        /// Xml Node Type: Attribute <br/>
        /// Xml Required: Y
        /// </remarks> 
        //using "shim" property b/c System.Uri is not serializable
        [XmlIgnore]
        public Uri Uri { get; set; }

        [XmlAttribute("Uri")]
        [EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
        public string UriString
        {
            get
            {
                return Uri?.AsString();
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                    Uri = new Uri(value);
            }
        }

        /// <summary>
        /// The name (including the extension) of the file.
        /// </summary>  
        /// <remarks>
        /// Xml Node Type: Element <br/>
        /// Xml Required: Y
        /// </remarks> 
        [XmlAttribute]
        public string Name { get; set; }

        /// <summary>
        /// Specifies the size (in bytes) of the file.
        /// </summary>  
        /// <remarks>
        /// Xml Node Type: Element <br/>
        /// Xml Required: Y
        /// </remarks> 
        [XmlAttribute]
        public long Size { get; set; }

        /// <summary>
        /// Specifies that the file is a read/write data file.
        /// </summary>  
        /// <remarks>
        /// Xml Node Type: Element <br/>
        /// Xml Required: N
        /// </remarks> 
        [XmlElement("Writeable", IsNullable = false)]
        public bool Writeable { get; set; }

        /// <summary>
        /// Indicated if the file is a zipped file that it should be decompressed. 
        /// </summary>  
        /// <remarks>
        /// If true, the file unzipped when downloaded. <br/> <br/>
        /// Xml Node Type: Element <br/>
        /// Xml Required: N
        /// </remarks> 
        [XmlElement("DecompressIfZipped", IsNullable = false)]
        public bool DecompressIfZipped { get; set; }

        /// <summary>
        /// Represents the physical file location on the client machine to which this file is 
        /// deployed. 
        /// </summary>  
        /// <remarks>
        /// Xml Node Type: Element <br/>
        /// Xml Required: Y
        /// </remarks> 
        [XmlElement("FilePlacement", typeof(FilePlacement), IsNullable = false)]
        public FilePlacement FilePlacement { get; set; }

        /// <summary>
        /// Represents a cryptographic hash that can be used to verify the authenticity of an the file.
        /// </summary>  
        /// <remarks>
        /// Xml Node Type: Element <br/>
        /// Xml Required: N 
        /// </remarks>  
        [XmlElement("Hash", typeof(Hash), IsNullable = false)]
        public Hash Hash { get; set; }
    }
}
