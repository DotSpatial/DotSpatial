// ********************************************************************************************************
// Product Name: DotSpatial.Projection
// Description:  This Interface defines how reprojection code should be called.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 3/11/2009 4:46:40 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//        Name         |    Date    |        Comment
// --------------------|------------|------------------------------------------------------------
// Ted Dunsford        |   5/3/2010 |  Updated project to DotSpatial.Projection and license to LGPL
// ********************************************************************************************************

namespace DotSpatial.Projections
{
    /// <summary>
    /// This interface defines how reprojection classes should be accessed
    /// </summary>
    public interface IReproject
    {
        /// <summary>
        /// Reprojects the specified points.  The first is the projection info to start from, while the destination
        /// is the projection to end with.
        /// </summary>
        /// <param name="points"></param>
        /// <param name="source"></param>
        /// <param name="dest"></param>
        /// <param name="startIndex"></param>
        /// <param name="numPoints"></param>
        void ReprojectPoints(double[][] points, ProjectionInfo source, ProjectionInfo dest, int startIndex,
                                   int numPoints);
    }
}