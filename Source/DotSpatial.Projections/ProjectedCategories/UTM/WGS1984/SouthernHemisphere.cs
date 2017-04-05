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

namespace DotSpatial.Projections.ProjectedCategories.UTM.WGS1984
{
    /// <summary>
    /// This class contains predefined CoordinateSystems for SouthernHemisphere.
    /// </summary>
    public class SouthernHemisphere : CoordinateSystemCategory
    {
        #region Private Variables
        // ReSharper disable InconsistentNaming
        public readonly ProjectionInfo WGS1984UTMZone10S;
        public readonly ProjectionInfo WGS1984UTMZone11S;
        public readonly ProjectionInfo WGS1984UTMZone12S;
        public readonly ProjectionInfo WGS1984UTMZone13S;
        public readonly ProjectionInfo WGS1984UTMZone14S;
        public readonly ProjectionInfo WGS1984UTMZone15S;
        public readonly ProjectionInfo WGS1984UTMZone16S;
        public readonly ProjectionInfo WGS1984UTMZone17S;
        public readonly ProjectionInfo WGS1984UTMZone18S;
        public readonly ProjectionInfo WGS1984UTMZone19S;
        public readonly ProjectionInfo WGS1984UTMZone1S;
        public readonly ProjectionInfo WGS1984UTMZone20S;
        public readonly ProjectionInfo WGS1984UTMZone21S;
        public readonly ProjectionInfo WGS1984UTMZone22S;
        public readonly ProjectionInfo WGS1984UTMZone23S;
        public readonly ProjectionInfo WGS1984UTMZone24S;
        public readonly ProjectionInfo WGS1984UTMZone25S;
        public readonly ProjectionInfo WGS1984UTMZone26S;
        public readonly ProjectionInfo WGS1984UTMZone27S;
        public readonly ProjectionInfo WGS1984UTMZone28S;
        public readonly ProjectionInfo WGS1984UTMZone29S;
        public readonly ProjectionInfo WGS1984UTMZone2S;
        public readonly ProjectionInfo WGS1984UTMZone30S;
        public readonly ProjectionInfo WGS1984UTMZone31S;
        public readonly ProjectionInfo WGS1984UTMZone32S;
        public readonly ProjectionInfo WGS1984UTMZone33S;
        public readonly ProjectionInfo WGS1984UTMZone34S;
        public readonly ProjectionInfo WGS1984UTMZone35S;
        public readonly ProjectionInfo WGS1984UTMZone36S;
        public readonly ProjectionInfo WGS1984UTMZone37S;
        public readonly ProjectionInfo WGS1984UTMZone38S;
        public readonly ProjectionInfo WGS1984UTMZone39S;
        public readonly ProjectionInfo WGS1984UTMZone3S;
        public readonly ProjectionInfo WGS1984UTMZone40S;
        public readonly ProjectionInfo WGS1984UTMZone41S;
        public readonly ProjectionInfo WGS1984UTMZone42S;
        public readonly ProjectionInfo WGS1984UTMZone43S;
        public readonly ProjectionInfo WGS1984UTMZone44S;
        public readonly ProjectionInfo WGS1984UTMZone45S;
        public readonly ProjectionInfo WGS1984UTMZone46S;
        public readonly ProjectionInfo WGS1984UTMZone47S;
        public readonly ProjectionInfo WGS1984UTMZone48S;
        public readonly ProjectionInfo WGS1984UTMZone49S;
        public readonly ProjectionInfo WGS1984UTMZone4S;
        public readonly ProjectionInfo WGS1984UTMZone50S;
        public readonly ProjectionInfo WGS1984UTMZone51S;
        public readonly ProjectionInfo WGS1984UTMZone52S;
        public readonly ProjectionInfo WGS1984UTMZone53S;
        public readonly ProjectionInfo WGS1984UTMZone54S;
        public readonly ProjectionInfo WGS1984UTMZone55S;
        public readonly ProjectionInfo WGS1984UTMZone56S;
        public readonly ProjectionInfo WGS1984UTMZone57S;
        public readonly ProjectionInfo WGS1984UTMZone58S;
        public readonly ProjectionInfo WGS1984UTMZone59S;
        public readonly ProjectionInfo WGS1984UTMZone5S;
        public readonly ProjectionInfo WGS1984UTMZone60S;
        public readonly ProjectionInfo WGS1984UTMZone6S;
        public readonly ProjectionInfo WGS1984UTMZone7S;
        public readonly ProjectionInfo WGS1984UTMZone8S;
        public readonly ProjectionInfo WGS1984UTMZone9S;
        // ReSharper restore InconsistentNaming
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of SouthernHemisphere.
        /// </summary>
        public SouthernHemisphere()
        {
            WGS1984UTMZone10S = ProjectionInfo.FromEpsgCode(32710).SetNames("WGS_1984_UTM_Zone_10S", "GCS_WGS_1984", "D_WGS_1984");
            WGS1984UTMZone11S = ProjectionInfo.FromEpsgCode(32711).SetNames("WGS_1984_UTM_Zone_11S", "GCS_WGS_1984", "D_WGS_1984");
            WGS1984UTMZone12S = ProjectionInfo.FromEpsgCode(32712).SetNames("WGS_1984_UTM_Zone_12S", "GCS_WGS_1984", "D_WGS_1984");
            WGS1984UTMZone13S = ProjectionInfo.FromEpsgCode(32713).SetNames("WGS_1984_UTM_Zone_13S", "GCS_WGS_1984", "D_WGS_1984");
            WGS1984UTMZone14S = ProjectionInfo.FromEpsgCode(32714).SetNames("WGS_1984_UTM_Zone_14S", "GCS_WGS_1984", "D_WGS_1984");
            WGS1984UTMZone15S = ProjectionInfo.FromEpsgCode(32715).SetNames("WGS_1984_UTM_Zone_15S", "GCS_WGS_1984", "D_WGS_1984");
            WGS1984UTMZone16S = ProjectionInfo.FromEpsgCode(32716).SetNames("WGS_1984_UTM_Zone_16S", "GCS_WGS_1984", "D_WGS_1984");
            WGS1984UTMZone17S = ProjectionInfo.FromEpsgCode(32717).SetNames("WGS_1984_UTM_Zone_17S", "GCS_WGS_1984", "D_WGS_1984");
            WGS1984UTMZone18S = ProjectionInfo.FromEpsgCode(32718).SetNames("WGS_1984_UTM_Zone_18S", "GCS_WGS_1984", "D_WGS_1984");
            WGS1984UTMZone19S = ProjectionInfo.FromEpsgCode(32719).SetNames("WGS_1984_UTM_Zone_19S", "GCS_WGS_1984", "D_WGS_1984");
            WGS1984UTMZone1S = ProjectionInfo.FromEpsgCode(32701).SetNames("WGS_1984_UTM_Zone_1S", "GCS_WGS_1984", "D_WGS_1984");
            WGS1984UTMZone20S = ProjectionInfo.FromEpsgCode(32720).SetNames("WGS_1984_UTM_Zone_20S", "GCS_WGS_1984", "D_WGS_1984");
            WGS1984UTMZone21S = ProjectionInfo.FromEpsgCode(32721).SetNames("WGS_1984_UTM_Zone_21S", "GCS_WGS_1984", "D_WGS_1984");
            WGS1984UTMZone22S = ProjectionInfo.FromEpsgCode(32722).SetNames("WGS_1984_UTM_Zone_22S", "GCS_WGS_1984", "D_WGS_1984");
            WGS1984UTMZone23S = ProjectionInfo.FromEpsgCode(32723).SetNames("WGS_1984_UTM_Zone_23S", "GCS_WGS_1984", "D_WGS_1984");
            WGS1984UTMZone24S = ProjectionInfo.FromEpsgCode(32724).SetNames("WGS_1984_UTM_Zone_24S", "GCS_WGS_1984", "D_WGS_1984");
            WGS1984UTMZone25S = ProjectionInfo.FromEpsgCode(32725).SetNames("WGS_1984_UTM_Zone_25S", "GCS_WGS_1984", "D_WGS_1984");
            WGS1984UTMZone26S = ProjectionInfo.FromEpsgCode(32726).SetNames("WGS_1984_UTM_Zone_26S", "GCS_WGS_1984", "D_WGS_1984");
            WGS1984UTMZone27S = ProjectionInfo.FromEpsgCode(32727).SetNames("WGS_1984_UTM_Zone_27S", "GCS_WGS_1984", "D_WGS_1984");
            WGS1984UTMZone28S = ProjectionInfo.FromEpsgCode(32728).SetNames("WGS_1984_UTM_Zone_28S", "GCS_WGS_1984", "D_WGS_1984");
            WGS1984UTMZone29S = ProjectionInfo.FromEpsgCode(32729).SetNames("WGS_1984_UTM_Zone_29S", "GCS_WGS_1984", "D_WGS_1984");
            WGS1984UTMZone2S = ProjectionInfo.FromEpsgCode(32702).SetNames("WGS_1984_UTM_Zone_2S", "GCS_WGS_1984", "D_WGS_1984");
            WGS1984UTMZone30S = ProjectionInfo.FromEpsgCode(32730).SetNames("WGS_1984_UTM_Zone_30S", "GCS_WGS_1984", "D_WGS_1984");
            WGS1984UTMZone31S = ProjectionInfo.FromEpsgCode(32731).SetNames("WGS_1984_UTM_Zone_31S", "GCS_WGS_1984", "D_WGS_1984");
            WGS1984UTMZone32S = ProjectionInfo.FromEpsgCode(32732).SetNames("WGS_1984_UTM_Zone_32S", "GCS_WGS_1984", "D_WGS_1984");
            WGS1984UTMZone33S = ProjectionInfo.FromEpsgCode(32733).SetNames("WGS_1984_UTM_Zone_33S", "GCS_WGS_1984", "D_WGS_1984");
            WGS1984UTMZone34S = ProjectionInfo.FromEpsgCode(32734).SetNames("WGS_1984_UTM_Zone_34S", "GCS_WGS_1984", "D_WGS_1984");
            WGS1984UTMZone35S = ProjectionInfo.FromEpsgCode(32735).SetNames("WGS_1984_UTM_Zone_35S", "GCS_WGS_1984", "D_WGS_1984");
            WGS1984UTMZone36S = ProjectionInfo.FromEpsgCode(32736).SetNames("WGS_1984_UTM_Zone_36S", "GCS_WGS_1984", "D_WGS_1984");
            WGS1984UTMZone37S = ProjectionInfo.FromEpsgCode(32737).SetNames("WGS_1984_UTM_Zone_37S", "GCS_WGS_1984", "D_WGS_1984");
            WGS1984UTMZone38S = ProjectionInfo.FromEpsgCode(32738).SetNames("WGS_1984_UTM_Zone_38S", "GCS_WGS_1984", "D_WGS_1984");
            WGS1984UTMZone39S = ProjectionInfo.FromEpsgCode(32739).SetNames("WGS_1984_UTM_Zone_39S", "GCS_WGS_1984", "D_WGS_1984");
            WGS1984UTMZone3S = ProjectionInfo.FromEpsgCode(32703).SetNames("WGS_1984_UTM_Zone_3S", "GCS_WGS_1984", "D_WGS_1984");
            WGS1984UTMZone40S = ProjectionInfo.FromEpsgCode(32740).SetNames("WGS_1984_UTM_Zone_40S", "GCS_WGS_1984", "D_WGS_1984");
            WGS1984UTMZone41S = ProjectionInfo.FromEpsgCode(32741).SetNames("WGS_1984_UTM_Zone_41S", "GCS_WGS_1984", "D_WGS_1984");
            WGS1984UTMZone42S = ProjectionInfo.FromEpsgCode(32742).SetNames("WGS_1984_UTM_Zone_42S", "GCS_WGS_1984", "D_WGS_1984");
            WGS1984UTMZone43S = ProjectionInfo.FromEpsgCode(32743).SetNames("WGS_1984_UTM_Zone_43S", "GCS_WGS_1984", "D_WGS_1984");
            WGS1984UTMZone44S = ProjectionInfo.FromEpsgCode(32744).SetNames("WGS_1984_UTM_Zone_44S", "GCS_WGS_1984", "D_WGS_1984");
            WGS1984UTMZone45S = ProjectionInfo.FromEpsgCode(32745).SetNames("WGS_1984_UTM_Zone_45S", "GCS_WGS_1984", "D_WGS_1984");
            WGS1984UTMZone46S = ProjectionInfo.FromEpsgCode(32746).SetNames("WGS_1984_UTM_Zone_46S", "GCS_WGS_1984", "D_WGS_1984");
            WGS1984UTMZone47S = ProjectionInfo.FromEpsgCode(32747).SetNames("WGS_1984_UTM_Zone_47S", "GCS_WGS_1984", "D_WGS_1984");
            WGS1984UTMZone48S = ProjectionInfo.FromEpsgCode(32748).SetNames("WGS_1984_UTM_Zone_48S", "GCS_WGS_1984", "D_WGS_1984");
            WGS1984UTMZone49S = ProjectionInfo.FromEpsgCode(32749).SetNames("WGS_1984_UTM_Zone_49S", "GCS_WGS_1984", "D_WGS_1984");
            WGS1984UTMZone4S = ProjectionInfo.FromEpsgCode(32704).SetNames("WGS_1984_UTM_Zone_4S", "GCS_WGS_1984", "D_WGS_1984");
            WGS1984UTMZone50S = ProjectionInfo.FromEpsgCode(32750).SetNames("WGS_1984_UTM_Zone_50S", "GCS_WGS_1984", "D_WGS_1984");
            WGS1984UTMZone51S = ProjectionInfo.FromEpsgCode(32751).SetNames("WGS_1984_UTM_Zone_51S", "GCS_WGS_1984", "D_WGS_1984");
            WGS1984UTMZone52S = ProjectionInfo.FromEpsgCode(32752).SetNames("WGS_1984_UTM_Zone_52S", "GCS_WGS_1984", "D_WGS_1984");
            WGS1984UTMZone53S = ProjectionInfo.FromEpsgCode(32753).SetNames("WGS_1984_UTM_Zone_53S", "GCS_WGS_1984", "D_WGS_1984");
            WGS1984UTMZone54S = ProjectionInfo.FromEpsgCode(32754).SetNames("WGS_1984_UTM_Zone_54S", "GCS_WGS_1984", "D_WGS_1984");
            WGS1984UTMZone55S = ProjectionInfo.FromEpsgCode(32755).SetNames("WGS_1984_UTM_Zone_55S", "GCS_WGS_1984", "D_WGS_1984");
            WGS1984UTMZone56S = ProjectionInfo.FromEpsgCode(32756).SetNames("WGS_1984_UTM_Zone_56S", "GCS_WGS_1984", "D_WGS_1984");
            WGS1984UTMZone57S = ProjectionInfo.FromEpsgCode(32757).SetNames("WGS_1984_UTM_Zone_57S", "GCS_WGS_1984", "D_WGS_1984");
            WGS1984UTMZone58S = ProjectionInfo.FromEpsgCode(32758).SetNames("WGS_1984_UTM_Zone_58S", "GCS_WGS_1984", "D_WGS_1984");
            WGS1984UTMZone59S = ProjectionInfo.FromEpsgCode(32759).SetNames("WGS_1984_UTM_Zone_59S", "GCS_WGS_1984", "D_WGS_1984");
            WGS1984UTMZone5S = ProjectionInfo.FromEpsgCode(32705).SetNames("WGS_1984_UTM_Zone_5S", "GCS_WGS_1984", "D_WGS_1984");
            WGS1984UTMZone60S = ProjectionInfo.FromEpsgCode(32760).SetNames("WGS_1984_UTM_Zone_60S", "GCS_WGS_1984", "D_WGS_1984");
            WGS1984UTMZone6S = ProjectionInfo.FromEpsgCode(32706).SetNames("WGS_1984_UTM_Zone_6S", "GCS_WGS_1984", "D_WGS_1984");
            WGS1984UTMZone7S = ProjectionInfo.FromEpsgCode(32707).SetNames("WGS_1984_UTM_Zone_7S", "GCS_WGS_1984", "D_WGS_1984");
            WGS1984UTMZone8S = ProjectionInfo.FromEpsgCode(32708).SetNames("WGS_1984_UTM_Zone_8S", "GCS_WGS_1984", "D_WGS_1984");
            WGS1984UTMZone9S = ProjectionInfo.FromEpsgCode(32709).SetNames("WGS_1984_UTM_Zone_9S", "GCS_WGS_1984", "D_WGS_1984");
        }

        #endregion
    }
}

#pragma warning restore 1591