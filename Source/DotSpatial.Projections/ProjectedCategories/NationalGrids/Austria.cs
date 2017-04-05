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
    /// This class contains predefined CoordinateSystems for Austria.
    /// </summary>
    public class Austria : CoordinateSystemCategory
    {
        #region Private Variables
        // ReSharper disable InconsistentNaming
        public readonly ProjectionInfo AustriaFerroCentralZone;
        public readonly ProjectionInfo AustriaFerroEastZone;
        public readonly ProjectionInfo AustriaFerroWestZone;
        public readonly ProjectionInfo ETRS1989AustriaLambert;
        public readonly ProjectionInfo MGIAustriaGKCentral;
        public readonly ProjectionInfo MGIAustriaGKEast;
        public readonly ProjectionInfo MGIAustriaGKM28;
        public readonly ProjectionInfo MGIAustriaGKM31;
        public readonly ProjectionInfo MGIAustriaGKM34;
        public readonly ProjectionInfo MGIAustriaGKWest;
        public readonly ProjectionInfo MGIAustriaLambert;
        public readonly ProjectionInfo MGIFerroAustriaGKCentral;
        public readonly ProjectionInfo MGIFerroAustriaGKEast;
        public readonly ProjectionInfo MGIFerroAustriaGKWest;
        public readonly ProjectionInfo MGIFerroM28;
        public readonly ProjectionInfo MGIFerroM31;
        public readonly ProjectionInfo MGIFerroM34;
        public readonly ProjectionInfo MGIM28;
        public readonly ProjectionInfo MGIM31;
        public readonly ProjectionInfo MGIM34;
        // ReSharper restore InconsistentNaming
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of Austria.
        /// </summary>
        public Austria()
        {
            AustriaFerroCentralZone = ProjectionInfo.FromEpsgCode(31282).SetNames("Austria_Central_Zone", "GCS_MGI_Ferro", "D_MGI");
            AustriaFerroEastZone = ProjectionInfo.FromEpsgCode(31283).SetNames("Austria_East_Zone", "GCS_MGI_Ferro", "D_MGI");
            AustriaFerroWestZone = ProjectionInfo.FromEpsgCode(31281).SetNames("Austria_West_Zone", "GCS_MGI_Ferro", "D_MGI");
            ETRS1989AustriaLambert = ProjectionInfo.FromEpsgCode(3416).SetNames("ETRS_1989_Austria_Lambert", "GCS_ETRS_1989", "D_ETRS_1989");
            MGIAustriaGKCentral = ProjectionInfo.FromEpsgCode(31255).SetNames("MGI_Austria_GK_Central", "GCS_MGI", "D_MGI");
            MGIAustriaGKEast = ProjectionInfo.FromEpsgCode(31256).SetNames("MGI_Austria_GK_East", "GCS_MGI", "D_MGI");
            MGIAustriaGKM28 = ProjectionInfo.FromEpsgCode(31257).SetNames("MGI_Austria_GK_M28", "GCS_MGI", "D_MGI");
            MGIAustriaGKM31 = ProjectionInfo.FromEpsgCode(31258).SetNames("MGI_Austria_GK_M31", "GCS_MGI", "D_MGI");
            MGIAustriaGKM34 = ProjectionInfo.FromEpsgCode(31259).SetNames("MGI_Austria_GK_M34", "GCS_MGI", "D_MGI");
            MGIAustriaGKWest = ProjectionInfo.FromEpsgCode(31254).SetNames("MGI_Austria_GK_West", "GCS_MGI", "D_MGI");
            MGIAustriaLambert = ProjectionInfo.FromEpsgCode(31287).SetNames("MGI_Austria_Lambert", "GCS_MGI", "D_MGI");
            MGIFerroAustriaGKCentral = ProjectionInfo.FromEpsgCode(31252).SetNames("MGI_Ferro_Austria_GK_Central", "GCS_MGI_Ferro", "D_MGI");
            MGIFerroAustriaGKEast = ProjectionInfo.FromEpsgCode(31253).SetNames("MGI_Ferro_Austria_GK_East", "GCS_MGI_Ferro", "D_MGI");
            MGIFerroAustriaGKWest = ProjectionInfo.FromEpsgCode(31251).SetNames("MGI_Ferro_Austria_GK_West", "GCS_MGI_Ferro", "D_MGI");
            MGIFerroM28 = ProjectionInfo.FromEpsgCode(31288).SetNames("MGI_Ferro_M28", "GCS_MGI_Ferro", "D_MGI");
            MGIFerroM31 = ProjectionInfo.FromEpsgCode(31289).SetNames("MGI_Ferro_M31", "GCS_MGI_Ferro", "D_MGI");
            MGIFerroM34 = ProjectionInfo.FromEpsgCode(31290).SetNames("MGI_Ferro_M34", "GCS_MGI_Ferro", "D_MGI");
            MGIM28 = ProjectionInfo.FromEpsgCode(31284).SetNames("MGI_M28", "GCS_MGI", "D_MGI");
            MGIM31 = ProjectionInfo.FromEpsgCode(31285).SetNames("MGI_M31", "GCS_MGI", "D_MGI");
            MGIM34 = ProjectionInfo.FromEpsgCode(31286).SetNames("MGI_M34", "GCS_MGI", "D_MGI");
        }

        #endregion
    }
}

#pragma warning restore 1591