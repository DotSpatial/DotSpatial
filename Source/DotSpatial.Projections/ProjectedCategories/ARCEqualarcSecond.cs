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

namespace DotSpatial.Projections.ProjectedCategories
{
    /// <summary>
    /// This class contains predefined CoordinateSystems for ARCEqualarcSecond.
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public class ARCEqualarcSecond : CoordinateSystemCategory
    {
        #region Private Variables
        // ReSharper disable InconsistentNaming
        public readonly ProjectionInfo WGS84ARCSystemZone01;
        public readonly ProjectionInfo WGS84ARCSystemZone02;
        public readonly ProjectionInfo WGS84ARCSystemZone03;
        public readonly ProjectionInfo WGS84ARCSystemZone04;
        public readonly ProjectionInfo WGS84ARCSystemZone05;
        public readonly ProjectionInfo WGS84ARCSystemZone06;
        public readonly ProjectionInfo WGS84ARCSystemZone07;
        public readonly ProjectionInfo WGS84ARCSystemZone08;
        public readonly ProjectionInfo WGS84ARCSystemZone09;
        public readonly ProjectionInfo WGS84ARCSystemZone10;
        public readonly ProjectionInfo WGS84ARCSystemZone11;
        public readonly ProjectionInfo WGS84ARCSystemZone12;
        public readonly ProjectionInfo WGS84ARCSystemZone13;
        public readonly ProjectionInfo WGS84ARCSystemZone14;
        public readonly ProjectionInfo WGS84ARCSystemZone15;
        public readonly ProjectionInfo WGS84ARCSystemZone16;
        public readonly ProjectionInfo WGS84ARCSystemZone17;
        public readonly ProjectionInfo WGS84ARCSystemZone18;
        // ReSharper restore InconsistentNaming
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of ARCEqualarcSecond.
        /// </summary>
        public ARCEqualarcSecond()
        {
            WGS84ARCSystemZone01 = ProjectionInfo.FromAuthorityCode("ESRI", 102421).SetNames("WGS_1984_ARC_System_Zone_01", "GCS_WGS_1984", "D_WGS_1984"); // missing
            WGS84ARCSystemZone02 = ProjectionInfo.FromAuthorityCode("ESRI", 102422).SetNames("WGS_1984_ARC_System_Zone_02", "GCS_WGS_1984", "D_WGS_1984"); // missing
            WGS84ARCSystemZone03 = ProjectionInfo.FromAuthorityCode("ESRI", 102423).SetNames("WGS_1984_ARC_System_Zone_03", "GCS_WGS_1984", "D_WGS_1984"); // missing
            WGS84ARCSystemZone04 = ProjectionInfo.FromAuthorityCode("ESRI", 102424).SetNames("WGS_1984_ARC_System_Zone_04", "GCS_WGS_1984", "D_WGS_1984"); // missing
            WGS84ARCSystemZone05 = ProjectionInfo.FromAuthorityCode("ESRI", 102425).SetNames("WGS_1984_ARC_System_Zone_05", "GCS_WGS_1984", "D_WGS_1984"); // missing
            WGS84ARCSystemZone06 = ProjectionInfo.FromAuthorityCode("ESRI", 102426).SetNames("WGS_1984_ARC_System_Zone_06", "GCS_WGS_1984", "D_WGS_1984"); // missing
            WGS84ARCSystemZone07 = ProjectionInfo.FromAuthorityCode("ESRI", 102427).SetNames("WGS_1984_ARC_System_Zone_07", "GCS_WGS_1984", "D_WGS_1984"); // missing
            WGS84ARCSystemZone08 = ProjectionInfo.FromAuthorityCode("ESRI", 102428).SetNames("WGS_1984_ARC_System_Zone_08", "GCS_WGS_1984", "D_WGS_1984"); // missing
            WGS84ARCSystemZone09 = ProjectionInfo.FromAuthorityCode("ESRI", 102429).SetNames("WGS_1984_ARC_System_Zone_09", "GCS_WGS_1984", "D_WGS_1984"); // missing
            WGS84ARCSystemZone10 = ProjectionInfo.FromAuthorityCode("ESRI", 102430).SetNames("WGS_1984_ARC_System_Zone_10", "GCS_WGS_1984", "D_WGS_1984"); // missing
            WGS84ARCSystemZone11 = ProjectionInfo.FromAuthorityCode("ESRI", 102431).SetNames("WGS_1984_ARC_System_Zone_11", "GCS_WGS_1984", "D_WGS_1984"); // missing
            WGS84ARCSystemZone12 = ProjectionInfo.FromAuthorityCode("ESRI", 102432).SetNames("WGS_1984_ARC_System_Zone_12", "GCS_WGS_1984", "D_WGS_1984"); // missing
            WGS84ARCSystemZone13 = ProjectionInfo.FromAuthorityCode("ESRI", 102433).SetNames("WGS_1984_ARC_System_Zone_13", "GCS_WGS_1984", "D_WGS_1984"); // missing
            WGS84ARCSystemZone14 = ProjectionInfo.FromAuthorityCode("ESRI", 102434).SetNames("WGS_1984_ARC_System_Zone_14", "GCS_WGS_1984", "D_WGS_1984"); // missing
            WGS84ARCSystemZone15 = ProjectionInfo.FromAuthorityCode("ESRI", 102435).SetNames("WGS_1984_ARC_System_Zone_15", "GCS_WGS_1984", "D_WGS_1984"); // missing
            WGS84ARCSystemZone16 = ProjectionInfo.FromAuthorityCode("ESRI", 102436).SetNames("WGS_1984_ARC_System_Zone_16", "GCS_WGS_1984", "D_WGS_1984"); // missing
            WGS84ARCSystemZone17 = ProjectionInfo.FromAuthorityCode("ESRI", 102437).SetNames("WGS_1984_ARC_System_Zone_17", "GCS_WGS_1984", "D_WGS_1984"); // missing
            WGS84ARCSystemZone18 = ProjectionInfo.FromAuthorityCode("ESRI", 102438).SetNames("WGS_1984_ARC_System_Zone_18", "GCS_WGS_1984", "D_WGS_1984"); // missing
        }

        #endregion
    }
}

#pragma warning restore 1591