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
    /// Represents a manifest file for an Excel add-in.
    /// </summary>  
    /// <remarks>
    /// The latest version of this file should be stored on a remote <see cref="FileHost"/>
    /// and the <see cref="Deployment"/> should point it. Based on the <see cref="DeploymentSettings"/>,
    /// a copy of this file will be downloded onto to the client. 
    /// </remarks> 
    [Serializable]
    [XmlType(AnonymousType = true)]
    [XmlRoot(Namespace = "https://github.com/XLAutoDeploy.Manifests/Schemas/AddIn.xsd", IsNullable = false)]
    public sealed class AddIn
    {
        public AddIn() { }

        /// <summary>
        /// Specifies the Uri of the add-in file.
        /// </summary>  
        /// <remarks>
        /// This is used locate the add-in file. Supported file extentions include .xlam, .xla, 
        /// .xll, or .dll. Note, if .dll, then the <see cref="AddInType"/> must be either 
        /// <see cref="AddInType.Automation"/> or <see cref="AddInType.Com"/>. The client 
        /// must have admin rights so that the assembly can be programatically registered
        /// for COM interop. <br/> <br/>  
        /// Xml Node Type: Element <br/>
        /// Xml Required: Y
        /// </remarks> 
        [XmlIgnore]
        public Uri Uri { get; set; }

        [XmlElement("Uri", typeof(string), IsNullable = false)]
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
        /// Specifies the Uri of the associated <see cref="Deployment"/>.
        /// </summary>  
        /// <remarks>
        /// This is used locate the depoyment manifest on the remote <see cref="FileHost"/> 
        /// so that it can be de-serialized. <br/> <br/>  
        /// Xml Node Type: Attribute <br/>
        /// Xml Required: Y
        /// </remarks> 
        [XmlIgnore]
        public Uri DeploymentUri { get; set; }

        [XmlElement("DeploymentUri", typeof(string), IsNullable = false)]
        [EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
        public string DeploymentUriString
        {
            get
            {
                return DeploymentUri?.AsString();
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                    DeploymentUri = new Uri(value);
            }
        }

        /// <summary>
        /// Specifies the "Bitness" (either 32 or 64) of the installation of Microsoft Office /on/ which the add-in can run.
        /// </summary>  
        /// <remarks>
        /// The physical add-in <see cref="AddIn"/> file must be compatible with this value. //Note that 64 bit
        /// operating systems are compatible with 32 bit installations of Microsoft Office.  <br/> <br/>
        /// Xml Node Type: Element <br/>
        /// Xml Required: Y
        /// </remarks> 
        [XmlElement("TargetOfficeInstallation", typeof(MicrosoftOfficeBitness), IsNullable = false)]
        public MicrosoftOfficeBitness TargetOfficeInstallation { get; set; }

        /// <summary>
        /// Identifies the attributes associated with a physical add-in file.
        /// </summary>  
        /// <remarks>
        /// Xml Node Type: Element <br/>
        /// Xml Required: Y 
        /// </remarks>
        [XmlElement("Identity", typeof(AddInIdentity), IsNullable = false)]
        public AddInIdentity Identity { get; set; }

        /// <summary>
        /// Identifies assembly(s) on which the add-in depends.
        /// </summary>  
        /// <remarks>
        /// If the <see cref="AddInIdentity.AddInType"/> == <see cref="AddInType.ExcelDna"/>, 
        /// and it is being deployed as un-packed, then there must be one or more 
        /// <see cref="Dependency"/>(s) included. The same is true for add-ins of type 
        /// <see cref="AddInType.Vba"/> that reference an external assembly(s). <br/> <br/>
        /// Xml Node Type: Array <br/>
        /// Xml Required: N <br/>
        /// </remarks> 
        [XmlArray("Dependencies", IsNullable = true), XmlArrayItem(typeof(Dependency), ElementName = "Dependency", IsNullable = false)]
        public List<Dependency> Dependencies { get; set; }

        /// <summary>
        /// Identifies nonassembly file(s) to be downloaded and used by the add-in.
        /// </summary>  
        /// <remarks>
        /// If the <see cref="AddInIdentity.AddInType"/> == <see cref="AddInType.ExcelDna"/>, and it 
        /// is being deployed as un-packed, then the .dna file must be included as an
        /// <see cref="AssetFile"/>. <br/> <br/>
        /// Xml Node Type: Array <br/>
        /// Xml Required: N <br/>
        /// </remarks> 
        [XmlArray("AssetFiles", IsNullable = false), XmlArrayItem(typeof(AssetFile), ElementName = "AssetFile", IsNullable = false)]
        public List<AssetFile> AssetFiles { get; set; }

        /// <summary>
        /// Specifies a Digital Signature that can be used to verify the authenticity of the 
        /// add-in Manifest.
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
