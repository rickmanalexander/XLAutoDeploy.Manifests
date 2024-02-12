using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace XLAutoDeploy.Manifests
{
    /// <summary>
    /// Represents the locations of all <see cref="Deployment"/>'s.
    /// </summary>  
    [Serializable]
    [XmlType(AnonymousType = true)]
    [XmlRoot(ElementName = "DeploymentRegistry", Namespace = "https://github.com/XLAutoDeploy.Manifests/Schemas/DeploymentRegistry.xsd", IsNullable = false)]
    public sealed class DeploymentRegistry
    {
        public DeploymentRegistry() { }

        /// <summary>
        /// Represents the locations of one or more <see cref="PublishedDeployment"/>'s.
        /// </summary>  
        /// <remarks>
        /// Xml Node Type: Element <br/>
        /// Xml Required: Y
        /// </remarks> 
        [XmlArray("PublishedDeployments", IsNullable = false), XmlArrayItem(typeof(PublishedDeployment), ElementName = "PublishedDeployment", IsNullable = false)]
        public List<PublishedDeployment> PublishedDeployments { get; set; }
    }
}