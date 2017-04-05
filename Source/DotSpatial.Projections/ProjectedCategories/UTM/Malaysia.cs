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
    /// This class contains predefined CoordinateSystems for Malaysia.
    /// </summary>
    public class Malaysia : CoordinateSystemCategory
    {
        #region Private Variables
        // ReSharper disable InconsistentNaming
        public readonly ProjectionInfo KertauUTMZone47N;
        public readonly ProjectionInfo KertauUTMZone48N;
        public readonly ProjectionInfo Timbalai1948UTMZone49N;
        public readonly ProjectionInfo Timbalai1948UTMZone50N;
        // ReSharper restore InconsistentNaming
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of Malaysia.
        /// </summary>
        public Malaysia()
        {
            KertauUTMZone47N = ProjectionInfo.FromEpsgCode(24547).SetNames("Kertau_UTM_Zone_47N", "GCS_Kertau", "D_Kertau");
            KertauUTMZone48N = ProjectionInfo.FromEpsgCode(24548).SetNames("Kertau_UTM_Zone_48N", "GCS_Kertau", "D_Kertau");
            Timbalai1948UTMZone49N = ProjectionInfo.FromEpsgCode(29849).SetNames("Timbalai_1948_UTM_Zone_49N", "GCS_Timbalai_1948", "D_Timbalai_1948");
            Timbalai1948UTMZone50N = ProjectionInfo.FromEpsgCode(29850).SetNames("Timbalai_1948_UTM_Zone_50N", "GCS_Timbalai_1948", "D_Timbalai_1948");
        }

        #endregion
    }
}

#pragma warning restore 1591