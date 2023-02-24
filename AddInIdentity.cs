using System;
using System.ComponentModel;
using System.Xml;
using System.Xml.Serialization;

namespace XLAutoDeploy.Manifests
{
    /// <summary>
    /// Represents information that identifies an Excel add-in.
    /// </summary> 
    public sealed class AddInIdentity
    {
        public AddInIdentity() { }

        /// <summary>
        /// Identifies the title of the application. 
        /// </summary>  
        /// <remarks>
        /// This is used by com interop methods to index the Microsoft.Office.Interop.Addins 
        /// collection. <br/> <br/>
        /// Xml Node Type: Attribute <br/>
        /// Xml Required: Y
        /// </remarks> 
        [XmlAttribute]
        public string Title { get; set; }

        /// <summary>
        /// Identifies the human-readable name of the application. 
        /// </summary>  
        /// <remarks>
        /// Cannot contain non-alpha-numeric characters. <br/> <br/>
        /// Xml Node Type: Attribute <br/>
        /// Xml Required: Y
        /// </remarks>  
        [XmlAttribute]
        public string Name { get; set; }

        /// <summary>
        /// Identifies the type of Excel Add-in.
        /// </summary>  
        /// <remarks>
        /// Xml Node Type: Element <br/>
        /// Xml Required: N 
        /// </remarks>
        [XmlElement("AddInType", typeof(AddInType), IsNullable = false)]
        public AddInType AddInType { get; set; }

        /// <summary>
        /// Identifies the file extension of the application. 
        /// </summary>  
        /// <remarks>
        /// Cannot contain non-alpha-numeric characters. <br/> <br/>
        /// Xml Node Type: Element <br/>
        /// Xml Required: Y
        /// </remarks>  
        [XmlElement("FileExtension", typeof(AddInFileExtensionType), IsNullable = false)]
        public AddInFileExtensionType FileExtension { get; set; }


        /// <summary>
        /// Specifies the version number of the add-in.
        /// </summary>  
        /// <remarks>
        /// To trigger an application update, this value must be incremented in an updated 
        /// <see cref="AddIn"/> manifest. <br/> <br/>
        /// Xml Node Type: Element <br/>
        /// Xml Required: Y
        /// </remarks> 
        //using "shim" property b/c System.Version is not serializable
        [XmlIgnore]
        public System.Version Version { get; set; }

        [XmlElement("Version", typeof(string), IsNullable = false)]
        [EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
        public string VersionString
        {
            get
            {
                return Version?.ToString() ?? String.Empty;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                    Version = new Version(value);
            }
        }
    }

    /// <summary>
    /// Types of Excel add-ins that can be created.
    /// </summary> 
    [Serializable]
    public enum AddInType
    {
        /// <summary>
        /// An .xla or .xlam file that contains functions/procedures (written in VBA) to extend 
        /// Excel's functionality.
        /// </summary>  
        /// <remarks>
        /// By defaul the only references/dependencies for this type of add-in are self contained; 
        /// however, the add-in can utilize COM-Visible assemblys (.dll(s)) that are called by  
        /// VBA. 
        /// </remarks> 
        [XmlEnum("vba")]
        Vba = 1,

        /// <summary>
        /// An .xll file (written in .Net) using the Excel-DNA project/library.
        /// </summary>  
        /// <remarks>
        /// <see href="https://excel-dna.net/"/>
        /// </remarks> 
        [XmlEnum("exceldna")]
        ExcelDna = 2,

        /// <summary>
        /// An assembly (.dll) that exposes .Net functions to Excel as UDFs.
        /// </summary>  
        /// <remarks>
        /// This type does NOT interact with Excel Objects (via COM interop) by default. These 
        /// functions should be visible in the excel function wizard. <br/> <br/>
        /// See: <see href="https://support.microsoft.com/en-us/topic/excel-com-add-ins-and-automation-add-ins-91f5ff06-0c9c-b98e-06e9-3657964eec72"/> <br/>
        ///      <see href="https://stackoverflow.com/a/2001275/9743237"/>
        /// </remarks> 
        [XmlEnum("automation")]
        Automation = 3,

        /// <summary>
        /// An assembly (.dll) that uses COM interop to in (using .Net) to interact with the Excel Application.
        /// </summary>  
        /// <remarks>
        /// This type is typically used to automate Excel in response to events such as the click 
        /// of a CommandBar button, a form/dialog box, or other. The functions within a COM Add-in 
        /// cannot be called from cell formulas in worksheets. <br/> <br/>
        /// See: <see href="https://support.microsoft.com/en-us/topic/excel-com-add-ins-and-automation-add-ins-91f5ff06-0c9c-b98e-06e9-3657964eec72"/> <br/>
        ///      <see href="https://stackoverflow.com/a/2001275/9743237"/>
        /// </remarks> 
        [XmlEnum("com")]
        Com = 4
    }

    /// <summary>
    /// File extensions available for Excel add-ins.
    /// </summary> 
    [Serializable]
    public enum AddInFileExtensionType
    {
        /// <summary>
        /// A .xlam file must be of type <see cref="AddInType.Vba"/>.
        /// </summary>  
        [XmlEnum("xlam")]
        Xlam = 1,

        /// <summary>
        /// A .xla file must be of type <see cref="AddInType.Vba"/>.
        /// </summary>  
        [XmlEnum("xla")]
        Xla = 2,

        /// <summary>
        /// A .xll file can be any type except <see cref="AddInType.Vba"/>.
        /// </summary>  
        [XmlEnum("xll")]
        Xll = 3,

        /// <summary>
        /// A .dll file must be of type <see cref="AddInType.Automation"/> or 
        /// <see cref="AddInType.Com"/>.
        /// </summary>  
        [XmlEnum("dll")]
        Dll = 4
    }
}
