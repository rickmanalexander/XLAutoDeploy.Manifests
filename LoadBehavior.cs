using System;
using System.Xml.Serialization;

namespace XLAutoDeploy.Manifests
{
    /// <summary>
    /// Defines how an add-in is loaded on the client's excel instance.
    /// </summary>
    [Serializable]
    public sealed class LoadBehavior
    {
        public LoadBehavior() { }

        /// <summary>
        /// Specifies whether the add-in should be installed when it is loaded.
        /// </summary>
        /// <remarks>
        /// Xml Node Type: Attribute <br/>
        /// Xml Required: Y 
        /// </remarks>
        [XmlAttribute]
        public bool Install { get; set; }

        /// <summary>
        /// Specifies the order in which an add-in is loaded in relation to 
        /// other add-ins if one or more is being loaded. 
        /// </summary>
        /// <remarks>
        /// Xml Node Type: Element <br/>
        /// Xml Required: N 
        /// </remarks>
        [XmlElement(IsNullable = false)]
        public uint LoadOrder { get; set; }
    }
}
