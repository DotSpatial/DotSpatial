// ********************************************************************************************************
// Product Name: DotSpatial.Projection
// Description:  The basic module for MapWindow version 6.0
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/14/2009 2:13:55 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//        Name         |    Date    |        Comment
// --------------------|------------|------------------------------------------------------------
// Ted Dunsford        |   5/3/2010 |  Updated project to DotSpatial.Projection and license to LGPL
// ********************************************************************************************************

#pragma warning disable 1591

namespace DotSpatial.Projections.ProjectedCategories
{
    /// <summary>
    /// SpheroidBased
    /// </summary>
    public class SpheroidBased : CoordinateSystemCategory
    {
        #region Fields

        /// <summary>
        /// Lambert 2 (Central France)
        /// </summary>
        public readonly ProjectionInfo Lambert2;

        /// <summary>
        /// Lambert 2 (Étendu)
        /// </summary>
        public readonly ProjectionInfo Lambert2Wide;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of SpheroidBased
        /// </summary>
        public SpheroidBased()
        {
            Lambert2 = ProjectionInfo.FromProj4String("+proj=lcc +lat_1=45.89893890000052 +lat_2=47.69601440000037 +lat_0=46.8 +lon_0=2.33722917 +x_0=600000 +y_0=200000 +ellps=clrk80 +units=m +no_defs");
            Lambert2Wide = ProjectionInfo.FromProj4String("+proj = lcc + lat_1 = 45.89891889999931 + lat_2 = 47.69601440000037 + lat_0 = 46.8 + lon_0 = 2.33722917 + x_0 = 600000 + y_0 = 2200000 + a = 6378249.145 + b = 6356514.96582849 + units = m + no_defs");
        }

        #endregion
    }
}

#pragma warning restore 1591