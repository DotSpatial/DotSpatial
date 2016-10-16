// ********************************************************************************************************
// Product Name: DotSpatial.Data.dll
// Description:  The data access libraries for the DotSpatial project.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 9/05/2010 10:36:12 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

namespace DotSpatial.Data
{
    /// <summary>
    /// Controls whether only intersecting shapes should be used or whether all shapes should be used.
    /// </summary>
    public enum ShapeRelateType
    {
        /// <summary>
        /// All shapes will be used
        /// </summary>
        All,
        /// <summary>
        /// Only intersecting shapes will be used
        /// </summary>
        Intersecting
    }
}