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
    public interface IDatumTransform
    {
        /// <summary>
        /// Transform function
        /// </summary>
        void Transform(ProjectionInfo source, ProjectionInfo dest, double[] xy, double[] z, int startIndex, int numPoints);
    }
}