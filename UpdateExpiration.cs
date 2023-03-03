using System;
using System.Xml;
using System.Xml.Serialization;

namespace XLAutoDeploy.Manifests
{
    /// <summary>
    /// Represents how old the current update can become before the application will perform the 
    /// next update check. 
    /// </summary>
    [Serializable]
    public sealed class UpdateExpiration
    {
        /// <summary>
        /// Represents a human-readable description of an <see cref="UpdateExpiration"/>.
        /// </summary>
        [XmlIgnore]
        public string Description => $"Check for update every {MaximumAge} {Enum.GetName(typeof(UnitOfTime), UnitOfTime)}";

        /// <summary>
        /// Identifies the unit of time for the <see cref="P:MaximumAge"/>. Valid units are hours, 
        /// days, and weeks.
        /// </summary>
        /// <remarks>
        /// Xml Node Type: Element <br/>
        /// Xml Required: Y 
        /// </remarks>
        [XmlElement("UnitOfTime", typeof(UnitOfTime), IsNullable = false)]
        public UnitOfTime UnitOfTime { get; set; }

        /// <summary>
        /// Identifies how old the current update should become before the application performs an 
        /// update check. The unit of time is determined by the <see cref="P:UnitOfTime"/>.
        /// </summary>
        /// <remarks>
        /// Xml Node Type: Element <br/>
        /// Xml Required: Y 
        /// </remarks>
        [XmlElement(IsNullable = false)]
        public uint MaximumAge { get; set; }
    }
}
