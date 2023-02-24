using System;
using System.Xml;
using System.Xml.Serialization;

namespace XLAutoDeploy.Manifests
{
    /// <summary>
    /// Represents the physical file location on client machine to which supporting files 
    /// (i.e. <see cref="Dependency"/>(s) and <see cref="AssetFile"/>(s)) are deployed. 
    /// </summary>  
    [Serializable]
    public sealed class FilePlacement
    {
        public FilePlacement() { }

        /// <summary>
        /// Specifies whether this file is placed next to the add-in when downloaded on the client. 
        /// </summary>  
        /// <remarks>
        /// This is particularly useful .dna for xll's deployed as un-packed. The dna file can redirect to 
        /// sub-directory(s) that contains another .dna file. This secondary dna file should point to the 
        /// <see cref="Dependency"/>(s) and <see cref="AssetFile"/>(s) in said sub-directory. 
        /// See <see href="https://stackoverflow.com/a/30377635/9743237"/> for more information. <br/> <br/>
        /// Xml Node Type: Attribute <br/>
        /// Xml Required: Y
        /// </remarks> 
        [XmlAttribute]
        public bool NextToAddIn { get; set; }

        /// <summary>
        /// The name or path of the SubDirectory to which this file belongs.
        /// </summary>  
        /// <remarks>
        /// If <see cref="NextToAddIn"/> is true, then this value will be ignored.
        /// This would be the sub-directory in which the file would downloaded on the client if 
        /// the add-in redirects it references. <br/> <br/>
        /// Xml Node Type: Element <br/>
        /// Xml Required: N
        /// </remarks> 
        [XmlElement("SubDirectory", IsNullable = false)]
        public string SubDirectory { get; set; }
    }
}
