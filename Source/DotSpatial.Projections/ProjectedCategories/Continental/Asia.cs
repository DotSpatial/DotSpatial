// ********************************************************************************************************
// Product Name: DotSpatial.Projection
// Description:  The basic module for MapWindow version 6.0
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/14/2009 4:23:38 PM
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
    /// This class contains predefined CoordinateSystems for Asia.
    /// </summary>
    public class Asia : CoordinateSystemCategory
    {
        #region Private Variables
        // ReSharper disable InconsistentNaming
        public readonly ProjectionInfo AsiaLambertConformalConic;
        public readonly ProjectionInfo AsiaNorthAlbersEqualAreaConic;
        public readonly ProjectionInfo AsiaNorthEquidistantConic;
        public readonly ProjectionInfo AsiaNorthLambertConformalConic;
        public readonly ProjectionInfo AsiaSouthAlbersEqualAreaConic;
        public readonly ProjectionInfo AsiaSouthEquidistantConic;
        public readonly ProjectionInfo AsiaSouthLambertConformalConic;
        // ReSharper restore InconsistentNaming
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of Asia.
        /// </summary>
        public Asia()
        {
            AsiaLambertConformalConic = ProjectionInfo.FromAuthorityCode("ESRI", 102012).SetNames("Asia_Lambert_Conformal_Conic", "GCS_WGS_1984", "D_WGS_1984");
            AsiaNorthAlbersEqualAreaConic = ProjectionInfo.FromAuthorityCode("ESRI", 102025).SetNames("Asia_North_Albers_Equal_Area_Conic", "GCS_WGS_1984", "D_WGS_1984");
            AsiaNorthEquidistantConic = ProjectionInfo.FromAuthorityCode("ESRI", 102026).SetNames("Asia_North_Equidistant_Conic", "GCS_WGS_1984", "D_WGS_1984");
            AsiaNorthLambertConformalConic = ProjectionInfo.FromAuthorityCode("ESRI", 102027).SetNames("Asia_North_Lambert_Conformal_Conic", "GCS_WGS_1984", "D_WGS_1984");
            AsiaSouthAlbersEqualAreaConic = ProjectionInfo.FromAuthorityCode("ESRI", 102028).SetNames("Asia_South_Albers_Equal_Area_Conic", "GCS_WGS_1984", "D_WGS_1984");
            AsiaSouthEquidistantConic = ProjectionInfo.FromAuthorityCode("ESRI", 102029).SetNames("Asia_South_Equidistant_Conic", "GCS_WGS_1984", "D_WGS_1984");
            AsiaSouthLambertConformalConic = ProjectionInfo.FromAuthorityCode("ESRI", 102030).SetNames("Asia_South_Lambert_Conformal_Conic", "GCS_WGS_1984", "D_WGS_1984");
        }

        #endregion
    }
}

#pragma warning restore 1591