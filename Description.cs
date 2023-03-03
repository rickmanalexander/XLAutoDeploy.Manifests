using XLAutoDeploy.Manifests.Utilities;

using System;
using System.ComponentModel;
using System.Xml;
using System.Xml.Serialization;

namespace XLAutoDeploy.Manifests
{
    /// <summary>
    /// Represents information pertaining to the add-in and its origin.
    /// </summary> 
    [Serializable]
    public sealed class Description
    {
        public Description() { }

        /// <summary>
        /// Name of the party that published the add-in.
        /// </summary>  
        /// <remarks>
        /// Xml Node Type: Attribute <br/>
        /// Xml Required: Y 
        /// </remarks>  
        [XmlAttribute]
        public string Publisher { get; set; }

        /// <summary>
        /// Name of the company that built the add-in.
        /// </summary>  
        /// <remarks>
        /// Xml Node Type: Attribute <br/>
        /// Xml Required: Y 
        /// </remarks>  
        [XmlAttribute]
        public string Manufacturer { get; set; }

        /// <summary>
        /// Identifies the full product name.
        /// </summary>  
        /// <remarks>
        /// Xml Node Type: Attribute <br/>
        /// Xml Required: Y 
        /// </remarks>  
        [XmlAttribute]
        public string Product { get; set; }

        /// <summary>
        /// Specifies a file path or Url where production information regarding the add-in can be 
        /// accessed.
        /// </summary>  
        /// <remarks>
        /// Xml Node Type: Element <br/>
        /// Xml Required: N 
        /// </remarks>  
        //using "shim" property b/c System.Uri is not serializable
        [XmlIgnore]
        public Uri SupportUri { get; set; }

        [XmlElement("SupportUri", IsNullable = false)]
        [EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
        public string SupportUriString
        {
            get
            {
                return SupportUri?.AsString();
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                    SupportUri = new Uri(value);
            }
        }
    }
}
