// ********************************************************************************************************
// Product Name: DotSpatial.Projection
// Description:  The basic module for MapWindow version 6.0
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/14/2009 4:51:42 PM
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
    /// NationalGridsJapan
    /// </summary>
    public class Japan : CoordinateSystemCategory
    {
        #region Private Variables
        // ReSharper disable InconsistentNaming
        public readonly ProjectionInfo JapanZone1;
        public readonly ProjectionInfo JapanZone10;
        public readonly ProjectionInfo JapanZone11;
        public readonly ProjectionInfo JapanZone12;
        public readonly ProjectionInfo JapanZone13;
        public readonly ProjectionInfo JapanZone14;
        public readonly ProjectionInfo JapanZone15;
        public readonly ProjectionInfo JapanZone16;
        public readonly ProjectionInfo JapanZone17;
        public readonly ProjectionInfo JapanZone18;
        public readonly ProjectionInfo JapanZone19;
        public readonly ProjectionInfo JapanZone2;
        public readonly ProjectionInfo JapanZone3;
        public readonly ProjectionInfo JapanZone4;
        public readonly ProjectionInfo JapanZone5;
        public readonly ProjectionInfo JapanZone6;
        public readonly ProjectionInfo JapanZone7;
        public readonly ProjectionInfo JapanZone8;
        public readonly ProjectionInfo JapanZone9;
        public readonly ProjectionInfo JGD2000JapanZone1;
        public readonly ProjectionInfo JGD2000JapanZone10;
        public readonly ProjectionInfo JGD2000JapanZone11;
        public readonly ProjectionInfo JGD2000JapanZone12;
        public readonly ProjectionInfo JGD2000JapanZone13;
        public readonly ProjectionInfo JGD2000JapanZone14;
        public readonly ProjectionInfo JGD2000JapanZone15;
        public readonly ProjectionInfo JGD2000JapanZone16;
        public readonly ProjectionInfo JGD2000JapanZone17;
        public readonly ProjectionInfo JGD2000JapanZone18;
        public readonly ProjectionInfo JGD2000JapanZone19;
        public readonly ProjectionInfo JGD2000JapanZone2;
        public readonly ProjectionInfo JGD2000JapanZone3;
        public readonly ProjectionInfo JGD2000JapanZone4;
        public readonly ProjectionInfo JGD2000JapanZone5;
        public readonly ProjectionInfo JGD2000JapanZone6;
        public readonly ProjectionInfo JGD2000JapanZone7;
        public readonly ProjectionInfo JGD2000JapanZone8;
        public readonly ProjectionInfo JGD2000JapanZone9;
        // ReSharper restore InconsistentNaming
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of Japan.
        /// </summary>
        public Japan()
        {
            JapanZone1 = ProjectionInfo.FromEpsgCode(30161).SetNames("Japan_Zone_1", "GCS_Tokyo", "D_Tokyo");
            JapanZone10 = ProjectionInfo.FromEpsgCode(30170).SetNames("Japan_Zone_10", "GCS_Tokyo", "D_Tokyo");
            JapanZone11 = ProjectionInfo.FromEpsgCode(30171).SetNames("Japan_Zone_11", "GCS_Tokyo", "D_Tokyo");
            JapanZone12 = ProjectionInfo.FromEpsgCode(30172).SetNames("Japan_Zone_12", "GCS_Tokyo", "D_Tokyo");
            JapanZone13 = ProjectionInfo.FromEpsgCode(30173).SetNames("Japan_Zone_13", "GCS_Tokyo", "D_Tokyo");
            JapanZone14 = ProjectionInfo.FromEpsgCode(30174).SetNames("Japan_Zone_14", "GCS_Tokyo", "D_Tokyo");
            JapanZone15 = ProjectionInfo.FromEpsgCode(30175).SetNames("Japan_Zone_15", "GCS_Tokyo", "D_Tokyo");
            JapanZone16 = ProjectionInfo.FromEpsgCode(30176).SetNames("Japan_Zone_16", "GCS_Tokyo", "D_Tokyo");
            JapanZone17 = ProjectionInfo.FromEpsgCode(30177).SetNames("Japan_Zone_17", "GCS_Tokyo", "D_Tokyo");
            JapanZone18 = ProjectionInfo.FromEpsgCode(30178).SetNames("Japan_Zone_18", "GCS_Tokyo", "D_Tokyo");
            JapanZone19 = ProjectionInfo.FromEpsgCode(30179).SetNames("Japan_Zone_19", "GCS_Tokyo", "D_Tokyo");
            JapanZone2 = ProjectionInfo.FromEpsgCode(30162).SetNames("Japan_Zone_2", "GCS_Tokyo", "D_Tokyo");
            JapanZone3 = ProjectionInfo.FromEpsgCode(30163).SetNames("Japan_Zone_3", "GCS_Tokyo", "D_Tokyo");
            JapanZone4 = ProjectionInfo.FromEpsgCode(30164).SetNames("Japan_Zone_4", "GCS_Tokyo", "D_Tokyo");
            JapanZone5 = ProjectionInfo.FromEpsgCode(30165).SetNames("Japan_Zone_5", "GCS_Tokyo", "D_Tokyo");
            JapanZone6 = ProjectionInfo.FromEpsgCode(30166).SetNames("Japan_Zone_6", "GCS_Tokyo", "D_Tokyo");
            JapanZone7 = ProjectionInfo.FromEpsgCode(30167).SetNames("Japan_Zone_7", "GCS_Tokyo", "D_Tokyo");
            JapanZone8 = ProjectionInfo.FromEpsgCode(30168).SetNames("Japan_Zone_8", "GCS_Tokyo", "D_Tokyo");
            JapanZone9 = ProjectionInfo.FromEpsgCode(30169).SetNames("Japan_Zone_9", "GCS_Tokyo", "D_Tokyo");
            JGD2000JapanZone1 = ProjectionInfo.FromEpsgCode(2443).SetNames("JGD_2000_Japan_Zone_1", "GCS_JGD_2000", "D_JGD_2000");
            JGD2000JapanZone10 = ProjectionInfo.FromEpsgCode(2452).SetNames("JGD_2000_Japan_Zone_10", "GCS_JGD_2000", "D_JGD_2000");
            JGD2000JapanZone11 = ProjectionInfo.FromEpsgCode(2453).SetNames("JGD_2000_Japan_Zone_11", "GCS_JGD_2000", "D_JGD_2000");
            JGD2000JapanZone12 = ProjectionInfo.FromEpsgCode(2454).SetNames("JGD_2000_Japan_Zone_12", "GCS_JGD_2000", "D_JGD_2000");
            JGD2000JapanZone13 = ProjectionInfo.FromEpsgCode(2455).SetNames("JGD_2000_Japan_Zone_13", "GCS_JGD_2000", "D_JGD_2000");
            JGD2000JapanZone14 = ProjectionInfo.FromEpsgCode(2456).SetNames("JGD_2000_Japan_Zone_14", "GCS_JGD_2000", "D_JGD_2000");
            JGD2000JapanZone15 = ProjectionInfo.FromEpsgCode(2457).SetNames("JGD_2000_Japan_Zone_15", "GCS_JGD_2000", "D_JGD_2000");
            JGD2000JapanZone16 = ProjectionInfo.FromEpsgCode(2458).SetNames("JGD_2000_Japan_Zone_16", "GCS_JGD_2000", "D_JGD_2000");
            JGD2000JapanZone17 = ProjectionInfo.FromEpsgCode(2459).SetNames("JGD_2000_Japan_Zone_17", "GCS_JGD_2000", "D_JGD_2000");
            JGD2000JapanZone18 = ProjectionInfo.FromEpsgCode(2460).SetNames("JGD_2000_Japan_Zone_18", "GCS_JGD_2000", "D_JGD_2000");
            JGD2000JapanZone19 = ProjectionInfo.FromEpsgCode(2461).SetNames("JGD_2000_Japan_Zone_19", "GCS_JGD_2000", "D_JGD_2000");
            JGD2000JapanZone2 = ProjectionInfo.FromEpsgCode(2444).SetNames("JGD_2000_Japan_Zone_2", "GCS_JGD_2000", "D_JGD_2000");
            JGD2000JapanZone3 = ProjectionInfo.FromEpsgCode(2445).SetNames("JGD_2000_Japan_Zone_3", "GCS_JGD_2000", "D_JGD_2000");
            JGD2000JapanZone4 = ProjectionInfo.FromEpsgCode(2446).SetNames("JGD_2000_Japan_Zone_4", "GCS_JGD_2000", "D_JGD_2000");
            JGD2000JapanZone5 = ProjectionInfo.FromEpsgCode(2447).SetNames("JGD_2000_Japan_Zone_5", "GCS_JGD_2000", "D_JGD_2000");
            JGD2000JapanZone6 = ProjectionInfo.FromEpsgCode(2448).SetNames("JGD_2000_Japan_Zone_6", "GCS_JGD_2000", "D_JGD_2000");
            JGD2000JapanZone7 = ProjectionInfo.FromEpsgCode(2449).SetNames("JGD_2000_Japan_Zone_7", "GCS_JGD_2000", "D_JGD_2000");
            JGD2000JapanZone8 = ProjectionInfo.FromEpsgCode(2450).SetNames("JGD_2000_Japan_Zone_8", "GCS_JGD_2000", "D_JGD_2000");
            JGD2000JapanZone9 = ProjectionInfo.FromEpsgCode(2451).SetNames("JGD_2000_Japan_Zone_9", "GCS_JGD_2000", "D_JGD_2000");
        }

        #endregion
    }
}

#pragma warning restore 1591