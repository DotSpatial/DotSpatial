// ********************************************************************************************************
// Product Name: DotSpatial.Projection
// Description:  The basic module for MapWindow version 6.0
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/14/2009 4:53:34 PM
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
    /// This class contains predefined CoordinateSystems for Norway.
    /// </summary>
    public class Norway : CoordinateSystemCategory
    {
        #region Private Variables
        // ReSharper disable InconsistentNaming
        public readonly ProjectionInfo NGO1948OsloBaerumKommune;
        public readonly ProjectionInfo NGO1948OsloBergenhalvoen;
        public readonly ProjectionInfo NGO1948OsloNorwayZone1;
        public readonly ProjectionInfo NGO1948OsloNorwayZone2;
        public readonly ProjectionInfo NGO1948OsloNorwayZone3;
        public readonly ProjectionInfo NGO1948OsloNorwayZone4;
        public readonly ProjectionInfo NGO1948OsloNorwayZone5;
        public readonly ProjectionInfo NGO1948OsloNorwayZone6;
        public readonly ProjectionInfo NGO1948OsloNorwayZone7;
        public readonly ProjectionInfo NGO1948OsloNorwayZone8;
        public readonly ProjectionInfo NGO1948OsloOsloKommune;
        // ReSharper restore InconsistentNaming
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of Norway.
        /// </summary>
        public Norway()
        {
            NGO1948OsloBaerumKommune = ProjectionInfo.FromAuthorityCode("ESRI", 102301).SetNames("NGO_1948_Baerum_Kommune_incorrect_CM", "GCS_NGO_1948_Oslo", "D_NGO_1948"); // missing
            NGO1948OsloBergenhalvoen = ProjectionInfo.FromAuthorityCode("ESRI", 102302).SetNames("NGO_1948_Bergenhalvoen_incorrect_CM", "GCS_NGO_1948_Oslo", "D_NGO_1948"); // missing
            NGO1948OsloNorwayZone1 = ProjectionInfo.FromEpsgCode(27391).SetNames("NGO_1948_Oslo_Norway_Zone_1", "GCS_NGO_1948_Oslo", "D_NGO_1948");
            NGO1948OsloNorwayZone2 = ProjectionInfo.FromEpsgCode(27392).SetNames("NGO_1948_Oslo_Norway_Zone_2", "GCS_NGO_1948_Oslo", "D_NGO_1948");
            NGO1948OsloNorwayZone3 = ProjectionInfo.FromEpsgCode(27393).SetNames("NGO_1948_Oslo_Norway_Zone_3", "GCS_NGO_1948_Oslo", "D_NGO_1948");
            NGO1948OsloNorwayZone4 = ProjectionInfo.FromEpsgCode(27394).SetNames("NGO_1948_Oslo_Norway_Zone_4", "GCS_NGO_1948_Oslo", "D_NGO_1948");
            NGO1948OsloNorwayZone5 = ProjectionInfo.FromEpsgCode(27395).SetNames("NGO_1948_Oslo_Norway_Zone_5", "GCS_NGO_1948_Oslo", "D_NGO_1948");
            NGO1948OsloNorwayZone6 = ProjectionInfo.FromEpsgCode(27396).SetNames("NGO_1948_Oslo_Norway_Zone_6", "GCS_NGO_1948_Oslo", "D_NGO_1948");
            NGO1948OsloNorwayZone7 = ProjectionInfo.FromEpsgCode(27397).SetNames("NGO_1948_Oslo_Norway_Zone_7", "GCS_NGO_1948_Oslo", "D_NGO_1948");
            NGO1948OsloNorwayZone8 = ProjectionInfo.FromEpsgCode(27398).SetNames("NGO_1948_Oslo_Norway_Zone_8", "GCS_NGO_1948_Oslo", "D_NGO_1948");
            NGO1948OsloOsloKommune = ProjectionInfo.FromAuthorityCode("ESRI", 102303).SetNames("NGO_1948_Oslo_Kommune_incorrect_CM", "GCS_NGO_1948_Oslo", "D_NGO_1948"); // missing
        }

        #endregion


        /// <summary>
        /// This class contains predefined CoordinateSystems for GreenwichBased.
        /// </summary>
        public class GreenwichBased : CoordinateSystemCategory
        {
            #region Private Variables
            // ReSharper disable InconsistentNaming
            public readonly ProjectionInfo NGO1948BaerumKommune;
            public readonly ProjectionInfo NGO1948Bergenhalvoen;
            public readonly ProjectionInfo NGO1948NorwayZone1;
            public readonly ProjectionInfo NGO1948NorwayZone2;
            public readonly ProjectionInfo NGO1948NorwayZone3;
            public readonly ProjectionInfo NGO1948NorwayZone4;
            public readonly ProjectionInfo NGO1948NorwayZone5;
            public readonly ProjectionInfo NGO1948NorwayZone6;
            public readonly ProjectionInfo NGO1948NorwayZone7;
            public readonly ProjectionInfo NGO1948NorwayZone8;
            public readonly ProjectionInfo NGO1948OsloKommune;
            // ReSharper restore InconsistentNaming
            #endregion

            #region Constructors

            /// <summary>
            /// Creates a new instance of GreenwichBased.
            /// </summary>
            public GreenwichBased()
            {
                NGO1948BaerumKommune = ProjectionInfo.FromAuthorityCode("ESRI", 102136).SetNames("NGO_1948_Baerum_Kommune", "GCS_NGO_1948", "D_NGO_1948"); // missing
                NGO1948Bergenhalvoen = ProjectionInfo.FromAuthorityCode("ESRI", 102137).SetNames("NGO_1948_Bergenhalvoen", "GCS_NGO_1948", "D_NGO_1948"); // missing
                NGO1948NorwayZone1 = ProjectionInfo.FromAuthorityCode("ESRI", 102101).SetNames("NGO_1948_Norway_Zone_1", "GCS_NGO_1948", "D_NGO_1948");
                NGO1948NorwayZone2 = ProjectionInfo.FromAuthorityCode("ESRI", 102102).SetNames("NGO_1948_Norway_Zone_2", "GCS_NGO_1948", "D_NGO_1948");
                NGO1948NorwayZone3 = ProjectionInfo.FromAuthorityCode("ESRI", 102103).SetNames("NGO_1948_Norway_Zone_3", "GCS_NGO_1948", "D_NGO_1948");
                NGO1948NorwayZone4 = ProjectionInfo.FromAuthorityCode("ESRI", 102104).SetNames("NGO_1948_Norway_Zone_4", "GCS_NGO_1948", "D_NGO_1948");
                NGO1948NorwayZone5 = ProjectionInfo.FromAuthorityCode("ESRI", 102105).SetNames("NGO_1948_Norway_Zone_5", "GCS_NGO_1948", "D_NGO_1948");
                NGO1948NorwayZone6 = ProjectionInfo.FromAuthorityCode("ESRI", 102106).SetNames("NGO_1948_Norway_Zone_6", "GCS_NGO_1948", "D_NGO_1948");
                NGO1948NorwayZone7 = ProjectionInfo.FromAuthorityCode("ESRI", 102107).SetNames("NGO_1948_Norway_Zone_7", "GCS_NGO_1948", "D_NGO_1948");
                NGO1948NorwayZone8 = ProjectionInfo.FromAuthorityCode("ESRI", 102108).SetNames("NGO_1948_Norway_Zone_8", "GCS_NGO_1948", "D_NGO_1948");
                NGO1948OsloKommune = ProjectionInfo.FromAuthorityCode("ESRI", 102138).SetNames("NGO_1948_Oslo_Kommune", "GCS_NGO_1948", "D_NGO_1948"); // missing
            }

            #endregion
        }

    }
}

#pragma warning restore 1591