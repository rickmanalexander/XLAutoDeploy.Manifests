using System;
using System.Xml.Serialization;

namespace XLAutoDeploy.Manifests
{
    /// <summary>
    /// Controls how add-in updates are processed on a client machine.
    /// </summary>
    [Serializable]
    public sealed class UpdateBehavior
    {
        public UpdateBehavior() { }

        /// <summary>
        /// Specifies if the update is required and if so, how update notifications should
        /// be displayed to the client.  
        /// </summary>
        /// <remarks>
        /// If <see cref="UpdateMode.Forced"/> then the update will be processed regardless if 
        /// the <see cref="AddInIdentity.Version"/> (of the <see cref="AddIn"/>) was incremented 
        /// or not. When <see cref="UpdateBehavior.NotifyClient"/> is set to true updates will 
        /// display a non-cancellable  dialog to the client that notifies them of a pending 
        /// update. If <see cref="UpdateMode.Normal"/>, then, updates can be deferred until a 
        /// later time. <br/> <br/>
        /// Xml Node Type: Element <br/>
        /// Xml Required: Y 
        /// </remarks>
        [XmlElement("Mode", typeof(UpdateMode), IsNullable = false)]
        public UpdateMode Mode { get; set; }

        /// <summary>
        /// Specifies whether the Excel Application shoud be restarted for the add-in update to take place. 
        /// </summary>
        /// <remarks>
        /// This must be set to "true" for the following types of add-ins: 
        /// 1. Add-ins that use any of the COM features (e.g. Ribbon, CTP, RTD, etc.)
        /// 2. Add-ins that reference external assemblies.
        /// See <see href="https://stackoverflow.com/questions/63117328/how-can-i-avoid-excel-dna-add-in-from-locking-external-library-dlls"/> for more information. <br/>
        /// If the add-in is installed on the client's instance of Excel, Excel must be restarted 
        /// regardless of the value of this property. <br/> <br/>
        /// Xml Node Type: Element <br/>
        /// Xml Required: Y 
        /// </remarks>
        [XmlElement(IsNullable = false)]
        public bool RequiresRestart { get; set; }

        /// <summary>
        /// Indicates if the old version should be removed (deleted) from the client. 
        /// </summary>
        /// <remarks>
        /// If true, the removal/deletion of the previously deployed version of the add-in would 
        /// (including all of its supporting files) will be performed once the update is complete. <br/> <br/>
        /// Xml Node Type: Element <br/>
        /// Xml Required: Y 
        /// </remarks>
        [XmlElement(IsNullable = false)]
        public bool RemoveDeprecatedVersion { get; set; }

        /// <summary>
        /// Specifies whether or not update notifications should be displayed to the client. 
        /// </summary>
        /// <remarks>
        /// If <see cref="P:RequiresRestart"/> is set to true, then this property will 
        /// be ignored and the client will be notified regardless. <br/> <br/>
        /// Xml Node Type: Element <br/>
        /// Xml Required: Y 
        /// </remarks>
        [XmlElement(IsNullable = false)]
        public bool NotifyClient { get; set; }

        /// <summary>
        /// Specifies whether the updates are pushed to the client machine in realtime.  
        /// </summary>
        /// <remarks>
        /// When set to true, updates will be pushed to the client as they occur on the remote 
        /// host. This only applies to remote hosts of type <see cref="FileHost.HostType"/> 
        /// equal to <see cref="FileHostType.FileServer"/>  <br/> <br/>
        /// Xml Node Type: Element <br/>
        /// Xml Required: Y 
        /// </remarks>
        [XmlElement(IsNullable = false)]
        public bool DoInRealTime { get; set; }

        /// <summary>
        /// Represents how old the current update can become before the application will perform 
        /// the next update check. 
        /// </summary> 
        /// <remarks>
        /// If <see cref="UpdateBehavior.DoInRealTime"/> is set to true, then the value of this 
        /// property will be ignored. 
        /// Xml Node Type: Element <br/>
        /// Xml Required: N
        /// </remarks>
        [XmlElement("UpdateExpiration", typeof(UpdateExpiration), IsNullable = false)]
        public UpdateExpiration Expiration { get; set; }
    }

    /// <summary>
    /// Represents how update notifications are displayed to the client.
    /// </summary>
    [Serializable]
    public enum UpdateMode
    {
        [XmlEnum("normal")]
        Normal,
        [XmlEnum("forced")]
        Forced
    }
}
