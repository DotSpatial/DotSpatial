// ********************************************************************************************************
// Product Name: DotSpatial.Tools.Enums
// Description:  An Enumeration defining all of the parameter types which can be passed back from a ITool
//
// ********************************************************************************************************
//
// The Original Code is Toolbox.dll for the DotSpatial 4.6/6 ToolManager project
//
// The Initial Developer of this Original Code is Brian Marchionni. Created in Oct, 2008.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

namespace DotSpatial.Modeling.Forms
{
    /// <summary>
    /// Indicates the allowed values for the status of the element, illustrated by the light
    /// </summary>
    public enum ToolStatus
    {
        /// <summary>
        /// Indicates that no value has been set for this yet.
        /// </summary>
        Empty,

        /// <summary>
        /// Indicates that the element parameter is ok and won't halt.
        /// </summary>
        Ok,

        /// <summary>
        /// Indicates that the element value will cause an error.
        /// </summary>
        Error,
    }
}