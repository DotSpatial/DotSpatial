// ********************************************************************************************************
// Product Name: DotSpatial.Projection
// Description:  The basic module for MapWindow version 6.0
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/14/2009 5:08:05 PM
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
    /// This class contains predefined CoordinateSystems for NAD1983.
    /// </summary>
    public class NAD1983 : CoordinateSystemCategory
    {
        #region Private Variables
        // ReSharper disable InconsistentNaming
        public readonly ProjectionInfo NAD1983UTMZone10N;
        public readonly ProjectionInfo NAD1983UTMZone11N;
        public readonly ProjectionInfo NAD1983UTMZone12N;
        public readonly ProjectionInfo NAD1983UTMZone13N;
        public readonly ProjectionInfo NAD1983UTMZone14N;
        public readonly ProjectionInfo NAD1983UTMZone15N;
        public readonly ProjectionInfo NAD1983UTMZone16N;
        public readonly ProjectionInfo NAD1983UTMZone17N;
        public readonly ProjectionInfo NAD1983UTMZone18N;
        public readonly ProjectionInfo NAD1983UTMZone19N;
        public readonly ProjectionInfo NAD1983UTMZone1N;
        public readonly ProjectionInfo NAD1983UTMZone20N;
        public readonly ProjectionInfo NAD1983UTMZone21N;
        public readonly ProjectionInfo NAD1983UTMZone22N;
        public readonly ProjectionInfo NAD1983UTMZone23N;
        public readonly ProjectionInfo NAD1983UTMZone2N;
        public readonly ProjectionInfo NAD1983UTMZone3N;
        public readonly ProjectionInfo NAD1983UTMZone4N;
        public readonly ProjectionInfo NAD1983UTMZone58N;
        public readonly ProjectionInfo NAD1983UTMZone59N;
        public readonly ProjectionInfo NAD1983UTMZone5N;
        public readonly ProjectionInfo NAD1983UTMZone60N;
        public readonly ProjectionInfo NAD1983UTMZone6N;
        public readonly ProjectionInfo NAD1983UTMZone7N;
        public readonly ProjectionInfo NAD1983UTMZone8N;
        public readonly ProjectionInfo NAD1983UTMZone9N;
        // ReSharper restore InconsistentNaming
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of NAD1983.
        /// </summary>
        public NAD1983()
        {
            NAD1983UTMZone10N = ProjectionInfo.FromEpsgCode(26910).SetNames("NAD_1983_UTM_Zone_10N", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983UTMZone11N = ProjectionInfo.FromEpsgCode(26911).SetNames("NAD_1983_UTM_Zone_11N", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983UTMZone12N = ProjectionInfo.FromEpsgCode(26912).SetNames("NAD_1983_UTM_Zone_12N", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983UTMZone13N = ProjectionInfo.FromEpsgCode(26913).SetNames("NAD_1983_UTM_Zone_13N", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983UTMZone14N = ProjectionInfo.FromEpsgCode(26914).SetNames("NAD_1983_UTM_Zone_14N", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983UTMZone15N = ProjectionInfo.FromEpsgCode(26915).SetNames("NAD_1983_UTM_Zone_15N", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983UTMZone16N = ProjectionInfo.FromEpsgCode(26916).SetNames("NAD_1983_UTM_Zone_16N", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983UTMZone17N = ProjectionInfo.FromEpsgCode(26917).SetNames("NAD_1983_UTM_Zone_17N", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983UTMZone18N = ProjectionInfo.FromEpsgCode(26918).SetNames("NAD_1983_UTM_Zone_18N", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983UTMZone19N = ProjectionInfo.FromEpsgCode(26919).SetNames("NAD_1983_UTM_Zone_19N", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983UTMZone1N = ProjectionInfo.FromEpsgCode(26901).SetNames("NAD_1983_UTM_Zone_1N", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983UTMZone20N = ProjectionInfo.FromEpsgCode(26920).SetNames("NAD_1983_UTM_Zone_20N", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983UTMZone21N = ProjectionInfo.FromEpsgCode(26921).SetNames("NAD_1983_UTM_Zone_21N", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983UTMZone22N = ProjectionInfo.FromEpsgCode(26922).SetNames("NAD_1983_UTM_Zone_22N", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983UTMZone23N = ProjectionInfo.FromEpsgCode(26923).SetNames("NAD_1983_UTM_Zone_23N", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983UTMZone2N = ProjectionInfo.FromEpsgCode(26902).SetNames("NAD_1983_UTM_Zone_2N", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983UTMZone3N = ProjectionInfo.FromEpsgCode(26903).SetNames("NAD_1983_UTM_Zone_3N", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983UTMZone4N = ProjectionInfo.FromEpsgCode(26904).SetNames("NAD_1983_UTM_Zone_4N", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983UTMZone58N = ProjectionInfo.FromAuthorityCode("ESRI", 102213).SetNames("NAD_1983_UTM_Zone_58N", "GCS_North_American_1983", "D_North_American_1983"); // missing
            NAD1983UTMZone59N = ProjectionInfo.FromEpsgCode(3372).SetNames("NAD_1983_UTM_Zone_59N", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983UTMZone5N = ProjectionInfo.FromEpsgCode(26905).SetNames("NAD_1983_UTM_Zone_5N", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983UTMZone60N = ProjectionInfo.FromEpsgCode(3373).SetNames("NAD_1983_UTM_Zone_60N", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983UTMZone6N = ProjectionInfo.FromEpsgCode(26906).SetNames("NAD_1983_UTM_Zone_6N", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983UTMZone7N = ProjectionInfo.FromEpsgCode(26907).SetNames("NAD_1983_UTM_Zone_7N", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983UTMZone8N = ProjectionInfo.FromEpsgCode(26908).SetNames("NAD_1983_UTM_Zone_8N", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983UTMZone9N = ProjectionInfo.FromEpsgCode(26909).SetNames("NAD_1983_UTM_Zone_9N", "GCS_North_American_1983", "D_North_American_1983");
        }

        #endregion
    }
}

#pragma warning restore 1591