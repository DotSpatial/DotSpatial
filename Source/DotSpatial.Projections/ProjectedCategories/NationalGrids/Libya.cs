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
    /// This class contains predefined CoordinateSystems for Libya.
    /// </summary>
    public class Libya : CoordinateSystemCategory
    {
        #region Private Variables
        // ReSharper disable InconsistentNaming
        public readonly ProjectionInfo ELD1979Libya10;
        public readonly ProjectionInfo ELD1979Libya11;
        public readonly ProjectionInfo ELD1979Libya12;
        public readonly ProjectionInfo ELD1979Libya13;
        public readonly ProjectionInfo ELD1979Libya5;
        public readonly ProjectionInfo ELD1979Libya6;
        public readonly ProjectionInfo ELD1979Libya7;
        public readonly ProjectionInfo ELD1979Libya8;
        public readonly ProjectionInfo ELD1979Libya9;
        public readonly ProjectionInfo ELD1979TM12NE;
        public readonly ProjectionInfo LGD2006LibyaTM;
        public readonly ProjectionInfo LGD2006LibyaTMZone10;
        public readonly ProjectionInfo LGD2006LibyaTMZone11;
        public readonly ProjectionInfo LGD2006LibyaTMZone12;
        public readonly ProjectionInfo LGD2006LibyaTMZone13;
        public readonly ProjectionInfo LGD2006LibyaTMZone5;
        public readonly ProjectionInfo LGD2006LibyaTMZone6;
        public readonly ProjectionInfo LGD2006LibyaTMZone7;
        public readonly ProjectionInfo LGD2006LibyaTMZone8;
        public readonly ProjectionInfo LGD2006LibyaTMZone9;
        // ReSharper restore InconsistentNaming
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of Libya.
        /// </summary>
        public Libya()
        {
            ELD1979Libya10 = ProjectionInfo.FromEpsgCode(2073).SetNames("ELD_1979_Libya_10", "GCS_European_Libyan_Datum_1979", "D_European_Libyan_1979");
            ELD1979Libya11 = ProjectionInfo.FromEpsgCode(2074).SetNames("ELD_1979_Libya_11", "GCS_European_Libyan_Datum_1979", "D_European_Libyan_1979");
            ELD1979Libya12 = ProjectionInfo.FromEpsgCode(2075).SetNames("ELD_1979_Libya_12", "GCS_European_Libyan_Datum_1979", "D_European_Libyan_1979");
            ELD1979Libya13 = ProjectionInfo.FromEpsgCode(2076).SetNames("ELD_1979_Libya_13", "GCS_European_Libyan_Datum_1979", "D_European_Libyan_1979");
            ELD1979Libya5 = ProjectionInfo.FromEpsgCode(2068).SetNames("ELD_1979_Libya_5", "GCS_European_Libyan_Datum_1979", "D_European_Libyan_1979");
            ELD1979Libya6 = ProjectionInfo.FromEpsgCode(2069).SetNames("ELD_1979_Libya_6", "GCS_European_Libyan_Datum_1979", "D_European_Libyan_1979");
            ELD1979Libya7 = ProjectionInfo.FromEpsgCode(2070).SetNames("ELD_1979_Libya_7", "GCS_European_Libyan_Datum_1979", "D_European_Libyan_1979");
            ELD1979Libya8 = ProjectionInfo.FromEpsgCode(2071).SetNames("ELD_1979_Libya_8", "GCS_European_Libyan_Datum_1979", "D_European_Libyan_1979");
            ELD1979Libya9 = ProjectionInfo.FromEpsgCode(2072).SetNames("ELD_1979_Libya_9", "GCS_European_Libyan_Datum_1979", "D_European_Libyan_1979");
            ELD1979TM12NE = ProjectionInfo.FromEpsgCode(2087).SetNames("ELD_1979_TM_12_NE", "GCS_European_Libyan_Datum_1979", "D_European_Libyan_1979");
            LGD2006LibyaTM = ProjectionInfo.FromEpsgCode(3177).SetNames("LGD2006_Libya_TM", "GCS_LGD2006", "D_Libyan_Geodetic_Datum_2006");
            LGD2006LibyaTMZone10 = ProjectionInfo.FromEpsgCode(3195).SetNames("LGD2006_Libya_TM_Zone_10", "GCS_LGD2006", "D_Libyan_Geodetic_Datum_2006");
            LGD2006LibyaTMZone11 = ProjectionInfo.FromEpsgCode(3196).SetNames("LGD2006_Libya_TM_Zone_11", "GCS_LGD2006", "D_Libyan_Geodetic_Datum_2006");
            LGD2006LibyaTMZone12 = ProjectionInfo.FromEpsgCode(3197).SetNames("LGD2006_Libya_TM_Zone_12", "GCS_LGD2006", "D_Libyan_Geodetic_Datum_2006");
            LGD2006LibyaTMZone13 = ProjectionInfo.FromEpsgCode(3198).SetNames("LGD2006_Libya_TM_Zone_13", "GCS_LGD2006", "D_Libyan_Geodetic_Datum_2006");
            LGD2006LibyaTMZone5 = ProjectionInfo.FromEpsgCode(3190).SetNames("LGD2006_Libya_TM_Zone_5", "GCS_LGD2006", "D_Libyan_Geodetic_Datum_2006");
            LGD2006LibyaTMZone6 = ProjectionInfo.FromEpsgCode(3191).SetNames("LGD2006_Libya_TM_Zone_6", "GCS_LGD2006", "D_Libyan_Geodetic_Datum_2006");
            LGD2006LibyaTMZone7 = ProjectionInfo.FromEpsgCode(3192).SetNames("LGD2006_Libya_TM_Zone_7", "GCS_LGD2006", "D_Libyan_Geodetic_Datum_2006");
            LGD2006LibyaTMZone8 = ProjectionInfo.FromEpsgCode(3193).SetNames("LGD2006_Libya_TM_Zone_8", "GCS_LGD2006", "D_Libyan_Geodetic_Datum_2006");
            LGD2006LibyaTMZone9 = ProjectionInfo.FromEpsgCode(3194).SetNames("LGD2006_Libya_TM_Zone_9", "GCS_LGD2006", "D_Libyan_Geodetic_Datum_2006");
        }

        #endregion
    }
}

#pragma warning restore 1591