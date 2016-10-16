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
    /// The Extent interface for Z dimension extent bounds.
    /// </summary>
    public interface IExtentZ
    {
        /// <summary>
        /// The minimum in the Z dimension (usually the bottom)
        /// </summary>
        double MinZ { get; set; }

        /// <summary>
        /// The maximum in the Z dimension (usually the top)
        /// </summary>
        double MaxZ { get; set; }
    }
}