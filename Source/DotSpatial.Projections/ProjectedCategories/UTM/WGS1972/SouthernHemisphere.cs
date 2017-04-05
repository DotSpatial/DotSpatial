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

namespace DotSpatial.Projections.ProjectedCategories.UTM.WGS1972
{
    /// <summary>
    /// This class contains predefined CoordinateSystems for SouthernHemisphere.
    /// </summary>
    public class SouthernHemisphere : CoordinateSystemCategory
    {
        #region Private Variables
        // ReSharper disable InconsistentNaming
        public readonly ProjectionInfo WGS1972UTMZone10S;
        public readonly ProjectionInfo WGS1972UTMZone11S;
        public readonly ProjectionInfo WGS1972UTMZone12S;
        public readonly ProjectionInfo WGS1972UTMZone13S;
        public readonly ProjectionInfo WGS1972UTMZone14S;
        public readonly ProjectionInfo WGS1972UTMZone15S;
        public readonly ProjectionInfo WGS1972UTMZone16S;
        public readonly ProjectionInfo WGS1972UTMZone17S;
        public readonly ProjectionInfo WGS1972UTMZone18S;
        public readonly ProjectionInfo WGS1972UTMZone19S;
        public readonly ProjectionInfo WGS1972UTMZone1S;
        public readonly ProjectionInfo WGS1972UTMZone20S;
        public readonly ProjectionInfo WGS1972UTMZone21S;
        public readonly ProjectionInfo WGS1972UTMZone22S;
        public readonly ProjectionInfo WGS1972UTMZone23S;
        public readonly ProjectionInfo WGS1972UTMZone24S;
        public readonly ProjectionInfo WGS1972UTMZone25S;
        public readonly ProjectionInfo WGS1972UTMZone26S;
        public readonly ProjectionInfo WGS1972UTMZone27S;
        public readonly ProjectionInfo WGS1972UTMZone28S;
        public readonly ProjectionInfo WGS1972UTMZone29S;
        public readonly ProjectionInfo WGS1972UTMZone2S;
        public readonly ProjectionInfo WGS1972UTMZone30S;
        public readonly ProjectionInfo WGS1972UTMZone31S;
        public readonly ProjectionInfo WGS1972UTMZone32S;
        public readonly ProjectionInfo WGS1972UTMZone33S;
        public readonly ProjectionInfo WGS1972UTMZone34S;
        public readonly ProjectionInfo WGS1972UTMZone35S;
        public readonly ProjectionInfo WGS1972UTMZone36S;
        public readonly ProjectionInfo WGS1972UTMZone37S;
        public readonly ProjectionInfo WGS1972UTMZone38S;
        public readonly ProjectionInfo WGS1972UTMZone39S;
        public readonly ProjectionInfo WGS1972UTMZone3S;
        public readonly ProjectionInfo WGS1972UTMZone40S;
        public readonly ProjectionInfo WGS1972UTMZone41S;
        public readonly ProjectionInfo WGS1972UTMZone42S;
        public readonly ProjectionInfo WGS1972UTMZone43S;
        public readonly ProjectionInfo WGS1972UTMZone44S;
        public readonly ProjectionInfo WGS1972UTMZone45S;
        public readonly ProjectionInfo WGS1972UTMZone46S;
        public readonly ProjectionInfo WGS1972UTMZone47S;
        public readonly ProjectionInfo WGS1972UTMZone48S;
        public readonly ProjectionInfo WGS1972UTMZone49S;
        public readonly ProjectionInfo WGS1972UTMZone4S;
        public readonly ProjectionInfo WGS1972UTMZone50S;
        public readonly ProjectionInfo WGS1972UTMZone51S;
        public readonly ProjectionInfo WGS1972UTMZone52S;
        public readonly ProjectionInfo WGS1972UTMZone53S;
        public readonly ProjectionInfo WGS1972UTMZone54S;
        public readonly ProjectionInfo WGS1972UTMZone55S;
        public readonly ProjectionInfo WGS1972UTMZone56S;
        public readonly ProjectionInfo WGS1972UTMZone57S;
        public readonly ProjectionInfo WGS1972UTMZone58S;
        public readonly ProjectionInfo WGS1972UTMZone59S;
        public readonly ProjectionInfo WGS1972UTMZone5S;
        public readonly ProjectionInfo WGS1972UTMZone60S;
        public readonly ProjectionInfo WGS1972UTMZone6S;
        public readonly ProjectionInfo WGS1972UTMZone7S;
        public readonly ProjectionInfo WGS1972UTMZone8S;
        public readonly ProjectionInfo WGS1972UTMZone9S;
        // ReSharper restore InconsistentNaming
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of SouthernHemisphere.
        /// </summary>
        public SouthernHemisphere()
        {
            WGS1972UTMZone10S = ProjectionInfo.FromEpsgCode(32310).SetNames("WGS_1972_UTM_Zone_10S", "GCS_WGS_1972", "D_WGS_1972");
            WGS1972UTMZone11S = ProjectionInfo.FromEpsgCode(32311).SetNames("WGS_1972_UTM_Zone_11S", "GCS_WGS_1972", "D_WGS_1972");
            WGS1972UTMZone12S = ProjectionInfo.FromEpsgCode(32312).SetNames("WGS_1972_UTM_Zone_12S", "GCS_WGS_1972", "D_WGS_1972");
            WGS1972UTMZone13S = ProjectionInfo.FromEpsgCode(32313).SetNames("WGS_1972_UTM_Zone_13S", "GCS_WGS_1972", "D_WGS_1972");
            WGS1972UTMZone14S = ProjectionInfo.FromEpsgCode(32314).SetNames("WGS_1972_UTM_Zone_14S", "GCS_WGS_1972", "D_WGS_1972");
            WGS1972UTMZone15S = ProjectionInfo.FromEpsgCode(32315).SetNames("WGS_1972_UTM_Zone_15S", "GCS_WGS_1972", "D_WGS_1972");
            WGS1972UTMZone16S = ProjectionInfo.FromEpsgCode(32316).SetNames("WGS_1972_UTM_Zone_16S", "GCS_WGS_1972", "D_WGS_1972");
            WGS1972UTMZone17S = ProjectionInfo.FromEpsgCode(32317).SetNames("WGS_1972_UTM_Zone_17S", "GCS_WGS_1972", "D_WGS_1972");
            WGS1972UTMZone18S = ProjectionInfo.FromEpsgCode(32318).SetNames("WGS_1972_UTM_Zone_18S", "GCS_WGS_1972", "D_WGS_1972");
            WGS1972UTMZone19S = ProjectionInfo.FromEpsgCode(32319).SetNames("WGS_1972_UTM_Zone_19S", "GCS_WGS_1972", "D_WGS_1972");
            WGS1972UTMZone1S = ProjectionInfo.FromEpsgCode(32301).SetNames("WGS_1972_UTM_Zone_1S", "GCS_WGS_1972", "D_WGS_1972");
            WGS1972UTMZone20S = ProjectionInfo.FromEpsgCode(32320).SetNames("WGS_1972_UTM_Zone_20S", "GCS_WGS_1972", "D_WGS_1972");
            WGS1972UTMZone21S = ProjectionInfo.FromEpsgCode(32321).SetNames("WGS_1972_UTM_Zone_21S", "GCS_WGS_1972", "D_WGS_1972");
            WGS1972UTMZone22S = ProjectionInfo.FromEpsgCode(32322).SetNames("WGS_1972_UTM_Zone_22S", "GCS_WGS_1972", "D_WGS_1972");
            WGS1972UTMZone23S = ProjectionInfo.FromEpsgCode(32323).SetNames("WGS_1972_UTM_Zone_23S", "GCS_WGS_1972", "D_WGS_1972");
            WGS1972UTMZone24S = ProjectionInfo.FromEpsgCode(32324).SetNames("WGS_1972_UTM_Zone_24S", "GCS_WGS_1972", "D_WGS_1972");
            WGS1972UTMZone25S = ProjectionInfo.FromEpsgCode(32325).SetNames("WGS_1972_UTM_Zone_25S", "GCS_WGS_1972", "D_WGS_1972");
            WGS1972UTMZone26S = ProjectionInfo.FromEpsgCode(32326).SetNames("WGS_1972_UTM_Zone_26S", "GCS_WGS_1972", "D_WGS_1972");
            WGS1972UTMZone27S = ProjectionInfo.FromEpsgCode(32327).SetNames("WGS_1972_UTM_Zone_27S", "GCS_WGS_1972", "D_WGS_1972");
            WGS1972UTMZone28S = ProjectionInfo.FromEpsgCode(32328).SetNames("WGS_1972_UTM_Zone_28S", "GCS_WGS_1972", "D_WGS_1972");
            WGS1972UTMZone29S = ProjectionInfo.FromEpsgCode(32329).SetNames("WGS_1972_UTM_Zone_29S", "GCS_WGS_1972", "D_WGS_1972");
            WGS1972UTMZone2S = ProjectionInfo.FromEpsgCode(32302).SetNames("WGS_1972_UTM_Zone_2S", "GCS_WGS_1972", "D_WGS_1972");
            WGS1972UTMZone30S = ProjectionInfo.FromEpsgCode(32330).SetNames("WGS_1972_UTM_Zone_30S", "GCS_WGS_1972", "D_WGS_1972");
            WGS1972UTMZone31S = ProjectionInfo.FromEpsgCode(32331).SetNames("WGS_1972_UTM_Zone_31S", "GCS_WGS_1972", "D_WGS_1972");
            WGS1972UTMZone32S = ProjectionInfo.FromEpsgCode(32332).SetNames("WGS_1972_UTM_Zone_32S", "GCS_WGS_1972", "D_WGS_1972");
            WGS1972UTMZone33S = ProjectionInfo.FromEpsgCode(32333).SetNames("WGS_1972_UTM_Zone_33S", "GCS_WGS_1972", "D_WGS_1972");
            WGS1972UTMZone34S = ProjectionInfo.FromEpsgCode(32334).SetNames("WGS_1972_UTM_Zone_34S", "GCS_WGS_1972", "D_WGS_1972");
            WGS1972UTMZone35S = ProjectionInfo.FromEpsgCode(32335).SetNames("WGS_1972_UTM_Zone_35S", "GCS_WGS_1972", "D_WGS_1972");
            WGS1972UTMZone36S = ProjectionInfo.FromEpsgCode(32336).SetNames("WGS_1972_UTM_Zone_36S", "GCS_WGS_1972", "D_WGS_1972");
            WGS1972UTMZone37S = ProjectionInfo.FromEpsgCode(32337).SetNames("WGS_1972_UTM_Zone_37S", "GCS_WGS_1972", "D_WGS_1972");
            WGS1972UTMZone38S = ProjectionInfo.FromEpsgCode(32338).SetNames("WGS_1972_UTM_Zone_38S", "GCS_WGS_1972", "D_WGS_1972");
            WGS1972UTMZone39S = ProjectionInfo.FromEpsgCode(32339).SetNames("WGS_1972_UTM_Zone_39S", "GCS_WGS_1972", "D_WGS_1972");
            WGS1972UTMZone3S = ProjectionInfo.FromEpsgCode(32303).SetNames("WGS_1972_UTM_Zone_3S", "GCS_WGS_1972", "D_WGS_1972");
            WGS1972UTMZone40S = ProjectionInfo.FromEpsgCode(32340).SetNames("WGS_1972_UTM_Zone_40S", "GCS_WGS_1972", "D_WGS_1972");
            WGS1972UTMZone41S = ProjectionInfo.FromEpsgCode(32341).SetNames("WGS_1972_UTM_Zone_41S", "GCS_WGS_1972", "D_WGS_1972");
            WGS1972UTMZone42S = ProjectionInfo.FromEpsgCode(32342).SetNames("WGS_1972_UTM_Zone_42S", "GCS_WGS_1972", "D_WGS_1972");
            WGS1972UTMZone43S = ProjectionInfo.FromEpsgCode(32343).SetNames("WGS_1972_UTM_Zone_43S", "GCS_WGS_1972", "D_WGS_1972");
            WGS1972UTMZone44S = ProjectionInfo.FromEpsgCode(32344).SetNames("WGS_1972_UTM_Zone_44S", "GCS_WGS_1972", "D_WGS_1972");
            WGS1972UTMZone45S = ProjectionInfo.FromEpsgCode(32345).SetNames("WGS_1972_UTM_Zone_45S", "GCS_WGS_1972", "D_WGS_1972");
            WGS1972UTMZone46S = ProjectionInfo.FromEpsgCode(32346).SetNames("WGS_1972_UTM_Zone_46S", "GCS_WGS_1972", "D_WGS_1972");
            WGS1972UTMZone47S = ProjectionInfo.FromEpsgCode(32347).SetNames("WGS_1972_UTM_Zone_47S", "GCS_WGS_1972", "D_WGS_1972");
            WGS1972UTMZone48S = ProjectionInfo.FromEpsgCode(32348).SetNames("WGS_1972_UTM_Zone_48S", "GCS_WGS_1972", "D_WGS_1972");
            WGS1972UTMZone49S = ProjectionInfo.FromEpsgCode(32349).SetNames("WGS_1972_UTM_Zone_49S", "GCS_WGS_1972", "D_WGS_1972");
            WGS1972UTMZone4S = ProjectionInfo.FromEpsgCode(32304).SetNames("WGS_1972_UTM_Zone_4S", "GCS_WGS_1972", "D_WGS_1972");
            WGS1972UTMZone50S = ProjectionInfo.FromEpsgCode(32350).SetNames("WGS_1972_UTM_Zone_50S", "GCS_WGS_1972", "D_WGS_1972");
            WGS1972UTMZone51S = ProjectionInfo.FromEpsgCode(32351).SetNames("WGS_1972_UTM_Zone_51S", "GCS_WGS_1972", "D_WGS_1972");
            WGS1972UTMZone52S = ProjectionInfo.FromEpsgCode(32352).SetNames("WGS_1972_UTM_Zone_52S", "GCS_WGS_1972", "D_WGS_1972");
            WGS1972UTMZone53S = ProjectionInfo.FromEpsgCode(32353).SetNames("WGS_1972_UTM_Zone_53S", "GCS_WGS_1972", "D_WGS_1972");
            WGS1972UTMZone54S = ProjectionInfo.FromEpsgCode(32354).SetNames("WGS_1972_UTM_Zone_54S", "GCS_WGS_1972", "D_WGS_1972");
            WGS1972UTMZone55S = ProjectionInfo.FromEpsgCode(32355).SetNames("WGS_1972_UTM_Zone_55S", "GCS_WGS_1972", "D_WGS_1972");
            WGS1972UTMZone56S = ProjectionInfo.FromEpsgCode(32356).SetNames("WGS_1972_UTM_Zone_56S", "GCS_WGS_1972", "D_WGS_1972");
            WGS1972UTMZone57S = ProjectionInfo.FromEpsgCode(32357).SetNames("WGS_1972_UTM_Zone_57S", "GCS_WGS_1972", "D_WGS_1972");
            WGS1972UTMZone58S = ProjectionInfo.FromEpsgCode(32358).SetNames("WGS_1972_UTM_Zone_58S", "GCS_WGS_1972", "D_WGS_1972");
            WGS1972UTMZone59S = ProjectionInfo.FromEpsgCode(32359).SetNames("WGS_1972_UTM_Zone_59S", "GCS_WGS_1972", "D_WGS_1972");
            WGS1972UTMZone5S = ProjectionInfo.FromEpsgCode(32305).SetNames("WGS_1972_UTM_Zone_5S", "GCS_WGS_1972", "D_WGS_1972");
            WGS1972UTMZone60S = ProjectionInfo.FromEpsgCode(32360).SetNames("WGS_1972_UTM_Zone_60S", "GCS_WGS_1972", "D_WGS_1972");
            WGS1972UTMZone6S = ProjectionInfo.FromEpsgCode(32306).SetNames("WGS_1972_UTM_Zone_6S", "GCS_WGS_1972", "D_WGS_1972");
            WGS1972UTMZone7S = ProjectionInfo.FromEpsgCode(32307).SetNames("WGS_1972_UTM_Zone_7S", "GCS_WGS_1972", "D_WGS_1972");
            WGS1972UTMZone8S = ProjectionInfo.FromEpsgCode(32308).SetNames("WGS_1972_UTM_Zone_8S", "GCS_WGS_1972", "D_WGS_1972");
            WGS1972UTMZone9S = ProjectionInfo.FromEpsgCode(32309).SetNames("WGS_1972_UTM_Zone_9S", "GCS_WGS_1972", "D_WGS_1972");
        }

        #endregion
    }
}

#pragma warning restore 1591