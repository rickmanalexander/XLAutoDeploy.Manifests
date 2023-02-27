using System;
using System.Xml.Serialization;

namespace XLAutoDeploy.Manifests
{
    public static class Constants
    {
        public const string XLAutoDeployFileExtention = "xlautodeploy";

        public const string DeploymentManifestParameterizedFileName = "{0}-Deployment.manifest.xml";
        public const string AddInManifestParameterizedFileName = "{0}-AddIn.manifest.xml";
        public const string UpdateQueryInfoManifestParameterizedFileName = "{0}-UpdateQueryInfo.manifest.xml";
    }
    
    /// <summary>
    /// Represents an interval of time. 
    /// </summary>
    [Serializable]
    public enum UnitOfTime
    {
        [XmlEnum("minutes")]
        Minutes = 1,
        [XmlEnum("days")]
        Days = 2,
        [XmlEnum("weeks")]
        Weeks = 3,
        [XmlEnum("months")]
        Months = 4
    }

    /// <summary>
    /// Represents the bits-per-word (i.e. "Bitness", either 32 or 64) of the installation of Microsoft Office targeted by the add-in.
    /// </summary> 
    /// <remarks>
    /// Note that 64 bit operating systems are compatible with 32 bit installations of Microsoft Office.
    /// </remarks>  
    [Serializable]
    public enum MicrosoftOfficeBitness
    {
        [XmlEnum("x86")]
        X86 = 32,
        [XmlEnum("x64")]
        X64 = 64
    }

    /// <summary>
    /// Represents the bits-per-word (i.e. "Bitness", either 32 or 64) of the platform targeted by 
    /// an assembly.
    /// </summary> 
    /// <remarks>
    /// For unmanaged assemblies that do not target the .Net framework, or are native 
    /// (e.g. written C, assembler, or similar), <see cref="OperatingSystemBitness.None"/> should 
    /// be used.
    /// </remarks>  
    [Serializable]
    public enum OperatingSystemBitness
    {
        [XmlEnum("none")]
        None = 0,
        [XmlEnum("x86")]
        X86 = 32,
        [XmlEnum("x64")]
        X64 = 64
    }

    /// <summary>
    /// Represents all .Net Framework CLR versions published and deployed by Microsoft.
    /// </summary> 
    [Serializable]
    public enum NetClrVersion
    {
        [XmlEnum("1.0")]
        V1 = 1,
        [XmlEnum("1.1")]
        V11 = 2,
        [XmlEnum("2.0")]
        V2 = 3,
        [XmlEnum("4.0")]
        V4 = 4
    }
}
