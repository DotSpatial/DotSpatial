// ********************************************************************************************************
// Product Name: DotSpatial.Projection
// Description:  The basic module for MapWindow version 6.0
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/14/2009 4:26:29 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//        Name         |    Date    |        Comment
// --------------------|------------|------------------------------------------------------------
// Ted Dunsford        |   5/3/2010 |  Updated project to DotSpatial.Projection and license to LGPL
// ********************************************************************************************************

#pragma warning disable 1591

namespace DotSpatial.Projections.ProjectedCategories.Continental
{
    /// <summary>
    /// This class contains predefined CoordinateSystems for SouthAmerica.
    /// </summary>
    public class SouthAmerica : CoordinateSystemCategory
    {
        #region Private Variables
        // ReSharper disable InconsistentNaming
        public readonly ProjectionInfo SouthAmericaAlbersEqualAreaConic;
        public readonly ProjectionInfo SouthAmericaEquidistantConic;
        public readonly ProjectionInfo SouthAmericaLambertConformalConic;
        // ReSharper restore InconsistentNaming
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of SouthAmerica.
        /// </summary>
        public SouthAmerica()
        {
            SouthAmericaAlbersEqualAreaConic = ProjectionInfo.FromAuthorityCode("ESRI", 102033).SetNames("South_America_Albers_Equal_Area_Conic", "GCS_South_American_1969", "D_South_American_1969");
            SouthAmericaEquidistantConic = ProjectionInfo.FromAuthorityCode("ESRI", 102032).SetNames("South_America_Equidistant_Conic", "GCS_South_American_1969", "D_South_American_1969");
            SouthAmericaLambertConformalConic = ProjectionInfo.FromAuthorityCode("ESRI", 102015).SetNames("South_America_Lambert_Conformal_Conic", "GCS_South_American_1969", "D_South_American_1969");
        }

        #endregion
    }
}

#pragma warning restore 1591