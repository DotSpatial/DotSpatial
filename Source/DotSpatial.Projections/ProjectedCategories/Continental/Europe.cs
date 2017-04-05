// ********************************************************************************************************
// Product Name: DotSpatial.Projection
// Description:  The basic module for MapWindow version 6.0
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/14/2009 4:24:27 PM
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
    /// This class contains predefined CoordinateSystems for Europe.
    /// </summary>
    public class Europe : CoordinateSystemCategory
    {
        #region Private Variables
        // ReSharper disable InconsistentNaming
        public readonly ProjectionInfo EMEP150KilometerGrid;
        public readonly ProjectionInfo EMEP50KilometerGrid;
        public readonly ProjectionInfo ETRS1989LAEA;
        public readonly ProjectionInfo ETRS1989LCC;
        public readonly ProjectionInfo EuropeAlbersEqualAreaConic;
        public readonly ProjectionInfo EuropeEquidistantConic;
        public readonly ProjectionInfo EuropeLambertConformalConic;
        // ReSharper restore InconsistentNaming
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of Europe.
        /// </summary>
        public Europe()
        {
            EMEP150KilometerGrid = ProjectionInfo.FromAuthorityCode("ESRI", 102069).SetNames("EMEP_150_Kilometer_Grid", "GCS_Sphere_EMEP", "D_Sphere_EMEP"); // missing
            EMEP50KilometerGrid = ProjectionInfo.FromAuthorityCode("ESRI", 102068).SetNames("EMEP_50_Kilometer_Grid", "GCS_Sphere_EMEP", "D_Sphere_EMEP"); // missing
            ETRS1989LAEA = ProjectionInfo.FromEpsgCode(3035).SetNames("ETRS_1989_LAEA", "GCS_ETRS_1989", "D_ETRS_1989");
            ETRS1989LCC = ProjectionInfo.FromEpsgCode(3034).SetNames("ETRS_1989_LCC", "GCS_ETRS_1989", "D_ETRS_1989");
            EuropeAlbersEqualAreaConic = ProjectionInfo.FromAuthorityCode("ESRI", 102013).SetNames("Europe_Albers_Equal_Area_Conic", "GCS_European_1950", "D_European_1950");
            EuropeEquidistantConic = ProjectionInfo.FromAuthorityCode("ESRI", 102031).SetNames("Europe_Equidistant_Conic", "GCS_European_1950", "D_European_1950");
            EuropeLambertConformalConic = ProjectionInfo.FromAuthorityCode("ESRI", 102014).SetNames("Europe_Lambert_Conformal_Conic", "GCS_European_1950", "D_European_1950");
        }

        #endregion
    }
}

#pragma warning restore 1591