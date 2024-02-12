using System;
using System.Xml;
using System.Xml.Serialization;

namespace XLAutoDeploy.Manifests
{
    /// <summary>
    /// Represents information regarding an update coming from a remote <see cref="FileHost"/>.
    /// </summary>  
    [Serializable]
    [XmlType(AnonymousType = true)]
    [XmlRoot(ElementName = "UpdateQueryInfo", IsNullable = false)]
    public sealed class UpdateQueryInfo
    {
        public UpdateQueryInfo() { }

        public UpdateQueryInfo(System.Version availableVersion, System.Version minimumRequiredVersion,
            System.Version deployedVersion, bool updateAvailable, bool isMandatoryUpdate, 
            bool isRestartRequired, long size) 
        {
            AvailableVersion = availableVersion;
            MinimumRequiredVersion = minimumRequiredVersion;
            DeployedVersion = deployedVersion;
            UpdateAvailable = updateAvailable;
            IsMandatoryUpdate = isMandatoryUpdate;
            IsRestartRequired = isRestartRequired;
            Size = size;
        }

        /// <summary>
        /// The version of the incoming update.
        /// </summary>  
        /// <remarks>
        /// The <see cref="AddInIdentity.Version"/> listed in the <see cref="AddIn"/> manifest on the
        /// remote <see cref="FileHost"/>. 
        /// </remarks> 
        [XmlIgnore]
        public System.Version AvailableVersion { get; set; }

        /// <summary>
        /// Specifies the minimum version of the application that can run on the client.
        /// </summary>  
        /// <remarks>
        /// This is used to determine if an incoming update is mandatory.
        /// </remarks> 
        [XmlIgnore]
        public System.Version MinimumRequiredVersion { get; set; }

        /// <summary>
        /// The version currently deployed on the client.
        /// </summary>  
        /// <remarks>
        /// The <see cref="AddInIdentity.Version"/> listed in the <see cref="AddIn"/> manifest 
        /// on the client machine. 
        /// </remarks> 
        [XmlIgnore]
        public System.Version DeployedVersion { get; set; }

        /// <summary>
        /// Indicates whether an update exists on the remote <see cref="FileHost"/>
        /// </summary>  
        [XmlIgnore]
        public bool UpdateAvailable { get; set; }

        /// <summary>
        /// Indicates whether or not to the incoming update is mandatory.
        /// </summary>  
        [XmlIgnore]
        public bool IsMandatoryUpdate { get; set; }

        /// <summary>
        /// Indicates whether or not to the incoming update requires a resart of Excel.
        /// </summary>  
        [XmlIgnore]
        public bool IsRestartRequired { get; set; }

        /// <summary>
        /// Specifies the toal size (in bytes) of all files in the incoming update.
        /// </summary>  
        [XmlIgnore]
        public long Size { get; set; }


        /// <summary>
        /// The last time an update check was performed.
        /// </summary>  
        /// <remarks>
        /// This should only be null if the latest version was deployed for the first time. <br/> <br/>
        /// Xml Node Type: Element <br/>
        /// Xml Required: Y 
        /// </remarks>  
        [XmlElement("LastChecked", typeof(DateTime?), IsNullable = true)]
        public DateTime? LastChecked { get; set; }

        /// <summary>
        /// The first time a user was notified that an update was avaliable.
        /// </summary>  
        /// <remarks>
        /// This should only be set (or re-set) if the incoming <see cref="AvailableVersion"/> 
        /// differs from the deployed <see cref="AvailableVersion"/>, AFTER a user has been 
        /// notified that the incoming update is available, for the first time. <br/> <br/>
        /// Xml Node Type: Element <br/>
        /// Xml Required: If <see cref="UpdateBehavior.NotifyClient"/> is set to true.  
        /// </remarks>  
        [XmlElement("FirstNotified", typeof(DateTime?), IsNullable = true)]
        public DateTime? FirstNotified { get; set; }

        /// <summary>
        /// The last time a user was notified that an update was avaliable.
        /// </summary>  
        /// <remarks>
        /// This should only be set (or re-set) after a user has been notified that an 
        /// update is available. If this is the first time the user is being notified, then
        /// then this should the same value as the <see cref="FirstNotified"/>. <br/> <br/>
        /// Xml Node Type: Element <br/>
        /// Xml Required: If <see cref="UpdateBehavior.NotifyClient"/> is set to true. 
        /// </remarks>  
        [XmlElement("LastNotified", typeof(DateTime?), IsNullable = true)]
        public DateTime? LastNotified { get; set; }
    }
}
