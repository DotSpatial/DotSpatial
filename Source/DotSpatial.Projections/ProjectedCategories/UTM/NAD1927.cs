// ********************************************************************************************************
// Product Name: DotSpatial.Projection
// Description:  The basic module for MapWindow version 6.0
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/14/2009 5:07:15 PM
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
    /// This class contains predefined CoordinateSystems for NAD1927.
    /// </summary>
    public class NAD1927 : CoordinateSystemCategory
    {
        #region Private Variables
        // ReSharper disable InconsistentNaming
        public readonly ProjectionInfo NAD1927UTMZone10N;
        public readonly ProjectionInfo NAD1927UTMZone11N;
        public readonly ProjectionInfo NAD1927UTMZone12N;
        public readonly ProjectionInfo NAD1927UTMZone13N;
        public readonly ProjectionInfo NAD1927UTMZone14N;
        public readonly ProjectionInfo NAD1927UTMZone15N;
        public readonly ProjectionInfo NAD1927UTMZone16N;
        public readonly ProjectionInfo NAD1927UTMZone17N;
        public readonly ProjectionInfo NAD1927UTMZone18N;
        public readonly ProjectionInfo NAD1927UTMZone19N;
        public readonly ProjectionInfo NAD1927UTMZone1N;
        public readonly ProjectionInfo NAD1927UTMZone20N;
        public readonly ProjectionInfo NAD1927UTMZone21N;
        public readonly ProjectionInfo NAD1927UTMZone22N;
        public readonly ProjectionInfo NAD1927UTMZone2N;
        public readonly ProjectionInfo NAD1927UTMZone3N;
        public readonly ProjectionInfo NAD1927UTMZone4N;
        public readonly ProjectionInfo NAD1927UTMZone59N;
        public readonly ProjectionInfo NAD1927UTMZone5N;
        public readonly ProjectionInfo NAD1927UTMZone60N;
        public readonly ProjectionInfo NAD1927UTMZone6N;
        public readonly ProjectionInfo NAD1927UTMZone7N;
        public readonly ProjectionInfo NAD1927UTMZone8N;
        public readonly ProjectionInfo NAD1927UTMZone9N;
        // ReSharper restore InconsistentNaming
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of NAD1927.
        /// </summary>
        public NAD1927()
        {
            NAD1927UTMZone10N = ProjectionInfo.FromEpsgCode(26710).SetNames("NAD_1927_UTM_Zone_10N", "GCS_North_American_1927", "D_North_American_1927");
            NAD1927UTMZone11N = ProjectionInfo.FromEpsgCode(26711).SetNames("NAD_1927_UTM_Zone_11N", "GCS_North_American_1927", "D_North_American_1927");
            NAD1927UTMZone12N = ProjectionInfo.FromEpsgCode(26712).SetNames("NAD_1927_UTM_Zone_12N", "GCS_North_American_1927", "D_North_American_1927");
            NAD1927UTMZone13N = ProjectionInfo.FromEpsgCode(26713).SetNames("NAD_1927_UTM_Zone_13N", "GCS_North_American_1927", "D_North_American_1927");
            NAD1927UTMZone14N = ProjectionInfo.FromEpsgCode(26714).SetNames("NAD_1927_UTM_Zone_14N", "GCS_North_American_1927", "D_North_American_1927");
            NAD1927UTMZone15N = ProjectionInfo.FromEpsgCode(26715).SetNames("NAD_1927_UTM_Zone_15N", "GCS_North_American_1927", "D_North_American_1927");
            NAD1927UTMZone16N = ProjectionInfo.FromEpsgCode(26716).SetNames("NAD_1927_UTM_Zone_16N", "GCS_North_American_1927", "D_North_American_1927");
            NAD1927UTMZone17N = ProjectionInfo.FromEpsgCode(26717).SetNames("NAD_1927_UTM_Zone_17N", "GCS_North_American_1927", "D_North_American_1927");
            NAD1927UTMZone18N = ProjectionInfo.FromEpsgCode(26718).SetNames("NAD_1927_UTM_Zone_18N", "GCS_North_American_1927", "D_North_American_1927");
            NAD1927UTMZone19N = ProjectionInfo.FromEpsgCode(26719).SetNames("NAD_1927_UTM_Zone_19N", "GCS_North_American_1927", "D_North_American_1927");
            NAD1927UTMZone1N = ProjectionInfo.FromEpsgCode(26701).SetNames("NAD_1927_UTM_Zone_1N", "GCS_North_American_1927", "D_North_American_1927");
            NAD1927UTMZone20N = ProjectionInfo.FromEpsgCode(26720).SetNames("NAD_1927_UTM_Zone_20N", "GCS_North_American_1927", "D_North_American_1927");
            NAD1927UTMZone21N = ProjectionInfo.FromEpsgCode(26721).SetNames("NAD_1927_UTM_Zone_21N", "GCS_North_American_1927", "D_North_American_1927");
            NAD1927UTMZone22N = ProjectionInfo.FromEpsgCode(26722).SetNames("NAD_1927_UTM_Zone_22N", "GCS_North_American_1927", "D_North_American_1927");
            NAD1927UTMZone2N = ProjectionInfo.FromEpsgCode(26702).SetNames("NAD_1927_UTM_Zone_2N", "GCS_North_American_1927", "D_North_American_1927");
            NAD1927UTMZone3N = ProjectionInfo.FromEpsgCode(26703).SetNames("NAD_1927_UTM_Zone_3N", "GCS_North_American_1927", "D_North_American_1927");
            NAD1927UTMZone4N = ProjectionInfo.FromEpsgCode(26704).SetNames("NAD_1927_UTM_Zone_4N", "GCS_North_American_1927", "D_North_American_1927");
            NAD1927UTMZone59N = ProjectionInfo.FromEpsgCode(3370).SetNames("NAD_1927_UTM_Zone_59N", "GCS_North_American_1927", "D_North_American_1927");
            NAD1927UTMZone5N = ProjectionInfo.FromEpsgCode(26705).SetNames("NAD_1927_UTM_Zone_5N", "GCS_North_American_1927", "D_North_American_1927");
            NAD1927UTMZone60N = ProjectionInfo.FromEpsgCode(3371).SetNames("NAD_1927_UTM_Zone_60N", "GCS_North_American_1927", "D_North_American_1927");
            NAD1927UTMZone6N = ProjectionInfo.FromEpsgCode(26706).SetNames("NAD_1927_UTM_Zone_6N", "GCS_North_American_1927", "D_North_American_1927");
            NAD1927UTMZone7N = ProjectionInfo.FromEpsgCode(26707).SetNames("NAD_1927_UTM_Zone_7N", "GCS_North_American_1927", "D_North_American_1927");
            NAD1927UTMZone8N = ProjectionInfo.FromEpsgCode(26708).SetNames("NAD_1927_UTM_Zone_8N", "GCS_North_American_1927", "D_North_American_1927");
            NAD1927UTMZone9N = ProjectionInfo.FromEpsgCode(26709).SetNames("NAD_1927_UTM_Zone_9N", "GCS_North_American_1927", "D_North_American_1927");
        }

        #endregion
    }
}

#pragma warning restore 1591