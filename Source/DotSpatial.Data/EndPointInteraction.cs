// ********************************************************************************************************
// Product Name: DotSpatial.Data.dll
// Description:  The data access libraries for the DotSpatial project.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Brian Marchionni. Created 3/18/2010
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

namespace DotSpatial.Data
{
    /// <summary>
    /// Used by Segment.ClosestPointTo() to detail how the input point interacts with the line segment
    /// </summary>
    public enum EndPointInteraction
    {
        /// <summary>
        /// The vertex found is on the line segment and between P1 and P2
        /// </summary>
        OnLine,
        /// <summary>
        /// The vertex found is beyond the end of P1
        /// </summary>
        PastP1,
        /// <summary>
        /// The vertex found is beyond the end of P2
        /// </summary>
        PastP2,
        /// <summary>
        /// P1 equals P2 so the segment cannot be extended into an infinite line the closest vertex is P1/P2
        /// </summary>
        P1equalsP2
    }
}