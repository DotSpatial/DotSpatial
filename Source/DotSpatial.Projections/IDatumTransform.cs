// ********************************************************************************************************
// Product Name: DotSpatial.Projections
// Description:  The basic module for MapWindow version 6.0
// ********************************************************************************************************
//
//
// The Initial Developer of this Original Code is Steve Riddell. Created 5/27/2011 1:04:00 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//        Name         |    Date    |        Comment
// --------------------|------------|------------------------------------------------------------
// ********************************************************************************************************
namespace DotSpatial.Projections
{
    /// <summary>
    /// IDatumTransform
    /// </summary>
    public interface IDatumTransform
    {
        /// <summary>
        /// Transform function
        /// </summary>
        /// <param name="source"></param>
        /// <param name="dest"></param>
        /// <param name="xy"></param>
        /// <param name="z"></param>
        /// <param name="startIndex"></param>
        /// <param name="numPoints"></param>
        void Transform(ProjectionInfo source, ProjectionInfo dest, double[] xy, double[] z, int startIndex, int numPoints);
    }
}