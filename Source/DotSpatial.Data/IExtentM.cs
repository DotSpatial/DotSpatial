// ********************************************************************************************************
// Product Name: DotSpatial.Data.dll
// Description:  The data access libraries for the DotSpatial project.
// ********************************************************************************************************
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 11/13/2010 9:30:48 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
// |      Name            |    Date     |                                Comments
// |----------------------|-------------|-----------------------------------------------------------------
// ********************************************************************************************************

namespace DotSpatial.Data
{
    /// <summary>
    /// Supports bounding in the M Dimension
    /// </summary>
    public interface IExtentM
    {
        /// <summary>
        /// Gets or sets the minimum M value
        /// </summary>
        double MinM { get; set; }

        /// <summary>
        /// Gets or sets the maximum M value
        /// </summary>
        double MaxM { get; set; }
    }
}