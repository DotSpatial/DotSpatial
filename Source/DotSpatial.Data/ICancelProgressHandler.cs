// ********************************************************************************************************
// Product Name: DotSpatial.Tools.ICancelProgressHandler
// Description:  Interface for tools for the DotSpatial toolbox
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

namespace DotSpatial.Data
{
    /// <summary>
    /// IProgressHandler that carries a boolean property allowing the process using the handler to know if you should cancelled
    /// </summary>
    public interface ICancelProgressHandler : IProgressHandler
    {
        /// <summary>
        /// Returns true if the progress handler has been notified that the running process should be cancelled
        /// </summary>
        bool Cancel
        {
            get;
        }
    }
}