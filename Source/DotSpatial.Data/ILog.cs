// ********************************************************************************************************
// Product Name: DotSpatial.Data.dll
// Description:  The data access libraries for the DotSpatial project.
//
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/25/2008 4:52:09 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

namespace DotSpatial.Data
{
    /// <summary>
    /// LogException
    /// </summary>
    public interface ILog
    {
        /// <summary>
        /// Actually logs the exception.  This happens after the message has been set in the constructors of the classes of this exception.
        /// </summary>
        void Log();
    }
}