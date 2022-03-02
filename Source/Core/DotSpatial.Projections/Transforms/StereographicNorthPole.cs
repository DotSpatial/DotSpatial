// ********************************************************************************************************
// Product Name: DotSpatial.Projection
// Description:  The basic module for MapWindow version 6.0
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 4/30/2010 10:01:46 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//        Name         |    Date    |        Comment
// --------------------|------------|------------------------------------------------------------
// Ted Dunsford        |   5/3/2010 |  Updated project to DotSpatial.Projection and license to LGPL
// ********************************************************************************************************

namespace DotSpatial.Projections.Transforms
{
    /// <summary>
    /// StereographicNorthPole
    /// </summary>
    public class StereographicNorthPole : Stereographic
    {
        #region Constructors

        /// <summary>
        /// Creates a new instance of StereographicNorthPole
        /// </summary>
        public StereographicNorthPole()
        {
            Name = "Stereographic_North_Pole";
        }

        #endregion
    }
}