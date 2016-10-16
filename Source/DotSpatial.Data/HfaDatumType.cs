// ********************************************************************************************************
// Product Name: DotSpatial.Data.dll
// Description:  The data access libraries for the DotSpatial project.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/24/2010 2:52:18 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

namespace DotSpatial.Data
{
    /// <summary>
    /// HfaDatumType
    /// </summary>
    public enum HfaDatumType
    {
        /// <summary>
        /// The datum info is 7 doubles
        /// </summary>
        Parametric,
        /// <summary>
        /// The datum info is a name
        /// </summary>
        Grid,
        /// <summary>
        ///
        /// </summary>
        Regression,
        /// <summary>
        /// No Datum info
        /// </summary>
        None
    }
}