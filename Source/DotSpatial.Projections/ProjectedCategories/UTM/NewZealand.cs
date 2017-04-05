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

namespace DotSpatial.Projections.ProjectedCategories.UTM
{
    /// <summary>
    /// This class contains predefined CoordinateSystems for NewZealand.
    /// </summary>
    public class NewZealand : CoordinateSystemCategory
    {
        #region Private Variables
        // ReSharper disable InconsistentNaming
        public readonly ProjectionInfo NZGD1949UTMZone58S;
        public readonly ProjectionInfo NZGD1949UTMZone59S;
        public readonly ProjectionInfo NZGD1949UTMZone60S;
        public readonly ProjectionInfo NZGD2000UTMZone58S;
        public readonly ProjectionInfo NZGD2000UTMZone59S;
        public readonly ProjectionInfo NZGD2000UTMZone60S;
        // ReSharper restore InconsistentNaming
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of NewZealand.
        /// </summary>
        public NewZealand()
        {
            NZGD1949UTMZone58S = ProjectionInfo.FromEpsgCode(27258).SetNames("NZGD_1949_UTM_Zone_58S", "GCS_New_Zealand_1949", "D_New_Zealand_1949");
            NZGD1949UTMZone59S = ProjectionInfo.FromEpsgCode(27259).SetNames("NZGD_1949_UTM_Zone_59S", "GCS_New_Zealand_1949", "D_New_Zealand_1949");
            NZGD1949UTMZone60S = ProjectionInfo.FromEpsgCode(27260).SetNames("NZGD_1949_UTM_Zone_60S", "GCS_New_Zealand_1949", "D_New_Zealand_1949");
            NZGD2000UTMZone58S = ProjectionInfo.FromEpsgCode(2133).SetNames("NZGD_2000_UTM_Zone_58S", "GCS_NZGD_2000", "D_NZGD_2000");
            NZGD2000UTMZone59S = ProjectionInfo.FromEpsgCode(2134).SetNames("NZGD_2000_UTM_Zone_59S", "GCS_NZGD_2000", "D_NZGD_2000");
            NZGD2000UTMZone60S = ProjectionInfo.FromEpsgCode(2135).SetNames("NZGD_2000_UTM_Zone_60S", "GCS_NZGD_2000", "D_NZGD_2000");
        }

        #endregion
    }
}

#pragma warning restore 1591