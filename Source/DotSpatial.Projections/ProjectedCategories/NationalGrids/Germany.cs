// ********************************************************************************************************
// Product Name: DotSpatial.Projection
// Description:  The basic module for MapWindow version 6.0
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/14/2009 4:23:00 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//        Name         |    Date    |        Comment
// --------------------|------------|------------------------------------------------------------
// Ted Dunsford        |   5/3/2010 |  Updated project to DotSpatial.Projection and license to LGPL
// ********************************************************************************************************

#pragma warning disable 1591

namespace DotSpatial.Projections.ProjectedCategories.NationalGrids
{
    /// <summary>
    /// This class contains predefined CoordinateSystems for Germany.
    /// </summary>
    public class Germany : CoordinateSystemCategory
    {
        #region Private Variables
        // ReSharper disable InconsistentNaming
        public readonly ProjectionInfo DHDN3DegreeGaussZone1;
        public readonly ProjectionInfo DHDN3DegreeGaussZone2;
        public readonly ProjectionInfo DHDN3DegreeGaussZone3;
        public readonly ProjectionInfo DHDN3DegreeGaussZone4;
        public readonly ProjectionInfo DHDN3DegreeGaussZone5;
        public readonly ProjectionInfo DHDNSoldnerBerlin;
        public readonly ProjectionInfo ETRS1989UTMZoneN32;
        public readonly ProjectionInfo GermanyZone1;
        public readonly ProjectionInfo GermanyZone2;
        public readonly ProjectionInfo GermanyZone3;
        public readonly ProjectionInfo GermanyZone4;
        public readonly ProjectionInfo GermanyZone5;
        public readonly ProjectionInfo RD83GKZone4;
        public readonly ProjectionInfo RD83GKZone5;
        // ReSharper restore InconsistentNaming
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of Germany.
        /// </summary>
        public Germany()
        {
            DHDN3DegreeGaussZone1 = ProjectionInfo.FromEpsgCode(31461).SetNames("DHDN_3_Degree_Gauss_Zone_1", "GCS_Deutsches_Hauptdreiecksnetz", "D_Deutsches_Hauptdreiecksnetz");
            DHDN3DegreeGaussZone2 = ProjectionInfo.FromEpsgCode(31466).SetNames("DHDN_3_Degree_Gauss_Zone_2", "GCS_Deutsches_Hauptdreiecksnetz", "D_Deutsches_Hauptdreiecksnetz");
            DHDN3DegreeGaussZone3 = ProjectionInfo.FromEpsgCode(31467).SetNames("DHDN_3_Degree_Gauss_Zone_3", "GCS_Deutsches_Hauptdreiecksnetz", "D_Deutsches_Hauptdreiecksnetz");
            DHDN3DegreeGaussZone4 = ProjectionInfo.FromEpsgCode(31468).SetNames("DHDN_3_Degree_Gauss_Zone_4", "GCS_Deutsches_Hauptdreiecksnetz", "D_Deutsches_Hauptdreiecksnetz");
            DHDN3DegreeGaussZone5 = ProjectionInfo.FromEpsgCode(31469).SetNames("DHDN_3_Degree_Gauss_Zone_5", "GCS_Deutsches_Hauptdreiecksnetz", "D_Deutsches_Hauptdreiecksnetz");
            DHDNSoldnerBerlin = ProjectionInfo.FromEpsgCode(3068).SetNames("DHDN_Soldner_Berlin", "GCS_Deutsches_Hauptdreiecksnetz", "D_Deutsches_Hauptdreiecksnetz");
            ETRS1989UTMZoneN32 = ProjectionInfo.FromAuthorityCode("EPSG", 102362).SetNames("ETRS_1989_UTM_Zone_N32", "GCS_ETRS_1989", "D_ETRS_1989"); // missing
            GermanyZone1 = ProjectionInfo.FromAuthorityCode("EPSG", 31491).SetNames("Germany_Zone_1", "GCS_Deutsches_Hauptdreiecksnetz", "D_Deutsches_Hauptdreiecksnetz"); // missing
            GermanyZone2 = ProjectionInfo.FromAuthorityCode("EPSG", 31492).SetNames("Germany_Zone_2", "GCS_Deutsches_Hauptdreiecksnetz", "D_Deutsches_Hauptdreiecksnetz"); // missing
            GermanyZone3 = ProjectionInfo.FromAuthorityCode("EPSG", 31493).SetNames("Germany_Zone_3", "GCS_Deutsches_Hauptdreiecksnetz", "D_Deutsches_Hauptdreiecksnetz"); // missing
            GermanyZone4 = ProjectionInfo.FromAuthorityCode("EPSG", 31494).SetNames("Germany_Zone_4", "GCS_Deutsches_Hauptdreiecksnetz", "D_Deutsches_Hauptdreiecksnetz"); // missing
            GermanyZone5 = ProjectionInfo.FromAuthorityCode("EPSG", 31495).SetNames("Germany_Zone_5", "GCS_Deutsches_Hauptdreiecksnetz", "D_Deutsches_Hauptdreiecksnetz"); // missing
            RD83GKZone4 = ProjectionInfo.FromEpsgCode(3398).SetNames("RD/83_GK_Zone_4", "GCS_RD/83", "D_Rauenberg_1983");
            RD83GKZone5 = ProjectionInfo.FromEpsgCode(3399).SetNames("RD/83_GK_Zone_5", "GCS_RD/83", "D_Rauenberg_1983");
        }

        #endregion
    }
}

#pragma warning restore 1591