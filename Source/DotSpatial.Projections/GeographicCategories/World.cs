// ********************************************************************************************************
// Product Name: DotSpatial.Projection
// Description:  The basic module for MapWindow version 6.0
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/14/2009 4:21:38 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//        Name         |    Date    |        Comment
// --------------------|------------|------------------------------------------------------------
// Ted Dunsford        |   5/3/2010 |  Updated project to DotSpatial.Projection and license to LGPL
// ********************************************************************************************************

#pragma warning disable 1591

namespace DotSpatial.Projections.GeographicCategories
{
    /// <summary>
    /// This class contains predefined CoordinateSystems for World.
    /// </summary>
    public class World : CoordinateSystemCategory
    {
        #region Private Variables
        // ReSharper disable InconsistentNaming
        public readonly ProjectionInfo ITRF1988;
        public readonly ProjectionInfo ITRF1989;
        public readonly ProjectionInfo ITRF1990;
        public readonly ProjectionInfo ITRF1991;
        public readonly ProjectionInfo ITRF1992;
        public readonly ProjectionInfo ITRF1993;
        public readonly ProjectionInfo ITRF1994;
        public readonly ProjectionInfo ITRF1996;
        public readonly ProjectionInfo ITRF1997;
        public readonly ProjectionInfo ITRF2000;
        public readonly ProjectionInfo ITRF2005;
        public readonly ProjectionInfo NSWC9Z2;
        public readonly ProjectionInfo WGS1966;
        public readonly ProjectionInfo WGS1972;
        public readonly ProjectionInfo WGS1972TBE;
        public readonly ProjectionInfo WGS1984;
        // ReSharper restore InconsistentNaming
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of World.
        /// </summary>
        public World()
        {
            ITRF1988 = ProjectionInfo.FromAuthorityCode("ESRI", 104115).SetNames("", "GCS_ITRF_1988", "D_ITRF_1988"); // missing
            ITRF1989 = ProjectionInfo.FromAuthorityCode("ESRI", 104116).SetNames("", "GCS_ITRF_1989", "D_ITRF_1989"); // missing
            ITRF1990 = ProjectionInfo.FromAuthorityCode("ESRI", 104117).SetNames("", "GCS_ITRF_1990", "D_ITRF_1990"); // missing
            ITRF1991 = ProjectionInfo.FromAuthorityCode("ESRI", 104118).SetNames("", "GCS_ITRF_1991", "D_ITRF_1991"); // missing
            ITRF1992 = ProjectionInfo.FromAuthorityCode("ESRI", 104119).SetNames("", "GCS_ITRF_1992", "D_ITRF_1992"); // missing
            ITRF1993 = ProjectionInfo.FromAuthorityCode("ESRI", 104120).SetNames("", "GCS_ITRF_1993", "D_ITRF_1993"); // missing
            ITRF1994 = ProjectionInfo.FromAuthorityCode("ESRI", 104121).SetNames("", "GCS_ITRF_1994", "D_ITRF_1994"); // missing
            ITRF1996 = ProjectionInfo.FromAuthorityCode("ESRI", 104122).SetNames("", "GCS_ITRF_1996", "D_ITRF_1996"); // missing
            ITRF1997 = ProjectionInfo.FromAuthorityCode("ESRI", 104123).SetNames("", "GCS_ITRF_1997", "D_ITRF_1997"); // missing
            ITRF2000 = ProjectionInfo.FromAuthorityCode("ESRI", 104124).SetNames("", "GCS_ITRF_2000", "D_ITRF_2000"); // missing
            ITRF2005 = ProjectionInfo.FromAuthorityCode("ESRI", 104896).SetNames("", "GCS_ITRF_2005", "D_ITRF_2005"); // missing
            NSWC9Z2 = ProjectionInfo.FromEpsgCode(4276).SetNames("", "GCS_NSWC_9Z_2", "D_NSWC_9Z_2");
            WGS1966 = ProjectionInfo.FromEpsgCode(4760).SetNames("", "GCS_WGS_1966", "D_WGS_1966");
            WGS1972 = ProjectionInfo.FromEpsgCode(4322).SetNames("", "GCS_WGS_1972", "D_WGS_1972");
            WGS1972TBE = ProjectionInfo.FromEpsgCode(4324).SetNames("", "GCS_WGS_1972_BE", "D_WGS_1972_BE");
            WGS1984 = ProjectionInfo.FromEpsgCode(4326).SetNames("", "GCS_WGS_1984", "D_WGS_1984");
        }

        #endregion
    }
}

#pragma warning restore 1591