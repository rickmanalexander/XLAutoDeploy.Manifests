using System;
using System.ComponentModel;
using System.Xml;
using System.Xml.Serialization;

namespace XLAutoDeploy.Manifests
{
    /// <summary>
    /// Represents a version of the .NET Framework where an application can be installed and run.
    /// </summary>  
    [Serializable]
    public sealed class CompatibleFramework
    {
        public CompatibleFramework() { }

        /// <summary>
        /// Specifies a URL where the preferred compatible .NET Framework version can be downloaded.
        /// </summary>  
        /// <remarks>
        /// Xml Node Type: Attribute <br/>
        /// Xml Required: Y <br/>
        /// </remarks> 
        [XmlAttribute]
        public string SupportUrl { get; set; }

        /// <summary>
        /// Specifies if this framework is required for the application to run.
        /// </summary>  
        /// <remarks>
        /// Xml Node Type: Attribute <br/>
        /// Xml Required: Y <br/>
        /// </remarks> 
        [XmlAttribute]
        public bool Required { get; set; }

        /// <summary>
        /// Specifies CLR version required to run the application.
        /// </summary>  
        /// <remarks>
        /// The CLR version required to run the application. <br/> <br/>
        /// Xml Node Type: Element <br/> 
        /// Xml Required: Y
        /// </remarks> 
        [XmlElement("SupportedRuntime", typeof(NetClrVersion), IsNullable = false)]
        public NetClrVersion SupportedRuntime { get; set; }

        /// <summary>
        /// Specifies the version number of the target .NET Framework.
        /// </summary>  
        /// <remarks>
        /// The .NET Framework version required to run the application. <br/> <br/>
        /// Xml Node Type: Element <br/>
        /// Xml Required: Y
        /// </remarks> 
        //using "shim" property b/c System.Version is not serializable
        [XmlIgnore]
        public System.Version TargetVersion { get; set; }

        [XmlElement("TargetVersion", IsNullable = false)]
        [EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
        public string TargetVersionString
        {
            get
            {
                return TargetVersion?.ToString() ?? String.Empty;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                    TargetVersion = new Version(value);
            }
        }
    }
}
