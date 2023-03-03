using System;
using System.ComponentModel;
using System.Xml;
using System.Xml.Serialization;

namespace XLAutoDeploy.Manifests
{
    /// <summary>
    /// Describes the identity of an assembly.
    /// </summary>  
    /// <remarks>
    /// All assemblies must target the .Net Framework. Valid ProcessorArchitecture values will be
    /// will be MSIL, X86, or IA64. The file extension should always be .dll.
    /// </remarks>  
    [Serializable]
    public sealed class AssemblyIdentity
    {
        public AssemblyIdentity() { }

        /// <summary>
        /// Identifies the human-readable name of the assembly. 
        /// </summary>  
        /// <remarks>
        /// Cannot contain non-alpha-numeric characters. <br/> <br/>
        /// Xml Node Type: Attribute <br/>
        /// Xml Required: Y
        /// </remarks>  
        [XmlAttribute]
        public string Name { get; set; }

        /// <summary>
        /// Specifies the version number of the assembly.
        /// </summary>  
        /// <remarks>
        /// This value must be incremented in an updated manifest to trigger an application update.  <br/> <br/>
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
                return Version?.ToString();
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                    Version = new Version(value);
            }
        }

        /// <summary>
        /// Specifies a 16-character hexadecimal string that represents the last 8 bytes of the 
        /// SHA-1 hash value of the public key under which the deployment manifest is signed. 
        /// </summary>  
        /// <remarks>
        /// Xml Node Type: Attribute <br/>
        /// Xml Required: N
        /// </remarks>  
        [XmlAttribute]
        public string PublicKey { get; set; }

        /// <summary>
        /// Identifies the processor and bits-per-word of the platform targeted by an executable
        /// </summary>  
        /// <remarks>
        /// Specifies the processor. Valid values are MSIL (neutral with respect to processor 
        /// and bits per word), X86 for 32-bit Windows, IA64 for 64-bit Windows. 
        /// The value of this property must match that of the assembly being deployed. See 
        /// <see href="https://docs.microsoft.com/en-us/dotnet/api/system.reflection.processorarchitecture?view=netframework-4.8"/> 
        /// for more information.<br/> <br/>
        /// Xml Node Type: Attribute <br/>
        /// Xml Required: Y
        /// </remarks> 
        [XmlAttribute]
        public string ProcessorArchitecture { get; set; }

        /// <summary>
        /// The name of the culture to associate with the assembly. 
        /// </summary>  
        /// <remarks>
        /// Specify null, Empty, or "neutral" (any casing) to represent InvariantCulture. 
        /// The name can be an arbitrary string that doesn't contain NUL character, the legality of the culture name is not validated. <br/> <br/>
        /// Xml Node Type: Attribute <br/>
        /// Xml Required: N
        /// </remarks> 
        [XmlAttribute]
        public string Culture { get; set; }


        /// <summary>
        /// Represents a cryptographic hash that can be used to verify the authenticity of an assembly.
        /// </summary>  
        /// <remarks>
        /// Xml Node Type: Element <br/>
        /// Xml Required: N 
        /// </remarks>  
        [XmlElement("Hash", typeof(Hash), IsNullable = false)]
        public Hash Hash { get; set; }
    }
}
