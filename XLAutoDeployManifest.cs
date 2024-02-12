using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace XLAutoDeploy.Manifests
{
    /// <summary>
    /// Represents the location of the <see cref="DeploymentRegistry"/>.
    /// </summary>  
    [Serializable]
    [XmlType(AnonymousType = true)]
    [XmlRoot(ElementName = "XLAutoDeployManifest", Namespace = "https://github.com/XLAutoDeploy/Schemas/XLAutoDeployManifest.xsd", IsNullable = false)]
    public sealed class XLAutoDeployManifest
    {
        public XLAutoDeployManifest() { }

        /// <summary>
        /// Specifies the version number of XLAutoDeploy.
        /// </summary>  
        //using "shim" property b/c System.Version is not serializable
        [XmlIgnore]
        public System.Version Version { get; set; }

        [XmlElement("Version", IsNullable = false)]
        [EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
        public string VersionString
        {
            get
            {
                return Version?.ToString() ?? String.Empty;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                    Version = new Version(value);
            }
        }

        /// <summary>
        /// Specifies the Excel DNA version number on which XLAutoDeploy was compiled.
        /// </summary>  
        /// <remarks>
        /// Xml Node Type: Element <br/>
        /// Xml Required: Y
        /// </remarks> 
        //using "shim" property b/c System.Version is not serializable
        [XmlIgnore]
        public System.Version ExcelDnaVersion { get; set; }

        [XmlElement("ExcelDnaVersion", IsNullable = false)]
        [EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
        public string ExcelDnaVersionString
        {
            get
            {
                return ExcelDnaVersion?.ToString() ?? String.Empty;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                    ExcelDnaVersion = new Version(value);
            }
        }

        /// <summary>
        /// Specifies the Uri of the associated <see cref="DeploymentRegistry"/>.
        /// </summary>  
        /// <remarks>
        /// This is used to locate and de-serialize the <see cref="DeploymentRegistry"/> manifest file on 
        /// the remote <see cref="FileHost"/>. <br/> <br/>
        /// Xml Node Type: Element <br/>
        /// Xml Required: Y
        /// </remarks> 
        [XmlIgnore]
        public Uri DeploymentRegistryUri { get; set; }

        [XmlElement("DeploymentRegistryUri", IsNullable = false)]
        [EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
        public string DeploymentRegistryUriString
        {
            get
            {
                return DeploymentRegistryUri?.AbsoluteUri;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                    DeploymentRegistryUri = new Uri(value);
            }
        }

        /// <summary>
        /// Specifies the root folder/directory path for log files.
        /// </summary>  
        /// <remarks>
        /// Xml Node Type: Element <br/>
        /// Xml Required: N
        /// </remarks> 
        [XmlElement("LoggerFileDirectory", IsNullable = true)]
        public string LoggerFileDirectory { get; set; }
    }
}
