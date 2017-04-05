// ********************************************************************************************************
// Product Name: DotSpatial.Projection
// Description:  The basic module for MapWindow version 6.0
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/14/2009 5:10:15 PM
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
    /// This class contains predefined CoordinateSystems for NorthernHemisphere.
    /// </summary>
    public class NorthernHemisphere : CoordinateSystemCategory
    {
        #region Private Variables
        // ReSharper disable InconsistentNaming
        public readonly ProjectionInfo WGS1972UTMZone10N;
        public readonly ProjectionInfo WGS1972UTMZone11N;
        public readonly ProjectionInfo WGS1972UTMZone12N;
        public readonly ProjectionInfo WGS1972UTMZone13N;
        public readonly ProjectionInfo WGS1972UTMZone14N;
        public readonly ProjectionInfo WGS1972UTMZone15N;
        public readonly ProjectionInfo WGS1972UTMZone16N;
        public readonly ProjectionInfo WGS1972UTMZone17N;
        public readonly ProjectionInfo WGS1972UTMZone18N;
        public readonly ProjectionInfo WGS1972UTMZone19N;
        public readonly ProjectionInfo WGS1972UTMZone1N;
        public readonly ProjectionInfo WGS1972UTMZone20N;
        public readonly ProjectionInfo WGS1972UTMZone21N;
        public readonly ProjectionInfo WGS1972UTMZone22N;
        public readonly ProjectionInfo WGS1972UTMZone23N;
        public readonly ProjectionInfo WGS1972UTMZone24N;
        public readonly ProjectionInfo WGS1972UTMZone25N;
        public readonly ProjectionInfo WGS1972UTMZone26N;
        public readonly ProjectionInfo WGS1972UTMZone27N;
        public readonly ProjectionInfo WGS1972UTMZone28N;
        public readonly ProjectionInfo WGS1972UTMZone29N;
        public readonly ProjectionInfo WGS1972UTMZone2N;
        public readonly ProjectionInfo WGS1972UTMZone30N;
        public readonly ProjectionInfo WGS1972UTMZone31N;
        public readonly ProjectionInfo WGS1972UTMZone32N;
        public readonly ProjectionInfo WGS1972UTMZone33N;
        public readonly ProjectionInfo WGS1972UTMZone34N;
        public readonly ProjectionInfo WGS1972UTMZone35N;
        public readonly ProjectionInfo WGS1972UTMZone36N;
        public readonly ProjectionInfo WGS1972UTMZone37N;
        public readonly ProjectionInfo WGS1972UTMZone38N;
        public readonly ProjectionInfo WGS1972UTMZone39N;
        public readonly ProjectionInfo WGS1972UTMZone3N;
        public readonly ProjectionInfo WGS1972UTMZone40N;
        public readonly ProjectionInfo WGS1972UTMZone41N;
        public readonly ProjectionInfo WGS1972UTMZone42N;
        public readonly ProjectionInfo WGS1972UTMZone43N;
        public readonly ProjectionInfo WGS1972UTMZone44N;
        public readonly ProjectionInfo WGS1972UTMZone45N;
        public readonly ProjectionInfo WGS1972UTMZone46N;
        public readonly ProjectionInfo WGS1972UTMZone47N;
        public readonly ProjectionInfo WGS1972UTMZone48N;
        public readonly ProjectionInfo WGS1972UTMZone49N;
        public readonly ProjectionInfo WGS1972UTMZone4N;
        public readonly ProjectionInfo WGS1972UTMZone50N;
        public readonly ProjectionInfo WGS1972UTMZone51N;
        public readonly ProjectionInfo WGS1972UTMZone52N;
        public readonly ProjectionInfo WGS1972UTMZone53N;
        public readonly ProjectionInfo WGS1972UTMZone54N;
        public readonly ProjectionInfo WGS1972UTMZone55N;
        public readonly ProjectionInfo WGS1972UTMZone56N;
        public readonly ProjectionInfo WGS1972UTMZone57N;
        public readonly ProjectionInfo WGS1972UTMZone58N;
        public readonly ProjectionInfo WGS1972UTMZone59N;
        public readonly ProjectionInfo WGS1972UTMZone5N;
        public readonly ProjectionInfo WGS1972UTMZone60N;
        public readonly ProjectionInfo WGS1972UTMZone6N;
        public readonly ProjectionInfo WGS1972UTMZone7N;
        public readonly ProjectionInfo WGS1972UTMZone8N;
        public readonly ProjectionInfo WGS1972UTMZone9N;
        // ReSharper restore InconsistentNaming
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of NorthernHemisphere.
        /// </summary>
        public NorthernHemisphere()
        {
            WGS1972UTMZone10N = ProjectionInfo.FromEpsgCode(32210).SetNames("WGS_1972_UTM_Zone_10N", "GCS_WGS_1972", "D_WGS_1972");
            WGS1972UTMZone11N = ProjectionInfo.FromEpsgCode(32211).SetNames("WGS_1972_UTM_Zone_11N", "GCS_WGS_1972", "D_WGS_1972");
            WGS1972UTMZone12N = ProjectionInfo.FromEpsgCode(32212).SetNames("WGS_1972_UTM_Zone_12N", "GCS_WGS_1972", "D_WGS_1972");
            WGS1972UTMZone13N = ProjectionInfo.FromEpsgCode(32213).SetNames("WGS_1972_UTM_Zone_13N", "GCS_WGS_1972", "D_WGS_1972");
            WGS1972UTMZone14N = ProjectionInfo.FromEpsgCode(32214).SetNames("WGS_1972_UTM_Zone_14N", "GCS_WGS_1972", "D_WGS_1972");
            WGS1972UTMZone15N = ProjectionInfo.FromEpsgCode(32215).SetNames("WGS_1972_UTM_Zone_15N", "GCS_WGS_1972", "D_WGS_1972");
            WGS1972UTMZone16N = ProjectionInfo.FromEpsgCode(32216).SetNames("WGS_1972_UTM_Zone_16N", "GCS_WGS_1972", "D_WGS_1972");
            WGS1972UTMZone17N = ProjectionInfo.FromEpsgCode(32217).SetNames("WGS_1972_UTM_Zone_17N", "GCS_WGS_1972", "D_WGS_1972");
            WGS1972UTMZone18N = ProjectionInfo.FromEpsgCode(32218).SetNames("WGS_1972_UTM_Zone_18N", "GCS_WGS_1972", "D_WGS_1972");
            WGS1972UTMZone19N = ProjectionInfo.FromEpsgCode(32219).SetNames("WGS_1972_UTM_Zone_19N", "GCS_WGS_1972", "D_WGS_1972");
            WGS1972UTMZone1N = ProjectionInfo.FromEpsgCode(32201).SetNames("WGS_1972_UTM_Zone_1N", "GCS_WGS_1972", "D_WGS_1972");
            WGS1972UTMZone20N = ProjectionInfo.FromEpsgCode(32220).SetNames("WGS_1972_UTM_Zone_20N", "GCS_WGS_1972", "D_WGS_1972");
            WGS1972UTMZone21N = ProjectionInfo.FromEpsgCode(32221).SetNames("WGS_1972_UTM_Zone_21N", "GCS_WGS_1972", "D_WGS_1972");
            WGS1972UTMZone22N = ProjectionInfo.FromEpsgCode(32222).SetNames("WGS_1972_UTM_Zone_22N", "GCS_WGS_1972", "D_WGS_1972");
            WGS1972UTMZone23N = ProjectionInfo.FromEpsgCode(32223).SetNames("WGS_1972_UTM_Zone_23N", "GCS_WGS_1972", "D_WGS_1972");
            WGS1972UTMZone24N = ProjectionInfo.FromEpsgCode(32224).SetNames("WGS_1972_UTM_Zone_24N", "GCS_WGS_1972", "D_WGS_1972");
            WGS1972UTMZone25N = ProjectionInfo.FromEpsgCode(32225).SetNames("WGS_1972_UTM_Zone_25N", "GCS_WGS_1972", "D_WGS_1972");
            WGS1972UTMZone26N = ProjectionInfo.FromEpsgCode(32226).SetNames("WGS_1972_UTM_Zone_26N", "GCS_WGS_1972", "D_WGS_1972");
            WGS1972UTMZone27N = ProjectionInfo.FromEpsgCode(32227).SetNames("WGS_1972_UTM_Zone_27N", "GCS_WGS_1972", "D_WGS_1972");
            WGS1972UTMZone28N = ProjectionInfo.FromEpsgCode(32228).SetNames("WGS_1972_UTM_Zone_28N", "GCS_WGS_1972", "D_WGS_1972");
            WGS1972UTMZone29N = ProjectionInfo.FromEpsgCode(32229).SetNames("WGS_1972_UTM_Zone_29N", "GCS_WGS_1972", "D_WGS_1972");
            WGS1972UTMZone2N = ProjectionInfo.FromEpsgCode(32202).SetNames("WGS_1972_UTM_Zone_2N", "GCS_WGS_1972", "D_WGS_1972");
            WGS1972UTMZone30N = ProjectionInfo.FromEpsgCode(32230).SetNames("WGS_1972_UTM_Zone_30N", "GCS_WGS_1972", "D_WGS_1972");
            WGS1972UTMZone31N = ProjectionInfo.FromEpsgCode(32231).SetNames("WGS_1972_UTM_Zone_31N", "GCS_WGS_1972", "D_WGS_1972");
            WGS1972UTMZone32N = ProjectionInfo.FromEpsgCode(32232).SetNames("WGS_1972_UTM_Zone_32N", "GCS_WGS_1972", "D_WGS_1972");
            WGS1972UTMZone33N = ProjectionInfo.FromEpsgCode(32233).SetNames("WGS_1972_UTM_Zone_33N", "GCS_WGS_1972", "D_WGS_1972");
            WGS1972UTMZone34N = ProjectionInfo.FromEpsgCode(32234).SetNames("WGS_1972_UTM_Zone_34N", "GCS_WGS_1972", "D_WGS_1972");
            WGS1972UTMZone35N = ProjectionInfo.FromEpsgCode(32235).SetNames("WGS_1972_UTM_Zone_35N", "GCS_WGS_1972", "D_WGS_1972");
            WGS1972UTMZone36N = ProjectionInfo.FromEpsgCode(32236).SetNames("WGS_1972_UTM_Zone_36N", "GCS_WGS_1972", "D_WGS_1972");
            WGS1972UTMZone37N = ProjectionInfo.FromEpsgCode(32237).SetNames("WGS_1972_UTM_Zone_37N", "GCS_WGS_1972", "D_WGS_1972");
            WGS1972UTMZone38N = ProjectionInfo.FromEpsgCode(32238).SetNames("WGS_1972_UTM_Zone_38N", "GCS_WGS_1972", "D_WGS_1972");
            WGS1972UTMZone39N = ProjectionInfo.FromEpsgCode(32239).SetNames("WGS_1972_UTM_Zone_39N", "GCS_WGS_1972", "D_WGS_1972");
            WGS1972UTMZone3N = ProjectionInfo.FromEpsgCode(32203).SetNames("WGS_1972_UTM_Zone_3N", "GCS_WGS_1972", "D_WGS_1972");
            WGS1972UTMZone40N = ProjectionInfo.FromEpsgCode(32240).SetNames("WGS_1972_UTM_Zone_40N", "GCS_WGS_1972", "D_WGS_1972");
            WGS1972UTMZone41N = ProjectionInfo.FromEpsgCode(32241).SetNames("WGS_1972_UTM_Zone_41N", "GCS_WGS_1972", "D_WGS_1972");
            WGS1972UTMZone42N = ProjectionInfo.FromEpsgCode(32242).SetNames("WGS_1972_UTM_Zone_42N", "GCS_WGS_1972", "D_WGS_1972");
            WGS1972UTMZone43N = ProjectionInfo.FromEpsgCode(32243).SetNames("WGS_1972_UTM_Zone_43N", "GCS_WGS_1972", "D_WGS_1972");
            WGS1972UTMZone44N = ProjectionInfo.FromEpsgCode(32244).SetNames("WGS_1972_UTM_Zone_44N", "GCS_WGS_1972", "D_WGS_1972");
            WGS1972UTMZone45N = ProjectionInfo.FromEpsgCode(32245).SetNames("WGS_1972_UTM_Zone_45N", "GCS_WGS_1972", "D_WGS_1972");
            WGS1972UTMZone46N = ProjectionInfo.FromEpsgCode(32246).SetNames("WGS_1972_UTM_Zone_46N", "GCS_WGS_1972", "D_WGS_1972");
            WGS1972UTMZone47N = ProjectionInfo.FromEpsgCode(32247).SetNames("WGS_1972_UTM_Zone_47N", "GCS_WGS_1972", "D_WGS_1972");
            WGS1972UTMZone48N = ProjectionInfo.FromEpsgCode(32248).SetNames("WGS_1972_UTM_Zone_48N", "GCS_WGS_1972", "D_WGS_1972");
            WGS1972UTMZone49N = ProjectionInfo.FromEpsgCode(32249).SetNames("WGS_1972_UTM_Zone_49N", "GCS_WGS_1972", "D_WGS_1972");
            WGS1972UTMZone4N = ProjectionInfo.FromEpsgCode(32204).SetNames("WGS_1972_UTM_Zone_4N", "GCS_WGS_1972", "D_WGS_1972");
            WGS1972UTMZone50N = ProjectionInfo.FromEpsgCode(32250).SetNames("WGS_1972_UTM_Zone_50N", "GCS_WGS_1972", "D_WGS_1972");
            WGS1972UTMZone51N = ProjectionInfo.FromEpsgCode(32251).SetNames("WGS_1972_UTM_Zone_51N", "GCS_WGS_1972", "D_WGS_1972");
            WGS1972UTMZone52N = ProjectionInfo.FromEpsgCode(32252).SetNames("WGS_1972_UTM_Zone_52N", "GCS_WGS_1972", "D_WGS_1972");
            WGS1972UTMZone53N = ProjectionInfo.FromEpsgCode(32253).SetNames("WGS_1972_UTM_Zone_53N", "GCS_WGS_1972", "D_WGS_1972");
            WGS1972UTMZone54N = ProjectionInfo.FromEpsgCode(32254).SetNames("WGS_1972_UTM_Zone_54N", "GCS_WGS_1972", "D_WGS_1972");
            WGS1972UTMZone55N = ProjectionInfo.FromEpsgCode(32255).SetNames("WGS_1972_UTM_Zone_55N", "GCS_WGS_1972", "D_WGS_1972");
            WGS1972UTMZone56N = ProjectionInfo.FromEpsgCode(32256).SetNames("WGS_1972_UTM_Zone_56N", "GCS_WGS_1972", "D_WGS_1972");
            WGS1972UTMZone57N = ProjectionInfo.FromEpsgCode(32257).SetNames("WGS_1972_UTM_Zone_57N", "GCS_WGS_1972", "D_WGS_1972");
            WGS1972UTMZone58N = ProjectionInfo.FromEpsgCode(32258).SetNames("WGS_1972_UTM_Zone_58N", "GCS_WGS_1972", "D_WGS_1972");
            WGS1972UTMZone59N = ProjectionInfo.FromEpsgCode(32259).SetNames("WGS_1972_UTM_Zone_59N", "GCS_WGS_1972", "D_WGS_1972");
            WGS1972UTMZone5N = ProjectionInfo.FromEpsgCode(32205).SetNames("WGS_1972_UTM_Zone_5N", "GCS_WGS_1972", "D_WGS_1972");
            WGS1972UTMZone60N = ProjectionInfo.FromEpsgCode(32260).SetNames("WGS_1972_UTM_Zone_60N", "GCS_WGS_1972", "D_WGS_1972");
            WGS1972UTMZone6N = ProjectionInfo.FromEpsgCode(32206).SetNames("WGS_1972_UTM_Zone_6N", "GCS_WGS_1972", "D_WGS_1972");
            WGS1972UTMZone7N = ProjectionInfo.FromEpsgCode(32207).SetNames("WGS_1972_UTM_Zone_7N", "GCS_WGS_1972", "D_WGS_1972");
            WGS1972UTMZone8N = ProjectionInfo.FromEpsgCode(32208).SetNames("WGS_1972_UTM_Zone_8N", "GCS_WGS_1972", "D_WGS_1972");
            WGS1972UTMZone9N = ProjectionInfo.FromEpsgCode(32209).SetNames("WGS_1972_UTM_Zone_9N", "GCS_WGS_1972", "D_WGS_1972");
        }

        #endregion
    }
}

#pragma warning restore 1591