// ********************************************************************************************************
// Product Name: DotSpatial.Projection
// Description:  The basic module for MapWindow version 6.0
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/14/2009 5:01:00 PM
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
    /// This class contains predefined CoordinateSystems for NAD1983IntlFeet.
    /// </summary>
    public class NAD1983IntlFeet : CoordinateSystemCategory
    {
        #region Private Variables
        // ReSharper disable InconsistentNaming
        public readonly ProjectionInfo NAD1983StatePlaneArizonaCentralFIPS0202IntlFeet;
        public readonly ProjectionInfo NAD1983StatePlaneArizonaEastFIPS0201IntlFeet;
        public readonly ProjectionInfo NAD1983StatePlaneArizonaWestFIPS0203IntlFeet;
        public readonly ProjectionInfo NAD1983StatePlaneMichiganCentralFIPS2112IntlFeet;
        public readonly ProjectionInfo NAD1983StatePlaneMichiganNorthFIPS2111IntlFeet;
        public readonly ProjectionInfo NAD1983StatePlaneMichiganSouthFIPS2113IntlFeet;
        public readonly ProjectionInfo NAD1983StatePlaneMontanaFIPS2500IntlFeet;
        public readonly ProjectionInfo NAD1983StatePlaneNorthDakotaNFIPS3301IntlFeet;
        public readonly ProjectionInfo NAD1983StatePlaneNorthDakotaSFIPS3302IntlFeet;
        public readonly ProjectionInfo NAD1983StatePlaneOregonNorthFIPS3601IntlFeet;
        public readonly ProjectionInfo NAD1983StatePlaneOregonSouthFIPS3602IntlFeet;
        public readonly ProjectionInfo NAD1983StatePlaneSouthCarolinaFIPS3900IntlFeet;
        public readonly ProjectionInfo NAD1983StatePlaneUtahCentralFIPS4302IntlFeet;
        public readonly ProjectionInfo NAD1983StatePlaneUtahNorthFIPS4301IntlFeet;
        public readonly ProjectionInfo NAD1983StatePlaneUtahSouthFIPS4303IntlFeet;
        // ReSharper restore InconsistentNaming
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of NAD1983IntlFeet.
        /// </summary>
        public NAD1983IntlFeet()
        {
            NAD1983StatePlaneArizonaCentralFIPS0202IntlFeet = ProjectionInfo.FromEpsgCode(2223).SetNames("NAD_1983_StatePlane_Arizona_Central_FIPS_0202_Feet_Intl", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneArizonaEastFIPS0201IntlFeet = ProjectionInfo.FromEpsgCode(2222).SetNames("NAD_1983_StatePlane_Arizona_East_FIPS_0201_Feet_Intl", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneArizonaWestFIPS0203IntlFeet = ProjectionInfo.FromEpsgCode(2224).SetNames("NAD_1983_StatePlane_Arizona_West_FIPS_0203_Feet_Intl", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneMichiganCentralFIPS2112IntlFeet = ProjectionInfo.FromEpsgCode(2252).SetNames("NAD_1983_StatePlane_Michigan_Central_FIPS_2112_Feet_Intl", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneMichiganNorthFIPS2111IntlFeet = ProjectionInfo.FromEpsgCode(2251).SetNames("NAD_1983_StatePlane_Michigan_North_FIPS_2111_Feet_Intl", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneMichiganSouthFIPS2113IntlFeet = ProjectionInfo.FromEpsgCode(2253).SetNames("NAD_1983_StatePlane_Michigan_South_FIPS_2113_Feet_Intl", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneMontanaFIPS2500IntlFeet = ProjectionInfo.FromEpsgCode(2256).SetNames("NAD_1983_StatePlane_Montana_FIPS_2500_Feet_Intl", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneNorthDakotaNFIPS3301IntlFeet = ProjectionInfo.FromEpsgCode(2265).SetNames("NAD_1983_StatePlane_North_Dakota_North_FIPS_3301_Feet_Intl", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneNorthDakotaSFIPS3302IntlFeet = ProjectionInfo.FromEpsgCode(2266).SetNames("NAD_1983_StatePlane_North_Dakota_South_FIPS_3302_Feet_Intl", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneOregonNorthFIPS3601IntlFeet = ProjectionInfo.FromEpsgCode(2269).SetNames("NAD_1983_StatePlane_Oregon_North_FIPS_3601_Feet_Intl", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneOregonSouthFIPS3602IntlFeet = ProjectionInfo.FromEpsgCode(2270).SetNames("NAD_1983_StatePlane_Oregon_South_FIPS_3602_Feet_Intl", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneSouthCarolinaFIPS3900IntlFeet = ProjectionInfo.FromEpsgCode(2273).SetNames("NAD_1983_StatePlane_South_Carolina_FIPS_3900_Feet_Intl", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneUtahCentralFIPS4302IntlFeet = ProjectionInfo.FromEpsgCode(2281).SetNames("NAD_1983_StatePlane_Utah_Central_FIPS_4302_Feet_Intl", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneUtahNorthFIPS4301IntlFeet = ProjectionInfo.FromEpsgCode(2280).SetNames("NAD_1983_StatePlane_Utah_North_FIPS_4301_Feet_Intl", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneUtahSouthFIPS4303IntlFeet = ProjectionInfo.FromEpsgCode(2282).SetNames("NAD_1983_StatePlane_Utah_South_FIPS_4303_Feet_Intl", "GCS_North_American_1983", "D_North_American_1983");
        }

        #endregion
    }
}

#pragma warning restore 1591