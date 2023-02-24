using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml;
using System.Xml.Serialization;

using XLAutoDeploy.Manifests.Utilities;

namespace XLAutoDeploy.Manifests
{
    /// <summary>
    /// Represents an assembly (and any accompanying files) on which an add-in depends.
    /// </summary>  
    [Serializable]
    public sealed class Dependency
    {
        public Dependency() { }

        /// <summary>
        /// Specifies the full path to the application Dependency.
        /// </summary>  
        /// <remarks>
        /// Xml Node Type: Attribute <br/>
        /// Xml Required: Y <br/>
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
        /// Specifies enumeration value that identifies the type of dependency.
        /// </summary>  
        /// <remarks>
        /// A dependent assembly of type <see cref="DependencyType.RequiredReference"/> is always downloaded alongside the application. 
        /// A dependent assembly of type <see cref="DependencyType.Prequisite"/> must be present in the global assembly cache (GAC) before 
        /// the application can be loaded or installed. <br/> <br/>
        /// Xml Node Type: Element <br/>
        /// Xml Required: Y <br/>
        /// </remarks> 
        [XmlElement("Type", typeof(DependencyType), IsNullable = false)]
        public DependencyType Type { get; set; }

        /// <summary>
        /// Specifies the size (in bytes) of the assembly file.
        /// </summary>  
        /// <remarks>
        /// Xml Node Type: Element <br/>
        /// Xml Required: Y
        /// </remarks> 
        [XmlElement]
        public long Size { get; set; }

        /// <summary>
        /// Describes the identity of an assembly.
        /// </summary>  
        /// <remarks>
        /// Xml Node Type: Element <br/>
        /// Xml Required: Y 
        /// </remarks>
        [XmlElement("AssemblyId", typeof(AssemblyIdentity), IsNullable = false)]
        public AssemblyIdentity AssemblyIdentity { get; set; }

        /// <summary>
        /// Specifies if the assembly is managed (CLR) or unmanaged (i.e. native/non-CLR).
        /// </summary>  
        /// <remarks>
        /// Xml Node Type: Element <br/>
        /// Xml Required: Y 
        /// </remarks>  
        [XmlElement("ManagedAssembly", IsNullable = false)]
        public bool ManagedAssembly { get; set; }

        /// <summary>
        /// Represents the physical file location on the client machine to which this file is deployed. 
        /// </summary>  
        /// <remarks>
        /// Xml Node Type: Element <br/>
        /// Xml Required: Y
        /// </remarks> 
        [XmlElement("FilePlacement", IsNullable = false)]
        public FilePlacement FilePlacement { get; set; }

        /// <summary>
        /// Identifies nonassembly file(s) to be downloaded and used by the assembly.
        /// </summary>  
        /// <remarks>
        /// Xml Node Type: Element <br/>
        /// Xml Required: N <br/>
        /// </remarks> 
        [XmlArray("AssetFiles", IsNullable = false), XmlArrayItem(typeof(AssetFile), ElementName = "AssetFile", IsNullable = false)]
        public List<AssetFile> AssetFiles { get; set; }
    }

    /// <summary>
    /// Describes the type/contents of file.
    /// </summary>
    [Serializable]
    public enum DependencyType
    {
        /// <summary>
        /// Must be present in the global assembly cache (GAC) before the application can be run or installed. 
        /// </summary>
        //this value should be used for installing required runtimes (require user interaction and elevated privileges)
        [XmlEnum("prequisite")]
        Prequisite = 1,

        /// <summary>
        /// A required file that is part or referenced by the application.
        /// </summary>
        [XmlEnum("requiredreference")]
        RequiredReference = 2
    }
}
