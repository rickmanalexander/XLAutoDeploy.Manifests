using XLAutoDeploy.Manifests.Utilities;

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

        [XmlIgnore]
        public Uri DeploymentRegistryUri { get; set; }

        [XmlElement("DeploymentRegistryUri", IsNullable = false)]
        [EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
        public string DeploymentRegistryUriString
        {
            get
            {
                return DeploymentRegistryUri?.AsString();
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                    DeploymentRegistryUri = new Uri(value);
            }
        }
    }
}
