using System;
using System.ComponentModel;
using System.Xml;
using System.Xml.Serialization;

namespace XLAutoDeploy.Manifests
{
    /// <summary>
    /// Identifies the attributes used for the deployment of updates and exposure to the system.
    /// </summary>  
    [Serializable]
    public sealed class DeploymentSettings
    {
        public DeploymentSettings() { }

        /// <summary>
        /// Indicates the basis (<see cref="DeploymentBasis.PerUser"/> or <see cref="DeploymentBasis.PerMachine"/>) 
        /// for the deployment.
        /// </summary> 
        /// <remarks>
        /// This is used to determined the parent directory (on the client) to which the application 
        /// is deployed. Admin privileges are required for <see cref="DeploymentBasis.PerMachine"/> deployment. <br/> <br/>
        /// Xml Node Type: Element <br/>
        /// Xml Required: Y
        /// </remarks> 
        [XmlElement("DeploymentBasis", typeof(DeploymentBasis), IsNullable = false)]
        public DeploymentBasis DeploymentBasis { get; set; }

        /// <summary>
        /// Specifies the minimum version of the application that can run on the client.
        /// </summary>  
        /// <remarks>
        /// the default version check on the client. If If the minimum version number of the application (located on the client) is less than the version number supplied in the deployment manifest, 
        /// the application will not run. Otherwise Version numbers must be specified in the format major.minor.build.revision, where each (decimal separated) number
        /// is an unsigned integer. <br/> <br/>
        /// Xml Node Type: Element <br/>
        /// Xml Required: Y
        /// </remarks> 
        [XmlIgnore]
        public System.Version MinimumRequiredVersion { get; set; }

        [XmlElement("MinimumRequiredVersion", IsNullable = false)]
        [EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
        public string MinimumRequiredVersionString
        {
            get
            {
                return MinimumRequiredVersion?.ToString();
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                    MinimumRequiredVersion = new Version(value);
            }
        }

        /// <summary>
        /// Specifies that all files in the <see cref="AddIn"/> manifest associated with a 
        /// <see cref="Deployment"/> must have a .xlautodeploy file extension. 
        /// </summary>  
        /// <remarks>
        /// The extension allows all the files within a deployment to be downloaded from a 
        /// Web server that blocks transmission of files ending in "unsafe" extensions such as 
        /// .exe. This extension will be stripped off of the files Uri(s) as soon as they are 
        /// downloaded them from the Web server. <br/> <br/>
        /// Xml Node Type: Element <br/>
        /// Xml Required: Y 
        /// </remarks>  
        [XmlElement(IsNullable = false)]
        public bool MapFileExtensions { get; set; }

        /// <summary>
        /// Controls how the application is loaded on a client machine.
        /// </summary>  
        /// <remarks>
        /// Xml Node Type: Element <br/>
        /// Xml Required: Y 
        /// </remarks>  
        [XmlElement("LoadBehavior", typeof(LoadBehavior), IsNullable = false)]
        public LoadBehavior LoadBehavior { get; set; }

        /// <summary>
        /// Controls how updates are processed on a client machine.
        /// </summary>  
        /// <remarks>
        /// Xml Node Type: Element <br/>
        /// Xml Required: Y 
        /// </remarks>
        [XmlElement("UpdateBehavior", typeof(UpdateBehavior), IsNullable = false)]
        public UpdateBehavior UpdateBehavior { get; set; }
    }

    [Serializable]
    public enum DeploymentBasis
    {
        [XmlEnum("peruser")]
        PerUser = 1,
        [XmlEnum("permachine")]
        PerMachine = 2
    }
}
