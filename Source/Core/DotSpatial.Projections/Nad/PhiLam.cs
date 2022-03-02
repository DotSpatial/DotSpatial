// ********************************************************************************************************
// Product Name: DotSpatial.Projection
// Description:  The basic module for MapWindow version 6.0
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/17/2009 1:48:45 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//        Name         |    Date    |        Comment
// --------------------|------------|------------------------------------------------------------
// Ted Dunsford        |   5/3/2010 |  Updated project to DotSpatial.Projection and license to LGPL
// ********************************************************************************************************

namespace DotSpatial.Projections.Nad
{
    /// <summary>
    /// PhiLam has two double values and is used like a coordinate.
    /// </summary>
    public struct PhiLam
    {
        /// <summary>
        /// Geodetic Lambda coordinate (longitude)
        /// </summary>
        public double Lambda;

        /// <summary>
        /// Geodetic Phi coordinate (latitude)
        /// </summary>
        public double Phi;
    }
}