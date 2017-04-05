// ********************************************************************************************************
// Product Name: DotSpatial.Projection
// Description:  The basic module for MapWindow version 6.0
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/14/2009 4:25:11 PM
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
    /// This class contains predefined CoordinateSystems for NorthAmerica.
    /// </summary>
    public class NorthAmerica : CoordinateSystemCategory
    {
        #region Private Variables
        // ReSharper disable InconsistentNaming
        public readonly ProjectionInfo AlaskaAlbersEqualAreaConic;
        public readonly ProjectionInfo CanadaAlbersEqualAreaConic;
        public readonly ProjectionInfo CanadaLambertConformalConic;
        public readonly ProjectionInfo HawaiiAlbersEqualAreaConic;
        public readonly ProjectionInfo NAD1983CanadaAtlasLambert;
        public readonly ProjectionInfo NAD1983CSRSCanadaAtlasLambert;
        public readonly ProjectionInfo NAD1983GreatLakesBasinAlbers;
        public readonly ProjectionInfo NAD1983GreatLakesStLawrenceAlbers;
        public readonly ProjectionInfo NorthAmericaAlbersEqualAreaConic;
        public readonly ProjectionInfo NorthAmericaEquidistantConic;
        public readonly ProjectionInfo NorthAmericaLambertConformalConic;
        public readonly ProjectionInfo USAContiguousAlbersEqualAreaConic;
        public readonly ProjectionInfo USAContiguousAlbersEqualAreaConicUSGS;
        public readonly ProjectionInfo USAContiguousEquidistantConic;
        public readonly ProjectionInfo USAContiguousLambertConformalConic;
        public readonly ProjectionInfo USNationalAtlasEqualArea;
        public readonly ProjectionInfo WGS1984CanadaAtlasLCC;
        // ReSharper restore InconsistentNaming
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of NorthAmerica.
        /// </summary>
        public NorthAmerica()
        {
            AlaskaAlbersEqualAreaConic = ProjectionInfo.FromEpsgCode(3338).SetNames("NAD_1983_Alaska_Albers", "GCS_North_American_1983", "D_North_American_1983");
            CanadaAlbersEqualAreaConic = ProjectionInfo.FromAuthorityCode("ESRI", 102001).SetNames("Canada_Albers_Equal_Area_Conic", "GCS_North_American_1983", "D_North_American_1983");
            CanadaLambertConformalConic = ProjectionInfo.FromAuthorityCode("ESRI", 102002).SetNames("Canada_Lambert_Conformal_Conic", "GCS_North_American_1983", "D_North_American_1983");
            HawaiiAlbersEqualAreaConic = ProjectionInfo.FromAuthorityCode("ESRI", 102007).SetNames("Hawaii_Albers_Equal_Area_Conic", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983CanadaAtlasLambert = ProjectionInfo.FromEpsgCode(3978).SetNames("NAD_1983_Canada_Atlas_Lambert", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983CSRSCanadaAtlasLambert = ProjectionInfo.FromEpsgCode(3979).SetNames("NAD_1983_CSRS_Canada_Atlas_Lambert", "GCS_North_American_1983_CSRS", "D_North_American_1983_CSRS");
            NAD1983GreatLakesBasinAlbers = ProjectionInfo.FromEpsgCode(3174).SetNames("NAD_1983_Great_Lakes_Basin_Albers", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983GreatLakesStLawrenceAlbers = ProjectionInfo.FromEpsgCode(3175).SetNames("NAD_1983_Great_Lakes_and_St_Lawrence_Albers", "GCS_North_American_1983", "D_North_American_1983");
            NorthAmericaAlbersEqualAreaConic = ProjectionInfo.FromAuthorityCode("ESRI", 102008).SetNames("North_America_Albers_Equal_Area_Conic", "GCS_North_American_1983", "D_North_American_1983");
            NorthAmericaEquidistantConic = ProjectionInfo.FromAuthorityCode("ESRI", 102010).SetNames("North_America_Equidistant_Conic", "GCS_North_American_1983", "D_North_American_1983");
            NorthAmericaLambertConformalConic = ProjectionInfo.FromAuthorityCode("ESRI", 102009).SetNames("North_America_Lambert_Conformal_Conic", "GCS_North_American_1983", "D_North_American_1983");
            USAContiguousAlbersEqualAreaConic = ProjectionInfo.FromAuthorityCode("ESRI", 102003).SetNames("USA_Contiguous_Albers_Equal_Area_Conic", "GCS_North_American_1983", "D_North_American_1983");
            USAContiguousAlbersEqualAreaConicUSGS = ProjectionInfo.FromAuthorityCode("ESRI", 102039).SetNames("USA_Contiguous_Albers_Equal_Area_Conic_USGS_version", "GCS_North_American_1983", "D_North_American_1983"); // missing
            USAContiguousEquidistantConic = ProjectionInfo.FromAuthorityCode("ESRI", 102005).SetNames("USA_Contiguous_Equidistant_Conic", "GCS_North_American_1983", "D_North_American_1983");
            USAContiguousLambertConformalConic = ProjectionInfo.FromAuthorityCode("ESRI", 102004).SetNames("USA_Contiguous_Lambert_Conformal_Conic", "GCS_North_American_1983", "D_North_American_1983");
            USNationalAtlasEqualArea = ProjectionInfo.FromEpsgCode(2163).SetNames("US_National_Atlas_Equal_Area", "GCS_Sphere_Clarke_1866_Authalic", "D_Sphere_Clarke_1866_Authalic");
            WGS1984CanadaAtlasLCC = ProjectionInfo.FromAuthorityCode("ESRI", 102215).SetNames("WGS_1984_Canada_Atlas_LCC", "GCS_WGS_1984", "D_WGS_1984"); // missing
        }

        #endregion
    }
}

#pragma warning restore 1591