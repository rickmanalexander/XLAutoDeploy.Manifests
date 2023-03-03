using System;
using System.ComponentModel;
using System.Xml;
using System.Xml.Serialization;

namespace XLAutoDeploy.Manifests
{
    /// <summary>
    /// Represents the platform where an application can be installed and run.
    /// </summary>
    [Serializable]
    public sealed class RequiredOperatingSystem
    {
        /// <summary>
        /// Specifies a support URL for the dependent platform.
        /// </summary>  
        /// <remarks>
        /// Xml Node Type: Attribute <br/>
        /// Xml Required: Y <br/>
        /// </remarks> 
        [XmlAttribute]
        public string SupportUrl { get; set; }

        /// <summary>
        /// Gets the enumeration value that identifies the platform of the target OS.
        /// </summary>  
        /// <remarks>
        /// The value of this property will always be <see cref="PlatformID.Win32NT"/> because this application or (any that it loads/installs) must target the Windows OS.
        /// </remarks> 
        [XmlIgnore]
        public PlatformID PlatformId => PlatformID.Win32NT;

        /// <summary>
        /// Gets common name of the target OS.
        /// </summary>  
        /// <remarks>
        /// The value of this property will always be Windows because this application or (any that 
        /// it loads/installs) must target the Windows OS.
        /// </remarks> 
        [XmlIgnore]
        public string PlatformName => "Windows";

        /// <summary>
        /// Specifies the version of the target OS.
        /// </summary>  
        /// <remarks>
        /// If the OS version number of the application (located on the client) is less than the version number supplied in the deployment manifest, 
        /// the application will not run. Version numbers must be specified in the format major.minor.build.revision, where each (decimal separated) number
        /// is an unsigned integer. <br/> <br/>
        /// Xml Node Type: Attribute <br/>
        /// Xml Required: Y
        /// </remarks> 
        //using "shim" property b/c System.Version is not serializable
        [XmlIgnore]
        public System.Version Version { get; set; }

        [XmlAttribute("Version")]
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
        /// Specifies the "Bitness" (either 32 or 64) of the target operating system.
        /// </summary>  
        /// <remarks>
        /// All <see cref="Dependency"/>(s) must be compatible with this value. Note that 64 bit
        /// systems can run 32 bit assemblies. <br/> <br/>
        /// Xml Node Type: Element <br/>
        /// Xml Required: Y
        /// </remarks> 
        [XmlElement("Bitness", typeof(OperatingSystemBitness), IsNullable = false)]
        public OperatingSystemBitness Bitness { get; set; }
    }
}
