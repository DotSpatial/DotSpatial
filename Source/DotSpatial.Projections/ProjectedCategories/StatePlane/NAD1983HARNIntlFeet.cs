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

namespace DotSpatial.Projections.ProjectedCategories.StatePlane
{
    /// <summary>
    /// This class contains predefined CoordinateSystems for NAD1983HARNIntlFeet.
    /// </summary>
    public class NAD1983HARNIntlFeet : CoordinateSystemCategory
    {
        #region Private Variables
        // ReSharper disable InconsistentNaming
        public readonly ProjectionInfo NAD1983HARNStatePlaneArizonaCentralFIPS0202IntlFeet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneArizonaEastFIPS0201IntlFeet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneArizonaWestFIPS0203IntlFeet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneMichiganCentralFIPS2112IntlFeet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneMichiganNorthFIPS2111IntlFeet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneMichiganSouthFIPS2113IntlFeet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneMontanaFIPS2500IntlFeet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneNorthDakotaNFIPS3301IntlFeet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneNorthDakotaSFIPS3302IntlFeet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneOregonNorthFIPS3601IntlFeet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneOregonSouthFIPS3602IntlFeet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneSouthCarolinaFIPS3900IntlFeet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneUtahCentralFIPS4302IntlFeet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneUtahNorthFIPS4301IntlFeet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneUtahSouthFIPS4303IntlFeet;
        // ReSharper restore InconsistentNaming
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of NAD1983HARNIntlFeet.
        /// </summary>
        public NAD1983HARNIntlFeet()
        {
            NAD1983HARNStatePlaneArizonaCentralFIPS0202IntlFeet = ProjectionInfo.FromEpsgCode(2868).SetNames("NAD_1983_HARN_StatePlane_Arizona_Central_FIPS_0202_Feet_Intl", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneArizonaEastFIPS0201IntlFeet = ProjectionInfo.FromEpsgCode(2867).SetNames("NAD_1983_HARN_StatePlane_Arizona_East_FIPS_0201_Feet_Intl", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneArizonaWestFIPS0203IntlFeet = ProjectionInfo.FromEpsgCode(2869).SetNames("NAD_1983_HARN_StatePlane_Arizona_West_FIPS_0203_Feet_Intl", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneMichiganCentralFIPS2112IntlFeet = ProjectionInfo.FromEpsgCode(2897).SetNames("NAD_1983_HARN_StatePlane_Michigan_Central_FIPS_2112_Feet_Intl", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneMichiganNorthFIPS2111IntlFeet = ProjectionInfo.FromEpsgCode(2896).SetNames("NAD_1983_HARN_StatePlane_Michigan_North_FIPS_2111_Feet_Intl", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneMichiganSouthFIPS2113IntlFeet = ProjectionInfo.FromEpsgCode(2898).SetNames("NAD_1983_HARN_StatePlane_Michigan_South_FIPS_2113_Feet_Intl", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneMontanaFIPS2500IntlFeet = ProjectionInfo.FromEpsgCode(2901).SetNames("NAD_1983_HARN_StatePlane_Montana_FIPS_2500_Feet_Intl", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneNorthDakotaNFIPS3301IntlFeet = ProjectionInfo.FromEpsgCode(2909).SetNames("NAD_1983_HARN_StatePlane_North_Dakota_North_FIPS_3301_Feet_Intl", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneNorthDakotaSFIPS3302IntlFeet = ProjectionInfo.FromEpsgCode(2910).SetNames("NAD_1983_HARN_StatePlane_North_Dakota_South_FIPS_3302_Feet_Intl", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneOregonNorthFIPS3601IntlFeet = ProjectionInfo.FromEpsgCode(2913).SetNames("NAD_1983_HARN_StatePlane_Oregon_North_FIPS_3601_Feet_Intl", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneOregonSouthFIPS3602IntlFeet = ProjectionInfo.FromEpsgCode(2914).SetNames("NAD_1983_HARN_StatePlane_Oregon_South_FIPS_3602_Feet_Intl", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneSouthCarolinaFIPS3900IntlFeet = ProjectionInfo.FromEpsgCode(3361).SetNames("NAD_1983_HARN_StatePlane_South_Carolina_FIPS_3900_Feet_Intl", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneUtahCentralFIPS4302IntlFeet = ProjectionInfo.FromEpsgCode(2922).SetNames("NAD_1983_HARN_StatePlane_Utah_Central_FIPS_4302_Feet_Intl", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneUtahNorthFIPS4301IntlFeet = ProjectionInfo.FromEpsgCode(2921).SetNames("NAD_1983_HARN_StatePlane_Utah_North_FIPS_4301_Feet_Intl", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneUtahSouthFIPS4303IntlFeet = ProjectionInfo.FromEpsgCode(2923).SetNames("NAD_1983_HARN_StatePlane_Utah_South_FIPS_4303_Feet_Intl", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
        }

        #endregion
    }
}

#pragma warning restore 1591