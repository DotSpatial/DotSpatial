// ********************************************************************************************************
// Product Name: DotSpatial.Data.dll
// Description:  The data access libraries for the DotSpatial project.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created Before 2010.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
// |      Name            |    Date     |                                Comments
// |----------------------|-------------|-----------------------------------------------------------------
// |   Ted Dunsford       |  6/30/2010  |  Moved to DotSpatial
// ********************************************************************************************************

namespace DotSpatial.Data
{
    /// <summary>
    /// An interface for sending progress messages. Percent is an integer from 0 to 100.
    /// </summary>
    public interface IProgressHandler
    {
        /// <summary>
        /// Progress is the method that should receive a progress message.
        /// </summary>
        /// <param name="key">The message string without any information about the status of completion.</param>
        /// <param name="percent">An integer from 0 to 100 that indicates the condition for a status bar etc.</param>
        /// <param name="message">A string containing both information on what the process is, as well as its completion status.</param>
        void Progress(string key, int percent, string message);
    }
}