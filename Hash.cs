using System;
using System.Xml;
using System.Xml.Serialization;

namespace XLAutoDeploy.Manifests
{
    /// <summary>
    /// Represents a cryptographic hash that can be used to verify the authenticity of a file.
    /// </summary>  
    public sealed class Hash
    {
        /// <summary>
        /// The algorithm used to compute the hash value. 
        /// </summary>
        /// <remarks>
        /// Xml Node Type: Element <br/>
        /// Xml Required: Y 
        /// </remarks> 
        [XmlElement("Algorithm", typeof(SecureHashAlgorithm), IsNullable = false)]
        public SecureHashAlgorithm Algorithm { get; set; }

        /// <summary>
        /// Hash value of the file.
        /// </summary>
        /// <remarks>
        /// Xml Node Type: Element <br/>
        /// Xml Required: Y 
        /// </remarks> 
        [XmlElement(IsNullable = false)]
        public string Value { get; set; }
    }


    //See https://docs.microsoft.com/en-us/dotnet/api/system.security.cryptography.hashalgorithmname?view=netframework-4.8
    //"Due to collision problems with MD5 and SHA1, Microsoft recommends a security model based on SHA256 or better."
    [Serializable]
    public enum SecureHashAlgorithm
    {
        [XmlEnum("SHA256")]
        SHA256 = 1,
        [XmlEnum("SHA384")]
        SHA384 = 2,
        [XmlEnum("SHA512")]
        SHA512 = 3
    }
}
