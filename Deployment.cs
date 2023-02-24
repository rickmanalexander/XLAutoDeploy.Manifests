using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Security.Cryptography.Xml;
using System.Xml;
using System.Xml.Serialization;

using XLAutoDeploy.Manifests.Utilities;

namespace XLAutoDeploy.Manifests
{
    /// <summary>
    /// Represents a manifest file that contains information required to deploy updates to 
    /// an Excel add-in from a remote <see cref="FileHost"/> to a client. 
    /// </summary>  
    [Serializable]
    [XmlType(AnonymousType = true)]
    [XmlRoot(Namespace = "https://github.com/XLAutoDeploy.Manifests/Schemas/Deployment.xsd", IsNullable = false)]
    public sealed class Deployment
    {
        public Deployment() { }

        /// <summary>
        /// Specifies the Uri of the associated <see cref="AddIn"/>.
        /// </summary>  
        /// <remarks>
        /// This is used to locate and de-serialize the <see cref="AddIn"/> manifest file on 
        /// the remote <see cref="FileHost"/>. If the <see cref="AddIn"/> manifest has been updated 
        /// to reflect a new Version of an add-in, the <see cref="Uri"/>(s) defined in the 
        /// <see cref="AddIn.Dependencies"/>(s) and <see cref="AddIn.AssetFiles"/>(s) are used 
        /// to locate the files so they can be downloaded onto the client. Once the update is complete, 
        /// the <see cref="AddIn"/> manifest is saved to the client machine, effectively 
        /// completing an update cycle. <br/> <br/>
        /// Xml Node Type: Element <br/>
        /// Xml Required: Y
        /// </remarks> 
        //using "shim" property b/c System.Uri is not serializable
        [XmlIgnore]
        public Uri AddInUri { get; set; }

        [XmlElement("AddInUri", IsNullable = false)]
        [EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
        public string AddInUriString
        {
            get
            {
                return AddInUri?.AsString();
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                    AddInUri = new Uri(value);
            }
        }

        /// <summary>
        /// Specifies an alternate Uri of the associated <see cref="AddIn"/>.
        /// </summary>  
        /// <remarks>
        /// The functionality for this property is the same as the <see cref="AddInUri"/> except in the event the <see cref="AddIn.TargetOfficeInstallation"/> defined for the first <see cref="AddInUri"/>, this Uri will be used to pull an <see cref="AddIn"/> with a different <see cref="AddIn.TargetOfficeInstallation"/> that will hopefully be compatible with the client's office instance. <br/> <br/>
        /// Xml Node Type: Element <br/>
        /// Xml Required: Y
        /// </remarks> 
        //using "shim" property b/c System.Uri is not serializable
        [XmlIgnore]
        public Uri AlternateAddInUri { get; set; }

        [XmlElement("AlternateAddInUri", IsNullable = false)]
        [EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
        public string AlternateAddInUriString
        {
            get
            {
                return AlternateAddInUri?.AsString();
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                    AlternateAddInUri = new Uri(value);
            }
        }

        /// <summary>
        /// Represents information pertaining to the application and its origin.
        /// </summary>  
        /// <remarks>
        /// Xml Node Type: Element <br/>
        /// Xml Required: Y 
        /// </remarks>  
        [XmlElement("Description", typeof(Description), IsNullable = false)]
        public Description Description { get; set; }

        /// <summary>
        /// Identifies the attributes used for the deployment of updates and exposure to the system.
        /// </summary>  
        /// <remarks>
        /// Xml Node Type: Element <br/>
        /// Xml Required: Y 
        /// </remarks>  
        [XmlElement("Settings", typeof(DeploymentSettings), IsNullable = false)]
        public DeploymentSettings Settings { get; set; }

        /// <summary>
        /// Represents the platform where an add-in can be installed and run.
        /// </summary>  
        /// <remarks>
        /// Xml Node Type: Element <br/>
        /// Xml Required: Y 
        /// </remarks>  
        [XmlElement("RequiredOperatingSystem", typeof(RequiredOperatingSystem), IsNullable = false)]
        public RequiredOperatingSystem RequiredOperatingSystem { get; set; }

        /// <summary>
        /// Identifies the versions of the .NET Framework where the add-in can be loaded/installed and run.
        /// </summary>  
        /// <remarks>
        /// Xml Node Type: Array <br/>
        /// Xml Required: Y <br/>
        /// </remarks> 
        [XmlArray("CompatibleFrameworks", IsNullable = false), XmlArrayItem(typeof(CompatibleFramework), ElementName = "CompatibleFramework", IsNullable = false)]
        public List<CompatibleFramework> CompatibleFrameworks { get; set; }

        /// <summary>
        /// Specifies a Digital Signature that can be used to verify the authenticity of the Manifest.
        /// </summary>  
        /// <remarks>
        /// Xml Node Type: Element <br/>
        /// Xml Required: N
        /// </remarks> 
        // using "shim" property b/c Signature is not serializable
        [XmlIgnore]
        public Signature DigitalSignature { get; set; }

        [XmlAnyElement("DigitalSignature")]
        [EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
        // See: https://stackoverflow.com/a/40621866/9743237
        public XmlElement DigitalSignatureXml
        {
            get
            {
                return DigitalSignature?.GetXml();
            }
            set
            {
                if (value != null)
                    DigitalSignature = new SignedXml(value).Signature;
            }
        }
    }
}
