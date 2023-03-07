using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace XLAutoDeploy.Manifests
{
    /// <summary>
    /// Represents the location of the <see cref="DeploymentRegistry">.
    /// </summary>  
    [Serializable]
    [XmlType(AnonymousType = true)]
    [XmlRoot(Namespace = "https://github.com/XLAutoDeploy/Schemas/XLAutoDeployManifest.xsd", IsNullable = false)]
    public sealed class XLAutoDeployManifest
    {
        public XLAutoDeployManifest() { }

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
    }
}
