// ********************************************************************************************************
// Product Name: DotSpatial.Projection
// Description:  The basic module for MapWindow version 6.0
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/14/2009 4:46:07 PM
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
    /// This class contains predefined CoordinateSystems for Australia.
    /// </summary>
    public class Australia : CoordinateSystemCategory
    {
        #region Private Variables
        // ReSharper disable InconsistentNaming
        public readonly ProjectionInfo AGD1966ACTGridAGCZone;
        public readonly ProjectionInfo AGD1966AMGZone48;
        public readonly ProjectionInfo AGD1966AMGZone49;
        public readonly ProjectionInfo AGD1966AMGZone50;
        public readonly ProjectionInfo AGD1966AMGZone51;
        public readonly ProjectionInfo AGD1966AMGZone52;
        public readonly ProjectionInfo AGD1966AMGZone53;
        public readonly ProjectionInfo AGD1966AMGZone54;
        public readonly ProjectionInfo AGD1966AMGZone55;
        public readonly ProjectionInfo AGD1966AMGZone56;
        public readonly ProjectionInfo AGD1966AMGZone57;
        public readonly ProjectionInfo AGD1966AMGZone58;
        public readonly ProjectionInfo AGD1966ISG542;
        public readonly ProjectionInfo AGD1966ISG543;
        public readonly ProjectionInfo AGD1966ISG551;
        public readonly ProjectionInfo AGD1966ISG552;
        public readonly ProjectionInfo AGD1966ISG553;
        public readonly ProjectionInfo AGD1966ISG561;
        public readonly ProjectionInfo AGD1966ISG562;
        public readonly ProjectionInfo AGD1966ISG563;
        public readonly ProjectionInfo AGD1966VICGRID;
        public readonly ProjectionInfo AGD1984AMGZone48;
        public readonly ProjectionInfo AGD1984AMGZone49;
        public readonly ProjectionInfo AGD1984AMGZone50;
        public readonly ProjectionInfo AGD1984AMGZone51;
        public readonly ProjectionInfo AGD1984AMGZone52;
        public readonly ProjectionInfo AGD1984AMGZone53;
        public readonly ProjectionInfo AGD1984AMGZone54;
        public readonly ProjectionInfo AGD1984AMGZone55;
        public readonly ProjectionInfo AGD1984AMGZone56;
        public readonly ProjectionInfo AGD1984AMGZone57;
        public readonly ProjectionInfo AGD1984AMGZone58;
        public readonly ProjectionInfo GDA1994AustraliaAlbers;
        public readonly ProjectionInfo GDA1994BCSG02;
        public readonly ProjectionInfo GDA1994GeoscienceAustraliaLambert;
        public readonly ProjectionInfo GDA1994MGAZone48;
        public readonly ProjectionInfo GDA1994MGAZone49;
        public readonly ProjectionInfo GDA1994MGAZone50;
        public readonly ProjectionInfo GDA1994MGAZone51;
        public readonly ProjectionInfo GDA1994MGAZone52;
        public readonly ProjectionInfo GDA1994MGAZone53;
        public readonly ProjectionInfo GDA1994MGAZone54;
        public readonly ProjectionInfo GDA1994MGAZone55;
        public readonly ProjectionInfo GDA1994MGAZone56;
        public readonly ProjectionInfo GDA1994MGAZone57;
        public readonly ProjectionInfo GDA1994MGAZone58;
        public readonly ProjectionInfo GDA1994NewSouthWalesLambert;
        public readonly ProjectionInfo GDA1994PCG94;
        public readonly ProjectionInfo GDA1994SouthAustraliaLambert;
        public readonly ProjectionInfo GDA1994VICGRID94;
        public readonly ProjectionInfo WGS1984AustalianCentreforRemoteSensingLambert;
        // ReSharper restore InconsistentNaming
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of Australia.
        /// </summary>
        public Australia()
        {
            AGD1966ACTGridAGCZone = ProjectionInfo.FromAuthorityCode("ESRI", 102071).SetNames("AGD_1966_ACT_Grid_AGC_Zone", "GCS_Australian_1966", "D_Australian_1966"); // missing
            AGD1966AMGZone48 = ProjectionInfo.FromEpsgCode(20248).SetNames("AGD_1966_AMG_Zone_48", "GCS_Australian_1966", "D_Australian_1966");
            AGD1966AMGZone49 = ProjectionInfo.FromEpsgCode(20249).SetNames("AGD_1966_AMG_Zone_49", "GCS_Australian_1966", "D_Australian_1966");
            AGD1966AMGZone50 = ProjectionInfo.FromEpsgCode(20250).SetNames("AGD_1966_AMG_Zone_50", "GCS_Australian_1966", "D_Australian_1966");
            AGD1966AMGZone51 = ProjectionInfo.FromEpsgCode(20251).SetNames("AGD_1966_AMG_Zone_51", "GCS_Australian_1966", "D_Australian_1966");
            AGD1966AMGZone52 = ProjectionInfo.FromEpsgCode(20252).SetNames("AGD_1966_AMG_Zone_52", "GCS_Australian_1966", "D_Australian_1966");
            AGD1966AMGZone53 = ProjectionInfo.FromEpsgCode(20253).SetNames("AGD_1966_AMG_Zone_53", "GCS_Australian_1966", "D_Australian_1966");
            AGD1966AMGZone54 = ProjectionInfo.FromEpsgCode(20254).SetNames("AGD_1966_AMG_Zone_54", "GCS_Australian_1966", "D_Australian_1966");
            AGD1966AMGZone55 = ProjectionInfo.FromEpsgCode(20255).SetNames("AGD_1966_AMG_Zone_55", "GCS_Australian_1966", "D_Australian_1966");
            AGD1966AMGZone56 = ProjectionInfo.FromEpsgCode(20256).SetNames("AGD_1966_AMG_Zone_56", "GCS_Australian_1966", "D_Australian_1966");
            AGD1966AMGZone57 = ProjectionInfo.FromEpsgCode(20257).SetNames("AGD_1966_AMG_Zone_57", "GCS_Australian_1966", "D_Australian_1966");
            AGD1966AMGZone58 = ProjectionInfo.FromEpsgCode(20258).SetNames("AGD_1966_AMG_Zone_58", "GCS_Australian_1966", "D_Australian_1966");
            AGD1966ISG542 = ProjectionInfo.FromAuthorityCode("ESRI", 102072).SetNames("AGD_1966_ISG_54_2", "GCS_Australian_1966", "D_Australian_1966"); // missing
            AGD1966ISG543 = ProjectionInfo.FromAuthorityCode("ESRI", 102073).SetNames("AGD_1966_ISG_54_3", "GCS_Australian_1966", "D_Australian_1966"); // missing
            AGD1966ISG551 = ProjectionInfo.FromAuthorityCode("ESRI", 102074).SetNames("AGD_1966_ISG_55_1", "GCS_Australian_1966", "D_Australian_1966"); // missing
            AGD1966ISG552 = ProjectionInfo.FromAuthorityCode("ESRI", 102075).SetNames("AGD_1966_ISG_55_2", "GCS_Australian_1966", "D_Australian_1966"); // missing
            AGD1966ISG553 = ProjectionInfo.FromAuthorityCode("ESRI", 102076).SetNames("AGD_1966_ISG_55_3", "GCS_Australian_1966", "D_Australian_1966"); // missing
            AGD1966ISG561 = ProjectionInfo.FromAuthorityCode("ESRI", 102077).SetNames("AGD_1966_ISG_56_1", "GCS_Australian_1966", "D_Australian_1966"); // missing
            AGD1966ISG562 = ProjectionInfo.FromAuthorityCode("ESRI", 102078).SetNames("AGD_1966_ISG_56_2", "GCS_Australian_1966", "D_Australian_1966"); // missing
            AGD1966ISG563 = ProjectionInfo.FromAuthorityCode("ESRI", 102079).SetNames("AGD_1966_ISG_56_3", "GCS_Australian_1966", "D_Australian_1966"); // missing
            AGD1966VICGRID = ProjectionInfo.FromEpsgCode(3110).SetNames("AGD_1966_VICGRID", "GCS_Australian_1966", "D_Australian_1966");
            AGD1984AMGZone48 = ProjectionInfo.FromEpsgCode(20348).SetNames("AGD_1984_AMG_Zone_48", "GCS_Australian_1984", "D_Australian_1984");
            AGD1984AMGZone49 = ProjectionInfo.FromEpsgCode(20349).SetNames("AGD_1984_AMG_Zone_49", "GCS_Australian_1984", "D_Australian_1984");
            AGD1984AMGZone50 = ProjectionInfo.FromEpsgCode(20350).SetNames("AGD_1984_AMG_Zone_50", "GCS_Australian_1984", "D_Australian_1984");
            AGD1984AMGZone51 = ProjectionInfo.FromEpsgCode(20351).SetNames("AGD_1984_AMG_Zone_51", "GCS_Australian_1984", "D_Australian_1984");
            AGD1984AMGZone52 = ProjectionInfo.FromEpsgCode(20352).SetNames("AGD_1984_AMG_Zone_52", "GCS_Australian_1984", "D_Australian_1984");
            AGD1984AMGZone53 = ProjectionInfo.FromEpsgCode(20353).SetNames("AGD_1984_AMG_Zone_53", "GCS_Australian_1984", "D_Australian_1984");
            AGD1984AMGZone54 = ProjectionInfo.FromEpsgCode(20354).SetNames("AGD_1984_AMG_Zone_54", "GCS_Australian_1984", "D_Australian_1984");
            AGD1984AMGZone55 = ProjectionInfo.FromEpsgCode(20355).SetNames("AGD_1984_AMG_Zone_55", "GCS_Australian_1984", "D_Australian_1984");
            AGD1984AMGZone56 = ProjectionInfo.FromEpsgCode(20356).SetNames("AGD_1984_AMG_Zone_56", "GCS_Australian_1984", "D_Australian_1984");
            AGD1984AMGZone57 = ProjectionInfo.FromEpsgCode(20357).SetNames("AGD_1984_AMG_Zone_57", "GCS_Australian_1984", "D_Australian_1984");
            AGD1984AMGZone58 = ProjectionInfo.FromEpsgCode(20358).SetNames("AGD_1984_AMG_Zone_58", "GCS_Australian_1984", "D_Australian_1984");
            GDA1994AustraliaAlbers = ProjectionInfo.FromEpsgCode(3577).SetNames("GDA_1994_Australia_Albers", "GCS_GDA_1994", "D_GDA_1994");
            GDA1994BCSG02 = ProjectionInfo.FromEpsgCode(3113).SetNames("GDA_1994_BCSG02", "GCS_GDA_1994", "D_GDA_1994");
            GDA1994GeoscienceAustraliaLambert = ProjectionInfo.FromEpsgCode(3112).SetNames("GDA_1994_Geoscience_Australia_Lambert", "GCS_GDA_1994", "D_GDA_1994");
            GDA1994MGAZone48 = ProjectionInfo.FromEpsgCode(28348).SetNames("GDA_1994_MGA_Zone_48", "GCS_GDA_1994", "D_GDA_1994");
            GDA1994MGAZone49 = ProjectionInfo.FromEpsgCode(28349).SetNames("GDA_1994_MGA_Zone_49", "GCS_GDA_1994", "D_GDA_1994");
            GDA1994MGAZone50 = ProjectionInfo.FromEpsgCode(28350).SetNames("GDA_1994_MGA_Zone_50", "GCS_GDA_1994", "D_GDA_1994");
            GDA1994MGAZone51 = ProjectionInfo.FromEpsgCode(28351).SetNames("GDA_1994_MGA_Zone_51", "GCS_GDA_1994", "D_GDA_1994");
            GDA1994MGAZone52 = ProjectionInfo.FromEpsgCode(28352).SetNames("GDA_1994_MGA_Zone_52", "GCS_GDA_1994", "D_GDA_1994");
            GDA1994MGAZone53 = ProjectionInfo.FromEpsgCode(28353).SetNames("GDA_1994_MGA_Zone_53", "GCS_GDA_1994", "D_GDA_1994");
            GDA1994MGAZone54 = ProjectionInfo.FromEpsgCode(28354).SetNames("GDA_1994_MGA_Zone_54", "GCS_GDA_1994", "D_GDA_1994");
            GDA1994MGAZone55 = ProjectionInfo.FromEpsgCode(28355).SetNames("GDA_1994_MGA_Zone_55", "GCS_GDA_1994", "D_GDA_1994");
            GDA1994MGAZone56 = ProjectionInfo.FromEpsgCode(28356).SetNames("GDA_1994_MGA_Zone_56", "GCS_GDA_1994", "D_GDA_1994");
            GDA1994MGAZone57 = ProjectionInfo.FromEpsgCode(28357).SetNames("GDA_1994_MGA_Zone_57", "GCS_GDA_1994", "D_GDA_1994");
            GDA1994MGAZone58 = ProjectionInfo.FromEpsgCode(28358).SetNames("GDA_1994_MGA_Zone_58", "GCS_GDA_1994", "D_GDA_1994");
            GDA1994NewSouthWalesLambert = ProjectionInfo.FromEpsgCode(3308).SetNames("GDA_1994_NSW_Lambert", "GCS_GDA_1994", "D_GDA_1994");
            GDA1994PCG94 = ProjectionInfo.FromAuthorityCode("ESRI", 102216).SetNames("GDA_1994_Perth_Coastal_Grid_1994", "GCS_GDA_1994", "D_GDA_1994"); // missing
            GDA1994SouthAustraliaLambert = ProjectionInfo.FromEpsgCode(3107).SetNames("GDA_1994_South_Australia_Lambert", "GCS_GDA_1994", "D_GDA_1994");
            GDA1994VICGRID94 = ProjectionInfo.FromEpsgCode(3111).SetNames("GDA_1994_VICGRID94", "GCS_GDA_1994", "D_GDA_1994");
            WGS1984AustalianCentreforRemoteSensingLambert = ProjectionInfo.FromAuthorityCode("EPSG", 102439).SetNames("WGS_1984_Australian_Centre_for_Remote_Sensing_Lambert", "GCS_WGS_1984", "D_WGS_1984"); // missing
        }

        #endregion
    }
}

#pragma warning restore 1591