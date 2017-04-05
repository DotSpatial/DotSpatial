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
    /// This class contains predefined CoordinateSystems for NAD1983NSRS2007IntlFeet.
    /// </summary>
    public class NAD1983NSRS2007IntlFeet : CoordinateSystemCategory
    {
        #region Private Variables
        // ReSharper disable InconsistentNaming
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneArizonaCentralFIPS0202IntlFt;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneArizonaEastFIPS0201IntlFeet;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneArizonaWestFIPS0203IntlFeet;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneMichiganCentralFIPS2112IFt;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneMichiganNorthFIPS2111IntlFt;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneMichiganSouthFIPS2113IntlFt;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneMontanaFIPS2500IntlFeet;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneNorthDakotaNFIPS3301IntlFt;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneNorthDakotaSFIPS3302IntlFt;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneOregonNorthFIPS3601IntlFeet;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneOregonSouthFIPS3602IntlFeet;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneSouthCarolinaFIPS3900IntlFt;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneUtahCentralFIPS4302IntlFeet;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneUtahNorthFIPS4301IntlFeet;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneUtahSouthFIPS4303IntlFeet;
        // ReSharper restore InconsistentNaming
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of NAD1983NSRS2007IntlFeet.
        /// </summary>
        public NAD1983NSRS2007IntlFeet()
        {
            NAD1983NSRS2007StatePlaneArizonaCentralFIPS0202IntlFt = ProjectionInfo.FromEpsgCode(3479).SetNames("NAD_1983_NSRS2007_StatePlane_Arizona_Central_FIPS_0202_Ft_Intl", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneArizonaEastFIPS0201IntlFeet = ProjectionInfo.FromEpsgCode(3481).SetNames("NAD_1983_NSRS2007_StatePlane_Arizona_East_FIPS_0201_Ft_Intl", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneArizonaWestFIPS0203IntlFeet = ProjectionInfo.FromEpsgCode(3483).SetNames("NAD_1983_NSRS2007_StatePlane_Arizona_West_FIPS_0203_Ft_Intl", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneMichiganCentralFIPS2112IFt = ProjectionInfo.FromEpsgCode(3588).SetNames("NAD_1983_NSRS2007_StatePlane_Michigan_Central_FIPS_2112_Ft_Intl", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneMichiganNorthFIPS2111IntlFt = ProjectionInfo.FromEpsgCode(3590).SetNames("NAD_1983_NSRS2007_StatePlane_Michigan_North_FIPS_2111_Ft_Intl", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneMichiganSouthFIPS2113IntlFt = ProjectionInfo.FromEpsgCode(3593).SetNames("NAD_1983_NSRS2007_StatePlane_Michigan_South_FIPS_2113_Ft_Intl", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneMontanaFIPS2500IntlFeet = ProjectionInfo.FromEpsgCode(3605).SetNames("NAD_1983_NSRS2007_StatePlane_Montana_FIPS_2500_Ft_Intl", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneNorthDakotaNFIPS3301IntlFt = ProjectionInfo.FromEpsgCode(3634).SetNames("NAD_1983_NSRS2007_StatePlane_North_Dakota_North_FIPS_3301_FtI", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneNorthDakotaSFIPS3302IntlFt = ProjectionInfo.FromEpsgCode(3636).SetNames("NAD_1983_NSRS2007_StatePlane_North_Dakota_South_FIPS_3302_FtI", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneOregonNorthFIPS3601IntlFeet = ProjectionInfo.FromEpsgCode(3646).SetNames("NAD_1983_NSRS2007_StatePlane_Oregon_North_FIPS_3601_Ft_Intl", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneOregonSouthFIPS3602IntlFeet = ProjectionInfo.FromEpsgCode(3648).SetNames("NAD_1983_NSRS2007_StatePlane_Oregon_South_FIPS_3602_Ft_Intl", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneSouthCarolinaFIPS3900IntlFt = ProjectionInfo.FromEpsgCode(3656).SetNames("NAD_1983_NSRS2007_StatePlane_South_Carolina_FIPS_3900_Ft_Intl", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneUtahCentralFIPS4302IntlFeet = ProjectionInfo.FromEpsgCode(3676).SetNames("NAD_1983_NSRS2007_StatePlane_Utah_Central_FIPS_4302_Ft_Intl", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneUtahNorthFIPS4301IntlFeet = ProjectionInfo.FromEpsgCode(3679).SetNames("NAD_1983_NSRS2007_StatePlane_Utah_North_FIPS_4301_Ft_Intl", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneUtahSouthFIPS4303IntlFeet = ProjectionInfo.FromEpsgCode(3682).SetNames("NAD_1983_NSRS2007_StatePlane_Utah_South_FIPS_4303_Ft_Intl", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
        }

        #endregion
    }
}

#pragma warning restore 1591