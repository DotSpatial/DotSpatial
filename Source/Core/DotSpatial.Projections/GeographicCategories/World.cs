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
    /// World
    /// </summary>
    public class World : CoordinateSystemCategory
    {
        #region Private Variables

        public readonly ProjectionInfo GRS1980;
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
        public readonly ProjectionInfo NSWC9Z2;
        public readonly ProjectionInfo WGS1966;
        public readonly ProjectionInfo WGS1972;
        public readonly ProjectionInfo WGS1972TBE;
        public readonly ProjectionInfo WGS1984;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of World
        /// </summary>
        public World()
        {
            GRS1980 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=GRS80 +no_defs ");
            GRS1980.Name = "GCS_GRS_1980";
            GRS1980.GeographicInfo.Name = "GCS_GRS_1980";
            GRS1980.GeographicInfo.Datum.Name = "D_GRS_1980";

            ITRF1988 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=GRS80 +no_defs ");
            ITRF1988.Name = "GCS_ITRF_1988";
            ITRF1988.GeographicInfo.Name = "GCS_ITRF_1988";
            ITRF1988.GeographicInfo.Datum.Name = "D_ITRF_1988";

            ITRF1989 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=GRS80 +no_defs ");
            ITRF1989.Name = "GCS_ITRF_1989";
            ITRF1989.GeographicInfo.Name = "GCS_ITRF_1989";
            ITRF1989.GeographicInfo.Datum.Name = "D_ITRF_1989";

            ITRF1990 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=GRS80 +no_defs ");
            ITRF1990.Name = "GCS_ITRF_1990";
            ITRF1990.GeographicInfo.Name = "GCS_ITRF_1990";
            ITRF1990.GeographicInfo.Datum.Name = "D_ITRF_1990";

            ITRF1991 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=GRS80 +no_defs ");
            ITRF1991.Name = "GCS_ITRF_1991";
            ITRF1991.GeographicInfo.Name = "GCS_ITRF_1991";
            ITRF1991.GeographicInfo.Datum.Name = "D_ITRF_1991";

            ITRF1992 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=GRS80 +no_defs ");
            ITRF1992.Name = "GCS_ITRF_1992";
            ITRF1992.GeographicInfo.Name = "GCS_ITRF_1992";
            ITRF1992.GeographicInfo.Datum.Name = "D_ITRF_1992";

            ITRF1993 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=GRS80 +no_defs ");
            ITRF1993.Name = "GCS_ITRF_1993";
            ITRF1993.GeographicInfo.Name = "GCS_ITRF_1993";
            ITRF1993.GeographicInfo.Datum.Name = "D_ITRF_1993";

            ITRF1994 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=GRS80 +no_defs ");
            ITRF1994.Name = "GCS_ITRF_1994";
            ITRF1994.GeographicInfo.Name = "GCS_ITRF_1994";
            ITRF1994.GeographicInfo.Datum.Name = "D_ITRF_1994";

            ITRF1996 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=GRS80 +no_defs ");
            ITRF1996.Name = "GCS_ITRF_1996";
            ITRF1996.GeographicInfo.Name = "GCS_ITRF_1996";
            ITRF1996.GeographicInfo.Datum.Name = "D_ITRF_1996";

            ITRF1997 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=GRS80 +no_defs ");
            ITRF1997.Name = "GCS_ITRF_1997";
            ITRF1997.GeographicInfo.Name = "GCS_ITRF_1997";
            ITRF1997.GeographicInfo.Datum.Name = "D_ITRF_1997";

            ITRF2000 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=GRS80 +no_defs ");
            ITRF2000.Name = "GCS_ITRF_2000";
            ITRF2000.GeographicInfo.Name = "GCS_ITRF_2000";
            ITRF2000.GeographicInfo.Datum.Name = "D_ITRF_2000";

            NSWC9Z2 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=WGS66 +no_defs ");
            NSWC9Z2.Name = "GCS_NSWC_9Z_2";
            NSWC9Z2.GeographicInfo.Name = "GCS_NSWC_9Z_2";
            NSWC9Z2.GeographicInfo.Datum.Name = "D_NSWC_9Z_2";

            WGS1966 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=WGS66 +no_defs ");
            WGS1966.Name = "GCS_WGS_1966";
            WGS1966.GeographicInfo.Name = "GCS_WGS_1966";
            WGS1966.GeographicInfo.Datum.Name = "D_WGS_1996";

            WGS1972 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=WGS72 +no_defs ");
            WGS1972.Name = "GCS_WGS_1972";
            WGS1972.GeographicInfo.Name = "GCS_WGS_1972";
            WGS1972.GeographicInfo.Datum.Name = "D_WGS_1972";

            WGS1972TBE = ProjectionInfo.FromProj4String("+proj=longlat +ellps=WGS72 +no_defs ");
            WGS1972TBE.Name = "GCS_WGS_1972_BE";
            WGS1972TBE.GeographicInfo.Name = "GCS_WGS_1972_BE";
            WGS1972TBE.GeographicInfo.Datum.Name = "D_WGS_1972_BE";

            WGS1984 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=WGS84 +datum=WGS84 +no_defs ");
            WGS1984.Name = "GCS_WGS_1984";
            WGS1984.GeographicInfo.Name = "GCS_WGS_1984";
            WGS1984.GeographicInfo.Datum.Name = "D_WGS_1984";
        }

        #endregion
    }
}

#pragma warning restore 1591
